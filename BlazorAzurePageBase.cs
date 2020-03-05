// using BlazorLibrary.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace blazor.wa.aadauth.sample
{
    public abstract class BlazorAzurePageBase : ComponentBase
    {
        // [Inject]
        // protected IUriHelper UriHelper { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected IClient client { get; set; }
    }
}