using System;
using ProyectoPrograAvanzada.Models;

namespace ProyectoPrograAvanzada.Services
{
    public class PagoService
    {
        private readonly AppDbContext _context;

        public PagoService(AppDbContext context)
        {
            _context = context;
        }

        // 🔑 Ahora devuelve un objeto Pago
        public Pago ProcesarPago(int pedidoId, decimal monto, string metodo)
        {
            string estado = "Pendiente";
            string referencia = Guid.NewGuid().ToString();

            switch (metodo.ToLower())
            {
                case "tarjeta":
                    estado = "Aprobado";
                    referencia = "TARJ-" + referencia.Substring(0, 8);
                    break;
                case "sinpe":
                    estado = "Aprobado";
                    referencia = "SINPE-" + DateTime.Now.Ticks;
                    break;
                case "paypal":
                    estado = "Aprobado";
                    referencia = "PAYPAL-" + referencia.Substring(0, 8);
                    break;
                default:
                    estado = "Rechazado";
                    break;
            }

            var pago = new Pago
            {
                PedidoId = pedidoId,
                Metodo = metodo,
                Monto = monto,
                Estado = estado,
                Referencia = referencia,
                Fecha = DateTime.Now
            };

          //_context.Pagos.Add(pago);
           //context.SaveChanges();

            return pago; // 🔑 devuelve el objeto
        }
    }
}