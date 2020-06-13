using System;
using Unosquare.WiringPi.Native;

namespace TroykaCap
{
    public class GpioExpander : IGpioExpander
    {
        private readonly int _i2CHandle;

        private const float AnalogReadDivisor = 4095.0f;
        private const int AnalogWriteMultiplier = 255;
        
        public const int DefaultI2CAddress = 0x2A;

        public GpioExpander(int i2CAddress = DefaultI2CAddress)
        {
            _i2CHandle = WiringPi.WiringPiI2CSetup(i2CAddress);
            if (_i2CHandle == -1)
            {
                throw new GpioExpanderException("Failed to init I2C");
            }
        }

        /// <inheritdoc />
        public int DigitalReadPort()
        {
            var data = WiringPi.WiringPiI2CReadReg16(_i2CHandle, GpioExpanderCommand.DigitalRead);
            
            return BitConverter.IsLittleEndian ? Swap(data) : data;
        }

        /// <inheritdoc />
        public bool DigitalRead(int pin)
        {
            var mask = 0x0001 << pin;
            return (DigitalReadPort() & mask) > 0;
        }

        /// <inheritdoc />
        public void DigitalWritePort(int data)
        {
            var output = BitConverter.IsLittleEndian ? Swap(data) : data;

            WiringPi.WiringPiI2CWriteReg16(_i2CHandle, GpioExpanderCommand.DigitalWriteHigh, output);
            WiringPi.WiringPiI2CWriteReg16(_i2CHandle, GpioExpanderCommand.DigitalWriteLow, ~output);
        }

        /// <inheritdoc />
        public void DigitalWrite(int pin, bool value)
        {
            var data = 0x0001 << pin;
            var output = BitConverter.IsLittleEndian ? Swap(data) : data;
            if (value)
            {
                WiringPi.WiringPiI2CWriteReg16(_i2CHandle, GpioExpanderCommand.DigitalWriteHigh, output);
            }
            else
            {
                WiringPi.WiringPiI2CWriteReg16(_i2CHandle, GpioExpanderCommand.DigitalWriteLow, ~output);
            }
        }

        /// <inheritdoc />
        public int AnalogRead16(int pin)
        {
            WiringPi.WiringPiI2CWriteReg16(_i2CHandle, GpioExpanderCommand.AnalogRead, pin);
            var data = WiringPi.WiringPiI2CReadReg16(_i2CHandle, GpioExpanderCommand.AnalogRead);
            return BitConverter.IsLittleEndian ? Swap(data) : data;
        }

        /// <inheritdoc />
        public float AnalogRead(int pin)
        {
            return AnalogRead16(pin) / AnalogReadDivisor;
        }

        /// <inheritdoc />
        public void PwmFreq(int frequency)
        {
            var data = BitConverter.IsLittleEndian ? Swap(frequency) : frequency;
            WiringPi.WiringPiI2CWriteReg16(_i2CHandle, GpioExpanderCommand.PwmFreq, data);
        }

        /// <inheritdoc />
        public void ChangeAddr(int newAddress)
        {
            WiringPi.WiringPiI2CWriteReg16(_i2CHandle, GpioExpanderCommand.ChangeI2CAddr, newAddress);
        }

        /// <inheritdoc />
        public void SaveAddr()
        {
            WiringPi.WiringPiI2CWrite(_i2CHandle, GpioExpanderCommand.SaveI2CAddr);
        }

        /// <inheritdoc />
        public void Reset()
        {
            WiringPi.WiringPiI2CWrite(_i2CHandle, GpioExpanderCommand.Reset);
        }

        /// <inheritdoc />
        public void PinMode(int pin, EPinMode mode)
        {
            var data = 0x0001 << pin;
            var output = BitConverter.IsLittleEndian ? Swap(data) : data;
            var command = mode switch
            {
                EPinMode.Input => GpioExpanderCommand.PortModeInput,
                EPinMode.Output => GpioExpanderCommand.PortModeOutput,
                EPinMode.InputPullDown => GpioExpanderCommand.PortModePullDown,
                EPinMode.InputPullUp => GpioExpanderCommand.PortModePullUp,
                _ => throw new GpioExpanderException("Unsupported pin mode")
            };

            WiringPi.WiringPiI2CWriteReg16(_i2CHandle, command, output);
        }

        /// <inheritdoc />
        public void AnalogWrite(int pin, float value)
        {
            var data = (int)(value * AnalogWriteMultiplier);
            var output = BitConverter.IsLittleEndian ? Swap(data) : data;
            WiringPi.WiringPiI2CWriteReg16(_i2CHandle, GpioExpanderCommand.AnalogWrite, output);
        }

        private int Swap(int data)
        {
            return ((0xFF & data) << 8) | (0xFF & (data >> 8));
        }
    }
}