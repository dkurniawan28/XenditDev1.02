using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using XenditDev1._02.ModelsNew;

namespace XenditDev1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

       
        [HttpPost("token")]
        public IActionResult token(RequestToken RequestToken_)
        {
            PaymentGatewayContext entities = new PaymentGatewayContext();
            string str = "{\r\n \"Status\":\"Error\", \r\n\"StatusCode\":\"400\",\r\n\"Description\":\"Username dan password tidak cocok\"\r\n}";
            JObject json = JObject.Parse(str);
            try
            {
                int userid = 0;
                string username = "";
                string reqpassword = RequestToken_.Password;

                try
                {
                     username = entities.Merchant.Where(p => p.Username == RequestToken_.Username).ToList()[0].Username;
                }
                catch (System.Exception e)
                {
                    username = "";
                }

              
                string password = entities.Merchant.Where(p => p.Password == reqpassword).ToList()[0].Password;

                if (username==RequestToken_.Username && password== reqpassword)
                {
                    try
                    {
                        userid = (int)entities.Merchant.Where(p => p.Username == RequestToken_.Username && p.Password==reqpassword).ToList()[0].IdMerchant;
                    }
                    catch (System.Exception e)
                    {
                        userid = 0;
                    }
                    var claimData = new[] { new Claim(ClaimTypes.Name, username) };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("209efb21-b99b-4dda-811a-328ac96315d1"));
                    var signInCred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
                    var token = new JwtSecurityToken(
                      
                        issuer: "209efb21-b99b-4dda-811a-328ac96315d1",
                        audience: "209efb21-b99b-4dda-811a-328ac96315d1",
                        expires: DateTime.Now.AddHours(1),
                        claims: claimData,
                        signingCredentials: signInCred
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                    HttpContext.Session.SetString("token", tokenString);
                    HttpContext.Session.SetInt32("userid",userid);
                  


                    return Ok(new { Userid=userid, Username = username, access_token = tokenString, token_type = "BearerToken", expires_in = ConvertToUnixTimestamp(DateTime.Now.AddHours(1)), scope = "resource.READ", DateTimePost = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
                }
                else
                {
                    return BadRequest(json);
                }
            }
            catch (System.Exception e)
            {

                return BadRequest(json);
            }
        }
        private static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        public class RequestToken
        {
            

            public String Username { get; set; }

            public String Password { get; set; }
        }
    }
}