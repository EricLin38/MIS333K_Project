using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Collections.Generic;
using Team1_FinalProject.DAL;

namespace Team1_FinalProject.Models
{
    public enum OrderHistory { Past, Cancelled, Future }
    public class Order
    {
		public const Decimal TAX_RATE = .0825m;

	    [Display(Name = "OrderID:")]
	    public Int32 OrderID {get; set;}

        [Display(Name = "Order Number:")]
        public Int32 OrderNumber { get; set; }

        [Display(Name = "Is this a gift order?")]	
	    public Boolean GiftOrder {get; set;}

        [Display(Name = "Enter the Gift Recipient's Email:")]
        public String GiftEmail { get; set; }
	
	    [Display(Name = "Popcorn Points Used:")]
	    public Boolean PopcornPointsUsed {get; set;}

		[Display(Name = "Date:")]
	    public DateTime Date { get; set; }

        [Display(Name = "Order History:")]
        public OrderHistory OrderHistory { get; set; }

        [Display(Name = "Order Subtotal:")]
        [DisplayFormat(DataFormatString = "{0:C}")]
		public Decimal OrderSubtotal
        {
            get 
            {
                decimal subtot = 0;
                foreach (Ticket ticket in Tickets)
                {
                    subtot += ticket.FixPrice;
                }
                return subtot;
            }
        }

		[Display(Name = "Tax (8.25%):")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal Tax
        {
            get { return OrderSubtotal * TAX_RATE; }
        }
        [Display(Name = "Order Total:")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal OrderTotal
        {
            get { return OrderSubtotal + Tax; }
        }

		public AppUser Customer { get; set; }

	    public List<Ticket> Tickets { get; set; }

        public Price Discount { get; set; }
        [Display(Name = "Order Total:")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal PostDiscount
        {
            get
            {
                decimal DiscountTotal = OrderTotal;

                if (Discount == null)
                {
                    return DiscountTotal;
                }

                decimal discount = Discount.PriceValue;
                List<Ticket> nonspecialtickets = Tickets.Where(t => t.Showing.SpecialEvent == false).ToList();

                if (nonspecialtickets.Count() >= 2)
                {
                        
                    DiscountTotal = Math.Round(((OrderSubtotal + (discount * 2)) * (1 + TAX_RATE)), 2);
                }

                else if (nonspecialtickets.Count() == 1)
                {
                        
                    DiscountTotal = Math.Round(((OrderSubtotal + (discount)) * (1 + TAX_RATE)), 2);
                }
  
                return DiscountTotal;
            }
        }

	    public Order()
        {
			if (Tickets == null)
			{
				Tickets = new List<Ticket>();
            }
        }

        
    }
}
