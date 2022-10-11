using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Uppgift2.Sender;

//Sender

//Creates new endpoint with port 5005
var endpoint = new IPEndPoint(IPAddress.Loopback, 5005);
//Creates new UdpClient
using var client = new UdpClient(endpoint);

//Create & Serialize Object
Text message = new();
message.message = "Hello there listener!";
var jsonSer = JsonSerializer.Serialize(message);

//Encodes json object into bytes
byte[] bytes = Encoding.UTF8.GetBytes(jsonSer);

//Sends encoded object to listener
Console.WriteLine($"Sending json object: {jsonSer} to 5006");
Console.Write("\n");
await client.SendAsync(bytes, new IPEndPoint(IPAddress.Loopback, 5006), CancellationToken.None);

//Recieves response
var result = await client.ReceiveAsync();

//Deserializes response
var jsonDes = JsonSerializer.Deserialize<Text>(result.Buffer);

//Writes out response
Console.WriteLine("Recieved json object: {0} from {1}", Encoding.UTF8.GetString(result.Buffer), result.RemoteEndPoint.Port);
Console.WriteLine("Deserialized message: {0}", jsonDes.message);

//Awaits potential response
await client.ReceiveAsync();
