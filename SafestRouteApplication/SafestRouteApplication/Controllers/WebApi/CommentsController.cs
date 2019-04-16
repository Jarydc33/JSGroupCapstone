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
        public CommentsController()
        {
            db = new ApplicationDbContext();
        }

        // GET api/comments
        public IEnumerable<LocationComment> GetComments()
        {
            return db.LocationComments.ToList();
        }
    }
}