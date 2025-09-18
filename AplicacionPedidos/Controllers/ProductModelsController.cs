using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AplicacionPedidos.Data;
using AplicacionPedidos.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace AplicacionPedidos.Controllers
{
    public class ProductModelsController : Controller
    {
        private readonly DBPedidosContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductModelsController(DBPedidosContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: ProductModels
        public async Task<IActionResult> Index(string searchString, string categoria, decimal? precioMin, decimal? precioMax, string sortOrder)
        {
            try
            {
                ViewData["NombreSortParm"] = String.IsNullOrEmpty(sortOrder) ? "nombre_desc" : "";
                ViewData["PrecioSortParm"] = sortOrder == "Precio" ? "precio_desc" : "Precio";
                ViewData["StockSortParm"] = sortOrder == "Stock" ? "stock_desc" : "Stock";
                ViewData["CategoriaSortParm"] = sortOrder == "Categoria" ? "categoria_desc" : "Categoria";

                ViewData["CurrentFilter"] = searchString;
                ViewData["CurrentCategoria"] = categoria;
                ViewData["CurrentPrecioMin"] = precioMin;
                ViewData["CurrentPrecioMax"] = precioMax;

                var productos = _context.Products.AsQueryable();

                List<string> categorias = new List<string>();
                try
                {
                    categorias = await productos
                        .Where(p => p.Categoria != null && p.Categoria != "")
                        .Select(p => p.Categoria)
                        .Distinct()
                        .ToListAsync();
                }
                catch
                {
                    categorias = new List<string> { "Electrónica", "Ropa", "Hogar", "Alimentos", "Juguetes", "Libros", "Otros" };
                }

                ViewData["Categorias"] = categorias;

                if (!String.IsNullOrEmpty(searchString))
                {
                    productos = productos.Where(s => s.Nombre.Contains(searchString) ||
                                                s.Descripcion.Contains(searchString));
                }

                if (!String.IsNullOrEmpty(categoria))
                {
                    productos = productos.Where(p => p.Categoria == categoria);
                }

                if (precioMin.HasValue)
                {
                    productos = productos.Where(p => p.Precio >= precioMin.Value);
                }

                if (precioMax.HasValue)
                {
                    productos = productos.Where(p => p.Precio <= precioMax.Value);
                }

                switch (sortOrder)
                {
                    case "nombre_desc":
                        productos = productos.OrderByDescending(p => p.Nombre);
                        break;
                    case "Precio":
                        productos = productos.OrderBy(p => p.Precio);
                        break;
                    case "precio_desc":
                        productos = productos.OrderByDescending(p => p.Precio);
                        break;
                    case "Stock":
                        productos = productos.OrderBy(p => p.Stock);
                        break;
                    case "stock_desc":
                        productos = productos.OrderByDescending(p => p.Stock);
                        break;
                    case "Categoria":
                        productos = productos.OrderBy(p => p.Categoria);
                        break;
                    case "categoria_desc":
                        productos = productos.OrderByDescending(p => p.Categoria);
                        break;
                    default:
                        productos = productos.OrderBy(p => p.Nombre);
                        break;
                }

                return View(await productos.ToListAsync());
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Ocurrió un error al cargar los productos: {ex.Message}";
                return View(new List<ProductModel>());
            }
        }

        // GET: ProductModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productModel == null)
            {
                return NotFound();
            }

            return View(productModel);
        }

        // GET: ProductModels/Create
        public IActionResult Create()
        {
            var categorias = new List<string> { "Electrónica", "Ropa", "Hogar", "Alimentos", "Juguetes", "Libros", "Otros" };
            ViewBag.Categorias = new SelectList(categorias);
            return View();
        }

        // POST: ProductModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductModel productModel)
        {
            try
            {
                if (productModel.Categoria == null)
                {
                    productModel.Categoria = string.Empty;
                }

                productModel.ImagenUrl = "/images/products/no-image.jpg";

                if (productModel.ImagenFile != null)
                {
                    try
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");

                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + productModel.ImagenFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await productModel.ImagenFile.CopyToAsync(fileStream);
                        }

                        productModel.ImagenUrl = "/images/products/" + uniqueFileName;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al procesar imagen: {ex.Message}");
                    }
                }

                _context.Add(productModel);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Producto creado con éxito";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al crear el producto: {ex.Message}");
                Console.WriteLine($"ERROR: {ex.Message}\n{ex.StackTrace}");

                var categorias = new List<string> { "Electrónica", "Ropa", "Hogar", "Alimentos", "Juguetes", "Libros", "Otros" };
                ViewBag.Categorias = new SelectList(categorias);

                return View(productModel);
            }
        }

        // GET: ProductModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _context.Products.FindAsync(id);
            if (productModel == null)
            {
                return NotFound();
            }

            var categorias = new List<string> { "Electrónica", "Ropa", "Hogar", "Alimentos", "Juguetes", "Libros", "Otros" };
            ViewBag.Categorias = new SelectList(categorias);
            return View(productModel);
        }

        // POST: ProductModels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductModel productModel)
        {
            if (id != productModel.Id)
            {
                return NotFound();
            }

            try
            {
                if (productModel.Categoria == null)
                {
                    productModel.Categoria = string.Empty;
                }

                if (productModel.ImagenFile != null)
                {
                    try
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");

                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + productModel.ImagenFile.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await productModel.ImagenFile.CopyToAsync(fileStream);
                        }

                        productModel.ImagenUrl = "/images/products/" + uniqueFileName;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al procesar imagen: {ex.Message}");
                    }
                }

                _context.Update(productModel);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Producto actualizado con éxito";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al editar el producto: {ex.Message}");

                var categorias = new List<string> { "Electrónica", "Ropa", "Hogar", "Alimentos", "Juguetes", "Libros", "Otros" };
                ViewBag.Categorias = new SelectList(categorias);

                return View(productModel);
            }
        }

        // GET: ProductModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productModel = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productModel == null)
            {
                return NotFound();
            }

            return View(productModel);
        }

        // POST: ProductModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var productModel = await _context.Products.FindAsync(id);
                if (productModel != null)
                {
                    _context.Products.Remove(productModel);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Producto eliminado con éxito";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al eliminar producto: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        private bool ProductModelExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
