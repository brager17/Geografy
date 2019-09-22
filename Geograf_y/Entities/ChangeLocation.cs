using System;
using Geograf_y.Base;

namespace Geograf_y
{
    public class ChangeLocation : ICanInsert
    {
        public ChangeLocation(int id, int peopleId, decimal longitude, decimal latitude, DateTime date)
        {
            Id = id;
            PeopleId = peopleId;
            Longitude = longitude;
            Latitude = latitude;
            Date = date;
        }

        public int Id;
        public int PeopleId;
        public decimal Longitude;
        public decimal Latitude;
        public DateTime Date;

        public string InsertCommand()
        {
            return InsertString();
        }

        public static string ClearAll() => "delete from ChangeLocation";

        private string LongitudeFormat => Longitude.ToString().Replace(",", ".");
        private string LatitudeFormat => Latitude.ToString().Replace(",", ".");

        public string InsertString()
        {
            return
                $"INSERT INTO ChangeLocation (Id,PeopleId,Longitude,Latitude,Date) VALUES({Id},{PeopleId},{LongitudeFormat},{LatitudeFormat},'{Date:dd-MM-yyyy HH:mm:ss}')";
        }

        public string UpdatePosition(decimal longitude, decimal latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
            return $"Update ChangeLocation SET Longitude = {LongitudeFormat},Latitude={LatitudeFormat} where Id = {Id}";
        }
    }
}