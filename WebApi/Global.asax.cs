using System.Web.Http;
using System.Web.Mvc;
using MCHost;
using Newtonsoft.Json.Serialization;

namespace WebApi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        // Our minecraft wrapper!
        private static Host _host;

        /// <summary>
        /// If you don't know what this does, you shouldn't be here.
        /// </summary>
        protected void Application_Start()
        {
            if (_host == null)
                _host = new Host();

            // Save a reference to our minecraft host in Application state so we can reference
            // it when we need to make decisions.
            Application.Add("host", _host);

            var config = GlobalConfiguration.Configuration;

            AreaRegistration.RegisterAllAreas();

            // Routes
            Controllers.IndexController.Register(config);
            Controllers.McController.Register(config);


            // Web APIs should return JSON.
            var fmt = config.Formatters.JsonFormatter;
            fmt.UseDataContractJsonSerializer = false;
            fmt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            GlobalFilters.Filters.Add(new HandleErrorAttribute());

            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
        }

        /// <summary>
        /// Clean up our minecraft host.
        /// </summary>
        public override void Dispose()
        {
            if (_host != null)
                _host.Dispose();
            base.Dispose();
        }
    }
}