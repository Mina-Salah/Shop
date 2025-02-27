using Microsoft.AspNetCore.Mvc;
using Shop.Data.Context;
using Shop.Entities.Interfaces;
using Shop.Entities.Models;

namespace Shop.program.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork UnitOfWork)
        {
            _unitOfWork = UnitOfWork;
        }

        public IActionResult Index()
        {
            var Category = _unitOfWork.Categories.GetAll();
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
                // _context.Categories.Add(category);
                // _context.SaveChanges();
                _unitOfWork.Categories.Add(category);
                _unitOfWork.Complet();
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
            var category = _unitOfWork.Categories.GetFirstOrDefault(x => x.Id == id);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                // _context.Categories.Update(category);
                // _context.SaveChanges();
                _unitOfWork.Categories.Update(category);
                _unitOfWork.Complet();
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
            var category = _unitOfWork.Categories.GetFirstOrDefault(x => x.Id == id);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfairm(int? id)
        {
            var category = _unitOfWork.Categories.GetFirstOrDefault(x => x.Id == id);

            if (category == null)
            {
                TempData["ErrorMessage"] = "Category not found.";
                return NotFound();
            }

            // _context.Categories.Remove(category);
            // _context.SaveChanges();
            _unitOfWork.Categories.Remove(category);
            _unitOfWork.Complet();
            TempData["Deleted"] = "Category Deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}