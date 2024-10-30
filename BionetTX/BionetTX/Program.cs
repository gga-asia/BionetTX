using BionetTX.Services.IServices;
using BionetTX.Services;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net.Http.Headers;

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
