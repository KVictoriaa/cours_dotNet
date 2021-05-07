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
            var hello = new User("hello", "bonjour", "Bonjour1", "hello@bonjour", Role.Student);
            var letitia = new User("T", "Letitia", "Password1", "letitia@epfc", Role.Student);
            var victoria = new User("V", "Victoria", "Password1", "hello@epfc", Role.Student);
            var Nicolas = new User("J", "Nicolas", "Password1", "hello@epfc", Role.Student);
            var Linh = new User("P", "Linh", "Bonjour1", "hello@epfc", Role.Student);
           
            Users.AddRange(new[] { Boris, Penelle, Bruno,hello });

            var PRBD = new Course("PRBD", "projet de developp", 6);
            var PRWB = new Course("PRWB","projet web", 5);
            var ANC3 = new Course("ANC3", "Projet de conception", 10);
            var PRM2 = new Course("PRM2", "Projet de conception", 3);
            var PRO2 = new Course("PRO2", "Projet de conception", 15);
            var MATH = new Course("MATH", "Projet de conception", 4);
            Courses.AddRange(new[] { PRBD, PRWB,ANC3,PRM2,PRO2,MATH });
            PRBD.Teacher = Penelle;
            PRWB.Teacher = Penelle;
            ANC3.Teacher = Bruno;
            PRM2.Teacher = Boris;
            PRO2.Teacher = Bruno;
            MATH.Teacher = Boris;

            var studentCourse = new StudentCourse();
            var studentCourseL = new StudentCourse();
            var studentCourseV = new StudentCourse();
            var studentCourseN = new StudentCourse();
            var studentCourseLi = new StudentCourse();

            studentCourse.Student = hello;
            studentCourse.Course = PRBD;
            studentCourse.IsValide = true;

            studentCourseL.Student = letitia;
            studentCourseL.Course = PRBD;
            studentCourseL.IsActif = true;

            studentCourseV.Student = victoria;
            studentCourseV.Course = PRWB;
            studentCourseV.IsActif = true;

            studentCourseN.Student = Nicolas;
            studentCourseN.Course = MATH;
            studentCourseN.IsActif = false;

            studentCourseLi.Student = Linh;
            studentCourseLi.Course = PRO2;
            studentCourseLi.IsValide = false;

            StudentCourses.AddRange(new[] {studentCourse,studentCourseL,studentCourseN, studentCourseLi,studentCourseV });
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
            Users.AddRange(user);
            SaveChanges();
            Database.CommitTransaction();
            return user;
        }
        public Course CreateCourse(
            string Title,
            string Description,
            int MaxStudent
            )
        {
            Database.BeginTransaction();
            var course = new Course(Title, Description, MaxStudent
                );
            course.Teacher = App.CurrentUser;
            Courses.AddRange(course);
            SaveChanges();
            Database.CommitTransaction();
            return course;
        }
    }
}
