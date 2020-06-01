namespace TroykaCap
{
    public interface IGpioExpander
    {
        int DigitalReadPort();
        
        bool DigitalRead(int pin);

        void DigitalWritePort(int data);

        void DigitalWrite(int pin, bool value);

        int AnalogRead16(int pin);

        float AnalogRead(int pin);

        void PwmFreq(int frequency);

        void ChangeAddr(int newAddress);

        void SaveAddr();

        void Reset();

        void PinMode(int pin, EPinMode mode);

        void AnalogWrite(int pin, float value);
    }
}