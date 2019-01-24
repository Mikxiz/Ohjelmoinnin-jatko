using Newtonsoft.Json;
using Ohjelmoinnin_jatko.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ohjelmoinnin_jatko.Controllers
{
    public class Taulukko3Controller : Controller
    {
        // GET: Taulukko3
        public ActionResult Tunnit()
        {
            return View();
        }

        #region Tunnit
        // Tunnit region
        public JsonResult GetList()
        {
            OhjelmoinninjatkoEntities entities = new OhjelmoinninjatkoEntities();

            var model = (from t in entities.Tunnit
                         select new
                         {
                             TuntiID = t.TuntiID,
                             ProjektiID = t.ProjektiID,
                             HenkiloID = t.HenkiloID,
                             Pvm = t.Pvm,
                             ProjektiTunti = t.ProjektiTunti,
                             ProjektiTunnit = t.ProjektiTunnit,
                             SuunnitellutTunnit = t.SuunnitellutTunnit,
                             ToteutuneetTunnit = t.ToteutuneetTunnit
                         }).ToList();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            Response.Expires = -1;
            Response.CacheControl = "no-cache";

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSingleTunti(int id)
        {
            OhjelmoinninjatkoEntities entities = new OhjelmoinninjatkoEntities();
            var model = (from t in entities.Tunnit
                         where t.TuntiID == id
                         select new
                         {
                             TuntiID = t.TuntiID,
                             ProjektiID = t.ProjektiID,
                             HenkiloID = t.HenkiloID,
                             Pvm = t.Pvm,
                             ProjektiTunti = t.ProjektiTunti,
                             ProjektiTunnit = t.ProjektiTunnit,
                             SuunnitellutTunnit = t.SuunnitellutTunnit,
                             ToteutuneetTunnit = t.ToteutuneetTunnit
                         }).FirstOrDefault();

            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(Tunnit tunn)
        {
            OhjelmoinninjatkoEntities entities = new OhjelmoinninjatkoEntities();
            int id = tunn.TuntiID;

            bool OK = false;

            // onko kyseessä muokkaus vai uuden lisääminen?
            if (id == 0)
            {
                // kyseessä on uusien tuntien lisääminen, kopioidaan kentät
                Tunnit dbItem = new Tunnit()
                {
                    TuntiID = tunn.TuntiID,
                    ProjektiID = tunn.ProjektiID,
                    HenkiloID = tunn.HenkiloID,
                    Pvm = tunn.Pvm,
                    ProjektiTunti = tunn.ProjektiTunti,
                    ProjektiTunnit = tunn.ProjektiTunnit,
                    SuunnitellutTunnit = tunn.SuunnitellutTunnit,
                    ToteutuneetTunnit = tunn.ToteutuneetTunnit
                };

                // tallennus tietokantaan
                entities.Tunnit.Add(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            else
            {
                // muokkaus, haetaan id:n perusteella riviä tietokannasta
                Tunnit dbItem = (from t in entities.Tunnit
                                    where t.TuntiID == id
                                    select t).FirstOrDefault();
                if (dbItem != null)
                {
                    dbItem.TuntiID = tunn.TuntiID;
                    dbItem.ProjektiID = tunn.ProjektiID;
                    dbItem.HenkiloID = tunn.HenkiloID;
                    dbItem.Pvm = tunn.Pvm;
                    dbItem.ProjektiTunti = tunn.ProjektiTunti;
                    dbItem.ProjektiTunnit = tunn.ProjektiTunnit;
                    dbItem.SuunnitellutTunnit = tunn.SuunnitellutTunnit;
                    dbItem.ToteutuneetTunnit = tunn.ToteutuneetTunnit;

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

            //etsitään id:n perusteella tuntirivi kannasta
            bool OK = false;
            Tunnit dbItem = (from t in entities.Tunnit
                                where t.TuntiID == id
                                select t).FirstOrDefault();
            if (dbItem != null)
            {
                // tietokannasta poisto
                entities.Tunnit.Remove(dbItem);
                entities.SaveChanges();
                OK = true;
            }
            entities.Dispose();

            return Json(OK, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}