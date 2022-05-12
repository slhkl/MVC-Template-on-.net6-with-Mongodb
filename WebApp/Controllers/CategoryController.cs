using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    public class CategoryController : Controller
    {
        private CategoryServices _categoryServices;

        public CategoryController(IOptions<DatabaseSetting> databaseSettingsFromAppSettings)
        {
            _categoryServices = new CategoryServices(databaseSettingsFromAppSettings.Value);
        }

        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync()
        {
            List<Category>? list = await _categoryServices.Get();
            return View(list);
        }

        //Get
        public IActionResult Create()
        {
            return View();
        }

        //Post
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(Category obj)
        {
            if (obj.DisplayOrder.ToString() == obj.Name)
            {
                //If we write name instead of CustomError, an error message will appear under name.
                ModelState.AddModelError("CustomError", "Name and Display Order can not be same");
            }

            if (ModelState.IsValid)
            {
                await _categoryServices.Create(obj);
                TempData["success"] = "Category created succesfuly";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //Get
        [ActionName("Edit")]
        public async Task<IActionResult> EditAsync(string? id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Category categoryFromDb = await _categoryServices.Get(id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        //Post
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(Category obj)
        {
            if (obj.DisplayOrder.ToString() == obj.Name)
            {
                //If we write name instead of CustomError, an error message will appear under name.
                ModelState.AddModelError("CustomError", "Name and Display Order can not be same");
            }

            if (ModelState.IsValid)
            {
                await _categoryServices.Update(obj);
                TempData["success"] = "Category updated succesfuly";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //Get
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteAsync(string? id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            Category categoryFromDb = await _categoryServices.Get(id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        //Post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePostAsync(string id)
        {
            Category categoryFromDb = await _categoryServices.Get(id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            await _categoryServices.Delete(id);
            TempData["success"] = "Category deleted succesfuly";
            return RedirectToAction("Index");
        }
    }
}
