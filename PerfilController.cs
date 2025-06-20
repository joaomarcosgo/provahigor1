using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProvaHigorr.Data;
using ProvaHigorr.Models;
using ProvaHigorr.Filters;

namespace ProvaHigorr.Controllers
{
    [AutorizacaoSimples]
    public class PerfilController : Controller
    {
        public IActionResult Editar()
        {
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Usuario");
            var usuario = UsuarioRepository.GetAll().FirstOrDefault(u => u.Id.ToString() == usuarioId);
            if (usuario == null) return RedirectToAction("Login", "Usuario");
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Editar(string nome, string senha)
        {
            var usuarioId = HttpContext.Session.GetString("UsuarioId");
            if (usuarioId == null) return RedirectToAction("Login", "Usuario");
            var usuarios = UsuarioRepository.GetAll();
            var usuario = usuarios.FirstOrDefault(u => u.Id.ToString() == usuarioId);
            if (usuario == null) return RedirectToAction("Login", "Usuario");
            usuario.Nome = nome;
            if (!string.IsNullOrWhiteSpace(senha))
            {
                usuario.SenhaHash = UsuarioController.HashSenhaStatic(senha);
            }
            UsuarioRepository.SaveAll(usuarios);
            HttpContext.Session.SetString("UsuarioNome", usuario.Nome);
            TempData["Mensagem"] = "Dados atualizados com sucesso!";
            return RedirectToAction("Editar");
        }
    }
}
