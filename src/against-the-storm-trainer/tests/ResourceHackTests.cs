using System;
using Xunit;
using AgainstTheStormTrainer.Core;
using Moq;

namespace AgainstTheStormTrainer.Tests
{
    /// <summary>
    /// Unit tests for ResourceHack using a mocked memory manager.
    /// </summary>
    public class ResourceHackTests
    {
        [Fact]
        public void SetAllResources_ReturnsTrue_WhenAllWritesSucceed()
        {
            // Arrange
            var mockMemory = new Mock<GameMemoryManager>("dummy");
            mockMemory.Setup(m => m.WriteInt(It.IsAny<IntPtr>(), It.IsAny<int>())).Returns(true);
            var resourceHack = new ResourceHack(mockMemory.Object);

            // Act
            bool result = resourceHack.SetAllResources(9999);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void SetAllResources_ReturnsFalse_WhenOneWriteFails()
        {
            // Arrange
            var mockMemory = new Mock<GameMemoryManager>("dummy");
            int callCount = 0;
            mockMemory.Setup(m => m.WriteInt(It.IsAny<IntPtr>(), It.IsAny<int>()))
                      .Returns(() => { callCount++; return callCount != 2; }); // Fail on second write
            var resourceHack = new ResourceHack(mockMemory.Object);

            // Act
            bool result = resourceHack.SetAllResources(500);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void GetResourceValue_ReturnsMinusOne_ForUnknownResource()
        {
            // Arrange
            var mockMemory = new Mock<GameMemoryManager>("dummy");
            var resourceHack = new ResourceHack(mockMemory.Object);

            // Act
            int value = resourceHack.GetResourceValue("nonexistent");

            // Assert
            Assert.Equal(-1, value);
        }
    }
}
