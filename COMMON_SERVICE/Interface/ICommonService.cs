using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.RequestReponseModels;
using CORPORATE_DISBURSEMENT_ADMIN_DAL.Models.ViewModels;
using CORPORATE_DISBURSEMENT_UTILITY;

namespace COMMON_SERVICE.Interface
{
    public interface ICommonService
    {
        Task<CommonResponse> HandleResponse(int httpStatusCode, CommonEnum.API_Response_Codes code, CommonResponse responseEntity, string tag = "", string custom_message = "");
        Task<List<DropdownViewModel>> GetRefundRequestStatusDropdown();
    }
}
