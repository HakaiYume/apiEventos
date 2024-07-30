using ApiEventos.Data;
using ApiEventos.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Cors
var MyOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyOrigins, policy => {
        policy.WithOrigins("*");
        policy.AllowAnyHeader();
        policy.WithMethods("GET", "POST", "PUT", "DELETE");
        });
});

//JTW
var key = builder.Configuration["Jwt:Key"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>{
        options.TokenValidationParameters = new TokenValidationParameters(){
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key is not null? key : "")),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//DBcontext
builder.Services.AddSqlServer<DwiApieventosContext>(builder.Configuration.GetConnectionString("BDConection"));

//Services
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<IInvitacioneService, InvitacioneService>();
builder.Services.AddScoped<IInvitadoEspecialService, InvitadoEspecialService>();
builder.Services.AddScoped<IRegistroEventoService, RegistroEventoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ILoginService, LoginService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//cords
app.UseCors(MyOrigins);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();