using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public class ProductDeletedEvent
    {
        public int ProductId { get; set; }
        public DateTime OccurredOn { get; set; } = DateTime.UtcNow;
    }
}
