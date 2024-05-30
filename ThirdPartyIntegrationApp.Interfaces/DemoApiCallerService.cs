using ThirdPartyIntegrationApp.Services;

namespace ThirdPartyIntegrationApp.Interfaces
{
    public class DemoApiCallerService : ApiCallerService, IDemoApiCallerService
    {
        public DemoApiCallerService(HttpClient httpClient)
            : base(httpClient)
        {
        }
    }
}
