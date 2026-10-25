using AutoMapper;
using HR.Application.Dtos.AppDtos;
using HR.Application.Helpers;
using HR.Application.Interfaces;
using HR.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HR.Application.Services
{
 /// <summary>
 /// Service for generating JWT tokens for authenticated users.
 /// </summary>
 public class JwtTokenService ( UserManager<AppUser> userManager,JwtOptions jwtOptions,IMapper mapper ) : IJwtTokenService
 {
  public async Task<JwtSecurityToken> CreateNewJwtSecurityToken ( AppUser user )
  {
   var userClaims = (await userManager.GetClaimsAsync (user))?.ToList ()?? [ ];
   if (userClaims.Count==0)
   {
    // local default claims for the token only — do NOT persist to the user store here
    userClaims=
        [
         new ("UserName", user.UserName!),
         new ("FullName", user.FullName ?? string.Empty),
         new (ClaimTypes.NameIdentifier, user.Id),
         new (ClaimTypes.Email, user.Email ?? string.Empty)
        ];
   }
   ;
   var securityStamp = await userManager.GetSecurityStampAsync (user);
   if (!string.IsNullOrEmpty (securityStamp))
    userClaims.Add (new Claim ("SecurityStamp",securityStamp));
   var userRoles = await userManager.GetRolesAsync (user);
   var roleClaims = new List<Claim> ();
   foreach (var role in userRoles)
   {
    roleClaims.Add (new Claim (ClaimTypes.Role,role));
   }

   var tokenClaims = new [ ] { new Claim (JwtRegisteredClaimNames.Jti,Guid.NewGuid ().ToString ()) }
                    .Union (userClaims)
                    .Union (roleClaims);

   var securityKey = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (jwtOptions.Key!));
   var signCredintials = new SigningCredentials (securityKey,SecurityAlgorithms.HmacSha512);

   var jwtSecurityToken = new JwtSecurityToken (
       issuer: jwtOptions.Issuer,
       audience: jwtOptions.Audience,
       claims: tokenClaims,
       // use UTC now for expiry
       expires: DateTime.UtcNow.AddMinutes ((double)jwtOptions.ExpireAt!),
       signingCredentials: signCredintials
   );

   return jwtSecurityToken;
  }

  public string GenerateRefreshToken ( )
  {
   return Convert.ToBase64String (RandomNumberGenerator.GetBytes (64));

  }

  public async Task<AuthDto> CreateToken ( AppUser user )
  {
   JwtSecurityToken jwtSecurityToken = await CreateNewJwtSecurityToken (user);
   AuthDto AuthUser = new ()
   {
    User=mapper.Map<UserRequest> (user),
    TokenExpiresIn=jwtSecurityToken.ValidTo,
    IsAuthenticated=true,
    Token=new JwtSecurityTokenHandler ().WriteToken (jwtSecurityToken),
    Roles= [.. (await userManager.GetRolesAsync (user))],
    Message=user.UserName+" is Logged in Successfully"
   };
   return AuthUser;
  }

  public string HashRefreshToken ( string token )
  {
   return Convert.ToHexString (SHA512.HashData (Encoding.UTF8.GetBytes (token)));
  }
 }
}
