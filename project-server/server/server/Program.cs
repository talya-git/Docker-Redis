using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using server.Auth.Jwt;
using server.BLL;
using server.BLL.Interfaces;
using server.DAL;
using server.DAL.Interfaces;
using System.Text;
using WebApplication1.BLL;
using WebApplication1.BLL.Interfaces;
using WebApplication1.DAL;
using WebApplication1.DAL.Interfaces;
using WebApplication1.DAL;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

// =======================
// 1. AutoMapper
// =======================
builder.Services.AddAutoMapper(typeof(MappingProfile));

// =======================
// 2. Dependency Injection (BLL + DAL)
// =======================
builder.Services.AddScoped<IcustomerBLL, CustomerBLL>();
builder.Services.AddScoped<IcustomerDAL, CustomerDAL>();
builder.Services.AddScoped<IgiftBLL, GiftBLL>();
builder.Services.AddScoped<IgiftDAL, GiftDAL>();
builder.Services.AddScoped<IdonorBLL, DonorBLL>();
builder.Services.AddScoped<IdonorDAL, DonorDAL>();
builder.Services.AddScoped<IAuthBLL, AuthBLL>();
builder.Services.AddScoped<IAuthDAL, AuthDAL>();
builder.Services.AddScoped<ICustomerDatailsDAL, CustomerDatailsDAL>();
builder.Services.AddScoped<ICustomerDatailsBLL, CustomerDatailsBLL>();

builder.Services.AddScoped<JwtTokenGenerator>();

// =======================
// 3. Database Context
// =======================
builder.Services.AddDbContext<LotteryContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ??
   "Data Source=SRV2\\PUPILS;Initial Catalog=talya;Integrated Security=True;Encrypt=True;Trust Server Certificate=True"));

// =======================
// 4. CORS Policy
// =======================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// =======================
// 5. JWT Authentication
// =======================
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "YourFallbackSecretKeyHere_MustBeLong");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// =======================
// 6. Swagger with Authorize Button
// =======================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Lottery API", Version = "v1" });

    // הגדרת האפשרות להכניס טוקן ב-Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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

var app = builder.Build();

// =======================
// 7. Middleware Pipeline
// =======================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AngularPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();