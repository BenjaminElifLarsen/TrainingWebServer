using Server;
using System;

namespace Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            WebServer server = new();
            server.Open("127.0.0.1", 8080);
        }
    }
}
