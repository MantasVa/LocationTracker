﻿using DataParser.Infrastructure.Interfaces;
using DataParser.Models;
using System;
using System.Collections.Generic;

namespace DataParser.Infrastructure.Visitor
{
    public class GpsDataVisitor : IVisitor
    {
        private GpsData _currentGpsData;
        public GpsDataVisitor()
        {
            GpsData = new List<GpsData>();
        }

        public List<GpsData> GpsData { get; }

        public void Visit(BaseData componentData)
        {
            if (componentData.Name == "Timestamp")
            {
                _currentGpsData = new GpsData
                {
                    Timestamp = componentData.Value
                };
            }
            else if (componentData.Name == "Longitude")
            {
                _currentGpsData.Longitude = (Double.Parse(componentData.Value) / 10000000).ToString();
            }
            else if (componentData.Name == "Latitude")
            {
                _currentGpsData.Latitude = (Double.Parse(componentData.Value) / 10000000).ToString();

                if (_currentGpsData != null)
                    GpsData.Add(_currentGpsData);
            }

        }
    }
}