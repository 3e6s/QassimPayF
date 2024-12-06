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
        [HttpGet]
        public IActionResult Transfer_History()
        {
            var username = HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToAction("Login");
            }

            // Retrieve the user by username
            var user = _context.User.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var userWallet = _context.Wallet.FirstOrDefault(w => w.User_ID == user.ID);
            if (userWallet == null)
            {
                return NotFound("Wallet not found.");
            }

            var userWalletId = userWallet.Wallet_ID;

            // Fetch transfer records where the user is the sender or receiver
            var transferRecords = _context.Transfer
                .Where(t => t.Sender_ID == userWalletId || t.Reciver == user.ID)
                .ToList();

            // Pass the required data to the view
            ViewBag.User = user; // Pass the user object to the view
            ViewBag.UserWalletID = userWalletId; // Pass the wallet ID for the sender comparison
            return View(transferRecords);
        }
        public IActionResult Billing()
        {
            var username = HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToAction("Login");
            }

            return View();
        }
        [HttpPost]
        public IActionResult Billing(string billingNumber, decimal amount)
        {
            // Retrieve the logged-in user's username from the session
            var username = HttpContext.Session.GetString("Username");


            // Find the user by username
            // Find the user by username
            var user = _context.User.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                ViewBag.ErrorMessage = "User not found.";
                return View();
            }

            // Find the user's wallet (assuming one wallet per user)
            var userWallet = _context.Wallet.FirstOrDefault(w => w.User_ID == user.ID);
            if (userWallet == null)
            {
                ViewBag.ErrorMessage = "You do not have a wallet.";
                return View();
            }

            // Validate the wallet balance
            if (userWallet.Balance < amount)
            {
                ViewBag.ErrorMessage = "Insufficient balance.";
                return View();
            }

            // Start a database transaction
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // Deduct the amount from the wallet balance
                userWallet.Balance -= amount;

                // Update the wallet balance in the database
                _context.SaveChanges();

                // Insert the billing record using raw SQL (date is handled by the DBMS)
                _context.Database.ExecuteSqlRaw(
                  "INSERT INTO \"Billing\" (\"Billing_number\", \"Amount\", \"W_ID\") VALUES ({0}, {1}, {2})",
                  billingNumber,
                  amount,
                  userWallet.Wallet_ID
                     );

                // Commit the transaction
                transaction.Commit();

                ViewBag.SuccessMessage = "Payment successful.";
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an error
                transaction.Rollback();
                ViewBag.ErrorMessage = "An error occurred while processing your payment: " + ex.Message;
            }

            return View();
        }
        [HttpGet]
        public IActionResult Billing_History()
        {
            // Retrieve the username from the session
            string username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                // If no username is found in the session, redirect to login page
                return RedirectToAction("Login");
            }

            // Step 1: Find the user in the UserModel
            var user = _context.User.FirstOrDefault(u => u.Username == username);

            if (user == null)
            {
                // If the user is not found, display an error
                return NotFound("User not found.");
            }

            // Step 2: Find the wallet for the user
            var wallet = _context.Wallet.FirstOrDefault(w => w.User_ID == user.ID);

            if (wallet == null)
            {
                // If no wallet is found for the user, display an error
                return NotFound("Wallet not found for the user.");
            }

            // Step 3: Get the Wallet_ID
            var walletId = wallet.Wallet_ID;

            // Step 4: Get all billing records where W_ID matches the Wallet_ID
            var billingRecords = _context.Billing
                .Where(b => b.W_ID == walletId)
                .ToList();

            // Step 5: Pass the billing records to the view
            return View(billingRecords);
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