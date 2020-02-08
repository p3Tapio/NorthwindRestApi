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
        [Route("{orderid}/{productid}")]
        public ActionResult GetOrderDetailByIds(int orderId, int productId)
        {
            NorthwindContext context = new NorthwindContext();
            OrderDetails order = context.OrderDetails.Find(orderId, productId);  
            context.Dispose();

            if (order!=null)
            {
                return Ok(order);
            }
            else
            {
                return NotFound("Order detail not found.");
            }

        }
        [HttpGet]
        [Route("productid/{id}")]
        public ActionResult GetOrderDetailsByProductId(int id)
        {
            NorthwindContext context = new NorthwindContext();
            var ordersByProductId = (from prod in context.OrderDetails
                                    where prod.ProductId == id
                                    select prod).ToList();
            context.Dispose(); 

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
        [HttpPut]
        [Route("update/{orderid}/{productid}")]
        public ActionResult UpdateOrderDetail([FromBody] OrderDetails orderInput, int orderId, int productId) {
            NorthwindContext context = new NorthwindContext();

            try {
                OrderDetails orderDb = context.OrderDetails.Find(orderId, productId);

                if (orderDb != null) {

                    orderDb.UnitPrice = orderInput.UnitPrice;
                    orderDb.Quantity = orderInput.Quantity;
                    orderDb.Discount = orderInput.Discount;

                    context.SaveChanges();
                    return Ok("Order detail updated.");

                } else {
                    return NotFound("Order detail not found");
                }
            } catch (Exception ex) {
                return BadRequest("Update failed!\n" + ex.GetType() + ": " + ex.Message);
            } finally {
                context.Dispose();
            }
        }
        [HttpDelete]
        [Route("delete/{orderid}/{productid}")]
        public ActionResult DeleteOrderDetail(int orderId, int productId) {
            NorthwindContext context = new NorthwindContext();
            try {
                OrderDetails ord = context.OrderDetails.Find(orderId, productId);
                if (ord != null) {
                    context.OrderDetails.Remove(ord);
                    context.SaveChanges();
                    return Ok("Order detail deleted.");
                } else {
                    return NotFound("Order detail not found");
                }
            } catch (Exception ex) {
                return BadRequest("Delete failed\n" + ex.GetType() + ": " + ex.Message);
            } finally {
                context.Dispose();
            }
        }



    }
}