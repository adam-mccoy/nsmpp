using System;
using System.Linq;
using System.Threading.Tasks;
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
                    Console.WriteLine("Session bound. Press ENTER to start sending.");
                    Console.ReadKey();

                    var tasks = Enumerable.Range(0, 100).Select(i => SendMessage(client, i));
                    Task.WhenAll(tasks).Wait();

                    Console.WriteLine("Done. Press ENTER to quit.");
                    Console.ReadKey(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ruh Roh!");
                Console.WriteLine(ex);
            }
        }

        public static async Task SendMessage(SmppClient client, int i)
        {
            var number = i.ToString("0000000000");
            var message = $"This is test message #{i}";
            Console.WriteLine($"Submitting message {i}...");
            var result = await client.Submit(number, number, message);
            Console.WriteLine($"Message {i} submitted with message ID {result.MessageId}.");
        }
    }
}
