using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Tests.UnitTests.Tags.Queries
{
    public class GetByIdTagsHandlerTests
    {
        private static async Task<Context> GetDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new Context(options);
            await context.Database.EnsureCreatedAsync();
            return context;
        }

        private Tag LoadTagFromJson()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var projectRoot = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\..\Tests"));
            var jsonPath = Path.Combine(projectRoot, "UnitTests", "TestData", "Tags", "GetByIdTagTestData.json");

            if (!File.Exists(jsonPath))
                throw new FileNotFoundException($"File not found: {jsonPath}");

            var json = File.ReadAllText(jsonPath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var tag = JsonSerializer.Deserialize<Tag>(json, options);

            return tag!;
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTag()
        {
            // Arrange
            var context = await GetDbContextAsync();
            var repo = new TagRepository(context);
            var tag = LoadTagFromJson();

            repo.Add(tag);
            await context.SaveChangesAsync();

            // Act
            var foundTag = await repo.GetByIdAsync(tag.Id);

            // Assert
            Assert.NotNull(foundTag);
            Assert.Equal(tag.Name, foundTag.Name);
            Assert.Equal(tag.Color, foundTag.Color);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowIfNotFound()
        {
            // Arrange
            var context = await GetDbContextAsync();
            var repo = new TagRepository(context);
            var randomId = Guid.NewGuid();

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                repo.GetByIdAsync(randomId));

            Assert.Contains("Tag with ID", exception.Message);
        }
    }
}
