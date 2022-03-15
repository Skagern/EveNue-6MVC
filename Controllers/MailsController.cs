using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GruppNrSexMVC.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using static GruppNrSexMVC.Models.Example;
using System.Net.Http;
using Newtonsoft.Json;


namespace GruppNrSexMVC.Controllers
{
    public class MailsController : Controller
    {
        private readonly SponsorContext _context;

        public MailsController(SponsorContext context)
        {
            _context = context;
        }

        // GET: Mails
        public async Task<IActionResult> Index()
        {
            List<MailListModel> email = new List<MailListModel>();
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://193.10.202.76/MailAPI/api/MailListModels");
            string jsonresponse = await response.Content.ReadAsStringAsync();
            email = JsonConvert.DeserializeObject<List<MailListModel>>(jsonresponse);

            return View(email);
        }

        // GET: Mails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mailListModel = await _context.MailList
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mailListModel == null)
            {
                return NotFound();
            }

            return View(mailListModel);
        }

        // GET: Mails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Mailadress")] MailListModel mailListModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mailListModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mailListModel);
        }

        // GET: Mails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mailListModel = await _context.MailList.FindAsync(id);
            if (mailListModel == null)
            {
                return NotFound();
            }
            return View(mailListModel);
        }

        // POST: Mails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Mailadress")] MailListModel mailListModel)
        {
            if (id != mailListModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mailListModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MailListModelExists(mailListModel.Id))
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
            return View(mailListModel);
        }

        // GET: Mails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mailListModel = await _context.MailList
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mailListModel == null)
            {
                return NotFound();
            }

            return View(mailListModel);
        }

        // POST: Mails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mailListModel = await _context.MailList.FindAsync(id);
            _context.MailList.Remove(mailListModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MailListModelExists(int id)
        {
            return _context.MailList.Any(e => e.Id == id);
        }

        public IActionResult Send()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(EmailModel emailmodel)
        {
            ViewData["Message"] = "Email Sent!!!...";
            Example emailexample = new Example();
            await emailexample.Execute(emailmodel.To, emailmodel.Subject, emailmodel.Body
                , emailmodel.Body);

            return View();
        }

        //public bool Execute()
        //{
        //    Example emailexample = new Example();
        //    emailexample.Execute().Wait();
        //    return true;

        //}
    }

    


}
