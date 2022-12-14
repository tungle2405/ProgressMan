using Blazored.LocalStorage;
using ProgressMan.AuthProviders;
using ProgressMan.HttpRepository;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProgressMan
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");

			builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:5011/api/") });
			builder.Services.AddScoped<IProductHttpRepository, ProductHttpRepository>();
			builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
			builder.Services.AddBlazoredLocalStorage(); 
			builder.Services.AddAuthorizationCore(); 
			builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

			await builder.Build().RunAsync();
		}
	}
}
