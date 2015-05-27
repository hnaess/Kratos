using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using KratosApp.Extensions;
using KratosApp.Models;
using System.Collections.Specialized;
using System.Web;
using System.Diagnostics;

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
            List<Kratos> kratosList = new List<Kratos>();

            var kratosFirst = GetFromQueryString();
            kratosFirst.Calc(Kratos.CalcMethod.Power);

            // Default
            AddKratos(kratosList, kratosFirst);
            
            // Riders weight
            BasedOnRiderWeight(kratosList, kratosFirst, 5);

            return kratosList;
        }

        private static Kratos BasedOnRiderWeight(List<Kratos> kratosList, Kratos kratos, int vRidersWeight)
        {
            var kratos1 = kratos.ShallowCopy();
            kratos1.fM += vRidersWeight;
            kratos1.Calc();
            AddKratos(kratosList, kratos1, String.Format("Rider weight {0}", vRidersWeight));

            var kratos2 = kratos.ShallowCopy();
            kratos2.fM -= vRidersWeight;
            kratos2.Calc();
            AddKratos(kratosList, kratos2, String.Format("Rider weight -{0}", vRidersWeight));

            return kratos;
        }

        private static void AddKratos(List<Kratos> kratosList, Kratos kratos, string text)
        {
            kratos.Ext = text;
            AddKratos(kratosList, kratos);
        }

        private static void AddKratos(List<Kratos> kratosList, Kratos kratos)
        {
            kratosList.Add(kratos);
            Debug.WriteLine("Speed " + kratos.SpeedOutput);
        }

        private static Kratos GetFromQueryString()
        {
            var kratos = new Kratos()
            {
                cad = 90,
                bike = "racetops",
                fstg = 7,
                fP = 300,
                fV = 20, //9.3,
                fW = 10,

                fh = 172,
                fM = 71.3,
                fmr = 9.5,
                fT = 20,
                fHn = 350,
                Ext = "",
            };
            return kratos;
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
