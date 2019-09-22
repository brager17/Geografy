using System;
using Xunit;

namespace Geograf_y
{
    public class TriggerTests: DatabaseTests
    {
        [Fact]
        public void TRIGGER_ADDED_ROWS_IN_INTERSECTIONS_TABLE()
        {
            Do(LocationIntersection.ClearAll());
            Do(ChangeLocation.ClearAll());
            Do(People.ClearAll());

            var mary = new People(1, "mary");
            var john = new People(2, "john");

            DoInsert(mary);
            DoInsert(john);

            var maryChangeLocation = new ChangeLocation(1, 1, 55.792891m, 49.116969m, DateTime.Now);
            var johnChangeLocation = new ChangeLocation(2, 2, 55.792000m, 49.116969m, DateTime.Now);

            DoInsert(maryChangeLocation);
            DoInsert(johnChangeLocation);

            var countsRecordsInIntersections = MakeScalar<int>("SELECT Count(*) FROM PeopleIntersection");
            Assert.Equal(1, countsRecordsInIntersections);
        }
    }
}