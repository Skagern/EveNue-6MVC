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
using System.Net.Http.Json;
using System.Text;

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
            var response = await client.GetAsync("http://193.10.202.76/MailAPI/api/MailList/getMailList");
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

            var mailListModel = await _context.MailListModel
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
        public async Task<IActionResult> Create([Bind("Id,Mailadress")] MailListModel mailListModel)
        {
            if (ModelState.IsValid)
            {
                HttpClient client = new HttpClient();
                var stringContent = new StringContent(JsonConvert.SerializeObject(mailListModel), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://193.10.202.76/MailAPI/api/MailList/addMail", stringContent);
                return RedirectToAction(nameof(Index));
            }
            return View(mailListModel);
        }

        // GET: Mails/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            HttpClient client = new HttpClient();
            var response = await client.GetAsync("http://193.10.202.76/MailAPI/api/MailList/getMailListById?id=" + id);
            MailListModel mailListModel = await response.Content.ReadAsAsync<MailListModel>();
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
                    HttpClient client = new HttpClient();
                    var stringContent = new StringContent(JsonConvert.SerializeObject(mailListModel), Encoding.UTF8, "application/json");
                    var response = await client.PutAsync("http://193.10.202.76/MailAPI/api/MailList/editMail?id=" + id, stringContent);
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

            var mailListModel = await _context.Sponsors
               .FirstOrDefaultAsync(m => m.Id == id);
            HttpClient client = new HttpClient();
            var response = await client.DeleteAsync("http://193.10.202.76/MailAPI/api/MailList/deleteMail?id=" + id);

            return View(mailListModel);
        }

        // POST: Mails/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            HttpClient client = new HttpClient();
            var response = await client.DeleteAsync("http://193.10.202.76/MailAPI/api/MailList/deleteMail?id=" + id);
            return RedirectToAction(nameof(Index));
        }

        private bool MailListModelExists(int id)
        {
            return _context.MailListModel.Any(e => e.Id == id);
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
            HttpClient client = new ();
            client.BaseAddress = new Uri("http://193.10.202.76/mailapi/api/email");
            var response = await client.PostAsJsonAsync<EmailModel>("email", emailmodel);

            return RedirectToAction("Index","Admin");
        }

        public IActionResult SendToAll()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendToAll(EmailModel emailmodel)
        {

            // List<MailListModel> email = new();
            //HttpClient client = new HttpClient();
            //var response = await client.GetAsync("http://193.10.202.76/MailAPI/api/MailList");
            //string jsonresponse = await response.Content.ReadAsStringAsync();
            //email = JsonConvert.DeserializeObject<List<MailListModel>>(jsonresponse);
            //var email = await this.Index();
            //if (email != null)
            //{
            //   List<String> adress = new List<String>();
            //  foreach (MailListModel mailListModel in email)
            // {
            //    adress.Add(mailListModel.Mailadress);
            //}
            //var result = String.Join(",", adress);
            //emailmodel.To = result; 
            //HttpClient client1 = new();
            //client1.BaseAddress = new Uri("http://193.10.202.76/mailapi/api/email");
            //var response1 = await client1.PostAsJsonAsync<EmailModel>("email", emailmodel);

            Example emailexample = new Example();
            await emailexample.ExecuteAll(emailmodel.Subject, emailmodel.Body, emailmodel.Body);
        
            return RedirectToAction("Index", "Admin");
        }

        //public Task<Response> Execute()
        //{
        //    Example emailexample = new Example();
        //    var response = emailexample.Execute();

        //    return response;

        //}
    }

    


}
