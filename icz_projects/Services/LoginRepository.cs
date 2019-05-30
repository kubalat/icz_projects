using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using icz_projects.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace icz_projects.Services
{
    public class LoginRepository : ILoginRepository
    {
        private string _hashedPassword;
        private readonly string _claimIdentifier;

        public LoginRepository(string hashedPassword, string claimIdentifier)
        {
            this._hashedPassword = hashedPassword;
            this._claimIdentifier = claimIdentifier;
        }
        public async Task<bool> Login(string password, HttpContext context)
        {
            StringBuilder sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(password));

                foreach (Byte b in result)
                    sb.Append(b.ToString("x2"));
            }

            string hashedPassword = sb.ToString();
            if (hashedPassword == this._hashedPassword)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, this._claimIdentifier, ClaimValueTypes.String, "localhost")
                    };
                var userIdentity = new ClaimsIdentity(claims, "SecureLogin");
                var userPrincipal = new ClaimsPrincipal(userIdentity);


                await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal,
                    new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(15),
                        IsPersistent = false,
                        AllowRefresh = false
                    });

                return true;
            }

            return false;
        }

        public async Task<bool> Logout(HttpContext context)
        {
            await context.SignOutAsync();
            return true;
        }

        public bool IsLogged(HttpContext context)
        {
            return context.User.Claims.Where(x => x.Type == ClaimTypes.Name).Any() ? true : false;
        }
    }
}
