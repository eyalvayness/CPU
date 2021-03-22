using System;

namespace CPU
{
    public class Cpu6502
    {
        public ushort PC { get; private set; } // Program Counter
        public byte SP { get; private set; } // Stack Pointer

        public Register A { get; private set; } // Accumulator Register
        public Register X { get; private set; } // Index Register X
        public Register Y { get; private set; } // Index Register Y

        public ProcessorStatus PS { get; private set; }


        public Cpu6502()
        {
            PS = new();
        }

        public void Reset()
        {

        }
    }

    public record ProcessorStatus
    {
        public bool C { get; init; } // Carry Flag
        public bool Z { get; init; } // Zero Flag
        public bool I { get; init; } // Interrupt Disable
        public bool D { get; init; } // Decimal Mode Flag
        public bool B { get; init; } // Break Command
        public bool V { get; init; } // Overflow Flag
        public bool N { get; init; } // Negative Flag
    }

    public struct Register
    {

    }
}
