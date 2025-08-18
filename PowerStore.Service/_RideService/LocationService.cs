using PowerStore.Core.Contract;
using PowerStore.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerStore.Service._RideService
{
    public class LocationService: ILocationService
    {
        private const double EarthRadiusKm = 6371;

        private readonly IUnitOfWork _unitOfWork;

        public LocationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public double HaversineDistance(double lat1, double lat2, double lon1, double lon2)
        {
            var dLat = DegreesToRadians(lat2 - lat1);
            var dLon = DegreesToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return EarthRadiusKm * c;
        }

        public double CalculatedTime(double distance, double averageSpeed = 50) // السرعة بالكيلومترات في الساعة
        {
            return distance / averageSpeed * 60; // الوقت بالدقائق
        }

        public double CalculatePrice(double distance, double baseFare = 5, double costPerKm = 10) /// pendding 
        {
            return baseFare + (distance * costPerKm);
        }

        public async Task<(double distance, double estimatedTime, double price,string category)> CalculateDestanceAndTimeAndPrice(double startLat, double startLon, double endLat, double endLon , int categoryId)
        {
            double distance = HaversineDistance(startLat, endLat, startLon, endLon);
            double estimatedTime = Math.Round(CalculatedTime(distance), 2);
            double price = Math.Round(CalculatePrice(distance),2);
            var category =await _unitOfWork.Repositoy<CategoryOfVehicle>().Find(x=>x.Id==categoryId);
            if (category.Name == "Ride")
                return (distance, estimatedTime, price, category.Name);
            else if (category.Name == "Comfort")
                return (distance, estimatedTime, price * 1.5, category.Name);
            else if (category.Name == "Scoter")
                return (distance, estimatedTime, price / 0.50, category.Name);
            else if (category.Name == "FastTripe")
                return (distance, estimatedTime, price * 2, category.Name);
            else
                return (distance, estimatedTime, price,"");
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
    }
}
