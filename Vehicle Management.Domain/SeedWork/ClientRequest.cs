using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vehicle_Management.Domain.SeedWork
{
    public class ClientRequest
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public DateTime Time { get; set; }
    }
}
