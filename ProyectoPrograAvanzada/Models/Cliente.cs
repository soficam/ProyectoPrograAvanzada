using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoPrograAvanzada.Models;

public partial class Cliente
{
    
    public int Id { get; set; }
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "La cédula es obligatoria")]
    [RegularExpression(@"^\d{9}$", ErrorMessage = "La cédula debe tener exactamente 9 dígitos")]
    public string Cedula { get; set; } = null!;

    [Required(ErrorMessage = "El correo es obligatorio")]
    [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "El correo debe tener un formato válido")]

    public string Correo { get; set; } = null!;

    [RegularExpression(@"^\d{8,15}$", ErrorMessage = "El teléfono debe contener solo números (8 a 15 dígitos)")]
    public string? Telefono { get; set; }

    [StringLength(200, ErrorMessage = "La dirección no puede superar los 200 caracteres")]
    public string? Direccion { get; set; }

    public bool Activo { get; set; }

    public int? UsuarioId { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public virtual Usuario? Usuario { get; set; }
}
