using Microsoft.AspNetCore.Server.Kestrel.Core;
using OpenRepairManager.PrintServer.Services;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseKestrel(options =>
{
    options.Listen(IPAddress.Any, 5000, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});
// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();
// Configure the HTTP request pipeline.
app.MapGrpcService<PrintItemService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

Console.WriteLine(@"  ____                     _____                  _        __  __                                   
 / __ \                   |  __ \                (_)      |  \/  |                                  
| |  | |_ __   ___ _ __   | |__) |___ _ __   __ _ _ _ __  | \  / | __ _ _ __   __ _  __ _  ___ _ __ 
| |  | | '_ \ / _ \ '_ \  |  _  // _ \ '_ \ / _` | | '__| | |\/| |/ _` | '_ \ / _` |/ _` |/ _ \ '__|
| |__| | |_) |  __/ | | | | | \ \  __/ |_) | (_| | | |    | |  | | (_| | | | | (_| | (_| |  __/ |   
 \____/| .__/ \___|_| |_| |_|  \_\___| .__/ \__,_|_|_|    |_|  |_|\__,_|_| |_|\__,_|\__, |\___|_|   
       | |                           | |                                             __/ |          
       |_|                           |_|                                            |___/           
");
Console.WriteLine(@" _           __            
|_)._o.__|_ (_  _ ._  _ ._ 
|  | || ||_ __)(/_|\/(/_|   
                             ");

Console.WriteLine("Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
Console.WriteLine($"© 2021-{DateTime.Now.Year} Mitch Hawks");
Console.WriteLine("Licensed under the MIT License");
Console.WriteLine();

Console.WriteLine("Starting server...");
var host = Dns.GetHostEntry(Dns.GetHostName());
Console.WriteLine("Server IP address is");
foreach (var ip in host.AddressList)
{
    if (ip.AddressFamily == AddressFamily.InterNetwork)
    {
        Console.WriteLine(ip.ToString());
        //app.Urls.Add("http://" + ip.ToString() + ":5000");
    }
}
Console.WriteLine("Enter the IP address above on each computer.");

Console.WriteLine();

app.Run();
