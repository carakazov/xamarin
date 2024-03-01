using System.Net;

namespace Frontend.dto
{
    public class Response
    {
        public HttpStatusCode Code { get; set; }
        public object Data { get; set; }
    }
}