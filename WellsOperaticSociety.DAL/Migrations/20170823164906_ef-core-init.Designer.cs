﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using WellsOperaticSociety.DAL;
using WellsOperaticSociety.Models.Enums;

namespace WellsOperaticSociety.DAL.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20170823164906_ef-core-init")]
    partial class efcoreinit
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WellsOperaticSociety.Models.AdminModels.Seat", b =>
                {
                    b.Property<int>("SeatId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("SeatNumber")
                        .IsRequired();

                    b.HasKey("SeatId");

                    b.ToTable("Seats");
                });

            modelBuilder.Entity("WellsOperaticSociety.Models.AdminModels.Voucher", b =>
                {
                    b.Property<int>("VoucherId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FunctionId");

                    b.Property<string>("Key")
                        .IsRequired();

                    b.Property<int>("MemberId");

                    b.HasKey("VoucherId");

                    b.ToTable("Vouchers");
                });

            modelBuilder.Entity("WellsOperaticSociety.Models.MemberModels.LongServiceAward", b =>
                {
                    b.Property<int>("LongServiceAwardId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Award");

                    b.Property<bool>("Awarded");

                    b.Property<bool>("Hide");

                    b.Property<int>("Member");

                    b.HasKey("LongServiceAwardId");

                    b.ToTable("LongServiceAwards");
                });

            modelBuilder.Entity("WellsOperaticSociety.Models.MemberModels.MemberRolesInShow", b =>
                {
                    b.Property<int>("MemberRolesInShowId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("FunctionId");

                    b.Property<string>("Group")
                        .IsRequired();

                    b.Property<int?>("MemberId")
                        .IsRequired();

                    b.Property<string>("Role");

                    b.HasKey("MemberRolesInShowId");

                    b.ToTable("MemberRolesInShows");
                });

            modelBuilder.Entity("WellsOperaticSociety.Models.MemberModels.Membership", b =>
                {
                    b.Property<int>("MembershipId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("CancelAtEnd");

                    b.Property<DateTime>("EndDate");

                    b.Property<bool>("IsSubscription");

                    b.Property<int>("Member");

                    b.Property<int>("MembershipType");

                    b.Property<DateTime>("StartDate");

                    b.Property<string>("StripeSubscriptionId");

                    b.HasKey("MembershipId");

                    b.ToTable("Memberships");
                });

            modelBuilder.Entity("WellsOperaticSociety.Models.ServiceModels.AuthorisationToken", b =>
                {
                    b.Property<int>("AuthorisationTokenId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Created");

                    b.Property<int>("Member");

                    b.Property<string>("Token");

                    b.Property<bool>("Used");

                    b.HasKey("AuthorisationTokenId");

                    b.ToTable("AuthorisationTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
