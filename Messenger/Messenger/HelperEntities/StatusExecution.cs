using System.Net;

namespace Messenger.HelperEntities
{
    public class StatusExecution
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }
}
