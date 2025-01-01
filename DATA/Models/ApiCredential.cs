using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DATA.Models
{
    [Table("api_credentials")]
    public class ApiCredential
    {
        [Key]
        public long Id { get; set; }

        public int ClientType { get; set; }

        public int CredentialType { get; set; }

        public string? CredentialTypeName { get; set; }

        public string FieldValue { get; set; } = null!;
    }

}
