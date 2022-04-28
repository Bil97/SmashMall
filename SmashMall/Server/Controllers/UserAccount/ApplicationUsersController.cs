using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmashMall.Server.Data;
using SmashMall.Shared.Models.UserAccount;
using SmashMall.Shared.Services;

namespace SmashStores.Controllers.UserAccount
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;


        public ApplicationUsersController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IEmailSender emailSender
            )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailSender = emailSender;

        }

        // GET: api/ApplicationUsers
        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<IEnumerable<UserDetails>>> GetApplicationUser()
        {
            var applicationUsers = await _userManager.Users.ToListAsync();
            List<UserDetails> users = new List<UserDetails>();
            foreach (var applicationUser in applicationUsers)
            {
                var userDetails = new UserDetails
                {
                    Id = applicationUser.Id,
                    UserName = applicationUser.UserName,
                    Email = applicationUser.Email,
                    EmailConfirmed = applicationUser.EmailConfirmed,
                    Phonenumber = applicationUser.PhoneNumber,
                    PhonenumberConfirmed = applicationUser.PhoneNumberConfirmed,
                    FirstName = applicationUser.FirstName,
                    LastName = applicationUser.LastName,
                    NationalIdNumber = applicationUser.NationalIdNumber,
                    DateCreated = applicationUser.DateCreated
                };
                users.Add(userDetails);
            }

            return users;
        }

        // GET: api/ApplicationUsers/UserName
        [HttpGet("{id}")]
        //[Authorize]
        public async Task<ActionResult<UserDetails>> GetApplicationUser(string id)
        {
            var applicationUser = await _userManager.Users.FirstOrDefaultAsync(i => i.UserName == id);

            if (applicationUser == null)
            {
                return NotFound("User not found");
            }
            var userDetails = new UserDetails
            {
                Id = applicationUser.Id,
                UserName = applicationUser.UserName,
                Email = applicationUser.Email,
                EmailConfirmed = applicationUser.EmailConfirmed,
                Phonenumber = applicationUser.PhoneNumber,
                PhonenumberConfirmed = applicationUser.PhoneNumberConfirmed,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                NationalIdNumber = applicationUser.NationalIdNumber,
                DateCreated = applicationUser.DateCreated
            };

            return userDetails;
        }

        // PUT: api/ApplicationUsers/update-account/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("update-account/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateApplicationUser(string id, UserDetails userDetails)
        {
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);

            if (id != applicationUser?.Id)
            {
                return BadRequest("Invalid request");
            }

            applicationUser.UserName = userDetails.UserName;
            applicationUser.Email = userDetails.Email;
            applicationUser.EmailConfirmed = userDetails.EmailConfirmed;
            applicationUser.PhoneNumber = userDetails.Phonenumber;
            applicationUser.PhoneNumberConfirmed = userDetails.PhonenumberConfirmed;
            applicationUser.FirstName = userDetails.FirstName;
            applicationUser.LastName = userDetails.LastName;
            applicationUser.NationalIdNumber = userDetails.NationalIdNumber;

            try
            {
                await _userManager.UpdateAsync(applicationUser);
                return Ok("Account update successful");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationUserExists(id))
                {
                    return NotFound("User does not exist");
                }
                else
                {
                    throw;
                }
            }
        }

        // POST: api/ApplicationUsers/create-account
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost("create-account")]
        public async Task<ActionResult> CreateApplicationUser([FromBody] SmashMall.Server.Models.UserAccount.Register register)
        {
            var applicationUser = new ApplicationUser
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                UserName = register.Email,
                Email = register.Email,
                PhoneNumber = $"{Convert.ToInt64(register.Phonenumber)}",
                DateCreated = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(applicationUser, register.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.FirstOrDefault().Description);
            }
            else
            {
                var user = await _userManager.FindByEmailAsync(register.Email);
                // Add the first two users to the admins role
                if (_userManager.Users.Count() <= 2)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "StandardUser");
                }

                var callbackUrl = $"{BaseApi.Url}/confirm-email/{user.Id}/{user.SecurityStamp}";

                var confirmEmail = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";
                try
                {
                    await _emailSender.SendEmailAsync(applicationUser.Email, "Confirm your email", confirmEmail);
                }
                catch (SmtpFailedRecipientException)
                {
                    await _userManager.DeleteAsync(user);
                    await _context.SaveChangesAsync();
                    return NotFound("Invalid email address");
                }
                await _signInManager.PasswordSignInAsync(register.Email, register.Password, register.RememberMe, false);

                return Ok(new JwtSecurityTokenHandler().WriteToken(await Token(register.Email)));
            }
        }

        // POST: api/ApplicationUsers/confirm-email
        [HttpPost("confirm-email")]
        public async Task<ActionResult> ConfirmEmail([FromBody] ConfirmEmail confirmEmail)
        {
            if (confirmEmail.UserId == null || confirmEmail.Code == null)
            {
                return NotFound($"Unable to load user with ID '{confirmEmail.UserId}'.");
            }

            var user = await _userManager.FindByIdAsync(confirmEmail.UserId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{confirmEmail.UserId}'.");
            }

            if (user.SecurityStamp == confirmEmail.Code)
            {
                user.EmailConfirmed = true;
                await _context.SaveChangesAsync();
                return Ok("Thank you for confirming your email. Login in to access your acoount");
            }
            else
            {
                return BadRequest("Error confirming your email.");
            }
        }

        // POST: api/ApplicationUsers/login
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] SmashMall.Server.Models.UserAccount.Login login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, login.RememberMe, false);

            if (!result.Succeeded)
            {
                return BadRequest("Incorect username or password");
            }
            //Console.WriteLine(await _userManager.GetAuthenticationTokenAsync(_userManager.FindByEmailAsync(login.Email),provider);
            return Ok(new JwtSecurityTokenHandler().WriteToken(await Token(login.Email)));
        }

        async private Task<JwtSecurityToken> Token(string email)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(email);
            var roles = await _signInManager.UserManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email)
            };
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryInDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtAudience"],
                claims,
                expires: expiry,
                signingCredentials: creds
            );
            return token;
        }


        // Post: api/ApplicationUsers/logout
        [HttpPost("logout")]
        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        // Post: api/ApplicationUsers/forgot-password
        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPassword forgotPassword)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
                if (user == null)
                {
                    return NotFound("Email does not exist");
                }

                var callbackUrl = $"{BaseApi.Url}/user-account/reset-password/{user.Id}/{user.SecurityStamp}";
                var confirmEmail = $"Reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";
                await _emailSender.SendEmailAsync(forgotPassword.Email, "Reset your password", confirmEmail);

                return Ok("Check your email for a link to reset your password");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Post: api/ApplicationUsers/forgot-password
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPassword resetPassword)
        {
            if (resetPassword.UserId == null || resetPassword.Code == null)
            {
                return NotFound($"Unable to load user with ID '{resetPassword.UserId}'.");
            }

            var user = await _userManager.FindByIdAsync(resetPassword.UserId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{resetPassword.UserId}'.");
            }

            if (user.SecurityStamp == resetPassword.Code)
            {
                try
                {
                    await _userManager.RemovePasswordAsync(user);
                    await _context.SaveChangesAsync();

                    var result = await _userManager.AddPasswordAsync(user, resetPassword.NewPassword);
                    if (result.Succeeded)
                    {
                        return Ok("Password reset successful. Login in to access your acoount");
                    }
                    else
                    {
                        return BadRequest(result.Errors.FirstOrDefault().Description);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest("Error reseting your password.");
            }
        }

        // Post: api/ApplicationUsers/forgot-password
        [HttpPost("change-password")]
        [Authorize]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePassword changePassword)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("An error occured when updating your password");
            }

            try
            {
                var result = await _userManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors.FirstOrDefault().Description);
                }
                else
                {
                    return Ok("Password change successfull.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/ApplicationUsers/delete-account/5
        [HttpDelete("delete-account/{id}")]
        [Authorize(Roles = "Admin,StandardUser")]
        public async Task<ActionResult<ApplicationUser>> DeleteApplicationUser(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ApplicationUser applicationUser = await _userManager.GetUserAsync(User);

            if (id == applicationUser.Id || User.IsInRole("Admin"))

            {
                try
                {
                    await _userManager.DeleteAsync(applicationUser);
                    return Ok("Account delete successful");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(id))
                    {
                        return NotFound("User does not exist");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return BadRequest("Invalid request");

        }

        // GET: api/ApplicationUsers/image/name
        [HttpGet("image/{id}")]

        private bool ApplicationUserExists(string id)
        {
            return _userManager.Users.Any(e => e.Id == id);
        }

    }
}
