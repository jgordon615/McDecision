using System.IO;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

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
            config.Routes.MapHttpRoute(
                name: "Yes",
                routeTemplate: "yes",
                defaults: new
                {
                    controller = "Index",
                    action = "Yes"
                }, constraints: new { httpMethod = new System.Web.Http.Routing.HttpMethodConstraint(System.Net.Http.HttpMethod.Get) }
            );

            config.Routes.MapHttpRoute(
                name: "No",
                routeTemplate: "no",
                defaults: new
                {
                    controller = "Index",
                    action = "No"
                }, constraints: new { httpMethod = new System.Web.Http.Routing.HttpMethodConstraint(System.Net.Http.HttpMethod.Get) }
            );

            // Home page for over-achievers.  /index
            config.Routes.MapHttpRoute(
                name: "Index",
                routeTemplate: "index",
                defaults: new
                {
                    controller = "Index",
                    action = "Index"
                }, constraints: new { httpMethod = new System.Web.Http.Routing.HttpMethodConstraint(System.Net.Http.HttpMethod.Get) }
            );

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
        }

        /// <summary>
        /// Home page.
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET")]
        public HttpResponseMessage Index()
        {
            var response = Request.CreateResponse();
            var path = HttpContext.Current.Server.MapPath("/Views/Index.html");
            response.Content = new StringContent(File.ReadAllText(path), Encoding.UTF8, "text/html");
            return response;
        }

        /// <summary>
        /// The "YES" image!
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET")]
        public HttpResponseMessage Yes()
        {
            var response = Request.CreateResponse();
            var path = HttpContext.Current.Server.MapPath("/Views/images/yes.png");
            response.Content = new StreamContent(new FileStream(path, FileMode.Open));
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
            return response;
        }

        /// <summary>
        /// The "NO" image!
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET")]
        public HttpResponseMessage No()
        {
            var response = Request.CreateResponse();
            var path = HttpContext.Current.Server.MapPath("/Views/images/no.png");
            response.Content = new StreamContent(new FileStream(path, FileMode.Open));
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
            return response;
        }
    }
}
