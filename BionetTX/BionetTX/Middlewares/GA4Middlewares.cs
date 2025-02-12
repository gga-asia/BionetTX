using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BionetTX.Middlewares
{
    public class GA4Middlewares
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        public GA4Middlewares(RequestDelegate next, IConfiguration configuration)
        {
            _next = next; // 儲存下一個 Middleware 的委派函數
            _configuration = configuration; // 儲存配置接口的實例
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var isGA4Enabled = _configuration.GetValue<bool>("GA4:Enabled");
            // 設定存在 HttpContext
            context.Items["GA4Enabled"]=isGA4Enabled;
            // 將請求傳遞到下一層 Middleware
            await _next(context);
        }

    }
}
