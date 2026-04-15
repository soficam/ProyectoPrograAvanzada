using System.Text.Json;
using ProyectoPrograAvanzada.Models.ViewModels;

namespace ProyectoPrograAvanzada.Helpers
{
    public static class CarritoSessionHelper
    {
        private const string SessionKey = "Carrito";

        public static CarritoViewModel ObtenerCarrito(ISession session)
        {
            var json = session.GetString(SessionKey);

            if (string.IsNullOrWhiteSpace(json))
            {
                return new CarritoViewModel();
            }

            var carrito = JsonSerializer.Deserialize<CarritoViewModel>(json);

            return carrito ?? new CarritoViewModel();
        }

        public static void GuardarCarrito(ISession session, CarritoViewModel carrito)
        {
            var json = JsonSerializer.Serialize(carrito);
            session.SetString(SessionKey, json);
        }

        public static void LimpiarCarrito(ISession session)
        {
            session.Remove(SessionKey);
        }
    }
}