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
            var logs = context.Logins.ToList();
            context.Dispose();
            return logs;
        }
        [HttpPost]
        [Route("create")]
        public string CreateNewLogin([FromBody] Logins login)
        {

            NorthwindContext context = new NorthwindContext();
            try
            {
                context.Logins.Add(login);
                context.SaveChanges();
                return "Login details for " + login.LoginId + " created.";
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
        // TODO: UPDATE + DELETE
    }
}