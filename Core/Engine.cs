using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SharpAdbClient;

namespace YueDroidBox.Core
{
    public class Engine : IDisposable
    {
        private static readonly Engine instance = new Engine();
        readonly AdbServer _server = new AdbServer();
        private bool _isEngineRunning = true;

        public static Engine Instance()
        {
            return instance;
        }

        private Engine()
        {
            StartServer(); 
            Task.Run(ServerWatchDog);
        }

        public void StartServer()
        {
            var result = _server.StartServer(GetAdbPath(), false);
            //return result;
        }

        public void ServerWatchDog()
        {
            while (_isEngineRunning)
            {
                if (!_server.GetStatus().IsRunning)
                {
                    StartServer();
                }
                Thread.Sleep(1000);
            }
        }

        public List<DeviceData> GetDevices()
        {
            var devices = AdbClient.Instance.GetDevices();

            foreach (var device in devices)
            {
                Console.WriteLine(device.Name);
            }

            return devices;
        }

        public DeviceMonitor StartMonitor()
        {
            var monitor = new DeviceMonitor(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)));
            monitor.Start();
            return monitor;
        }

        private string GetAdbPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Adb", "adb.exe");
        }

        public void Dispose()
        {
            _isEngineRunning = false;
        }
    }
}