using DataParser.Models;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System.Collections.Generic;
using System.Linq;

namespace DataParser.Handlers.Map
{
    public class MarkersHandler
    {
        private readonly GMapControl _gmap;
        private GpsData _previouslySelectedGpsData;

        public MarkersHandler(MapInitializer mapInitializer)
        {
            _gmap = mapInitializer.GetGMapControl();
        }

        public void UpdateSelectedMarker(GpsData gpsData)
        {
            if (_previouslySelectedGpsData != null)
            {
                ChangeMarkerColor(_previouslySelectedGpsData, GMarkerGoogleType.red_small);
            }

            ChangeMarkerColor(gpsData, GMarkerGoogleType.green_small);

            _previouslySelectedGpsData = gpsData;
        }

        private void ChangeMarkerColor(GpsData data, GMarkerGoogleType markerType)
        {
            RemoveMarker(data.Timestamp);
            var marker = new GMarkerGoogle(data.Coordinates, markerType)
            {
                ToolTipText = data.Timestamp.ToString(),
                Tag = data.Timestamp.ToString()
            };
            AddMarker(marker);
        }

        public void LoadMarkers(IList<GpsData> gpsData)
        {
            _gmap.Overlays.Clear();


            var markers = new GMapOverlay("markers");
            IList<PointLatLng> points = new List<PointLatLng>();
            foreach (var selector in gpsData)
            {

                selector.Coordinates = GetPointLatLng(selector.Latitude, selector.Longitude);
                points.Add(selector.Coordinates);
                GMapMarker marker = new GMarkerGoogle(selector.Coordinates,
                     GMarkerGoogleType.red_small)
                {
                    ToolTipText = selector.Timestamp.ToString(),
                    Tag = selector.Timestamp.ToString()
                };

                markers.Markers.Add(marker);

            }
            _gmap.Zoom = 19;
            _gmap.Position = GetPointLatLng(points.Last().Lat.ToString(), points.Last().Lng.ToString());

            _gmap.Overlays.Add(markers);

            DrawPolygon(points);
        }

        public void CenterMapToMarker(GpsData gpsData)
        {
            _gmap.Position = gpsData.Coordinates;
        }

        private PointLatLng GetPointLatLng(string lat, string lng)
        {
            double latitude = double.Parse(lat);
            double longitude = double.Parse(lng);
            return new PointLatLng(latitude, longitude);
        }

        private void AddMarker(GMapMarker marker)
        {
            var overlay = GetOverlayById("markers");
            overlay.Markers.Add(marker);
        }

        private void RemoveMarker(string timestamp)
        {
            var overlay = GetOverlayById("markers");
            var marker = overlay.Markers.Where(t => t.Tag.ToString() == timestamp.ToString()).First();
            overlay.Markers.Remove(marker);
        }

        private void DrawPolygon(IList<PointLatLng> points)
        {
            GMapOverlay polyOverlay = new GMapOverlay("overlay");
            GMapRoute polygon = new GMapRoute(points, "mypolygon")
            {
                Stroke = new System.Drawing.Pen(System.Drawing.Color.Red, 1)
            };
            polyOverlay.Routes.Add(polygon);
            _gmap.Overlays.Add(polyOverlay);

            _gmap.Zoom++;
            _gmap.Zoom--;
        }
        private GMapOverlay GetOverlayById(string id)
        {
            return _gmap.Overlays.Where(m => m.Id == id).FirstOrDefault();
        }
    }
}
