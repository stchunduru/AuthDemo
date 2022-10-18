using AuthDemo.Models;
using AuthDemo.Repositories;
using AuthDemo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with jwt Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IMovieService, MovieService>();
builder.Services.AddSingleton<IUserService, UserService>();


var app = builder.Build();
app.UseSwagger();

app.MapGet("/", () => "Hello World!");

app.MapPost("/login",
    (UserLogin user, IUserService service) => Login(user, service))
    .Accepts<UserLogin>("application/json")
    .Produces<string>();

app.MapPost("/create",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
(Movie movie, IMovieService service) => Create(movie, service));

app.MapGet("/get",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Standard, Administrator")]
(int id, IMovieService service) => Get(id, service));

app.MapGet("/list",
    (IMovieService service) => List(service));

app.MapPut("/update",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Standard, Administrator")]
(Movie movie, IMovieService service) => Update(movie, service));

app.MapDelete("/delete",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Standard, Administrator")]
(int id, IMovieService service) => Delete(id, service));

IResult Login(UserLogin user, IUserService service)
{
    var loggedUser = service.Get(user);
    if (loggedUser == null) return Results.NotFound();

    var claims = new[]
    {
        new Claim(ClaimTypes.NameIdentifier, loggedUser.Name),
        new Claim(ClaimTypes.Email, loggedUser.EmailAddress),
        new Claim(ClaimTypes.Name, loggedUser.Name),
        new Claim(ClaimTypes.Role, loggedUser.Role)
    };

    var token = new JwtSecurityToken
    (
        issuer: builder.Configuration["Jwt:Issuer"],
        audience: builder.Configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddDays(60),
        signingCredentials: new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            SecurityAlgorithms.HmacSha256)
    );

    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
    return Results.Ok(tokenString);
}

IResult Create(Movie movie, IMovieService service)
{
    var result = service.Create(movie);
    return Results.Ok(result);
}

IResult Update(Movie movie, IMovieService service)
{
    var result = service.Update(movie);
    return Results.Ok(result);
}

IResult Delete(int id, IMovieService service)
{
    var result = service.Delete(id);
    return Results.Ok(result);
}

IResult Get(int id, IMovieService service)
{
    var result = service.Get(id);
    return Results.Ok(result);
}

IResult List(IMovieService service)
{
    var movies = service.List();
    return Results.Ok(movies);
}

app.UseSwaggerUI();

app.Run();
