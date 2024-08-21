using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System.Net.Http.Headers;

namespace IronDomeApi.Utils
{
    public class HttpUtils
    {
        public static object Response(int status, object message)
        {
            if (status != null)
            {
                if (status >= 200 && status < 300)
                {
                    return new
                    {
                        success = true,
                        message = message,

                    };

                }
                else if (status > 400)
                {
                    return new
                    {
                        success = false,
                        message = message,
                    };

                }
            }
            return null;
        }
    }
}
