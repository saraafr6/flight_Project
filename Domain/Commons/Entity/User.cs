using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fly.Model.Entity
{
    [Table("User", Schema = "dbo")]
    public class User : EntityBase
    {
        public User()
        {
        }

        public User(bool initialize) : base(initialize)
        {
        }

        [MaxLength(50)]
        public virtual string FirstName { get; set; }

        [MaxLength(100)]
        public virtual string LastName { get; set; }

        [Column(TypeName = "char(11)")]
        public virtual string MobileNumber { get; set; }

        [MaxLength(255)]
        public virtual string Password { get; set; }

        [Column(TypeName = "varchar(200)")]
        public virtual string Email { get; set; }

        public virtual DateTimeOffset? BirthDate { get; set; }

        public virtual bool? IsActive { get; set; }

        public virtual DateTimeOffset? RegisterDate { get; set; }

        public virtual DateTimeOffset? CreateDateTime { get; set; }

        public virtual List<UserAuthentication> UserAuthentication { get; set; }
        public virtual List<FlightBook> Bookings { get; set; }
    }
}