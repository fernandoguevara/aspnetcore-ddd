using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notes.Domain.AggregatesModel.NoteAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notes.Infrastructure.EntityConfigurations
{
    public class EmailEntityConfiguration : IEntityTypeConfiguration<Email>
    {
        public void Configure(EntityTypeBuilder<Email> builder)
        {
            builder.ToTable("Email");

            builder.HasKey(b => b.Id);

            builder.Ignore(b => b.DomainEvents);

            builder.Property<Guid>("NoteId")
                .IsRequired();

            builder
                .Property<string>("_action")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("Action")
                .IsRequired();

            builder
                .Property<DateTime>("_createdAt")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("CreatedAt")
                .IsRequired();
        }
    }
}
