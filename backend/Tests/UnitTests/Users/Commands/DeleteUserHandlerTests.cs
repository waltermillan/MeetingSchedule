using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Tests.UnitTests.Users.Commands
{
    public class DeleteUserHandlerTests
    {
        // Helper to load test data from a JSON file
        private List<User> LoadUserDataFromJson()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var projectRoot = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\..\Tests"));
            var jsonPath = Path.Combine(projectRoot, "UnitTests", "TestData", "Users", "DeleteUserTestData.json");

            if (!File.Exists(jsonPath))
                throw new FileNotFoundException($"File not found: {jsonPath}");

            var json = File.ReadAllText(jsonPath);
            var data = JsonSerializer.Deserialize<List<User>>(json);
            return data!;
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
        public async Task Remove_ShouldDeleteUser()
        {
            // Arrange
            var context = await GetDbContextAsync();
            var repo = new UserRepository(context);

            var users = LoadUserDataFromJson();
            var user = users[0];

            // Agregar usuario
            repo.Add(user);
            await context.SaveChangesAsync();

            // Act
            repo.Remove(user);
            await context.SaveChangesAsync();

            // Assert
            var deletedUser = await context.Users.FindAsync(user.Id);
            Assert.Null(deletedUser);
        }
    }
}
