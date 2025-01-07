
using CORPORATE_DISBURSEMENT_UTILITY;
using DATA.Models.RequestReponseModels;
using DATA.Models.ViewModels;
using UTILITY;

namespace COMMON_SERVICE.Interface
{
    public interface ICommonService
    {
        Task<CommonResponse> HandleResponse(int httpStatusCode, CommonEnum.API_Response_Codes code, CommonResponse responseEntity, string tag = "", string custom_message = "");
        Task<List<DropdownViewModel>> GetRefundRequestStatusDropdown();
    }
}
