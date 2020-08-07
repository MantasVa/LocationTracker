using DataParser.Models;

namespace DataParser.Infrastructure.Interfaces
{
    public interface IPacketDecoder
    {
        CompositeData Decode(string transferType, string input);
    }
}