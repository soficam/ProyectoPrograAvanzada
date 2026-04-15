namespace Proyecto_Gestion_Proyecto.Services
{
    public class PagoService
    {
        private readonly string _connectionString;

        public PagoService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void ProcesarPago(int pedidoId, decimal monto, string metodo)
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

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO Pago (PedidoId, Metodo, Monto, Estado, Referencia)
                               VALUES (@PedidoId, @Metodo, @Monto, @Estado, @Referencia)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@PedidoId", pedidoId);
                    cmd.Parameters.AddWithValue("@Metodo", metodo);
                    cmd.Parameters.AddWithValue("@Monto", monto);
                    cmd.Parameters.AddWithValue("@Estado", estado);
                    cmd.Parameters.AddWithValue("@Referencia", referencia);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
