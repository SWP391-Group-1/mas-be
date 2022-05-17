using System.ComponentModel.DataAnnotations;

namespace MAS.Core.Dtos.Incoming.Account
{
    public class GoogleLoginRequest
    {
        [Required]
        public string ProviderName { get; set; }

        [Required]
        public string IdToken { get; set; }
    }
}
