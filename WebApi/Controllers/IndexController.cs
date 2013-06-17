using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;

namespace WebApi.Controllers
{
    public class IndexController : ApiController
    {
        /// <summary>
        /// Register our MVC routes.
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            // Default home page.  /
            config.Routes.MapHttpRoute(
                name: "DefaultIndex",
                routeTemplate: "",
                defaults: new
                {
                    controller = "Index",
                    action = "Index"
                }, constraints: new { httpMethod = new System.Web.Http.Routing.HttpMethodConstraint(System.Net.Http.HttpMethod.Get) }
            );

            config.Routes.MapHttpRoute(
                name: "StaticFiles",
                routeTemplate: "{*allValues}",
                defaults: new
                {
                    controller = "Index",
                    action = "Static"
                },
                constraints: new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) }
            );
        }

        [AcceptVerbs("GET")]
        public HttpResponseMessage Static(string allValues)
        {
            HttpResponseMessage response = null;
            var path = HttpContext.Current.Server.MapPath(Path.Combine("/Static", allValues));

            //Look for the following file types:
            var types = new[] { 
                new { extension = "html", contentType = "text/html" },
                new { extension = "js", contentType = "text/javascript" },
                new { extension = "png", contentType = "image/png" },
                new { extension = "gif", contentType = "image/gif" },
                new { extension = "css", contentType = "text/css" }
            };

            Func<string, string, HttpResponseMessage> fn = (filepath, contentType) =>
            {
                HttpResponseMessage resp;

                resp = Request.CreateResponse();
                resp.Content = new ByteArrayContent(File.ReadAllBytes(filepath));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);

                return resp;
            };

            if (allValues.Contains("."))
            {
                var exists = File.Exists(path);
                if (exists)
                {
                    var mime = IndexController.GetMimeType(path);
                    response = fn(path, mime);
                }
                else
                {
                    throw new Exception(path);
                }
            }
            else
            {
                foreach (var type in types)
                {
                    var pe = path + "." + type.extension;
                    var exists = File.Exists(pe);
                    if (exists)
                    {
                        response = fn(pe, type.contentType);

                        break;
                    }
                }
            }

            if (response == null)
                throw new HttpException(404, "Not found.");

            return response;
        }

        private static string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        /// <summary>
        /// Home page.
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET")]
        public HttpResponseMessage Index()
        {
            var response = Request.CreateResponse();
            var path = HttpContext.Current.Server.MapPath("/Static/Index.html");
            response.Content = new StringContent(File.ReadAllText(path), Encoding.UTF8, "text/html");
            return response;
        }
    }
}
