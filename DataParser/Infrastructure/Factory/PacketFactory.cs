using DataParser.Handlers.Packets;
using System;

namespace DataParser.Infrastructure.Factory
{
    public static class PacketFactory
    {
        public static PacketData CreatePacketReader(string packetId)
        {
            PacketData packet;
            switch (packetId)
            {
                case "4":
                    packet = new Protocol4();
                    break;
                case "5":
                    packet = new Protocol5();
                    break;
                default:
                    throw new ArgumentException();
            }
            return packet;
        }
    }
}
