using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCartSW.Models;
using ShoppingCartSW.Models.Data;
using System.Security.Claims;

namespace ShoppingCartSW.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCartDBContext _context;

        public ShoppingCartController(ShoppingCartDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var value = HttpContext.User.FindFirstValue("ID") ?? "-1";

            int.TryParse(value, out int id);

            if (id <= 0)
            {
                return Unauthorized();
            }

            var shoppingCart = _context.ShoppingCarts.Where(c => c.AppUserId == id && c.FinalisedDate == null)
                                                     .Include(c => c.CartItem)
                                                     .ThenInclude(ci => ci.product)
                                                     .FirstOrDefault();


            return PartialView("_ShoppingCartPartial", shoppingCart);
        }

        public IActionResult AddToCart(int ItemId)
        {
            var value = HttpContext.User.FindFirstValue("ID") ?? "-1";

            int.TryParse(value, out int id);

            if (id <= 0)
            {
                return Unauthorized();
            }
            // Active current cart 
            var cart = _context.ShoppingCarts.Where(c => c.AppUserId == id && c.FinalisedDate == null)
                                             .Include(c => c.CartItem)
                                             .FirstOrDefault();


            var cartItem = new ShoppingOrderLine
            {
                ProductId = ItemId,
                Quantity = 1
            };
            // No exsiting cart 
            if (cart == null)
            {
                // Create a new shopping cart object
                cart = new ShoppingCart
                {
                    AppUserId = id,
                    CartItem = new List<ShoppingOrderLine> { cartItem }
                };
                // Add new shopping item into the cart
                _context.ShoppingCarts.Add(cart);
            }
            else
            {
                var item = cart.CartItem.Where(ci => ci.ProductId == ItemId).FirstOrDefault();
                // Is the item already in the cart?
                if (item != null)
                {
                    // Increase a quality by 1
                    item.Quantity++;
                    // Pass the updated info
                    _context.ShoppingOrderLines.Attach(item);
                    // Only update the quantity of the item details
                    _context.Entry(item).Property(x => x.Quantity).IsModified = true;
                }
                else
                {
                    // If the item is not in the cart, add the item in the cart
                    cartItem.ShoppingCartId = cart.Id;
                    _context.ShoppingOrderLines.Add(cartItem);
                }

            }
            // Save to the database
            _context.SaveChanges();
            return Ok();
        }


        [HttpPut]
        public ActionResult UpdateQuantity([FromBody] ShoppingOrderLine item)
        {
            //Pass the new object details to Entity Framework
            _context.ShoppingOrderLines.Attach(item);
            //Set the quantity field in the object as the old field to be changed
            _context.Entry(item).Property(x => x.Quantity).IsModified = true;
            //Save the changes in the database
            _context.SaveChanges();
            //Send a response back to the caller.
            return Ok();


        }

        [HttpDelete]
        public ActionResult RemoveFromCart(int Id)
        {
            //Find the item with the provided ID and remove it
            _context.Remove(_context.ShoppingOrderLines.Single(ci => ci.Id == Id));
            //Save the changes in the database
            _context.SaveChanges();
            //Send a response back to the caller.
            return Ok();

        }

        public ActionResult CancelCart(int id)
        {
            //Remove all the items found by the subquery that have the carts id on them
            _context.ShoppingOrderLines.RemoveRange(_context.ShoppingOrderLines
                                      .Where(ci => ci.ShoppingCartId == id).ToList());
            //Save the changes in the database
            _context.SaveChanges();
            //Send a response back to the caller.
            return Ok();
        }

        public ActionResult FinaliseCart(int id)
        {
            //Find the cart that matches the provided cart ID
            var cart = _context.ShoppingCarts.Where(c => c.Id == id).FirstOrDefault();
            //Return an error if no cart was found
            if (cart == null)
            {
                return NotFound();
            }
            //Update the cart with the final date and total
            cart.Total = CalculateCartTotal(id);
            cart.FinalisedDate = DateTime.Now;

            _context.Update(cart);
            //Save the changes in the database
            _context.SaveChanges();

            return Ok();
        }

        public double CalculateCartTotal(int id)
        {
            //Find all the cart items that are int the cart with the provided cart id
            var cartItems = _context.ShoppingOrderLines.Where(ci => ci.ShoppingCartId == id)
                                                      .Include(ci => ci.product)
                                                      .ToList();
            //Create a available to hold our total and start it as zero.
            double total = 0.0;
            //Cycle through each item and add the line price to the total
            foreach (var item in cartItems)
            {
                total += item.Quantity * item.product.UnitPrice;
            }

            return total;
        }
    }
}

   
