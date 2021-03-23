using System;
using CPU;

namespace CPUConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] memory = new byte[ushort.MaxValue + 1];

            memory[Cpu6502.Consts.RESET_ADDRESS_L] = 0x00;
            memory[Cpu6502.Consts.RESET_ADDRESS_U] = 0x08;

            memory[0x0800] = Cpu6502ByteCodesIntructions.LDA_IMMEDIATE;
            memory[0x0801] = 42;

            memory[0x000A] = 0xFF;
            memory[0x0223] = 0x04;
            memory[0x08FF] = 0x55;

            memory[0x0802] = Cpu6502ByteCodesIntructions.LDX_ABSOLUTE;
            memory[0x0803] = 0x23;
            memory[0x0804] = 0x02;

            memory[0x0805] = Cpu6502ByteCodesIntructions.LDY_ZEROPAGE_X;
            memory[0x0806] = 0x06;

            memory[0x0807] = Cpu6502ByteCodesIntructions.LDA_ABSOLUTE_Y;
            memory[0x0808] = 0x00;
            memory[0x0809] = 0x08;

            var cpu = new Cpu6502(memory);
            cpu.Reset();
        }
    }
}
