using System;

namespace Coord
{
    class Program
    {
        static void Main(string[] args)
        {
            args = new string[] { "1", "1", "5" };

            ShowHelp();
            new RunCalculation(args);

            Console.WriteLine("\nTHE END");
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
        public RunCalculation(string[] args)
        {
            GeoCoordinate geoCoordinate = new GeoCoordinate(args[0], args[1]);
            var quadrantSizeInKm = int.Parse(args[2]);


        }
    }


    public class GeoCoordinate
    {
        public float Latitude { get; set; }
        public float Longitude { get; set; }

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
