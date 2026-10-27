using AutoMapper;
using HR.Application.Dtos.AppDtos;
using HR.Application.Helpers;
using HR.Application.Interfaces;
using HR.Domain.Models.Authorization;
using HR.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
 public class JwtTokenService ( UserManager<AppUser> userManager,JwtOptions jwtOptions,IMapper mapper,IBaseRepository<RefreshToken> refreshTokenRepo ) : IJwtTokenService
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
   var signCredintials = new SigningCredentials (securityKey,SecurityAlgorithms.HmacSha256);

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
   };

   AppUser? UserActiveRefreshTokens = await userManager.Users.Include (u => u.RefreshTokens).FirstOrDefaultAsync (u => u.RefreshTokens.Any (t => t.RevokedOn==null));

   RefreshToken? activeRefreshToken = UserActiveRefreshTokens!.RefreshTokens.SingleOrDefault (t => t.IsActive);

   if (activeRefreshToken is not null)
   {
    AuthUser.RefreshToken=activeRefreshToken!.TokenRefreshed;
    AuthUser.RefreshTokenExpiration=activeRefreshToken.ExpiresAtUtc.ToLocalTime ();
   }
   else
   {
    var refreshToken = await GenerateRefreshTokenAsync (user);
    AuthUser.RefreshToken=refreshToken.TokenRefreshed;
    AuthUser.RefreshTokenExpiration=refreshToken.ExpiresAtUtc.ToLocalTime ();
   }
   AuthUser.Message=user.UserName+" is Logged in Successfully";
   return AuthUser;
  }


  public async Task<RefreshToken> GenerateRefreshTokenAsync ( AppUser user )
  {
   var randomNumber = new byte [32];
   using var generator = RandomNumberGenerator.Create ();
   generator.GetBytes (randomNumber);

   RefreshToken newRefreshToken = new ()
   {
    UserId=user.Id,
    TokenRefreshed=Convert.ToBase64String (randomNumber),
    ExpiresAtUtc=DateTime.UtcNow.AddDays ((double)jwtOptions.RefreshExpiresAt!),
    CreatedAtUtc=DateTime.UtcNow
   };
   await refreshTokenRepo.CreateAsync (newRefreshToken);
   return newRefreshToken;
  }

  public async Task<AuthDto> RefreshTokenAsync ( string token )
  {
   var authModel = new AuthDto ();
   var user = await userManager.Users.Include (u => u.RefreshTokens).SingleOrDefaultAsync (u => u.RefreshTokens.Any (t => t.TokenRefreshed==token));

   if (user==null)
   {
    authModel.IsAuthenticated=false;
    authModel.Message="Invalid token.";
    return authModel;
   }

   var refreshToken = user.RefreshTokens.Single (t => t.TokenRefreshed==token);
   if (!refreshToken.IsActive)
   {
    authModel.IsAuthenticated=false;
    authModel.Message="InActive Token";
    return authModel;
   }
   refreshToken.RevokedOn=DateTime.UtcNow;
   var LastTokenIsRevoked = await RevokeTokenAsync (token);

   var newRefreshToken = await GenerateRefreshTokenAsync (user);

   return new AuthDto
   {
    IsAuthenticated=true,
    Token=new JwtSecurityTokenHandler ().WriteToken (await CreateNewJwtSecurityToken (user)),
    Roles= [.. (await userManager.GetRolesAsync (user))],
    Message="Token refreshed successfully.",
    User=mapper.Map<UserRequest> (user),
    RefreshToken=newRefreshToken.TokenRefreshed,
    RefreshTokenExpiration=newRefreshToken.ExpiresAtUtc.ToLocalTime ()
   };
  }

  public async Task<bool> RevokeTokenAsync ( string token )
  {
   var userToken = await userManager.Users.Include (u => u.RefreshTokens).FirstOrDefaultAsync (u => u.RefreshTokens.Any (t => t.TokenRefreshed==token));
   if (userToken==null)
    return false;

   var refreshToken = userToken.RefreshTokens.Single (t => t.TokenRefreshed==token);
   if (!refreshToken!.IsActive)
    return false;

   refreshToken.RevokedOn=DateTime.UtcNow;
   await refreshTokenRepo.PartialUpdateAsync (refreshToken,t => t.RevokedOn!);
   return true;
  }

 }
}
