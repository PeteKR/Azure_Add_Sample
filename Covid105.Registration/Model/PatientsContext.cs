using Microsoft.EntityFrameworkCore;

namespace Covid105.Registration.Model
{
    public class PatientsContext : DbContext
    {
        public PatientsContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Patient> Patients { get; set; }
    }
}
