using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ProvaHigorr.Data;
using ProvaHigorr.Models;

namespace ProvaHigorr.Controllers
{
    public class UsuarioController : Controller
    {
        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(string nome, string login, string senha)
        {
            var usuarios = UsuarioRepository.GetAll();
            if (usuarios.Any(u => u.Login == login))
            {
                ModelState.AddModelError("Login", "Login já existe.");
                return View();
            }
            var senhaHash = HashSenha(senha);
            usuarios.Add(new Usuario { Nome = nome, Login = login, SenhaHash = senhaHash });
            UsuarioRepository.SaveAll(usuarios);
            TempData["Mensagem"] = "Usuário cadastrado com sucesso!";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string login, string senha)
        {
            var usuarios = UsuarioRepository.GetAll();
            var usuario = usuarios.FirstOrDefault(u => u.Login == login);
            if (usuario == null || usuario.SenhaHash != HashSenha(senha))
            {
                ModelState.AddModelError("Login", "Login ou senha inválidos.");
                return View();
            }
            // Autenticação simples via sessão
            HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
            HttpContext.Session.SetString("UsuarioNome", usuario.Nome);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        private static string HashSenha(string senha)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
            return Convert.ToBase64String(bytes);
        }

        public static string HashSenhaStatic(string senha) => HashSenha(senha);
    }
}
