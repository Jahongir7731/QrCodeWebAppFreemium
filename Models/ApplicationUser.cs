using Microsoft.AspNetCore.Identity;

namespace QrCodeWebAppFreemium.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int QrGenerationCount { get; set; } = 0;
    }
}