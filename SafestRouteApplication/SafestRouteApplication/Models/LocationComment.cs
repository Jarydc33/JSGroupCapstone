using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SafestRouteApplication.Models
{
    public class LocationComment
    {
        [Key]
        public int id { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Comment { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser  ApplicationUser { get; set; }


    }
}