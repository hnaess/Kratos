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
            //new Kratos { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 }, 
            //new Kratos { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M }, 
            //new Kratos { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M } 
        };

        public IEnumerable<Kratos> Get()
        {
            var kratos = new Kratos()
            {
                cad = 90,
                bike = "racetops",
                fstg = 7,
                fCadc = 90,
                fP = 160,
                fV = 20, //9.3,
                fW = 10,

                fh = 172,
                fM = 71.3,
                fT = 20,
                fHn = 350,
                
            };
            kratos.Calc(Kratos.CalcMethod.Power);
            kratos.Calc(Kratos.CalcMethod.Speed);
            return new List<Kratos>() { kratos };
        }

        //public IEnumerable<Kratos> GetAllProducts()
        //{
        //    return kratoses;
        //}

        //public IHttpActionResult GetProduct(int id)
        //{
        //    var nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);

        //    }
            

        //    //var Kratos = kratoses.FirstOrDefault((p) => p.Id == id);
        //    //if (Kratos == null)
        //    //{
        //    //    return NotFound();
        //    //}
        //    //return Ok(Kratos);

        //    var Kratos = kratoses.First();
        //    return Ok(Kratos);
        //}
    }
}
