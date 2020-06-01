using System;

namespace TroykaCap
{
    public class GpioExpanderException : Exception
    {
        public GpioExpanderException(string message)
        : base(message)
        {
        }
    }
}