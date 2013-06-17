using System;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using MCHost;
using Newtonsoft.Json.Serialization;
using System.Diagnostics;
using System.IO;

namespace WebApi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static List<int> _hostPids = new List<int>();

        public static Host ResetHost()
        {
            if (_hostPids.Count() > 0)
            {
                foreach (var pid in _hostPids)
                {
                    var proc = Process.GetProcessById(pid);
                    proc.Kill();
                }
                _hostPids.Clear();
            }

            Host host = (Host)HttpContext.Current.Application["host"];
            if (host != null)
                host.Dispose();
            HttpContext.Current.Application.Remove("host");


            // Kill any remaining minecraft server apps.  They have a process name of "java",
            // whereas the minecraft client uses a process name of "javaw".
            var procs = from proc in Process.GetProcesses()
                        where proc.ProcessName == "java"
                        select proc;

            foreach (var proc in procs)
            {
                proc.Kill();
            }

            host = new Host();
            if (File.Exists("C:\\mcstarted.txt"))
                File.Delete("C:\\mcstarted.txt");
            File.WriteAllText("C:\\mcstarted.txt", DateTime.Now.ToString());

            _hostPids.Add(host.HostProcessId);

            // Save a reference to our minecraft host in Application state so we can reference
            // it when we need to make decisions.
            HttpContext.Current.Application.Add("host", host);

            return host;
        }


        /// <summary>
        /// If you don't know what this does, you shouldn't be here.
        /// </summary>
        protected void Application_Start()
        {
            ResetHost();

            var config = GlobalConfiguration.Configuration;

            AreaRegistration.RegisterAllAreas();

            // Routes
            Controllers.McController.Register(config);
            Controllers.IndexController.Register(config);


            // Web APIs should return JSON.
            var fmt = config.Formatters.JsonFormatter;
            fmt.UseDataContractJsonSerializer = false;
            fmt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            GlobalFilters.Filters.Add(new HandleErrorAttribute());

            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
        }

        public override void Dispose()
        {
        }
    }
}