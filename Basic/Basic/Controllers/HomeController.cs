using AuthSystem.Data;
using Basic.Areas.Identity.Data;
using Basic.Migrations;
using Basic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using X.PagedList;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;
using System.Text.Json;
using Basic.Helpers;

namespace Basic.Controllers
{
    // Used this attribute to reroute non-logged in users to the login menu
    [Authorize]
    [CheckSession]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthDbContext _context;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, AuthDbContext context, ICompositeViewEngine viewEngine, ITempDataProvider tempDataProvider)
        {
            _logger = logger;
            this._userManager = userManager;
            _context = context;
            _viewEngine = viewEngine ?? throw new ArgumentNullException(nameof(viewEngine));
            _tempDataProvider = tempDataProvider ?? throw new ArgumentNullException(nameof(tempDataProvider));
        }

        public async Task<IActionResult> Index(string? email)
        {
            ViewData ["UserID"] = _userManager.GetUserId(this.User);
            var userEmail = await _userManager.GetEmailAsync(await _userManager.GetUserAsync(this.User));

            Merchant merchant = null;
            // set merchant
            if (email == null)
            {
                merchant = await _context.Merchants.Where(m => m.Email == userEmail).FirstOrDefaultAsync();
                HttpContext.Session.SetString("Merchant", JsonSerializer.Serialize(merchant));
            }
            else 
            {
                merchant = await _context.Merchants.Where(m => m.Email == email).FirstOrDefaultAsync();
                HttpContext.Session.SetString("Merchant", JsonSerializer.Serialize(merchant));
            }

            // set transactions

            IPagedList<Basic.Models.TransactionHistory>? transactions = merchant == null ? null : await Transactions(1, merchant.Id);
            IPagedList<Basic.Models.CardPayment>? cardPayments = merchant == null ? null : await CardPayments(1, merchant.Id);
            IPagedList<Basic.Models.Invoice>? invoices = merchant == null ? null : await Invoices(1, merchant.Id);

            DashboardViewModel model = new DashboardViewModel()
            { 
                Merchant = ViewBag.Merchant,
                Transactions = transactions,
                CardPayments = cardPayments,
                Invoices = invoices
            };
              
            return View(model);
        }

        public async Task<IActionResult> Invoice(int MerchantId)
        {
            // email isn't selected anyways, so return without one
            if (MerchantId == -1) 
            {
                return Redirect(Url.Action("Index", "Home", new { Area="" }));
            }

            Merchant? merchant = await _context.Merchants.Where(m => m.Id == MerchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Redirect(Url.Action("Index", "Home", new { Area = "" }));
            }

            InvoiceViewModel model = new InvoiceViewModel()
            {
                Merchant = merchant,
                SendInvoiceViewModel = new SendInvoiceViewModel 
                { 
                    MerchantId = MerchantId,
                    InvoiceItems = new List<InvoiceItemViewModel>
                    {
                        new InvoiceItemViewModel(), // Item 1
                        new InvoiceItemViewModel(), // Item 2
                        new InvoiceItemViewModel(), // Item 2
                        new InvoiceItemViewModel(), // Item 2
                        new InvoiceItemViewModel(), // Item 2
                    }
                }
            };

            return View(model);
        }

        public async Task<IActionResult> SalesSettings(int MerchantId)
        {
            // email isn't selected anyways, so return without one
            if (MerchantId == -1)
            {
                return Redirect(Url.Action("Index", "Home", new { Area = "" }));
            }

            Merchant? merchant = await _context.Merchants.Where(m => m.Id == MerchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Redirect(Url.Action("Index", "Home", new { Area = "" }));
            }

            SalesSettingsViewModel model = new SalesSettingsViewModel()
            {
                Merchant = merchant,
                SaveSalesSettingsViewModel = new SaveSalesSettingsViewModel()
                {
                    MerchantId = MerchantId
                }
            };

            return View(model);
        }

        /// <summary>
        /// Save sales settings
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSalesSettings([Bind(Prefix = "SaveSalesSettingsViewModel")] SaveSalesSettingsViewModel model)
        {
            if (ModelState.IsValid) {
                // Save sales settings here
                Merchant merchant = _context.Merchants.Where(m => m.Id == model.MerchantId).FirstOrDefault();
                merchant.TaxRate = model.TaxRate;
                merchant.FeeName = model.FeeName;
                merchant.FeePercent = model.FeePercent;
                await _context.SaveChangesAsync();

                TempData["Message"] = "Settings Saved Successfully!";
                TempData["MessageType"] = "Success";
                return RedirectToAction("SalesSettings", new { MerchantId = model.MerchantId });
            }
            else
            {
                Merchant merchant = _context.Merchants.Where(m => m.Id == model.MerchantId).FirstOrDefault();
                TempData["Message"] = "Settings Failed To Save. Please try again.";
                TempData["MessageType"] = "Error";
                return View("SalesSettings", new SalesSettingsViewModel { Merchant = merchant, SaveSalesSettingsViewModel = model});
            }
        }

        /// <summary>
        /// Get Merchant Settings
        /// </summary>
        /// <param name="MerchantId"></param>
        /// <returns></returns>
        public async Task<IActionResult> MerchantSettings(int MerchantId)
        {
            // email isn't selected anyways, so return without one
            if (MerchantId == -1)
            {
                return Redirect(Url.Action("Index", "Home", new { Area = "" }));
            }

            Merchant? merchant = await _context.Merchants.Where(m => m.Id == MerchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Redirect(Url.Action("Index", "Home", new { Area = "" }));
            }

            MerchantSettingsViewModel model = new MerchantSettingsViewModel()
            {
                Merchant = merchant,
                SaveMerchantSettingsViewModel = new SaveMerchantSettingsViewModel()
                {
                    MerchantId = MerchantId
                }
            };

            return View(model);
        }

        /// <summary>
        /// Get Card Payment
        /// </summary>
        /// <param name="MerchantId"></param>
        /// <returns></returns>
        public async Task<IActionResult> CardPayment(int MerchantId)
        {
            // email isn't selected anyways, so return without one
            if (MerchantId == -1)
            {
                return Redirect(Url.Action("Index", "Home", new { Area = "" }));
            }

            Merchant? merchant = await _context.Merchants.Where(m => m.Id == MerchantId).FirstOrDefaultAsync();

            if (merchant == null)
            {
                return Redirect(Url.Action("Index", "Home", new { Area = "" }));
            }

            CardPaymentViewModel model = new CardPaymentViewModel()
            {
                Merchant = merchant,
                SaveCardPaymentViewModel = new SaveCardPaymentViewModel()
                {
                    MerchantId = MerchantId,
                }
            };

            return View(model);
        }

        /// <summary>
        /// Send and Save Card Payment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendCardPayment([Bind(Prefix = "SaveCardPaymentViewModel")] SaveCardPaymentViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Handle the form submission
                // ...
                // Error conditions after
                // but for now make it save to a table


                // Save to DB
                Basic.Models.CardPayment cardPayment = new Basic.Models.CardPayment()
                {
                    MerchantId = model.MerchantId,
                    CardNumber = model.CardNumber,
                    Expiration = model.Expiration,
                    SecurityCode = model.SecurityCode,
                    Amount = model.Amount,
                    RecipitentEmail = model.RecipitentEmail == null ? "" : model.RecipitentEmail,
                    Timestamp = DateTime.Now
                };

                _context.CardPayments.Add(cardPayment);
                await _context.SaveChangesAsync();

                // Generate invoice and send
                model.Merchant = await _context.Merchants.Where(m => m.Id == model.MerchantId).FirstOrDefaultAsync();
                var emailBody = await RenderViewToStringAsync("CardPaymentTemplate", model);
                SendInvoiceEmail("shergertons@gmail.com", model.Merchant.ReceiptEmail, model.Merchant.AppPassword, emailBody);
                if (model.RecipitentEmail != null && model.RecipitentEmail != "")
                {
                    var receiptEmailBody = await RenderViewToStringAsync("CardPaymentReceiptTemplate", model);
                    SendInvoiceEmail(model.RecipitentEmail, model.Merchant.ReceiptEmail, model.Merchant.AppPassword, receiptEmailBody);
                }

                TempData["Message"] = "Payment Successful!";
                TempData["MessageType"] = "Success";
                return RedirectToAction("CardPayment", new { MerchantId = model.MerchantId });
            }
            else
            {
                Merchant merchant = _context.Merchants.Where(m => m.Id == model.MerchantId).FirstOrDefault();
                TempData["Message"] = "Failed To Make Payment. Please try again.";
                TempData["MessageType"] = "Error";
                return View("CardPayment", new CardPaymentViewModel { Merchant = merchant, SaveCardPaymentViewModel = model });
            }
        }

        /// <summary>
        /// Save Merchant Settings
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveMerchantSettings([Bind(Prefix = "SaveMerchantSettingsViewModel")] SaveMerchantSettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Save sales settings here
                Merchant merchant = _context.Merchants.Where(m => m.Id == model.MerchantId).FirstOrDefault();
                merchant.Name = model.Name;
                merchant.StreetAddress = model.StreetAddress;
                merchant.City = model.City;
                merchant.State = model.State;
                merchant.Zip = model.Zip;
                merchant.Country = model.Country;
                merchant.ReceiptEmail = model.ReceiptEmail;
                merchant.AppPassword = model.AppPassword;

                await _context.SaveChangesAsync();

                TempData["Message"] = "Settings Saved Successfully!";
                TempData["MessageType"] = "Success";
                return RedirectToAction("MerchantSettings", new { MerchantId = model.MerchantId });
            }
            else
            {
                Merchant merchant = _context.Merchants.Where(m => m.Id == model.MerchantId).FirstOrDefault();
                TempData["Message"] = "Settings Failed To Save. Please try again.";
                TempData["MessageType"] = "Error";
                return View("MerchantSettings", new MerchantSettingsViewModel { Merchant = merchant, SaveMerchantSettingsViewModel = model });
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<List<Merchant>> LoadMerchantsAsync()
        {
            List<Merchant> merchants = await _context.Merchants.ToListAsync();
            return merchants;
        }

        /// <summary>
        /// Get transactions for table
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<IPagedList<Basic.Models.TransactionHistory>> Transactions(int? page, int merchantId)
        {
            const int recordsPerPage = 10;
            var pageNumber = (page ?? 1);

            var transactions = _context.TransactionHistories.Where(th => th.MerchantId == merchantId).AsQueryable(); // No execution here
            var pagedList = await transactions.ToPagedListAsync(pageNumber, recordsPerPage);

            return pagedList;
        }

        /// <summary>
        /// Get Card Payments for table
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<IPagedList<Basic.Models.CardPayment>> CardPayments(int? page, int merchantId)
        {
            const int recordsPerPage = 10;
            var pageNumber = (page ?? 1);

            var transactions = _context.CardPayments.Where(th => th.MerchantId == merchantId).OrderByDescending(th => th.Timestamp).AsQueryable(); // No execution here
            var pagedList = await transactions.ToPagedListAsync(pageNumber, recordsPerPage);

            return pagedList;
        }

        /// <summary>
        /// Get Invoices for table
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<IPagedList<Basic.Models.Invoice>> Invoices(int? page, int merchantId)
        {
            const int recordsPerPage = 10;
            var pageNumber = (page ?? 1);

            var transactions = _context.Invoices.Where(th => th.MerchantId == merchantId).OrderByDescending(th => th.Timestamp).AsQueryable(); // No execution here
            var pagedList = await transactions.ToPagedListAsync(pageNumber, recordsPerPage);

            return pagedList;
        }

        /// <summary>
        /// Get transactions table
        /// </summary>
        /// <param name="page"></param>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> PageTransactions(int page, int merchantId)
        {
            var transactions = await Transactions(page, merchantId); // Assuming you have a method to get paged data
            var viewModel = new TransactionTableViewModel
            {
                Transactions = transactions,
                MerchantId = merchantId
            };
            return PartialView("_TransactionTablePartial", viewModel);
        }

        /// <summary>
        /// Get Card Payments table
        /// </summary>
        /// <param name="page"></param>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> PageCardPayments(int page, int merchantId)
        {
            var transactions = await CardPayments(page, merchantId); // Assuming you have a method to get paged data
            var viewModel = new CardPaymentTableViewModel
            {
                CardPayments = transactions,
                MerchantId = merchantId
            };
            return PartialView("_CardPaymentTablePartial", viewModel);
        }

        /// <summary>
        /// Get Card Payments table
        /// </summary>
        /// <param name="page"></param>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> PageInvoices(int page, int merchantId)
        {
            var transactions = await Invoices(page, merchantId); // Assuming you have a method to get paged data
            var viewModel = new InvoiceTableViewModel
            {
                Invoices = transactions,
                MerchantId = merchantId
            };
            return PartialView("_InvoiceTablePartial", viewModel);
        }

        /// <summary>
        /// Send and Save Invoice
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendInvoice([Bind(Prefix = "SendInvoiceViewModel")] SendInvoiceViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Handle the form submission
                // ...
                // Error conditions after
                // but for now make it save to a table


                // Save to DB
                Basic.Models.Invoice invoice = new Basic.Models.Invoice()
                {
                    MerchantId = model.MerchantId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    InvoiceItems = model.InvoiceItems.Select(item => new InvoiceItem
                    {
                        Item = item.Item,
                        Count = item.Count,
                        Price = item.Price
                    }).ToList(),
                    Timestamp = DateTime.Now
                };

                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();

                // Generate invoice and send
                model.Merchant = await _context.Merchants.Where(m => m.Id == model.MerchantId).FirstOrDefaultAsync();
                var emailBody = await RenderViewToStringAsync("InvoiceTemplate", model);
                await SendInvoiceEmailWithAttachmentAsync(model.Email, model.Merchant.ReceiptEmail, model.Merchant.AppPassword, emailBody);

                TempData["Message"] = "Invoice sent successfully!";
                TempData["MessageType"] = "Success";
                return RedirectToAction("Invoice", new { MerchantId = model.MerchantId });
            }
            else
            {
                Merchant merchant = _context.Merchants.Where(m => m.Id == model.MerchantId).FirstOrDefault();
                TempData["Message"] = "Failed to send the invoice. Please try again.";
                TempData["MessageType"] = "Error";
                return View("Invoice", new InvoiceViewModel { Merchant = merchant, SendInvoiceViewModel = model });
            }
        }


        /// <summary>
        /// Send Invoice Email, Not used right now, could use for card payments recipt
        /// </summary>
        /// <param name="recipientEmail"></param>
        /// <param name="invoiceHtml"></param>
        public void SendInvoiceEmail(string recipientEmail, string receiptEmail, string appPassword, string invoiceHtml)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(receiptEmail, appPassword),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(receiptEmail),
                    Subject = "Your Invoice",
                    Body = invoiceHtml,
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(recipientEmail);

                smtpClient.Send(mailMessage);

                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email. Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Get invoice email html as string
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<string> RenderViewToStringAsync(string viewName, object model)
        {
            using (var sw = new StringWriter())
            {
                ControllerContext.RouteData.Values["controller"] = "Home";
                var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);

                if (viewResult.View == null)
                {
                    return null;
                }

                var viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {
                        Model = model
                    },
                    new TempDataDictionary(ControllerContext.HttpContext, _tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }


        public async Task SendInvoiceEmailWithAttachmentAsync(string toEmail, string receiptEmail, string appPassword, string htmlContent)
        {
            // Save the HTML content to a temp file
            var tempHtmlPath = Path.GetTempFileName() + ".html";
            System.IO.File.WriteAllText(tempHtmlPath, htmlContent);

            // Convert the HTML file to PDF using wkhtmltopdf
            var tempPdfPath = Path.GetTempFileName() + ".pdf";
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf",  // Ensure wkhtmltopdf is in your PATH or provide the full path
                    Arguments = $"--enable-local-file-access {tempHtmlPath} {tempPdfPath}",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            await process.WaitForExitAsync();
            process.Close(); // Frees resources associated with process


            // Send the email with the PDF attachment
            using (var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(receiptEmail, appPassword),
                EnableSsl = true,
            })
            using (var mailMessage = new MailMessage
            {
                From = new MailAddress(receiptEmail),
                Subject = "Your Invoice",
                Body = "Please find attached your invoice.",
                IsBodyHtml = true
            })
            {
                mailMessage.To.Add(toEmail);
                mailMessage.Attachments.Add(new Attachment(tempPdfPath));
                await smtpClient.SendMailAsync(mailMessage);
            }

            // Cleanup temp files
            System.IO.File.Delete(tempHtmlPath);
            System.IO.File.Delete(tempPdfPath);
        }

    }
}