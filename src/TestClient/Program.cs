using System;
using NSmpp;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var client = new SmppClient())
                {

                    client.Connect("localhost", 2775).Wait();
                    client.Bind(BindType.Transmitter, "smppclient1", "password").Wait();
                    Console.WriteLine("Bound. Press ENTER to quit.");
                    Console.ReadKey(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ruh Roh!");
                Console.WriteLine(ex);
            }
        }
    }
}
