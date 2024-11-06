using AutoMapper;
using Ecommerse_shoes_backend.Data.Dto;
using Ecommerse_shoes_backend.Data.Models;
using Ecommerse_shoes_backend.Dbcontext;
using Ecommerse_shoes_backend.Migrations;
using Microsoft.EntityFrameworkCore;

namespace Ecommerse_shoes_backend.Services.Productservice
{
    public class Productservice : IProductservice
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        public Productservice(ApplicationContext context, IMapper mapper,IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            try
            {
                var products = await _context.Products.Include(p => p.Category).ToListAsync();
                var product = products.Select(p =>
                new ProductDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    Stock = p.Stock,
                    ImageUrl = $"{_configuration["HostUrl:images"]}/Products/{p.ImageUrl}",
                    Price = p.Price,
                    Category_Name = p.Category.Category_Name,
                });

                return product.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProductDto> GetProductsById(int id)
        {
            try
            {
                var product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
                if (product == null)
                {
                    return null;
                }
                var pr = _mapper.Map<ProductDto>(product);
                pr.Category_Name = product.Category?.Category_Name;
                return pr;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

public async Task<IEnumerable<ProductDto>> GetProductsByCategory(string name)
{
    try
    {
        var products = await _context.Products.Include(p => p.Category).Where(p => p.Category.Category_Name == name).ToListAsync();

        if (!products.Any())
        {
            return Enumerable.Empty<ProductDto>();
        }

        var productDtos = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Title = p.Title,
            Description = p.Description,
            Stock = p.Stock,
            ImageUrl = $"{_configuration["HostUrl:images"]}/Products/{p.ImageUrl}",
            Price = p.Price,
            Category_Name = p.Category.Category_Name
        });

        return productDtos.ToList();
    }
    catch (Exception ex)
    {
        throw new Exception("An error occurred while retrieving products by category.", ex);
    }
}


        public async Task<string> AddProduct(ProductAddDto addDto, IFormFile image)
        {
            try
            {
                var ex = await _context.Categories.FindAsync(addDto.CategoryId);
                if (ex == null)
                {
                    return ("Category Id is not valid");
                }
                var product = _mapper.Map<Products>(addDto);
                if(image != null && image.Length>0)
                {
                    var filename=Guid.NewGuid().ToString()+Path.GetExtension(image.FileName);
                    var filepath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Products", filename);
                    using (var stream = new FileStream(filepath,FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    product.ImageUrl=filename;
                }
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return ("Product added successfully");
            }
            catch (Exception ex)
            {
                return (ex.Message);
            }
        }

        public async Task<string> DeleteProduct(int id)
        {
            try
            {
                var ex = await _context.Products.FindAsync(id);
                if (ex != null)
                {
                    _context.Products.Remove(ex);
                    await _context.SaveChangesAsync();
                    return ("Product deleted successfully");
                }
                return ("Product not found");

            }
            catch (Exception ex)
            {
                return (ex.Message);
            }


        }


        
        public async Task<ProductAddDto> UpadateProduct(int id, ProductAddDto addDto, IFormFile image)
        {
            try
            {
                var exist = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
                if (exist == null)
                {
                    return null; 
                }
                string productimg=null;
                if (image != null && image.Length > 0)
                {
                    var filename=Guid.NewGuid().ToString()+Path.GetExtension(image.FileName);
                    var directrypath=Path.Combine(_webHostEnvironment.WebRootPath,"Images","Products",filename);
                    if (!Directory.Exists(directrypath))
                    {
                        Directory.CreateDirectory(directrypath);
                    }
                    var filepath=Path.Combine(directrypath,filename);
                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                        productimg=filename;
                    }

                }
                exist.Title =addDto.Title;
                exist.Description =addDto.Description;
                exist.Price =addDto.Price;
                exist.CategoryId =addDto.CategoryId;
                exist.Stock=addDto.Stock;
                exist.ImageUrl = productimg;

                _context.Products.Update(exist); 

                await _context.SaveChangesAsync();

                return addDto; 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<IEnumerable<ProductDto>> SearchProduct(string name)
        {
            try
            {
               
                var products = _context.Products.Include(p => p.Category).Where(pr => pr.Title.ToLower().Contains(name.ToLower()));

                if (!products.Any())
                {
                    return Enumerable.Empty<ProductDto>();
                }
                var pr = products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    ImageUrl = $"{_configuration["HostUrl:images"]}/Products/{p.ImageUrl}",
                    Description = p.Description,
                    Stock = p.Stock,
                    Category_Name = p.Category.Category_Name,
                    Price = p.Price,
                });

                return pr.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
