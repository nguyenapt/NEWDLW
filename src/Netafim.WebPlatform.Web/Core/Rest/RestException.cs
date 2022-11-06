namespace Netafim.WebPlatform.Web.Core.Rest
{
    public class RestException : System.Exception
    {
        public RestException(string message) : base(message)
        {
        }

        public RestException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}