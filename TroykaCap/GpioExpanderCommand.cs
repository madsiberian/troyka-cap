namespace TroykaCap
{
    class GpioExpanderCommand
    {
        public const int WhoAmI = 0x00;
        public const int Reset = 0x01;
        public const int ChangeI2CAddr =  0x02;
        public const int SaveI2CAddr = 0x03;
        public const int PortModeInput = 0x04;
        public const int PortModePullUp = 0x05;
        public const int PortModePullDown = 0x06;
        public const int PortModeOutput = 0x07;
        public const int DigitalRead = 0x08;
        public const int DigitalWriteHigh = 0x09;
        public const int DigitalWriteLow = 0x0A;
        public const int AnalogWrite = 0x0B;
        public const int AnalogRead = 0x0C;
        public const int PwmFreq = 0x0D;
        public const int AdcSpeed = 0x0E;
    }
}