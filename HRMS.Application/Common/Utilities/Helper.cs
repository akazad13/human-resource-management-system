using HRMS.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace HRMS.Application.Common.Utilities
{
    public class Helper : IHelper
    {
        private readonly ConfigModel _configModel;
        private readonly IIdentityService _identityService;
        public Helper(
            IOptions<ConfigModel> configModel,
            IIdentityService identityService
        )
        {
            _configModel = configModel.Value;
            _identityService = identityService;
        }
    }
}
