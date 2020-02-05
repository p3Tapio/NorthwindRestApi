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
                                    where ord.DetailId == id
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
        [HttpPut]
        [Route("update/{detailid}")] // 1. 2-part composite key, joten find ei toimi, tai 2 update/{detailid}/{orderid} niin ei löydä 
                                     // Pura aikaisempi PK OrderIdstä?
        public ActionResult UpdateOrderDetail([FromBody] OrderDetails orderInput, int detailId) {
            NorthwindContext context = new NorthwindContext();

            try {
                OrderDetails orderDb = context.OrderDetails.Find(detailId);

                if (orderDb != null) {
                    orderDb.OrderId = orderInput.OrderId;
                    orderDb.ProductId = orderInput.ProductId;
                    orderDb.UnitPrice = orderInput.UnitPrice;
                    orderDb.Quantity = orderInput.Quantity;
                    orderDb.Discount = orderInput.Discount;

                    context.SaveChanges();
                    return Ok("Order detail " + orderDb.DetailId + " updated.");

                } else {
                    return NotFound("Order detail not found");
                }
            } catch (Exception ex) {
                return BadRequest("Update failed!\n" + ex.GetType() + ": " + ex.Message);
            } finally {
                context.Dispose();
            }
        }
        // Sama Exception: two part composite "Key"
        [HttpDelete]
        [Route("delete/{id}")]
        public ActionResult DeleteOrderDetail(int id) {
            NorthwindContext context = new NorthwindContext();
            try {
                OrderDetails ord = context.OrderDetails.Find(id);
                if (ord != null) {
                    context.OrderDetails.Remove(ord);
                    context.SaveChanges();
                    return Ok("Order " + id + " deleted.");
                } else {
                    return NotFound("Order " + id + " not found");
                }
            } catch (Exception ex) {
                return BadRequest("Delete failed\n" + ex.GetType() + ": " + ex.Message);
            } finally {
                context.Dispose();
            }
        }



    }
}