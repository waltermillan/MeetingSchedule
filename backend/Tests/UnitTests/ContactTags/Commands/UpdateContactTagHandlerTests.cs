using API.Features.Contacts.Update;
using API.Features.ContactTags.Update;
using Core.Entities;
using Core.Interfaces;
using Moq;
using System.Text.Json;

namespace Tests.UnitTests.ContactTags.Commands
{
    public class UpdateContactTagHandlerTests
    {
        // Helper to load test data from a JSON file
        private UpdateContactTagCommand LoadCommandFromJson()
        {
            var currentDir = Directory.GetCurrentDirectory();
            var projectRoot = Path.GetFullPath(Path.Combine(currentDir, @"..\..\..\..\Tests"));
            var jsonPath = Path.Combine(projectRoot, "UnitTests", "TestData", "ContactTags", "UpdateContactTagTestData.json");

            if (!File.Exists(jsonPath))
                throw new FileNotFoundException($"File not found: {jsonPath}");

            var json = File.ReadAllText(jsonPath);
            var command = JsonSerializer.Deserialize<UpdateContactTagCommand>(json);
            return command!;
        }

        [Fact]
        public async Task Handle_ShouldUpdateContactTag_AndReturnTrue()
        {
            // Arrange
            var command = LoadCommandFromJson();

            var existingEntity = new ContactTag
            {
                Id = command.Id,
                ContactId = Guid.NewGuid(),
                TagId = Guid.NewGuid()
            };

            var fakeRepo = new Mock<IContactTagRepository>();
            var fakeUow = new Mock<IUnitOfWork>();

            fakeRepo.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync(existingEntity);
            fakeUow.Setup(u => u.ContactTags).Returns(fakeRepo.Object);
            fakeUow.Setup(u => u.SaveAsync(default)).ReturnsAsync(1);

            var handler = new UpdateContactTagHandler(fakeUow.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.True(result);
            Assert.Equal(command.ContactId, existingEntity.ContactId);
            Assert.Equal(command.ContactId, existingEntity.TagId);

            fakeRepo.Verify(r => r.Update(existingEntity), Times.Once);
            fakeUow.Verify(u => u.SaveAsync(default), Times.Once);
        }
    }
}
