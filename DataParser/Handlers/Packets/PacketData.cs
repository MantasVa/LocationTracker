using DataParser.Infrastructure;
using DataParser.Models;

namespace DataParser.Handlers.Packets
{
    public abstract class PacketData
    {
        public abstract CompositeData GetPacketData(DataReader codecReader);
    }
}
