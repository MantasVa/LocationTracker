using DataParser.Handlers;
using DataParser.Infrastructure;
using DataParser.Infrastructure.Enums;
using DataParser.Models;
using System;

namespace DataParser.Handlers
{
    public class Protocol5 : PacketData
    {
        public override CompositeData GetPacketData(DataReader codecReader)
        {
            CompositeData basePacketComposite = new CompositeData("Packet Data");
            CompositeData gpsElementComposite = new CompositeData("GPS Data");

            basePacketComposite.Add(codecReader.ReadData(8, DataType.Timestamp));
            basePacketComposite.Add(codecReader.ReadData(1, DataType.Priority));
            gpsElementComposite.Add(codecReader.ReadData(4, DataType.Longitude));
            gpsElementComposite.Add(codecReader.ReadData(4, DataType.Latitude));
            gpsElementComposite.Add(codecReader.ReadData(2, DataType.Altitude));
            gpsElementComposite.Add(codecReader.ReadData(2, DataType.Angle));
            gpsElementComposite.Add(codecReader.ReadData(1, DataType.Satellites));
            gpsElementComposite.Add(codecReader.ReadData(2, DataType.Speed));


            basePacketComposite.Add(gpsElementComposite);
            gpsElementComposite.ConfigureArraySegment();
            basePacketComposite.ConfigureArraySegment(gpsElementComposite.Last().ArraySegment.Offset);

            return basePacketComposite;
        }
    }
}
