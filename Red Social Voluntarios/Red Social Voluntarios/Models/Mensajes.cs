using Red_Social_Voluntarios.Models;

public class Mensaje
{
    public int Id { get; set; }

    public string EmisorId { get; set; } = string.Empty;
    public Usuario? Emisor { get; set; }

    public string ReceptorId { get; set; } = string.Empty;
    public Usuario? Receptor { get; set; }

    public string Contenido { get; set; } = string.Empty;

    public DateTime FechaEnvio { get; set; } = DateTime.Now;

    public bool Leido { get; set; } = false;
}
