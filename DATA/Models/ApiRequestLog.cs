namespace DATA.Models;

public partial class ApiRequestLog
{
    public long Id { get; set; }

    public string? FunctionName { get; set; }

    public string? ModuleName { get; set; }

    public string? RequestUrl { get; set; }

    public string? Scope { get; set; }

    public string? RequestBody { get; set; }

    public string? ResponseBody { get; set; }

    public string? Headers { get; set; }

    public string? Ip { get; set; }

    public string? ResponseCode { get; set; }

    public decimal? Longitude { get; set; }

    public decimal? Latitude { get; set; }

    public string? RequestMethod { get; set; }

    public string? OrderId { get; set; }

    public string? UserId { get; set; }

    public DateTime? RequestStartTime { get; set; }

    public DateTime? RequestEndTime { get; set; }

    public string? AppVersion { get; set; }

    public string? DeviceId { get; set; }

    public string? DeviceOs { get; set; }

    public string? DeviceModel { get; set; }

    public DateTime? CreatedAt { get; set; }
}
public class ApiSuccessResponse
{
    public int code { get; set; }
    public List<string> messages { get; set; }
    public object data { get; set; }
    public string details { get; set; }
}
public class ErrorResponseFromCashBaba
{
    public string ErrorMessages { get; set; }
}
public class ResponseFromCashBaba
{
    public string TransactionId { get; set; }
}


