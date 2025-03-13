using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Exstensions;

public static class IdentityServiceExstentions
{
    public static IServiceCollection AddIdentityService(this IServiceCollection service, IConfiguration config)
    {
        service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot find token key");
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });

            return service;
    }
}