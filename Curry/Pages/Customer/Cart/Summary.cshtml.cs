using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Curry.DataAccess.Data.Repository.IRepository;
using Curry.Models;
using Curry.Models.ViewModels;
using Curry.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Stripe;

namespace Curry.Pages.Customer.Cart
{
    public class SummaryModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public SummaryModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [BindProperty]
        public OrderDetailsCartVM OrderDetailsCartVM { get; set; }
        public void OnGet()
        {
            OrderDetailsCartVM = new OrderDetailsCartVM()
            {
                OrderHeader = new Models.OrderHeader(),
                listCart = new List<ShoppingCart>()
            };
            OrderDetailsCartVM.OrderHeader.OrderTotal = 0;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                IEnumerable<ShoppingCart> cart = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == claim.Value);

                if (cart != null)
                {
                    OrderDetailsCartVM.listCart = cart.ToList();

                }
                foreach (var cartList in OrderDetailsCartVM.listCart)
                {
                    cartList.MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault
                        (m => m.Id == cartList.MenuItemId);
                    OrderDetailsCartVM.OrderHeader.OrderTotal +=
                        (cartList.MenuItem.Price * cartList.Count);
                }
                OrderDetailsCartVM.OrderHeader.OrderTotal +=
                    OrderDetailsCartVM.OrderHeader.OrderTotal * SD.SalesTaxPercent;
                ApplicationUser applicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(c => c.Id == claim.Value);
                OrderDetailsCartVM.OrderHeader.DeliveryName = applicationUser.FullName;
                OrderDetailsCartVM.OrderHeader.DeliveryTime = DateTime.Now;
                OrderDetailsCartVM.OrderHeader.PhoneNumber = applicationUser.PhoneNumber;

            }
        }
        public IActionResult OnPost(string stripeToken)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            OrderDetailsCartVM.listCart = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == claim.Value).ToList();

            OrderDetailsCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            OrderDetailsCartVM.OrderHeader.OrderDate = DateTime.Now;
            OrderDetailsCartVM.OrderHeader.UserId = claim.Value;
            OrderDetailsCartVM.OrderHeader.Status = SD.StatusSubmitted;
            OrderDetailsCartVM.OrderHeader.DeliveryTime = Convert.ToDateTime(OrderDetailsCartVM.OrderHeader.DeliveryDate.ToShortDateString() + " " + OrderDetailsCartVM.OrderHeader.DeliveryTime.ToShortTimeString());

            List<OrderDetails> orderDetailsList = new List<OrderDetails>();
            _unitOfWork.OrderHeader.Add(OrderDetailsCartVM.OrderHeader);
            _unitOfWork.Save();
            foreach (var item in OrderDetailsCartVM.listCart)
            {
                item.MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(m => m.Id == item.MenuItemId);
                OrderDetails orderDetails = new OrderDetails
                {
                    MenuItemId = item.MenuItemId,
                    OrderId = OrderDetailsCartVM.OrderHeader.Id,
                    Name = item.MenuItem.Name,
                    Price = item.MenuItem.Price,
                    Count = item.Count
                };
                OrderDetailsCartVM.OrderHeader.OrderTotal += (orderDetails.Count * orderDetails.Price) * (1 + SD.SalesTaxPercent);
                _unitOfWork.OrderDetails.Add(orderDetails);

            }
            OrderDetailsCartVM.OrderHeader.OrderTotal = Convert.ToDouble(String.Format("{0:.##}", OrderDetailsCartVM.OrderHeader.OrderTotal));
            _unitOfWork.ShoppingCart.RemoveRange(OrderDetailsCartVM.listCart);
            HttpContext.Session.SetInt32(SD.ShoppingCart, 0);
            //_unitOfWork.Save();

            if(stripeToken != null)
            {
                var options = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(OrderDetailsCartVM.OrderHeader.OrderTotal * 100 * SD.SalesTaxPercent),
                    Currency = "usd",
                    Description = "Order ID : " + OrderDetailsCartVM.OrderHeader.Id,
                    Source = stripeToken
                };
                var service = new ChargeService();
                Charge charge = service.Create(options);

                OrderDetailsCartVM.OrderHeader.TransactionId = charge.Id;
                if(charge.Status.ToLower() == "succeeded")
                {
                    OrderDetailsCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
                }
                else
                {
                    OrderDetailsCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
                }
            }
            else
            {
                OrderDetailsCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
            }
            _unitOfWork.Save();
            return RedirectToPage("/Customer/Cart/OrderConfirmation", new { id = OrderDetailsCartVM.OrderHeader.Id });
        }
    }
}