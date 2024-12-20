using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QassimPay.Models;
using QassimPay.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;

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

        // Displays the "About Us" page
        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        // Displays the "Home" page
        public IActionResult Index()
        {
            return View();
        }

        // Displays the login page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Processes login submissions
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Validate user credentials
            var user = _context.User.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);
                return RedirectToAction("Wallet");
            }

            ModelState.AddModelError("", "Wrong username or password");
            return View();
        }

        // Displays the wallet page for logged-in users
        public IActionResult Wallet()
        {
            // Check if user is logged in
            if (!IsUserLoggedIn(out string username))
                return RedirectToAction("Login");

            // Set the username to the ViewBag for use in the view
            ViewBag.Username = username;

            // Retrieve the user
            var user = _context.User.FirstOrDefault(u => u.Username == username);
            if (user == null)
                return NotFound("User not found.");

            // Retrieve the user's wallet
            var wallet = _context.Wallet.FirstOrDefault(w => w.User_ID == user.ID);
            ViewBag.Wallet = wallet;

            if (wallet != null)
            {
                // Retrieve transfer history
                var transferHistory = _context.Transfer
                    .Where(t => t.Sender_ID == wallet.Wallet_ID || t.Reciver == user.ID)
                    .ToList();

                // Retrieve billing history
                var billingHistory = _context.Billing
                    .Where(b => b.W_ID == wallet.Wallet_ID)
                    .ToList();

                // Pass the histories to ViewBag
                ViewBag.TransferHistory = transferHistory;
                ViewBag.BillingHistory = billingHistory;
            }

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

        // Displays the transfer form
        [HttpGet]
        public IActionResult Transfer()
        {
            if (!IsUserLoggedIn())
                return RedirectToAction("Login");

            return View();
        }

        // Processes fund transfers
        [HttpPost]
        public IActionResult Transfer(int recipientWalletId, decimal amount)
        {
            if (!IsUserLoggedIn(out string username))
                return RedirectToAction("Login");

            var sender = _context.User.FirstOrDefault(u => u.Username == username);
            var senderWallet = _context.Wallet.FirstOrDefault(w => w.User_ID == sender.ID);
            var recipientWallet = _context.Wallet.FirstOrDefault(w => w.Wallet_ID == recipientWalletId);

            if (!ValidateTransfer(senderWallet, recipientWallet, amount, out string errorMessage))
            {
                ViewBag.ErrorMessage = errorMessage;
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
                    senderWallet.Wallet_ID, recipientWallet.User_ID, amount);
                var a = sender.Email;
                //send email transfer
                SendEmailNotification(sender.Email, recipientWallet.User_ID.ToString(), amount, "Transfer");
                transaction.Commit();
                ViewBag.SuccessMessage = "Transfer successful.";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError("Transfer failed: {Message}", ex.Message);
                ViewBag.ErrorMessage = "An error occurred while processing the transfer.";
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
                //email billing
                SendEmailNotification(user.Email, "Billing Company", amount, "Billing");
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

        // Displays the registration form
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Processes registration submissions
        [HttpPost]
        public IActionResult Register(UserModel user)
        {
            if (!ValidateRegistration(user, out string errorMessage))
            {
                ModelState.AddModelError("", errorMessage);
                return View(user);
            }

            // Save new user to the database
            _context.User.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Login");
        }


        public IActionResult Logout()
        {
            // Clear the session
            HttpContext.Session.Clear();

            // Redirect to the About page
            return RedirectToAction("About");
        }


        // Helper: Checks if a user is logged in
        private bool IsUserLoggedIn(out string username)
        {
            username = HttpContext.Session.GetString("Username");
            return !string.IsNullOrEmpty(username);
        }

        private bool IsUserLoggedIn()
        {
            return IsUserLoggedIn(out _);
        }

        // Helper: Validates transfer details
        private bool ValidateTransfer(WalletModel senderWallet, WalletModel recipientWallet, decimal amount, out string errorMessage)
        {
            if (recipientWallet == null)
            {
                errorMessage = "Recipient wallet does not exist.";
                return false;
            }

            if (senderWallet == null)
            {
                errorMessage = "You do not have a wallet.";
                return false;
            }

            if (senderWallet.Balance < amount)
            {
                errorMessage = "Insufficient balance.";
                return false;
            }

            errorMessage = null;
            return true;
        }

        // Helper: Validates registration details
        private bool ValidateRegistration(UserModel user, out string errorMessage)
        {
            if (_context.User.Any(u => u.Username == user.Username))
            {
                errorMessage = "Username already exists.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(user.First_name) ||
                string.IsNullOrWhiteSpace(user.Last_name) ||
                string.IsNullOrWhiteSpace(user.Email) ||
                string.IsNullOrWhiteSpace(user.Password) ||
                string.IsNullOrWhiteSpace(user.Username) ||
                user.Monthly_income <= 0)
            {
                errorMessage = "All fields are required.";
                return false;
            }

            if (user.Password?.Length < 8)
            {
                errorMessage = "Password must be at least 8 characters long.";
                return false;
            }

            errorMessage = null;
            return true;
        }

        // GET: Address Page
        [HttpGet]
        public IActionResult Address()
        {
            // Get the username from the session
            var username = HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToAction("Login");
            }

            // Find the user based on the username
            var user = _context.User.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            // Get the user's addresses
            var addresses = _context.Address.Where(a => a.U_ID == user.ID).ToList();

            // Pass the addresses and a flag to allow adding new addresses
            ViewBag.Addresses = addresses; // List of addresses
            ViewBag.CanAddAddress = !addresses.Any(); // True if no address exists

            return View();
        }

        // POST: Add New Address
        [HttpPost]
        public IActionResult Address(AddressModel model)
        {
            // Get the username from the session
            var username = HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToAction("Login");
            }

            // Find the user based on the username
            var user = _context.User.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            // Check if the user already has an address
            if (!_context.Address.Any(a => a.U_ID == user.ID))
            {
                // Create a new address and link it to the user
                var newAddress = new AddressModel
                {
                    U_ID = user.ID,
                    Street_Adress = model.Street_Adress,
                    City = model.City,
                    State = model.State,
                    Postal_Code = model.Postal_Code,
                    Country = model.Country
                };

                // Add the new address to the database
                _context.Address.Add(newAddress);
                _context.SaveChanges();
            }

            // Redirect back to the Address page
            return RedirectToAction("Address");
        }

        // POST: Delete an Address
      
        [HttpPost]
        public IActionResult DeleteAddress(int addId)
        {
            // Get the username from the session
            var username = HttpContext.Session.GetString("Username");
            if (username == null)
            {
                return RedirectToAction("Login");
            }

            // Find the user based on the username
            var user = _context.User.FirstOrDefault(u => u.Username == username);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            // Find the address by its ID and ensure it belongs to the current user
            var address = _context.Address.FirstOrDefault(a => a.ADD_ID == addId && a.U_ID == user.ID);
            if (address == null)
            {
                return NotFound("Address not found or does not belong to you.");
            }

            // Remove the address from the database
            _context.Address.Remove(address);
            _context.SaveChanges();

            // Redirect back to the Address page
            return RedirectToAction("Address");
        }
        //Send Email
        private void SendEmailNotification(string senderEmail, string recipientDetails, decimal amount, string transactionType)
        {
            if (string.IsNullOrEmpty(senderEmail))
            {
                // Skip email sending if sender's email is not available
                return;
            }

            try
            {
                // Configure SMTP client
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("complaintmangsys@gmail.com", "vpubnsbenqclkkzp"),
                    EnableSsl = true,
                };

                // Prepare email content in Arabic and English
                string emailBody = $@"
                    <html>
                    <body>
                        <div dir='rtl' style='text-align:right;'>
                            <p>عزيزي العميل،</p>
                            <p>يسرنا إبلاغك بأن عملية <strong>{(transactionType == "Transfer" ? "التحويل" : "دفع الفاتورة")}</strong> قد تمت بنجاح.</p>
                            <p><strong>المبلغ:</strong> {amount:C}</p>
                            {(transactionType == "Transfer" ? $"<p><strong>المستلم:</strong> {recipientDetails}</p>" : "")}
                            <p>شكرًا لاستخدامك خدمات Qassim Pay. إذا كان لديك أي استفسار، يرجى التواصل مع فريق الدعم لدينا.</p>
                        </div>
                        <hr>
                        <div dir='ltr' style='text-align:left;'>
                            <p>Dear Valued Customer,</p>
                            <p>We are pleased to inform you that your <strong>{(transactionType == "Transfer" ? "transfer" : "bill payment")}</strong> transaction has been successfully completed.</p>
                            <p><strong>Amount:</strong> {amount:C}</p>
                            {(transactionType == "Transfer" ? $"<p><strong>Recipient:</strong> {recipientDetails}</p>" : "")}
                            <p>Thank you for using Qassim Pay. If you have any questions, please contact our support team.</p>
                        </div>
                        <p><img src='cid:QassimPayImage' alt='QassimPay Logo' style='width:200px; display:block; margin:auto;'></p>
                    </body>
                    </html>";

                // Create email message
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("complaintmangsys@gmail.com"),
                    Subject = transactionType == "Transfer" ? "إشعار تحويل مصرفي / Bank Transfer Notification" : "إشعار دفع فاتورة / Bill Payment Notification",
                    Body = emailBody,
                    IsBodyHtml = true, // Important to send HTML emails
                };

                // Set email encoding to UTF-8
                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;

                mailMessage.To.Add(senderEmail);

                // Attach the image from local path
                string imagePath = @"C:\Users\huawei\Desktop\Cons\QassimPay\wwwroot\images\QassimPas.png";
                var inlineLogo = new Attachment(imagePath)
                {
                    ContentId = "QassimPayImage",
                    ContentDisposition = { Inline = true, DispositionType = DispositionTypeNames.Inline }
                };
                mailMessage.Attachments.Add(inlineLogo);

                // Send email
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // Log or handle the email sending error
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }




    }
}

    
