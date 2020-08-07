using DataParser.Infrastructure.Enums;
using DataParser.Models;

namespace DataParser.Infrastructure.Interfaces
{
    public interface IDataReader
    {
        ComponentData ReadData(int size, DataType dataType);
    }
}