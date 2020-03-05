using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Mono.WebAssembly.Interop;
using Microsoft.JSInterop;
using Serilog;

namespace blazor.wa.aadauth.sample
{
    public class MainModel : BlazorAzurePageBase
    {
        protected override async Task OnParametersSetAsync()
        {
            await DoStuff();
        }

        public async Task DoStuff()
        {
            // the first call always returns null for some odd reason
            await JSRuntime.InvokeAsync<string>("blazorInterop.void", null);
            
            if(!(await JSRuntime.InvokeAsync<bool>("blazorInterop.isLoggedIn", null)))
            {
                Log.Information($"Not logged in yet");
                return;
            }
            var UserName = await JSRuntime.InvokeAsync<string>("blazorInterop.getUserName", null);
            var TenantId = await JSRuntime.InvokeAsync<string>("blazorInterop.getTenantId", null);
            var Hostname = await JSRuntime.InvokeAsync<string>("blazorInterop.getHostname", null);

            Log.Information($"Hostname '{Hostname}', UserName '{UserName}', TenantId '{TenantId}'");
            client.UserName = UserName;
            client.TenantId = TenantId;
            client.Hostname = Hostname;            
            
            Action<string> action = async (token) =>
            {
                if(token == null)
                {
                    Log.Information($"Token is null, not logged in yet...");
                }
                client.Token = token;
                try
                {
                    Log.Information("Connecting...");
                    await client.DoWork();
                }
                catch (System.Exception e)
                {
                    Log.Error($"Failed to do stuff, error was {e.Message}");
                }
                
            };
            
            if(((MonoWebAssemblyJSRuntime)JSRuntime).InvokeUnmarshalled<Action<string>, bool>("blazorInterop.executeWithToken", action))
            {
                StateHasChanged();
            }
        }        
        public async Task Login()
        {
            var RedirectPath = "/loggedin";
            object[] args = new object[]{RedirectPath};
            await JSRuntime.InvokeAsync<object>("blazorInterop.login", args);
        }
        public bool IsLoggedIn()
        {
            return ((IJSInProcessRuntime)JSRuntime).Invoke<bool>("blazorInterop.isLoggedIn", null);
        }
    }
}