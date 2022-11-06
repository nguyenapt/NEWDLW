using System;
using System.Runtime.Serialization;

namespace Netafim.WebPlatform.Web.Core.Exceptions
{
    public class BusinessException : Exception
    {
        public string Code { get; }

        public BusinessException()
        { }

        public BusinessException(string code, string message) : base(message)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException(nameof(code));

            Code = code;
        }

        public BusinessException(string code, string message, Exception innerException) : base(message, innerException)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentNullException(nameof(code));

            Code = code;
        }

        protected BusinessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            Code = info.GetString("Code");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Code", Code);

            base.GetObjectData(info, context);
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Code)) return base.ToString();

            return $"Code {Code} - {base.ToString()}";
        }
    }
}