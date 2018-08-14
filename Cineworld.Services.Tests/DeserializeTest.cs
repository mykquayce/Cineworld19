using Xunit;

namespace Cineworld.Services.Tests
{
	public class DeserializeTest
	{
		private readonly ISerializationService _service;

		public DeserializeTest()
		{
			_service = new Concrete.SerializationService();
		}

		[Theory]
		[InlineData("test", 1)]
		public void DeserializeTest_Deserialze_BehavesPredictably(
			string s,
			int i)
		{
			// Arrange
			var before = new Root { String = s, Int32 = i, };
			Root after;
			using (var stream = _service.Serialize(before))
			{
				// Act
				after = _service.Deserialize<Root>(stream);
			}

			// Assert
			Assert.NotNull(after);
			Assert.NotSame(before, after);
			Assert.Equal(before.String, after.String);
			Assert.Equal(before.Int32, after.Int32);
		}
	}
}
