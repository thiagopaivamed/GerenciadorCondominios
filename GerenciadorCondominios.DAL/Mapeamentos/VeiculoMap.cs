using GerenciadorCondominios.BLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerenciadorCondominios.DAL.Mapeamentos
{
    public class VeiculoMap : IEntityTypeConfiguration<Veiculo>
    {
        public void Configure(EntityTypeBuilder<Veiculo> builder)
        {
            builder.HasKey(v => v.VeiculoId);
            builder.Property(v => v.Nome).IsRequired().HasMaxLength(40);
            builder.Property(v => v.Cor).IsRequired().HasMaxLength(20);
            builder.Property(v => v.Marca).IsRequired().HasMaxLength(20);
            builder.Property(v => v.Placa).IsRequired().HasMaxLength(20);
            builder.HasIndex(v => v.Placa).IsUnique();
            builder.Property(v => v.UsuarioId).IsRequired();

            builder.HasOne(v => v.Usuario).WithMany(v => v.Veiculos).HasForeignKey(v => v.UsuarioId);

            builder.ToTable("Veiculos");
        }
    }
}
