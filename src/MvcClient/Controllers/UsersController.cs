using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcClient.IdentityModel;
using MvcClient.Models;
using MvcClient.ViewModels;

namespace MvcClient.Controllers
{
    [Authorize("Admin")]
    public class UsersController : Controller
    {
        public authsampleContext _ctx { get; }

        public UsersController(
            authsampleContext context)
        {
            _ctx = context;
        }
        // GET: UsersController
        public async Task<ActionResult> Index(string name = "")
        {
            var users = _ctx.AspNetUsers.AsQueryable().Include("AspNetUserClaims").Where(x => (x.FirstName + " " + x.LastName).ToLower().Contains(name.ToLower())).ToList();
            var models = users.Select(x => new UserViewModel(x)).ToList();
            return View(models);
        }
        // GET: UsersController/Details/5
        public ActionResult Details(string id)
        {
            var user = _ctx.AspNetUsers.Where(x => x.Id == id)?.FirstOrDefault();
            if (user == null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var model = new UserViewModel(user);
            return View(model);
        }
        public ActionResult ChangePassword(string id)
        {
            var user = _ctx.AspNetUsers.Where(x => x.Id == id).FirstOrDefault();
            var model = new ChangePasswordViewModel() { Id = id };
            return View(model);
        }

        // POST: UsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _ctx.AspNetUsers.Where(x => x.Id == model.Id).FirstOrDefault();
                var hash1 = user.PasswordHash;
                PasswordHasher<AspNetUsers> hasher = new PasswordHasher<AspNetUsers>();
                var hash = hasher.HashPassword(user, model.Password);
                //var res = hasher.VerifyHashedPassword(user, hash3, "alice");

                user.PasswordHash = hash;
                _ctx.Update(user);
                _ctx.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: UsersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    AspNetUsers user = new AspNetUsers();
                    user.UserName = model.Username;
                    user.Email = model.Email;
                    user.NormalizedEmail = model.Email.ToUpper();
                    user.NormalizedUserName = model.Username.ToUpper();
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.IsEnabled = true;
                    user.SecurityStamp = Guid.NewGuid().ToString();
                    PasswordHasher<AspNetUsers> hasher = new PasswordHasher<AspNetUsers>();
                    var hash = hasher.HashPassword(user, model.Password);
                    user.PasswordHash = hash;
                    user.Id = Guid.NewGuid().ToString();
                    _ctx.Add(user);
                    _ctx.SaveChanges();

                    AspNetUserClaims claim = new AspNetUserClaims();
                    claim.UserId = user.Id;
                    claim.ClaimType = JwtClaimTypes.Name;
                    claim.ClaimValue = model.Username;
                    _ctx.Add(claim);

                    if (model.IsAdmin)
                    {
                        claim = new AspNetUserClaims();
                        claim.UserId = user.Id;
                        claim.ClaimType = JwtClaimTypes.Role;
                        claim.ClaimValue = "admin";
                        _ctx.Add(claim);
                    }


                    claim = new AspNetUserClaims();
                    claim.UserId = user.Id;
                    claim.ClaimType = JwtClaimTypes.GivenName;
                    claim.ClaimValue = model.FirstName;
                    _ctx.Add(claim);

                    claim = new AspNetUserClaims();
                    claim.UserId = user.Id;
                    claim.ClaimType = JwtClaimTypes.FamilyName;
                    claim.ClaimValue = model.LastName;
                    _ctx.Add(claim);

                    claim = new AspNetUserClaims();
                    claim.UserId = user.Id;
                    claim.ClaimType = JwtClaimTypes.Email;
                    claim.ClaimValue = model.Email;
                    _ctx.Add(claim);

                    claim = new AspNetUserClaims();
                    claim.UserId = user.Id;
                    claim.ClaimType = JwtClaimTypes.EmailVerified;
                    claim.ClaimValue = true.ToString();
                    _ctx.Add(claim);

                    claim = new AspNetUserClaims();
                    claim.UserId = user.Id;
                    claim.ClaimType = JwtClaimTypes.Scope;
                    claim.ClaimValue = "api1";
                    _ctx.Add(claim);

                    _ctx.SaveChanges();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    return View();
                }
            }
            return View();
        }

        // GET: UsersController/Edit/5
        public ActionResult Edit(string id)
        {
            var user = _ctx.AspNetUsers.Where(x => x.Id == id).FirstOrDefault();
            if(user==null)
            {
                return RedirectToAction("NotFound", "Error");
            }
            var model = new EditViewModel();
            model.Id = id;
            model.Username = user.UserName;
            model.Email = user.Email;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
            var claim = _ctx.AspNetUserClaims.Where(x => x.UserId == user.Id &&
                  x.ClaimType == JwtClaimTypes.Role &&
                  x.ClaimValue == "admin")?.FirstOrDefault();

            if (claim != null) model.IsAdmin = true;

                return View(model);
        }

        // POST: UsersController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, EditViewModel model)
        {
            try
            {
                var user = _ctx.AspNetUsers.Where(x => x.Id == id)?.FirstOrDefault();
                if (user == null)
                {
                    return RedirectToAction("NotFound", "Error");
                }
                user.UserName = model.Username;
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;

                if (!string.IsNullOrEmpty(model.Password))
                {
                    PasswordHasher<AspNetUsers> hasher = new PasswordHasher<AspNetUsers>();
                    var hash = hasher.HashPassword(user, model.Password);
                    user.PasswordHash = hash;
                }
                _ctx.Update(user);

                var claim = _ctx.AspNetUserClaims.Where(x => x.UserId == user.Id &&
                  x.ClaimType == JwtClaimTypes.Role &&
                  x.ClaimValue == "admin")?.FirstOrDefault();

                if (model.IsAdmin && claim == null)
                {
                    AspNetUserClaims claim1 = new AspNetUserClaims();
                    claim1.UserId = user.Id;
                    claim1.ClaimType = JwtClaimTypes.Role;
                    claim1.ClaimValue = "admin";

                    _ctx.Add(claim1);
                }
                else if (!model.IsAdmin && claim != null)
                {
                    _ctx.Remove(claim);
                }

                _ctx.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        //// GET: UsersController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}


        public ActionResult ChangeState(string id, bool enable)
        {
            try
            {
                var user = _ctx.AspNetUsers.Where(x => x.Id == id).FirstOrDefault();
                user.IsEnabled = enable;
                _ctx.Update(user);
                _ctx.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
