namespace ProyectoPrograAvanzada.Models.ViewModels
{
    public class CarritoViewModel
    {
        public List<CarritoItemViewModel> Items { get; set; } = new();

        public decimal Subtotal => Items.Sum(x => x.Subtotal);
        public decimal Impuestos => Items.Sum(x => x.ImpuestoMonto);
        public decimal Total => Items.Sum(x => x.Total);
        public int CantidadProductos => Items.Sum(x => x.Cantidad);
    }
}