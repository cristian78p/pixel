using Microsoft.EntityFrameworkCore;

namespace Red_Social_Voluntarios.Data
{
    public class ContextoDB : DbContext
    {
        public ContextoDB(DbContextOptions opciones) : base(opciones)
        { 
        }
    }
}
