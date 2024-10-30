using BionetTX.Models;
using BionetTX.Services;
using BionetTX.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BionetTX.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]
    public class MailController(IMailService mailService, IDBDapper dBDapper) : Controller
    {

        public IDBDapper DBDapper = dBDapper;
        public IMailService mailService = mailService;



        #region 寄件功能
        [HttpPost]

        public IActionResult SendToTX(string infoStr)
        {

            var result = "";
            mailService.SendToTX(infoStr);
            return Ok(new { status = 200, message = "Message Sent! Thank you.", data = result });

        }
        #endregion



    }
}
