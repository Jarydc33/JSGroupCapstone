using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SafestRouteApplication.Models
{
    public class NavigationViewModel
    {
        public bool newRoute { get; set; }
        public List<string> routes { get; set; }
        public string selectedRoute { get; set; }
        [Display(Name = "Starting Address")]
        public string StartAddress { get; set; }
        [Display(Name = "Navigate To")]
        public string EndAddress { get; set; }
        public bool routeSelected { get; set; }

    }
}