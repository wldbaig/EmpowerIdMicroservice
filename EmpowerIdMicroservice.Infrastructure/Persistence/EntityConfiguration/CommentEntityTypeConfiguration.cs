using EmpowerIdMicroservice.Domain.AggregateModules.CommentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders; 

namespace EmpowerIdMicroservice.Infrastructure.Persistence.EntityConfiguration
{
    internal class CommentEntityTypeConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("Comment");

            builder.Property(_ => _.CommentId)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn(seed: 100, increment: 10); 

            builder.HasKey(_=>_.CommentId).HasName("PK_Comment").IsClustered();
            builder.Property(e => e.Text).IsRequired();

            builder.HasOne(_ => _.Post)
                .WithMany()
                .HasForeignKey(_ => _.PostId)
                .HasConstraintName("FK_Comment_Post")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
