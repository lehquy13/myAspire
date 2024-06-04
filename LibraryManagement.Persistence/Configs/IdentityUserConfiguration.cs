using LibraryManagement.Domain.Library.UserAggregate.Identity;
using LibraryManagement.Domain.Library.UserAggregate.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Persistence.Configs;

public class IdentityUserConfiguration : IEntityTypeConfiguration<IdentityUser>
{
    public void Configure(EntityTypeBuilder<IdentityUser> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasColumnName("Id")
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => IdentityGuid.Create(value)
            );
        builder.Property(r => r.BalanceAmount).IsRequired();
        builder.Property(r => r.PasswordHash).IsRequired();
        builder.Property(r => r.PasswordSalt).IsRequired();

        //Mark UserId as a foreign key for IdentityUser
        builder.HasOne(r => r.User)
            .WithOne(r => r.IdentityUser)
            .HasForeignKey<IdentityUser>(r => r.Id)
            .IsRequired();

        builder.OwnsOne(r => r.OtpCode,
            navigationBuilder =>
            {
                navigationBuilder.Property(address => address.Value)
                    .HasColumnName("OtpCode")
                    .HasMaxLength(6);
                navigationBuilder.Property(address => address.ExpiredTime)
                    .HasColumnName("ExpiredTime");
            });
    }
}
