using MapVisualizer.Models;
using System.Collections.Generic;

namespace MapVisualizer.Infrastructure.Inferfaces
{
    public interface IMarkersHandler
    {
        float GetMarkersDistance { get; }

        void CenterMapToMarker(GpsData gpsData);
        void Handle(IList<GpsData> gpsData, IRouteStrategy routeStrategy);
        void UpdateSelectedMarker(GpsData gpsData);
    }
}