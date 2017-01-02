using System;
using System.Linq;
using System.Threading.Tasks;
using NSmpp;

namespace TestClient
{
    class Program
    {
        private const string FromNumber = "1234567890";

        static void Main(string[] args)
        {
            try
            {
                using (var client = new SmppClient())
                {

                    client.Connect("localhost", 2775).GetAwaiter().GetResult();
                    client.Bind(BindType.Transmitter, "smppclient1", "password").GetAwaiter().GetResult();
                    Console.WriteLine("Session bound. Press ENTER to start sending.");
                    Console.ReadKey();

                    var tasks = Enumerable.Range(0, 100).Select(i => SendMessage(client, i));
                    var ids = Task.WhenAll(tasks).Result;
                    Console.WriteLine("Sending complete. Press ENTER to start queries.");
                    Console.ReadKey();

                    var queryTasks = ids.Select(i => QueryMessage(client, i));
                    Task.WhenAll(queryTasks).GetAwaiter().GetResult();
                    Console.WriteLine("Querying complete. Press ENTER to start cancellations.");
                    Console.ReadKey();

                    var cancelTasks = ids.Select((id, i)  => CancelMessage(client, id, i));
                    Task.WhenAll(cancelTasks).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ruh Roh!");
                Console.WriteLine(ex);
            }
            Console.WriteLine("Done. Press ENTER to quit.");
            Console.ReadKey(true);
        }

        public static async Task<string> SendMessage(SmppClient client, int i)
        {
            var number = i.ToString("0000000000");
            var message = $"This is test message #{i}";
            Console.WriteLine($"Submitting message {i}...");
            var result = await client.Submit(FromNumber, number, message);
            Console.WriteLine($"Message {i} submitted with message ID {result.MessageId}.");
            return result.MessageId;
        }

        public static async Task QueryMessage(SmppClient client, string id)
        {
            Console.WriteLine($"Querying message {id}.");
            var result = await client.Query(id, FromNumber);
            Console.WriteLine($"Status for message {id} is {result.State}, completed at {result.FinalDate}.");
        }

        private static async Task CancelMessage(SmppClient client, string id, int index)
        {
            var toNumber = index.ToString("0000000000");
            Console.WriteLine($"Cancelling message {id}.");
            await client.Cancel(id, FromNumber, toNumber);
            Console.WriteLine($"Cancelled message {id}.");
        }
    }
}
