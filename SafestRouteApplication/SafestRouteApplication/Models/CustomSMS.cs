using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SafestRouteApplication.Models
{
    public class CustomSMS
    {

        [Key]
        public int id { get; set; }
        public string CustomMessage { get; set; }

        [ForeignKey("Observer")]
        public int ObserverId { get; set; }
        public Observer Observer { get; set; }

    }
}