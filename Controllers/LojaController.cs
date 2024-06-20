using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RedeLojas.Data;
using RedeLojas.Models;
using RedeLojas.ViewModel;

namespace RedeLojas.Controllers
{
    public class LojaController : Controller
    {
        private readonly RedeLojasContext _context;

        public LojaController(RedeLojasContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Lojas.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loja = await _context.Lojas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loja == null)
            {
                return NotFound();
            }

            return View(loja);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome")] Loja loja)
        {
            if (loja.Nome != null)
            {
                _context.Add(loja);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(loja);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loja = await _context.Lojas
                .Include(l => l.ClienteLojas)
                .ThenInclude(cl => cl.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loja == null)
            {
                return NotFound();
            }
            
            var viewModel = new LojaViewModel
            {
                Id = loja.Id,
                Nome = loja.Nome,
                ClientesSelecionados = loja.ClienteLojas.Select(cl => cl.ClienteId).ToList(),
                TodosClientes = await _context.Clientes.ToListAsync()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LojaViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (viewModel.Id != null && viewModel.Nome != null)
            {
                var lojaParaAtualizar = await _context.Lojas
                    .Include(l => l.ClienteLojas)
                    .FirstOrDefaultAsync(l => l.Id == id);

                if (lojaParaAtualizar == null)
                {
                    return NotFound();
                }

                lojaParaAtualizar.Nome = viewModel.Nome;

                var clientesExistentes = lojaParaAtualizar.ClienteLojas.Select(cl => cl.ClienteId).ToList();
                var novosClientes = viewModel.ClientesSelecionados.Except(clientesExistentes).ToList();
                var clientesParaRemover = clientesExistentes.Except(viewModel.ClientesSelecionados).ToList();

                foreach (var clienteId in clientesParaRemover)
                {
                    var clienteParaRemover = lojaParaAtualizar.ClienteLojas.FirstOrDefault(cl => cl.ClienteId == clienteId);
                    lojaParaAtualizar.ClienteLojas.Remove(clienteParaRemover);
                }

                foreach (var clienteId in novosClientes)
                {
                    lojaParaAtualizar.ClienteLojas.Add(new ClienteLoja { ClienteId = clienteId, LojaId = lojaParaAtualizar.Id });
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LojaExists(viewModel.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }
            viewModel.TodosClientes = await _context.Clientes.ToListAsync();
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var loja = await _context.Lojas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (loja == null)
            {
                return NotFound();
            }

            return View(loja);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var loja = await _context.Lojas.FindAsync(id);
            if (loja != null)
            {
                _context.Lojas.Remove(loja);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LojaExists(int id)
        {
            return _context.Lojas.Any(e => e.Id == id);
        }
    }
}
