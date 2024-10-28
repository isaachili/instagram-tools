using IsaacHili.InstagramTools.App;
using IsaacHili.InstagramTools.Followers.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add services
_ = builder.Services.AddScoped(sp => 
	new HttpClient 
	{
		BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
	});

_ = builder.Services.AddFollowers();

var host = builder.Build();
await host.RunAsync();
