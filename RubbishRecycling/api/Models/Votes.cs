using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RubbishRecyclingAU.Models
{
    public class Votes
    {
        public bool UpOrDown { get; set; }

        public int ProductId { get; set; }

        public int UserId { get; set; }

        virtual public User User { get; set; }
        virtual public Product Product { get; set; }
    }
}
