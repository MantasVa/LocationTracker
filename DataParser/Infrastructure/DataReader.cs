using DataParser.Infrastructure.Enums;
using DataParser.Infrastructure.Interfaces;
using DataParser.Models;
using System;

namespace DataParser.Infrastructure
{
    public class DataReader : IDataReader
    {
        private readonly byte[] _data;
        private int _offset;

        public DataReader(byte[] data)
        {
            _data = data;
            _offset = 0;
        }
        public ComponentData ReadData(int size, DataType dataType)
        {
            var arraySegment = new ArraySegment<byte>(_data, _offset, size);
            _offset += size;
            return new ComponentData(dataType, arraySegment);
        }

    }
}
