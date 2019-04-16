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
        public float TopLeftLatitude { get; set; }
        public float TopLeftLongitude { get; set; }
        public float BottomRightLatitude { get; set; }
        public float BottomRightLongitude { get; set; }
        public string Reason { get; set; }

        [ForeignKey("Observee")]
        public int? ObserveeId { get; set; }
        public Observee Observee { get; set; }
    }
}