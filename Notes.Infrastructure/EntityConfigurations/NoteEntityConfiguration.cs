using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notes.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notes.Infrastructure.EntityConfigurations
{
    class NoteEntityConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.ToTable("Note");
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedOnAdd();
            builder.Property(b => b.Title).IsRequired();
            builder.Property(b => b.Description).IsRequired();

           builder.HasMany(b => b.EmailActions)
                .WithOne()
                .HasForeignKey("NoteId")
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Property<Guid>("_userId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("UserId")
                .IsRequired(true);

            builder
                .Property<DateTime>("_createdAt")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("CreatedAt")
                .IsRequired(true);

            builder
                .Property<DateTime>("_modifiedAt")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("ModifiedAt")
                .IsRequired();

            var navigation = builder.Metadata.FindNavigation(nameof(Note.EmailActions));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
