using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.RequestReponseModels;
using CORPORATE_DISBURSEMENT_UTILITY;

namespace SERVICE.Interface
{
    public interface ICommonManager
    {
        Task<ResponseModel> GetExternalAPIResponse(ExternalAPIRequestModel externalAPIRequestViewModel);
    }
}
