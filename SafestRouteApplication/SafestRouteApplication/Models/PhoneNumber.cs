using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SafestRouteApplication.Models
{
    public class PhoneNumber
    {

        [Key]
        public int id { get; set; }
        public string Number { get; set; }

        [ForeignKey("Observer")]
        public int ObserverId { get; set; }
        public Observer Observer { get; set; }
    }
}