using WebBanHang.Models;

namespace WebBanHang.Repositories
{
    public class MockProductRepository : IProductRepository
    {
        private readonly List<Product> _products;
        public MockProductRepository()
        {
            // Tạo một số dữ liệu mẫu
            _products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop Asus VivoBook", ImageUrl= "/images/hinh1.jpg", Price = 21000, Description = "A high-end laptop"},
                new Product { Id = 1, Name = "Laptop Msi", ImageUrl= "/images/hinh2.jpg", Price = 34400, Description = "A high-end laptop"},
                new Product { Id = 1, Name = "Laptop Asus Tuf", ImageUrl= "/images/hinh3.jpg", Price = 32210, Description = "A high-end laptop"},
                new Product { Id = 1, Name = "Laptop Lenovo", ImageUrl= "/images/hinh4.jpg", Price = 28000, Description = "A high-end laptop"},
                new Product { Id = 1, Name = "MacBook", ImageUrl= "/images/hinh6.jpg", Price = 47000, Description = "A high-end laptop"},
                new Product { Id = 1, Name = "Laptop Acer Nitro", ImageUrl= "/images/hinh7.jpg", Price = 20300, Description = "A high-end laptop"},
                // Thêm các sản phẩm khác
            };
        }
        public IEnumerable<Product> GetAll()
        {
            return _products;
        }
        public Product GetById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }
        public void Add(Product product)
        {
            product.Id = _products.Max(p => p.Id) + 1;
            _products.Add(product);
        }

        public void Update(Product product)
        {
            var index = _products.FindIndex(p => p.Id == product.Id);
            if (index != -1)
            {
                _products[index] = product;
            }
        }
        public void Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
        }
    }
}

