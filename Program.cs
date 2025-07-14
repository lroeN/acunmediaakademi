using InventoryManagement.Data;
using InventoryManagement.Models.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Net.Mail;
using System.Net;
using System.Text;
using InventoryManagement.Interfaces;


namespace InventoryManagement;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddOpenApi("v1", options =>
        {
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });

        // DbContext
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        // JWT Ayarları
        var jwtSettings = builder.Configuration.GetSection("Jwt");
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!))
                };
            });
        
        //Swagger
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "InventoryManagement Api",
                Version = "v1",
                Description = "InventoryManagement uygulaması için API dokümantasyonu"
            });
            c.EnableAnnotations();
        });

        // FluentEmail
        var smtpSettings = builder.Configuration.GetSection("SmtpSettings").Get<SmtpSettings>();
        builder.Services
            .AddFluentEmail(smtpSettings.FromEmail, smtpSettings.FromName)
            .AddRazorRenderer()
            .AddSmtpSender(new SmtpClient(smtpSettings.Host, smtpSettings.Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password),
            });

        builder.Services.AddTransient<IEmailService, EmailService>();

        
        builder.Services.AddCors(opt =>
        {
            opt.AddDefaultPolicy(policy =>
            {
                policy
                    .WithOrigins()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddRazorPages();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();     
        builder.Services.AddOpenApi();

       
        var app = builder.Build();

        /*
        if (app.Environment.IsDevelopment())
        {
            
        }
        */
        
        app.UseSwagger();
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "InventoryManagement v1"); });
        app.MapOpenApi();
        app.UseHttpsRedirection();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();
        
        //Scalar
        app.MapScalarApiReference(opt =>
        {
            opt.WithTitle("InventoryManagement.com")
                .WithTheme(ScalarTheme.DeepSpace)
                .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
        });
        
        app.MapControllers();
        app.Run();
    }
}
