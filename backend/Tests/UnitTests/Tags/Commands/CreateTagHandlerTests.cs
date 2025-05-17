using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests.Tags.Commands
{
    public class CreateTagHandlerTests
    {
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
        public async Task Add_ShouldAddNewTag()
        {
            // Arrange
            var context = await GetDbContextAsync();
            var repo = new TagRepository(context);

            var tag = new Tag
            {
                Name = "Technology",
                Color = "Blue"
            };

            // Act
            repo.Add(tag);
            await context.SaveChangesAsync();

            // Assert
            var dbTag = await context.Tags.FirstOrDefaultAsync(t => t.Name == "Technology");
            Assert.NotNull(dbTag);
            Assert.Equal("Technology", dbTag.Name);
            Assert.Equal("Blue", dbTag.Color);
        }
    }
}
