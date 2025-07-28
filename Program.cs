using Lab_App.Models;
using Lab_App.services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// إضافة الخدمات إلى الحاوية
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ✅ تكوين Swagger لدعم التوثيق عبر التوكن (JWT)
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "أدخل التوكن بهذا الشكل: Bearer {your_token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// ✅ إعداد قاعدة البيانات
builder.Services.AddDbContext<myDBcontext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("con"))
);

// ✅ إعداد الهوية (Identity) مع EF Core
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<myDBcontext>();

// ✅ إعداد المصادقة عبر JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"], // ✅ تصحيح الخطأ الإملائي
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
        ValidateLifetime = true, // ✅ التأكد من أن التوكن لم ينتهِ
        ValidateIssuerSigningKey = true
    };
});

// ✅ إعداد CORS للسماح بالطلبات من أي مصدر
builder.Services.AddCors(options =>
{
    options.AddPolicy("corsPolicy", policy =>
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin()
    );
});

builder.Services.AddScoped<LicenseService>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    options.JsonSerializerOptions.WriteIndented = true; // Optional: makes JSON output more readable
});


var app = builder.Build();

// ✅ تفعيل Swagger فقط في وضع التطوير
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corsPolicy"); // 🔥 تأكد من تشغيل CORS قبل التوثيق
app.UseAuthentication();    // ✅ يجب أن يكون قبل UseAuthorization
app.UseAuthorization();

app.MapControllers();
app.Run();
