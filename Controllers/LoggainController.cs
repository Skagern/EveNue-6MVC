using GruppNrSexMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GruppNrSexMVC.Controllers
{
    public class LoggainController : Controller
    {

        public async Task<IActionResult> Loggain(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();

        }



        [HttpPost]
        public async Task<dynamic> Loggain(string username, string password, string returnUrl)

        {

            try
            {
                var url = "http://193.10.202.75/LoginAPI/" + $"Authenticate?Username={username}&Password={password}";

                HttpClient httpClient = new HttpClient();

                HttpResponseMessage response = await httpClient.GetAsync(new Uri(url));



                if (!response.IsSuccessStatusCode)
                {

                    Debug.WriteLine("Login failed");

                    return View();

                }

                var content = await response.Content.ReadAsStringAsync();

                var recievedLogin = JsonConvert.DeserializeObject<LoginDetails>(content);

                Debug.WriteLine("Login successful");

                //göra inloggning 



                // Allt stämmer, logga in användaren 

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                identity.AddClaim(new Claim(ClaimTypes.Name, recievedLogin.Username));







                await HttpContext.SignInAsync(

                CookieAuthenticationDefaults.AuthenticationScheme,

                new ClaimsPrincipal(identity));

                if (returnUrl == null)
                {
                   return RedirectToAction("Index", "Home");
                }

                return Redirect(returnUrl);



            }

            catch (Exception e)

            {

                Debug.WriteLine("Login failed: " + e.StackTrace);

                return null;

            }

        }

        public async Task<IActionResult> LoggaUt()
        {

            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


    }

}

