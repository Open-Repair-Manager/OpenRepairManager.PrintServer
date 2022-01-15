using RepairCafeBassendeanSessionServer.Services;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<PrintItemService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

Console.WriteLine(@" ____  _____ ____   _    ___ ____     ____    _    _____ _____ 
|  _ \| ____|  _ \ / \  |_ _|  _ \   / ___|  / \  |  ___| ____|
| |_) |  _| | |_) / _ \  | || |_) | | |     / _ \ | |_  |  _|  
|  _ <| |___|  __/ ___ \ | ||  _ <  | |___ / ___ \|  _| | |___ 
|_| \_\_____|_| /_/   \_\___|_| \_\  \____/_/   \_\_|   |_____|");
Console.WriteLine(@" _                           
|_) _. _ _ _ ._  _| _  _.._  
|_)(_|_>_>(/_| |(_|(/_(_|| | 
                             ");

Console.WriteLine("Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
Console.WriteLine();

Console.WriteLine("Starting server...");
var host = Dns.GetHostEntry(Dns.GetHostName());
Console.WriteLine("Server IP address is");
foreach (var ip in host.AddressList)
{
    if (ip.AddressFamily == AddressFamily.InterNetwork)
    {
        Console.WriteLine(ip.ToString());
        app.Urls.Add("http://" + ip.ToString() + ":5000");
    }
}
Console.WriteLine("Enter the IP address above on each computer.");

Console.WriteLine();

app.Run();
