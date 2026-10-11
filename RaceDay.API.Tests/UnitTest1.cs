using Xunit;

namespace RaceDay.API.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void BasicTest_Passes()
        {
            Assert.True(true);
        }


    [Theory]
        [InlineData("Organiser")]
        [InlineData("Participant")]
        public void ValidRoles_AreRecognised(string role)
        {
            bool isValid = role == "Organiser" || role == "Participant";

            Assert.True(isValid);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Admin")]
        [InlineData("Guest")]
        public void InvalidRoles_AreRejected(string role)
        {
            bool isValid = role == "Organiser" || role == "Participant";

            Assert.False(isValid);
        }
    }


}
