using System.Threading;
using System.Web;
using System.Web.Http;
using MCHost;
using WebApi.ViewModels;

namespace WebApi.Controllers
{
    public class McController : ApiController
    {
        /// <summary>
        /// Register our MVC routes.
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "MakeDecision",
                routeTemplate: "decision",
                defaults: new
                {
                    controller = "Mc",
                    action = "MakeDecision"
                }, constraints: new { httpMethod = new System.Web.Http.Routing.HttpMethodConstraint(System.Net.Http.HttpMethod.Get) }
            );
        }

        /// <summary>
        /// We need a decision from steve!  Kick off the process within the host and wait for an answer.
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET")]
        public DecisionResult MakeDecision()
        {
            DecisionResult result = new DecisionResult();
            Host host = (Host)HttpContext.Current.Application["host"];
            if (host.HasExited)
            {
                host = MvcApplication.ResetHost();
            }
            bool? decision = null;

            host.DecisionMade += (s, e) => {
                decision = e;
                result.ServerResponded = true;
            };

            host.BeginDecision();

            var duration = 0;
            while(!decision.HasValue) {
                if (duration >= 5000)
                {
                    // Steve didn't answer, so send the default answer.
                    //decision = new Random().Next(0, 1) == 0;
                    decision = true;
                    result.ServerResponded = false;
                    break;
                }
                duration += 100;
                Thread.Sleep(100);
            }

            result.Decision = decision.Value;

            return result;
        }
    }
}
