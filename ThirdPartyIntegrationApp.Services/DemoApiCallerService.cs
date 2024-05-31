using ThirdPartyIntegrationApp.Interfaces;

namespace ThirdPartyIntegrationApp.Services
{
    public class DemoApiCallerService : ApiCallerService, IDemoApiCallerService
    {
        public DemoApiCallerService(HttpClient httpClient)
            : base(httpClient)
        {
        }
    }
}
