using System;
using System.Collections.Generic;

namespace ProyectoPrograAvanzada.Models;

public partial class Pedido
{
    public int Id { get; set; }

    public int ClienteId { get; set; }

    public int UsuarioId { get; set; }

    public DateTime Fecha { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Impuestos { get; set; }

    public decimal Total { get; set; }

    public string Estado { get; set; } 

    public bool Activo { get; set; }

    public virtual Cliente Cliente { get; set; } 

    public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; } = new List<PedidoDetalle>();

    public virtual Usuario Usuario { get; set; }
}
