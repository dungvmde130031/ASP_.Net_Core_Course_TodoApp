using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Service.Database;
using TodoApp.Business;
using System.Security.Claims;
using TodoApp.WebMvc.PolicyRequirements;
using Microsoft.AspNetCore.Authorization;
using Service.UnitOfWork;
using Service.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//  Not use Entity Framework
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddCookie(options =>
  {
      options.Cookie.Name = "MyLoginCookie";
      options.AccessDeniedPath = "/Account/AccessDenied";
      options.LoginPath = "/Account/Login";
      options.LogoutPath = "/Account/Logout";
      options.ExpireTimeSpan = TimeSpan.FromMilliseconds(20);
      options.SlidingExpiration = true;
  });

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;

    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;

    options.User.RequireUniqueEmail = true;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireRole("SuperAdmin");
        policy.RequireClaim(ClaimTypes.Role, "SuperAdmin");
        policy.RequireClaim(ClaimTypes.Role, "Admin");
    });

    options.AddPolicy("AdminOnlyNew", policy =>
    {
        // 1 Policy xu ly cho nhieu Requirement
        // 1 Requirement duoc xy ly boi nhieu Handler
        policy.Requirements.Add(new AdminOnlyRequirement("Admin", "DungVM2"));
        //policy.Requirements.Add(new GenderRequirement("Men"));
    });
});

builder.Services.AddSingleton<IAuthorizationHandler, AdminOnlyRequirementHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, GenderRequirementHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

builder.Services.AddScoped<ITaskBusiness, TaskBusiness>();

//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .RequireAuthorization();

app.Run();

