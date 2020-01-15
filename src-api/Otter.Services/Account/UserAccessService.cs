using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Otter.Core;
using Otter.Data;
using Repository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Otter.Services.Account
{
    public interface IUserAccessService
    {
        Core.Account.User Authenticate(Core.Account.LoginModel login);

        IEnumerable<Core.Account.PolicyModel> GetPolicies();
    }

    public class UserAccessService : BaseService, IUserAccessService
    {
        private readonly IRepository<User> _repoUser;
        private readonly IRepository<Policy> _repoPolicy;
        private readonly AppSettings _appSettings;

        public UserAccessService(Core.Account.ICurrentUser currentUser
            , IRepository<User> repoUser
            , IRepository<Policy> repoPolicy
            , IOptions<AppSettings> appSettings) : base(currentUser)
        {
            _repoUser = repoUser;
            _repoPolicy = repoPolicy;
            _appSettings = appSettings.Value;
        }

        public Core.Account.User Authenticate(Core.Account.LoginModel login)
        {
            var user = _repoUser
                .Query()
                .Filter(x => x.Username == login.Email)
                .Filter(x => x
                    .UserRoles
                    .Any(ur =>
                        ur.Role
                            .CompanySiteRoles
                            .Any(csr =>
                                csr.CompanySite.Code == login.SiteCode
                                && csr.CompanySite.Company.Code == login.CompanyCode)))
                .Get()
                .Select(x => new
                {
                    x.Status,
                    x.PasswordHash,
                    x.Username,
                    x.Email,
                    roles = x.UserRoles.Select(ur => ur.Role.Code)
                })
                .FirstOrDefault();

            if (user == null)
                return new Core.Account.User() { Email = login.Email, Status = Core.Account.LoginStatus.NotFound };

            Core.Account.LoginStatus status = Core.Account.LoginStatus.None;
            var userStatus = (Core.Account.UserStatus)(user.Status ?? 0);
            switch (userStatus)
            {
                case Core.Account.UserStatus.Active:

                    var key = Encoding.ASCII.GetBytes(_appSettings.SecretKey);
                    var userRoles = string.Join(",", user.roles);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, login.Email),
                        new Claim("CompanyCode", login.CompanyCode)
                    };

                    claims.AddRange(user.roles.Select(item => new Claim("CLAIM_" + item, item)));

                    claims.AddRange(new[] {
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    });

                    var authSigningKey = new SymmetricSecurityKey(key);

                    var jwtSecToken = new JwtSecurityToken(
                        issuer: "OTTER",
                        audience: "OTTER",
                        expires: DateTime.UtcNow.AddMinutes(_appSettings.TokenExpirationMinutes),
                        claims: claims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );
                    status = Core.Account.LoginStatus.Success;
                    return new Core.Account.User()
                    {
                        Email = login.Email,
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtSecToken),
                        Status = status,
                    };
                case Core.Account.UserStatus.ResetPassword:
                    status = Core.Account.LoginStatus.ResetPassword;
                    return new Core.Account.User()
                    {
                        Email = login.Email,
                        Status = status,
                    };

                case Core.Account.UserStatus.Locked:
                    status = Core.Account.LoginStatus.Locked;
                    return new Core.Account.User()
                    {
                        Email = login.Email,
                        Status = status,
                    };
                default:
                    return new Core.Account.User()
                    {
                        Email = login.Email,
                        Status = status,
                    };
            }
        }

        public IEnumerable<Core.Account.PolicyModel> GetPolicies()
        {
            var data = _repoPolicy
                .Query()
                .Select(x => new Core.Account.PolicyModel()
                {
                    PolicyCode = x.Code,
                    RoleCodes = x.PolicyRoles.Select(pr => pr.Role.Code)
                })
                ;
            return data.ToList();
        }
    }
}