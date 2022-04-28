using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmashMall.Client.Services;
using SmashMall.Shared.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmashMall.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient("SmashMall.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("SmashMall.ServerAPI"));

            // Supply HttpClient instances that don't include access tokens when making requests to the server project
            builder.Services.AddHttpClient<UnAuthorizedHttpClient>(client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

            builder.Services.AddApiAuthorization(options => options.AuthenticationPaths.LogOutSucceededPath = "").AddAccountClaimsPrincipalFactory<CustomUserFactory>();

            builder.Services.AddSingleton<StateContainer>();
            builder.Services.AddBlazoredLocalStorage();

            BaseApi.Url = builder.HostEnvironment.BaseAddress;
            await builder.Build().RunAsync();
        }
    }
}
