namespace ProyectoPrograAvanzada.Models
{
    public class Pago
    {
            public int Id { get; set; }
            public int PedidoId { get; set; }
            public string Metodo { get; set; }
            public decimal Monto { get; set; }
            public DateTime Fecha { get; set; } = DateTime.Now;
            public string Estado { get; set; }
            public string Referencia { get; set; }

            public Pedido Pedido { get; set; }
        }
    }


