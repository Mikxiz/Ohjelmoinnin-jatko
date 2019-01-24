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
        // GET: Taulukko2
        public ActionResult Tunnit()
        {
            return View();
        }

        #region Tunnit
        // Tunnit region
        public JsonResult GetList()
        {
            OhjelmoinninjatkoEntities entities = new OhjelmoinninjatkoEntities();

            var model = (from c in entities.Tunnit
                         select new
                         {
                             TuntiID = c.TuntiID,
                             ProjektiID = c.ProjektiID,
                             HenkiloID = c.HenkiloID,
                             Pvm = c.Pvm,
                             ProjektiTunti = c.ProjektiTunti,
                             ProjektiTunnit = c.ProjektiTunnit,
                             SuunnitellutTunnit = c.SuunnitellutTunnit,
                             ToteutuneetTunnit = c.ToteutuneetTunnit
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
            var model = (from c in entities.Tunnit
                         where c.TuntiID == id
                         select new
                         {
                             TuntiID = c.TuntiID,
                             ProjektiID = c.ProjektiID,
                             HenkiloID = c.HenkiloID,
                             Pvm = c.Pvm,
                             ProjektiTunti = c.ProjektiTunti,
                             ProjektiTunnit = c.ProjektiTunnit,
                             SuunnitellutTunnit = c.SuunnitellutTunnit,
                             ToteutuneetTunnit = c.ToteutuneetTunnit
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
                // kyseessä on uuden asiakkaan lisääminen, kopioidaan kentät
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
                Tunnit dbItem = (from c in entities.Tunnit
                                    where c.TuntiID == id
                                    select c).FirstOrDefault();
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

            //etsitään id:n perusteella asiakasrivi kannasta
            bool OK = false;
            Tunnit dbItem = (from c in entities.Tunnit
                                where c.TuntiID == id
                                select c).FirstOrDefault();
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