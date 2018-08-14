using Xunit;

namespace Cineworld.Services.Tests
{
	public class SerializeTest
	{
		private readonly ISerializationService _service;

		public SerializeTest()
		{
			_service = new Concrete.SerializationService();
		}

		[Theory]
		[InlineData("test", 1)]
		public void SerializeTest_Serialze_BehavesPredictably(
			string s,
			int i)
		{
			// Arrange
			var version = new Root { String = s, Int32 = i, };

			// Act
			var stream = _service.Serialize(version);

			// Assert
			Assert.NotNull(stream);
			Assert.True(stream.CanRead);
			Assert.NotEqual(0L, stream.Length);
			Assert.Equal(0L, stream.Position);

			var length = 0;

			do
			{
				var buffer = new byte[1_024];

				length = stream.Read(buffer, 0, 1_024);

				Assert.NotNull(buffer);
				Assert.NotEmpty(buffer);
			}
			while (length > 0);

			// Arrange
			stream?.Dispose();
		}
	}
}
