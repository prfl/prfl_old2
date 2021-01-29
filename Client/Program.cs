using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Profile.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Profile.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");        

            builder.Services.AddSingleton<StateContainer>();
            builder.Services.AddSingleton<PageContainer>();

            builder.Services.AddScoped<AuthenticationState>();

            builder.Services.AddHttpClient("Profile.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Profile.ServerAPI"));

            builder.Services.AddHttpClient("ServerAPI.NoAuthenticationClient", 
                    client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddApiAuthorization();

            await builder.Build().RunAsync();
        }
    }
}
