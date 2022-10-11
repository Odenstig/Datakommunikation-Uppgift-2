using System.IO.Pipes;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Uppgift2.Sender;

//Listener

//Creates new endpoint with port 5006
var endpoint = new IPEndPoint(IPAddress.Loopback, 5006);
//Creates new UdpClient
using var client = new UdpClient(endpoint);

//Receive Object
Console.WriteLine($"Listening on port: {endpoint.Port}..");
Console.Write("\n");
var result = await client.ReceiveAsync();

//Deserialize Object
var jsonDes = JsonSerializer.Deserialize<Text>(result.Buffer);

//Writes out object
Console.WriteLine("Received json object {0} from {1}", Encoding.UTF8.GetString(result.Buffer), result.RemoteEndPoint.Port);
Console.WriteLine("Deserialized message: {0}", jsonDes.message);
Console.Write("\n");

//Creates response object
Text message = new();
message.message = "Hello there sender! Thank you for your message! :-)";

//Serializes object
var jsonSer = JsonSerializer.Serialize(message);

Console.WriteLine($"Responding with json object: {jsonSer} to port 5005");

//Sends response
await client.SendAsync(Encoding.UTF8.GetBytes(jsonSer),
    result.RemoteEndPoint);

//Awaits potential response
await client.ReceiveAsync();