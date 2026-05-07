using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ESAN.LOGISTICA.API.Data;

namespace ESAN.LOGISTICA.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly LogisticaDbContext _context;

        public ProductsController(LogisticaDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        // api/products
        // api/products?includeCategory=true
        [HttpGet]
        public async Task<ActionResult> GetProducts(
            bool includeCategory = false)
        {
            // SIN JOIN
            if (!includeCategory)
            {
                var products = await _context.Products
                    .Select(p => new
                    {
                        p.IdProducto,
                        p.Name,
                        p.Price,
                        p.IdCategory
                    })
                    .ToListAsync();

                return Ok(products);
            }

            // CON JOIN
            var result = await _context.Products
                .Include(p => p.IdCategoryNavigation)
                .Select(p => new
                {
                    p.IdProducto,
                    p.Name,
                    p.Price,
                    p.IdCategory,
                    Category = p.IdCategoryNavigation == null
                        ? null
                        : new
                        {
                            p.IdCategoryNavigation.IdCategory,
                            p.IdCategoryNavigation.CategoryName
                        }
                })
                .ToListAsync();

            return Ok(result);
        }

        // GET: api/Products/5
        // api/products/1
        // api/products/1?includeCategory=true
        [HttpGet("{id}")]
        public async Task<ActionResult> GetProducts(
            int id,
            bool includeCategory = false)
        {
            // SIN JOIN
            if (!includeCategory)
            {
                var product = await _context.Products
                    .Where(p => p.IdProducto == id)
                    .Select(p => new
                    {
                        p.IdProducto,
                        p.Name,
                        p.Price,
                        p.IdCategory
                    })
                    .FirstOrDefaultAsync();

                if (product == null)
                {
                    return NotFound();
                }

                return Ok(product);
            }

            // CON JOIN
            var result = await _context.Products
                .Include(p => p.IdCategoryNavigation)
                .Where(p => p.IdProducto == id)
                .Select(p => new
                {
                    p.IdProducto,
                    p.Name,
                    p.Price,
                    p.IdCategory,
                    Category = p.IdCategoryNavigation == null
                        ? null
                        : new
                        {
                            p.IdCategoryNavigation.IdCategory,
                            p.IdCategoryNavigation.CategoryName
                        }
                })
                .FirstOrDefaultAsync();

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducts(int id, Products products)
        {
            if (id != products.IdProducto)
            {
                return BadRequest();
            }

            _context.Entry(products).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Products>> PostProducts(Products products)
        {
            _context.Products.Add(products);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetProducts),
                new { id = products.IdProducto },
                products
            );
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducts(int id)
        {
            var products = await _context.Products.FindAsync(id);

            if (products == null)
            {
                return NotFound();
            }

            _context.Products.Remove(products);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.IdProducto == id);
        }
    }
}