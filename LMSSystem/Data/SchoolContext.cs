﻿using LMSSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LMSSystem.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<User_Schedule> User_Schedule { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<User_Class> User_Class { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cấu hình quan hệ giữa các bảng

            // Một User có một Role
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(u => u.Users)
                .HasForeignKey(u => u.RoleID);


            // Một Schedule thuộc về một Class
            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Class)
                .WithMany(s => s.Schedules)
                .HasForeignKey(s => s.ClassID);

            // Một Schedule thuộc về một Course
            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Course)
                .WithMany(s => s.Schedules)
                .HasForeignKey(s => s.CourseID);

            // Một Announcement thuộc về một User
            modelBuilder.Entity<Announcement>()
                .HasOne(a => a.User)
                .WithMany(a => a.Announcements)
                .HasForeignKey(a => a.UserID);

            // Một Lesson thuộc về một Course
            modelBuilder.Entity<Lesson>()
                .HasOne(l => l.Course)
                .WithMany(l => l.Lessons)
                .HasForeignKey(l => l.CourseID);

            // Một Assignment thuộc về một Class
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Class)
                .WithMany(a => a.Assignments)
                .HasForeignKey(a => a.ClassID);

            // Một Exam thuộc về một Course
            modelBuilder.Entity<Exam>()
                .HasOne(e => e.Course)
                .WithMany(e => e.Exams)
                .HasForeignKey(e => e.CourseID);

            // Một Question thuộc về một Exam
            modelBuilder.Entity<Question>()
                .HasOne(q => q.Exam)
                .WithMany(e => e.Questions)
                .HasForeignKey(q => q.ExamID);

            // Quan hệ User - User_Schedule - Schedule
            modelBuilder.Entity<User_Schedule>()
                .HasKey(us => new { us.UserID, us.ScheduleID });

            modelBuilder.Entity<User_Schedule>()
                .HasOne(us => us.User)
                .WithMany(us => us.User_Schedule)
                .HasForeignKey(us => us.UserID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User_Schedule>()
                .HasOne(us => us.Schedule)
                .WithMany(us => us.User_Schedule)
                .HasForeignKey(us => us.ScheduleID)
                .OnDelete(DeleteBehavior.NoAction);



            // Một Message có một User là Sender và một User là Receiver
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverID)
                .OnDelete(DeleteBehavior.NoAction);

            // Quan hệ User - User_Class - Class
            modelBuilder.Entity<User_Class>()
                .HasKey(uc => new { uc.UserID, uc.ClassID });

            modelBuilder.Entity<User_Class>()
                .HasOne(uc => uc.User)
                .WithMany(uc => uc.User_Class)
                .HasForeignKey(uc => uc.UserID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User_Class>()
                .HasOne(uc => uc.Class)
                .WithMany(uc => uc.User_Class)
                .HasForeignKey(uc => uc.ClassID)
                .OnDelete(DeleteBehavior.NoAction);

            // Quan hệ Exam - Exam_User - User
            modelBuilder.Entity<Exam_User>()
                .HasKey(uc => new { uc.UserID, uc.ExamID });

            modelBuilder.Entity<Exam_User>()
                .HasOne(uc => uc.User)
                .WithMany(uc => uc.Exam_User)
                .HasForeignKey(uc => uc.UserID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Exam_User>()
                .HasOne(uc => uc.Exam)
                .WithMany(uc => uc.Exam_User)
                .HasForeignKey(uc => uc.ExamID)
                .OnDelete(DeleteBehavior.NoAction);


            // Một Material thuộc về một Course
            modelBuilder.Entity<Material>()
                .HasOne(m => m.Course)
                .WithMany(m => m.Materials)
                .HasForeignKey(m => m.CourseID);


            // Một Feedback thuộc về một User
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.User)
                .WithMany(f => f.Feedbacks)
                .HasForeignKey(f => f.UserID);


            base.OnModelCreating(modelBuilder);
        }


    }
}