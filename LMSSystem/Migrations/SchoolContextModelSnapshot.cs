﻿// <auto-generated />
using System;
using LMSSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LMSSystem.Migrations
{
    [DbContext(typeof(SchoolContext))]
    partial class SchoolContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("LMSSystem.Models.Announcement", b =>
                {
                    b.Property<int>("AnnouncementID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AnnouncementID"), 1L, 1);

                    b.Property<string>("AnnouncementContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("AnnouncementDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("AnnouncementID");

                    b.HasIndex("UserID");

                    b.ToTable("Announcements");
                });

            modelBuilder.Entity("LMSSystem.Models.Answer", b =>
                {
                    b.Property<int>("AnswerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AnswerID"), 1L, 1);

                    b.Property<string>("AnswerContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("bit");

                    b.Property<int>("QuestionID")
                        .HasColumnType("int");

                    b.HasKey("AnswerID");

                    b.HasIndex("QuestionID");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("LMSSystem.Models.Assignment", b =>
                {
                    b.Property<int>("AssignmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AssignmentID"), 1L, 1);

                    b.Property<string>("AssignmentFile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssignmentTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ClassID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("datetime2");

                    b.HasKey("AssignmentID");

                    b.HasIndex("ClassID");

                    b.ToTable("Assignments");
                });

            modelBuilder.Entity("LMSSystem.Models.Class", b =>
                {
                    b.Property<int>("ClassID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClassID"), 1L, 1);

                    b.Property<int?>("AssignmentID")
                        .HasColumnType("int");

                    b.Property<string>("ClassName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ClassID");

                    b.HasIndex("AssignmentID");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("LMSSystem.Models.Course", b =>
                {
                    b.Property<int>("CourseID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseID"), 1L, 1);

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CourseID");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("LMSSystem.Models.Exam", b =>
                {
                    b.Property<int>("ExamID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExamID"), 1L, 1);

                    b.Property<int>("CourseID")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time");

                    b.Property<string>("ExamTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExamType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ExamID");

                    b.HasIndex("CourseID");

                    b.ToTable("Exams");
                });

            modelBuilder.Entity("LMSSystem.Models.Exam_User", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("ExamID")
                        .HasColumnType("int");

                    b.Property<DateTime>("ExamDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("GradedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Remark")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Score")
                        .HasColumnType("float");

                    b.HasKey("UserID", "ExamID");

                    b.HasIndex("ExamID");

                    b.ToTable("Exam_User");
                });

            modelBuilder.Entity("LMSSystem.Models.Feedback", b =>
                {
                    b.Property<int>("FeedbackID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FeedbackID"), 1L, 1);

                    b.Property<string>("FeedbackContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FeedbackDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("FeedbackID");

                    b.HasIndex("UserID");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("LMSSystem.Models.Lesson", b =>
                {
                    b.Property<int>("LessonID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LessonID"), 1L, 1);

                    b.Property<string>("ApprovalStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CourseID")
                        .HasColumnType("int");

                    b.Property<string>("LessonContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LessonTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LessonID");

                    b.HasIndex("CourseID");

                    b.ToTable("Lessons");
                });

            modelBuilder.Entity("LMSSystem.Models.Material", b =>
                {
                    b.Property<int>("MaterialID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaterialID"), 1L, 1);

                    b.Property<int>("CourseID")
                        .HasColumnType("int");

                    b.Property<string>("MaterialFile")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MaterialTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("datetime2");

                    b.HasKey("MaterialID");

                    b.HasIndex("CourseID");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("LMSSystem.Models.Message", b =>
                {
                    b.Property<int>("MessageID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MessageID"), 1L, 1);

                    b.Property<string>("MessageContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("MessageDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReceiverID")
                        .HasColumnType("int");

                    b.Property<int>("SenderID")
                        .HasColumnType("int");

                    b.HasKey("MessageID");

                    b.HasIndex("ReceiverID");

                    b.HasIndex("SenderID");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("LMSSystem.Models.Question", b =>
                {
                    b.Property<int>("QuestionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuestionID"), 1L, 1);

                    b.Property<int>("ExamID")
                        .HasColumnType("int");

                    b.Property<string>("QuestionContent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuestionType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("QuestionID");

                    b.HasIndex("ExamID");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("LMSSystem.Models.Role", b =>
                {
                    b.Property<int>("RoleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleID"), 1L, 1);

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleID");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("LMSSystem.Models.Schedule", b =>
                {
                    b.Property<int>("ScheduleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ScheduleID"), 1L, 1);

                    b.Property<int>("ClassID")
                        .HasColumnType("int");

                    b.Property<int>("CourseID")
                        .HasColumnType("int");

                    b.Property<DateTime>("ScheduleDate")
                        .HasColumnType("datetime2");

                    b.HasKey("ScheduleID");

                    b.HasIndex("ClassID");

                    b.HasIndex("CourseID");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("LMSSystem.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"), 1L, 1);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordResetToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RefreshTokenCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("RefreshTokenExpries")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ResetTokenExpries")
                        .HasColumnType("datetime2");

                    b.Property<int>("RoleID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdateAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VerificationToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("VerifyAt")
                        .HasColumnType("datetime2");

                    b.HasKey("UserID");

                    b.HasIndex("RoleID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("LMSSystem.Models.User_Class", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("ClassID")
                        .HasColumnType("int");

                    b.Property<DateTime>("EnrollmentDate")
                        .HasColumnType("datetime2");

                    b.HasKey("UserID", "ClassID");

                    b.HasIndex("ClassID");

                    b.ToTable("User_Class");
                });

            modelBuilder.Entity("LMSSystem.Models.User_Schedule", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.Property<int>("ScheduleID")
                        .HasColumnType("int");

                    b.Property<string>("IsPresent")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID", "ScheduleID");

                    b.HasIndex("ScheduleID");

                    b.ToTable("User_Schedule");
                });

            modelBuilder.Entity("LMSSystem.Models.Announcement", b =>
                {
                    b.HasOne("LMSSystem.Models.User", "User")
                        .WithMany("Announcements")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("LMSSystem.Models.Answer", b =>
                {
                    b.HasOne("LMSSystem.Models.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("LMSSystem.Models.Assignment", b =>
                {
                    b.HasOne("LMSSystem.Models.Class", "Class")
                        .WithMany("Assignments")
                        .HasForeignKey("ClassID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");
                });

            modelBuilder.Entity("LMSSystem.Models.Class", b =>
                {
                    b.HasOne("LMSSystem.Models.Assignment", null)
                        .WithMany("Classes")
                        .HasForeignKey("AssignmentID");
                });

            modelBuilder.Entity("LMSSystem.Models.Exam", b =>
                {
                    b.HasOne("LMSSystem.Models.Course", "Course")
                        .WithMany("Exams")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("LMSSystem.Models.Exam_User", b =>
                {
                    b.HasOne("LMSSystem.Models.Exam", "Exam")
                        .WithMany("Exam_User")
                        .HasForeignKey("ExamID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("LMSSystem.Models.User", "User")
                        .WithMany("Exam_User")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Exam");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LMSSystem.Models.Feedback", b =>
                {
                    b.HasOne("LMSSystem.Models.User", "User")
                        .WithMany("Feedbacks")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("LMSSystem.Models.Lesson", b =>
                {
                    b.HasOne("LMSSystem.Models.Course", "Course")
                        .WithMany("Lessons")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("LMSSystem.Models.Material", b =>
                {
                    b.HasOne("LMSSystem.Models.Course", "Course")
                        .WithMany("Materials")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("LMSSystem.Models.Message", b =>
                {
                    b.HasOne("LMSSystem.Models.User", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("LMSSystem.Models.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("LMSSystem.Models.Question", b =>
                {
                    b.HasOne("LMSSystem.Models.Exam", "Exam")
                        .WithMany("Questions")
                        .HasForeignKey("ExamID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Exam");
                });

            modelBuilder.Entity("LMSSystem.Models.Schedule", b =>
                {
                    b.HasOne("LMSSystem.Models.Class", "Class")
                        .WithMany("Schedules")
                        .HasForeignKey("ClassID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LMSSystem.Models.Course", "Course")
                        .WithMany("Schedules")
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("Course");
                });

            modelBuilder.Entity("LMSSystem.Models.User", b =>
                {
                    b.HasOne("LMSSystem.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("LMSSystem.Models.User_Class", b =>
                {
                    b.HasOne("LMSSystem.Models.Class", "Class")
                        .WithMany("User_Class")
                        .HasForeignKey("ClassID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("LMSSystem.Models.User", "User")
                        .WithMany("User_Class")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Class");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LMSSystem.Models.User_Schedule", b =>
                {
                    b.HasOne("LMSSystem.Models.Schedule", "Schedule")
                        .WithMany("User_Schedule")
                        .HasForeignKey("ScheduleID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("LMSSystem.Models.User", "User")
                        .WithMany("User_Schedule")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Schedule");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LMSSystem.Models.Assignment", b =>
                {
                    b.Navigation("Classes");
                });

            modelBuilder.Entity("LMSSystem.Models.Class", b =>
                {
                    b.Navigation("Assignments");

                    b.Navigation("Schedules");

                    b.Navigation("User_Class");
                });

            modelBuilder.Entity("LMSSystem.Models.Course", b =>
                {
                    b.Navigation("Exams");

                    b.Navigation("Lessons");

                    b.Navigation("Materials");

                    b.Navigation("Schedules");
                });

            modelBuilder.Entity("LMSSystem.Models.Exam", b =>
                {
                    b.Navigation("Exam_User");

                    b.Navigation("Questions");
                });

            modelBuilder.Entity("LMSSystem.Models.Question", b =>
                {
                    b.Navigation("Answers");
                });

            modelBuilder.Entity("LMSSystem.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("LMSSystem.Models.Schedule", b =>
                {
                    b.Navigation("User_Schedule");
                });

            modelBuilder.Entity("LMSSystem.Models.User", b =>
                {
                    b.Navigation("Announcements");

                    b.Navigation("Exam_User");

                    b.Navigation("Feedbacks");

                    b.Navigation("User_Class");

                    b.Navigation("User_Schedule");
                });
#pragma warning restore 612, 618
        }
    }
}
