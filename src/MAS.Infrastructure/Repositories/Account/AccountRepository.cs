using AutoMapper;
using MAS.Core.Dtos.Incoming.Account;
using MAS.Core.Dtos.Outcoming.Account;
using MAS.Core.Interfaces.Repositories.Account;
using MAS.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MAS.Infrastructure.Repositories.Account;

public class AccountRepository : BaseRepository, IAccountRepository
{
    private UserManager<IdentityUser> userManager;
    private RoleManager<IdentityRole> roleManager;
    private readonly IConfiguration configuration;
    private readonly IConfigurationSection jwtSettings;

    public AccountRepository(
        IMapper mapper,
        AppDbContext context,
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration) : base(mapper, context)
    {
        this.userManager = userManager;
        this.roleManager = roleManager;
        this.configuration = configuration;
        jwtSettings = this.configuration.GetSection("Jwt");
    }

    /// <summary>
    /// Login user with Google Account.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<AuthenResult> LoginWithGoogleAsync(GoogleLoginRequest request)
    {
        var payload = PayloadInfo(request.IdToken);

        if (payload is null) {
            return new AuthenResult {
                Errors = new List<string>() { "IdToken not valid." }
            };
        }

        var info = new UserLoginInfo(request.ProviderName, payload.Sub, request.ProviderName);

        var user = await userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

        if (user is null) {
            user = await userManager.FindByEmailAsync(payload["email"].ToString());

            if (user is null) {
                user = new IdentityUser() {
                    Email = payload["email"].ToString(),
                    UserName = payload["email"].ToString(),
                    EmailConfirmed = Convert.ToBoolean(payload["email_verified"].ToString())
                };

                var result = await userManager.CreateAsync(user);
                await SetRoleForUser(user, RoleConstants.User);
                await userManager.AddLoginAsync(user, info);

                if (result.Succeeded) {
                    var isMentor = CheckIsMailMentor(payload["email"].ToString());
                    if (isMentor) {
                        var masUser = new Core.Entities.MasUser {
                            Id = Guid.NewGuid().ToString(),
                            IdentityId = user.Id,
                            Name = payload["name"].ToString(),
                            Email = payload["email"].ToString(),
                            Avatar = payload["picture"].ToString(),
                            Introduce = "",

                            Rate = 0,
                            NumOfRate = 0,
                            NumOfAppointment = 0,

                            IsMentor = true,
                            MeetUrl = "",

                            IsActive = true,

                            CreateDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null
                        };
                        await _context.MasUsers.AddAsync(masUser);
                    }
                    else {
                        var masUser = new Core.Entities.MasUser {
                            Id = Guid.NewGuid().ToString(),
                            IdentityId = user.Id,
                            Name = payload["name"].ToString(),
                            Email = payload["email"].ToString(),
                            Avatar = payload["picture"].ToString(),
                            Introduce = "",

                            Rate = 0,
                            NumOfRate = 0,
                            NumOfAppointment = 0,

                            IsMentor = null,
                            MeetUrl = "",

                            IsActive = true,

                            CreateDate = DateTime.UtcNow.AddHours(7),
                            UpdateDate = null
                        };
                        await _context.MasUsers.AddAsync(masUser);
                    }

                    if (await _context.SaveChangesAsync() >= 0) {
                        var token = await GenerateToken(user);
                        return new AuthenResult {
                            Message = token[0],
                            ExpireDate = DateTime.Parse(token[1])
                        };
                    }
                }
            }
        }
        else {
            await userManager.AddLoginAsync(user, info);

            var appUser = await _context.MasUsers.FirstOrDefaultAsync(x => x.IdentityId == user.Id);

            if (appUser is null) {
                return new AuthenResult {
                    Errors = new List<string>() { "Account not exist." }
                };
            }

            if (appUser.IsActive is false) {
                return new AuthenResult {
                    Errors = new List<string>() { "Your account is disable. Please contact with FPT University for more information." }
                };
            }

            var token = await GenerateToken(user);
            return new AuthenResult {
                Message = token[0],
                ExpireDate = DateTime.Parse(token[1])
            };
        }

        return new AuthenResult {
            Errors = new List<string>() { "Error when authenticate with Google!" }
        };
    }

    private bool CheckIsMailMentor(string v)
    {
        var strSplit = v.Split("@");
        if (strSplit[1] == "fe.edu.vn") {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Generate token for user use to Authentication.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    private async Task<List<String>> GenerateToken(IdentityUser user)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims(user);
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        List<String> result = new List<string>();
        result.Add(token);
        result.Add(tokenOptions.ValidTo.ToString());

        return result;
    }

    /// <summary>
    /// Token options.
    /// </summary>
    /// <param name="signingCredentials"></param>
    /// <param name="claims"></param>
    /// <returns></returns>
    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var timeExpireInStr = jwtSettings.GetSection("Expired").Value;
        var timeExpire = Int32.Parse(timeExpireInStr);

        var tokenOptions = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(7).AddDays(timeExpire),
            signingCredentials: signingCredentials);

        return tokenOptions;
    }

    /// <summary>
    /// Get user claims.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    private async Task<List<Claim>> GetClaims(IdentityUser user)
    {
        var claims = new List<Claim>
        {
            new Claim("Id", user.Id),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
        };

        var roles = await userManager.GetRolesAsync(user);
        foreach (var role in roles) {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    /// <summary>
    /// Get user credential.
    /// </summary>
    /// <returns></returns>
    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(jwtSettings.GetSection("SecretKey").Value);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    /// <summary>
    /// Analyze the Id Token from Google.
    /// </summary>
    /// <param name="idToken"></param>
    /// <returns></returns>
    private JwtPayload PayloadInfo(string idToken)
    {
        var token = idToken;
        var handler = new JwtSecurityTokenHandler();
        var tokenData = handler.ReadJwtToken(token);
        return tokenData.Payload;
    }

    /// <summary>
    /// Set role for user. Exp: User, Admin,....
    /// </summary>
    /// <param name="identityUser"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    private async Task SetRoleForUser(IdentityUser identityUser, string role)
    {
        if (!await roleManager.RoleExistsAsync(role)) {
            await roleManager.CreateAsync(new IdentityRole(role));
        }

        if (await roleManager.RoleExistsAsync(role)) {
            await userManager.AddToRoleAsync(identityUser, role);
        }
    }

    /// <summary>
    /// Register admin account.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<AuthenResult> RegisterAdminAsync(RegisterAdminRequest request)
    {
        if (await IsExistEmail(request.Email)) {
            return new AuthenResult {
                Errors = new List<string>() { "Account already existed." }
            };
        }

        var identityUser = new IdentityUser() {
            Email = request.Email,
            UserName = request.Email
        };

        var result = await userManager.CreateAsync(identityUser, request.Password);
        await SetRoleForUser(identityUser, RoleConstants.Admin);

        if (result.Succeeded) {
            return new AuthenResult {
                Message = "Create Admin account successfully!"
            };
        }

        return new AuthenResult {
            Errors = result.Errors.Select(e => e.Description).ToList()
        };
    }

    /// <summary>
    /// Check exist email.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    private async Task<bool> IsExistEmail(string email)
    {
        var existEmail = await userManager.FindByEmailAsync(email);
        if (existEmail is not null) {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Login with Admin account.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<AuthenResult> LoginAdminAsync(LoginAdminRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null) {
            return new AuthenResult {
                Errors = new List<string>() { "Account is not existed." }
            };
        }

        if (!await IsAdmin(user)) {
            return new AuthenResult {
                Errors = new List<string>() { "Account is not existed." }
            };
        }

        var result = await userManager.CheckPasswordAsync(user, request.Password);

        if (!result) {
            return new AuthenResult {
                Errors = new List<string>() { "Wrong password!" }
            };
        }

        var token = await GenerateToken(user);

        return new AuthenResult {
            Message = token[0],
            ExpireDate = DateTime.Parse(token[1])
        };
    }

    /// <summary>
    /// Check admin account.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    private async Task<bool> IsAdmin(IdentityUser user)
    {
        // return role
        var roles = await userManager.GetRolesAsync(user);

        foreach (var item in roles) {
            if (item == RoleConstants.Admin) {
                return true;
            }
        }

        return false;
    }

    public async Task<AuthenResult> RegisterUserAsync(RegisterUserRequest request)
    {
        if (await IsExistEmail(request.Email)) {
            return new AuthenResult {
                Errors = new List<string>() { $"{request.Email} is already existed!" }
            };
        }

        var identityUser = new IdentityUser() {
            Email = request.Email,
            UserName = request.Email
        };

        var result = await userManager.CreateAsync(identityUser, request.Password);
        await SetRoleForUser(identityUser, RoleConstants.User);

        if (result.Succeeded) {
            var userEntityModel = _mapper.Map<Core.Entities.MasUser>(request);
            if (CheckIsMailMentor(request.Email)) {
                userEntityModel.Id = Guid.NewGuid().ToString();
                userEntityModel.IdentityId = identityUser.Id;
                userEntityModel.CreateDate = DateTime.UtcNow.AddHours(7);
                userEntityModel.UpdateDate = null;
                userEntityModel.Avatar = "";

                userEntityModel.Rate = 0;
                userEntityModel.NumOfRate = 0;
                userEntityModel.NumOfAppointment = 0;

                userEntityModel.IsMentor = true;
                userEntityModel.MeetUrl = "";

                userEntityModel.IsActive = true;
            }
            else {
                userEntityModel.Id = Guid.NewGuid().ToString();
                userEntityModel.IdentityId = identityUser.Id;
                userEntityModel.CreateDate = DateTime.UtcNow.AddHours(7);
                userEntityModel.UpdateDate = null;
                userEntityModel.Avatar = "";

                userEntityModel.Rate = 0;
                userEntityModel.NumOfRate = 0;
                userEntityModel.NumOfAppointment = 0;

                userEntityModel.IsMentor = null;
                userEntityModel.MeetUrl = "";

                userEntityModel.IsActive = true;
            }

            await _context.MasUsers.AddAsync(userEntityModel);

            if (await _context.SaveChangesAsync() >= 0) {
                return new AuthenResult {
                    Message = "Create User Account Successfully!"
                };
            }
        }

        return new AuthenResult {
            Errors = result.Errors.Select(e => e.Description).ToList()
        };
    }

    public async Task<AuthenResult> LoginUserAsync(LoginUserRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null) {
            return new AuthenResult {
                Errors = new List<string>() { "Account is not register!" }
            };
        }

        var appUser = await _context.MasUsers.FirstOrDefaultAsync(x => x.IdentityId == user.Id);
        if (appUser is not null) {
            if (appUser.IsActive != true) {
                return new AuthenResult {
                    Errors = new List<string>() { $"Please contact FPT University for support!" }
                };
            }
        }
        else {
            return new AuthenResult {
                Errors = new List<string>() { "Not found your info!" }
            };
        }

        var result = await userManager.CheckPasswordAsync(user, request.Password);
        if (!result) {
            return new AuthenResult {
                Errors = new List<string>() { "Wrong password!" }
            };
        }

        var token = await GenerateToken(user);
        return new AuthenResult {
            Message = token[0],
            ExpireDate = DateTime.Parse(token[1])
        };
    }
}
