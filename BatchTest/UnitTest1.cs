using ODataBatchResponseSample;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace BatchTest
{

    public class UnitTest1 : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private readonly ITestOutputHelper _output;

        public UnitTest1(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _client = factory.CreateClient();
            _output = output;
        }

        [Fact()]
        public async Task LineBreak()
        {
            var batch = new MultipartContent("mixed", "batch" + Guid.NewGuid());
            var changeset = new MultipartContent("mixed", "changeset" + Guid.NewGuid());

            {
                var message = new HttpRequestMessage(HttpMethod.Post, "http://localhost/Data")
                {
                    Content = Helpers.JSON(new
                    {
                        Name = "D"
                    })
                };
                message.Headers.Add("Content-ID", "1");

                var content = new HttpMessageContent(message);
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/http");
                content.Headers.Add("Content-Transfer-Encoding", "binary");

                changeset.Add(content);
            }

            batch.Add(changeset);
            var response = await _client.PostAsync("/$batch", batch);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("multipart/mixed", response.Content.Headers.ContentType.MediaType);

            var binary = await response.Content.ReadAsByteArrayAsync();
            var hexdump = Helpers.HexDump(binary);

            _output.WriteLine(hexdump);
            Console.WriteLine(hexdump);

            for (var i = 0; i < binary.Length; i++)
            {
                if (binary[i] == 0x0a)
                {
                    Assert.Equal(0x0d, binary[i - 1]);
                }
            };
        }
    }
}
