using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDoUygulaması.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [StringLength(50, ErrorMessage = "Kategori adı en fazla 50 karakter olabilir.")]
        public string Name { get; set; }

        public string Description { get; set; }

        // Todos koleksiyonunu başlatarak null olmasını önlüyoruz
        public ICollection<Todo> Todos { get; set; } = new List<Todo>();
    }
}