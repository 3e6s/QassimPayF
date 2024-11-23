using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QassimPay.Models;
using QassimPay.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace QassimPay.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.User.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);
                return RedirectToAction("Wallet");
            }

            ModelState.AddModelError("", "Wrong username or password");
            return View();
        }

        public IActionResult Wallet()
        {
            var username = HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToAction("Login");
            }

            var user = _context.User.FirstOrDefault(u => u.Username == username);
            var wallet = _context.Wallet.FirstOrDefault(w => w.User_ID == user.ID);

            ViewBag.Username = username;
            ViewBag.Wallet = wallet;

            return View();
        }
        [HttpPost]
        [HttpPost]
        public IActionResult CreateWallet(decimal balance)
        {
            var username = HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToAction("Login");
            }

            var user = _context.User.FirstOrDefault(u => u.Username == username);

            if (!_context.Wallet.Any(w => w.User_ID == user.ID))
            {
                var wallet = new WalletModel
                {
                    User_ID = user.ID,
                    Balance = balance
                };
                _context.Wallet.Add(wallet);
                _context.SaveChanges();
            }

            return RedirectToAction("Wallet");
        }

        [HttpGet]
        public IActionResult Transfer()
        {
            var username = HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Transfer(int recipientWalletId, decimal amount)
        {
            var username = HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToAction("Login");
            }

            var sender = _context.User.FirstOrDefault(u => u.Username == username);
            var senderWallet = _context.Wallet.FirstOrDefault(w => w.User_ID == sender.ID);
            var recipientWallet = _context.Wallet.FirstOrDefault(w => w.Wallet_ID == recipientWalletId);

            if (recipientWallet == null)
            {
                ViewBag.ErrorMessage = "This wallet does not exist.";
                return View();
            }

            var recipientUser = _context.User.FirstOrDefault(u => u.ID == recipientWallet.User_ID);
            if (senderWallet == null)
            {
                ViewBag.ErrorMessage = "You do not have a wallet.";
                return View();
            }

            if (senderWallet.Balance < amount)
            {
                ViewBag.ErrorMessage = "Insufficient balance.";
                return View();
            }
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                senderWallet.Balance -= amount;
                recipientWallet.Balance += amount;
                _context.SaveChanges();

                _context.Database.ExecuteSqlRaw(
                    "INSERT INTO \"Transfer\" (\"Sender_ID\", \"Reciver\", \"AmountM\") VALUES ({0}, {1}, {2})",
                    senderWallet.Wallet_ID, recipientUser.ID, amount);

                transaction.Commit();
                ViewBag.SuccessMessage = "Transfer successful.";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                ViewBag.ErrorMessage = ex;
                // Log the exception
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserModel user)
        {
            if (_context.User.Any(u => u.Username == user.Username))
            {
                ModelState.AddModelError("Username", "Username already exists.");
            }

            if (string.IsNullOrWhiteSpace(user.First_name) ||
                string.IsNullOrWhiteSpace(user.Last_name) ||
                string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.Password) ||
                string.IsNullOrWhiteSpace(user.Username) ||
                user.Monthly_income <= 0)
            {
                ModelState.AddModelError("", "All fields are required.");
            }

            if (user.Password?.Length < 8)
            {
                ModelState.AddModelError("Password", "Password must be at least 8 characters long.");
            }

            if (ModelState.IsValid)
            {
                _context.User.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(user);
        }
    }
}