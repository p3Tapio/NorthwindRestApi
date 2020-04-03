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

        [HttpPost]
        [Route("singin")]
        public IActionResult Authenticate([FromBody] Logins login)
        {
            NorthwindContext context = new NorthwindContext();
            try
            {
                var pass = PasswordHash.Hasher(login.Password);
                var log = context.Logins.SingleOrDefault(x => x.Username == login.Username && x.Password == pass);

                if (log == null)
                {
                    return BadRequest("{ \"message\":\"Väärä käyttäjätunnus tai salasana\"}");
                }
                else
                {
                    string token = TokenGenerator.GenerateToken(log.Username);

                    string userJson = $"{{ \"user\": {{" +
                        $"\"userId\": \"{log.LoginId}\"," +
                        $"\"username\":\"{log.Username}\"," +
                        $"\"email\":\"{log.Email}\"}}," +
                        $"\"token\":\"{token}\"}}";

                    return Ok(userJson);
                }

            }
            catch (Exception ex)
            {
                return BadRequest("{\"message\":\"Woops, joku meni vikaan! " + ex.GetType() + " - " + ex.Message + "\"}");
            }
            finally
            {
                context.Dispose();
            }


        }

        [HttpPost]
        [Route("")]
        public IActionResult CreateNewLogin([FromBody] Logins login)
        {

            NorthwindContext context = new NorthwindContext();
            try
            {
                var check = context.Logins.SingleOrDefault(x => x.Username == login.Username);

                if (check == null)
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
                    return Ok("Käyttäjä luotu.");
                }
                else
                {
                    var kaytossa = "{\"message\":\"Käyttäjätunnus " + login.Username + " on jo käytössä. Valitse toinen tunnus\"}";
                    return BadRequest(kaytossa);
                }
            }
            catch (Exception ex)
            {
                var error = "{\"message\":\"Woops, joku meni vikaan! " + ex.GetType() + " - " + ex.Message + "\"}";
                return BadRequest(error);
            }
            finally
            {
                context.Dispose();
            }
        }
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
