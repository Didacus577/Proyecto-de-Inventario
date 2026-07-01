namespace Inventario.AppWeb.Utilidades.Response
{
    public class GenericResponse<T>
    {
        public bool Estado { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public T? Objeto { get; set; }

        public string Url { get; set; } = string.Empty;
        public string Token { get; internal set; }
    }
}
