using System;
using System.Diagnostics;
using System.Globalization;
using System.Net.Sockets;
using System.Threading;

namespace AutomatedTester.BrowserMob
{
    public class Server
    {
        private Process _serverProcess;
        private readonly int _port;
        private readonly String _path = string.Empty;
        private const string Host = "localhost";

        public Server(string path): this(path, 8444)
        {}

        public Server(string path, int port)
        {
            _path = path;
            _port = port;
        }

        public string Start()
        {
            _serverProcess = new Process
                                 {
                                     StartInfo = {FileName = _path}
                                 };
            if (_port != 0)
            {
                _serverProcess.StartInfo.Arguments = String.Format("--port={0}", _port);
            }
            
            try
            {
                if (IsListening())
                {
                    throw new Exception("BrowserMob Proxy Already running");
                    //var runningProcesses = Process.GetProcesses();
                    //// now check the modules of the process
                    //foreach (var process in runningProcesses)
                    //{
                    //    Debug.WriteLine(String.Format("Process: {0} ID: {1}", process.ProcessName, process.Id));
                    //    if (process.ProcessName.Equals("cmd"))
                    //    {
                    //        //Debug.WriteLine(process.MainModule.FileName);
                    //        foreach (ProcessModule module in process.Modules)
                    //        {
                    //            Debug.WriteLine(module.FileName);
                    //            Thread.Sleep(10);
                    //        }
                    //        Thread.Sleep(10);
                    //    }
                    //    //// now check the modules of the process
                    //    //foreach (ProcessModule module in process.Modules)
                    //    //{
                    //    //    Debug.WriteLine(module.FileName);
                    //    //    if (module.FileName.Contains("BrowserMob"))
                    //    //    {
                    //    //        process.Kill();
                    //    //    }
                    //    //}
                    //}
                    //Thread.Sleep(1000);
                }
                _serverProcess.Start();
                var count = 0;
                while (!IsListening())
                {
                    Thread.Sleep(1000);
                    count++;
                    if (count == 30)
                    {
                        throw new Exception("Can not connect to BrowserMob Proxy");
                    }
                }
            }
            catch(Exception e)
            {
                return("Exception: " + e.Message);
                _serverProcess.Dispose();
                _serverProcess = null;
            }
            return "Success from Starting Server";
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {            
            if (_serverProcess != null && !_serverProcess.HasExited)
            {
                _serverProcess.CloseMainWindow();
                _serverProcess.Dispose();                
                _serverProcess = null;
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Client CreateProxy(){
            return new Client(Url);
        }

        /// <summary>
        /// 
        /// </summary>
        public string Url
        {
            get { return String.Format("http://{0}:{1}", Host, _port.ToString(CultureInfo.InvariantCulture)); }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool IsListening()
        {
            try
            {
                var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(Host, _port);
                socket.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }            
        }
    }
}
