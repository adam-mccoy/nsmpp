using System;
using NSmpp;
using NSmpp.Pdu;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var client = new SmppClient();
                client.Connect("localhost", 2775).Wait();
                client.Bind(BindType.Transmitter, "smppclient1", "password").Wait();
                Console.WriteLine("Done!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ruh Roh!");
                Console.WriteLine(ex);
            }
        }
    }
}
