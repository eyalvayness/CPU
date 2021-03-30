using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU
{
    public interface IMemoryReadObserver
    {
        bool IsInRangeOfReadObservation(int address);
        void IsReadingAt(int address);
    }
    public interface IMemoryWriteObserver
    {
        bool IsInRangeOfWriteObservation(int address);
        void IsWritingAt(int address);
    }
    public interface IMemoryObserver : IMemoryReadObserver, IMemoryWriteObserver
    { }

    public class MemoryManager
    {
        readonly byte[] _memory;

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
