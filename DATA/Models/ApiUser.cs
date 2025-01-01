namespace DATA.Models;

public partial class ApiUser
{
    public int Id { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string ApiScope { get; set; }
}
