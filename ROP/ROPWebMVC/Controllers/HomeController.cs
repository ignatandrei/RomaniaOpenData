﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ROPInfrastructure;

namespace ROPWebMVC.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var dataSv = this.HttpContext.Application["dataSaved"] as RopDataSaved[];
            return View(dataSv);
        }

        public async Task<ActionResult> LoadData(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
            
            
            var dataSaved=new List<RopDataSaved>();

            using (var rep = new Repository<RopDataSaved>())
            {
                var q = rep.RetrieveData().ToArray();
                if (q.Length > 0) { 

                    dataSaved.AddRange(q);
                }
            }

            string typeName = id.Replace("_", ".");
            var existTypeName = (dataSaved.FirstOrDefault(it => it.Name == typeName) != null);
            if (!existTypeName)
            {

                var dataRetr = await instanceRavenStore.GetOrLoad(typeName);


                dataSaved.AddRange(
                    dataRetr.Select(ropDocument => new RopDataSaved()
                    {
                        ID = ropDocument.ID,
                        Name = typeName,
                        Document = ropDocument
                    }));


                using (var rep = new Repository<RopDataSaved>())
                {
                    var q = await rep.StoreDataAsNew(dataSaved.ToArray());
                }
                
                
            }
            this.HttpContext.Application["dataSaved"] = dataSaved.ToArray();
            return View();

        }

        public ActionResult ShowMap(string id)
        {
            var lst =this.HttpContext.Application["dataSaved"] as RopDataSaved[];
            foreach (var ropDataSaved in lst)
            {
                if (ropDataSaved.ID == id)
                {
                    return View(ropDataSaved);
                }

            }
            return Content("not found " + id);
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