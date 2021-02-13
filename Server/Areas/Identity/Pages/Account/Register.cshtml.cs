using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Profile.Server.Data;
using Profile.Shared.Models;
using Profile.Shared.Models.Admin;
using Profile.Shared.Models.Codgram;

namespace Profile.Server.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ProfileUser> _signInManager;
        private readonly UserManager<ProfileUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ProfileContext _context;
        private readonly CodgramContext _codgramContext;

        public RegisterModel(
            UserManager<ProfileUser> userManager,
            SignInManager<ProfileUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            ProfileContext context,
            CodgramContext codgramContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
            _codgramContext = codgramContext;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        public API API { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Username")]
            [StringLength(24, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
            [RegularExpression("^[0-9a-zA-Z_]*$", ErrorMessage="Only letters, numbers and undersocre are allowed")]
            public string UserName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "I am a ")]
            public ProfileUserType ProfileUserType { get; set; }
        }

        public async Task OnGetAsync( ProfileUserType profileUserType, string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            ViewData["ProfileUserType"] = profileUserType;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            var reservedLinks = await _context.ReservedLink.FirstOrDefaultAsync(r => r.Link.ToUpper() == Input.UserName.ToUpper());

            if (ModelState.IsValid && reservedLinks == null)
            {
                var user = new ProfileUser { UserName = Input.UserName, Email = Input.Email, ProfileUserType = Input.ProfileUserType };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await CreateSubscription(user.Id);
                    await CreateGettingStarted(user.Id);
                    
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    
                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { username = Input.UserName, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }

                    
                }

                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);                    
                }
            }

            if(reservedLinks != null) {
                ModelState.AddModelError(string.Empty, $"Username '{Input.UserName}' is already taken (Error 0002)");
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }


        public async Task CreateSubscription(string userId) {
            var subscription = new Subscription() {
                ProfileUserId = userId,
                Type = SubscriptionType.Free,
            };
            _context.Subscription.Add(subscription);
            await _context.SaveChangesAsync();
        }

        public async Task CreateGettingStarted(string userId) {
            var gettingStarted = new GettingStarted() {
                ProfileUserId = userId,
            };
            _context.GettingStarted.Add(gettingStarted);
            await _context.SaveChangesAsync();
        }
    }
}
