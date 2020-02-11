using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoginRegistration.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace LoginRegistration.Controllers
{
    public class LoginController : Controller
    {
        // database setup
        public LoginContext dbContext;
        public LoginController(LoginContext context)
        {
            dbContext = context;
        }
        // routes
        [HttpGet("")]
        public IActionResult Login(){
            return View();
        }
        [HttpGet("register")]
        public IActionResult Register(){
            return View();
        }
        [HttpPost("submitregister")]
        public IActionResult SubmitRegister(User newUser) {
            if (ModelState.IsValid) { // pass validations
                if (dbContext.Users.Any(u => u.Email == newUser.Email))
                { // email in use
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Register");
                }
                else { // valid, email not in use, go ahead and register
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                    dbContext.Add(newUser);
                    dbContext.SaveChanges();
                    User thisUser = dbContext.Users.FirstOrDefault(u => u.Email == newUser.Email);
                    HttpContext.Session.SetInt32("UserId", thisUser.UserId);
                    return RedirectToAction("Index");
                }
            }
            else { // failed validations
                return View("Register");
            }
        }
        [HttpPost("submitlogin")]
        public IActionResult SubmitLogin(LoginUser loginUser) {
            if (ModelState.IsValid) {
                var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == loginUser.Email);
                if (userInDb == null) { // email not found in db
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Login");
                }
                PasswordHasher<LoginUser> Hasher = new PasswordHasher<LoginUser>();
                var result = Hasher.VerifyHashedPassword(loginUser, userInDb.Password, loginUser.Password);
                if (result == 0) { // password doesn't match
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Login");
                }
                else { // success
                    HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                    return RedirectToAction("Index");
                }
            }
            else { // failed validations
                return View("Login");
            }
        }
        [HttpGet("index")]
        public IActionResult Index()
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            if (userid == null) {
                ModelState.AddModelError("Email", "You must login before you can view that page!");
                return View("Login");
            }
            return View();
        }
        [HttpGet("logout")]
        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // other methods














        // built in stuff
        

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
