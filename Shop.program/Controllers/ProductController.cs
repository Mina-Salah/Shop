using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop.Data.Context;
using Shop.Entities.Interfaces;
using Shop.Entities.Models;
using Shop.Entities.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Program.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }


        public IActionResult Index()
        {
            var products = _unitOfWork.Products.GetAll();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var categories = _unitOfWork.Categories.GetAll()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();
           
            var viewModel = new ProductViewModel
            {
                Product = new Product(),
                categorySelect = categories
            };
           
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductViewModel viewModel, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload
                if (file != null && file.Length > 0)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productImagePath = Path.Combine(wwwRootPath, "images", "products");

                    // Create directory if it doesn't exist
                    if (!Directory.Exists(productImagePath))
                    {
                        Directory.CreateDirectory(productImagePath);
                    }

                    // Save the file
                    string fullPath = Path.Combine(productImagePath, fileName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    // Save the image path in the database
                    viewModel.Product.Img = Path.Combine("images", "products", fileName);
                }

                _unitOfWork.Products.Add(viewModel.Product);
                _unitOfWork.Complet();
                TempData["message"] = "Product created successfully!";
                return RedirectToAction("Index");
            }

            // Repopulate categories if validation fails
            viewModel.categorySelect = _unitOfWork.Categories.GetAll()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

            TempData["ErrorMessage"] = "Error occurred while creating the product.";
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var product = _unitOfWork.Products.GetFirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var categories = _unitOfWork.Categories.GetAll()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

            var viewModel = new ProductViewModel
            {
                Product = product,
                categorySelect = categories
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductViewModel viewModel, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload if a new file is provided
                if (file != null && file.Length > 0)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productImagePath = Path.Combine(wwwRootPath, "images", "products");

                    // Create directory if it doesn't exist
                    if (!Directory.Exists(productImagePath))
                    {
                        Directory.CreateDirectory(productImagePath);
                    }

                    // Save the file
                    string fullPath = Path.Combine(productImagePath, fileName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    // Delete the old image if it exists
                    if (!string.IsNullOrEmpty(viewModel.Product.Img))
                    {
                        string oldImagePath = Path.Combine(wwwRootPath, viewModel.Product.Img.TrimStart('\\', '/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Save the new image path in the database
                    viewModel.Product.Img = Path.Combine("images", "products", fileName);
                }

                _unitOfWork.Products.Update(viewModel.Product);
                _unitOfWork.Complet();
                TempData["Update"] = "Product updated successfully!";
                return RedirectToAction("Index");
            }

            // Repopulate categories if validation fails
            viewModel.categorySelect = _unitOfWork.Categories.GetAll()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

            TempData["ErrorMessage"] = "Error occurred while updating the product.";
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var product = _unitOfWork.Products.GetFirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            var product = _unitOfWork.Products.GetFirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return NotFound();
            }

            // Remove the product image from the server if it exists
            if (!string.IsNullOrEmpty(product.Img))
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string imagePath = Path.Combine(wwwRootPath, product.Img.TrimStart('\\', '/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            // Remove the product from the database
            _unitOfWork.Products.Remove(product);
            _unitOfWork.Complet();
            TempData["Deleted"] = "Product and image deleted successfully!";
            return RedirectToAction("Index");
        }

    }
}