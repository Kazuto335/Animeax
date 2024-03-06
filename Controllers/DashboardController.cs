using Animeax.Data;
using Animeax.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Animeax.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IMemoryCache _cache;

        public DashboardController(ApplicationDbContext db, IMemoryCache cache)
        {
            _db = db;
            _cache = cache;
        }

        public IActionResult Index(int page = 1, int pageSize = 100, string search = null)
        {
            IQueryable<AAnime> animeQuery = _db.AAnime;

            // Filtering by search term
            if (!string.IsNullOrEmpty(search))
            {
                animeQuery = animeQuery.Where(a => a.Name.Contains(search));
            }

            int totalCount = animeQuery.Count();

            // Paginate the results
            List<AAnime> animeList = animeQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.Search = search;

            return View(animeList);
        }


        public IActionResult AddAnime()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddAnime(AAnime obj)
        {
            if (ModelState.IsValid)
            {
                _db.AAnime.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if(id==null || id == 0)
            {
                return NotFound();
            }
            AAnime AnimeFromDb = _db.AAnime.Find(id);
            if (AnimeFromDb == null)
            {
                return NotFound();
            }

            return View(AnimeFromDb);
        }
        [HttpPost]
        public IActionResult Edit(AAnime obj)
        {
            if (ModelState.IsValid)
            {
                _db.AAnime.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            AAnime AnimeFromDb = _db.AAnime.Find(id);
            if (AnimeFromDb == null)
            {
                return NotFound();
            }

            return View(AnimeFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int id)
        {
            AAnime obj = _db.AAnime.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.AAnime.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index", "Dashboard");
        }

        // Other action methods

        public IActionResult AddDownlodLink(int id)
        {
            var obj = new AnimeDLink
            {
                AAnimeID = id
            };
            return View(obj);
        }


        [HttpPost]
        public IActionResult AddDownlodLink(AnimeDLink obj)
        {
            if (ModelState.IsValid)
            {
                _db.AnimeDLinks.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }

}
