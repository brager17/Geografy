using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using FsCheck;
using FsCheck.Xunit;
using Geograf_y.Entities;
using Geograf_y.Infrastructure;
using Xunit;

namespace Geograf_y.Tests
{
    public class Coordinate
    {
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        
        public static Arbitrary<IEnumerable<Coordinate>> CoordinatesArbitrary()
        {
            
            return Arb.Default.Array<decimal[]>().Generator
                .Select(x => x.Select(xx => new Coordinate {Longitude = xx.Item1, Latitude = xx.Item2}))
                .ToArbitrary();
        }

        public void Deconstruct(out decimal l1, out decimal l2)
        {
            l1 = Latitude;
            l2 = Latitude;
        }
    }

    
    public class TriggerTests : DatabaseTests
    {
        readonly People Mary;
        readonly People John;
        readonly People Jane;
        readonly ChangeLocation MaryChangeLocation;
        readonly ChangeLocation JohnChangeLocation;
        readonly ChangeLocation JaneChangeLocation;

        static TriggerTests()
        {
            Arb.Register(typeof(Coordinate));
        }
        public TriggerTests()
        {
            ClearTabels();
            
            Mary = new People(1, "mary");
            John = new People(2, "john");
            Jane = new People(3, "Jane");


            MaryChangeLocation = new ChangeLocation(1, 1, 55.792891m, 49.116969m, DateTime.Now);
            JohnChangeLocation = new ChangeLocation(2, 2, 55.792000m, 49.116969m, DateTime.Now);
            JaneChangeLocation = new ChangeLocation(3, 3, 55.792000m, 49.116969m, DateTime.Now);
        }

        private void ClearTabels()
        {
            Do(LocationIntersection.ClearAll());
            Do(ChangeLocation.ClearAll());
            Do(People.ClearAll());
        }

        [Fact]
        public void INSERT_TWO_PEOPLE_ON_DISTANCE_LESS_100_METERS()
        {
            DoInsert(Mary);
            DoInsert(John);

            DoInsert(MaryChangeLocation);
            DoInsert(JohnChangeLocation);

            var countsRecordsInIntersections = MakeScalar<int>("SELECT Count(*) FROM PeopleIntersection");
            Assert.Equal(1, countsRecordsInIntersections);
        }

        [Fact]
        public void INSERT_TWO_PEOPLE_ON_DISTANCE_LESS_100_METERS_AND_MOVE_MORE_100_METERS()
        {
            DoInsert(Mary);
            DoInsert(John);

            DoInsert(MaryChangeLocation);
            DoInsert(JohnChangeLocation);

            Do(MaryChangeLocation.UpdatePosition(55.792891m, 55.992891m));
            var countsRecordsInIntersections = MakeScalar<int>("SELECT Count(*) FROM PeopleIntersection");
            Assert.Equal(0, countsRecordsInIntersections);
        }

        [Fact]
        public void INSERT_TWO_PEOPLE_ON_DISTANCE_LESS_100_METERS_AND_MOVE_MORE_100_METERS_AND_BACK()
        {
            DoInsert(Mary);
            DoInsert(John);
            DoInsert(MaryChangeLocation);
            DoInsert(JohnChangeLocation);

            Do(MaryChangeLocation.UpdatePosition(55.792891m, 55.992891m));
            Do(MaryChangeLocation.UpdatePosition(55.792891m, 49.116969m));
            var countsRecordsInIntersections = MakeScalar<int>("SELECT Count(*) FROM PeopleIntersection");
            Assert.Equal(1, countsRecordsInIntersections);
        }

        [Fact]
        public void INSERT_THREE_PEOPLE()
        {
            DoInsert(Mary);
            DoInsert(John);
            DoInsert(Jane);

            DoInsert(MaryChangeLocation);
            DoInsert(JohnChangeLocation);
            DoInsert(JaneChangeLocation);

            var countsRecordsInIntersections = MakeScalar<int>("SELECT Count(*) FROM PeopleIntersection");
            Assert.Equal(3, countsRecordsInIntersections);
        }

       
        [Property]
        public void RANDOM_CHANGE_LOCATION_END_CHANGE_LOCATION_IN_ONE_PLACE(
            IEnumerable<Coordinate> john,
            IEnumerable<Coordinate> mary,
            IEnumerable<Coordinate> jane)
        {
            ClearTabels();
            DoInsert(Mary);
            DoInsert(John);
            DoInsert(Jane);

            DoInsert(MaryChangeLocation);
            DoInsert(JohnChangeLocation);
            DoInsert(JaneChangeLocation);

            foreach (var (longitude, latitude) in mary)
            {
                Do(MaryChangeLocation.UpdatePosition(longitude, latitude));
            }

            foreach (var (longitude, latitude) in john)
            {
                Do(JohnChangeLocation.UpdatePosition(longitude, latitude));
            }

            foreach (var (longitude, latitude) in jane)
            {
                Do(JaneChangeLocation.UpdatePosition(longitude, latitude));
            }
        }
    }
}