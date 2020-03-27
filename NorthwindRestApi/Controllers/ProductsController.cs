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
    public class ProductsController : ControllerBase
    {
        [HttpGet]
        [Route("R")]
        public IActionResult GetSomeProducts(int offset, int limit)
        {
            NorthwindContext context = new NorthwindContext();
            try
            {
                var products = (from p in context.Products
                                join cats in context.Categories
                                on p.CategoryId equals cats.CategoryId
                                orderby p.ProductId
                                select new
                                {
                                    ProductId = p.ProductId,
                                    ProductName = p.ProductName,
                                    CategoryId = cats.CategoryId,
                                    CategoryName = cats.CategoryName,
                                    CategoryDescription = cats.Description,
                                    SupplierId = p.SupplierId,
                                    Quantity = p.QuantityPerUnit,
                                    UnitPrice = p.UnitPrice,
                                    UnitsInStock = p.UnitsInStock,
                                    UnitsOnOrder = p.UnitsOnOrder,
                                    ReorderLevel = p.ReorderLevel,
                                    Discontinued = p.Discontinued

                                }).Skip(offset).Take(limit).ToList();



                if (products.Any())
                {
                    return Ok(products);
                }
                else
                {
                    return NotFound("No prods");
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
        [Route("")]
        public List<Products> GetAllProducts()
        {
            NorthwindContext context = new NorthwindContext();
            var products = context.Products.ToList();
            context.Dispose();
            return products;
        }
        [HttpGet]
        [Route("{id}")]
        public ActionResult GetProductById(int id)
        {
            NorthwindContext context = new NorthwindContext();
            Products product = context.Products.Find(id);
            context.Dispose();
            if (product != null)
            {
                return Ok(product);
            }
            else
            {
                return NotFound("Product id " + id + " not found");
            }
        }
        [HttpGet]
        [Route("category/{cat}")]
        public ActionResult GetProductsByCategoryId(int cat)
        {

            NorthwindContext context = new NorthwindContext();
            var products = (from p in context.Products
                            join cats in context.Categories
                            on p.CategoryId equals cats.CategoryId
                            where p.CategoryId == cat
                            orderby p.ProductId
                            select new
                            {
                                ProductId = p.ProductId,
                                ProductName = p.ProductName,
                                CategoryId = cats.CategoryId,
                                CategoryName = cats.CategoryName,
                                CategoryDescription = cats.Description,
                                SupplierId = p.SupplierId,
                                Quantity = p.QuantityPerUnit,
                                UnitPrice = p.UnitPrice,
                                UnitsInStock = p.UnitsInStock,
                                UnitsOnOrder = p.UnitsOnOrder,
                                ReorderLevel = p.ReorderLevel,
                                Discontinued = p.Discontinued

                            }).ToList();

            context.Dispose();

            if (products.Any())
            {
                return Ok(products);
            }
            else
            {
                return NotFound("Products not found");
            }
        }
        [HttpPost]
        [Route("create")]
        public string CreateNewProduct([FromBody] Products product)
        {
            NorthwindContext context = new NorthwindContext();
            try
            {
                context.Products.Add(product);
                context.SaveChanges();
                return "Product " + product.ProductId + " created.";
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
        public ActionResult UpdateProduct([FromBody] Products productInput, int id)
        {
            NorthwindContext context = new NorthwindContext();
            try
            {
                Products productDb = context.Products.Find(id);
                if (productDb != null)
                {
                    productDb.ProductName = productInput.ProductName;
                    productDb.SupplierId = productInput.SupplierId;
                    productDb.CategoryId = productInput.CategoryId;
                    productDb.QuantityPerUnit = productInput.QuantityPerUnit;
                    productDb.UnitPrice = productInput.UnitPrice;
                    productDb.UnitsInStock = productInput.UnitsInStock;
                    productDb.UnitsOnOrder = productInput.UnitsOnOrder;
                    productDb.ReorderLevel = productInput.ReorderLevel;
                    productDb.Discontinued = productInput.Discontinued;

                    context.SaveChanges();
                    return Ok("Product " + productDb.ProductId + " updated");
                }
                else
                {
                    return NotFound("Product not found");
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
        public ActionResult DeleteProduct(int id)
        {
            NorthwindContext context = new NorthwindContext();
            try
            {
                Products product = context.Products.Find(id);
                if (product != null)
                {
                    context.Products.Remove(product);
                    context.SaveChanges();
                    return Ok("Product '" + product.ProductId + "' deleted.");
                }
                else
                {
                    return NotFound("Product '" + id + "' not found.");
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
        [HttpGet]
        [Route("categories")]
        public ActionResult GetDistinctCats()
        {
            NorthwindContext context = new NorthwindContext();

            var categories = (from x in context.Categories
                              select x).ToList();
            context.Dispose();
            return Ok(categories);
        }
    }
}