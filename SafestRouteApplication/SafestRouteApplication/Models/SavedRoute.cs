using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SafestRouteApplication.Models
{
    public class SavedRoute
    {

        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string start_location { set; get; }
        public string start_latitude { set; get; }
        public string start_longitude { set; get; }
        public string waypoint1 { set; get; }
        public string waypoint2 { set; get; }
        public string avoidstring { set; get; }
        public string end_location { set; get; }
        public string end_latitude { set; get; }
        public string end_logitude { set; get; }
        public string routeRequest { set; get; }
        [ForeignKey("Observee")]
        public int? ObserveeId { get; set; }
        public Observee Observee { get; set; }
    }
}