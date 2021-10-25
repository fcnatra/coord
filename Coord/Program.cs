using System;

namespace Coord
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0) args = new string[] { "5.581595877671971", "-0.17829882552805323", "5" }; // 622,98kms

            ShowHelp();
            new RunCalculation(args);

            Console.WriteLine("\nTHE END");
            Console.ReadLine();
        }

        private static void ShowHelp()
        {
            Console.WriteLine("\nCOORD <latitude> <longitude> <quadrant size in Km>");
            Console.WriteLine("\tTells you the upper right coordinates of the quadrant where that point is");
            Console.WriteLine("\tand the distance of that quadrant from the point 0,0");
        }
    }

    public class RunCalculation
    {
        private const float EARTH_RADIUS = 6378.0f;//6367.45f; //

        public RunCalculation(string[] args)
        {
            GeoCoordinate geoCoordinate = new GeoCoordinate(args[0], args[1]);
            var quadrantSizeInKm = int.Parse(args[2]);

            double distanceKm = GetDistance(new GeoCoordinate(0, 0), geoCoordinate);
            Console.WriteLine($"Distance between points: {distanceKm}Km");

            GeoDistance geoDistanceInKm = GetGeoDistance(new GeoCoordinate(0,0), geoCoordinate);

            Console.WriteLine($"\nDistance in Lat axel (Y): {geoDistanceInKm.LatAxel}Km");
            Console.WriteLine($"Distance in Lon axel (X): {geoDistanceInKm.LongAxel}Km");
        }

        private GeoDistance GetGeoDistance(GeoCoordinate geoCoordinateFrom, GeoCoordinate geoCoordinateTo)
        {
            var distanceInLatAxel = GetDistance(geoCoordinateFrom, new GeoCoordinate(geoCoordinateTo.Latitude, geoCoordinateFrom.Longitude));
            var distanceInLontAxel = GetDistance(geoCoordinateFrom, new GeoCoordinate(geoCoordinateFrom.Latitude, geoCoordinateTo.Longitude));

            var geoDistance = GeoDistance.From(geoCoordinateTo);
            geoDistance.LatAxel = distanceInLatAxel;
            geoDistance.LongAxel = distanceInLontAxel;

            return geoDistance;
        }

        private double GetDistance(GeoCoordinate geoCoordinateFrom, GeoCoordinate geoCoordinateTo)
        {
            // https://www.genbeta.com/desarrollo/como-calcular-la-distancia-entre-dos-puntos-geograficos-en-c-formula-de-haversine

            float latDelta = EnRadianes(geoCoordinateTo.Latitude - geoCoordinateFrom.Latitude);
            float lontDelta = EnRadianes(geoCoordinateTo.Longitude - geoCoordinateFrom.Longitude);

            double a = Math.Pow(Math.Sin(latDelta / 2), 2)
                + Math.Cos(EnRadianes(geoCoordinateFrom.Latitude))
                * Math.Cos(EnRadianes(geoCoordinateTo.Latitude))
                * Math.Pow(Math.Sin(lontDelta / 2), 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double distanceKm = EARTH_RADIUS * c;

            return distanceKm;
        }

        private float EnRadianes(float valor)
        {
            return Convert.ToSingle(Math.PI / 180) * valor;
        }
    }

    public class GeoDistance : GeoCoordinate
    {
        public static GeoDistance From(GeoCoordinate geoCoordinate)
        {
            return new GeoDistance
            {
                Latitude = geoCoordinate.Latitude,
                Longitude = geoCoordinate.Longitude
            };
        }

        public double LatAxel { get; set; }
        public double LongAxel { get; set; }
    }

    public class GeoCoordinate
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public GeoCoordinate()
        {}

        public GeoCoordinate(float latitude, float longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public GeoCoordinate(string latitude, string longitude)
        {
            this.Latitude = float.Parse(latitude);
            this.Longitude = float.Parse(longitude);
        }

        public override bool Equals(Object other)
        {
            return other is GeoCoordinate && Equals((GeoCoordinate)other);
        }

        public bool Equals(GeoCoordinate other)
        {
            return Latitude == other.Latitude && Longitude == other.Longitude;
        }

        public override int GetHashCode()
        {
            return Latitude.GetHashCode() ^ Longitude.GetHashCode();
        }
    }
}
