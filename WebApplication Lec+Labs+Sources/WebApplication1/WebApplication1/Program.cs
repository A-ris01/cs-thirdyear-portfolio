// Program.cs
using System.Text;
using WebApplication1.Models;
using WebApplication1.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
//   SECTION 1: SERVICES  (builder.Services.*)
//   Everything here runs ONCE at startup.
//   We are registering capabilities into the DI container.
// ============================================================

builder.Services.AddControllers();

// --- 1A: Bind the JwtSettings section from appsettings.json ---
// This lets you inject IOptions<JwtSettings> anywhere in the app.
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings")
);

// --- 1B: Register our custom token service ---
builder.Services.AddScoped<ITokenService, TokenService>();

// --- 1C: Register Authentication & configure JWT Bearer ---
// AddAuthentication() registers the auth system.
// AddJwtBearer() tells it HOW to validate incoming tokens.
builder.Services.AddAuthentication(options =>
{
    // Set JWT Bearer as the default scheme for both
    // authenticating (reading the token) and challenging (returning 401)
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // These parameters MUST match what you used to CREATE the token
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,            // Check the 'iss' claim
        ValidateAudience = true,          // Check the 'aud' claim
        ValidateLifetime = true,          // Reject expired tokens
        ValidateIssuerSigningKey = true,  // Verify the signature

        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                builder.Configuration["JwtSettings:SecretKey"]!
            )
        ),

        // Important: By default, .NET adds 5 minutes clock skew tolerance.
        // Set to zero for exact expiration enforcement.
        ClockSkew = TimeSpan.Zero
    };
});

// --- 1D: Register Authorization ---
// AddAuthorization handles the PERMISSION logic (what you can DO).
// AddAuthentication (above) handles WHO you are.
builder.Services.AddAuthorization();

// --- 1E: Configure Swagger WITH the Authorize lock icon ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "JWT Auth Demo", Version = "v1" });

    // Step 1: Define the security scheme (tells Swagger about Bearer tokens)
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer {your_token}\n\nExample: Bearer eyJhbGci..."
    });

    // Step 2: Apply this scheme globally — all endpoints show the lock icon
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"  // Must match the name in AddSecurityDefinition
                }
            },
            Array.Empty<string>()
        }
    });
});

// ============================================================
//   app.Build() — This is the WALL between Services & Middleware.
//   You CANNOT call builder.Services after this line.
//   You CANNOT call app.Use* before this line.
// ============================================================
var app = builder.Build();

// ============================================================
//   SECTION 2: MIDDLEWARE  (app.Use* / app.Map*)
//   Everything here runs on EVERY REQUEST, in ORDER.
//   Think of it as a chain of interceptors.
//
//   THE CORRECT ORDER IS CRITICAL:
//   Request enters → [Swagger] → [Routing] → [Authentication] → [Authorization] → [Controller]
// ============================================================

if (app.Environment.IsDevelopment())
{
    // Only expose Swagger in Development mode
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ⚠️ ORDER MATTERS — These two lines MUST be in this exact order:

// FIRST: Authentication — "Who are you?" 
// Reads the Authorization header, validates the JWT, and populates HttpContext.User
app.UseAuthentication();

// SECOND: Authorization — "Are you allowed to do this?"
// Checks [Authorize] attributes AFTER we know who the user is.
// If you swap these, [Authorize] runs before the user is identified → always 401!
app.UseAuthorization();

app.MapControllers();

app.Run();