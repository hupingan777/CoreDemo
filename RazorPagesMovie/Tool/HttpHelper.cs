using Microsoft.AspNetCore.Http;

namespace RazorPagesMovie.Tool
{
    public class HttpHelper
    {
        public static string GetRequestUrl(HttpContext httpContext, string handlerName)
        {
            var result = httpContext.Request.Path.Value;
            //string result = $"/usermains/edit";//?handler={handlerName}
            if (handlerName.Trim() != string.Empty)
            {
                result += $"?handler={handlerName}";
            }
            return result;
        }
    }
}
