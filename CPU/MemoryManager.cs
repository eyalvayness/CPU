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
        List<IMemoryObserver> _observers;

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
        public void RegisterObservers(params IMemoryObserver[] observers) => _observers.AddRange(observers);

        public void WriteAt(int address, byte b)
        {
            _observers.Where(obs =>
            {
                var (offset, length) = obs.ObervationRange.GetOffsetAndLength(_memory.Length);
                return offset <= address && address < offset + length;
            }).ToList().ForEach(obs => obs.IsWritingAt(address, b));

            _memory[address] = b;
        }

        public byte ReadAt(int address)
        {
            _observers.Where(obs =>
            {
                var (offset, length) = obs.ObervationRange.GetOffsetAndLength(_memory.Length);
                return offset <= address && address <= offset + length;
            }).ToList().ForEach(obs => obs.IsReadingAt(address));

            return _memory[address];
        }
    }
}
