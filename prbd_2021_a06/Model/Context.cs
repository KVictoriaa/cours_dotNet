using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRBD_Framework;

namespace prbd_2021_a06.Model {
    public class Context : DbContextBase {
        public static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => {
            builder.AddConsole();
        });

        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=prbd-2021-a06")
                .EnableSensitiveDataLogging().UseLazyLoadingProxies(true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            // Course.Teacher (1) <--> User.CoursesForTeacher (*)
            modelBuilder.Entity<Course>()
                .HasOne<User>(c => c.Teacher)                  // définit la propriété de navigation pour le côté (1) de la relation
                .WithMany(p => p.CoursesForTeacher)            // définit la propriété de navigation pour le côté (N) de la relation
                .OnDelete(DeleteBehavior.Restrict);            // spécifie le comportement en cas de delete : ici, un refus



            // StudentCourse.Student (1) <--> User.CoursesForStudent (*)
            modelBuilder.Entity<StudentCourse>()
                .HasOne<User>(c => c.Student)                  // définit la propriété de navigation pour le côté (1) de la relation
                .WithMany(p => p.StudentCourses)            // définit la propriété de navigation pour le côté (N) de la relation
                .OnDelete(DeleteBehavior.Restrict);

            // StudentCourse.Course (1) <--> Course.StudentCourses (*)
            modelBuilder.Entity<StudentCourse>()
                .HasOne<User>(c => c.Student)                  // définit la propriété de navigation pour le côté (1) de la relation
                .WithMany(p => p.StudentCourses)            // définit la propriété de navigation pour le côté (N) de la relation
                .OnDelete(DeleteBehavior.Restrict);

            // Quiz.Course (1) <--> Course.Quizzes (*)
            modelBuilder.Entity<Quiz>()
                .HasOne<Course>(c => c.Course)                  // définit la propriété de navigation pour le côté (1) de la relation
                .WithMany(p => p.Quizzes)            // définit la propriété de navigation pour le côté (N) de la relation
                .OnDelete(DeleteBehavior.Restrict);

            //Question.Course (1) <--> Course.Questions (*)
            modelBuilder.Entity<Quiz>()
                .HasOne<Course>(c => c.Course)                  // définit la propriété de navigation pour le côté (1) de la relation
                .WithMany(p => p.Quizzes)            // définit la propriété de navigation pour le côté (N) de la relation
                .OnDelete(DeleteBehavior.Restrict);

            //CategoryQuestion.Question (1) <--> Question.CategoryQuestions (*)
            modelBuilder.Entity<CategoryQuestion>()
                .HasOne<Question>(c => c.Questions)                  // définit la propriété de navigation pour le côté (1) de la relation
                .WithMany(p => p.CategoryQuestions)            // définit la propriété de navigation pour le côté (N) de la relation
                .OnDelete(DeleteBehavior.Restrict);

            //CategoryQuestion.Categories (1) <--> Category.CategoryQuestions (*)
            modelBuilder.Entity<CategoryQuestion>()
                .HasOne<Category>(c => c.Categories)               // définit la propriété de navigation pour le côté (1) de la relation
                .WithMany(p => p.CategoryQuestions) // définit la propriété de navigation pour le côté (N) de la relation
                .OnDelete(DeleteBehavior.Restrict);

            //CategoryQuestion.Categories (1) <--> Category.CategoryQuestions (*)
            modelBuilder.Entity<CategoryQuestion>()
                .HasOne<Category>(c => c.Categories)               // définit la propriété de navigation pour le côté (1) de la relation
                .WithMany(p => p.CategoryQuestions) // définit la propriété de navigation pour le côté (N) de la relation
                .OnDelete(DeleteBehavior.Restrict);



        }

        public void SeedData () {
            Database.BeginTransaction();
            var Boris = new User("Verh", "Boris", "Password1,", "boris@epfc.eu", Role.Teacher);
            var Penelle = new User("Penelle", "Benoit", "Password1,", "benoit@epfc.eu", Role.Teacher);
            var Bruno = new User("Lacroix", "Bruno", "Password1,", "bruno@epfc.eu", Role.Teacher);
            Users.AddRange(new[] { Boris, Penelle, Bruno });
            SaveChanges();
            Database.CommitTransaction();
        }

        public DbSet<User> Users { get; set; } 
        public DbSet<Question> Questions { get; set; }
        public DbSet<Proposition> Propositions { get; set; }
      
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryQuestion> CategoryQuestions { get; set; }
        public DbSet<AnswerQuestions> AnswerQuestions { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<QuestionQuiz> QuestionQuizzes { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }

        public User CreateUser(
            string Email,
            string FirstName, 
            string LastName, 
            string Password
        ) {
            Database.BeginTransaction();
            var user = new User(FirstName,LastName,Password,Email);
            Console.WriteLine(user);
            Users.AddRange(user);
            SaveChanges();
            Database.CommitTransaction();
            return user;
        }
    }
}
