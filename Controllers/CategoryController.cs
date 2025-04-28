using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ToDoUygulaması.Models;
using ToDoUygulaması.Services;

namespace ToDoUygulaması.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return View(categories);
        }

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Kategori ekleme işlemi öncesi debug bilgisi
                    Console.WriteLine($"Kategori ekleniyor: {category.Name}, {category.Description}");
                    
                    await _categoryService.AddCategoryAsync(category);
                    
                    // Başarılı mesajını TempData'ya ekle
                    TempData["SuccessMessage"] = "Kategori başarıyla eklendi!";
                    
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Hata detaylarını loglama
                    Console.WriteLine($"Kategori eklenirken hata: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                    
                    ModelState.AddModelError("", "Kategori eklenirken bir hata oluştu: " + ex.Message);
                }
            }
            else
            {
                // ModelState hatalarını loglama
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine($"ModelState hatası: {error.ErrorMessage}");
                    }
                }
            }
            
            return View(category);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryService.UpdateCategoryAsync(category);
                    TempData["SuccessMessage"] = "Kategori başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Kategori güncellenirken bir hata oluştu: " + ex.Message);
                }
            }
            return View(category);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            TempData["SuccessMessage"] = "Kategori başarıyla silindi!";
            return RedirectToAction(nameof(Index));
        }
    }
}