using GT.Application.Service;
using GT.Application.Validator;
using GT.Domain.Entites;
using GT.Domain.Interfaces;
using GT.Infra.Context;
using GT.Infra.Repository;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.IdentityModel.Tokens;              
using System.Text;                                 
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURAÇÃO DO BANCO ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- 2. CONFIGURAÇÃO DO JWT ---
var key = Encoding.UTF8.GetBytes(GT.Api.Settings.Secret);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };

    x.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine("=================================");
            Console.WriteLine("ERRO DE TOKEN: " + context.Exception.Message);
            Console.WriteLine("=================================");
            return Task.CompletedTask;
        }
    };
});


builder.Services.AddScoped<IBaseRepository<User>, UserRepository>();
builder.Services.AddScoped<IBaseRepository<Tarefa>, TarefaRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITarefaService, TarefaService>();
builder.Services.AddScoped<IValidator<User>, UserValidator>();
builder.Services.AddScoped<IValidator<Tarefa>, TarefaValidator>();


builder.Services.AddControllers()
    .AddFluentValidation(config => 
    {
        config.RegisterValidatorsFromAssemblyContaining<UserValidator>();
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Insira o token JWT desta maneira: Bearer {seu token}"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization(); 

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        if (!context.Users.Any())
        {
            Console.WriteLine("Banco vazio detectado. Criando usuário ADMIN...");

            var adminUser = new User
            {
                Username = "admin",
                Email = "admin@sistema.com",
                Password = BCrypt.Net.BCrypt.HashPassword("admin123", workFactor: 12) 
            };

            context.Users.Add(adminUser);
            context.SaveChanges();
            
            Console.WriteLine("Usuário ADMIN criado com sucesso! (User: admin / Pass: admin123)");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erro ao criar usuário Admin: {ex.Message}");
    }
}

app.Run();