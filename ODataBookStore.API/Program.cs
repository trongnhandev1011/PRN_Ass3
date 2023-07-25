using ODataBookStore.API.Extensions;
using ODataBookStore.DataAccess.Data;
using ODataBookStore.DataAccess.Repositories.AccountRepository;
using ODataBookStore.DataAccess.Repositories.BookRepository;
using ODataBookStore.DataAccess.Repositories.PressRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using System.Text;
using static System.Net.WebRequestMethods;
using ODataBookStore.BusinessObject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var MY_SITE = "https://localhost:7261";

ODataConventionModelBuilder oDataBuilder = new ODataConventionModelBuilder();
oDataBuilder.EntitySet<Book>("books");
oDataBuilder.EntitySet<Press>("presses");
oDataBuilder.EntitySet<Account>("accounts");

builder.Services.AddControllers().AddOData(option => option.Select().Filter().Count().OrderBy().Expand().SetMaxTop(100).AddRouteComponents("odata", oDataBuilder.GetEdmModel()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddScoped<JwtService>();

var configuration = builder.Configuration;

builder.Services.AddDbContext<ODataBookStoreContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DbConnectionString"),
        builder => builder.MigrationsAssembly(typeof(ODataBookStoreContext).Assembly.FullName));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    // Validate JWT Token
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new ArgumentException("Jwt:Key is required")))
    };
}
);
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "BookManagment API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MY_SITE,
                     policy =>
                     {
                         policy.AllowAnyOrigin()
                         .AllowAnyHeader()
                         .AllowAnyMethod();
                     });
});

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IPressRepository, PressRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseODataBatching();

app.UseRouting();

app.Use((context, next) =>
{
    var endpoint = context.GetEndpoint();
    if (endpoint == null)
    {
        return next(context);
    }

    IEnumerable<string> templates;
    IODataRoutingMetadata metadata = endpoint.Metadata.GetMetadata<IODataRoutingMetadata>();
    if (metadata != null)
    {
        templates = metadata.Template.GetTemplates();
    }

    return next(context);
});

app.UseHttpsRedirection();
app.UseCors(MY_SITE);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
