using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Web_Triolingo.Model
{
    public partial class TriolingoDBContext : DbContext
    {
        public TriolingoDBContext()
        {
        }

        public TriolingoDBContext(DbContextOptions<TriolingoDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; } = null!;
        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<Lesson> Lessons { get; set; } = null!;
        public virtual DbSet<Question> Questions { get; set; } = null!;
        public virtual DbSet<Setting> Settings { get; set; } = null!;
        public virtual DbSet<StudentCourse> StudentCourses { get; set; } = null!;
        public virtual DbSet<StudentLesson> StudentLessons { get; set; } = null!;
        public virtual DbSet<Unit> Units { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var conStr = config.GetConnectionString("TriolingoConStr");
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(conStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("Answer");

                entity.Property(e => e.Answer1).HasColumnName("answer");

                entity.Property(e => e.IsCorrect).HasColumnName("isCorrect");

                entity.Property(e => e.QuestionId).HasColumnName("questionId");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Answer__question__5BE2A6F2");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.ToTable("Lesson");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Unit)
                    .WithMany(p => p.Lessons)
                    .HasForeignKey(d => d.UnitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Lesson_Unit");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question");

                entity.Property(e => e.File).HasColumnName("file");

                entity.Property(e => e.LessonId).HasColumnName("lessonId");

                entity.Property(e => e.Mark)
                    .HasColumnName("mark")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Question1)
                    .HasMaxLength(150)
                    .HasColumnName("Question");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TypeId).HasColumnName("typeId");

                entity.HasOne(d => d.Lesson)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.LessonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Question__lesson__571DF1D5");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Question__typeId__5629CD9C");
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.ToTable("Setting");

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.Value).HasMaxLength(250);

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Setting_Setting1");
            });

            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity.ToTable("StudentCourse");

                entity.Property(e => e.Comment).HasMaxLength(250);

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.StudentCourses)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentCourse_Course");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentCourses)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentCourse_User");
            });

            modelBuilder.Entity<StudentLesson>(entity =>
            {
                entity.ToTable("StudentLesson");

                entity.HasOne(d => d.Lession)
                    .WithMany(p => p.StudentLessons)
                    .HasForeignKey(d => d.LessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentLesson_Lesson");

                entity.HasOne(d => d.StudentCourse)
                    .WithMany(p => p.StudentLessons)
                    .HasForeignKey(d => d.StudentCourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentLesson_StudentCourse");
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.ToTable("Unit");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Units)
                    .HasForeignKey(d => d.CourseId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Unit_Course");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.AvatarUrl).HasColumnName("AvatarURL");

                entity.Property(e => e.Email).HasMaxLength(150);

                entity.Property(e => e.FullName).HasMaxLength(150);

                entity.Property(e => e.Password).HasMaxLength(150);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRole");

                entity.HasOne(d => d.RoleTypeNavigation)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.RoleType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_Setting");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
