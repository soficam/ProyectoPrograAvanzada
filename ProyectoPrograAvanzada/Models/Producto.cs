using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoPrograAvanzada.Models;

public partial class Producto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string Nombre { get; set; } = null!;


    [Required(ErrorMessage = "La categoría es obligatoria")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría válida")]
    public int? CategoriaId { get; set; }


    [Required(ErrorMessage = "El precio es obligatorio")]
    [Range(0.01, 999999.99, ErrorMessage = "El precio debe ser mayor que 0")]
    public decimal? Precio { get; set; }


    [Required(ErrorMessage = "El impuesto es obligatorio")]
    [Range(0, 100, ErrorMessage = "El impuesto debe estar entre 0 y 100")]
    public decimal? ImpuestoPorc { get; set; }


    [Required(ErrorMessage = "El stock es obligatorio")]
    [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
    public int? Stock { get; set; }

    //[Required(ErrorMessage = "La imagen es obligatoria")]
    public string? ImagenUrl { get; set; }

    public bool Activo { get; set; }

    [ValidateNever]
    public virtual Categorium Categoria { get; set; } = null!;

    public virtual ICollection<PedidoDetalle> PedidoDetalles { get; set; } = new List<PedidoDetalle>();
}
