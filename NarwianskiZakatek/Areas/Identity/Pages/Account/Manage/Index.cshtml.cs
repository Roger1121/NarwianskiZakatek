// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NarwianskiZakatek.CustomAnnotations;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;

namespace NarwianskiZakatek.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Display(Name = "Login")]
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Numer telefonu")]
            [Phone(ErrorMessage = "Nieprawidłowy numer telefonu")]
            public string PhoneNumber { get; set; }

            [Display(Name = "Miasto")]
            [Required(ErrorMessage = "Pole wymagane")]
            public string City { get; set; }

            [Display(Name = "Ulica")]
            public string Street { get; set; }

            [Display(Name = "Numer budynku")]
            [Required(ErrorMessage = "Pole wymagane")]
            public string BuildingNumber { get; set; }

            [Display(Name = "Numer lokalu")]
            public string? LocalNumber { get; set; }

            [Display(Name = "Kod pocztowy")]
            [Required(ErrorMessage = "Pole wymagane")]
            [PostalCode]
            public string PostalCode { get; set; }

            [Display(Name = "Poczta")]
            [Required(ErrorMessage = "Pole wymagane")]
            public string PostCity { get; set; }

            [Display(Name = "Imię/Imiona")]
            [Required(ErrorMessage = "Pole wymagane")]
            public string Name { get; set; }

            [Display(Name = "Nazwisko")]
            [Required(ErrorMessage = "Pole wymagane")]
            public string Surname { get; set; }
        }

        private async Task LoadAsync(AppUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                City = user.City,
                Street = user.Street,
                BuildingNumber = user.BuildingNumber,
                LocalNumber = user.LocalNumber,
                PostalCode = user.PostalCode,
                PostCity = user.PostCity,
                PhoneNumber = phoneNumber,
                Name = user.Name,
                Surname = user.Surname
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Nastąpił błąd, spróbuj ponownie później.";
                    return RedirectToPage();
                }
            }

            if(Input.City != user.City)
            {
                user.City = Input.City;
            }
            if(Input.Street != user.Street)
            {
                user.Street = Input.Street;
            }
            if(Input.BuildingNumber != user.BuildingNumber)
            {
                user.BuildingNumber = Input.BuildingNumber;
            }
            if(Input.LocalNumber != user.LocalNumber)
            {
                user.LocalNumber = Input.LocalNumber;
            }
            if(Input.PostalCode != user.PostalCode)
            {
                user.PostalCode = Input.PostalCode;
            }
            if(Input.PostCity != user.PostCity)
            {
                user.PostCity = Input.PostCity;
            }
            if (Input.Name != user.Name)
            {
                user.Name = Input.Name;
            }
            if (Input.Surname != user.Surname)
            {
                user.Surname = Input.Surname;
            }
            _context.SaveChanges();

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Twoje dane zostały pomyślnie zaktualizowane";
            return RedirectToPage();
        }
    }
}
