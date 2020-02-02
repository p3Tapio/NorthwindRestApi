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
    public class OrdersController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public List<Orders> GetAllOrders()
        {
            NorthwindContext context = new NorthwindContext();
            var orders = context.Orders.ToList();
            context.Dispose();
            return orders;
        }
        [HttpGet]
        [Route("{id}")]
        public ActionResult GetOrderById(int id)
        {
            NorthwindContext context = new NorthwindContext();
            var order = context.Orders.Find(id);
            if (order != null)
            {
                return Ok(order);
            }
            else
            {
                return NotFound("Order " + id + " not found");
            }

        }
        [HttpGet]
        [Route("customer/{customerid}")]
        public ActionResult GetOrdersByCustomerId(string customerId)
        {
            NorthwindContext context = new NorthwindContext();
            var OrdersByCustomer = from ord in context.Orders
                                   where ord.CustomerId == customerId
                                   select ord;
            if (OrdersByCustomer.Any())
            {
                return Ok(OrdersByCustomer);
            }
            else
            {
                return NotFound("No orders found with Id " + customerId.ToUpper());
            }
        }
        [HttpPost]
        [Route("")]
        public ActionResult CreateNewOrder([FromBody] Orders newOrder)
        {
            NorthwindContext context = new NorthwindContext();
            try
            {
                context.Orders.Add(newOrder);
                context.SaveChanges();
                return Ok("Order " + newOrder.OrderId + " created.");
            }
            catch (Exception ex)
            {
                return BadRequest("Woops, something went wrong!\n" + ex.GetType() + ": " + ex.Message);
            }
            finally
            {
                context.Dispose();
            }
        }
        [HttpPut]
        [Route("{id}")]
        public ActionResult UpdateOrder([FromBody] Orders orderInput, int id)
        {
            NorthwindContext context = new NorthwindContext();

            try
            {
                Orders orderDb = context.Orders.Find(id);
                if (orderDb != null)
                {
                    orderDb.CustomerId = orderInput.CustomerId;
                    orderDb.EmployeeId = orderInput.EmployeeId;
                    orderDb.OrderDate = orderInput.OrderDate;
                    orderDb.RequiredDate = orderInput.RequiredDate;
                    orderDb.ShippedDate = orderInput.ShippedDate;
                    orderDb.ShipVia = orderInput.ShipVia;
                    orderDb.Freight = orderInput.Freight;
                    orderDb.ShipName = orderInput.ShipName;
                    orderDb.ShipAddress = orderInput.ShipAddress;
                    orderDb.ShipCity = orderInput.ShipCity;
                    orderDb.ShipRegion = orderInput.ShipRegion;
                    orderDb.ShipPostalCode = orderInput.ShipPostalCode;
                    orderDb.ShipCountry = orderInput.ShipCountry;
                    orderDb.Customer = orderInput.Customer;
                    orderDb.Employee = orderInput.Employee;
                    orderDb.ShipViaNavigation = orderInput.ShipViaNavigation;

                    context.SaveChanges();
                    return Ok("Order " + orderDb.OrderId + " updated");
                }
                else
                {
                    return NotFound("Order not found");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Order update failed!\n" + ex.GetType() + ": " + ex.Message);
            }
            finally
            {
                context.Dispose();
            }

        }
        [HttpDelete]
        [Route("delete/{id}")]
        public ActionResult DeleteOrder(int id)
        {
            NorthwindContext context = new NorthwindContext();
            try
            {
                Orders order = context.Orders.Find(id);
                if (order != null)
                {
                    context.Orders.Remove(order);
                    context.SaveChanges();
                    return Ok("Order " + order.OrderId + " deleted.");
                }
                else
                {
                    return NotFound("Order " + id + " not found");
                }

            }
            catch (Exception ex)
            {
                return BadRequest("Delete failed\n" + ex.GetType() + ": " + ex.Message);
            }
            finally
            {
                context.Dispose();
            }
        }




    }
}