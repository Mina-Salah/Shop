using Microsoft.AspNetCore.Mvc;
using Shop.Data.Context;
using Shop.Entities.Models;

namespace Shop.Program.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var Category = _context.Categories;
            return View(Category);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();

                TempData["message"] = "Category created successfully!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error occurred while creating the category.";
            return View(category);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null | id == 0) 
            { 
                return NotFound();
            }
            var category = _context.Categories.Find(id);
            return View(category);  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();

                TempData["Update"] = "Category Update successfully!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error occurred while updating the category.";
            return View(category);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null | id == 0)
            {
                return NotFound();
            }
            var category = _context.Categories.Find(id);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfairm(int? id)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
            {
                TempData["ErrorMessage"] = "Category not found.";
                return NotFound();
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();

            TempData["Deleted"] = "Category Deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}