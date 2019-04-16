using SafestRouteApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SafestRouteApplication.Controllers.WebApi
{
    public class CommentsController : ApiController
    {

        private readonly ApplicationDbContext db;
        public EditedComments[] Property1 { get; set; }
        public CommentsController()
        {
            db = new ApplicationDbContext();
        }

        // GET api/comments
        public IEnumerable<EditedComments> GetComments()
        {
            var edited = db.LocationComments.ToList();
            List<EditedComments> changeComments = new List<EditedComments>();
            foreach (var thing in edited)
            {
                EditedComments temp = new EditedComments();
                temp.Latitude = thing.Latitude;
                temp.Longitude = thing.Longitude;
                temp.Comment = thing.Comment;
                changeComments.Add(temp);
            }
            return changeComments;
        }
    }

    public class EditedComments
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Comment { get; set; }
    }
}