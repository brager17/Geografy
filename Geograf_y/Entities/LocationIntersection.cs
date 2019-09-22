namespace Geograf_y.Entities
{
    public class LocationIntersection
    {
        public int Id;
        public decimal Longitude;
        public decimal Latitude;
        public decimal Distance;
        public static string ClearAll() => "delete from PeopleIntersection";
    }
}