using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FlubuCore.Context;
using FlubuCore.Infrastructure;
using FlubuCore.Tasks;

namespace FlubuCore.Gitter
{
    public class GitterTask : TaskBase<int, GitterTask>
    {
        private const string GitterBaseUrl = "https://api.gitter.im";


        private readonly string _message;
        private readonly string _room;
        private readonly string _token;
        private readonly IHttpClientFactory _httpClientFactory;

        public GitterTask(string message, string room, string token, IHttpClientFactory httpClientFactory)
        {
            _message = message;
            _room = room;
            _token = token;
            _httpClientFactory = httpClientFactory;
        }

        protected override int DoExecute(ITaskContextInternal context)
        {
            var task = DoExecuteAsync(context);

            return task.GetAwaiter().GetResult();
        }

        protected override async Task<int> DoExecuteAsync(ITaskContextInternal context)
        {
            var client = _httpClientFactory.Create(GitterBaseUrl);

            client.DefaultRequestHeaders.Authorization = !string.IsNullOrEmpty(_token)
                ? new AuthenticationHeaderValue("Bearer", _token)
                : null;

            var uri = new Uri($"{GitterBaseUrl}/v1/rooms/{_room}/chatMessages");

            HttpRequestMessage requestMessage = new HttpRequestMessage();
            requestMessage.RequestUri = uri;
            requestMessage.Method = HttpMethod.Post;
            requestMessage.Content = new FormUrlEncodedContent(new Dictionary<string, string> {{"text", _message}});
            var response = await client.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode)
            {
                throw new TaskExecutionException(await response.Content.ReadAsStringAsync(), 20);
            }

            return 0;
        }

        protected override string Description { get; set; }
    }
}
