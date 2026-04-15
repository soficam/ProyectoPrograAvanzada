namespace ProyectoPrograAvanzada.Models.ViewModels
{
    public class CarritoItemViewModel
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? ImagenUrl { get; set; }
        public decimal Precio { get; set; }
        public decimal ImpuestoPorc { get; set; }
        public int Cantidad { get; set; }
        public int StockDisponible { get; set; }

        public decimal Subtotal => Precio * Cantidad;
        public decimal ImpuestoMonto => Subtotal * (ImpuestoPorc / 100m);
        public decimal Total => Subtotal + ImpuestoMonto;
    }
}