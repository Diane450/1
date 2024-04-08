using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace _1.Models;

public partial class Ispr2438IbragimovaDm1Context : DbContext
{
    public Ispr2438IbragimovaDm1Context()
    {
    }

    public Ispr2438IbragimovaDm1Context(DbContextOptions<Ispr2438IbragimovaDm1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<AcceptedGroupRequest> AcceptedGroupRequests { get; set; }

    public virtual DbSet<AcceptedPrivateRequest> AcceptedPrivateRequests { get; set; }

    public virtual DbSet<BlackListGuest> BlackListGuests { get; set; }

    public virtual DbSet<CheckGroupRequest> CheckGroupRequests { get; set; }

    public virtual DbSet<CheckPrivateRequest> CheckPrivateRequests { get; set; }

    public virtual DbSet<DeniedReason> DeniedReasons { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupDeniedRequest> GroupDeniedRequests { get; set; }

    public virtual DbSet<GroupMeeting> GroupMeetings { get; set; }

    public virtual DbSet<GroupMeetingsGuest> GroupMeetingsGuests { get; set; }

    public virtual DbSet<GroupRequestsAllowedAccess> GroupRequestsAllowedAccesses { get; set; }

    public virtual DbSet<Guest> Guests { get; set; }

    public virtual DbSet<MeetingStatus> MeetingStatuses { get; set; }

    public virtual DbSet<PrivateDeniedRequest> PrivateDeniedRequests { get; set; }

    public virtual DbSet<PrivateMeeting> PrivateMeetings { get; set; }

    public virtual DbSet<PrivateMeetingsGuest> PrivateMeetingsGuests { get; set; }

    public virtual DbSet<PrivateRequestsAllowedAccess> PrivateRequestsAllowedAccesses { get; set; }

    public virtual DbSet<Subdepartment> Subdepartments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<VisitPurpose> VisitPurposes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=cfif31.ru;database=ISPr24-38_IbragimovaDM_1;uid=ISPr24-38_IbragimovaDM;pwd=ISPr24-38_IbragimovaDM", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.36-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<AcceptedGroupRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("AcceptedGroupRequest");

            entity.HasIndex(e => e.GroupRequestId, "FK_AGR_GroupRequestId_idx");

            entity.Property(e => e.Time).HasColumnType("time");

            entity.HasOne(d => d.GroupRequest).WithMany(p => p.AcceptedGroupRequests)
                .HasForeignKey(d => d.GroupRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AGR_GroupRequestId");
        });

        modelBuilder.Entity<AcceptedPrivateRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("AcceptedPrivateRequest");

            entity.HasIndex(e => e.PrivateRequestId, "FK_APR_PrivateRequestId_idx");

            entity.Property(e => e.Time).HasColumnType("time");

            entity.HasOne(d => d.PrivateRequest).WithMany(p => p.AcceptedPrivateRequests)
                .HasForeignKey(d => d.PrivateRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_APR_PrivateRequestId");
        });

        modelBuilder.Entity<BlackListGuest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.GuestId, "FK_BlackListGuests_GuestId_idx");

            entity.Property(e => e.Reason).HasColumnType("text");

            entity.HasOne(d => d.Guest).WithMany(p => p.BlackListGuests)
                .HasForeignKey(d => d.GuestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BlackListGuests_GuestId");
        });

        modelBuilder.Entity<CheckGroupRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("CheckGroupRequest");

            entity.HasIndex(e => e.GroupRequestId, "FK_GPR_GroupRequestId_idx");

            entity.HasOne(d => d.GroupRequest).WithMany(p => p.CheckGroupRequests)
                .HasForeignKey(d => d.GroupRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GPR_GroupRequestId");
        });

        modelBuilder.Entity<CheckPrivateRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("CheckPrivateRequest");

            entity.HasIndex(e => e.PrivateRequestId, "FK_CPR_PrivateRequestId_idx");

            entity.HasOne(d => d.PrivateRequest).WithMany(p => p.CheckPrivateRequests)
                .HasForeignKey(d => d.PrivateRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CPR_PrivateRequestId");
        });

        modelBuilder.Entity<DeniedReason>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Descryption).HasColumnType("text");
            entity.Property(e => e.ShortName).HasMaxLength(6);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Department");

            entity.Property(e => e.DepartmentName).HasMaxLength(45);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.IdEmployees).HasName("PRIMARY");

            entity.HasIndex(e => e.Department, "FK_EmployeesDepartment_idx");

            entity.HasIndex(e => e.Subdepartment, "FK_EmployeesSubdepartment_idx");

            entity.Property(e => e.IdEmployees).HasColumnName("idEmployees");
            entity.Property(e => e.FullName).HasMaxLength(45);

            entity.HasOne(d => d.DepartmentNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.Department)
                .HasConstraintName("FK_EmployeesDepartment");

            entity.HasOne(d => d.SubdepartmentNavigation).WithMany(p => p.Employees)
                .HasForeignKey(d => d.Subdepartment)
                .HasConstraintName("FK_EmployeesSubdepartment");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.IdGroups).HasName("PRIMARY");

            entity.Property(e => e.IdGroups).HasColumnName("idGroups");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<GroupDeniedRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.DeniedReasonId, "FK_GDRDeniedReason_idx");

            entity.HasIndex(e => e.GroupRequestId, "FK_GDRPrivateRequest_idx");

            entity.HasOne(d => d.DeniedReason).WithMany(p => p.GroupDeniedRequests)
                .HasForeignKey(d => d.DeniedReasonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GDRDeniedReason");

            entity.HasOne(d => d.GroupRequest).WithMany(p => p.GroupDeniedRequests)
                .HasForeignKey(d => d.GroupRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GDRPrivateRequest");
        });

        modelBuilder.Entity<GroupMeeting>(entity =>
        {
            entity.HasKey(e => e.GroupMeetingId).HasName("PRIMARY");

            entity.ToTable("GroupMeeting");

            entity.HasIndex(e => e.DeprtmentId, "FK_DepartentGM_idx");

            entity.HasIndex(e => e.EmployeeId, "FK_GMEmployee_idx");

            entity.HasIndex(e => e.GroupId, "FK_GMGroup_idx");

            entity.HasIndex(e => e.VisitPurposeId, "FK_GMVisitPurpose_idx");

            entity.HasIndex(e => e.StatusId, "FK_GroupStatus_idx");

            entity.HasOne(d => d.Deprtment).WithMany(p => p.GroupMeetings)
                .HasForeignKey(d => d.DeprtmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GMDepartment");

            entity.HasOne(d => d.Employee).WithMany(p => p.GroupMeetings)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GMEmployee");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupMeetings)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GMGroup");

            entity.HasOne(d => d.Status).WithMany(p => p.GroupMeetings)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GMStatus");

            entity.HasOne(d => d.VisitPurpose).WithMany(p => p.GroupMeetings)
                .HasForeignKey(d => d.VisitPurposeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GMVisitPurpose");
        });

        modelBuilder.Entity<GroupMeetingsGuest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("GroupMeetingsGuest");

            entity.HasIndex(e => e.GuestId, "FK_GMGuests_idx");

            entity.HasIndex(e => e.GroupMeetingId, "FK_UGMK_idx");

            entity.HasOne(d => d.GroupMeeting).WithMany(p => p.GroupMeetingsGuests)
                .HasForeignKey(d => d.GroupMeetingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UGMK");

            entity.HasOne(d => d.Guest).WithMany(p => p.GroupMeetingsGuests)
                .HasForeignKey(d => d.GuestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GMGuests");
        });

        modelBuilder.Entity<GroupRequestsAllowedAccess>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("GroupRequestsAllowedAccess");

            entity.HasIndex(e => e.GroupRequestsId, "FK_AllowedAccess_GroupRequests_idx");

            entity.Property(e => e.CompletionTime).HasColumnType("time");
            entity.Property(e => e.EndingTime).HasColumnType("time");
            entity.Property(e => e.EnterTime).HasColumnType("time");
            entity.Property(e => e.StartTime).HasColumnType("time");

            entity.HasOne(d => d.GroupRequests).WithMany(p => p.GroupRequestsAllowedAccesses)
                .HasForeignKey(d => d.GroupRequestsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AllowedAccess_GroupRequests");
        });

        modelBuilder.Entity<Guest>(entity =>
        {
            entity.HasKey(e => e.IdGuests).HasName("PRIMARY");

            entity.HasIndex(e => e.UserId, "FK_PGUserId_idx");

            entity.Property(e => e.IdGuests).HasColumnName("idGuests");
            entity.Property(e => e.Email).HasMaxLength(45);
            entity.Property(e => e.LastName).HasMaxLength(45);
            entity.Property(e => e.Name).HasMaxLength(45);
            entity.Property(e => e.Note).HasMaxLength(45);
            entity.Property(e => e.Organization).HasMaxLength(45);
            entity.Property(e => e.PassportNumber).HasMaxLength(6);
            entity.Property(e => e.PassportSeries).HasMaxLength(4);
            entity.Property(e => e.Patronymic).HasMaxLength(45);
            entity.Property(e => e.Phone).HasMaxLength(45);

            entity.HasOne(d => d.User).WithMany(p => p.Guests)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_PGUserId");
        });

        modelBuilder.Entity<MeetingStatus>(entity =>
        {
            entity.HasKey(e => e.IdStatus).HasName("PRIMARY");

            entity.ToTable("MeetingStatus");

            entity.Property(e => e.IdStatus).HasColumnName("idStatus");
            entity.Property(e => e.StatusName).HasMaxLength(45);
        });

        modelBuilder.Entity<PrivateDeniedRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.DeniedReasonId, "FK_PDRDeniedReason_idx");

            entity.HasIndex(e => e.PrivateRequestId, "FK_PDRPrivateRequest_idx");

            entity.HasOne(d => d.DeniedReason).WithMany(p => p.PrivateDeniedRequests)
                .HasForeignKey(d => d.DeniedReasonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PDRDeniedReason");

            entity.HasOne(d => d.PrivateRequest).WithMany(p => p.PrivateDeniedRequests)
                .HasForeignKey(d => d.PrivateRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PDRPrivateRequest");
        });

        modelBuilder.Entity<PrivateMeeting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("PrivateMeeting");

            entity.HasIndex(e => e.DepartmentId, "FK_PMDeprtment_idx");

            entity.HasIndex(e => e.EmployeeId, "FK_PrivateMEmployeeId_idx");

            entity.HasIndex(e => e.StatusId, "FK_PrivateMeetingStatus_idx");

            entity.HasIndex(e => e.VisitPurposeId, "FK_VisitPurposeId_idx");

            entity.HasOne(d => d.Department).WithMany(p => p.PrivateMeetings)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PMDepartment");

            entity.HasOne(d => d.Employee).WithMany(p => p.PrivateMeetings)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrivateMEmployeeId");

            entity.HasOne(d => d.Status).WithMany(p => p.PrivateMeetings)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrivateStatus");

            entity.HasOne(d => d.VisitPurpose).WithMany(p => p.PrivateMeetings)
                .HasForeignKey(d => d.VisitPurposeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VisitPurposeId");
        });

        modelBuilder.Entity<PrivateMeetingsGuest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.HasIndex(e => e.PrivateMeetingId, "FK__idx");

            entity.HasIndex(e => e.GuestId, "fk_User_idx");

            entity.HasOne(d => d.Guest).WithMany(p => p.PrivateMeetingsGuests)
                .HasForeignKey(d => d.GuestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GuestsPrivate");

            entity.HasOne(d => d.PrivateMeeting).WithMany(p => p.PrivateMeetingsGuests)
                .HasForeignKey(d => d.PrivateMeetingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UPM");
        });

        modelBuilder.Entity<PrivateRequestsAllowedAccess>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("PrivateRequestsAllowedAccess");

            entity.HasIndex(e => e.PrivateRequestId, "FK_AllowedAccess_PrivateRequestId_idx");

            entity.Property(e => e.CompletionTime).HasColumnType("time");
            entity.Property(e => e.EndingTime).HasColumnType("time");
            entity.Property(e => e.EnterTime)
                .HasColumnType("time")
                .HasColumnName("EnterTIme");
            entity.Property(e => e.StartTime).HasColumnType("time");

            entity.HasOne(d => d.PrivateRequest).WithMany(p => p.PrivateRequestsAllowedAccesses)
                .HasForeignKey(d => d.PrivateRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AllowedAccess_PrivateRequestId");
        });

        modelBuilder.Entity<Subdepartment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("Subdepartment");

            entity.Property(e => e.SubdepartmentName).HasMaxLength(45);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PRIMARY");

            entity.ToTable("User");

            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.Email).HasMaxLength(45);
            entity.Property(e => e.Login).HasMaxLength(45);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Salt).HasMaxLength(225);
        });

        modelBuilder.Entity<VisitPurpose>(entity =>
        {
            entity.HasKey(e => e.IdVisitPurpose).HasName("PRIMARY");

            entity.ToTable("VisitPurpose");

            entity.Property(e => e.IdVisitPurpose).HasColumnName("idVisitPurpose");
            entity.Property(e => e.Name).HasMaxLength(45);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
