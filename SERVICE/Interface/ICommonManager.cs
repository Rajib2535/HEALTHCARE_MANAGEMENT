using DATA.Models.RequestReponseModels;
using UTILITY;

namespace SERVICE.Interface
{
    public interface ICommonManager
    {
        Task<ResponseModel> GetExternalAPIResponse(ExternalAPIRequestModel externalAPIRequestViewModel);
    }
}
