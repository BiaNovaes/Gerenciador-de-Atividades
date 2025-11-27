using Microsoft.EntityFrameworkCore;
using GT.Domain.Entites;
using GT.Domain.Enums;
 
namespace GT.Infra.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
 
        public DbSet<User> Users { get; set; }
        public DbSet<Tarefa> Tarefas { get; set; }
 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
 
           
            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);
 
            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(100);
 
            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);
 
            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired();
 
            modelBuilder.Entity<User>()
                .HasMany(u => u.Tarefas)
                .WithOne(t => t.Usuario)
                .HasForeignKey(t => t.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
 
           
            modelBuilder.Entity<Tarefa>()
                .HasKey(t => t.Id);
 
            modelBuilder.Entity<Tarefa>()
                .Property(t => t.Titulo)
                .IsRequired()
                .HasMaxLength(100);
 
            modelBuilder.Entity<Tarefa>()
                .Property(t => t.Descricao)
                .HasMaxLength(500);
 
            modelBuilder.Entity<Tarefa>()
                .Property(t => t.Status)
                .HasDefaultValue(TarefaEnum.Pendente);
        }
    }
}