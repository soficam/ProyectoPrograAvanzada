using System;
using System.Collections.Generic;

namespace ProyectoPrograAvanzada.Models;

public partial class PedidoDetalle
{
    public int Id { get; set; }

    public int PedidoId { get; set; }

    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnit { get; set; }

    public decimal? Descuento { get; set; }

    public decimal ImpuestoPorc { get; set; }

    public decimal TotalLinea { get; set; }

    public bool Activo { get; set; }

    public virtual Pedido Pedido { get; set; } = null!;

    public virtual Producto Producto { get; set; } = null!;
}
