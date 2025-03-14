using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBanHang.Models;
using WebBanHang.Repositories;

namespace WebBanHang.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public ProductController(IProductRepository productRepository,ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            var categories = _categoryRepository.GetAllCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        
        public IActionResult Add(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.Add(product);
                return RedirectToAction("Index"); 
            }
            return View(product);
        }

        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            return View(products);
        }

        public IActionResult Display(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public IActionResult Update(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Update(Product product)
        {
            if (ModelState.IsValid)
            {
                _productRepository.Update(product);
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public IActionResult DeleteConfirmed(int id)
        {
            _productRepository.Delete(id);
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public async Task<IActionResult> Add(Product product, IFormFile imageUrl, List<IFormFile> imageUrls)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (imageUrl != null)
        //        {
        //            // Lưu hình ảnh đại diện
        //            product.ImageUrl = await SaveImage(imageUrl);
        //        }
        //        if (imageUrls != null)
        //        {
        //            product.ImageUrls = new List<string>();
        //            foreach (var file in imageUrls)
        //            {
        //                // Lưu các hình ảnh khác
        //                product.ImageUrls.Add(await SaveImage(file));
        //            }
        //        }
        //        _productRepository.Add(product);
        //        return RedirectToAction("Index");
        //    }
        //    return View(product);
        //}

        //private async Task<string> SaveImage(IFormFile image)
        //{
        //    // Thay đổi đường dẫn theo cấu hình của bạn
        //    var savePath = Path.Combine("wwwroot/images", image.FileName);
        //    using (var fileStream = new FileStream(savePath, FileMode.Create))
        //    {
        //        await image.CopyToAsync(fileStream);
        //    }
        //    return "/images/" + image.FileName; // Trả về đường dẫn tương đối
        //}

        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile imageUrl, List<IFormFile> imageUrls)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (imageUrl != null)
                    {
                        product.ImageUrl = await SaveImage(imageUrl);
                    }
                    if (imageUrls != null)
                    {
                        product.ImageUrls = new List<string>();
                        foreach (var file in imageUrls)
                        {
                            product.ImageUrls.Add(await SaveImage(file));
                        }
                    }

                    _productRepository.Add(product);
                    return RedirectToAction("Index");
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi tải lên hình ảnh.");
                }
            }
            return View(product);
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            // Danh sách định dạng ảnh hợp lệ
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
            var fileExtension = Path.GetExtension(image.FileName).ToLower();

            // Kiểm tra định dạng tệp
            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new InvalidOperationException("File không hợp lệ. Vui lòng tải lên tệp ảnh (.jpg, .png, .gif, .bmp).");
            }

            // Kiểm tra kích thước tệp (giới hạn 5MB)
            long maxFileSize = 5 * 1024 * 1024; // 5MB
            if (image.Length > maxFileSize)
            {
                throw new InvalidOperationException("Tệp quá lớn. Kích thước tối đa là 5MB.");
            }

            // Lưu ảnh vào thư mục wwwroot/images
            var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", image.FileName);

            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }

            return "/images/" + image.FileName; // Trả về đường dẫn ảnh
        }

    }
}
