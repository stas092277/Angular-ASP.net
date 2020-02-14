#define DISABLE_EMAIL

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using InGo.Identity;
using InGo.Models;
using InGo.Models.Identity;
using InGo.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using InGo.Models.Configuration;
using Microsoft.Extensions.Options;
using InGo.Services;
using ILogger = Microsoft.Extensions.Logging.ILogger;


namespace InGo.Controllers
{

    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<UserIdentity> _userManager;
        private readonly SignInManager<UserIdentity> _signInManager;
        private readonly IngoContext _context;
        private readonly JwtSettings _jwtSettings;
        private readonly CurrentUser _currentUser;
        private readonly ILogger _logger;
        private readonly EmailService _emailService;

        public AccountController(UserManager<UserIdentity> userManager, SignInManager<UserIdentity> signInManager, IngoContext context
            , IOptions<JwtSettings> jwtSettings, CurrentUser currentUser, ILogger<AccountController> logger
            , EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _jwtSettings = jwtSettings.Value;
            _currentUser = currentUser;
            _logger = logger;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            string[] allowedDomains = { "gmail.com" };
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!allowedDomains.Any(d => model.Email.EndsWith("@" + d)))
                return BadRequest("Only Ingosstrakh members are allowed to register!");

            UserIdentity identity = new UserIdentity { Email = model.Email, UserName = model.Email };

            var result = await _userManager.CreateAsync(identity, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);


            User user = new User { FirstName = model.FirstName, LastName = model.LastName, Identity = identity, Email = model.Email, ImgUrl = "images/userCat.png" };
            var res = _context.UserProfiles.Add(user);
            await _context.SaveChangesAsync();

#if !DISABLE_EMAIL
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(identity);
            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                new { userId = identity.Id, code },
                protocol: HttpContext.Request.Scheme);
            _emailService.SendEmailAsync(model.Email, "Регистрация на ingos.ru", 
                "Привет!\n Если ты регистрировался на ingos.ru, пожалуйста, перейди " +
                $"<a href={callbackUrl}>по этой ссылке</a>.");
#endif

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            //_logger.LogInformation($"{model.Email}: {model.Password}");
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
                return BadRequest("user == null!");

#if !DISABLE_EMAIL
            if (!await _userManager.IsEmailConfirmedAsync(user))
                return BadRequest("Confirm your email first!");
#endif

            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID",user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);

                await _signInManager.SignInAsync(user, true);
                return Ok(new { token });
            }
            else
                return BadRequest(new { message = "Username or password is incorrect." });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            
            if (returnUrl != null)
            {
                return Redirect("/Home/Index");
            }
            else
            {
                return Ok();
            }
        }

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)            
                return BadRequest($"No user {userId} found!");
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return Redirect("/authorization/login");
            return BadRequest("Wrong confirmation code!");
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            return Ok(await _currentUser.GetCurrentUser(HttpContext));
        }
    }
}