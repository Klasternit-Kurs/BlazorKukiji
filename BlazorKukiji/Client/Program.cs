using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BlazorKukiji.Client.Servisi;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorKukiji.Client
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);
			builder.RootComponents.Add<App>("#app");

			builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

			builder.Services.AddOptions();
			builder.Services.AddAuthorizationCore();
			builder.Services.AddScoped<IAuthServis, AuthServis>();
			builder.Services.AddScoped<AuthProvajder>();
			builder.Services.AddScoped<AuthenticationStateProvider>(s =>
				s.GetRequiredService<AuthProvajder>());

			builder.Services.AddSingleton(s =>
			{
				string serv = s.GetRequiredService<NavigationManager>().BaseUri;
				var hKli = new HttpClient(new GrpcWebHandler
					(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
				var k = GrpcChannel.ForAddress(serv, new GrpcChannelOptions { HttpClient = hKli });
				return new grpcAuth.GrpcA.GrpcAClient(k);
			});

			await builder.Build().RunAsync();
		}
	}
}
