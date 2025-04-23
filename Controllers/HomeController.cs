using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using QrCodeWebAppFreemium.Models;
using QRCoder;
using System.Drawing;
using Microsoft.EntityFrameworkCore;

namespace QrCodeWebAppFreemium.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Generate(string content)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user.QrGenerationCount >= 3)
            {
                return RedirectToAction("Payment");
            }

            using var generator = new QRCodeGenerator();
            var data = generator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new PngByteQRCode(data);
            var qrBytes = qrCode.GetGraphic(20, "#000000", "#FFFFFF");

            user.QrGenerationCount++;
            await _context.SaveChangesAsync();

            string imageBase64 = Convert.ToBase64String(qrBytes);
            ViewBag.QrImage = $"data:image/png;base64,{imageBase64}";
            return View("Index");
        }

        [HttpGet]
        public IActionResult Payment()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProcessPayment()
        {
            // Fake payment processing, in reality, you would integrate a payment gateway here.
            return RedirectToAction("Index");
        }
    }
}