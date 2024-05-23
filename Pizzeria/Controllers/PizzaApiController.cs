using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pizzeria_project.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace pizzeria_project.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PizzaApiController : ControllerBase
    {
        // GET: api/<PizzaControllerApi>
        [HttpGet]
        public IActionResult Get()
        {
            using PizzaContext db = new();
            IQueryable<Pizza> pizzas = db.Pizzas;
            return Ok(pizzas.ToList());
        }

        // GET api/<PizzaControllerApi>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            using PizzaContext db = new();
            Pizza? pizza = db.Pizzas.Find(id);
            if (pizza == null)
                return NotFound();
            return Ok(pizza);
        }

        // POST api/<PizzaControllerApi>
        [HttpPost]
        public IActionResult Post([FromBody] Pizza pizza)
        {
            using PizzaContext db = new();
            db.Pizzas.Add(new Pizza(pizza.Name, pizza.Description, pizza.Price, pizza.CategoryId));
            db.SaveChanges();

            return Ok();
        }

        // PUT api/<PizzaControllerApi>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Pizza pizza)
        {
            using PizzaContext db = new();
            Pizza? pizzaToEdit = db.Pizzas.Find(id);
            if (pizzaToEdit == null)
                return NotFound();

            pizza.Id = id;
            db.Pizzas.Update(pizza);
            db.SaveChanges();

            return Ok();
        }

        // DELETE api/<PizzaControllerApi>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using PizzaContext db = new();
            Pizza? pizzaToDelete = db.Pizzas.Find(id);
            if (pizzaToDelete == null)
                return NotFound();

            db.Pizzas.Remove(pizzaToDelete);
            db.SaveChanges();
            return Ok();
        }
    }
}
