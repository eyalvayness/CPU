using System;
using CPU;

namespace CPUConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var memory = Cpu6502Compiler.Compile("main.s");

            var cpu = new Cpu6502(memory);
            cpu.Reset();
        }
    }
}
