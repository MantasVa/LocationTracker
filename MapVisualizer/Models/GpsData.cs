using GMap.NET;

namespace MapVisualizer.Models
{
    public class GpsData
    {
        public string Timestamp { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public PointLatLng Coordinates { get; set; }
    }
}
