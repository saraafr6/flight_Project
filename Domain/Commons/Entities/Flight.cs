using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Commons.Entities

{
    [Table("Flight", Schema = "dbo")]
    public class Flight : EntityBase
    {
        public Flight() { }
        public Flight(bool initialize) : base(initialize)
        {
            
        }

        [Required]
        public string FlightNumber 
        {
            get
            {
                return _flightNumber;
            }
        }

        [Required]
        public string AirlineName { get; set; }

        [Required]
        public string Origin { get; set; }

        [Required]
        public string Destination { get; set; }

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public int AvailableSeats { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public virtual List<FlightBook> Bookings { get; set; }


        private static long _counter = DateTime.UtcNow.Ticks % 10000;

        private string _flightNumber => GenerateFlightNumber();
        private static string GenerateFlightNumber()
        {
            string airlineCode = "W5";
            long uniqueNumber = Interlocked.Increment(ref _counter) % 10000; // conccurency
            int randomComponent = (int)(DateTime.UtcNow.Ticks % 90 + 10); 

            return $"{airlineCode}{uniqueNumber:D4}{randomComponent}";
        }


    }
}
