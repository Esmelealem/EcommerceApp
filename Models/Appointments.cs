using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceProduct.Models
{
    public class Appointments
    {
        public int Id { get; set; }
        public DateTime ApointmentDate { get; set; }
        [NotMapped]
        public DateTime ApointmentTime { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string  CustomerEmail { get; set; }
        public bool isConfirmed { get; set; }



    }
}
