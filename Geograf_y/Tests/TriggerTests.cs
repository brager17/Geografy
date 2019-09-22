using System;
using Xunit;

namespace Geograf_y
{
    public class TriggerTests : DatabaseTests
    {
        readonly People Mary;
        readonly People John;
        readonly People Jane;
        readonly ChangeLocation MaryChangeLocation;
        readonly ChangeLocation JohnChangeLocation;
        readonly ChangeLocation JaneChangeLocation;

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
    }
}