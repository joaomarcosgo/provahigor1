using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ProvaHigorr.Models;

namespace ProvaHigorr.Data
{
    public static class UsuarioRepository
    {
        private static readonly string FilePath = Path.Combine("Data", "usuarios.json");

        public static List<Usuario> GetAll()
        {
            if (!File.Exists(FilePath)) return new List<Usuario>();
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Usuario>>(json) ?? new List<Usuario>();
        }

        public static void SaveAll(List<Usuario> usuarios)
        {
            var json = JsonSerializer.Serialize(usuarios, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
