﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ScheduleCore.Entities
{
    public partial class StudentManagementContext : DbContext
    {
        public StudentManagementContext()
        {
        }

        public StudentManagementContext(DbContextOptions<StudentManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Attend> Attends { get; set; } = null!;
        public virtual DbSet<Building> Buildings { get; set; } = null!;
        public virtual DbSet<Classroom> Classrooms { get; set; } = null!;
        public virtual DbSet<Mark> Marks { get; set; } = null!;
        public virtual DbSet<Room> Rooms { get; set; } = null!;
        public virtual DbSet<Slot> Slots { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<StudentClassroom> StudentClassrooms { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;
        public virtual DbSet<Teacher> Teachers { get; set; } = null!;
        public virtual DbSet<Timetable> Timetables { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server =(local);database=StudentManagement;uid=sa;pwd=123456;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attend>(entity =>
            {
                entity.ToTable("Attend");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.StudentId).HasColumnName("studentId");

                entity.Property(e => e.TimeTableId).HasColumnName("timeTableId");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Attends)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("attend_studentid_foreign");

                entity.HasOne(d => d.TimeTable)
                    .WithMany(p => p.Attends)
                    .HasForeignKey(d => d.TimeTableId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("attend_timetableid_foreign");
            });

            modelBuilder.Entity<Building>(entity =>
            {
                entity.ToTable("Building");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Classroom>(entity =>
            {
                entity.ToTable("Classroom");

                entity.HasIndex(e => e.Code, "class_code_unique")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Semesters).HasColumnName("semesters");

                entity.Property(e => e.SubjectId).HasColumnName("subjectId");

                entity.Property(e => e.Year).HasColumnName("year");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Classrooms)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("classroom_subjectid_foreign");
            });

            modelBuilder.Entity<Mark>(entity =>
            {
                entity.ToTable("Mark");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClassroomId).HasColumnName("classroomId");

                entity.Property(e => e.Mark1).HasColumnName("mark");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Percent).HasColumnName("percent");

                entity.Property(e => e.StudentId).HasColumnName("studentId");

                entity.Property(e => e.SubjectId).HasColumnName("subjectId");

                entity.HasOne(d => d.Classroom)
                    .WithMany(p => p.Marks)
                    .HasForeignKey(d => d.ClassroomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("mark_classroomid_foreign");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Marks)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("mark_studentid_foreign");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Marks)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("mark_subjectid_foreign");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");

                entity.HasIndex(e => e.RoomNumber, "room_unique_roomNumber")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BuildingId).HasColumnName("buildingId");

                entity.Property(e => e.Capacity)
                    .HasColumnName("capacity")
                    .HasDefaultValueSql("('30')");

                entity.Property(e => e.Note)
                    .HasMaxLength(255)
                    .HasColumnName("note");

                entity.Property(e => e.RoomNumber).HasColumnName("roomNumber");

                entity.HasOne(d => d.Building)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.BuildingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("room_buildingid_foreign");
            });

            modelBuilder.Entity<Slot>(entity =>
            {
                entity.ToTable("Slot");

                entity.HasIndex(e => e.Name, "slot_name_unique")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.End).HasColumnName("end");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.Start).HasColumnName("start");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");

                entity.HasIndex(e => e.Code, "student_code_unique")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .HasColumnName("address");

                entity.Property(e => e.Code)
                    .HasMaxLength(255)
                    .HasColumnName("code");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(255)
                    .HasColumnName("phoneNumber");
            });

            modelBuilder.Entity<StudentClassroom>(entity =>
            {
                entity.ToTable("StudentClassroom");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClassroomId).HasColumnName("classroomId");

                entity.Property(e => e.StudentId).HasColumnName("studentId");

                entity.HasOne(d => d.Classroom)
                    .WithMany(p => p.StudentClassrooms)
                    .HasForeignKey(d => d.ClassroomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("studentclassroom_classroomid_foreign");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentClassrooms)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("studentclassroom_studentid_foreign");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("Subject");

                entity.HasIndex(e => e.Code, "subject_code_unique")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Code)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("code");

                entity.Property(e => e.Credit).HasColumnName("credit");

                entity.Property(e => e.CreditSlot).HasColumnName("creditSlot");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("Teacher");

                entity.HasIndex(e => e.Email, "teacher_email_unique")
                    .IsUnique();

                entity.HasIndex(e => e.TeacherCode, "teacher_teacherCode_unique")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("phoneNumber");

                entity.Property(e => e.TeacherCode)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("teacherCode");
            });

            modelBuilder.Entity<Timetable>(entity =>
            {
                entity.ToTable("Timetable");

                entity.HasIndex(e => new { e.Date, e.SlotId, e.ClassroomId }, "timetable_unique_class_per_teacher_slot")
                    .IsUnique();

                entity.HasIndex(e => new { e.Date, e.SlotId, e.RoomId }, "timetable_unique_room_per_day_slot")
                    .IsUnique();

                entity.HasIndex(e => new { e.Date, e.SlotId, e.TeacherId }, "timetable_unique_teacher_per_day_slot")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ClassroomId).HasColumnName("classroomId");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.RoomId).HasColumnName("roomId");

                entity.Property(e => e.SlotId).HasColumnName("slotId");

                entity.Property(e => e.SubjectId).HasColumnName("subjectId");

                entity.Property(e => e.TeacherId).HasColumnName("teacherId");

                entity.HasOne(d => d.Classroom)
                    .WithMany(p => p.Timetables)
                    .HasForeignKey(d => d.ClassroomId)
                    .HasConstraintName("timetable_classid_foreign");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Timetables)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("timetable_roomid_foreign");

                entity.HasOne(d => d.Slot)
                    .WithMany(p => p.Timetables)
                    .HasForeignKey(d => d.SlotId)
                    .HasConstraintName("timetable_slotid_foreign");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Timetables)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("timetable_subjectid_foreign");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Timetables)
                    .HasForeignKey(d => d.TeacherId)
                    .HasConstraintName("timetable_teacherid_foreign");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}