using BionetTX.Services.IServices;
using BionetTX.Services;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Http.Headers;
using Microsoft.Extensions.Hosting;

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



var app = builder.Build();

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

//測試1
//app.Use(async (context, next) =>
//{
//    // Check if the request is specifically on localhost:7292
//    if (context.Request.Host.Port == 7292 && context.Request.Path == "/")
//    {
//        // Perform a 302 Redirect to the specific controller/action
//        context.Response.Redirect("/ExosomeFoundry/Index", permanent: false);
//        return; // Exit the middleware to prevent further processing
//    }

//    await next();
//});

//測試 2 可用
//Middleware for domain-based routing
//app.Use(async (context, next) =>
//{
//    // Check if the request is coming from port 7292
//    if (context.Request.Host.Port == 7292)
//    {
//        // Map to exosomefoundry-specific route
//        if (context.Request.Path == "/")
//        {
//            context.Response.Redirect("/ExosomeFoundry/Index", permanent: false);
//        }
//    }
//    else if (context.Request.Host.Port == 7291)
//    {
//        // This is the default setup for biotx.com; no path modification needed unless there’s a specific landing route
//        if (context.Request.Path == "/")
//        {

//            context.Request.Path = "/Home/Index"; // Update if there's a specific controller/action for biotx.com
//        }
//    }

//    await next();
//});

// 測試3
//var exosomePort = builder.Configuration["DomainPorts:ExosomePort"];
//var biotxPort = builder.Configuration["DomainPorts:BioTxPort"];

//var exosomeDomain = builder.Configuration["DomainRouting:ExosomeDomain"];
//var biotxDomain = builder.Configuration["DomainRouting:BioTxDomain"];

//app.Use(async (context, next) =>
//{
//    var host = context.Request.Host;

//    if (app.Environment.IsStaging()) // 測試機
//    {
//        if (host.Port.ToString() == exosomePort && context.Request.Path == "/")
//        {
//            context.Response.Redirect("/ExosomeFoundry/Index", permanent: false);
//            return;
//        }
//        else if (host.Port.ToString() == biotxPort && context.Request.Path == "/")
//        {
//            context.Request.Path = "/Home/Index";
//        }
//    }
//    else if (app.Environment.IsProduction()) // 正式機
//    {
//        if ((host.Host == exosomeDomain || host.Host == $"www.{exosomeDomain}") && context.Request.Path == "/")
//        {
//            context.Response.Redirect("/ExosomeFoundry/Index", permanent: false);
//            return;
//        }
//        else if ((host.Host == biotxDomain || host.Host == $"www.{biotxDomain}") && context.Request.Path == "/")
//        {
//            context.Request.Path = "/Home/Index";
//        }
//    }

//    await next();
//});

// 測試 4
app.Use(async (context, next) =>
{
    var host = context.Request.Host;
  
    var exosomeDomain = builder.Configuration["DomainRouting:ExosomeDomain"];
    var biotxDomain = builder.Configuration["DomainRouting:BioTxDomain"];
    // Check if the request is coming from port 7292
    if (context.Request.Host.Port == 7292  || context.Request.Host.Port == 8089)
    {
        // Map to exosomefoundry-specific route
        if (context.Request.Path == "/")
        {
            context.Response.Redirect("/ExosomeFoundry/Index", permanent: false);
        }
    }
    else if (context.Request.Host.Port == 7291 || context.Request.Host.Port == 8088 )
    {
        // This is the default setup for biotx.com; no path modification needed unless there’s a specific landing route
        if (context.Request.Path == "/")
        {

            context.Request.Path = "/Home/Index"; // Update if there's a specific controller/action for biotx.com
        }
    }

    await next();
    //Console.WriteLine("host:", host);
    //Console.WriteLine("exosomeDomain:", exosomeDomain);
    //Console.WriteLine("biotxDomain:", biotxDomain);
});





app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
