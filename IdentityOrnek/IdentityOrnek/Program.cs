using IdentityOrnek.Areas.ClaimsProvider;
using IdentityOrnek.CustomValidators;
using IdentityOrnek.Localization;
using IdentityOrnek.Models;
using IdentityOrnek.Permissions;
using IdentityOrnek.Requirements;
using IdentityOrnek.Seeds;
using IdentityOrnek.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

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



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AnkaraPolicy", policy =>
    {
          policy.RequireClaim("city", "Ankara");
    });
    options.AddPolicy("ExchangePolicy", policy =>
    {
        policy.AddRequirements(new ExchangeExpireRequirement());
    });
    options.AddPolicy("AgePolicy", policy =>
    {
        policy.AddRequirements(new AgeExpireRequirement() { ThresholdAge=18});
    });
    options.AddPolicy("OrderPermissionReadAndDelete", policy =>
    {
        policy.AddRequirements(new OrderPermissionReadAndDeleteRequirement());
    });


});

builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));
builder.Services.AddScoped<IAuthorizationHandler, ExchangeExpireRequirementHandler>();
builder.Services.AddScoped<IClaimsTransformation, UserClaimsProvider>();
builder.Services.AddScoped<IAuthorizationHandler, AgeExpiredRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, OrderPermissionReadAndDeleteRequirementHandler>();

builder.Services.Configure<SecurityStampValidatorOptions>(opt =>
{
    opt.ValidationInterval=TimeSpan.FromMinutes(30); //her 30 dk da bir arkada securitystamp cookiedeki deðeriyle veritabanýndaki deðeri kontrol edilir. Farklýlýk varsa otomatikmen logout yapýlýr 
});
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService,EMailService>();

builder.Services.ConfigureApplicationCookie(opt =>
{
    var cookie = new CookieBuilder();
    cookie.Name = "giriscookie";
    opt.LoginPath = new PathString("/Home/Signin");
    opt.LogoutPath = new PathString("/Member/Logout"); //logout'un oldugu sayfa verilir
    opt.Cookie = cookie;
    opt.AccessDeniedPath = new PathString("/Member/AccessDenied");
    opt.ExpireTimeSpan = TimeSpan.FromDays(60);
    opt.SlidingExpiration = true;
});


var app = builder.Build();

using (var scope=app.Services.CreateScope()) //uygulama ayaga kalkarken 1 kere oluþturulacak
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
   await PermissionSeed.PermissionSeeds(roleManager);

}

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
