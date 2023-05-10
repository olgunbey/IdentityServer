
# IdentityServer

Bu proje basit Cookie Authorize-Authentication uygulamasını , şifre unuttum uygulamalarını, kullanıcı giriş çıkışlarını kapsar.

## IdentityServer Başlarken

Projenize IdentityServer kurmak için aşşağıdaki kodu Package Manager yerine yazın. 

```bash
  NuGet\Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore -Version 7.0.5

```

  
## IdentityServer Basit Ayarlar

 Program.cs dosyasında AddIdentity<AppUser,AppRole>() Fonksiyonunu kullanarak
 kullanıcı tarafından gelen Email, Password, gibi yerleri kısıtlayabiliriz.
 Burada kendimiz'de şifre veya email için bir kural oluşturabiliriz. Bu kuralları class içinde IPasswordValidator<AppUser> implement ederek kullanabiliriz.
 Örneğin bir kullanıcının adı, şifre içinde olamaz.
  

```javascript
builder.Services.AddIdentity<AppUser,AppRole>(x=>{
 x.User.RequireUniqueEmail = true,
 x.Password.RequiredLength=7,
 x.Password.RequireDigit = true,
 x.Password.RequireUppercase = true,
 x.Password.RequireLowercase = true,    
 x.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3),
 x.Lockout.MaxFailedAccessAttempts = 3
}).AddPasswordValidator<PasswordValidate>().
AddUserValidator<UserValidate>().
AddErrorDescriber<LocalizationIdentityErrorDescriber>().AddEntityFrameworkStores<AppDbContext>().
AddDefaultTokenProviders();


```






  