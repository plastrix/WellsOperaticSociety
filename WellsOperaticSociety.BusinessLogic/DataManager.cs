using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;
using System.Xml.Linq;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using WellsOperaticSociety.Models;
using WellsOperaticSociety.Models.MemberModels;
using WellsOperaticSociety.DAL;
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
            return funcListNode.Children.Select(n => new Function(n)).Where(n => n.EndDate>= DateTime.Now).Skip(rowIndex).Take(pageSize).OrderByDescending(n=>n.EndDate).ToList();
        }

        public List<Function> GetExpiredFunctions(int pageSize, int rowIndex)
        {
            var funcListNode = GetFunctionListNode();
            return funcListNode.Children.Select(n => new Function(n)).Where(n => n.EndDate < DateTime.Now).Skip(rowIndex).Take(pageSize).OrderByDescending(n => n.EndDate).ToList();
        }

        public List<Function> GetAllFunctions(int pageSize, int rowIndex)
        {
            var funcListNode = GetFunctionListNode();
            return funcListNode.Children.Select(n => new Function(n)).Skip(rowIndex).Take(pageSize).OrderByDescending(n => n.EndDate).ToList();
        }

        public int GetCountOfExpiredFunctions()
        {
            var funcListNode = GetFunctionListNode();
            return funcListNode.Children.Count(n => n.GetPropertyValue<DateTime>("endDate") < DateTime.Now);
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
            if(func != null)
                return new Function(func);
            return null;
        }

        #region DataContext
        public List<Membership> GetMembershipsForUser(int memberId)
        {
            using(var db = new DataContext())
            {
                return db.Memberships.Where(m => m.Member == memberId).OrderByDescending(m=>m.EndDate).ToList();
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
                if(memberhsip!=null)
                    db.Memberships.Remove(memberhsip);

                db.SaveChanges();
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

                roles.ForEach(m =>m.Member = new Member(helper.TypedMember(m.MemberId)));
                return roles.ToList();
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
                return db.MemberRolesInShows.Where(m=>m.Role.ToLower().Contains(query.ToLower())).Select(m=>m.Role).ToList();
            }
        }

        public List<string> GetGroupSuggestions(string query)
        {
            using (var db = new DataContext())
            {
                return db.MemberRolesInShows.Where(m => m.Group.ToLower().Contains(query.ToLower())).Select(m => m.Group).ToList();
            }
        }

        public object AcitveMemberSuggestions(string query)
        {
            //TODO: Make this return only members with active membership
            return ApplicationContext.Current.Services.MemberService.GetAllMembers().Select(m=> new { label = m.Name, value = m.Id });
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
            return new List<string> {};//"Truth", "Poll", "PollItem", "PollFolder" };
        }
        #endregion
    }
}
