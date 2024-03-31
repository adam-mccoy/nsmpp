using NSmpp;

namespace TestClient;

public class Program
{
    private const string FromNumber = "1234567890";

    static async Task Main(string[] args)
    {
        try
        {
            await using var client = new SmppClient();
            client.DeliverReceived += Client_DeliverReceived;
            await client.Connect("localhost", 2775);
            await client.Bind(BindType.Transceiver, "smppclient1", "password");
            Console.WriteLine("Session bound. Press ENTER to start sending.");
            Console.ReadKey();

            var tasks = Enumerable.Range(0, 100).Select(i => SendMessage(client, i));
            var ids = await Task.WhenAll(tasks);
            Console.WriteLine("Sending complete. Press ENTER to start queries.");
            Console.ReadKey();

            var queryTasks = ids.Select(i => QueryMessage(client, i));
            await Task.WhenAll(queryTasks);
            Console.WriteLine("Querying complete. Press ENTER to start cancellations.");
            Console.ReadKey();

            var cancelTasks = ids.Select((id, i)  => CancelMessage(client, id, i));
            await Task.WhenAll(cancelTasks);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ruh Roh!");
            Console.WriteLine(ex);
        }
        Console.WriteLine("Done. Press ENTER to quit.");
        Console.ReadKey(true);
    }

    private static void Client_DeliverReceived(object? sender, DeliverReceivedEventArgs e)
    {
        Console.WriteLine("Deliver received - Message = {0}", e.ShortMessage);
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