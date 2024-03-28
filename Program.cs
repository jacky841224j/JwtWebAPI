using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "JWT Demo����",
        Description = "JWT Demo ���ұM��",
        TermsOfService = new Uri("https://github.com/jacky841224j"),
        Contact = new OpenApiContact
        {
            Name = "jacky841224j",
            Email = string.Empty,
            Url = new Uri("https://github.com/jacky841224j"),
        }
    });

    options.AddSecurityDefinition("Bearer",
    new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer xxxxxxxxxxxxxxx\"",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

builder.Services.AddAuthentication().AddJwtBearer("Bearer", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
        
                ValidateIssuer = false, //�����ҵo��̡]Issuer�^�������ѧO
                RequireExpirationTime = true, //�n�D�O�P(Token)�����㦳�L���ɶ�
                ValidateAudience = false, //�������[���]Audience�^
                ValidateIssuerSigningKey = false, //�����ҵo���ñ�W���_
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ba8347e6-3504-460d-979e-7a4555c890ee"))
            };
        }
    );


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
