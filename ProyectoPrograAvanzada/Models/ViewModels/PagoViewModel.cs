using System.ComponentModel.DataAnnotations;

namespace ProyectoPrograAvanzada.Models
{
    public class PagoViewModel
    {
        // Relación con el pedido
        public int PedidoId { get; set; }

        // Monto total calculado en el checkout
        public decimal Monto { get; set; }

        // Método de pago seleccionado (Tarjeta, SINPE, PayPal)
        [Required]
        [Display(Name = "Método de pago")]
        public string Metodo { get; set; }

        // Datos de tarjeta (solo si el método es Tarjeta)
        [Display(Name = "Número de tarjeta")]
        [CreditCard(ErrorMessage = "Número de tarjeta inválido")]
        public string NumeroTarjeta { get; set; }

        [Display(Name = "Nombre en la tarjeta")]
        public string NombreTarjeta { get; set; }

        [Display(Name = "Fecha de expiración (MM/AA)")]
        public string Expiracion { get; set; }

        [Display(Name = "CVV")]
        public string CVV { get; set; }
    }
}