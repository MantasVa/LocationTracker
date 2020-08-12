using DataParser.Infrastructure.Interfaces;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataParser.Handlers.Map
{
    public class VehicleTravelStrategy : IRouteStrategy
    {
        public float TravelDistance { get; private set; } = 0;
        public GMapOverlay GetRouteOverlay(IList<PointLatLng> points)
        {
            var routes = new GMapOverlay("Routes");
            for (short i = 0; i < points.Count - 1; i++)
            {
                var route = OpenStreetMapProvider.Instance.GetRoute(points[i], points[i + 1], false, false, 10);
                var gmapRoute = new GMapRoute(route.Points, "Route");
                TravelDistance += (float)gmapRoute.Distance;
                routes.Routes.Add(gmapRoute);
            }
            return routes;
        }

    }
}
