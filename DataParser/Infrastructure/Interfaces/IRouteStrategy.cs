using DataParser.Models;
using GMap.NET;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataParser.Infrastructure.Interfaces
{
    public interface IRouteStrategy
    {
        float TravelDistance { get; }
        GMapOverlay GetRouteOverlay(IList<PointLatLng> points);
    }
}
