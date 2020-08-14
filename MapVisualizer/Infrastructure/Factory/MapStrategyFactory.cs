using MapVisualizer.Infrastructure.Inferfaces;
using MapVisualizer.Infrastructure.Strategy;

namespace MapVisualizer.Infrastructure.Factory
{
    public static class MapStrategyFactory
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
