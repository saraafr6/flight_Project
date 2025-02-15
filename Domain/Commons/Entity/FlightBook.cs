using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly.Model.Entity
{

    [Table("FlightBook", Schema = "dbo")]
    public class FlightBook : EntityBase
    {
        public FlightBook()
        {
        }

        public FlightBook(bool initialize) : base(initialize)
        {
        }

        [Required]
        public virtual int FlightId { get; set; }

        [Required]
        public virtual int UserId { get; set; }

        [Required]
        public virtual DateTimeOffset BookingDate { get; set; }

        [Required]
        public virtual BookingStatus Status { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public virtual decimal TotalPrice { get; set; }

        public virtual bool? IsActive { get; set; }
        public virtual DateTimeOffset? CreateDateTime { get; set; }

        
        public virtual Flight Flight { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<BookingSeat> BookedSeats { get; set; }
    }
}