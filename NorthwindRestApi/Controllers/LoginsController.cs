using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;
using NorthwindRestApi.Tools; 

namespace NorthwindRestApi.Controllers
{
    [Route("northwind/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public List<Logins> GetAllLogins()
        {
   
            NorthwindContext context = new NorthwindContext();
            var logs = (from log in context.Logins
                        select new Logins
                        {
                            LoginId = log.LoginId,
                            Firstname = log.Firstname,
                            Lastname = log.Lastname,
                            Email = log.Email,
                            Username = log.Username,
                            AccesslevelId = log.AccesslevelId

                        }).ToList();

            context.Dispose();
            return logs;
        }
        [HttpPost]
        [Route("")]
        public string CreateNewLogin([FromBody] Logins login)
        {

            NorthwindContext context = new NorthwindContext();
            try
            {
           
                Logins newLogin = new Logins();
                newLogin.Firstname = login.Firstname;
                newLogin.Lastname = login.Lastname;
                newLogin.Email = login.Email;
                newLogin.Username = login.Username;
                newLogin.Password = PasswordHash.Hasher(login.Password);
                newLogin.AccesslevelId = login.AccesslevelId; 

                context.Logins.Add(newLogin);
                context.SaveChanges();
                return "Login details created.";
            }
            catch (Exception ex)
            {
                return "Woops, something went wrong! \n" + ex.GetType() + ": " + ex.Message;
            }
            finally
            {
                context.Dispose();
            }

        }
        [HttpGet]
        [Route("{lastname}")]
        public List<Logins> GetLogsByLastName(string lastname)
        {
            NorthwindContext context = new NorthwindContext();
            var logins = (from x in context.Logins
                          where x.Lastname.Contains(lastname)
                          select x).ToList();

            context.Dispose();
            return logins;
        }
        [HttpPut]
        [Route("{id}")]
        public ActionResult UpdateLogin([FromBody] Logins loginInput, int id)
        {
            NorthwindContext context = new NorthwindContext();
            try
            {
                Logins loginDb = context.Logins.Find(id);
                if (loginDb != null)
                {

                    loginDb.Firstname = loginInput.Firstname;
                    loginDb.Lastname = loginInput.Lastname;
                    loginDb.Email = loginInput.Email;
                    loginDb.Username = loginInput.Username;
                    //loginDb.Password = loginInput.Password;
                    loginDb.AccesslevelId = loginInput.AccesslevelId;

                    context.SaveChanges();
                    return Ok("Login details for " + loginInput.LoginId + " updated.");
                }
                else
                {
                    return NotFound("Login details not found");
                }
            }
            catch
            {
                return BadRequest();
            }
            finally
            {
                context.Dispose();
            }
        }
        [HttpDelete]
        [Route("delete/{id}")]
        public ActionResult DeleteLogin(int id)
        {
            NorthwindContext context = new NorthwindContext();
            try
            {
                Logins login = context.Logins.Find(id);
                if (login != null)
                {
                    context.Logins.Remove(login);
                    context.SaveChanges();
                    return Ok("User '" + login.LoginId + "' deleted.");
                }
                else
                {
                    return NotFound("User '" + id + "' not found.");
                }
            }
            catch
            {
                return BadRequest("Delete failed");
            }
            finally
            {
                context.Dispose();
            }
        }

    }
}
// login:
// ks https://github.com/p3Tapio/OmniaPruju/blob/master/UsersCtrls/Tools/UserService.cs
// tarviit väliaikaisesti feikki tokenin tai sen auth osan tuolta, tsekkaa php-versiosta vaikka miten toteutit sen