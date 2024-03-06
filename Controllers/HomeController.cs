using Animeax.Data;
using Animeax.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Animeax.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IMemoryCache _cache; // Inject IMemoryCache

        public HomeController(ApplicationDbContext db, IMemoryCache cache)
        {
            _db = db;
            _cache = cache;
        }

        public IActionResult Index()
        {
            // Check if the data is cached
            if (!_cache.TryGetValue("AllAnime", out List<AAnime> allAnime))
            {
                // If not cached, fetch from the database
                allAnime = _db.AAnime
                    .OrderByDescending(anime => anime.Score)
                    .Take(16)
                    .ToList();

                // Cache the result for 10 minutes
                _cache.Set("AllAnime", allAnime, TimeSpan.FromMinutes(10));
            }

            return View(allAnime);
        }
        public IActionResult All(int page = 1, int pageSize = 20, string genre = null)
        {
            // Create a cache key based on genre and pagination parameters
            string cacheKey = $"Anime_{genre}_{page}_{pageSize}";

            // Check if the data is cached
            if (!_cache.TryGetValue(cacheKey, out List<AAnime> cachedAnime))
            {
                // If not cached, fetch from the database
                IQueryable<AAnime> animeQuery = _db.AAnime;

                // Filter by genre if specified
                if (!string.IsNullOrEmpty(genre))
                {
                    animeQuery = animeQuery.Where(anime => anime.Genres.Contains(genre));
                }

                int totalCount = animeQuery.Count();

                // Paginate the results
                cachedAnime = animeQuery
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Cache the result for 10 minutes
                _cache.Set(cacheKey, cachedAnime, TimeSpan.FromMinutes(10));
            }

            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = _db.AAnime.Count();
            ViewBag.Genre = genre;

            return View(cachedAnime);
        }
        public ActionResult Download(int id)
        {
            // Fetch data of the specific item using the ID
            var item = _db.AAnime.FirstOrDefault(a => a.ID == id);

            // Check if the item exists
            if (item == null)
            {
                // Handle the case where the item with the specified ID was not found
                return NotFound(); // Or return a view indicating that the item was not found
            }

            // Pass the item to the view
            return View(item);
        }

       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
