using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using ProvaHigorr.Models;

namespace ProvaHigorr.Data
{
    public static class ClienteRepository
    {
        private static readonly string FilePath = Path.Combine("Data", "clientes.json");

        public static List<Cliente> GetAll()
        {
            if (!File.Exists(FilePath)) return new List<Cliente>();
            var json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<List<Cliente>>(json) ?? new List<Cliente>();
        }

        public static void SaveAll(List<Cliente> clientes)
        {
            var json = JsonSerializer.Serialize(clientes, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }
    }
}
