﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection.Metadata;
using WebApi.Models;

namespace WebApi.Context
{
    public class LMSDbContext : DbContext
    {
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Course> Courses { get; set; }
        public string DbPath { get; }
        public LMSDbContext(DbContextOptions options):base(options)
        {
            
        }
    }
}
