using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    public class WebServer
    {
        private TcpListener _receiver;
       // private List<Thread> _clientThreads;


        public void Open(string ipAddress, int port)
        {
            ReceiveSetup(ipAddress, port);
            //_clientThreads = new();
            Run();
        }

        private void Run()
        {
            List<Thread> _clientThreads = new List<Thread>();
            _receiver.Start();
            Debug.WriteLine("Started");
            while (true)
            {
                while (!_receiver.Pending())
                {
                    try
                    {
                        for (int i = _clientThreads.Count - 1; i >= 0; i--)
                            if (!_clientThreads[i].IsAlive)
                                _clientThreads.RemoveAt(_clientThreads.Count - 1);

                    }
                    catch (Exception)
                    {

                    }
                }
                Debug.WriteLine("Clinet Connection");
                _clientThreads.Add(new Thread(new ThreadStart(Handle)));
                _clientThreads[_clientThreads.Count - 1].Name = $"Client Thread {_clientThreads.Count - 1}";
                _clientThreads[_clientThreads.Count - 1].Start();
            }
        }

        private void Handle()
        {
                TcpClient client = null;
                NetworkStream stream = null;
            try
            {
                client = _receiver.AcceptTcpClient();
                stream = client.GetStream();
                List<byte> bytes = new();
                while (stream.DataAvailable)
                {
                    bytes.Add((byte)stream.ReadByte());
                }
                string request = System.Text.Encoding.UTF8.GetString(bytes.ToArray(), 0, bytes.Count);
                Debug.WriteLine(request);

                string path = request.Split("HTTP")[0].Split("/")[1].Trim();

                string content = "";

                if (path == "")
                {
                    Pages page = new();
                    content = page.Index();
                    content = $@"HTTP/1.1 200 OK
                    Date: Mon, 27 Jul 2009 12:28:53 GMT
                    Server: Apache/2.2.14 (Win32)
                    Last-Modified: Wed, 22 Jul 2009 19:15:56 GMT
                    Content-Length: {content.Length}
                    Content-Type: text/html
                    Connection: Closed

                    " + content;
                }
                else if (path.ToLower() == nameof(Pages.Test).ToLower()) 
                { // Could use reflection, if the found method is null return NotFound()
                    Pages page = new();
                    content = page.Test();
                    content = $@"HTTP/1.1 200 OK
                    Date: Mon, 27 Jul 2009 12:28:53 GMT
                    Server: Apache/2.2.14 (Win32)
                    Last-Modified: Wed, 22 Jul 2009 19:15:56 GMT
                    Content-Length: {content.Length}
                    Content-Type: text/html
                    Connection: Closed

                    " + content;
                }
                else
                {
                    content = $@"HTTP/1.1 404 NotFound
                    Date: Mon, 27 Jul 2009 12:28:53 GMT
                    Server: Apache/2.2.14 (Win32)
                    Last-Modified: Wed, 22 Jul 2009 19:15:56 GMT
                    Content-Length: 0
                    Content-Type: text/html
                    Connection: Closed

                    ";

                }

                stream.Write(System.Text.Encoding.UTF8.GetBytes(content));
                stream.Flush();
                client.Close();
            }
            catch (Exception e)
            {
                if (stream != null)
                    stream.Close();
                if(client != null)
                {
                    client.Close();
                }
                Debug.WriteLine(e.Message);
            }
        }

        private bool ReceiveSetup(string IPaddress, int portNumber = 23000)
        {
            try
            { //tries to start up the receiver. 
                int port = portNumber;
                IPAddress receiverAddress = IPAddress.Parse(IPaddress);
                _receiver = new TcpListener(receiverAddress, port);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
