using DataParser.Handlers;
using DataParser.Infrastructure;
using DataParser.Infrastructure.Enums;
using DataParser.Infrastructure.Factory;
using DataParser.Infrastructure.Interfaces;
using DataParser.Models;
using System;
using System.Globalization;

namespace DataParser
{
    public class PacketDecoder : IPacketDecoder
    {
        private DataReader _reader;

        public CompositeData Decode(string transferType, string input)
        {
            _reader = new DataReader(StringToBytes(input));
            CompositeData compositeData = null;
            switch (transferType)
            {
                case "TCP":
                    compositeData = DecodeTCPData();
                    break;
                case "UDP":
                    compositeData = DecodeUdpData();
                    break;
                default:
                    break;
            }
            return compositeData;
        }

        private CompositeData DecodeTCPData()
        {
            var parsedPacket = new CompositeData("TCP Data Packet");
            parsedPacket.Add(_reader.ReadData(4, DataType.VehicleID));
            parsedPacket.Add(_reader.ReadData(4, DataType.PacketDataArrayLength));
            parsedPacket.Add(DecodeAvlData());

            parsedPacket.Add(_reader.ReadData(4, DataType.Error));

            return parsedPacket;
        }

        private CompositeData DecodeUdpData()
        {
            var parsedPacket = new CompositeData("UDP Data Packet");

            parsedPacket.Add(_reader.ReadData(2, DataType.Length));
            parsedPacket.Add(_reader.ReadData(2, DataType.DeviceId));
            parsedPacket.Add(_reader.ReadData(1, DataType.PacketType));
            parsedPacket.Add(_reader.ReadData(1, DataType.PacketId));

            var imeiLength = _reader.ReadData(2, DataType.ImeiLength);
            parsedPacket.Add(imeiLength);
            parsedPacket.Add(_reader.ReadData((byte)(int.Parse(imeiLength.Value)), DataType.Imei));
            parsedPacket.Add(DecodeAvlData());

            return parsedPacket;
        }

        private CompositeData DecodeAvlData()
        {
            var packetDataComposite = new CompositeData("Data");

            var packetId = _reader.ReadData(1, DataType.TransferId);
            packetDataComposite.Add(packetId);

            var countData = _reader.ReadData(1, DataType.DataPacketCount);
            packetDataComposite.Add(countData);

            PacketData packetDataHandler = PacketFactory.CreatePacketReader(packetId.Value);

            for (var i = 0; i < int.Parse(countData.Value); i++)
            {
                var packetData = packetDataHandler.GetPacketData(_reader);
                packetDataComposite.Add(packetData);
            }

            packetDataComposite.Add(_reader.ReadData(1, DataType.DataPacketCount));

            packetDataComposite.ConfigureArraySegment();

            return packetDataComposite;
        }

        private static byte[] StringToBytes(string data)
        {
            var array = new byte[data.Length / 2];

            var substring = 0;
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = Byte.Parse(data.Substring(substring, 2), NumberStyles.AllowHexSpecifier);
                substring += 2;
            }

            return array;
        }
    }
}
