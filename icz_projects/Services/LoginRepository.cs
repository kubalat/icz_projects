﻿using System;
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
        private readonly string _hashedPassword;
        private readonly string _claimIdentifier;

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        /// <param name="hashedPassword">Hashed password for login to the system</param>
        /// <param name="claimIdentifier">Claim identifier for cookie</param>
        public LoginRepository(string hashedPassword, string claimIdentifier)
        {
            if (string.IsNullOrWhiteSpace(hashedPassword))
            {
                throw new ArgumentNullException(nameof(hashedPassword), "Parameter is null");
            }

            if (string.IsNullOrWhiteSpace(claimIdentifier))
            {
                throw new ArgumentNullException(nameof(claimIdentifier), "Parameter is null");
            }

            this._hashedPassword = hashedPassword;
            this._claimIdentifier = claimIdentifier;
        }

        /// <summary>
        /// Login to the system.
        /// </summary>
        /// <returns><c>true</c>, if login was successful, <c>false</c> otherwise.</returns>
        /// <param name="password">Password for login to the system</param>
        /// <param name="context">HttpContext of the request</param>
        public async Task<bool> Login(string password, HttpContext context)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password), "Parameter is null");
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context), "Parameter is null");
            }

            try
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        /// <summary>
        /// Logout from the system
        /// </summary>
        /// <returns><c>true</c>, if logout was successful, <c>false</c> otherwise.</returns>
        /// <param name="context">HttpContext of the request</param>
        public async Task<bool> Logout(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context), "Parameter is null");
            }

            await context.SignOutAsync();
            return true;
        }

        /// <summary>
        /// Check if already logged in system.
        /// </summary>
        /// <returns><c>true</c>, if logged, <c>false</c> otherwise.</returns>
        /// <param name="context">HttpContext of the request</param>
        public bool IsLogged(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context), "Parameter is null");
            }

            return context.User.Claims.Where(x => x.Type == ClaimTypes.Name).Any() ? true : false;
        }
    }
}
