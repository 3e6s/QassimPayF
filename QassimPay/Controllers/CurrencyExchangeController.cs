using Microsoft.AspNetCore.Mvc;
using QassimPay.Data;
using QassimPay.Models;
using System.Linq;

public class CurrencyExchangeController : Controller
{
    private readonly ApplicationDbContext _context;

    public CurrencyExchangeController(ApplicationDbContext context)
    {
        _context = context;
    }

    // عرض جميع أسعار الصرف والنموذج
    public IActionResult Index()
    {
        try
        {
            // جلب العملات المميزة من قاعدة البيانات
            var currencies = _context.CurrencyExchange
                .Select(c => c.CurrencyFrom)
                .Union(_context.CurrencyExchange.Select(c => c.CurrencyTo))
                .Distinct()
                .ToList();

            ViewBag.Currencies = currencies; // إرسال العملات إلى ViewBag

            var exchangeRates = _context.CurrencyExchange.ToList(); // جلب أسعار الصرف
            return View(exchangeRates); // إرسال البيانات إلى الواجهة
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = $"Error loading data: {ex.Message}";
            return View(new List<CurrencyExchangeModel>());
        }
    }

    [HttpPost]
    public JsonResult Convert(string fromCurrency, string toCurrency, decimal amount)
    {
        try
        {
            // البحث عن سعر الصرف
            var exchangeRate = _context.CurrencyExchange
                .FirstOrDefault(c => c.CurrencyFrom == fromCurrency && c.CurrencyTo == toCurrency);

            if (exchangeRate == null)
            {
                return Json(new { success = false, message = "Exchange rate not found. Please check the currency codes and try again." });
            }

            // حساب المبلغ المحول
            var convertedAmount = amount * exchangeRate.ExchangeRate;

            return Json(new { success = true, convertedAmount = convertedAmount, fromCurrency = fromCurrency, toCurrency = toCurrency });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
        }
    }
}
