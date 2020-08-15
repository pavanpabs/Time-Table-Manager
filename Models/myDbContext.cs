﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TimeTableManager.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            Database.EnsureCreated();

        }
      
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<LecturerDetails> LectureInformation { get; set; }
        public DbSet<SubjectDetails> SubjectInformation { get; set; }

        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Building>()
            .HasMany(c => c.RoomsAS)
            .WithOne(e => e.BuildingAS);

            modelBuilder.Entity<Schedule>().HasData(GetSchedules());
            modelBuilder.Entity<LecturerDetails>().HasData(GetLectureDetails());
            modelBuilder.Entity<SubjectDetails>().HasData(GetSubjectDetails());
            //modelBuilder.Entity<Building>().HasData(GetBuildings());

            base.OnModelCreating(modelBuilder);

        }
        private Schedule[] GetSchedules()
        {
            return new Schedule[]
                {
                    new Schedule{ Id=1, Working_days_count=3,Working_days="Monday,Tuesday,Friday",working_time_hrs=5,Working_time_mins=30,Working_duration="One Hour"}
                };
        }

        private LecturerDetails[] GetLectureDetails()
        {
            return new LecturerDetails[]
                {
                    new LecturerDetails{ Id=1, LecName="Saman Perera",EmpId="emp1500245",Faculty="Computing",Department="Software Engineering",Center="Malabe",Building="Main Building",EmpLevel=5,Rank="5.emp1500245"}
                };
        }


        private SubjectDetails[] GetSubjectDetails()
        {
            return new SubjectDetails[]
                {
                    new SubjectDetails{ Id=1, OfferedYear="4thYear",OfferedSemester="2nd Semester",SubjectName="OOP",SubjectCode="SE1258",LecHours=14,TutorialHours=10,LabHours=8,EvalHours=2}
                };
        }



    }

        private Building[] GetBuildings()
        {
            return new Building[]
                {
                    new Building{ Id = -1,Bid="NB", Name="New Building"}
                };
        }

        private Room[] GetRooms()
        {
            return new Room[]
                {
                    new Room{Id = -1,Rid="B02", Type="Lab",BuildingAS = new Building{ Id = -1,Bid="NB", Name="New Building"}}
                };
        }
    }

}

