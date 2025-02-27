using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop.Data.Context;
using Shop.Entities.Interfaces;
using Shop.Entities.Models;
using Shop.Entities.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace Shop.program.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        // Dependency Injection: Injecting IUnitOfWork and IWebHostEnvironment
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        // Action: Display a list of all products
        // Action: Display a list of all products
        public IActionResult Index()
        {
            return View();
        }

        // Action: Get products data for DataTables (AJAX)
        public IActionResult GetProducts()
        {
            var products = _unitOfWork.Products.GetAll(includeProperties: "Category")
                .Select(p => new
                {
                    id = p.Id,
                    name = p.Name,
                    description = p.Description,
                    price = p.Price,
                    category = p.Category?.Name,
                    image = p.Img
                }).ToList();

            return Json(new { data = products });
        }

        // Action: Display the form to create a new product (GET)
        [HttpGet]
        public IActionResult Create()
        {
            // Retrieve all categories and convert them to SelectListItem for the dropdown
            var categories = _unitOfWork.Categories.GetAll()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

            // Create a ProductViewModel to pass both the product and categories to the view
            var viewModel = new ProductViewModel
            {
                Product = new Product(),
                categorySelect = categories
            };

            // Return the view with the viewModel
            return View(viewModel);
        }

        // Action: Handle the form submission to create a new product (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductViewModel viewModel, IFormFile? file)
        {
            // Check if the model state is valid (e.g., required fields are filled)
            if (ModelState.IsValid)
            {
                // Handle file upload if a file is provided
                if (file != null && file.Length > 0)
                {
                    // Define the path to save the file
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productImagePath = Path.Combine(wwwRootPath, "images", "products");

                    // Create the directory if it doesn't exist
                    if (!Directory.Exists(productImagePath))
                    {
                        Directory.CreateDirectory(productImagePath);
                    }

                    // Save the file to the server
                    string fullPath = Path.Combine(productImagePath, fileName);
                    using (var fileStream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    // Save the image path in the database
                    viewModel.Product.Img = Path.Combine("images", "products", fileName);
                }

                // Add the product to the database
                _unitOfWork.Products.Add(viewModel.Product);
                _unitOfWork.Complet(); // Save changes to the database
                TempData["message"] = "Product created successfully!"; // Success message
                return RedirectToAction("Index"); // Redirect to the product list
            }

            // If model state is invalid, repopulate the categories dropdown
            viewModel.categorySelect = _unitOfWork.Categories.GetAll()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

            // Display an error message
            TempData["ErrorMessage"] = "Error occurred while creating the product.";
            return View(viewModel); // Return to the form with validation errors
        }

        // Action: Display the form to edit an existing product (GET)
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            // Check if the ID is valid
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // Retrieve the product from the database
            var product = _unitOfWork.Products.GetFirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            // Retrieve all categories for the dropdown
            var categories = _unitOfWork.Categories.GetAll()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

            // Create a ProductViewModel to pass the product and categories to the view
            var viewModel = new ProductViewModel
            {
                Product = product,
                categorySelect = categories
            };

            // Return the view with the viewModel
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductViewModel viewModel, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                // Handle file upload if a new file is provided
                if (file != null && file.Length > 0)
                {
                    // Define the path to save the new file
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productImagePath = Path.Combine(wwwRootPath, "images", "products");

                    // Create the directory if it doesn't exist
                    if (!Directory.Exists(productImagePath))
                    {
                        Directory.CreateDirectory(productImagePath);
                    }

                    // Save the new file to the server
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

                // Ensure CategoryId is updated properly
                viewModel.Product.CategoryId = viewModel.Product.CategoryId;

                // Update the product in the database
                _unitOfWork.Products.Update(viewModel.Product);
                _unitOfWork.Complet(); // Save changes to the database
                TempData["Update"] = "Product updated successfully!"; // Success message
                return RedirectToAction("Index"); // Redirect to the product list
            }

            // If model state is invalid, repopulate the categories dropdown
            viewModel.categorySelect = _unitOfWork.Categories.GetAll()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

            // Display an error message
            TempData["ErrorMessage"] = "Error occurred while updating the product.";
            return View(viewModel); // Return to the form with validation errors
        }

        // Action: Display the confirmation page to delete a product (GET)
        [HttpGet]
        [HttpGet]
        public IActionResult Delete(int? id)
        {
            // Check if the ID is valid
            if (id == null || id == 0)
            {
                return NotFound();
            }

            // Retrieve the product from the database
            var product = _unitOfWork.Products.GetFirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            // Return the view with the product
            return View(product);
        }

        // Action: Handle the confirmation to delete a product (POST)
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            // Retrieve the product from the database
            var product = _unitOfWork.Products.GetFirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found."; // Error message
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

                //to remove img from erver
                product.Img = null;
            }

            // Remove the product from the database
            _unitOfWork.Products.Remove(product);
            _unitOfWork.Complet(); // Save changes to the database
            TempData["Deleted"] = "Product and image deleted successfully!"; // Success message
            return RedirectToAction("Index"); // Redirect to the product list
        }
    }
}