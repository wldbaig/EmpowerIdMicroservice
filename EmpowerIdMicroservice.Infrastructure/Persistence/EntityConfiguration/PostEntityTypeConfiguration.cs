using EmpowerIdMicroservice.Domain.AggregateModules.PostAggregate;
using Microsoft.EntityFrameworkCore; 
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmpowerIdMicroservice.Infrastructure.Persistence.EntityConfiguration
{
    internal class PostEntityTypeConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Post");

            builder.Property(_ => _.PostId)
            .ValueGeneratedOnAdd()
            .UseIdentityColumn(seed: 100, increment: 10);

            builder.HasKey(_ => _.PostId).HasName("PK_Post").IsClustered();
            builder.Property(e => e.Title).IsRequired(); 
        }
    }
}
