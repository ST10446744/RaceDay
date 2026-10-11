using Xunit;
using System;
using System.Linq;
using Xunit;

namespace RaceDay.API.Tests
{
    public class RaceDayTests
    {
        // 1. USER ROLE TESTS (5 cases)
        [Theory]
        [InlineData("Organiser", true)]
        [InlineData("Participant", true)]
        [InlineData("Admin", false)]
        [InlineData("Guest", false)]
        [InlineData("", false)]
        public void ValidateUserRole(string role, bool expected)
        {
            bool result = role == "Organiser" || role == "Participant";
            Assert.Equal(expected, result);
        }

        // 2. EMAIL TESTS (5 cases)
        [Theory]
        [InlineData("user@example.com", true)]
        [InlineData("faizaan@gmail.com", true)]
        [InlineData("invalid-email", false)]
        [InlineData("user@", false)]
        [InlineData("", false)]
        public void ValidateEmail(string email, bool expected)
        {
            bool result = !string.IsNullOrWhiteSpace(email)
                && email.Contains("@")
                && email.IndexOf("@") > 0
                && email.IndexOf(".") > email.IndexOf("@") + 1
                && email.IndexOf(".") < email.Length - 1;

            Assert.Equal(expected, result);
        }

        // 3. PASSWORD TESTS (5 cases)
        [Theory]
        [InlineData("Password123", true)]
        [InlineData("RaceDay2026", true)]
        [InlineData("Abc12345", true)]
        [InlineData("123", false)]
        [InlineData("", false)]
        public void ValidatePassword(string password, bool expected)
        {
            bool result = !string.IsNullOrWhiteSpace(password)
                && password.Length >= 8;

            Assert.Equal(expected, result);
        }

        // 4. PHONE NUMBER TESTS (5 cases)
        [Theory]
        [InlineData("0821234567", true)]
        [InlineData("0712345678", true)]
        [InlineData("1234567890", true)]
        [InlineData("123", false)]
        [InlineData("", false)]
        public void ValidatePhoneNumber(string phone, bool expected)
        {
            bool result = !string.IsNullOrWhiteSpace(phone)
                && phone.Length == 10
                && phone.All(char.IsDigit);

            Assert.Equal(expected, result);
        }

        // 5. EVENT DISTANCE TESTS (5 cases)
        [Theory]
        [InlineData(5, true)]
        [InlineData(10, true)]
        [InlineData(42, true)]
        [InlineData(0, false)]
        [InlineData(-5, false)]
        public void ValidateEventDistance(double distance, bool expected)
        {
            bool result = distance > 0;
            Assert.Equal(expected, result);
        }

        // 6. EVENT FEE TESTS (5 cases)
        [Theory]
        [InlineData(0, true)]
        [InlineData(50, true)]
        [InlineData(250.50, true)]
        [InlineData(-1, false)]
        [InlineData(-100, false)]
        public void ValidateEventFee(decimal fee, bool expected)
        {
            bool result = fee >= 0;
            Assert.Equal(expected, result);
        }

        // 7. PARTICIPANT LIMIT TESTS (5 cases)
        [Theory]
        [InlineData(1, true)]
        [InlineData(50, true)]
        [InlineData(1000, true)]
        [InlineData(0, false)]
        [InlineData(-10, false)]
        public void ValidateParticipantLimit(int limit, bool expected)
        {
            bool result = limit > 0;
            Assert.Equal(expected, result);
        }

        // 8. CATEGORY AGE RANGE TESTS (5 cases)
        [Theory]
        [InlineData(18, 30, true)]
        [InlineData(5, 12, true)]
        [InlineData(1, 100, true)]
        [InlineData(30, 18, false)]
        [InlineData(-1, 10, false)]
        public void ValidateAgeRange(int minimumAge, int maximumAge, bool expected)
        {
            bool result = minimumAge >= 0
                && maximumAge >= minimumAge;

            Assert.Equal(expected, result);
        }

        // 9. ENROLMENT STATUS TESTS (5 cases)
        [Theory]
        [InlineData("Pending", true)]
        [InlineData("Confirmed", true)]
        [InlineData("Cancelled", true)]
        [InlineData("Unknown", false)]
        [InlineData("", false)]
        public void ValidateEnrolmentStatus(string status, bool expected)
        {
            string[] validStatuses =
            {
                "Pending",
                "Confirmed",
                "Cancelled"
            };

            bool result = validStatuses.Contains(status);
            Assert.Equal(expected, result);
        }

        // 10. RESULT POSITION TESTS (5 cases)
        [Theory]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(100, true)]
        [InlineData(0, false)]
        [InlineData(-1, false)]
        public void ValidateResultPosition(int position, bool expected)
        {
            bool result = position > 0;
            Assert.Equal(expected, result);
        }

        // 11. PAYMENT METHOD TESTS (5 cases)
        [Theory]
        [InlineData("Card", true)]
        [InlineData("EFT", true)]
        [InlineData("Cash", true)]
        [InlineData("Unknown", false)]
        [InlineData("", false)]
        public void ValidatePaymentMethod(string method, bool expected)
        {
            string[] validMethods =
            {
                "Card",
                "EFT",
                "Cash"
            };

            bool result = validMethods.Contains(method);
            Assert.Equal(expected, result);
        }

        // 12. PAYMENT STATUS TESTS (5 cases)
        [Theory]
        [InlineData("Pending", true)]
        [InlineData("Completed", true)]
        [InlineData("Failed", true)]
        [InlineData("Unknown", false)]
        [InlineData("", false)]
        public void ValidatePaymentStatus(string status, bool expected)
        {
            string[] validStatuses =
            {
                "Pending",
                "Completed",
                "Failed"
            };

            bool result = validStatuses.Contains(status);
            Assert.Equal(expected, result);
        }
    }
}