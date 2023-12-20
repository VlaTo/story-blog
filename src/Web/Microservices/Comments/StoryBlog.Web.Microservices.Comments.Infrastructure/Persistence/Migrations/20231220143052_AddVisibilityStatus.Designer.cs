﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using StoryBlog.Web.Microservices.Comments.Infrastructure.Persistence;

#nullable disable

namespace StoryBlog.Web.Microservices.Comments.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(CommentsDbContext))]
    [Migration("20231220143052_AddVisibilityStatus")]
    partial class AddVisibilityStatus
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("StoryBlog.Web.Microservices.Comments.Domain.Entities.Comment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTimeOffset>("CreateAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset?>("ModifiedAt")
                        .ValueGeneratedOnUpdate()
                        .HasColumnType("timestamp with time zone");

                    b.Property<long?>("ParentId")
                        .HasColumnType("bigint");

                    b.Property<Guid>("PostKey")
                        .HasColumnType("uuid");

                    b.Property<int>("PublicationStatus")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(1024)
                        .IsUnicode(true)
                        .HasColumnType("character varying(1024)");

                    b.Property<int>("VisibilityStatus")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.HasIndex("PostKey");

                    b.ToTable("Comments", "Comment");
                });

            modelBuilder.Entity("StoryBlog.Web.Microservices.Comments.Domain.Entities.Comment", b =>
                {
                    b.HasOne("StoryBlog.Web.Microservices.Comments.Domain.Entities.Comment", "Parent")
                        .WithMany("Comments")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.NoAction);

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
