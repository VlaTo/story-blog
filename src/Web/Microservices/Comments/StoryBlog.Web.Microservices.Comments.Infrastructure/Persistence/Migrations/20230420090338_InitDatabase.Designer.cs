﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoryBlog.Web.Microservices.Comments.Infrastructure.Persistence;

#nullable disable

namespace StoryBlog.Web.Microservices.Comments.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(CommentsDbContext))]
    [Migration("20230420090338_InitDatabase")]
    partial class InitDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StoryBlog.Web.Microservices.Comments.Domain.Entities.Comment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("bit");

                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ModifiedAt")
                        .ValueGeneratedOnUpdate()
                        .HasColumnType("datetime2");

                    b.Property<long?>("ParentId")
                        .IsRequired()
                        .HasColumnType("bigint");

                    b.Property<Guid>("PostKey")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.HasIndex("PostKey");

                    b.ToTable("Comments", (string)null);
                });

            modelBuilder.Entity("StoryBlog.Web.Microservices.Comments.Domain.Entities.Comment", b =>
                {
                    b.HasOne("StoryBlog.Web.Microservices.Comments.Domain.Entities.Comment", "Parent")
                        .WithMany("Comments")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("StoryBlog.Web.Microservices.Comments.Domain.Entities.Comment", b =>
                {
                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}
