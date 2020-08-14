using GMap.NET;
using GMap.NET.WindowsForms;
using System.Collections.Generic;

namespace MapVisualizer.Infrastructure.Inferfaces
{
    public interface IRouteStrategy
    {
        float TravelDistance { get; }
        GMapOverlay GetRouteOverlay(IList<PointLatLng> points);
    }
}
