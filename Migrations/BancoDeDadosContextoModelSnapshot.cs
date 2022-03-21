﻿// <auto-generated />
using System;
using EmprestimoBancario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EmprestimoBancario.Migrations
{
    [DbContext(typeof(BancoDeDadosContexto))]
    partial class BancoDeDadosContextoModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.15")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EmprestimoBancario.Models.Banco", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Documento")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Banco");
                });

            modelBuilder.Entity("EmprestimoBancario.Models.Empresa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Documento")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Empresa");
                });

            modelBuilder.Entity("EmprestimoBancario.Models.Emprestimo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("LinhaDeCreditoId")
                        .HasColumnType("int");

                    b.Property<double>("Quantia")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("LinhaDeCreditoId");

                    b.ToTable("Emprestimos");
                });

            modelBuilder.Entity("EmprestimoBancario.Models.Investimento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("InvestidorId")
                        .HasColumnType("int");

                    b.Property<int?>("LinhaDeCreditoId")
                        .HasColumnType("int");

                    b.Property<double>("Porcentagem")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("InvestidorId");

                    b.HasIndex("LinhaDeCreditoId");

                    b.ToTable("Investimento");
                });

            modelBuilder.Entity("EmprestimoBancario.Models.InvestimentoDeEmprestimo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("EmprestimoId")
                        .HasColumnType("int");

                    b.Property<int?>("InvestimentoId")
                        .HasColumnType("int");

                    b.Property<double>("Quantia")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("EmprestimoId");

                    b.HasIndex("InvestimentoId");

                    b.ToTable("InvestimentoDeEmprestimo");
                });

            modelBuilder.Entity("EmprestimoBancario.Models.LinhaDeCredito", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("EmpresaId")
                        .HasColumnType("int");

                    b.Property<float>("Limite")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("EmpresaId");

                    b.ToTable("LinhaDeCredito");
                });

            modelBuilder.Entity("EmprestimoBancario.Models.Taxa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Data")
                        .HasColumnType("datetime2");

                    b.Property<int?>("InvestimentoId")
                        .HasColumnType("int");

                    b.Property<double>("Quantia")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("InvestimentoId");

                    b.ToTable("Taxa");
                });

            modelBuilder.Entity("EmprestimoBancario.Models.Emprestimo", b =>
                {
                    b.HasOne("EmprestimoBancario.Models.LinhaDeCredito", "LinhaDeCredito")
                        .WithMany()
                        .HasForeignKey("LinhaDeCreditoId");

                    b.Navigation("LinhaDeCredito");
                });

            modelBuilder.Entity("EmprestimoBancario.Models.Investimento", b =>
                {
                    b.HasOne("EmprestimoBancario.Models.Banco", "Investidor")
                        .WithMany()
                        .HasForeignKey("InvestidorId");

                    b.HasOne("EmprestimoBancario.Models.LinhaDeCredito", null)
                        .WithMany("Investimentos")
                        .HasForeignKey("LinhaDeCreditoId");

                    b.Navigation("Investidor");
                });

            modelBuilder.Entity("EmprestimoBancario.Models.InvestimentoDeEmprestimo", b =>
                {
                    b.HasOne("EmprestimoBancario.Models.Emprestimo", null)
                        .WithMany("InvestimentoDeEmprestimos")
                        .HasForeignKey("EmprestimoId");

                    b.HasOne("EmprestimoBancario.Models.Investimento", "Investimento")
                        .WithMany()
                        .HasForeignKey("InvestimentoId");

                    b.Navigation("Investimento");
                });

            modelBuilder.Entity("EmprestimoBancario.Models.LinhaDeCredito", b =>
                {
                    b.HasOne("EmprestimoBancario.Models.Empresa", "Empresa")
                        .WithMany()
                        .HasForeignKey("EmpresaId");

                    b.Navigation("Empresa");
                });

            modelBuilder.Entity("EmprestimoBancario.Models.Taxa", b =>
                {
                    b.HasOne("EmprestimoBancario.Models.Investimento", null)
                        .WithMany("Taxas")
                        .HasForeignKey("InvestimentoId");
                });

            modelBuilder.Entity("EmprestimoBancario.Models.Emprestimo", b =>
                {
                    b.Navigation("InvestimentoDeEmprestimos");
                });

            modelBuilder.Entity("EmprestimoBancario.Models.Investimento", b =>
                {
                    b.Navigation("Taxas");
                });

            modelBuilder.Entity("EmprestimoBancario.Models.LinhaDeCredito", b =>
                {
                    b.Navigation("Investimentos");
                });
#pragma warning restore 612, 618
        }
    }
}
