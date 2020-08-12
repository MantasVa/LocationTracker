using DataParser.Infrastructure.Interfaces;
using GMap.NET;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Text;

namespace DataParser.Handlers.Map
{
    public class ScooterTravelStrategy : IRouteStrategy
    {
        public float TravelDistance { get; private set; } = 0;
        public GMapOverlay GetRouteOverlay(IList<PointLatLng> points)
        {
            GMapOverlay polyOverlay = new GMapOverlay("overlay");
            GMapRoute polygon = new GMapRoute(points, "mypolygon")
            {
                Stroke = new System.Drawing.Pen(System.Drawing.Color.Red, 1)
            };
            polyOverlay.Routes.Add(polygon);
            CalculateScooterDistance(points);
            return polyOverlay;
        }

        private void CalculateScooterDistance(IList<PointLatLng> points)
        {
            GeoCoordinate c1;
            GeoCoordinate c2;
            for (short i = 0; i < points.Count - 1; i++)
            {
                c1 = new GeoCoordinate(points[i].Lat, points[i].Lng);
                c2 = new GeoCoordinate(points[i + 1].Lat, points[i + 1].Lng);
                TravelDistance += (float)c1.GetDistanceTo(c2) / 1000;
            }



        }
    }
}
