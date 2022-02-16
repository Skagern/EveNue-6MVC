using GruppNrSexMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GruppNrSexMVC.Controllers
{
    public class SponsorsController : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<Sponsor> Sponsorer = new List<Sponsor>();
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://localhost:64409/api/Sponsors");
            string jsonresponse = await response.Content.ReadAsStringAsync();
            Sponsorer = JsonConvert.DeserializeObject<List<Sponsor>>(jsonresponse);

            return View(Sponsorer);
        }
    }
}
