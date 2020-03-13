using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;

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
                context.Logins.Add(login);
                context.SaveChanges();
                return "Login details for created.";
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
        public ActionResult UpdateLogin([FromBody] Logins loginInput, string id)
        {
            NorthwindContext context = new NorthwindContext();
            try
            {
                Logins loginDb = context.Logins.Find(id);
                if (loginDb != null)
                {
                    loginDb.LoginId = loginInput.LoginId;
                    loginDb.Firstname = loginInput.Firstname;
                    loginDb.Lastname = loginInput.Lastname;
                    loginDb.Email = loginInput.Email;
                    loginDb.Username = loginInput.Username;
                    loginDb.Password = loginInput.Password;
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
        public ActionResult DeleteLogin(string id)
        {
            NorthwindContext context = new NorthwindContext();
            try
            {
                Logins login = context.Logins.Find(id);
                if (login != null)
                {
                    context.Logins.Remove(login);
                    context.SaveChanges();
                    return Ok("Customer '" + login.LoginId + "' deleted.");
                }
                else
                {
                    return NotFound("Customer '" + id + "' not found.");
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