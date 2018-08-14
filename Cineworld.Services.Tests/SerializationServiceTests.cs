using Xunit;

namespace Cineworld.Services.Tests
{
	public class SerializationServiceTests
	{
		private readonly ISerializationService _service;

		public SerializationServiceTests()
		{
			_service = new Concrete.SerializationService();
		}

		[Theory]
		[InlineData(null, 0)]
		[InlineData(null, 1)]
		[InlineData("test", 0)]
		[InlineData("test", 1)]
		public void DeepCloneTests_DeepClone_BehavesPredictably(
			string s,
			int i)
		{
			// Arrange
			var original = new Root { String = s, Int32 = i, };

			// Act
			var copy = _service.DeepClone(original);

			// Assert
			Assert.NotNull(copy);
			Assert.NotSame(original, copy);
			Assert.Equal(original.String, copy.String);
			Assert.Equal(original.Int32, copy.Int32);
		}
	}
}
