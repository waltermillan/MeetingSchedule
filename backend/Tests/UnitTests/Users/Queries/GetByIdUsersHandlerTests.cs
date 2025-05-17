using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Tests.UnitTests.Users.Queries
{
    public class GetByIdUsersHandlerTests
    {
        // Helper to load test data from a JSON file
        private User LoadUserDataFromJson()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var projectRoot = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\..\Tests"));
            var jsonPath = Path.Combine(projectRoot, "UnitTests", "TestData", "Users", "GetByIdUsersTestData.json");

            if (!File.Exists(jsonPath))
                throw new FileNotFoundException($"File not found: {jsonPath}");

            var json = File.ReadAllText(jsonPath);
            var data = JsonSerializer.Deserialize<List<User>>(json);
            return data?.FirstOrDefault()!;
        }

        // Helper to create an in-memory DbContext
        private static async Task<Context> GetDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new Context(options);
            await context.Database.EnsureCreatedAsync();

            return context;
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser()
        {
            // Arrange
            var context = await GetDbContextAsync();
            var repo = new UserRepository(context);

            var user = LoadUserDataFromJson();

            // Agregar usuario
            repo.Add(user);
            await context.SaveChangesAsync();

            // Act
            var foundUser = await repo.GetByIdAsync(user.Id);

            // Assert
            Assert.NotNull(foundUser);
            Assert.Equal(user.Name, foundUser.Name);
            Assert.Equal(user.UserName, foundUser.UserName);
            Assert.Equal(user.Password, foundUser.Password);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowIfNotFound()
        {
            // Arrange
            var context = await GetDbContextAsync();
            var repo = new UserRepository(context);

            var randomId = Guid.NewGuid();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                repo.GetByIdAsync(randomId));

            Assert.Contains("User with ID", exception.Message);
        }
    }
}
