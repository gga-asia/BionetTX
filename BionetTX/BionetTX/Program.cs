using BionetTX.Services.IServices;
using BionetTX.Services;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Http.Headers;
using Microsoft.Extensions.Hosting;
using BionetTX.Middlewares;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// 增加MVC服務, 預設的
builder.Services.AddControllersWithViews();

// 自行增加
// Default DbConnection
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("API_LOG"))
);
// DBdapper
builder.Services.AddScoped<IDBDapper, DBDapper>();
// MailService
builder.Services.AddScoped<IMailService, MailService>();
// mail API
builder.Services.AddHttpClient<IMailService, MailService>(client =>
{
    client.BaseAddress = new Uri("https://wpa.bionetcorp.com:9004");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});
// 將 appsettings.json 加入配置 , Configuration 是內建配置
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

var app = builder.Build();

// 註冊middleware
app.UseMiddleware<GA4Middlewares>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


// Redirects
app.Use(async (context, next) =>
{
    // 獲取獲取協議、主機名稱、埠號
    var scheme = context.Request.Scheme; // http 或 https
    var host = context.Request.Host; // 輸出類似：localhost:7292
    var hostName = context.Request.Host.Host; // 僅取得網域名稱 輸出：localhost
    var port = context.Request.Host.Port; // 取得埠號 輸出：7292

    // 獲取設定檔的環境資訊與網域配置
    var environment = builder.Configuration["Environment"];
    var exosomeDomain = builder.Configuration["DomainRouting:ExosomeDomain"];
    var biotxDomain = builder.Configuration["DomainRouting:BioTxDomain"];
    

    // Check if the request is coming from  port or domain
    // 定義跳轉邏輯
    void Redirect(string path)
    {
        if (context.Request.Path == "/")
        {
            context.Response.Redirect(path, permanent: false);
        }
    }

    bool IsLocalRequest()
    {
        return hostName == "localhost" || hostName == "172.31.1.15";
    }

    bool IsExosomePort()
    {
        return port == 7292 || port == 8088;
    }

    bool IsBioTxPort()
    {
        return port == 7291 || port == 8087;
    }

    // 環境判斷
    if (environment == "Development" || environment == "UAT")
    {
        if (IsLocalRequest() && IsExosomePort())
        {
            Redirect("/Exosomefoundry/Index");
        }
        else if (IsLocalRequest() && IsBioTxPort())
        {
            Redirect("/Home/Index");
        }
    }
    else if (environment == "Production")
    {
        if (hostName == exosomeDomain)
        {
            Redirect("https://www.exosomefoundry.com/exosomefoundry/Index");
        }
        else if (hostName == biotxDomain)
        {
            Redirect("/Home/Index");
        }
    }


    await next();
});





app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
