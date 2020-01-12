using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantMVC.Data;
using RestaurantMVC.Models.ViewModels;

namespace RestaurantMVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubcategoryController : Controller
    {
        private ApplicationDbContext _db;

        //[TempData]
        //public string StatusMessage { get; set; }

        public SubcategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var subcategories = await _db.Subcategories.Include(s=>s.Category).ToListAsync();
            return View(subcategories);
        }

        public async Task<IActionResult> Create()
        {
            SubcategoryAndCategoryViewModel model = new SubcategoryAndCategoryViewModel();
            model.CategoryList = await _db.Category.ToListAsync();
            model.Subcategory = new Models.Subcategory();
            model.SubcategoryList = await _db.Subcategories.OrderBy(p => p.Name).Select(p => p.Name).Distinct().ToListAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubcategoryAndCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // he recreates a new viewmodel
                // dont know why
                return View(model);
            }
            var category = _db.Category.Find(model.Subcategory.CategoryId);
            if (category == null)
            {
                return View(model);
            }
            var existingSubCategory = await _db.Subcategories.Include(s => s.Category)
                .Where(s => s.Name.Equals(model.Subcategory.Name) && s.Category.Id == model.Subcategory.CategoryId)
                .FirstOrDefaultAsync();
            if (existingSubCategory != null)
            {
                model.StatusMessage = "There is already subcategory with the same name";
                return View(model);
            }
            model.Subcategory.Category = category;
             _db.Subcategories.Add(model.Subcategory);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}