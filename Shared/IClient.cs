using System.Collections.Generic;
using System.Threading.Tasks;

namespace blazor.wa.aadauth.sample
{
    public interface IClient
    {
        Task<List<Class1>> DoWork();
        string Token { get; set; }
        string UserName { get; set; }
        string TenantId { get; set; }
        string Hostname { get; set; }
    }
}