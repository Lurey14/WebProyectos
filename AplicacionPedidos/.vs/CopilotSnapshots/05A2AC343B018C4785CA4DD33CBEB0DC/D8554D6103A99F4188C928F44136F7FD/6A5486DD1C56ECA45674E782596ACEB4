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
using System.Diagnostics;

namespace AplicacionPedidos.Controllers
{
    [Authorize] // Agregamos el decorador para restringir acceso a usuarios autenticados
    public class OrderModelsController : Controller
    {
        private readonly DBPedidosContext _context;

        public OrderModelsController(DBPedidosContext context)
        {
            _context = context;
        }

        // GET: OrderModels
        public async Task<IActionResult> Index(string searchString, string estado, DateTime? fechaInicio, DateTime? fechaFin)
        {
            try
            {
                ViewData["CurrentFilter"] = searchString;
                ViewData["CurrentEstado"] = estado;
                ViewData["CurrentFechaInicio"] = fechaInicio?.ToString("yyyy-MM-dd");
                ViewData["CurrentFechaFin"] = fechaFin?.ToString("yyyy-MM-dd");

                ViewData["Estados"] = OrderModel.EstadosDisponibles;
                var pedidos = _context.Orders
                    .Include(o => o.Cliente)
                    .Include(o => o.Items)
                        .ThenInclude(i => i.Producto)
                    .AsQueryable();

                if (!String.IsNullOrEmpty(searchString))
                {
                    pedidos = pedidos.Where(o => 
                        o.Cliente.Nombre.Contains(searchString) || 
                        o.DireccionEntrega.Contains(searchString));
                }

                if (!String.IsNullOrEmpty(estado))
                {
                    pedidos = pedidos.Where(o => o.Estado == estado);
                }

                if (fechaInicio.HasValue)
                {
                    pedidos = pedidos.Where(o => o.Fecha.Date >= fechaInicio.Value.Date);
                }

                if (fechaFin.HasValue)
                {
                    pedidos = pedidos.Where(o => o.Fecha.Date <= fechaFin.Value.Date);
                }

                pedidos = pedidos.OrderByDescending(o => o.Fecha);

                return View(await pedidos.ToListAsync());
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Ocurrió un error al cargar los pedidos: {ex.Message}";
                return View(new List<OrderModel>());
            }
        }

        // GET: OrderModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderModel = await _context.Orders
                .Include(o => o.Cliente)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (orderModel == null)
            {
                return NotFound();
            }

            ViewBag.Estados = OrderModel.EstadosDisponibles;
            return View(orderModel);
        }

        // GET: OrderModels/Create
        public IActionResult Create()
        {
            var clientes = _context.Users.ToList();
            ViewBag.ClienteId = new SelectList(clientes, "Id", "Nombre");
            
            ViewBag.Productos = _context.Products.Where(p => p.Stock > 0).ToList();
            
            return View(new OrderModel { 
                Fecha = DateTime.Now, 
                Estado = "Pendiente" 
            });
        }

        // POST: OrderModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int UserId, string DireccionEntrega, string Notas, string Estado, DateTime Fecha, int[] productoIds, int[] cantidades)
        {
            try
            {
                // Sanitización de entradas
                DireccionEntrega = string.IsNullOrWhiteSpace(DireccionEntrega) ? string.Empty : DireccionEntrega.Trim();
                Notas = string.IsNullOrWhiteSpace(Notas) ? string.Empty : Notas.Trim();
                Estado = string.IsNullOrWhiteSpace(Estado) ? "Pendiente" : Estado.Trim();

                if (UserId <= 0)
                {
                    ModelState.AddModelError("UserId", "El cliente es obligatorio");
                    var clientes = _context.Users.ToList();
                    ViewBag.ClienteId = new SelectList(clientes, "Id", "Nombre", UserId);
                    ViewBag.Productos = _context.Products.Where(p => p.Stock > 0).ToList();
                    return View(new OrderModel { UserId = UserId, DireccionEntrega = DireccionEntrega, Notas = Notas });
                }

                if (string.IsNullOrWhiteSpace(DireccionEntrega))
                {
                    ModelState.AddModelError("DireccionEntrega", "La dirección de entrega es obligatoria");
                    var clientes = _context.Users.ToList();
                    ViewBag.ClienteId = new SelectList(clientes, "Id", "Nombre", UserId);
                    ViewBag.Productos = _context.Products.Where(p => p.Stock > 0).ToList();
                    return View(new OrderModel { UserId = UserId, DireccionEntrega = DireccionEntrega, Notas = Notas });
                }

                if (productoIds == null || productoIds.Length == 0 || cantidades == null || cantidades.Length == 0)
                {
                    ModelState.AddModelError("", "Debe seleccionar al menos un producto para el pedido");
                    var clientes = _context.Users.ToList();
                    ViewBag.ClienteId = new SelectList(clientes, "Id", "Nombre", UserId);
                    ViewBag.Productos = _context.Products.Where(p => p.Stock > 0).ToList();
                    return View(new OrderModel { UserId = UserId, DireccionEntrega = DireccionEntrega, Notas = Notas });
                }

                var orderModel = new OrderModel
                {
                    UserId = UserId,
                    DireccionEntrega = DireccionEntrega,
                    Notas = Notas ?? string.Empty,
                    Estado = string.IsNullOrEmpty(Estado) ? "Pendiente" : Estado,
                    Fecha = Fecha == default(DateTime) ? DateTime.Now : Fecha,
                    Items = new List<OrderItemModel>()
                };

                Dictionary<int, ProductModel> productosInfo = new Dictionary<int, ProductModel>();
                
                for (int i = 0; i < productoIds.Length; i++)
                {
                    int productoId = productoIds[i];
                    if (productoId <= 0) continue;
                    
                    if (!productosInfo.ContainsKey(productoId))
                    {
                        var producto = await _context.Products.FindAsync(productoId);
                        if (producto != null)
                        {
                            productosInfo[productoId] = producto;
                        }
                    }
                }
                
                // Ahora creamos los items del pedido
                for (int i = 0; i < Math.Min(productoIds.Length, cantidades.Length); i++)
                {
                    int productoId = productoIds[i];
                    int cantidad = cantidades[i];
                    
                    if (productoId <= 0 || cantidad <= 0) continue;
                    
                    if (!productosInfo.ContainsKey(productoId)) continue;
                    
                    var producto = productosInfo[productoId];
                    
                    // Verificar stock
                    if (producto.Stock < cantidad)
                    {
                        ModelState.AddModelError("", $"No hay suficiente stock para el producto {producto.Nombre}. Stock disponible: {producto.Stock}");
                        continue;
                    }
                    
                    // Crear item
                    var item = new OrderItemModel
                    {
                        ProductoId = productoId,
                        Cantidad = cantidad,
                        PrecioUnitario = producto.Precio,
                        Subtotal = producto.Precio * cantidad
                    };
                    
                    orderModel.Items.Add(item);
                }
                
                // Verificar que se agregaron productos
                if (orderModel.Items.Count == 0)
                {
                    ModelState.AddModelError("", "No se pudieron agregar productos al pedido debido a problemas de stock");
                    var clientes = _context.Users.ToList();
                    ViewBag.ClienteId = new SelectList(clientes, "Id", "Nombre", UserId);
                    ViewBag.Productos = _context.Products.Where(p => p.Stock > 0).ToList();
                    return View(orderModel);
                }
                
                // Calcular total
                orderModel.CalcularTotal();
                
                // Guardar en la base de datos usando una transacción
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // Guardar el pedido
                        _context.Orders.Add(orderModel);
                        await _context.SaveChangesAsync();
                        
                        // Actualizar stock de productos
                        foreach (var item in orderModel.Items)
                        {
                            var producto = productosInfo[item.ProductoId];
                            producto.Stock -= item.Cantidad;
                            _context.Update(producto);
                        }
                        
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        
                        TempData["SuccessMessage"] = "Pedido creado correctamente";
                        return RedirectToAction(nameof(Details), new { id = orderModel.Id });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        ModelState.AddModelError("", $"Error al guardar el pedido: {ex.Message}");
                        
                        var clientes = _context.Users.ToList();
                        ViewBag.ClienteId = new SelectList(clientes, "Id", "Nombre", UserId);
                        ViewBag.Productos = _context.Products.Where(p => p.Stock > 0).ToList();
                        return View(orderModel);
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error inesperado: {ex.Message}");
                
                var clientes = _context.Users.ToList();
                ViewBag.ClienteId = new SelectList(clientes, "Id", "Nombre", UserId);
                ViewBag.Productos = _context.Products.Where(p => p.Stock > 0).ToList();
                return View(new OrderModel { UserId = UserId, DireccionEntrega = DireccionEntrega, Notas = Notas });
            }
        }

        // GET: OrderModels/Edit/5
        [Authorize(Roles = "Admin,Empleado")] // Solo administradores y empleados pueden editar pedidos
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderModel = await _context.Orders
                .Include(o => o.Cliente)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (orderModel == null)
            {
                return NotFound();
            }

            ViewBag.Estados = new SelectList(OrderModel.EstadosDisponibles, orderModel.Estado);
            return View(orderModel);
        }

        // POST: OrderModels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Empleado")] // Solo administradores y empleados pueden editar pedidos
        public async Task<IActionResult> Edit(int id, OrderModel orderModel)
        {
            if (id != orderModel.Id)
            {
                return NotFound();
            }

            try
            {
                // Sanitización de entradas
                orderModel.DireccionEntrega = string.IsNullOrWhiteSpace(orderModel.DireccionEntrega) ? string.Empty : orderModel.DireccionEntrega.Trim();
                orderModel.Notas = string.IsNullOrWhiteSpace(orderModel.Notas) ? string.Empty : orderModel.Notas.Trim();
                orderModel.Estado = string.IsNullOrWhiteSpace(orderModel.Estado) ? "Pendiente" : orderModel.Estado.Trim();
                
                // Solo permitir actualizar el estado y las notas
                var existingOrder = await _context.Orders
                    .Include(o => o.Items)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (existingOrder == null)
                {
                    return NotFound();
                }

                // Actualizar solo estado y notas
                existingOrder.Estado = orderModel.Estado;
                existingOrder.Notas = orderModel.Notas;
                existingOrder.DireccionEntrega = orderModel.DireccionEntrega;

                _context.Update(existingOrder);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Estado del pedido actualizado correctamente";
                return RedirectToAction(nameof(Details), new { id = orderModel.Id });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderModelExists(orderModel.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al actualizar el pedido: {ex.Message}");
            }
            
            ViewBag.Estados = new SelectList(OrderModel.EstadosDisponibles, orderModel.Estado);
            return View(orderModel);
        }

        // GET: OrderModels/Delete/5
        [Authorize(Roles = "Admin")] // Solo administradores pueden eliminar pedidos
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderModel = await _context.Orders
                .Include(o => o.Cliente)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (orderModel == null)
            {
                return NotFound();
            }

            return View(orderModel);
        }

        // POST: OrderModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Solo administradores pueden eliminar pedidos
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderModel = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (orderModel != null)
            {
                try
                {
                    if (orderModel.Estado != "Entregado")
                    {
                        foreach (var item in orderModel.Items)
                        {
                            var producto = await _context.Products.FindAsync(item.ProductoId);
                            if (producto != null)
                            {
                                producto.Stock += item.Cantidad;
                                _context.Update(producto);
                            }
                        }
                    }

                    _context.OrderItems.RemoveRange(orderModel.Items);
                    
                    _context.Orders.Remove(orderModel);
                    await _context.SaveChangesAsync();
                    
                    TempData["SuccessMessage"] = "Pedido eliminado correctamente";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error al eliminar el pedido: {ex.Message}";
                }
            }
            
            return RedirectToAction(nameof(Index));
        }

        // POST: OrderModels/CambiarEstado/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Empleado")] // Solo administradores y empleados pueden cambiar el estado
        public async Task<IActionResult> CambiarEstado(int id, string nuevoEstado)
        {
            var orderModel = await _context.Orders.FindAsync(id);
            if (orderModel == null)
            {
                return NotFound();
            }

            // Sanitizar el estado
            nuevoEstado = string.IsNullOrWhiteSpace(nuevoEstado) ? "Pendiente" : nuevoEstado.Trim();

            if (OrderModel.EstadosDisponibles.Contains(nuevoEstado))
            {
                orderModel.Estado = nuevoEstado;
                _context.Update(orderModel);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Estado del pedido actualizado a {nuevoEstado}";
            }
            else
            {
                TempData["ErrorMessage"] = "Estado no válido";
            }
            
            return RedirectToAction(nameof(Details), new { id = id });
        }

        // GET: OrderModels/CreateRapido
        [Authorize] // Asegura que solo usuarios autenticados puedan crear pedidos rápidos
        public IActionResult CreateRapido()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Productos = _context.Products.Where(p => p.Stock > 0).ToList();
                var userId = int.Parse(User.Identity.Name.Split('@')[0]);
                return View(new OrderModel { UserId = userId, Fecha = DateTime.Now, Estado = "Pendiente" });
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // POST: OrderModels/CreateRapido
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize] // Asegura que solo usuarios autenticados puedan crear pedidos rápidos
        public async Task<IActionResult> CreateRapido(OrderModel orderModel, int[] productoIds, int[] cantidades)
        {
            try
            {
                // Sanitización de entradas
                orderModel.DireccionEntrega = string.IsNullOrWhiteSpace(orderModel.DireccionEntrega) ? string.Empty : orderModel.DireccionEntrega.Trim();
                orderModel.Notas = string.IsNullOrWhiteSpace(orderModel.Notas) ? string.Empty : orderModel.Notas.Trim();
                orderModel.Estado = "Pendiente"; // Estado fijo para pedidos rápidos
                
                if (ModelState.IsValid)
                {
                    if (productoIds == null || productoIds.Length == 0 || cantidades == null || cantidades.Length == 0)
                    {
                        ModelState.AddModelError("", "Debe seleccionar al menos un producto para el pedido");
                        ViewBag.Productos = _context.Products.Where(p => p.Stock > 0).ToList();
                        return View(orderModel);
                    }

                    Dictionary<int, int> stockDisponible = new Dictionary<int, int>();
                    Dictionary<int, ProductModel> productosInfo = new Dictionary<int, ProductModel>();
                    
                    for (int i = 0; i < productoIds.Length; i++)
                    {
                        int productoId = productoIds[i];
                        if (!stockDisponible.ContainsKey(productoId))
                        {
                            var producto = await _context.Products.FindAsync(productoId);
                            if (producto == null) continue;
                            stockDisponible[productoId] = producto.Stock;
                            productosInfo[productoId] = producto;
                        }
                    }

                    orderModel.Items = new List<OrderItemModel>();
                    for (int i = 0; i < productoIds.Length; i++)
                    {
                        if (i >= cantidades.Length) continue;
                        
                        int productoId = productoIds[i];
                        int cantidad = cantidades[i];
                        
                        if (cantidad <= 0 || !stockDisponible.ContainsKey(productoId)) continue;
                        
                        if (stockDisponible[productoId] < cantidad)
                        {
                            ModelState.AddModelError("", $"No hay suficiente stock para el producto {productosInfo[productoId].Nombre}");
                            continue;
                        }
                        
                        stockDisponible[productoId] -= cantidad;
                        
                        var item = new OrderItemModel
                        {
                            ProductoId = productoId,
                            Cantidad = cantidad,
                            PrecioUnitario = productosInfo[productoId].Precio,
                            Subtotal = productosInfo[productoId].Precio * cantidad
                        };
                        
                        orderModel.Items.Add(item);
                    }
                    
                    if (orderModel.Items.Count == 0)
                    {
                        ModelState.AddModelError("", "No se pudieron agregar productos al pedido");
                        ViewBag.Productos = _context.Products.Where(p => p.Stock > 0).ToList();
                        return View(orderModel);
                    }

                    orderModel.CalcularTotal();
                    orderModel.Fecha = DateTime.Now;
                    orderModel.Estado = "Pendiente"; // Aseguramos que siempre comienza como pendiente
                    
                    // Usar transacción para garantizar integridad de datos
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            _context.Add(orderModel);
                            await _context.SaveChangesAsync();
                            
                            foreach (var item in orderModel.Items)
                            {
                                var producto = await _context.Products.FindAsync(item.ProductoId);
                                if (producto != null)
                                {
                                    producto.Stock -= item.Cantidad;
                                    _context.Update(producto);
                                }
                            }
                            
                            await _context.SaveChangesAsync();
                            transaction.Commit();
                            
                            TempData["SuccessMessage"] = "Su pedido ha sido creado correctamente";
                            return RedirectToAction(nameof(Details), new { id = orderModel.Id });
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Error al guardar el pedido en la base de datos", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al crear el pedido: {ex.Message}");
            }
            
            ViewBag.Productos = _context.Products.Where(p => p.Stock > 0).ToList();
            return View(orderModel);
        }

        private bool OrderModelExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
