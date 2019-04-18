using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SafestRouteApplication.Models
{
    public class ObserverSaveRouteModel
    {
        public SavedRoute route { get; set; }
        public NavigationViewModel nav { get; set; }
        [Display(Name = "Observee")]
        public string observeeId { get; set; }
        public List<SelectListItem> observees { get; set; }
    }
}