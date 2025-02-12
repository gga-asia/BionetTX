using BionetTX.Services.IServices;
using BionetTX.Services;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Http.Headers;
using Microsoft.Extensions.Hosting;
using BionetTX.Middlewares;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// �W�[MVC�A��, �w�]��
builder.Services.AddControllersWithViews();

// �ۦ�W�[
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
// �N appsettings.json �[�J�t�m , Configuration �O���ذt�m
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

var app = builder.Build();

// ���Umiddleware
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
    // ��������ĳ�B�D���W�١B��
    var scheme = context.Request.Scheme; // http �� https
    var host = context.Request.Host; // ��X�����Glocalhost:7292
    var hostName = context.Request.Host.Host; // �Ȩ��o����W�� ��X�Glocalhost
    var port = context.Request.Host.Port; // ���o�� ��X�G7292

    // ����]�w�ɪ����Ҹ�T�P����t�m
    var environment = builder.Configuration["Environment"];
    var exosomeDomain = builder.Configuration["DomainRouting:ExosomeDomain"];
    var biotxDomain = builder.Configuration["DomainRouting:BioTxDomain"];
    

    // Check if the request is coming from  port or domain
    // �w�q�����޿�
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

    // ���ҧP�_
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
