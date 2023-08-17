using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace IB_projekat.tools
{
    public class CustomMediaTypeInputFormatter : InputFormatter
    {
        private readonly string _mediaType;

        public CustomMediaTypeInputFormatter(string mediaType)
        {
            _mediaType = mediaType;
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse(mediaType));
        }

        protected override bool CanReadType(Type type)
        {
            return type == typeof(byte[]);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;
            using (var ms = new MemoryStream())
            {
                await request.Body.CopyToAsync(ms);
                var certData = ms.ToArray();
                return await InputFormatterResult.SuccessAsync(certData);
            }
        }
    }
}
