using System.Data.Entity;
using WellsOperaticSociety.Models.AdminModels;
using WellsOperaticSociety.Models.MemberModels;

namespace WellsOperaticSociety.DAL
{
    public class DataContext : DbContext
    {
        public DbSet<MemberRolesInShow> MemberRolesInShows { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Seat> Seats { get; set; }
    }
}