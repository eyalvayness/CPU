using CPU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPUConsole
{
    class OutputObserver : IMemoryObserver
    {
        public Range ObervationRange => new(0x6000, 0x6001);

        public void IsReadingAt(int address)
        {
            throw new NotImplementedException();
        }

        public void IsWritingAt(int address, byte val)
        {
            Console.Write((char)val);
        }
    }
}
