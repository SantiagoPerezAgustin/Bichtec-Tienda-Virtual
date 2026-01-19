using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class CarritoWhatsappResumenDTO
    {
        public string UsuarioNombre { get; set; }
        public string UsuarioEmail { get; set; }

        public List<CarritoWhatsappItemDTO> Items { get; set; } = new();

        public decimal Total { get; set; }
    }
}
