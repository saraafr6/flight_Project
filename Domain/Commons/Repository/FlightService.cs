using Domain.Commons.Contract;
using Fly.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Commons.Repository
{
    public class FlightService: IflightService
    {
        public async Task<List<Flight>> GetFlightsAsync()
        {
            return [];
        }
    }
}
