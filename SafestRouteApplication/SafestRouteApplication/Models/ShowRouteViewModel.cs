using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SafestRouteApplication.Models
{
    public class ShowRouteViewModel
    {
        public Observee observee { get; set; }
        public Route route { get; set; }
        public string keycode { get { return Keys.HEREAppCode; } }
        public string keyid { get { return Keys.HEREAppID; } }
    }
}