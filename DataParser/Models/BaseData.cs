using DataParser.Infrastructure.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using DataType = DataParser.Infrastructure.Enums.DataType;

namespace DataParser.Models
{
    public abstract class BaseData : IComponent
    {
        private readonly string _name;
        public BaseData(string name)
        {
            _name = name;
        }
        public BaseData(DataType dataType, ArraySegment<byte> arraySegment)
        {
            DataType = dataType;
            Value = GetStringValue(arraySegment, dataType);
            ArraySegment = arraySegment;
        }
        public DataType DataType { get; set; }
        public ArraySegment<byte> ArraySegment { get; set; }
        public string Value { get; set; }
        public string HexValue
        {
            get
            {
                return _name == null && ArraySegment.Array != null
                        ? BitConverter.ToString(ArraySegment.Array.Skip(ArraySegment.Offset).Take(ArraySegment.Count).ToArray())
                        : null;
            }
        }

        public string Size
        {
            get
            {
                if (ArraySegment.Count == 0 || _name != null)
                    return "var";

                return ArraySegment.Count.ToString(CultureInfo.InvariantCulture);
            }
        }

        public string Name
        {
            get
            {
                return _name ?? GetDataTypeDisplayName();
            }
        }

        public abstract void Accept(IVisitor visitor);

        private string GetDataTypeDisplayName()
        {
            var type = DataType.GetType();

            var members = type.GetMember(DataType.ToString());
            if (members.Length == 0) return DataType.ToString();

            var member = members[0];
            var attributes = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attributes.Length == 0) return DataType.ToString();

            var attribute = (DisplayAttribute)attributes[0];
            return attribute.GetName();
        }


        private static string GetStringValue(ArraySegment<byte> arraySegment, DataType dataType)
        {
            DateTime AvlEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var subArray = arraySegment.Array.Skip(arraySegment.Offset).Take(arraySegment.Count).ToArray();

            switch (dataType)
            {
                // Packet Data types
                case DataType.TransferId:
                case DataType.DataPacketCount:
                case DataType.Priority:
                case DataType.Satellites:
                    return subArray[0].ToString();
                case DataType.Latitude:
                case DataType.Longitude:
                    Array.Reverse(subArray);
                    return BitConverter.ToInt32(subArray, 0).ToString();
                case DataType.Altitude:
                case DataType.Angle:
                case DataType.Speed:
                    Array.Reverse(subArray);
                    return BitConverter.ToInt16(subArray, 0).ToString();
                case DataType.Timestamp:
                    Array.Reverse(subArray);
                    return AvlEpoch.AddMilliseconds(BitConverter.ToInt64(subArray, 0)).ToString();
                // Tcp types
                case DataType.VehicleID:
                case DataType.PacketDataArrayLength:
                case DataType.Error:
                    Array.Reverse(subArray);
                    var signed32 = BitConverter.ToInt32(subArray, 0).ToString();
                    var unsigned = BitConverter.ToUInt32(subArray, 0).ToString();
                    return signed32 == unsigned ? signed32 : string.Format("{0} / {1}", signed32, unsigned);
                // Udp types
                case DataType.Length:
                case DataType.DeviceId:
                case DataType.ImeiLength:
                    Array.Reverse(subArray);
                    var signed16 = BitConverter.ToInt16(subArray, 0).ToString();
                    var unsigned16 = BitConverter.ToUInt16(subArray, 0).ToString();
                    return signed16 == unsigned16 ? signed16 : string.Format("{0} / {1}", signed16, unsigned16);
                case DataType.PacketType:
                case DataType.PacketId:
                    return subArray[0].ToString();
                case DataType.Imei:
                    return Encoding.UTF8.GetString(subArray);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
