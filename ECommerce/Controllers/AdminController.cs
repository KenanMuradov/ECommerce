using AutoMapper;
using ECommerce.Data;
using ECommerce.Helpers;
using ECommerce.Models;
using ECommerce.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext context;
        private static IMapper _mapper;

        public AdminController(AppDbContext context, IMapper mapper)
        {
            this.context = context;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View("Products",context.Products.ToList());
        }

        public IActionResult Categories()
        {
            return View(context.Categories.ToList());
        }

        public IActionResult Tags()
        {
            return View(context.Tags.ToList());
        }

        public IActionResult AddProduct()
        {
            ViewBag.Categories = new SelectList(context.Categories, "Id", "Name");
            ViewBag.Tags = new MultiSelectList(context.Tags, "Id", "Name");
            return View();
        }

        public IActionResult AddCategory()
        {
            return View();
        }

        public IActionResult AddTag()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductViewModel model)
        {
            try
            {
                    string path = await UploadFileHelper.UploadFile(model.ImageUrl);

                    Product product = _mapper.Map<Product>(model);
                    product.ImageUrl = path;

                    context.Add(product);
                    await context.SaveChangesAsync();


                    foreach (var tag in model.TagIds)
                    {
                        context.Add(new ProductTag { TagId = tag, ProductId = product.Id });
                    }

                    await context.SaveChangesAsync();

                    return View("Products", context.Products.ToList());
            }
            catch (Exception)
            {
                return View("Error", new ErrorViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(AddCategoryViewModel category)
        {
            if(ModelState.IsValid)
            {
                var c = _mapper.Map<Category>(category);
                context.Categories.Add(c);
                await context.SaveChangesAsync();
                return RedirectToAction("Categories");
            }

            return View("Error", new ErrorViewModel());
            
        }

        [HttpPost]
        public async Task<IActionResult> AddTag(AddTagViewModel tag)
        {

            if (ModelState.IsValid)
            {
                var t = _mapper.Map<Tag>(tag);
                context.Tags.Add(t);
                await context.SaveChangesAsync();
                return RedirectToAction("Tags");
            }
            return RedirectToAction("Error",new ErrorViewModel());
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var c = context.Categories.FirstOrDefault(x => x.Id == id);
                if (c is not null)
                {
                    context.Remove(c);
                    await context.SaveChangesAsync();
                }

                return RedirectToAction("Categories");
            }
            catch (Exception)
            {

                return View("Error");
            }
            
        }

        public async Task<IActionResult> DeleteTag(int id)
        {
            try
            {
                var t = context.Tags.FirstOrDefault(x => x.Id == id);
                if (t is not null)
                {
                    context.Remove(t);
                    await context.SaveChangesAsync();
                }

                return RedirectToAction("Tags");
            }
            catch (Exception)
            {

                return View("Error");
            }
        }
    }
}
