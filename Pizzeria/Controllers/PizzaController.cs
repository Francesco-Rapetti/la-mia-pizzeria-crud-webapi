﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pizzeria_project.Models;

namespace pizzeria_project.Controllers
{
    public class PizzaController : Controller
    {
        public IActionResult Seed()
        {
            using PizzaContext db = new();
            db.Pizzas.RemoveRange(db.Pizzas);
			db.Categories.RemoveRange(db.Categories);
			db.Ingredients.RemoveRange(db.Ingredients);
            List<Pizza> pizzas = new();
            List<Category> categories = new();
            List<Ingredient> ingredients = new();
            {
                db.Categories.Add(new Category("Classica", "primary"));
                db.Categories.Add(new Category("Bianca", "light"));
                db.Categories.Add(new Category("Rossa", "danger"));
                db.Categories.Add(new Category("Vegetariana", "success"));
                db.Categories.Add(new Category("Esotica", "warning"));

                db.SaveChanges();

                categories = db.Categories.ToList();
            }

            {
                db.Ingredients.Add(new Ingredient("Mozzarella"));
                db.Ingredients.Add(new Ingredient("Pomodoro"));
                db.Ingredients.Add(new Ingredient("Ananas"));
                db.Ingredients.Add(new Ingredient("Salame piccante"));
                db.Ingredients.Add(new Ingredient("Gorgonzola"));
                db.Ingredients.Add(new Ingredient("Fior di latte"));
                db.Ingredients.Add(new Ingredient("Funghi"));
                db.Ingredients.Add(new Ingredient("Carciofi"));
                db.Ingredients.Add(new Ingredient("Olive"));
                db.Ingredients.Add(new Ingredient("Prosciutto"));
                
                db.SaveChanges();

                ingredients = db.Ingredients.ToList();
            }

            {
                pizzas.Add(new Pizza(
                    "Margherita", 
                    "Pizza rossa con mozzarella e pomodoro", 
                    4.99, 
                    categories[0].Id,
                    "~/img/margherita.png"
                    ));
                pizzas[0].Ingredients?.Add(db.Ingredients.Find(ingredients[0].Id));
                pizzas[0].Ingredients?.Add(db.Ingredients.Find(ingredients[1].Id));
                pizzas.Add(new Pizza(
                    "Diavola", 
                    "Pizza rossa con mozzarella, pomodoro e salame piccante", 
                    5.99, 
                    categories[2].Id, 
                    "~/img/diavola.png"
                    ));
				pizzas[1].Ingredients?.Add(db.Ingredients.Find(ingredients[0].Id));
				pizzas[1].Ingredients?.Add(db.Ingredients.Find(ingredients[1].Id));
				pizzas[1].Ingredients?.Add(db.Ingredients.Find(ingredients[3].Id));
                pizzas.Add(new Pizza(
                    "Hawaiana", 
                    "Pizza bianca con mozzarella, prosciutto e ananas", 
                    6.99, categories[4].Id, 
                    "~/img/hawaiana.png"
                    ));
				pizzas[2].Ingredients?.Add(db.Ingredients.Find(ingredients[0].Id));
				pizzas[2].Ingredients?.Add(db.Ingredients.Find(ingredients[2].Id));
				pizzas[2].Ingredients?.Add(db.Ingredients.Find(ingredients[9].Id));
                pizzas.Add(new Pizza(
                    "Quattro Formaggi",
                    "Pizza bianca con mozzarella, gorgonzola, fior di latte e parmigiano", 
                    7.99, categories[1].Id, 
                    "~/img/quattro-formaggi.png"
                    ));
				pizzas[3].Ingredients?.Add(db.Ingredients.Find(ingredients[0].Id));
				pizzas[3].Ingredients?.Add(db.Ingredients.Find(ingredients[4].Id));
				pizzas[3].Ingredients?.Add(db.Ingredients.Find(ingredients[5].Id));
                pizzas.Add(new Pizza(
                    "Quattro Stagioni",
                    "Pizza bianca con mozzarella, fior di latte, funghi e olive", 
                    8.99, categories[3].Id, 
                    "~/img/quattro-stagioni.png"
                    ));
				pizzas[4].Ingredients?.Add(db.Ingredients.Find(ingredients[0].Id));
				pizzas[4].Ingredients?.Add(db.Ingredients.Find(ingredients[5].Id));
				pizzas[4].Ingredients?.Add(db.Ingredients.Find(ingredients[6].Id));
				pizzas[4].Ingredients?.Add(db.Ingredients.Find(ingredients[8].Id));
                pizzas.Add(new Pizza(
                    "Funghi", 
                    "Pizza bianca con mozzarella, funghi", 
                    9.99, categories[1].Id, 
                    "~/img/funghi.png"
                    ));
				pizzas[5].Ingredients?.Add(db.Ingredients.Find(ingredients[0].Id));
				pizzas[5].Ingredients?.Add(db.Ingredients.Find(ingredients[6].Id));
                pizzas.Add(new Pizza(
                    "Capricciosa", 
                    "Pizza bianca con mozzarella, carciofi, fior di latte e olive", 
                    10.99, categories[3].Id, 
                    "~/img/capricciosa.png"
                    ));
				pizzas[6].Ingredients?.Add(db.Ingredients.Find(ingredients[0].Id));
				pizzas[6].Ingredients?.Add(db.Ingredients.Find(ingredients[5].Id));
				pizzas[6].Ingredients?.Add(db.Ingredients.Find(ingredients[7].Id));
				pizzas[6].Ingredients?.Add(db.Ingredients.Find(ingredients[8].Id));
                db.Pizzas.AddRange(pizzas);

                db.SaveChanges();

            }


            return RedirectToAction("Index");
        }

        [Authorize(Roles = "ADMIN,USER")]
        public IActionResult Index()
        {
            using PizzaContext db = new();
            List<Pizza> pizzas = db.Pizzas.Include(p => p.Category).Include(p => p.Ingredients).ToList();
            return View(pizzas);
        }

        public IActionResult Show(int id)
        {
			using PizzaContext db = new();
			Pizza? pizza = db.Pizzas.Include(p => p.Category).Where(p => p.Id == id).Include(p => p.Ingredients).FirstOrDefault();
			return View(pizza);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                using PizzaContext db1 = new();
                //PizzaFormModel model = new();
                //model.Pizza = data.Pizza;
                data.Categories = db1.Categories.ToList();
                data.Ingredients = db1.Ingredients.ToList();
                //model.SelectedIngredients = pizza.Ingredients?.ToList();
                return View(data);
            }

            Pizza newPizza = new(data.Pizza.Name, data.Pizza.Description, data.Pizza.Price, data.Pizza.CategoryId);
            using PizzaContext db = new();
            foreach (int ingredientId in data.SelectedIngredientsIds)
            {
                newPizza.Ingredients?.Add(db.Ingredients.Find(ingredientId));
            }
            db.Pizzas.Add(newPizza);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Create()
        {
            using PizzaContext db = new();
            PizzaFormModel model = new();
            model.Pizza = new();
            model.Categories = db.Categories.ToList();
            model.Ingredients = db.Ingredients.ToList();
            return View(model);
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult Delete(int id)
        {
            using PizzaContext db = new();
            Pizza? pizza = db.Pizzas.Find(id);
            if (pizza == null)
            {
                return NotFound();
            }
            db.Pizzas.Remove(pizza);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "ADMIN")]
        public IActionResult Edit(int id)
        {
            using PizzaContext db = new();
            Pizza? pizza = db.Pizzas.Where(p => p.Id == id).Include(p => p.Ingredients).Include(p => p.Category).FirstOrDefault();
            if (pizza == null)
            {
                return NotFound();
            }
            PizzaFormModel model = new();
            model.Pizza = pizza;
            model.Categories = db.Categories.ToList();
            model.Ingredients = db.Ingredients.ToList();
            model.SelectedIngredientsIds = pizza.Ingredients?.Select(i => i.Id).ToList();
            return View(model);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                using PizzaContext db1 = new();
                PizzaFormModel model = new();
                model.Pizza = data.Pizza;
                model.Categories = db1.Categories.ToList();
                model.Ingredients = db1.Ingredients.ToList();
                model.SelectedIngredientsIds = data.SelectedIngredientsIds;
                return View(model);
            }
            using PizzaContext db = new();
            Pizza pizza = db.Pizzas
                .Include(p => p.Ingredients)
                .FirstOrDefault(p => p.Id == id);

            pizza.Ingredients.Clear();
            foreach (int ingredientId in data.SelectedIngredientsIds)
            {
                pizza.Ingredients.Add(db.Ingredients.Find(ingredientId));
            }
            pizza.Name = data.Pizza.Name;
            pizza.Description = data.Pizza.Description;
            pizza.Price = data.Pizza.Price;
            pizza.CategoryId = data.Pizza.CategoryId;

            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
