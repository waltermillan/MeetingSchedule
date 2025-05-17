using API.Controllers;
using API.DTOs;
using API.Responses;
using API.Services;
using Core.Constants;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Tests.UnitTests.Users.Queries
{
    public class UsersLoginTests
    {
        [Fact]
        public async Task Login_ShouldReturnOk_WhenCredentialsAreValid()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Francis",
                UserName = "BLOSSON",
                Password = "hashed-password"
            };

            var loginRequest = new LoginRequest
            {
                UserName = "blosson",
                Password = "plain-password"
            };

            // Mock del repositorio interno
            var mockUserRepo = new Mock<IUserRepository>();
            mockUserRepo.Setup(r => r.GetByNameAsync("BLOSSON")).ReturnsAsync(user);

            var userService = new UserService(mockUserRepo.Object);

            var mockHasher = new Mock<IPasswordHasher>();
            mockHasher.Setup(h => h.VerifyPassword("plain-password", "hashed-password")).Returns(true);

            var controller = new UsersController(null!, userService, mockHasher.Object);

            // Act
            var result = await controller.Login(loginRequest) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            var response = Assert.IsType<ApiResponse<object>>(result.Value);
            Assert.Equal(UserMessages.LoginSuccess, response.Message);
        }
    }
}
