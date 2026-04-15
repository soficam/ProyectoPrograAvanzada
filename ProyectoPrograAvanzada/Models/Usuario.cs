using System;
using System.Collections.Generic;

namespace ProyectoPrograAvanzada.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public string ContrasenaHash { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual Cliente? Cliente { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}