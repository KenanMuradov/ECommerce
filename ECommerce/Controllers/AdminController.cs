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

        public AdminController(AppDbContext context)
        {
            this.context = context;
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

                Product product = new()
                {
                    Name = model.Name,
                    Description = model.Description,
                    ImageUrl = path,
                    CategoryId = model.CategoryId,
                };

                context.Add(product);
                await context.SaveChangesAsync();

                foreach (var tag in model.TagIds)
                {
                    context.Add(new ProductTag { TagId = tag, ProductId = product.Id });
                }

                await context.SaveChangesAsync();

                return View("Index");
            }
            catch (Exception)
            {

                return View("error.cshtml");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {

            context.Add(category);
            await context.SaveChangesAsync();
            return RedirectToAction("Categories");
        }

        [HttpPost]
        public async Task<IActionResult> AddTag(Tag tag)
        {
            context.Add(tag);
            await context.SaveChangesAsync();
            return RedirectToAction("Tags");
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
