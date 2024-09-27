using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MyStore.Server.Controllers.Dtos.ViewModels;
using MyStore.Server.Controllers.Dtos.Parameters;
using MyStore.Server.Models.Service.Interfaces;
using MyStore.Server.Models.Service.Dtos.Infos;
using Microsoft.AspNetCore.Authorization;
using MyStore.Server.Models.Service.Dtos.ResultModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
            var loginTask = _memberService.LoginAsync(info);//非同步並行

            var isHumanTask =  _recaptchaService.VerifyRecaptchaAsync(memberInfo.RecaptchaToken);
            var isHuman = await isHumanTask;//非同步並行
            if (!isHuman)
            {
                return BadRequest("Recaptcha判定您為機器人，請再嘗試一次");
            }

            var loginResult = await loginTask;
            if (loginResult == null)
            {
                return BadRequest("密碼錯誤或會員不存在");
            }
            var token = GetJwtToken(loginResult);

            return Ok(new {token});
        }

        private string GetJwtToken(MemberResultModel memberResultModel)
        {
            var claims = new List<Claim>
                {
                  new Claim(ClaimTypes.Name, memberResultModel.UserName),
                  new Claim(ClaimTypes.Email, memberResultModel.Email),
                  new Claim(ClaimTypes.NameIdentifier,memberResultModel.MemberId.ToString())
                 };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKey"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.Now.AddDays(7); 

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7266/",
                audience: "https://localhost:5173/",
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterParameter memberInfo)
        {
            var isHuman = await  _recaptchaService.VerifyRecaptchaAsync(memberInfo.RecaptchaToken);
            if (!isHuman)
            {
                return BadRequest("Recaptcha判定您為機器人，請重新嘗試");
            }
            var registerInfo = new MemberAuthInfo
            {
                Email = memberInfo.Email,
                Password = memberInfo.Password
            };
            bool success = await _memberService.CreateMemberAsync(registerInfo);
            if (!success)
            {
                return BadRequest("註冊失敗，Email已被註冊");
            }
            return Ok("註冊成功");
        }

    }
}
