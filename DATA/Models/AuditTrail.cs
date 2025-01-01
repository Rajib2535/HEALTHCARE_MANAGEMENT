namespace DATA.Models;

public partial class AuditTrail
{
    public long Id { get; set; }

    public int? UserId { get; set; }

    public string? Summary { get; set; }

    public string? Purpose { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Remarks { get; set; }
}
