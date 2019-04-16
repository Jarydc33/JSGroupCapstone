using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SafestRouteApplication.Models
{
    public class AvoidanceRouteViewModel
    {
        public int? id { get; set; }
        public float TopLeftLatitude { get; set; }
        public float TopLeftLongitude { get; set; }
        public float BottomRightLatitude { get; set; }
        public float BottomRightLongitude { get; set; }
        public string Reason { get; set; }
        public string ObserveeId { get; set; }
        public IEnumerable<SelectListItem> Observees { get; set; }
    }
}