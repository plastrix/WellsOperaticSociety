using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Xml.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using WellsOperaticSociety.Models;
using WellsOperaticSociety.Models.MemberModels;
using WellsOperaticSociety.DAL;
using WellsOperaticSociety.Models.AdminModels;
using WellsOperaticSociety.Models.Enums;
using WellsOperaticSociety.Models.ReportModels;
using Member = WellsOperaticSociety.Models.MemberModels.Member;

namespace WellsOperaticSociety.BusinessLogic
{
    public class DataManager
    {

        public UmbracoContext Umbraco { get; set; }

        public DataManager(UmbracoContext umbContext)
        {
            Umbraco = umbContext;
        }

        public DataManager() : this(UmbracoContext.Current)
        {
        }

        public IPublishedContent GetFunctionListNode()
        {
            UmbracoHelper helper = new UmbracoHelper(Umbraco);
            return helper.TypedContentAtRoot().Single(m => m.DocumentTypeAlias == "functions");
        }

        public IPublishedContent GetLoginNode()
        {
            UmbracoHelper helper = new UmbracoHelper(Umbraco);
            return helper.TypedContentAtRoot().Single(m => m.DocumentTypeAlias == "login");
        }

        public IPublishedContent GetMembersNode()
        {
            UmbracoHelper helper = new UmbracoHelper(Umbraco);
            return helper.TypedContentAtRoot().Single(m => m.DocumentTypeAlias == "memberSection");
        }

        public IPublishedContent GetEditMemberAdminNode()
        {
            UmbracoHelper helper = new UmbracoHelper(Umbraco);
            return helper.TypedContent(1103);
        }

        public IPublishedContent GetManualsNode()
        {
            var membersNode = GetMembersNode();
            return membersNode.Children().SingleOrDefault(m => m.DocumentTypeAlias == "manuals");
        }

        public IPublishedContent GetMinuetsNode()
        {
            var membersNode = GetMembersNode();
            return membersNode.Children().SingleOrDefault(m => m.DocumentTypeAlias == "minutes");
        }

        public List<Function> GetUpcomingFunctions(int pageSize, int rowIndex)
        {
            var funcListNode = GetFunctionListNode();
            return funcListNode.Children.Select(n => new Function(n)).Where(n => n.EndDate.Date >= DateTime.Now.Date).OrderByDescending(n => n.EndDate).Skip(rowIndex*pageSize).Take(pageSize).ToList();
        }

        public List<Function> GetExpiredFunctions(int pageSize, int rowIndex)
        {
            var funcListNode = GetFunctionListNode();
            return funcListNode.Children.Select(n => new Function(n)).Where(n => n.EndDate.Date < DateTime.Now.Date && !n.DoNotShowInPastProductions).OrderByDescending(n => n.EndDate).Skip(rowIndex * pageSize).Take(pageSize).ToList();
        }

        public List<Function> GetFunctions(int pageSize, int rowIndex)
        {
            var funcListNode = GetFunctionListNode();
            return funcListNode.Children.Select(n => new Function(n)).OrderByDescending(n => n.EndDate).Skip(rowIndex * pageSize).Take(pageSize).ToList();
        }

        public List<Function> GetFunctions(IEnumerable<int> functionIds)
        {
            var funcListNode = GetFunctionListNode();
            return funcListNode.Children.Where(m=>functionIds.Contains(m.Id)).Select(n => new Function(n)).OrderByDescending(n => n.EndDate).ToList();
        }

        public int GetCountOfExpiredFunctions()
        {
            var funcListNode = GetFunctionListNode();
            return funcListNode.Children.Count(n => n.GetPropertyValue<DateTime>("endDate").Date < DateTime.Now.Date && !n.GetPropertyValue<bool>("doNotShowInPastProductions"));
        }

        public IList<IPublishedContent> GetManuals()
        {
            return GetManualsNode().Children().ToList();
        }

        public IList<IPublishedContent> GetMinuets()
        {
            return GetMinuetsNode().Children().ToList();
        }

        public Function GetFunction(int id)
        {
            var helper = new UmbracoHelper(Umbraco);
            var func = helper.TypedContent(id);
            if (func != null)
            {
                var function = new Function(func);
                function.MemberRoles = GetMemberRolesInFunction(id);
                return function;
            }
            return null;
        }

        /// <summary>
        /// Returns a list of active members that have a valid Membership record
        /// </summary>
        /// <returns></returns>
        public List<Member> GetActiveMembers()
        {
            var key = "ActiveMembers";
            var activeMembers = HttpContext.Current.Session[key] as List<Member>;
            if (activeMembers == null)
            {
                var helper = new UmbracoHelper(Umbraco);
                var members = ApplicationContext.Current.Services.MemberService.GetAllMembers()
                    .Select(m => new Member(helper.TypedMember(m.Id)))
                    .Where(m => m.Deactivated == false)
                    .ToList();

                var currentMemberships = GetCurrentMemberships();
                activeMembers = new List<Member>();
                foreach (var membership in currentMemberships)
                {
                    var m = members.SingleOrDefault(x => x.Id == membership.Member);
                    if (m != null)
                    {
                        activeMembers.Add(m);
                    }
                }
                HttpContext.Current.Session[key] = activeMembers;
            }
            return activeMembers;
        }

        /// <summary>
        /// Returns all vehicle registrations for active members
        /// </summary>
        /// <returns></returns>
        public List<VehicleRegistrationModel> GetVehicleRegistrations()
        {
            var membersWithReg = GetActiveMembers().Where(m => m.VehicleRegistration1.IsNotNullOrEmpty() || m.VehicleRegistration2.IsNotNullOrEmpty()).ToList();
            var regList = new List<VehicleRegistrationModel>();
            regList.AddRange(membersWithReg.Where(m => m.VehicleRegistration1.IsNotNullOrEmpty())
                    .Select(m => new VehicleRegistrationModel() { Member = m, Registration = m.VehicleRegistration1 })
                    .ToList());
            regList.AddRange(membersWithReg.Where(m => m.VehicleRegistration2.IsNotNullOrEmpty())
                    .Select(m => new VehicleRegistrationModel() { Member = m, Registration = m.VehicleRegistration1 })
                    .ToList());
            return regList.OrderBy(m => m.Registration).ToList();
        }


        public object AcitveMemberSuggestions(string query)
        {
            return GetActiveMembers().Where(m=>m.Name.ToLower().Contains(query.ToLower())).Select(m => new { label = m.Name, value = m.Id });
        }

        public List<LongServiceAward> GetDueLongServiceAwards()
        {
            var members = GetActiveMembers();
            var previousAwards = GetAwardedLongServiceAwards();
            var unawrdedAwards = GetLongServiceAwards().Where(m => m.Awarded == false).ToList();
            var dueAwards = new List<LongServiceAward>();
            foreach (var member in members)
            {
                int startYear;
                int currentYear = DateTime.UtcNow.Year;
                if (member.DateApprovedForMembership == null)
                    continue;
                startYear = ((DateTime) member.DateApprovedForMembership).Year;
                var membersMemberships = GetMembershipsForUser(member.Id);

                var activeYears = member.PreviousYears;

                for (int i = startYear; i < currentYear; i++)
                {

                    if (membersMemberships.Any(m => m.StartDate.Year == i))
                        activeYears++;
                }

                int tmp = activeYears/5;

                
                for (int x = 0; x <= tmp-2; x++)
                {
                    if (x > (int)Enum.GetValues(typeof(NodaLongServiceAward)).Cast<NodaLongServiceAward>().Max())
                    {
                        break;
                    }
                    //This is where we check if already given or hidden
                    if (!previousAwards.Any(m => m.Award == (NodaLongServiceAward) x && m.Member == member.Id) && !unawrdedAwards.Any(m => m.Award == (NodaLongServiceAward)x && m.Member == member.Id))
                    {
                        dueAwards.Add(new LongServiceAward()
                        {
                            Award = (NodaLongServiceAward) x,
                            Member = member.Id,
                            MemberDetails = member
                        });
                    }
                }
            }
            dueAwards.AddRange(unawrdedAwards);
            return dueAwards.OrderByDescending(m=>m.Award).ToList();
        }

        public List<LongServiceAward> GetAwardedLongServiceAwards()
        {
            return GetLongServiceAwards().Where(m=>m.Awarded).ToList();
        }

        #region DataContext
        public List<Membership> GetMembershipsForUser(int memberId)
        {
            using (var db = new DataContext())
            {
                return db.Memberships.Where(m => m.Member == memberId).OrderByDescending(m => m.EndDate).ToList();
            }
        }

        public void CreateMembership(Membership membership)
        {
            using (var db = new DataContext())
            {
                db.Memberships.Add(membership);
                db.SaveChanges();
            }
        }

        public void DeleteMembership(int membershipId)
        {
            using (var db = new DataContext())
            {
                var memberhsip = db.Memberships.SingleOrDefault(m => m.MembershipId == membershipId);
                if (memberhsip != null)
                {
                    db.Memberships.Remove(memberhsip);
                    db.SaveChanges();
                }
            }
        }

        public List<MemberRolesInShow> GetMemberRolesInFunction(int functionId, int? memberId = null)
        {
            var helper = new UmbracoHelper(Umbraco);
            using (var db = new DataContext())
            {
                var roles = db.MemberRolesInShows.Where(m => m.FunctionId == functionId);
                if (memberId != null)
                    roles = roles.Where(m => m.MemberId == (int)memberId);

                roles.ForEach(m => m.Member = new Member(helper.TypedMember(m.MemberId)));
                return roles.OrderBy(m=>m.Group).ThenBy(m=>m.Role).ToList();
            }
        }

        public void CreateMemberInFunction(MemberRolesInShow memberRoleInShow)
        {
            using (var db = new DataContext())
            {
                db.MemberRolesInShows.Add(memberRoleInShow);
                db.SaveChanges();
            }
        }

        public void DeleteMemberRoleInFunction(int memberRoleInShowId)
        {
            using (var db = new DataContext())
            {
                var memeberRoleInShow = db.MemberRolesInShows.SingleOrDefault(m => m.MemberRolesInShowId == memberRoleInShowId);
                if (memeberRoleInShow != null)
                {
                    db.MemberRolesInShows.Remove(memeberRoleInShow);
                    db.SaveChanges();
                }
            }
        }

        public List<string> GetRoleSuggestions(string query)
        {
            using (var db = new DataContext())
            {
                return db.MemberRolesInShows.Where(m => m.Role.ToLower().Contains(query.ToLower())).GroupBy(m=>new {m.Role}).Select(m => m.FirstOrDefault().Role).ToList();
            }
        }

        public List<string> GetGroupSuggestions(string query)
        {
            using (var db = new DataContext())
            {
                return db.MemberRolesInShows.Where(m => m.Group.ToLower().Contains(query.ToLower())).GroupBy(m => new { m.Group }).Select(m => m.FirstOrDefault().Group).ToList();
            }
        }

        public List<Seat> GetSeats()
        {
            using (var db = new DataContext())
            {
                return db.Seats.OrderBy(m => m.SeatNumber).ToList();
            }
        }

        public void AddSeat(Seat seat)
        {
            using (var db = new DataContext())
            {
                db.Seats.Add(seat);
                db.SaveChanges();
            }
        }

        public void DeleteSeat(int seatId)
        {
            using (var db = new DataContext())
            {
                var seat = db.Seats.SingleOrDefault(m => m.SeatId == seatId);
                if (seat != null)
                {
                    db.Seats.Remove(seat);
                    db.SaveChanges();
                }
            }
        }

        public List<Function> GetPreviousFunctionsForMember(int memberId)
        {
            //Get all memberotfunction roles
            using (var db = new DataContext())
            {
                var memberRolesInShows = db.MemberRolesInShows.Where(m => m.MemberId == memberId);
                return GetFunctions(memberRolesInShows.Select(m => m.FunctionId).ToList());
            }
        }

        public List<Membership> GetCurrentMemberships()
        {
            using (var db = new DataContext())
            {
                var key = "CurrentMemberships";
                var memberships = HttpContext.Current.Session[key] as List<Membership>;
                if (memberships == null)
                {
                    memberships =
                        db.Memberships.Where(m => m.StartDate <= DateTime.UtcNow && m.EndDate >= DateTime.UtcNow)
                            .GroupBy(m => new {m.Member})
                            .Select(m => m.FirstOrDefault())
                            .ToList();
                    HttpContext.Current.Session[key] = memberships;
                }

                return memberships;
                //return db.MemberRolesInShows.Where(m => m.Role.ToLower().Contains(query.ToLower())).GroupBy(m => new { m.Role }).Select(m => m.FirstOrDefault().Role).ToList();
            }
        }

        public List<LongServiceAward> GetLongServiceAwards()
        {
            var helper = new UmbracoHelper(Umbraco);
            using (var db = new DataContext())
            {
                var list = db.LongServiceAwards.ToList();
                list.ForEach(m => m.MemberDetails = new Member(helper.TypedMember(m.Member)));
                return list.OrderByDescending(m=>m.Award).ToList();

            }
        }

        public void AddOrUpdateLongServiceAward(LongServiceAward longServiceAward)
        {
            using (var db = new DataContext())
            {
                db.LongServiceAwards.AddOrUpdate(longServiceAward);
                db.SaveChanges();
            }
        }
        #endregion

        #region Robot and siitemap fuinctions
        public void PublishRobots()
        {
            UmbracoHelper helper = new UmbracoHelper(Umbraco);
            var list = new List<IPublishedContent>();

            GetPublishedNodes(helper.TypedContentAtRoot(), list, true);

            var path = HostingEnvironment.MapPath("~/robots.txt");
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);

            IList<string> builder = new List<string>();
            builder.Add("User-agent: *");

            foreach (var content in list)
            {
                builder.Add($"Disallow: {content.Url.EnsureEndsWith('/')}");
            }

            builder.Add("Disallow: /umbraco/");
            builder.Add("Disallow: /search/");

            builder.Add($"Sitemap: {HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority)}/sitemap.xml");

            System.IO.File.AppendAllLines(path, builder);
        }

        private readonly XNamespace _xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        public void PublishSitemap()
        {
            UmbracoHelper helper = new UmbracoHelper(Umbraco);
            var list = new List<IPublishedContent>();

            GetPublishedNodes(helper.TypedContentAtRoot(), list, false);

            var path = HostingEnvironment.MapPath("~/sitemap.xml");
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);


            XDocument doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement(_xmlns + "urlset",
                    new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
                    new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance")));
            //XNamespace ns1 = "http://www.sitemaps.org/schemas/sitemap/0.9";
            //XNamespace ns2 = "http://www.w3.org/2001/XMLSchema-instance";
            //XNamespace ns3 = "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd";

            var root = doc.FirstNode as XElement;

            foreach (var content in list)
            {

                var url = content.Url.ToAbsoluteUrl().EnsureEndsWith('/');
                XElement locNode = new XElement(_xmlns + "url", new XElement(_xmlns + "loc", url));
                root.Add(locNode);
            }

            doc.Save(path);
        }

        private void GetPublishedNodes(IEnumerable<IPublishedContent> nodes, IList<IPublishedContent> list, bool getExcluded)
        {
            var excludedDt = GetSitemapExcludedDocumentTypes();
            var items = nodes.Where("Visible").Where(n => !excludedDt.Contains(n.DocumentTypeAlias));

            //items = getExcluded ? items.Where(n => n.GetPropertyValue<int>("excludeFromSitemap") == 1) : items.Where(n => n.GetPropertyValue<int>("excludeFromSitemap") != 1);
            foreach (var item in items)
            {
                if (getExcluded && item.GetPropertyValue<int>("excludeFromSitemap") == 1) list.Add(item);
                else if (!getExcluded && item.GetPropertyValue<int>("excludeFromSitemap") != 1) list.Add(item);

                GetPublishedNodes(item.Children, list, getExcluded);
            }
        }
        public static List<string> GetSitemapExcludedDocumentTypes()
        {
            return new List<string> { };//"Truth", "Poll", "PollItem", "PollFolder" };
        }
        #endregion
    }
}
