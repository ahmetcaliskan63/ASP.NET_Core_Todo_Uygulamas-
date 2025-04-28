using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoUygulaması.Models;
using ToDoUygulaması.Repositories;

namespace ToDoUygulaması.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICacheService _cacheService;
        private const string CategoryListCacheKey = "CategoryList";

        public CategoryService(ICategoryRepository categoryRepository, ICacheService cacheService)
        {
            _categoryRepository = categoryRepository;
            _cacheService = cacheService;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            var cachedCategories = _cacheService.Get<IEnumerable<Category>>(CategoryListCacheKey);
            if (cachedCategories != null)
            {
                return cachedCategories;
            }

            var categories = await _categoryRepository.GetAllAsync();
            _cacheService.Set(CategoryListCacheKey, categories, TimeSpan.FromMinutes(10));
            return categories;
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _categoryRepository.GetByIdAsync(id);
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _categoryRepository.AddAsync(category);
            _cacheService.Remove(CategoryListCacheKey);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            await _categoryRepository.UpdateAsync(category);
            _cacheService.Remove(CategoryListCacheKey);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _categoryRepository.DeleteAsync(id);
            _cacheService.Remove(CategoryListCacheKey);
        }
    }
}