//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using QassimPay.Data;
//using QassimPay.Models;
//using System;
//using System.Linq;

//namespace QassimPay
//{
//    [Authorize]
//    public class TransferController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public TransferController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public IActionResult Transfer()
//        {
//            return View();
//        }

//        [HttpPost]
//        public IActionResult Transfer(TransferModel model)
//        {
//            var currentUserId = GetCurrentUserId();

//            var senderWallet = _context.Wallet.FirstOrDefault(w => w.User_ID == currentUserId);
//            var recipientWallet = _context.Wallet.FirstOrDefault(w => w.Wallet_ID == model.Reciver);

//            if (recipientWallet == null)
//            {
//                ViewBag.ErrorMessage = "Recipient wallet does not exist.";
//                return View();
//            }

//            if (senderWallet == null)
//            {
//                ViewBag.ErrorMessage = "You do not have a wallet.";
//                return View();
//            }

//            if (senderWallet.Balance < model.AmountM)
//            {
//                ViewBag.ErrorMessage = "Insufficient balance.";
//                return View();
//            }

//            senderWallet.Balance -= model.AmountM;
//            recipientWallet.Balance += model.AmountM;

//            var transfer = new TransferModel
//            {
//                AmountM = model.AmountM,
//                Reciver = model.Reciver,
//                Sender_ID = currentUserId
//            };

//            _context.Transfer.Add(transfer);
//            _context.SaveChanges();

//            ViewBag.SuccessMessage = "Transfer successful.";
//            return View();
//        }

//        private int GetCurrentUserId()
//        {
//            var user = _context.User.FirstOrDefault(u => u.Username == User.Identity.Name);
//            return user?.ID ?? 0;
//        }
//    }
//}