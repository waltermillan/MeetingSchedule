using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Tests.UnitTests.Tags.Commands
{
    public class DeleteTagHandlerTests
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
            var jsonPath = Path.Combine(projectRoot, "UnitTests", "TestData", "Tags", "DeleteTagTestData.json");

            if (!File.Exists(jsonPath))
                throw new FileNotFoundException($"File not found: {jsonPath}");

            var json = File.ReadAllText(jsonPath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var tag = JsonSerializer.Deserialize<Tag>(json, options);

            return tag!;
        }

        [Fact]
        public async Task Remove_ShouldDeleteTag()
        {
            // Arrange
            var context = await GetDbContextAsync();
            var repo = new TagRepository(context);

            var tag = LoadTagFromJson();

            repo.Add(tag);
            await context.SaveChangesAsync();

            // Act
            repo.Remove(tag);
            await context.SaveChangesAsync();

            // Assert
            var deletedTag = await context.Tags.FindAsync(tag.Id);
            Assert.Null(deletedTag);
        }
    }
}
