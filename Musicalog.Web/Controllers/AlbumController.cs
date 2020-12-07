using Musicalog.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Musicalog.Web.Controllers
{
    public class AlbumController : Controller
    {
        public ActionResult List()
        {
            var albumListModel = new AlbumListModel
            {
                PageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pagesize"])
            };

            return View("AlbumList", albumListModel);
        }

        public ActionResult Detail(int id)
        {
            return View("AlbumDetail", id);
        }

        public ActionResult Create()
        {
            return View("AlbumDetail", 0);
        }
    }
}