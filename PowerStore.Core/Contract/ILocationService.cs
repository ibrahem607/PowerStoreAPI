using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Core.Contract
{
    public interface ILocationService
    {
        double HaversineDistance(double lat1, double lat2, double lon1, double lon2);
        double CalculatedTime(double distance, double averageSpeed = 50);
        double CalculatePrice(double distance, double baseFare = 5, double costPerKm = 10);
        Task<(double distance, double estimatedTime, double price, string category)> CalculateDestanceAndTimeAndPrice(double startLat, double startLon, double endLat, double endLon, int categoryId);
    }
}
