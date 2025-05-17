using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Tests.UnitTests.Tags.Queries
{
    public class GetAllTagsHandlerTests
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

        private List<Tag> LoadTagsFromJson()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var projectRoot = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\..\Tests"));
            var jsonPath = Path.Combine(projectRoot, "UnitTests", "TestData", "Tags", "GetAllTagsTestData.json");

            if (!File.Exists(jsonPath))
                throw new FileNotFoundException($"File not found: {jsonPath}");

            var json = File.ReadAllText(jsonPath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var tags = JsonSerializer.Deserialize<List<Tag>>(json, options);

            return tags!;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllTags()
        {
            // Arrange
            var context = await GetDbContextAsync();
            var repo = new TagRepository(context);

            var tags = LoadTagsFromJson();

            context.Tags.AddRange(tags);
            await context.SaveChangesAsync();

            // Act
            var result = await repo.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(tags.Count, result.Count());
            Assert.All(tags, tag =>
            Assert.Contains(result, r => r.Name == tag.Name));
        }
    }
}
