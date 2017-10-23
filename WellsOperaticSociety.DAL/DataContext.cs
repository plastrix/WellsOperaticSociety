using Microsoft.EntityFrameworkCore;
using WellsOperaticSociety.Models.AdminModels;
using WellsOperaticSociety.Models.MemberModels;
using WellsOperaticSociety.Models.ServiceModels;
using WellsOperaticSociety.Models.StandardModels;

namespace WellsOperaticSociety.DAL
{
    public class DataContext : DbContext
    {
        public DbSet<MemberRolesInShow> MemberRolesInShows { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<LongServiceAward> LongServiceAwards { get; set; }
        public DbSet<AuthorisationToken> AuthorisationTokens { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<BoxOfficeTime> BoxOfficeTimes { get; set; }
    
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(System.Configuration.ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString);
        }
    }
}