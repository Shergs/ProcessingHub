using AuthSystem.Data;
using Basic.Areas.Identity.Data;
using Basic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using X.PagedList;

namespace Basic.Controllers
{
    // Used this attribute to reroute non-logged in users to the login menu
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AuthDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager, AuthDbContext context)
        {
            _logger = logger;
            this._userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index(string? email)
        {
            ViewData ["UserID"] = _userManager.GetUserId(this.User);
            var userEmail = await _userManager.GetEmailAsync(await _userManager.GetUserAsync(this.User));
            
            // set merchant
            if (email == null)
            {
                ViewBag.Merchant = await _context.Merchants.Where(m => m.Email == userEmail).FirstOrDefaultAsync();
            }
            else 
            {
                ViewBag.Merchant = await _context.Merchants.Where(m => m.Email == email).FirstOrDefaultAsync();
            }

            // set transactions
            
            IPagedList<TransactionHistory>? transactions = ViewBag.Merchant == null ? null : await Transactions(1, ViewBag.Merchant.Id);

            DashboardViewModel model = new DashboardViewModel()
            { 
                Merchant = ViewBag.Merchant,
                Transactions = transactions,
            };
              
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
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
        public async Task<IPagedList<TransactionHistory>> Transactions(int? page, int merchantId)
        {
            const int recordsPerPage = 10;
            var pageNumber = (page ?? 1);

            var transactions = _context.TransactionHistories.Where(th => th.MerchantId == merchantId).AsQueryable(); // No execution here
            var pagedList = await transactions.ToPagedListAsync(pageNumber, recordsPerPage);

            return pagedList;
        }

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
    }
}