using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU
{
    public interface IMemoryObserver
    {
        Range ObervationRange { get; }
        //bool IsInRangeofObservation(int address);
        void IsReadingAt(int address);
        void IsWritingAt(int address, byte val);
    }

    public class MemoryManager
    {
        readonly byte[] _memory;

        public byte this[int address]
        {
            get => ReadAt(address);
            set => WriteAt(address, value);
        }

        public MemoryManager(int size)
        {
            _memory = new byte[size];
        }
        public MemoryManager(byte[] memory)
        {
            _memory = memory.ToArray();
        }

        public void WriteAt(int address, byte b)
        {
            _memory[address] = b;
        }

        public byte ReadAt(int address)
        {
            return _memory[address];
        }
    }
}
