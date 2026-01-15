using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class VideoGameConfiguration : IEntityTypeConfiguration<VideoGame>
{
    public void Configure(EntityTypeBuilder<VideoGame> builder)
    {
        builder.ToTable("VideoGames");

        builder.HasKey(vg => vg.Id);
        builder.Property(vg => vg.Id).ValueGeneratedOnAdd();

        builder.Property(vg => vg.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(vg => vg.Publisher)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(vg => vg.Developer)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(vg => vg.Description)
            .HasMaxLength(2000);

        builder.Property(vg => vg.Price)
            .HasPrecision(18, 2);

        builder.Property(vg => vg.Rating)
            .IsRequired();

        builder.Property(vg => vg.Genre)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(vg => vg.CoverImageUrl)
            .HasMaxLength(500);

        builder.Property(vg => vg.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(vg => vg.CreatedAt)
            .IsRequired();

        builder.Property(vg => vg.UpdatedAt)
            .IsRequired(false);

        builder.HasIndex(vg => vg.Title);
        builder.HasIndex(vg => vg.Genre);
        builder.HasIndex(vg => vg.IsActive);
    }
}

