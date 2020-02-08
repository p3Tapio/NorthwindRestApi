using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;

namespace NorthwindRestApi.Controllers
{
    [Route("northwind/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public List<Employees> GetAllEmps()
        {
            // Metodeissa tehty uudet listaukset, joka ilman kuvaa, koska "photo" näyttää sekoittavan palautuksen

            NorthwindContext context = new NorthwindContext();
            var emps = (from emp in context.Employees
                        select new Employees
                        {
                            EmployeeId = emp.EmployeeId,
                            LastName = emp.LastName,
                            FirstName = emp.FirstName,
                            Title = emp.Title,
                            TitleOfCourtesy = emp.TitleOfCourtesy,
                            BirthDate = emp.BirthDate,
                            HireDate = emp.HireDate,
                            Address = emp.Address,
                            City = emp.City,
                            Region = emp.Region,
                            PostalCode = emp.PostalCode,
                            Country = emp.Country,
                            HomePhone = emp.HomePhone,
                            Extension = emp.Extension,
                            Notes = emp.Notes,
                            ReportsTo = emp.ReportsTo,
                            PhotoPath = emp.PhotoPath

                        }).ToList();

            context.Dispose(); 

            return emps;

        }
        [HttpGet]
        [Route("{id}")]
        public ActionResult GetEmployeeById(int id)
        {
            NorthwindContext context = new NorthwindContext();

            var employee = (from emp in context.Employees
                           where emp.EmployeeId == id
                           select new Employees
                           {
                               EmployeeId = emp.EmployeeId,
                               LastName = emp.LastName,
                               FirstName = emp.FirstName,
                               Title = emp.Title,
                               TitleOfCourtesy = emp.TitleOfCourtesy,
                               BirthDate = emp.BirthDate,
                               HireDate = emp.HireDate,
                               Address = emp.Address,
                               City = emp.City,
                               Region = emp.Region,
                               PostalCode = emp.PostalCode,
                               Country = emp.Country,
                               HomePhone = emp.HomePhone,
                               Extension = emp.Extension,
                               Notes = emp.Notes,
                               ReportsTo = emp.ReportsTo,
                               PhotoPath = emp.PhotoPath

                           }).ToList();

            context.Dispose();
            if (employee.Count>0)
            {
                return Ok(employee);
            }
            else
            {
                return NotFound("Employee id " + id + " not found");
            }
        }
        [HttpGet]
        [Route("city/{city}")]
        public ActionResult GetEmployeeByCity(string city)
        {
            NorthwindContext context = new NorthwindContext();
            var employee = (from emp in context.Employees
                            where emp.City == city
                            select new Employees
                            {
                                EmployeeId = emp.EmployeeId,
                                LastName = emp.LastName,
                                FirstName = emp.FirstName,
                                Title = emp.Title,
                                TitleOfCourtesy = emp.TitleOfCourtesy,
                                BirthDate = emp.BirthDate,
                                HireDate = emp.HireDate,
                                Address = emp.Address,
                                City = emp.City,
                                Region = emp.Region,
                                PostalCode = emp.PostalCode,
                                Country = emp.Country,
                                HomePhone = emp.HomePhone,
                                Extension = emp.Extension,
                                Notes = emp.Notes,
                                ReportsTo = emp.ReportsTo,
                                PhotoPath = emp.PhotoPath

                            }).ToList();

            context.Dispose();

            if (employee.Any())
            {
                return Ok(employee);
            }
            else
            {
                return NotFound("Employees not found");
            }
        }
        [HttpPost]
        [Route("create")]
        public string CreateNewEmployee([FromBody] Employees employee)
        {
            NorthwindContext context = new NorthwindContext();
            try
            {
                context.Employees.Add(employee);
                context.SaveChanges();
                return "Employee " + employee.EmployeeId + " created.";
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
        [HttpPut]
        [Route("update/{id}")]
        public ActionResult UpdateEmployee([FromBody] Employees empInput, int id)
        {
            NorthwindContext context = new NorthwindContext();
            try
            {
                Employees empDb = context.Employees.Find(id);
                if (empDb != null)
                {
                    empDb.LastName = empInput.LastName;
                    empDb.FirstName = empInput.FirstName;
                    empDb.Title = empInput.Title;
                    empDb.TitleOfCourtesy = empInput.TitleOfCourtesy;
                    empDb.BirthDate = empInput.BirthDate;
                    empDb.HireDate = empInput.HireDate;
                    empDb.Address = empInput.Address;
                    empDb.City = empInput.City;
                    empDb.Region = empInput.Region;
                    empDb.PostalCode = empInput.PostalCode;
                    empDb.Country = empInput.Country;
                    empDb.HomePhone = empInput.HomePhone;
                    empDb.Extension = empInput.Extension;
                    empDb.Notes = empInput.Notes;
                    empDb.ReportsTo = empInput.ReportsTo;
                    empDb.PhotoPath = empInput.PhotoPath;

                    context.SaveChanges();
                    return Ok("Employee " + empDb.EmployeeId + " updated");
                }
                else
                {
                    return NotFound("Employee not found");
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
        public ActionResult DeleteEmployee(int id)
        {
            NorthwindContext context = new NorthwindContext();
            try
            {
                Employees emp = context.Employees.Find(id);
                if (emp != null)
                {
                    context.Employees.Remove(emp);
                    context.SaveChanges();
                    return Ok("Employees '" + emp.EmployeeId + "' deleted.");
                }
                else
                {
                    return NotFound("Employee '" + id + "' not found.");
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