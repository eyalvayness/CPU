using System;
using CPU;

namespace CPUConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var memory = Cpu6502Compiler.Compile("main.s");

            var memManager = new MemoryManager(memory);
            var obs = new OutputObserver();
            memManager.RegisterObserver(obs);

            var cpu = new Cpu6502(memManager);
            cpu.Reset();

            Console.WriteLine();
        }
    }
}
