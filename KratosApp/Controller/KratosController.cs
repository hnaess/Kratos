using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KratosApp.Models;
using System.Collections.Specialized;
using System.Web;

namespace KratosApp.Controller
{
    public class KratosController : ApiController
    {
        Kratos[] kratoses = new Kratos[] 
        { 
            new Kratos { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 }, 
            new Kratos { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M }, 
            new Kratos { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M } 
        };

        public IEnumerable<Kratos> Get()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            var system = nvc["System"];
            // BL comes here
            return kratoses;
        }

        //public IEnumerable<Kratos> GetAllProducts()
        //{
        //    return kratoses;
        //}

        public IHttpActionResult GetProduct(int id)
        {
            var Kratos = kratoses.FirstOrDefault((p) => p.Id == id);
            if (Kratos == null)
            {
                return NotFound();
            }
            return Ok(Kratos);
        }
    }
}
