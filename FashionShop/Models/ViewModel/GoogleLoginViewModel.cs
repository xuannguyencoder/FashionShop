using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace FashionShop.Models.ViewModel
{
    public class GoogleLoginViewModel
    {
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public string GiveName { get; set; }
        public string SurName { get; set; }
        public string NameIdentifier { get; set; }
        internal static GoogleLoginViewModel GetLoginInfo(ClaimsIdentity identity)
        {
            if (identity.Claims.Count() == 0 || identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email) == null)
            {
                return null;
            }
            return new GoogleLoginViewModel
            {
                EmailAddress = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value,
                Name = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value,
                SurName = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname).Value,
                NameIdentifier = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value,
                GiveName = identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName).Value,


            };
        }
    }
}