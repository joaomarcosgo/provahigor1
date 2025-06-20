using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProvaHigorr.Data;
using ProvaHigorr.Models;
using ProvaHigorr.Filters;

namespace ProvaHigorr.Controllers
{
    [AutorizacaoSimples]
    public class ClienteController : Controller
    {
        public IActionResult Index()
        {
            var clientes = ClienteRepository.GetAll();
            return View(clientes);
        }

        [HttpGet]
        public IActionResult Editar(Guid? id)
        {
            var cliente = id.HasValue ? ClienteRepository.GetAll().FirstOrDefault(c => c.Id == id) : new Cliente();
            ViewBag.Cidades = Cidades.Lista;
            return View(cliente);
        }

        [HttpPost]
        public IActionResult Editar(Cliente cliente, IFormFile imagemUpload)
        {
            var clientes = ClienteRepository.GetAll();
            // Validação de duplicidade
            if (clientes.Any(c => (c.CodigoFiscal == cliente.CodigoFiscal || c.InscricaoEstadual == cliente.InscricaoEstadual) && c.Id != cliente.Id))
            {
                ModelState.AddModelError("CodigoFiscal", "Já existe cliente com este Código Fiscal ou Inscrição Estadual.");
                ViewBag.Cidades = Cidades.Lista;
                return View(cliente);
            }
            // Upload de imagem
            if (imagemUpload != null && imagemUpload.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(imagemUpload.FileName);
                var path = Path.Combine("wwwroot", "uploads", fileName);
                var directory = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    imagemUpload.CopyTo(stream);
                }
                cliente.ImagemPath = "/uploads/" + fileName;
            }
            if (cliente.Id == Guid.Empty) cliente.Id = Guid.NewGuid();
            var idx = clientes.FindIndex(c => c.Id == cliente.Id);
            if (idx >= 0) clientes[idx] = cliente;
            else clientes.Add(cliente);
            ClienteRepository.SaveAll(clientes);
            TempData["Mensagem"] = "Cliente salvo com sucesso!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Excluir(Guid id)
        {
            var clientes = ClienteRepository.GetAll();
            var cliente = clientes.FirstOrDefault(c => c.Id == id);
            if (cliente != null)
            {
                clientes.Remove(cliente);
                ClienteRepository.SaveAll(clientes);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Exportar(Guid id)
        {
            var cliente = ClienteRepository.GetAll().FirstOrDefault(c => c.Id == id);
            if (cliente == null) return NotFound();
            var json = System.Text.Json.JsonSerializer.Serialize(cliente, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            return File(System.Text.Encoding.UTF8.GetBytes(json), "application/json", $"cliente_{cliente.Id}.json");
        }
    }
}
