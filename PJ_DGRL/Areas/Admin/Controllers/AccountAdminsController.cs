using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PJ_DGRL.Models.DGRLModels;

namespace PJ_DGRL.Areas.Admin.Controllers
{
    public class AccountAdminsController : BaseController
    {
        private readonly DbDgrlContext _context;

        public AccountAdminsController(DbDgrlContext context)
        {
            _context = context;
        }

        // GET: Admin/AccountAdmins
        public async Task<IActionResult> Index()
        {
            var admin = JsonConvert.DeserializeObject<AccountAdmin>(HttpContext.Session.GetString("AdminLogin"));
            if(admin.RoleId != 1)
            {
                return RedirectToAction("Index","Status");
            }
            var dbDgrlContext = _context.AccountAdmins.Include(a => a.Role);
            return View(await dbDgrlContext.ToListAsync());
        }

        // GET: Admin/AccountAdmins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var admin = JsonConvert.DeserializeObject<AccountAdmin>(HttpContext.Session.GetString("AdminLogin"));
            if (admin.RoleId != 1)
            {
                return RedirectToAction("Index", "Status");
            }
            var accountAdmin = await _context.AccountAdmins
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accountAdmin == null)
            {
                return NotFound();
            }
            return View(accountAdmin);
        }

        // GET: Admin/AccountAdmins/Create
        public async Task<IActionResult> Create()
        {
            var admin = JsonConvert.DeserializeObject<AccountAdmin>(HttpContext.Session.GetString("AdminLogin"));
            if (admin.RoleId != 1)
            {
                return RedirectToAction("Index", "Status");
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "RoleName");
            return View();
        }

        // POST: Admin/AccountAdmins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,UserName,Password,CreateBy,CreateDate,UpdateDate,IsActive,RoleId,Remember")] AccountAdmin accountAdmin)
        {
            if (ModelState.IsValid)
            {
                var admin = JsonConvert.DeserializeObject<AccountAdmin>(HttpContext.Session.GetString("AdminLogin"));
                if (admin.RoleId != 1)
                {   

                    return RedirectToAction("Index", "Status");
                }
                var acc = _context.AccountAdmins.FirstOrDefault(a => a.UserName == accountAdmin.UserName);
                if (acc != null)
                {
                    ViewBag.Status = "Tài khoản đã tồn tại!";
                    ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "RoleName");
                    return View(accountAdmin);
                }
                accountAdmin.Password = GetSHA26Hash(accountAdmin.Password);
                accountAdmin.CreateDate = DateTime.Now;
                accountAdmin.CreateBy = admin.UserName;
                _context.Add(accountAdmin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "RoleName", accountAdmin.RoleId);
            return View(accountAdmin);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var admin = JsonConvert.DeserializeObject<AccountAdmin>(HttpContext.Session.GetString("AdminLogin"));
            if (admin.RoleId != 1)
            {
                return RedirectToAction("Index", "Status");
            }
            var acc = await _context.AccountAdmins.FindAsync(id);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "RoleName");
            return View(acc);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,[Bind("Id,FullName,UserName,Password,CreateBy,CreateDate,UpdateDate,IsActive,RoleId,Remember")] AccountAdmin accountAdmin)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var admin = JsonConvert.DeserializeObject<AccountAdmin>(HttpContext.Session.GetString("AdminLogin"));
                if (admin.RoleId != 1)
                {

                    return RedirectToAction("Index", "Status");
                }
                accountAdmin.UpdateDate = DateTime.Now;
                accountAdmin.CreateBy = admin.UserName;
                _context.Update(accountAdmin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "RoleName", accountAdmin.RoleId);
            return View(accountAdmin);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var admin = JsonConvert.DeserializeObject<AccountAdmin>(HttpContext.Session.GetString("AdminLogin"));
            if (admin.RoleId != 1)
            {
                return RedirectToAction("Index", "Status");
            }
            var accountAdmin = await _context.AccountAdmins
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (accountAdmin.FullName == null)
            {
                return RedirectToAction("Index", "Status");
            }
            if (accountAdmin == null)
            {
                return NotFound();
            }

            return View(accountAdmin);
        }

        // POST: Admin/AccountAdmins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accountAdmin = await _context.AccountAdmins.FindAsync(id);
            if (accountAdmin != null)
            {
                _context.AccountAdmins.Remove(accountAdmin);
            }
            if (accountAdmin.FullName == null)
            {
                return RedirectToAction("Index", "Status");
            }
            var admin = JsonConvert.DeserializeObject<AccountAdmin>(HttpContext.Session.GetString("AdminLogin"));
            if (admin.RoleId != 1)
            {
                return RedirectToAction("Index", "Status");
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Lock(int id)
        {
            var admin = JsonConvert.DeserializeObject<AccountAdmin>(HttpContext.Session.GetString("AdminLogin"));
            
            var acc = await _context.AccountAdmins.FindAsync(id);
            if (acc.FullName == null)
            {
                return RedirectToAction("Index","Status");
            }
            acc.IsActive = 1;
            acc.CreateBy = admin.UserName;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UnLock(int id)
        {
            var admin = JsonConvert.DeserializeObject<AccountAdmin>(HttpContext.Session.GetString("AdminLogin"));
            var acc = await _context.AccountAdmins.FindAsync(id);
            acc.IsActive = 0;
            acc.CreateBy = admin.UserName;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Reset(int id)
        {
            var admin = JsonConvert.DeserializeObject<AccountAdmin>(HttpContext.Session.GetString("AdminLogin"));
            var acc = await _context.AccountAdmins.FindAsync(id);
            if (acc.FullName == null)
            {
                return RedirectToAction("Index", "Status");
            }
            acc.Password = GetSHA26Hash("12345");
            acc.CreateBy = admin.UserName;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        private bool AccountAdminExists(int id)
        {
            return _context.AccountAdmins.Any(e => e.Id == id);
        }
        
        static string GetSHA26Hash(string input)
        {
            string hash = "";
            using (var sha256 = new SHA256Managed())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
            return hash;
        }
    }
}
