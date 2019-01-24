using Newtonsoft.Json;
using Ohjelmoinnin_jatko.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ohjelmoinnin_jatko.Controllers
{
    public class TaulukkoController : Controller
    {
        // GET: Taulukko
        public ActionResult Henkilöt()
        {
            return View();
        }

        #region Henkilöt
        // Henkilöt region
        public JsonResult GetList()
        {
            OhjelmoinninjatkoEntities entities = new OhjelmoinninjatkoEntities();

            var model = (from h in entities.Henkilot
                         select new
                         {
                             HenkiloID = h.HenkiloID,
                             Etunimi = h.Etunimi,
                             Sukunimi = h.Sukunimi,
                             Esimies = h.Esimies,
                             Osoite = h.Osoite,
                             Postinumero = h.Postinumero
                         }).ToList();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            Response.Expires = -1;
            Response.CacheControl = "no-cache";

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSingleHenkilo(int id)
        {
            OhjelmoinninjatkoEntities entities = new OhjelmoinninjatkoEntities();
            var model = (from h in entities.Henkilot
                         where h.HenkiloID == id
                         select new
                         {
                             HenkiloID = h.HenkiloID,
                             Etunimi = h.Etunimi,
                             Sukunimi = h.Sukunimi,
                             Esimies = h.Esimies,
                             Osoite = h.Osoite,
                             Postinumero = h.Postinumero
                         }).FirstOrDefault();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(Henkilot henk)
        {
            OhjelmoinninjatkoEntities entities = new OhjelmoinninjatkoEntities();
            int id = henk.HenkiloID;

            bool OK = false;

            // onko kyseessä muokkaus vai uuden lisääminen?
            if (id == 0)
            {
                // kyseessä on uuden henkilön lisääminen, kopioidaan kentät
                Henkilot dbItem = new Henkilot()
                {
                    HenkiloID = henk.HenkiloID,
                    Etunimi = henk.Etunimi,
                    Sukunimi = henk.Sukunimi,
                    Esimies = henk.Esimies,
                    Osoite = henk.Osoite,
                    Postinumero = henk.Postinumero
                };

                // tallennus tietokantaan
                entities.Henkilot.Add(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            else
            {
                // muokkaus, haetaan id:n perusteella riviä tietokannasta
                Henkilot dbItem = (from h in entities.Henkilot
                                   where h.HenkiloID == id
                                   select h).FirstOrDefault();
                if (dbItem != null)
                {
                    dbItem.Etunimi = henk.Etunimi;
                    dbItem.Sukunimi = henk.Sukunimi;
                    dbItem.Esimies = henk.Esimies;
                    dbItem.Osoite = henk.Osoite;
                    dbItem.Postinumero = henk.Postinumero;

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

            //etsitään id:n perusteella henkilörivi kannasta
            bool OK = false;
            Henkilot dbItem = (from h in entities.Henkilot
                               where h.HenkiloID == id
                               select h).FirstOrDefault();
            if (dbItem != null)
            {
                // tietokannasta poisto
                entities.Henkilot.Remove(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            entities.Dispose();

            return Json(OK, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}