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


        //[AutoValidateAntiforgeryToken]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginParameter memberInfo)
        {
            //if (!ModelState.IsValid)
            //{
            //    ModelState.AddModelError("", "資料不符規定，或Recaptcha尚未回應，請重新嘗試");
            //    return ReturnLoginView(memberInfo);
            //}

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
                //ModelState.AddModelError("", "Recaptcha判定您為機器人，請再嘗試一次");
                return ReturnLoginView(memberInfo);
            }

            var loginResult = await loginTask;
            if (loginResult == null)
            {
                //ModelState.AddModelError("", "密碼錯誤或會員不存在");
                return ReturnLoginView(memberInfo);
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
                  //new Claim(ClaimTypes.Role, "Customer")
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

        private IActionResult ReturnLoginView(LoginParameter memberInfo)
        {
            var loginViewModel = new LoginViewModel
            {
                Email = memberInfo.Email,
                Password = memberInfo.Password
            };
            return BadRequest(loginViewModel);
        }

        //[Authorize]
        //[HttpPost("logout")]
        //public IActionResult Logout()
        //{
        //    HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        //    return Ok("登出成功");
        //}


        //[AutoValidateAntiforgeryToken]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterParameter memberInfo)
        {
            //if (!ModelState.IsValid)
            //{
            //    ModelState.AddModelError("", "資料不符規定，或Recaptcha尚未回應，請重新嘗試");
            //    return ReturnRegisterView(memberInfo);
            //}
            var isHuman = await  _recaptchaService.VerifyRecaptchaAsync(memberInfo.RecaptchaToken);
            if (!isHuman)
            {
                //ModelState.AddModelError("", "Recaptcha判定您為機器人，請重新嘗試");
                return ReturnRegisterView(memberInfo);
            }
            var registerInfo = new MemberAuthInfo
            {
                Email = memberInfo.Email,
                Password = memberInfo.Password
            };
            bool success = await _memberService.CreateMemberAsync(registerInfo);
            if (!success)
            {
                //ModelState.AddModelError("", "註冊失敗，Email已被註冊");
                return ReturnRegisterView(memberInfo);
            }
            return Ok("註冊成功");
        }

        private IActionResult ReturnRegisterView(RegisterParameter memberInfo)
        {
            var registerViewModel = new RegisterViewModel
            {
                Email = memberInfo.Email,
                Password = memberInfo.Password,
                ConfirmPassword = memberInfo.ConfirmPassword
            };
            return BadRequest(registerViewModel);
        }

    }
}
