using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GruppNrSexMVC.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.IO;

namespace GruppNrSexMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly SponsorContext _context;

        public AdminController(SponsorContext context)
        {
            _context = context;
        }

        public async Task<int> GetHighestID()
        {
            int Highest;
            List<Sponsor> SponsorList = new List<Sponsor>();
            HttpClient client = new HttpClient();

            var response = await client.GetAsync("http://193.10.202.76/SponsorsAPI/api/Sponsors");
            string jsonresponse = await response.Content.ReadAsStringAsync();
            SponsorList = JsonConvert.DeserializeObject<List<Sponsor>>(jsonresponse);

            var HighIdVar = SponsorList.Max(s => s.Id);

            Highest = HighIdVar; 

            return Highest; //ID IS CORRECT : Recived info is not
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            List<Sponsor> SponsorList = new List<Sponsor>();
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://193.10.202.76/SponsorsAPI/api/Sponsors");
            string jsonresponse = await response.Content.ReadAsStringAsync();
            SponsorList = JsonConvert.DeserializeObject<List<Sponsor>>(jsonresponse);

            return View(SponsorList);
            
        }

        // GET: Admin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create(Sponsor sponsorinfo)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://193.10.202.76/SponsorsAPI/api/Sponsors");
                var posttask = client.PostAsJsonAsync<Sponsor>("Sponsors", sponsorinfo);
                posttask.Wait();

                var result = posttask.Result;
                if (result.IsSuccessStatusCode)
                {

                    var HighestIDVar = GetHighestID();
                    int Id = HighestIDVar.Result;

                    return RedirectToAction(nameof(AddPicture), new { id = Id });
                }

            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(sponsorinfo);
        }

        public ActionResult AddPicture(int id, SponsorImageModel sponsorImage)
        {
            sponsorImage.Id = id;
            return View(sponsorImage);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPicture(SponsorImageModel sponsorimage)
        {
            
            foreach (var file in sponsorimage.ImageFile) //WHAT IS FILE?
            {
                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        string s = Convert.ToBase64String(fileBytes);
                        // act on the Base64 data
                    }
                }
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:64189/api/student");

                //HTTP POST
                var putTask = client.PutAsJsonAsync<SponsorImageModel>("Sponsors", sponsorimage);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    return RedirectToAction("Index");
                }
            }
            return View(sponsorimage);
        }


        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsor = await _context.Sponsors.FindAsync(id);
            byte[] img = sponsor.Image;
            
            if (sponsor == null)
            {
                return NotFound();
            }
            return View(sponsor);

        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,URL,Email,Image")] Sponsor sponsor)
        {
            if (id != sponsor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sponsor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SponsorExists(sponsor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sponsor);
        }

        // GET: Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsor = await _context.Sponsors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sponsor == null)
            {
                return NotFound();
            }

            return View(sponsor);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sponsor = await _context.Sponsors.FindAsync(id);
            _context.Sponsors.Remove(sponsor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SponsorExists(int id)
        {
            return _context.Sponsors.Any(e => e.Id == id);
        }
    }
}
