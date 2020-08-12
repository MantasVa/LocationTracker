using DataParser.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataParser.Handlers.Map
{
    public static class MapStrategyInitializer
    {
        public static IRouteStrategy GetRouteStrategy(string type)
        {
            IRouteStrategy strategy;
            if (type == "1")
            {
                strategy = new VehicleTravelStrategy();
            }
            else
            {
                strategy = new ScooterTravelStrategy();
            }
            return strategy;
        }
    }
}
