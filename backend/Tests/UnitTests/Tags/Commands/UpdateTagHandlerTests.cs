using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Tests.UnitTests.Tags.Commands
{
    public class UpdateTagHandlerTests
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
            var jsonPath = Path.Combine(projectRoot, "UnitTests", "TestData", "Tags", "UpdateTagTestData.json");

            if (!File.Exists(jsonPath))
                throw new FileNotFoundException($"File not found: {jsonPath}");

            var json = File.ReadAllText(jsonPath);
            var tag = JsonSerializer.Deserialize<Tag>(json);
            return tag!;
        }

        [Fact]
        public async Task Update_ShouldModifyTag()
        {
            // Arrange
            var context = await GetDbContextAsync();
            var repo = new TagRepository(context);

            var tag = LoadTagFromJson();

            repo.Add(tag);
            await context.SaveChangesAsync();

            tag.Name = "Updated Science";
            tag.Color = "Red";

            // Act
            repo.Update(tag);
            await context.SaveChangesAsync();

            // Assert
            var updatedTag = await context.Tags.FindAsync(tag.Id);
            Assert.NotNull(updatedTag);
            Assert.Equal("Updated Science", updatedTag.Name);
            Assert.Equal("Red", updatedTag.Color);
        }
    }
}
