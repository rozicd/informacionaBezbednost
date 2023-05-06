using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace IB_projekat.tools
{
    public class CustomMediaTypeOutputFormatter : OutputFormatter
    {
        private readonly string _mediaType;

        public CustomMediaTypeOutputFormatter(string mediaType)
        {
            _mediaType = mediaType;
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(mediaType));
        }

        protected override bool CanWriteType(Type type)
        {
            return type == typeof(byte[]);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context)
        {
            var response = context.HttpContext.Response;
            var bytes = context.Object as byte[];
            if (bytes != null)
            {
                response.ContentType = _mediaType;
                await response.Body.WriteAsync(bytes, 0, bytes.Length);
            }
        }
    }
}
