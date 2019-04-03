using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hola.Models;
using System.Diagnostics;

namespace Hola.Controllers
{
    public class HomeController : Controller
    {
        static List<double> lista;
        public string api = "http://127.0.0.1:5000/ml";
        public ActionResult Index()
        {


            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(this.api);
            //string listaTxt = "";

            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            using (System.IO.Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                Console.WriteLine(json + "hola");
                lista = JsonConvert.DeserializeObject<List<double>>(json);
                Console.WriteLine(lista);
            }
            ViewData["Message"] = "Hola la lista de los datos es la siguiente";
            ViewBag.Message = lista;
            ViewBag.numTimes = lista.Count();
            Console.WriteLine(lista);




            string[] eje = new string[30];
            for (int i = 1; i <= 30; i++)
            {
                eje[i - 1] = "" + i;
            }

            DotNet.Highcharts.Highcharts chart = new DotNet.Highcharts.Highcharts("chart")
                  .SetXAxis(new DotNet.Highcharts.Options.XAxis
                  {

                      Categories = eje
                  })
                  .SetSeries(new DotNet.Highcharts.Options.Series
                  {

                  });
            return View(chart);


        }

        public ContentResult JSON()
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            for (int i = 0; i < 30; i++) { 

                double v = lista[i];

                dataPoints.Add(new DataPoint(i, v));
                Debug.WriteLine(dataPoints.Count);
            }



            JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
            return Content(JsonConvert.SerializeObject(dataPoints, _jsonSetting), "application/json");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}