using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PJ_DGRL.Models.DGRLModels;

public partial class DbDgrlContext : DbContext
{
    public DbDgrlContext()
    {
    }

    public DbDgrlContext(DbContextOptions<DbDgrlContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountAdmin> AccountAdmins { get; set; }

    public virtual DbSet<AccountLecturer> AccountLecturers { get; set; }

    public virtual DbSet<AccountStudent> AccountStudents { get; set; }

    public virtual DbSet<AnswerList> AnswerLists { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<ClassAnswer> ClassAnswers { get; set; }

    public virtual DbSet<Classify> Classifies { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<GroupQuestion> GroupQuestions { get; set; }

    public virtual DbSet<Lecturers> Lecturers { get; set; }

    public virtual DbSet<LecturerInfor> LecturerInfors { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<QuestionHisory> QuestionHisories { get; set; }

    public virtual DbSet<QuestionList> QuestionLists { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SelfAnswer> SelfAnswers { get; set; }

    public virtual DbSet<Semester> Semesters { get; set; }

    public virtual DbSet<Students> Students { get; set; }

    public virtual DbSet<SumaryOfPoint> SumaryOfPoints { get; set; }

    public virtual DbSet<TypeQuestion> TypeQuestions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=DB_DGRL;Trusted_Connection=True;MultipleActiveResultSets=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountAdmin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AccountA__3214EC07ACFA1E1C");

            entity.ToTable("AccountAdmin");

            entity.HasIndex(e => e.UserName, "UQ__AccountA__C9F28456B49EE784").IsUnique();

            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.AccountAdmins)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__AccountAd__RoleI__5441852A");
        });

        modelBuilder.Entity<AccountLecturer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AccountL__3214EC07AFFD456C");

            entity.ToTable("AccountLecturer");

            entity.HasIndex(e => e.UserName, "UQ__AccountL__C9F28456036DFC55").IsUnique();

            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.LecturerId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Lecturer).WithMany(p => p.AccountLecturers)
                .HasForeignKey(d => d.LecturerId)
                .HasConstraintName("FK__AccountLe__Lectu__4E88ABD4");
        });

        modelBuilder.Entity<AccountStudent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AccountS__3214EC07AD864394");

            entity.ToTable("AccountStudent");

            entity.HasIndex(e => e.UserName, "UQ__AccountS__C9F2845604BA037B").IsUnique();

            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.StudentId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Student).WithMany(p => p.AccountStudents)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__AccountSt__Stude__4AB81AF0");
        });

        modelBuilder.Entity<AnswerList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AnswerLi__3214EC07A3F75B00");

            entity.ToTable("AnswerList");

            entity.Property(e => e.ContentAnswer).HasMaxLength(500);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Question).WithMany(p => p.AnswerLists)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK__AnswerLis__Quest__60A75C0F");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Class__3214EC0788388886");

            entity.ToTable("Class");

            entity.HasIndex(e => e.Name, "UQ__Class__737584F6AFC9C057").IsUnique();

            entity.Property(e => e.CourseId)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Name)
                .HasMaxLength(7)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Course).WithMany(p => p.Classes)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Class__CourseId__3C69FB99");

            entity.HasOne(d => d.Department).WithMany(p => p.Classes)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__Class__Departmen__3D5E1FD2");
        });

        modelBuilder.Entity<ClassAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ClassAns__3214EC074062DEA6");

            entity.ToTable("ClassAnswer");

            entity.Property(e => e.CreateBy)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.StudentId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Answer).WithMany(p => p.ClassAnswers)
                .HasForeignKey(d => d.AnswerId)
                .HasConstraintName("FK__ClassAnsw__Answe__6D0D32F4");

            entity.HasOne(d => d.Semester).WithMany(p => p.ClassAnswers)
                .HasForeignKey(d => d.SemesterId)
                .HasConstraintName("FK__ClassAnsw__Semes__01142BA1");

            entity.HasOne(d => d.Student).WithMany(p => p.ClassAnswers)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__ClassAnsw__Stude__6C190EBB");
        });

        modelBuilder.Entity<Classify>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Classify__3214EC07EAB3A006");

            entity.ToTable("Classify");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Course__3214EC07F2EF2AE8");

            entity.ToTable("Course");

            entity.Property(e => e.Id)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC073B8259C8");

            entity.ToTable("Department");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<GroupQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GroupQue__3214EC07448C918B");

            entity.ToTable("GroupQuestion");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Lecturers>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Lecturer__3214EC0766CCFF5A");

            entity.ToTable("Lecturer");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Department).WithMany(p => p.Lecturers)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__Lecturer__Depart__45F365D3");

            entity.HasOne(d => d.Position).WithMany(p => p.Lecturers)
                .HasForeignKey(d => d.PositionId)
                .HasConstraintName("FK__Lecturer__Positi__2B0A656D");
        });

        modelBuilder.Entity<LecturerInfor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Lecturer__3214EC07B598D90F");

            entity.ToTable("LecturerInfor");

            entity.Property(e => e.LecturerId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Note)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("note");
            entity.Property(e => e.Position)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Rank)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Lecturer).WithMany(p => p.LecturerInfors)
                .HasForeignKey(d => d.LecturerId)
                .HasConstraintName("FK__LecturerI__Lectu__160F4887");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Position__3214EC07B1F12F48");

            entity.ToTable("Position");

            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<QuestionHisory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC071344B57A");

            entity.ToTable("QuestionHisory");

            entity.Property(e => e.CreateBy)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CreateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Question).WithMany(p => p.QuestionHisories)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK__QuestionH__Quest__6383C8BA");

            entity.HasOne(d => d.Semester).WithMany(p => p.QuestionHisories)
                .HasForeignKey(d => d.SemesterId)
                .HasConstraintName("FK__QuestionH__Semes__6477ECF3");
        });

        modelBuilder.Entity<QuestionList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC07959E7A8B");

            entity.ToTable("QuestionList");

            entity.Property(e => e.ContentQuestion).HasMaxLength(500);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateBy).HasMaxLength(50);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.GroupQuestion).WithMany(p => p.QuestionLists)
                .HasForeignKey(d => d.GroupQuestionId)
                .HasConstraintName("FK__QuestionL__Group__5DCAEF64");

            entity.HasOne(d => d.TypeQuestion).WithMany(p => p.QuestionLists)
                .HasForeignKey(d => d.TypeQuestionId)
                .HasConstraintName("FK__QuestionL__TypeQ__5CD6CB2B");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC0741565F8D");

            entity.Property(e => e.RoleName).HasMaxLength(100);
        });

        modelBuilder.Entity<SelfAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SelfAnsw__3214EC07C6A3F5A7");

            entity.ToTable("SelfAnswer");

            entity.Property(e => e.StudentId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Answer).WithMany(p => p.SelfAnswers)
                .HasForeignKey(d => d.AnswerId)
                .HasConstraintName("FK__SelfAnswe__Answe__68487DD7");

            entity.HasOne(d => d.Semester).WithMany(p => p.SelfAnswers)
                .HasForeignKey(d => d.SemesterId)
                .HasConstraintName("FK__SelfAnswe__Semes__693CA210");

            entity.HasOne(d => d.Student).WithMany(p => p.SelfAnswers)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__SelfAnswe__Stude__6754599E");
        });

        modelBuilder.Entity<Semester>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Semester__3214EC07A1546B64");

            entity.ToTable("Semester");

            entity.Property(e => e.DateEndClass).HasColumnType("datetime");
            entity.Property(e => e.DateEndLecturer).HasColumnType("datetime");
            entity.Property(e => e.DateEndStudent).HasColumnType("datetime");
            entity.Property(e => e.DateOpenStudent).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(2)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.SchoolYear)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Students>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Student__3214EC075BB1316D");

            entity.ToTable("Student");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.HasOne(d => d.Class).WithMany(p => p.Students)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("FK__Student__ClassId__4222D4EF");

            entity.HasOne(d => d.Position).WithMany(p => p.Students)
                .HasForeignKey(d => d.PositionId)
                .HasConstraintName("FK__Student__Positio__2BFE89A6");
        });

        modelBuilder.Entity<SumaryOfPoint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SumaryOf__3214EC07531C2497");

            entity.ToTable("SumaryOfPoint");

            entity.Property(e => e.Classify).HasMaxLength(50);
            entity.Property(e => e.StudentId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UserClass)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.UserLecturer)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Class).WithMany(p => p.SumaryOfPoints)
                .HasForeignKey(d => d.ClassId)
                .HasConstraintName("fk_htk_ClassId");

            entity.HasOne(d => d.Semester).WithMany(p => p.SumaryOfPoints)
                .HasForeignKey(d => d.SemesterId)
                .HasConstraintName("FK__SumaryOfP__Semes__70DDC3D8");

            entity.HasOne(d => d.Student).WithMany(p => p.SumaryOfPoints)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("FK__SumaryOfP__Stude__6FE99F9F");
        });

        modelBuilder.Entity<TypeQuestion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TypeQues__3214EC078BEFA553");

            entity.ToTable("TypeQuestion");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
