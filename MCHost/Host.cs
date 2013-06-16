using System;
using System.Configuration;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;

namespace MCHost
{
    /// <summary>
    /// Host that handles our minecraft process and facilitates communication
    /// to/from minecraft.
    /// </summary>
    public class Host : IDisposable
    {
        private Process _hostProcess;
        //private Process _clientProcess;

        public event EventHandler<bool> DecisionMade;

        /// <summary>
        /// This sets up the minecraft server to run in the background and
        /// redirects stdout and stderr to methods in this class so we can
        /// act on them.
        /// </summary>
        public Host()
        {
            var pi = new ProcessStartInfo();
            pi.FileName = ConfigurationManager.AppSettings["JavaPath"];
            pi.Arguments = ConfigurationManager.AppSettings["MinecraftArguments"];
            pi.RedirectStandardError = true;
            pi.RedirectStandardInput = true;
            pi.RedirectStandardOutput = true;
            pi.UseShellExecute = false;
            pi.WorkingDirectory = ConfigurationManager.AppSettings["WorkingFolder"];
            
            _hostProcess = Process.Start(pi);
            _hostProcess.OutputDataReceived += _process_OutputDataReceived;
            _hostProcess.BeginOutputReadLine();

            _hostProcess.ErrorDataReceived += _process_ErrorDataReceived;
            _hostProcess.BeginErrorReadLine();

            Thread.Sleep(2000);

            //pi = new ProcessStartInfo();
            //pi.FileName = ConfigurationManager.AppSettings["MinecraftExePath"];
            //pi.Arguments = ConfigurationManager.AppSettings["MinecraftExeArguments"];
            //pi.UseShellExecute = true;
            //pi.LoadUserProfile = true;

            //_clientProcess = Process.Start(pi);
        }

        public int HostProcessId { get { return _hostProcess.Id; } }
        //public int ClientProcessId { get { return _clientProcess.Id; } }

        public bool HasExited { get { return _hostProcess == null || _hostProcess.HasExited; } }

        /// <summary>
        /// The minecraft .jar seems to send it's "normal" output to STDERR.  Kindof
        /// a WTF in itself!  Watch the output for STEVEBOT responding and send the
        /// corresponding response back to the caller.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
            if (e.Data.Contains("[@] STEVEBOT says"))
            {
                Regex re = new Regex("\\[\\@\\] STEVEBOT says \\\"(.*)\\\"$");
                var match = re.Match(e.Data);
                if (match.Success)
                {
                    switch (match.Groups[1].Value.ToLowerInvariant())
                    {
                        case "yes":
                            SendResponse(true);
                            break;
                        case "no":
                            SendResponse(false);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Sends the input signal to minecraft.  The daylight sensor is signaled when we set the
        /// time to morning (/time set 0).
        /// </summary>
        public void BeginDecision()
        {
            _hostProcess.StandardInput.Write("/say Oh great STEVEBOT, please tell us the answer...\n");
            _hostProcess.StandardInput.Write("/time set 0\n");
        }

        /// <summary>
        /// Sends a true or false back to the caller.
        /// </summary>
        /// <param name="response"></param>
        private void SendResponse(bool response)
        {
            if (DecisionMade != null)
                DecisionMade(null, response);
        }

        /// <summary>
        /// Some setup information is printed to stdout.  Not sure why it's not all in stdout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        /// <summary>
        /// Clean up our minecraft process.
        /// </summary>
        public void Dispose()
        {
            //if (_clientProcess != null)
            //{
            //    _clientProcess.Kill();
            //    _clientProcess = null;
            //}

            if (_hostProcess != null)
            {
                _hostProcess.StandardInput.Write("/stop\n");
                _hostProcess.OutputDataReceived -= _process_OutputDataReceived;
                _hostProcess.ErrorDataReceived -= _process_ErrorDataReceived;

                Thread.Sleep(4000);
                _hostProcess.Kill();

                _hostProcess = null;
            }
        }
    }
}
