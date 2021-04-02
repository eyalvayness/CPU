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
        void IsReadingAt(int address);
        void IsWritingAt(int address, byte val);
    }

    public class MemoryManager
    {
        readonly byte[] _memory;
        readonly List<IMemoryObserver> _observers;

        public byte this[int address]
        {
            get => ReadAt(address);
            set => WriteAt(address, value);
        }

        public MemoryManager(int size) : this(new byte[size])
        { }
        public MemoryManager(byte[] memory)
        {
            _memory = memory.ToArray();
            _observers = new();
        }

        public void RegisterObserver(IMemoryObserver observer) => _observers.Add(observer);
        public void RegisterObservers(params IMemoryObserver[] observers)
        {
            foreach (var obs in observers)
                RegisterObserver(obs);
        }
        public void UnregisterObserver(IMemoryObserver observer) => _observers.Remove(observer);
        public void UnregisterObservers(params IMemoryObserver[] observers)
        {
            foreach (var obs in observers)
                UnregisterObserver(obs);
        }

        public void WriteAt(int address, byte b)
        {
            _memory[address] = b;
            
            _observers.Where(obs => CheckIfIsInRange(obs, address)).ToList().ForEach(obs => obs.IsWritingAt(address, b));
        }

        public byte ReadAt(int address)
        {
            _observers.Where(obs => CheckIfIsInRange(obs, address)).ToList().ForEach(obs => obs.IsReadingAt(address));

            return _memory[address];
        }

        bool CheckIfIsInRange(IMemoryObserver obs, int address)
        {
            var (offset, length) = obs.ObervationRange.GetOffsetAndLength(_memory.Length);
            return offset <= address && address <= offset + length;
        }
    }
}
