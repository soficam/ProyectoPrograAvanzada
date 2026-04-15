using System.ComponentModel.DataAnnotations;

namespace ProyectoPrograAvanzada.Models.ViewModels
{
    public class PedidoCreateViewModel
    {
        [Required]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        [Required]
        [Display(Name = "Usuario")]
        public int UsuarioId { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Pendiente";

        public List<PedidoDetalleCreateItemViewModel> Detalles { get; set; } = new();
    }

    public class PedidoDetalleCreateItemViewModel
    {
        [Required]
        [Display(Name = "Producto")]
        public int ProductoId { get; set; }

        [Required]
        [Range(1, 9999)]
        public int Cantidad { get; set; }

        [Range(0, 999999)]
        public decimal? Descuento { get; set; }
    }
}