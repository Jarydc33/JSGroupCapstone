using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SafestRouteApplication.Models
{
    public class ObserverDetailsViewModel
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<PhoneNumber> Numbers { get; set; }


    }
}