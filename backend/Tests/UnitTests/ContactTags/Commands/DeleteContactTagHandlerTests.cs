using API.Features.ContactTags.Delete;
using Core.Entities;
using Core.Interfaces;
using Moq;
using System.Text.Json;

namespace Tests.UnitTests.ContactTags.Commands
{
    public class DeleteContactTagHandlerTests
    {
        // Helper to load test data from a JSON file
        private DeleteContactTagCommand LoadCommandFromJson()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var projectRoot = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\..\Tests"));
            var jsonPath = Path.Combine(projectRoot, "UnitTests", "TestData", "ContactTags", "DeleteContactTagTestData.json");

            if (!File.Exists(jsonPath))
                throw new FileNotFoundException($"File not found: {jsonPath}");

            var json = File.ReadAllText(jsonPath);
            var command = JsonSerializer.Deserialize<DeleteContactTagCommand>(json);
            return command!;
        }

        [Fact]
        public async Task Handle_ShouldDeleteContactTag_AndReturnTrue()
        {
            // Arrange
            var command = LoadCommandFromJson();

            var existingEntity = new ContactTag
            {
                Id = command.Id,
                ContactId = Guid.NewGuid(),
                TagId = Guid.NewGuid()
            };

            var mockRepo = new Mock<IContactTagRepository>();
            var mockUow = new Mock<IUnitOfWork>();

            mockRepo.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(existingEntity);
            mockUow.Setup(u => u.ContactTags).Returns(mockRepo.Object);
            mockUow.Setup(u => u.SaveAsync(default)).ReturnsAsync(1);

            var handler = new DeleteContactTagHandler(mockUow.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.True(result);
            mockRepo.Verify(r => r.Remove(existingEntity), Times.Once);
            mockUow.Verify(u => u.SaveAsync(default), Times.Once);
        }
    }
}
