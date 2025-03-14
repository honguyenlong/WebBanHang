using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebBanHang.Models;
using WebBanHang.Repositories;

namespace WebBanHang.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductRepository _productRepository; // Thêm ProductRepository

    public HomeController(ILogger<HomeController> logger, IProductRepository productRepository)
    {
        //_logger = logger;
        _productRepository = productRepository; // Inject repository
    }

    public IActionResult Index()
    {
        var products = _productRepository.GetAll(); // Lấy danh sách sản phẩm từ database

        if (products == null)
        {
            products = new List<Product>(); // Tránh lỗi null
        }

        return View(products); // Truyền danh sách sản phẩm xuống View
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
