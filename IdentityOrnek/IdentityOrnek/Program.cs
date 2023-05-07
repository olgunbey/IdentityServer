using IdentityOrnek.CustomValidators;
using IdentityOrnek.Localization;
using IdentityOrnek.Models;
using IdentityOrnek.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("mydb")));
builder.Services.AddIdentity<AppUser, AppRole>(x => {
    x.User.RequireUniqueEmail = true; //ayný email olamaz.
    x.Password.RequireNonAlphanumeric = false; //þifrede alfasayýsal karakterler olmasý zorunlu 
    x.Password.RequiredLength = 7; //en az 7 karakter
    x.Password.RequireDigit = true; //rakam gerekli
    x.Password.RequireUppercase = true; //büyük harf zorunlu
    x.Password.RequireLowercase = true; //küçük harf zorunlu
    //x.User.AllowedUserNameCharacters = "123456789olgunhaluk"; // karakterlerin tektek belirtildigi sadece bu karakterlerin kullanýlacaðýný gösterir
    x.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3); //3 dakika boyunca kilitlenecegini belirtiyoruz.
    x.Lockout.MaxFailedAccessAttempts = 3; //3 kere hakkýný oldugunu belirtiyoruz ardýndan kilitlenecek.
}).AddPasswordValidator<PasswordValidate>().AddUserValidator<UserValidate>().AddErrorDescriber<LocalizationIdentityErrorDescriber>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService,EMailService>();

builder.Services.ConfigureApplicationCookie(opt =>
{
    var cookie = new CookieBuilder();
    cookie.Name = "giriscookie";
    opt.LoginPath = new PathString("/Home/Signin");
    opt.LogoutPath = new PathString("/Member/Logout"); //logout'un oldugu sayfa verilir
    opt.Cookie = cookie;
    opt.ExpireTimeSpan = TimeSpan.FromDays(60);
    opt.SlidingExpiration = true;
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
app.UseAuthentication();
app.UseAuthorization();




app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
