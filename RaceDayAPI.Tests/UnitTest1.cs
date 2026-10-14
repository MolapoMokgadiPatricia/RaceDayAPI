using RaceDayAPI.Services;
using Xunit;

namespace RaceDayAPI.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void HashPassword_ShouldReturnDifferentHashFromPlainPassword()
        {
            var passwordService = new PasswordService();
            string password = "RaceDayTest123!";

            string hashedPassword = passwordService.HashPassword(password);

            Assert.NotEqual(password, hashedPassword);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrueForCorrectPassword()
        {
            var passwordService = new PasswordService();
            string password = "RaceDayTest123!";

            string hashedPassword = passwordService.HashPassword(password);

            bool result = passwordService.VerifyPassword(
                password,
                hashedPassword);

            Assert.True(result);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalseForIncorrectPassword()
        {
            var passwordService = new PasswordService();
            string password = "RaceDayTest123!";

            string hashedPassword = passwordService.HashPassword(password);

            bool result = passwordService.VerifyPassword(
                "WrongPassword123!",
                hashedPassword);

            Assert.False(result);
        }
        [Fact]
        public void HashPassword_ShouldGenerateDifferentHashesForSamePassword()
        {
            var passwordService = new PasswordService();
            string password = "RaceDayTest123!";

            string firstHash = passwordService.HashPassword(password);
            string secondHash = passwordService.HashPassword(password);

            Assert.NotEqual(firstHash, secondHash);
        }
    }
}