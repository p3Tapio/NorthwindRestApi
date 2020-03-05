using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NorthwindRestApi.Models;


namespace NwRESTapi.Controllers
{

    [Route("northwind/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public List<Customers> GetAllCustomers()
        {
            NorthwindContext context = new NorthwindContext();
            var customers = context.Customers.ToList();
            context.Dispose();
            return customers;
        }
        [HttpGet]
        [Route("R")]
        public IActionResult GetSomeCustomers(int offset, int limit, string country)
        {

            NorthwindContext context = new NorthwindContext();
            List<Customers> customers = new List<Customers>();

            try
            {
                if (country != null)
                {
                    customers = context.Customers.Where(x => x.Country == country).Take(limit).ToList();
                }
                else
                {
                    customers = context.Customers.Skip(offset).Take(limit).ToList();
                }
                if (customers.Any())
                {
                    return Ok(customers);
                }
                else
                {
                    return NotFound("No customers found.");
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
        [HttpGet]
        [Route("{id}")]
        public ActionResult GetCustomerById(string id)
        {
            NorthwindContext context = new NorthwindContext();
            Customers customer = context.Customers.Find(id);
            context.Dispose();
            if (customer != null)
            {
                return Ok(customer);
            }
            else
            {
                return NotFound("Customer not found");
            }
        }
        [HttpGet]
        [Route("country/{country}")]
        public ActionResult GetCustomersByCountry(string country)
        {

            NorthwindContext context = new NorthwindContext();
            var customers = (from cust in context.Customers
                            where cust.Country == country
                            select cust).ToList();

            context.Dispose(); 

            if (customers.Any())
            {
                return Ok(customers);
            }
            else
            {
                return NotFound("Customers not found");
            }
        }
        [HttpGet]
        [Route("country")]
        public ActionResult GetDistinctCountries()
        {
            NorthwindContext context = new NorthwindContext();
            var countríes = ((from x in context.Customers
                              select x.Country).Distinct()).ToList();
            context.Dispose(); 
            return Ok(countríes); 
        }
        [HttpPost]
        [Route("")]
        public string CreateNewCustomer([FromBody] Customers customer)
        {
            NorthwindContext context = new NorthwindContext();
            try
            {
                context.Customers.Add(customer);
                context.SaveChanges();
                return "Customer '" + customer.CustomerId + " - " + customer.ContactName + "' created.";
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
        [Route("{id}")]
        public ActionResult UpdateCustomer([FromBody] Customers customerInput, string id)
        {
            NorthwindContext context = new NorthwindContext();
            try
            {
                Customers customerDb = context.Customers.Find(id);
                if (customerDb != null)
                {
                    customerDb.CustomerId = customerInput.CustomerId;
                    customerDb.CompanyName = customerInput.CompanyName;
                    customerDb.ContactName = customerInput.ContactName;
                    customerDb.ContactTitle = customerInput.ContactTitle;
                    customerDb.Address = customerInput.Address;
                    customerDb.City = customerInput.City;
                    customerDb.Region = customerInput.Region;
                    customerDb.PostalCode = customerInput.PostalCode;
                    customerDb.Country = customerInput.Country;
                    customerDb.Phone = customerInput.Phone;
                    customerDb.Fax = customerInput.Fax;

                    context.SaveChanges();
                    return Ok("Customer " + customerInput.CustomerId + " updated");
                }
                else
                {
                    return NotFound("Customer not found");
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
        public ActionResult DeleteCustomer(string id)
        {
            NorthwindContext context = new NorthwindContext();
            try
            {
                Customers customer = context.Customers.Find(id);
                if (customer != null)
                {
                    context.Customers.Remove(customer);
                    context.SaveChanges();
                    return Ok("Customer '" + customer.CustomerId + "' deleted.");
                }
                else
                {
                    return NotFound("Customer '" + id.ToUpper() + "' not found.");
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