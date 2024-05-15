using System;
using NUnit.Framework;
using objectSpread;

namespace objectSpread.Tests
{
    public class ObjectExtensionsTests
    {
        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public string Country { get; set; }
        }

        [Test]
        public void Spread_ShouldMergeProperties_FromMultipleObjects()
        {
            // Arrange
            var obj1 = new { Name = "Alice", Age = 25 };
            var obj2 = new { Age = 30, Country = "Wonderland" };

            // Act
            var result = ObjectExtensions.Spread<Person>(obj1, obj2);

            // Assert
            Assert.AreEqual("Alice", result.Name);
            Assert.AreEqual(30, result.Age);
            Assert.AreEqual("Wonderland", result.Country);
        }

        [Test]
        public void Spread_ShouldHandleNullSources()
        {
            // Arrange
            var obj1 = new { Name = "Alice" };
            object obj2 = null;

            // Act
            var result = ObjectExtensions.Spread<Person>(obj1, obj2);

            // Assert
            Assert.AreEqual("Alice", result.Name);
            Assert.AreEqual(0, result.Age); // Default int value
            Assert.IsNull(result.Country); // Default string value
        }

        [Test]
        public void Spread_ShouldOverwritePropertiesWithNullValues()
        {
            // Arrange
            var obj1 = new { Name = "Alice", Age = 25 };
            var obj2 = new { Name = (string)null, Country = "Wonderland" };

            // Act
            var result = ObjectExtensions.Spread<Person>(obj1, obj2);

            // Assert
            Assert.IsNull(result.Name); 
            Assert.AreEqual(25, result.Age); 
            Assert.AreEqual("Wonderland", result.Country); 
        }


        [Test]
        public void Spread_ShouldNotSetReadOnlyProperties()
        {
            // Arrange
            var obj1 = new { ReadOnlyProp = "ReadOnlyValue" };
            var obj2 = new { };

            // Act
            var result = ObjectExtensions.Spread<ReadOnlyClass>(obj1, obj2);

            // Assert
            Assert.IsNull(result.ReadOnlyProp); // ReadOnly property should not be set
        }

        public class ReadOnlyClass
        {
            public string ReadOnlyProp { get; }
        }

        [Test]
        public void Spread_ShouldSkipPropertiesWithMismatchedTypes()
        {
            // Arrange
            var obj1 = new { Name = "Alice", Age = "NotANumber" }; // Age is string, should be int
            var obj2 = new { Country = "Wonderland" };

            // Act
            var result = ObjectExtensions.Spread<Person>(obj1, obj2);

            // Assert
            Assert.AreEqual("Alice", result.Name);
            Assert.AreEqual(0, result.Age); // Default int value, as "NotANumber" should be skipped
            Assert.AreEqual("Wonderland", result.Country);
        }

        [Test]
        public void Spread_ShouldWorkWithNoSourceObjects()
        {
            // Act
            var result = ObjectExtensions.Spread<Person>();

            // Assert
            Assert.NotNull(result); // Should create a new instance
            Assert.IsNull(result.Name); // Default string value
            Assert.AreEqual(0, result.Age); // Default int value
            Assert.IsNull(result.Country); // Default string value
        }
    }
}
