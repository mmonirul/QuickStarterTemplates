using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("token")]
        public ActionResult GetToken()
        {
            // security key
            string securityKey ="security_key_security_key";

            // symentric security key

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            // signing credentials
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            // Add claims

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("custom_claim", "custom_claim_value")
            };


            //create token 

            var token = new JwtSecurityToken(
                    issuer: "sumon@0340",
                    audience: "my_readers",
                    expires: DateTime.Now.AddHours(5),
                    signingCredentials: signingCredentials,
                    claims: claims
                );



            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}