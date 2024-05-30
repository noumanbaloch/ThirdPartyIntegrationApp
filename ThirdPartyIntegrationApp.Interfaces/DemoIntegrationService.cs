using ThirdPartyIntegrationApp.Services;

namespace ThirdPartyIntegrationApp.Interfaces
{
    public class DemoIntegrationService : IDemoIntegrationService
    {
        private readonly IDemoApiCallerService _apiCallerService;

        public DemoIntegrationService(IDemoApiCallerService apiCallerService)
        {
            _apiCallerService = apiCallerService;
        }
    }

}
