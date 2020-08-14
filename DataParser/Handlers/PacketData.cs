using DataParser.Infrastructure;
using DataParser.Models;

namespace DataParser.Handlers
{
    public abstract class PacketData
    {
        public abstract CompositeData GetPacketData(DataReader codecReader);
    }
}
