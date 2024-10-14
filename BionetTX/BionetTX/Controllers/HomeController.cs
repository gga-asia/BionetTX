using BionetTX.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BionetTX.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

 
        #region 中文版-首頁
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region 中文版-最新消息
        public IActionResult News()
        {
            return View();
        }
        #endregion

        #region 中文版-訊聯細胞智藥簡介
        public IActionResult TxIntro()
        {
            return View();
        }
        #endregion

        #region 中文版-集團簡介
        public IActionResult CorpIntro()
        {
            return View();
        }
        #endregion

        #region 中文版-細胞製劑
        public IActionResult CellTherapy()
        {
            return View();
        }
        #endregion

        #region 中文版-外泌體Exosome
        public IActionResult Exosome()
        {
            return View();
        }
        #endregion

        #region 中文版-聯絡我們
        public IActionResult Contact()
        {
            return View();
        }
        #endregion        
        
        #region 中文版-個資宣告
        public IActionResult Disclaimer()
        {
            return View();
        }
        #endregion

        #region 中文版-新聞消息-15
        public IActionResult NewsList15()
        {
            return View();
        }
        #endregion        
        
        #region 中文版-新聞消息-1
        public IActionResult NewsList1()
        {
            return View();
        }
        #endregion

        #region 中文版-新聞消息-2
        public IActionResult NewsList2()
        {
            return View();
        }
        #endregion

        #region 中文版-新聞消息-3
        public IActionResult NewsList3()
        {
            return View();
        }
        #endregion

        #region 中文版-新聞消息-4
        public IActionResult NewsList4()
        {
            return View();
        }
        #endregion

        #region 中文版-新聞消息-5
        public IActionResult NewsList5()
        {
            return View();
        }
        #endregion

        #region 中文版-新聞消息-6
        public IActionResult NewsList6()
        {
            return View();
        }
        #endregion

        #region 中文版-新聞消息-7
        public IActionResult NewsList7()
        {
            return View();
        }
        #endregion



       



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
