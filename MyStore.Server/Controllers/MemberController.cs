using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyStore.Server.Controllers.Dtos.Parameters;
using MyStore.Server.Models.Service.Dtos.Infos;
using MyStore.Server.Models.Service.Dtos.ResultModels;
using MyStore.Server.Models.Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace MyStore.Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly IRecaptchaService _recaptchaService;
        public MemberController(IMemberService memberService, IRecaptchaService recaptchaService)
        {
            _memberService = memberService;
            _recaptchaService = recaptchaService;
        }



        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginParameter memberInfo)
        {
            var info = new MemberAuthInfo
            {
                Email = memberInfo.Email,
                Password = memberInfo.Password
            };
            var loginTask = _memberService.GetMemberAsync(info);//非同步並行

            var isHumanTask = _recaptchaService.VerifyRecaptchaAsync(memberInfo.RecaptchaToken);
            var isHuman = await isHumanTask;//非同步並行
            if (!isHuman)
            {
                return BadRequest(new { apiMessage = "Recaptcha判定您為機器人，請再嘗試一次" });
            }

            var loginResult = await loginTask;
            if (loginResult == null)
            {
                return BadRequest(new { apiMessage = "密碼錯誤或會員不存在" });
            }
            var token = GetJwtToken(loginResult);

            return Ok(new { token });
        }

        private string GetJwtToken(MemberResultModel memberResultModel)
        {
            var claims = new List<Claim>
                {
                  new Claim(ClaimTypes.Name, memberResultModel.UserName),
                  new Claim(ClaimTypes.Email, memberResultModel.Email),
                  new Claim(ClaimTypes.NameIdentifier,memberResultModel.MemberId.ToString())
                 };
            if (memberResultModel.Email == "afsnj9732@gmail.com")
            {
                claims.Add(new Claim("role", "Admin"));
            }
            else
            {
                claims.Add(new Claim("role", "Customer"));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecretKeyMySuperSecretKeyMySuperSecretKey"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.Now.AddDays(1);

            var token = new JwtSecurityToken(
                //issuer: "https://localhost:7266/",
                //audience: "https://localhost:5173/",
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterParameter memberInfo)
        {
            var isHuman = await _recaptchaService.VerifyRecaptchaAsync(memberInfo.RecaptchaToken);
            if (!isHuman)
            {
                return BadRequest(new { apiMessage = "Recaptcha判定您為機器人，請重新嘗試" });
            }
            var registerInfo = new MemberAuthInfo
            {
                Email = memberInfo.Email,
                Password = memberInfo.Password
            };
            bool success = await _memberService.CreateMemberAsync(registerInfo);
            if (!success)
            {
                return BadRequest(new { apiMessage = "註冊失敗，Email已被註冊" });
            }
            return Ok();
        }

    }
}
