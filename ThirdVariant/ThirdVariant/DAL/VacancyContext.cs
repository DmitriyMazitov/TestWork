using Microsoft.EntityFrameworkCore;
using ThirdVariant.Models;

namespace ThirdVariant.DAL
{
    public class VacancyContext : DbContext
    {
        //public VacancyContext(DbContextOptions<VacancyContext> options) : base(options) { }
        // When used with ASP.net core, add these lines to Startup.cs
        //   var connectionString = Configuration.GetConnectionString("BlogContext");
        //   services.AddEntityFrameworkNpgsql().AddDbContext<BlogContext>(options => options.UseNpgsql(connectionString));
        // and add this to appSettings.json
        // "ConnectionStrings": { "BlogContext": "Server=localhost;Database=blog" }

        public DbSet<Vacancy> Vacancies { get; set; }
        //public DbSet<Course> Courses { get; set; }
        //public DbSet<Department> Departments { get; set; }
        //public DbSet<Enrollment> Enrollments { get; set; }
        //public DbSet<Instructor> Instructors { get; set; }
        //public DbSet<Student> Students { get; set; }
        //public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        //public DbSet<Person> People { get; set; }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        //    modelBuilder.Entity<Course>()
        //        .HasMany(c => c.Instructors).WithMany(i => i.Courses)
        //        .Map(t => t.MapLeftKey("CourseID")
        //            .MapRightKey("PersonID")
        //            .ToTable("CourseInstructor"));
        //}

        //Configure default schema
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Vars.pgConnString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("vacancy");

            //modelBuilder.Entity<VacancyKeySkill>(entity =>
            //{
            //    entity.HasOne(d => d.VacancyEntity)
            //        .WithMany(p => p.VacancyKeySkill)
            //        .HasForeignKey(d => d.VacancyId);
            //});
        }
    }
}
