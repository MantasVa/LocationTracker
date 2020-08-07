using System.ComponentModel.DataAnnotations;

namespace DataParser.Infrastructure.Enums
{
    public enum DataType
    {
        Composite,
        [Display(Name = "Transfer ID")]
        TransferId,
        [Display(Name = "Packet Data Count")]
        DataPacketCount,
        Timestamp,
        Priority,
        Latitude,
        Longitude,
        Altitude,
        Angle,
        Satellites,
        Speed,
        // Tcp types
        [Display(Name = "Message ID")]
        MessageID,
        [Display(Name = "Packet Data Length")]
        PacketDataArrayLength,
        [Display(Name = "Error")]
        Error,
        // Udp types
        Length,
        [Display(Name = "Device ID")]
        DeviceId,
        [Display(Name = "Packet Type")]
        PacketType,
        [Display(Name = "Packet ID")]
        PacketId,
        [Display(Name = "Imei length")]
        ImeiLength,
        Imei
    }
}
