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
                .OnDelete(DeleteBehavior.Cascade);            // spécifie le comportement en cas de delete : ici, un refus



            // StudentCourse.Student (1) <--> User.CoursesForStudent (*)
            modelBuilder.Entity<StudentCourse>()
                .HasOne<User>(c => c.Student)                  // définit la propriété de navigation pour le côté (1) de la relation
                .WithMany(p => p.StudentCourses)            // définit la propriété de navigation pour le côté (N) de la relation
                .OnDelete(DeleteBehavior.Cascade);

            // StudentCourse.Course (1) <--> Course.StudentCourses (*)
            modelBuilder.Entity<StudentCourse>()
                .HasOne<User>(c => c.Student)                  // définit la propriété de navigation pour le côté (1) de la relation
                .WithMany(p => p.StudentCourses)            // définit la propriété de navigation pour le côté (N) de la relation
                .OnDelete(DeleteBehavior.Cascade);

            // Quiz.Course (1) <--> Course.Quizzes (*)
            modelBuilder.Entity<Quiz>()
                .HasOne<Course>(c => c.Course)                  // définit la propriété de navigation pour le côté (1) de la relation
                .WithMany(p => p.Quizzes)            // définit la propriété de navigation pour le côté (N) de la relation
                .OnDelete(DeleteBehavior.Cascade);

            //Question.Course (1) <--> Course.Questions (*)
            modelBuilder.Entity<Question>()
                .HasOne<Course>(c => c.Course)                  // définit la propriété de navigation pour le côté (1) de la relation
                .WithMany(c => c.Questions)            // définit la propriété de navigation pour le côté (N) de la relation
                .OnDelete(DeleteBehavior.Cascade);

            //CategoryQuestion.Question (1) <--> Question.CategoryQuestions (*)
            modelBuilder.Entity<CategoryQuestion>()
                .HasOne<Question>(c => c.Question)                  // définit la propriété de navigation pour le côté (1) de la relation
                .WithMany(p => p.CategoryQuestions)            // définit la propriété de navigation pour le côté (N) de la relation
                .OnDelete(DeleteBehavior.Cascade);

            //CategoryQuestion.Categories (1) <--> Category.CategoryQuestions (*)
            modelBuilder.Entity<CategoryQuestion>()
                .HasOne<Category>(c => c.Category)               // définit la propriété de navigation pour le côté (1) de la relation
                .WithMany(p => p.CategoryQuestions) // définit la propriété de navigation pour le côté (N) de la relation
                .OnDelete(DeleteBehavior.Cascade);

            //CategoryQuestion.Categories (1) <--> Category.CategoryQuestions (*)
            modelBuilder.Entity<CategoryQuestion>()
                .HasOne<Category>(c => c.Category)               // définit la propriété de navigation pour le côté (1) de la relation
                .WithMany(p => p.CategoryQuestions) // définit la propriété de navigation pour le côté (N) de la relation
                .OnDelete(DeleteBehavior.Restrict);
            //Category.Course (1) <--> Course.Categories (*)
            modelBuilder.Entity<Category>()
                .HasOne<Course>(category => category.Course)               // définit la propriété de navigation pour le côté (1) de la relation
                .WithMany(course => course.Categories) // définit la propriété de navigation pour le côté (N) de la relation
                .OnDelete(DeleteBehavior.Cascade);



            // QuestionQuiz.Quiz (1) <--> Quiz.QuestionQuizzs (*)
            modelBuilder.Entity<QuestionQuiz>()
                .HasOne<Quiz>(c => c.Quiz)                  // définit la propriété de navigation pour le côté (1) de la relation
                .WithMany(p => p.QuestionQuizzes)            // définit la propriété de navigation pour le côté (N) de la relation
                .OnDelete(DeleteBehavior.Restrict);

            // QuestionQuiz.Question (1) <--> Question.QuestionQuizzs (*)
            modelBuilder.Entity<QuestionQuiz>()
                .HasOne<Question>(c => c.Question)                  // définit la propriété de navigation pour le côté (1) de la relation
                .WithMany(p => p.QuestionQuizzes)            // définit la propriété de navigation pour le côté (N) de la relation
                .OnDelete(DeleteBehavior.Restrict);


        }

        public void SeedData () {
            Database.BeginTransaction();

            var Boris = new User("Verh", "Boris", "Password1,", "boris@epfc.eu", "unknwon-user.jpg", Role.Teacher);
            var Penelle = new User("Penelle", "Benoit", "Password1,", "benoit@epfc.eu", "", Role.Teacher);
            var Bruno = new User("Lacroix", "Bruno", "Password1,", "bruno@epfc.eu", "unknwon-user.jpg", Role.Teacher);
            var hello = new User("hello", "bonjour", "Bonjour1", "hello@bonjour", "", Role.Student);
            var letitia = new User("T", "Letitia", "Password1", "letitia@epfc", null, Role.Student);
            var victoria = new User("V", "Victoria", "Password1", "hello@epfc", null, Role.Student);
            var Nicolas = new User("J", "Nicolas", "Password1", "hello@epfc", null, Role.Student);
            var Linh = new User("P", "Linh", "Bonjour1", "hello@epfc", null,Role.Student);
           
            Users.AddRange(new[] { Boris, Penelle, Bruno,hello });

            var categories = new List<Category>() {
                new Category("Informatique"),
                new Category("Arithmetique"),
                new Category("Logique"),
                new Category("Mathematique"),
            };

            Categories.AddRange(categories);

            var PRBD = new Course("PRBD", "projet de developp", 6);
            var PRWB = new Course("PRWB","projet web", 5);
            var ANC3 = new Course("ANC3", "Projet de conception", 10);
            var PRM2 = new Course("PRM2", "Projet de conception", 3);
            var PRO2 = new Course("PRO2", "Projet de conception", 15);
            var MATH = new Course("MATH", "Projet de conception", 4);

            PRBD.Teacher = Penelle;
            PRBD.Categories.Add(categories[0]);
            PRWB.Teacher = Penelle;
            ANC3.Teacher = Bruno;
            PRM2.Teacher = Boris;
            PRO2.Teacher = Bruno;
            MATH.Teacher = Boris;
            MATH.Categories.Add(categories[2]);
            MATH.Categories.Add(categories[3]);
            MATH.Categories.Add(categories[1]);
            Courses.AddRange(new[] { PRBD, PRWB,ANC3,PRM2,PRO2,MATH });

            
            var Quiz1 = new Quiz("Quizz1", new DateTime(2021, 5, 10), new DateTime(2021, 5, 30));
            var Quiz2 = new Quiz("Quizz2", new DateTime(2021, 6, 10), new DateTime(2021, 6, 30));
            var Quiz3 = new Quiz("Quizz3", new DateTime(2021, 5, 10), new DateTime(2021, 5, 30));
            var Quiz4 = new Quiz("Quizz1", new DateTime(2021, 5, 10), new DateTime(2021, 5, 30));
            var Quiz5 = new Quiz("Quizz2", new DateTime(2021, 6, 10), new DateTime(2021, 6, 30));
            var Quiz6 = new Quiz("Quizz3", new DateTime(2021, 5, 10), new DateTime(2021, 5, 30));
            var Quiz7 = new Quiz("Quizz1", new DateTime(2021, 5, 10), new DateTime(2021, 5, 30));
            var Quiz8 = new Quiz("Quizz2", new DateTime(2021, 6, 10), new DateTime(2021, 6, 30));
            var Quiz9 = new Quiz("Quizz3", new DateTime(2021, 5, 10), new DateTime(2021, 5, 30));
            var Quiz10 = new Quiz("Quizz1", new DateTime(2021, 5, 10), new DateTime(2021, 5, 30));
            var Quiz11 = new Quiz("Quizz2", new DateTime(2021, 6, 10), new DateTime(2021, 6, 30));
            var Quiz12 = new Quiz("Quizz3", new DateTime(2021, 5, 10), new DateTime(2021, 5, 30));

            Quiz1.Course = PRBD;
            Quiz2.Course = PRBD;
            Quiz3.Course = PRBD;
            Quiz4.Course = MATH;
            Quiz5.Course = MATH;
            Quiz6.Course = MATH;
            Quiz7.Course = PRM2;
            Quiz8.Course = PRM2;
            Quiz9.Course = PRM2;
            Quiz10.Course = PRO2;
            Quiz11.Course = PRO2;
            Quiz12.Course = PRO2;


            Quizzes.AddRange(new[] { Quiz1, Quiz2, Quiz3, Quiz4, Quiz5, Quiz6, Quiz7, Quiz8, Quiz9, Quiz10, Quiz11, Quiz12 });


            

            var studentCourses = new List<StudentCourse>() {
               new StudentCourse() {
                   Student = hello,
                   Course = PRBD,
                   IsValide = true
               },
               new StudentCourse() {
                Student = letitia,
                Course = PRBD,
                IsActif = true
               },
               new StudentCourse() {
                Student = victoria,
                Course = PRWB,
                IsActif = true
               },
               new StudentCourse() {
                Student = Nicolas,
                Course = MATH,
                IsActif = false
               },
               new StudentCourse() {
                Student = Linh,
                Course = PRO2,
                IsActif = false
               }
            };
            


            CategoryQuestions.AddRange(new List<CategoryQuestion>() {
                new CategoryQuestion(){
                    Category = categories[0],
                    Question =  new Question() {
                    Course = PRBD,
                    Type = Type.One,
                    Enonce = "CLR signifie",
                    Propositions = new HashSet<Proposition>() {
                         new Proposition() {
                             Body = "Common Local Runtime",
                             IsCorrect = false,
                         },
                         new Proposition() {
                             Body = "Common Language Runtime",
                             IsCorrect = true,
                         },
                         new Proposition() {
                             Body = "Common Language Realtime",
                             IsCorrect = false,
                         },
                         new Proposition() {
                             Body = "Common Local Realtime",
                             IsCorrect = false,
                         }
                    }
                }
                },
                new CategoryQuestion() {
                    Category =  new Category("Arithmetique"),
                    Question =  new Question() {
                    Course = MATH,
                    Type = Type.One,
                    Enonce = "Quel est la réponse de l'équation x+2 = 0",
                    Propositions = new HashSet<Proposition>() {
                         new Proposition() {
                             Body = "0",
                             IsCorrect = false,
                         },
                         new Proposition() {
                             Body = "x+2",
                             IsCorrect = false,
                         },
                         new Proposition() {
                             Body = "2",
                             IsCorrect = false,
                         },
                         new Proposition() {
                             Body = "-2",
                             IsCorrect = true,
                         }
                    }
                },
                }, new CategoryQuestion() {
                    Category =  categories[2],
                    Question =  new Question() {
                    Course = MATH,
                    Type = Type.One,
                    Enonce = "Quel est la réponse de l'équation x+2 = 1",
                    Propositions = new HashSet<Proposition>() {
                         new Proposition() {
                             Body = "3",
                             IsCorrect = false,
                         },
                         new Proposition() {
                             Body = "-1",
                             IsCorrect = true,
                         }

                    }
                }
                },
                new CategoryQuestion() {
                    Category =    categories[3],
                    Question = new Question() {
                    Course = MATH,
                    Type = Type.Many,
                    Enonce = "Que signifie a/b",
                    Propositions = new HashSet<Proposition>() {
                         new Proposition() {
                             Body = "a divise b",
                             IsCorrect = true,
                         },
                         new Proposition() {
                             Body = "b est un multiple de a",
                             IsCorrect = true,
                         },
                         new Proposition() {
                             Body = "a est un multiple de b",
                             IsCorrect = false,
                         },
                         new Proposition() {
                             Body = "b divise a",
                             IsCorrect = false,
                         },

                    }
                }
                }
            });


            StudentCourses.AddRange(studentCourses);

            var Question1 = new Question("le label est-il editable", Type.One);
            var Question2 = new Question("PRWB",Type.Many);
            var Question3 = new Question("Que signifie mvvm", Type.One);
            var Question4 = new Question("PRM2", Type.Many);
            var Question5 = new Question("PRO2", Type.One);
            var Question6 = new Question("MATH", Type.Many);
            var Question7 = new Question("le label est-il editable", Type.One);
            var Question8 = new Question("Choisir une proposition ", Type.One);
            var Question9 = new Question("Choisir une", Type.Many);
            var Question10 = new Question("PRM2", Type.One);
            var Question11 = new Question("PRO2", Type.Many);
            var Question12 = new Question("MATH", Type.One);


            Question1.Course = ANC3;
            Question2.Course = ANC3;
            Question3.Course = ANC3;
            Question4.Course = PRM2;
            Question5.Course = PRM2;
            Question6.Course = PRO2;
            Question7.Course = PRM2;
            Question8.Course = PRO2;
            Question9.Course = PRO2;
            Question10.Course = PRBD;
            Question11.Course = PRBD;
            Question12.Course = PRBD;


            Questions.AddRange(new[] {
                Question1, Question2,Question3, Question4, 
                Question5, Question6,Question7,Question8,
                Question9, Question10,Question11,Question12 
            });
            var CategoryQuestion1 = new Proposition();
            var CategoryQuestion2 = new Proposition();
            var CategoryQuestion3 = new Proposition();
            var CategoryQuestion4 = new Proposition();
            var CategoryQuestion5 = new Proposition();
            var CategoryQuestion6 = new Proposition();
            var CategoryQuestion7 = new Proposition();


            CategoryQuestion1.Body = "hello test";
            CategoryQuestion2.Body = "hello test1";
            CategoryQuestion3.Body = "hello test2";
            CategoryQuestion4.Body = "hello test3";
            CategoryQuestion5.Body = "hello test3";
            CategoryQuestion6.Body = "hello test4";
            CategoryQuestion7.Body = "hello testà";

            CategoryQuestion1.IsCorrect = true;
            CategoryQuestion2.IsCorrect = false;
            CategoryQuestion5.IsCorrect = false;
            CategoryQuestion6.IsCorrect = true;
            CategoryQuestion7.IsCorrect = false;
            CategoryQuestion3.IsCorrect = true;

            CategoryQuestion1.Question = Question10;
            CategoryQuestion2.Question = Question10;
            CategoryQuestion3.Question = Question11;
            CategoryQuestion5.Question = Question11;
            CategoryQuestion6.Question = Question11;
            CategoryQuestion7.Question = Question11;
            CategoryQuestion4.Question = Question12;

            Propositions.AddRange(new[] {
                CategoryQuestion1,CategoryQuestion2,CategoryQuestion3,CategoryQuestion4,
                CategoryQuestion5,CategoryQuestion6,CategoryQuestion7
            });
            /*var CategoryQuestion1 = new CategoryQuestion();
            var CategoryQuestion2 = new CategoryQuestion();
            var CategoryQuestion3 = new CategoryQuestion();
            var CategoryQuestion4 = new CategoryQuestion();*/


            var QuestionQuiz1 = new QuestionQuiz();
            var QuestionQuiz2 = new QuestionQuiz();
            var QuestionQuiz3 = new QuestionQuiz();



            QuestionQuiz1.Question = Question10;
            QuestionQuiz2.Question = Question11;
            QuestionQuiz3.Question = Question12;
            QuestionQuiz1.Quiz = Quiz1;
            QuestionQuiz2.Quiz = Quiz1;
            QuestionQuiz3.Quiz = Quiz1;
            QuestionQuiz3.Point = 4;
            QuestionQuiz2.Point = 5;
            QuestionQuiz1.Point = 1;


            QuestionQuizzes.AddRange(new[] {QuestionQuiz1, QuestionQuiz2, QuestionQuiz3 });

            //StudentCourses.AddRange(new[] {studentCourse,studentCourseL,studentCourseN, studentCourseLi,studentCourseV });
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
            string Password,
            string Picturepath
        ) {
            Database.BeginTransaction();
            var user = new User(FirstName,LastName,Password,Email,Picturepath);
            Users.AddRange(user);
            SaveChanges();
            Database.CommitTransaction();
            return user;
        }
        public Course CreateCourse()
        {
            Database.BeginTransaction();
            var course = new Course();
            course.Teacher = App.CurrentUser;
            //Courses.AddRange(course);
            //SaveChanges();
            Database.CommitTransaction();
            return course;
        }
        public Quiz CreateQuizz()
        {
            Database.BeginTransaction();
            var quizz = new Quiz();
            //quizz = App.CurrentUser.
            //Courses.AddRange(course);
            //SaveChanges();
            Database.CommitTransaction();
            return quizz;
        }
        public Question CreateQuestion(Course course)
        {
            Database.BeginTransaction();
            var question = new Question();
            question.Enonce = "";
            question.Type = Type.One;
            question.Course = course;
            Questions.AddRange(question);
            //SaveChanges();
            Database.CommitTransaction();
            return question;
        }
    }
}
