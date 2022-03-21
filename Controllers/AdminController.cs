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

namespace GruppNrSexMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly SponsorContext _context;

        public AdminController(SponsorContext context)
        {
            _context = context;
        }

        // GET: Admin
        public async Task<IActionResult> Index()
        {
            List<Sponsor> Sponsorer = new List<Sponsor>();
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://193.10.202.76/SponsorsAPI/api/Sponsors");
            string jsonresponse = await response.Content.ReadAsStringAsync();
            Sponsorer = JsonConvert.DeserializeObject<List<Sponsor>>(jsonresponse);


            return View(Sponsorer);
            //return View(await _context.Sponsors.ToListAsync());
        }


        // GET: Admin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        //EJ TESTAD
        public async Task <IActionResult> Create(Sponsor sponsor)
        {
            using (var client = new HttpClient())
            {
                if (sponsor.UppladdadBild != null)
                {
                    using var memoryStream = new System.IO.MemoryStream();
                    await sponsor.UppladdadBild.CopyToAsync(memoryStream);
                    sponsor.Image = memoryStream.ToArray();
                }

                client.BaseAddress = new Uri("http://193.10.202.76/SponsorsAPI/api/Sponsors");
                var posttask = client.PostAsJsonAsync<Sponsor>("Sponsors", sponsor);
                posttask.Wait();

                var result = posttask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");   
                }

            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(sponsor);
        }

        // GET: Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var sponsor = await _context.Sponsors.FindAsync(id);

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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,URL,Email,Image,UppladdadBild")] Sponsor sponsor)
        {
            if (id != sponsor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using var memoryStream = new System.IO.MemoryStream();
                    await sponsor.UppladdadBild.CopyToAsync(memoryStream);
                    sponsor.Image = memoryStream.ToArray();
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
