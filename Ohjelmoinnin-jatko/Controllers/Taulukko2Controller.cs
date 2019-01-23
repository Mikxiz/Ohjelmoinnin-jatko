using Newtonsoft.Json;
using Ohjelmoinnin_jatko.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ohjelmoinnin_jatko.Controllers
{
    public class Taulukko2Controller : Controller
    {
        // GET: Taulukko2
        public ActionResult Projektit()
        {
            return View();
        }

        #region Projektit
        // Projektit
        public JsonResult GetList()
        {
            OhjelmoinninjatkoEntities entities = new OhjelmoinninjatkoEntities();

            var model = (from c in entities.Projektit
                         select new
                         {
                             ProjektiID = c.ProjektiID,
                             ProjektiNimi = c.ProjektiNimi,
                             Esimies = c.Esimies,
                             Avattu = c.Avattu,
                             Suljettu = c.Suljettu,
                             Status = c.Status
                         }).ToList();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            Response.Expires = -1;
            Response.CacheControl = "no-cache";

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSingleProjekti(int id)
        {
            OhjelmoinninjatkoEntities entities = new OhjelmoinninjatkoEntities();
            var model = (from c in entities.Projektit
                         where c.ProjektiID == id
                         select new
                         {
                             ProjektiID = c.ProjektiID,
                             ProjektiNimi = c.ProjektiNimi,
                             Esimies = c.Esimies,
                             Avattu = c.Avattu,
                             Suljettu = c.Suljettu,
                             Status = c.Status
                         }).FirstOrDefault();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(Projektit proj)
        {
            OhjelmoinninjatkoEntities entities = new OhjelmoinninjatkoEntities();
            int id = proj.ProjektiID;

            bool OK = false;

            // onko kyseessä muokkaus vai uuden lisääminen?
            if (id == 0)
            {
                // kyseessä on uuden asiakkaan lisääminen, kopioidaan kentät
                Projektit dbItem = new Projektit()
                {
                    ProjektiID = proj.ProjektiID,
                    ProjektiNimi = proj.ProjektiNimi,
                    Esimies = proj.Esimies,
                    Avattu = proj.Avattu,
                    Suljettu = proj.Suljettu,
                    Status = proj.Status
                };

                // tallennus tietokantaan
                entities.Projektit.Add(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            else
            {
                // muokkaus, haetaan id:n perusteella riviä tietokannasta
                Projektit dbItem = (from c in entities.Projektit
                                    where c.ProjektiID == id
                                    select c).FirstOrDefault();
                if (dbItem != null)
                {
                    dbItem.ProjektiNimi = proj.ProjektiNimi;
                    dbItem.Esimies = proj.Esimies;
                    dbItem.Avattu = proj.Avattu;
                    dbItem.Suljettu = proj.Suljettu;
                    dbItem.Status = proj.Status;

                    // tallennus tietokantaan
                    entities.SaveChanges();
                    OK = true;
                }
            }
            entities.Dispose();

            return Json(OK, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            OhjelmoinninjatkoEntities entities = new OhjelmoinninjatkoEntities();

            //etsitään id:n perusteella asiakasrivi kannasta
            bool OK = false;
            Projektit dbItem = (from c in entities.Projektit
                                where c.ProjektiID == id
                                select c).FirstOrDefault();
            if (dbItem != null)
            {
                // tietokannasta poisto
                entities.Projektit.Remove(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            entities.Dispose();

            return Json(OK, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}