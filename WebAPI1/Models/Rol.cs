namespace WebAPI1.Models
{
    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool isDeleted { get; set; } = false;

        public List<User> Users { get; set; } = new List<User>();
    }
}
