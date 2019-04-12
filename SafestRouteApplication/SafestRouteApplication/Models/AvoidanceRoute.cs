using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SafestRouteApplication.Models
{
    public class AvoidanceRoute
    {

        [Key]
        public int id { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Reason { get; set; }

        [ForeignKey("Observee")]
        public int ObserveeId { get; set; }
        public Observee Observee { get; set; }



    }
}