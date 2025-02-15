using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly.Model.Entity
{
    [Table("Payment", Schema = "dbo")]
    public class Payment : EntityBase
    {
        public Payment()
        {
            
        }

        public Payment(bool initialize) : base(initialize)
        {
           
        }
        public Guid BookingId { get; set; }

        public FlightBook Booking { get; set; }  

        public decimal Amount { get; set; } 

        public DateTime PaymentDate { get; set; }

        public bool IsSuccessful { get; set; }
    }
}
