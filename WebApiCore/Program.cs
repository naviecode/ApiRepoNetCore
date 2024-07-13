﻿using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ShopApi.Data;
using ShopApi.Data.Infrastructure;
using ShopApi.Data.Repositories;
using ShopApi.Service.Abstractions;
using ShopApi.Service.Helpers;
using ShopApi.Service.Models.AuthDto;
using ShopApi.Service.Models.ProductCategoryDto;
using ShopApi.Service.Models.ProductDto;
using ShopApi.Service.Models.UserDto;
using ShopApi.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));


// Đăng ký AutoMapper
//builder.Services.AddScoped<IMapper, Mapper>();
builder.Services.AddAutoMapper(typeof(ProductMapperProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ProductCategoryMapperProfile).Assembly);
builder.Services.AddAutoMapper(typeof(UserMapperProfile).Assembly);


//Add Services
builder.Services.AddScoped<IDbFactory, DbFactory>();
builder.Services.AddScoped<IDisposable, Disposable>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductTagRepository, ProductTagRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IErrorRepository, ErrorRepository>();
builder.Services.AddScoped<IPostCategoryRepository, PostCategoryRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserTokenRepository, UserTokenRepository>();
builder.Services.AddScoped<IUploadFileService, UploadFileService>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddScoped<IServiceManager, ServiceManager>();


builder.Services.AddHttpContextAccessor();

//Add DbContext
builder.Services.AddDbContext<WebShopDbContext>(options =>
{
    var connectString = builder.Configuration.GetConnectionString("DataBase");
    options.UseSqlServer(connectString);
});

//Custom UI swagger mix JWT
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication();   // Phuc hoi thong tin dang nhap(xac thuc)
app.UseAuthorization();   // Phuc hoi thong tin ve quyen cua user

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.UseCors(MyAllowSpecificOrigins);
app.Run();
