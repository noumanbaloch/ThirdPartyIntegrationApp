using ThirdPartyIntegrationApp.Interfaces;

namespace ThirdPartyIntegrationApp.Services
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
