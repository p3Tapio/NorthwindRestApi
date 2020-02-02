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
    public class OrderdetailsController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public List<OrderDetails> GetAllOrderDetails()
        {
            NorthwindContext context = new NorthwindContext();
            var orderDetails = context.OrderDetails.ToList();
            context.Dispose();
            return orderDetails;
        }
        [HttpGet]
        [Route("{id}")] 
        public ActionResult GetOrderDetailById(int id)
        {
            NorthwindContext context = new NorthwindContext();
            var ordersByOrderId = (from ord in context.OrderDetails
                                    where ord.OrderId == id
                                    select ord).ToList();
            context.Dispose(); 

            if (ordersByOrderId.Any())
            {
                return Ok(ordersByOrderId);
            }
            else
            {
                return NotFound("Order details for id " + id + " not found");
            }

        }
        [HttpGet]
        [Route("productid/{id}")]
        public ActionResult GetOrderDetailsByProductId(int id)
        {
            NorthwindContext context = new NorthwindContext();
            var ordersByProductId = from prod in context.OrderDetails
                                    where prod.ProductId == id
                                    select prod;
            if (ordersByProductId.Any())
            {
                return Ok(ordersByProductId);
            }
            else
            {
                return NotFound("No data for product id " + id + " found");

            }
        }
        [HttpPost]
        [Route("create")]
        public ActionResult CreateNewOrderDetail([FromBody] OrderDetails orderDetail)
        {
 
            NorthwindContext context = new NorthwindContext();
            try
            {
                context.OrderDetails.Add(orderDetail);
                context.SaveChanges();
                return Ok("Order detail created");
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

    }
}