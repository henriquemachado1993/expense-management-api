using FluentAssertions;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseApi.TestIntegration
{
    public class TestControllerTest
    {
        private readonly TestContext _testContext;

        public TestControllerTest()
        {
            _testContext = new TestContext();
        }

        [Fact]
        public async Task TestGetNumbersReturnOkReponse()
        {
            var response = await _testContext.Client.GetAsync("test");
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task TestGetNumbersReturnNotFoundReponse()
        {
            var response = await _testContext.Client.GetAsync("test/ff");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}