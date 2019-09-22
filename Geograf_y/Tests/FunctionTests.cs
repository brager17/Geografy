using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Geograf_y
{
    public class FunctionTests : DatabaseTests
    {
        [Theory]
        [InlineData("55.852022", "48.537093", "55.828693", "49.065840", 33000, 33500)]
        [InlineData("56.035921", "48.589219", "56.203911", "51.077807", 155500, 156500)]
        [InlineData("56.277824", "49.257087", "31.112542", "3.797092", 4425600, 4900000)]
        public void GET_DISTANCE_FUNCTION_TESTS(string long1, string lat1, string long2, string lat2,
            decimal expMin, decimal expMax)
        {
            var result = MakeScalar<decimal>($@"select dbo.GetDistance({long1},{lat1},{long2},{lat2})");
            Assert.True(result <= expMax && result >= expMin);
        }
    }
}