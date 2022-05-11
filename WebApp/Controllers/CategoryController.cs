using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class CategoryController : Controller
    {
        private IConfiguration _configuration;
        private CategoryServices _categoryServices;
        public CategoryController(IConfiguration iConfig)
        {
            _configuration = iConfig;
            DatabaseSetting databaseSetting = new DatabaseSetting(GetValue("ConnectionString"), GetValue("DatabaseName"), GetValue("CollectionName"));
            _categoryServices = new CategoryServices(databaseSetting);
        }
        public string GetValue(string s)
        {
            return _configuration.GetValue<string>("DatabaseSetting:" + s);
        }
        public IActionResult Index()
        {
            List<Category>? list = _categoryServices.Get().Result;
            return View(list);
        }

        //Get
        public IActionResult Create()
        {
            return View();
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.DisplayOrder.ToString() == obj.Name)
            {
                //If we write name instead of CustomError, an error message will appear under name.
                ModelState.AddModelError("CustomError", "Name and Display Order can not be same");
            }

            if (ModelState.IsValid)
            {
                _categoryServices.Create(obj).Wait();
                TempData["success"] = "Category created succesfuly";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //Get
        public IActionResult Edit(string? id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Category categoryFromDb = _categoryServices.Get(id).Result;
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.DisplayOrder.ToString() == obj.Name)
            {
                //If we write name instead of CustomError, an error message will appear under name.
                ModelState.AddModelError("CustomError", "Name and Display Order can not be same");
            }

            if (ModelState.IsValid)
            {
                _categoryServices.Update(obj.Id, obj).Wait();
                TempData["success"] = "Category updated succesfuly";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //Get
        public IActionResult Delete(string? id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Category categoryFromDb = _categoryServices.Get(id).Result;
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        //Post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(string id)
        {
            Category categoryFromDb = _categoryServices.Get(id).Result;
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            _categoryServices.Delete(id).Wait();
            TempData["success"] = "Category deleted succesfuly";
            return RedirectToAction("Index");
        }
    }
}
