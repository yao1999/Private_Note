﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Private_Note.Areas.Identity.Data;
using Private_Note.Models;

namespace Private_Note.Data
{
    public class PrivateNoteDBContext : IdentityDbContext<ApplicationUser>
    {
        public PrivateNoteDBContext(DbContextOptions<PrivateNoteDBContext> options)
            : base(options)
        {
        }

        public DbSet<Files> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Files>()
                .HasKey(c => new
                {
                    c.FileName,
                    c.FileType,
                    c.File,
                    c.CreatedDate
                });
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}