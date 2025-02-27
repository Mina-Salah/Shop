using Microsoft.AspNetCore.Mvc;
using shop.Data.Implementation;
using Shop.Entities.Interfaces;
using Shop.Entities.ViewModel;

namespace Shop.program.Areas.Customer.Controllers
{
        [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var GetProduct = _unitOfWork.Products.GetAll();
            return View(GetProduct);
        }

        public IActionResult Details(int id) 
        {
            ShopCardViewModel viewModel = new ShopCardViewModel()
            {
                product = _unitOfWork.Products.GetFirstOrDefault(X=>X.Id==id,includeProperties:"Category"),
                count = 1
            };
            return View(viewModel);
        }


    }
}
