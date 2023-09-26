using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Phonemax.dataaccess.Data;
using Phonemax.dataaccess.Repository.Irepository;
using Phonemax.dataaccess.Repository;
using Microsoft.AspNetCore.Identity.UI.Services;
using Phonemax.uitility;
using Stripe;
using Phonemax.Models.Twilio;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//   .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<Iunitofwork, unitofwork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<Isms, sms>();

builder.Services.Configure<Stripesettings>(builder.Configuration.GetSection("Stripe"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSetting"));
builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("Twilio"));
builder.Services.Configure<Twilioset>(builder.Configuration.GetSection("TwilioSettings"));



builder.Services.AddRazorPages();
builder.Services.ConfigureApplicationCookie(option =>
{
    option.LoginPath = "/Identity/Account/Login";
    option.AccessDeniedPath = "/Identity/Account/AccessDenied";
    option.LogoutPath = "/Identity/Account/Logout";
});
builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = "612864600430677";
    options.AppSecret = "01a35994c8ceb90f4e198920f0f88f61";
});
builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = "500996044302-356c3bc9p4kiv3pjiemqiotmkv9hov1j.apps.googleusercontent.com";
    options.ClientSecret = "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";
});
builder.Services.AddAuthentication().AddTwitter(options =>
{
    options.ConsumerKey = "hwleXBLkGk7LFZTpbID7ypW6Z";
    options.ConsumerSecret = "92GHcBbpET7EpQeJf69xbvH7lzwaCSGTSPXg1MO4h5flUteUgf";
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe")["Secretkey"];

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
