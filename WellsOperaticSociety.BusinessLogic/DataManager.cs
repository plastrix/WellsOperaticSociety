using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Xml.Linq;
using MailChimp.Net;
using MailChimp.Net.Core;
using MailChimp.Net.Interfaces;
using MailChimp.Net.Models;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Security;
using Umbraco.Web;
using WellsOperaticSociety.Models;
using WellsOperaticSociety.Models.MemberModels;
using WellsOperaticSociety.DAL;
using WellsOperaticSociety.Models.AdminModels;
using WellsOperaticSociety.Models.Enums;
using WellsOperaticSociety.Models.ReportModels;
using Member = WellsOperaticSociety.Models.MemberModels.Member;
using Task = System.Threading.Tasks.Task;

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

        public IPublishedContent GetForgotPasswordNode()
        {
            UmbracoHelper helper = new UmbracoHelper(Umbraco);
            return helper.TypedContentAtRoot().Single(m => m.DocumentTypeAlias == "forgotPassword");
        }

        public IPublishedContent GetResetPasswordNode()
        {
            UmbracoHelper helper = new UmbracoHelper(Umbraco);
            return helper.TypedContentAtRoot().Single(m => m.DocumentTypeAlias == "resetPassword");
        }

        public List<Function> GetUpcomingFunctions(int pageSize, int rowIndex)
        {
            var funcListNode = GetFunctionListNode();
            return funcListNode.Children.Select(n => new Function(n)).Where(n => n.EndDate.Date >= DateTime.Now.Date).OrderBy(n => n.EndDate).Skip(rowIndex*pageSize).Take(pageSize).ToList();
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
        /// Gets member by stripeid
        /// </summary>
        /// <returns></returns>
        public Member GetActiveMember(string stripeUserId)
        {
            var helper = new UmbracoHelper(Umbraco);
            var member = ApplicationContext.Current.Services.MemberService
                    .GetAllMembers()
                    .Select(m => new Member(helper.TypedMember(m.Id)))
                    .SingleOrDefault(m => m.Deactivated == false && m.StripeUserId == stripeUserId);

            return member;
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
            return GetActiveMembers().Where(m=>m.Name.ToLower().Contains(query.ToLower()) || m.Email.ToLower().Contains(query.ToLower()) ).Select(m => new { label = m.Name, value = m.Id });
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

        public async Task AddUserToMailChimpList(string listId, string emailToAdd,string firstName,string lastName)
        {
            try
            {
                IMailChimpManager mailChimpManager =
                    new MailChimpManager(SensativeInformation.MailChimpKeys.MailChimpApiKey);

                var member = new MailChimp.Net.Models.Member() {EmailAddress = emailToAdd, Status = Status.Subscribed};
                if(firstName.IsNotNullOrEmpty())
                    member.MergeFields.Add("FNAME", firstName);
                if(lastName.IsNotNullOrEmpty())
                    member.MergeFields.Add("LNAME", lastName);
                await mailChimpManager.Members.AddOrUpdateAsync(listId, member);
            }
            catch (MailChimpNotFoundException e)
            {
                //Could not remove from list as they did not exist
            }
            catch (MailChimpException e)
            {
                //TODO: Log error
            }
        }

        public async Task RemoveUserFromMailChimpList(string listId, string emailToRemove)
        {
            try
            {
                IMailChimpManager mailChimpManager =
                    new MailChimpManager(SensativeInformation.MailChimpKeys.MailChimpApiKey);

                await mailChimpManager.Members.DeleteAsync(listId, emailToRemove);
            }
            catch (MailChimpNotFoundException e)
            {
                //Could not remove from list as they did not exist
            }
            catch( MailChimpException e)
            {
                //TODO: Log error
            }
        }

        #region DataContext
        public List<Membership> GetMembershipsForUser(int memberId)
        {
            using (var db = new DataContext())
            {
                return db.Memberships.Where(m => m.Member == memberId).OrderByDescending(m => m.EndDate).ToList();
            }
        }

        public async Task AddOrUpdateMembership(Membership membership)
        {
            using (var db = new DataContext())
            {
                db.Memberships.AddOrUpdate(membership);
                db.SaveChanges();
            }
            var memberService = ApplicationContext.Current.Services.MemberService;
            //Add to Mailchimp
            var member = memberService.GetById(membership.Member);
            if (member != null)
            {
                var firstName = member.GetValue<string>("firstName");
                var lastName = member.GetValue<string>("lastName");
                //Added this check in as first and last name didnt exist originally so as we are converting back to first and last name then we will need this.
                if (firstName.IsNullOrEmpty() && lastName.IsNullOrEmpty())
                {
                    firstName = member.Name;
                }
                await AddUserToMailChimpList(MailChimpListIds.Membership, member.Email, firstName, lastName);
                await AddUserToMailChimpList(MailChimpListIds.MailingList, member.Email, firstName, lastName);
            }
        }

        public async Task DeleteMembership(int membershipId)
        {
            int memberId =0;
            using (var db = new DataContext())
            {
                var membership = db.Memberships.SingleOrDefault(m => m.MembershipId == membershipId);
                if (membership != null)
                {
                    memberId = membership.Member;
                    db.Memberships.Remove(membership);
                    db.SaveChanges();
                }
            }
            

            //Remove from mailchimp member list if not a member any more
            if (memberId != 0)
            {
                if (!DoesUserHaveCurrentMembership(memberId))
                {
                    var memberService = ApplicationContext.Current.Services.MemberService;
                    var member = memberService.GetById(memberId);
                    await RemoveUserFromMailChimpList(MailChimpListIds.Membership, member.Email);
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

        public Membership GetLatestMembership(string stripeSubscriptionId)
        {
            using (var db = new DataContext())
            {
                return db.Memberships.OrderByDescending(m=>m.StartDate).SingleOrDefault(m => m.StripeSubscriptionId == stripeSubscriptionId);
            }
        }

        public void SendResetPasswordEmail(Member member, ViewDataDictionary viewData, ControllerContext contContext, TempDataDictionary tempData)
        {
            using (RandomNumberGenerator rng = new RNGCryptoServiceProvider())
            {
                byte[] tokenData = new byte[32];
                rng.GetBytes(tokenData);

                string token = System.Convert.ToBase64String(tokenData).TrimEnd('=').Replace('+', '-').Replace('/', '_'); ;

                using (var db = new DataContext())
                {
                    //store token
                    var authtoken = new Models.ServiceModels.AuthorisationToken
                    {
                        Member = member.Id,
                        Token = token,
                        Created = DateTime.UtcNow
                    };
                    db.AuthorisationTokens.Add(authtoken);
                    db.SaveChanges();
                    var model = new Models.EmailModels.ResetPassword {Member = member};
                    var baseUri = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath.TrimEnd('/');
                    model.BaseUri = baseUri;
                    UriBuilder resetPasswordBsoluteUrl = new UriBuilder(baseUri)
                    {
                        Path = GetResetPasswordNode().Url,
                        Query = "token=" + token
                    };
                    model.Link = resetPasswordBsoluteUrl.ToString();
                    viewData.Model = model;
                    var html = RazorHelpers.RenderRazorViewToString("~/Views/Emails/ResetPassword.cshtml",
                        contContext, viewData, tempData);

                    //Generate email
                    var emailService = new EmailService.EmailHelpers();
                    emailService.SendEmail(member.Email,"Reset password request",html);
                }
            }
        }

        public Member ValidateToken(string token,int minsKeepAlive)
        {
            if (token.IsNullOrEmpty())
                return null;
            using (var db = new DataContext())
            {
                var expiredDate = DateTime.UtcNow.AddMinutes(minsKeepAlive*-1);
                var authToken = db.AuthorisationTokens.FirstOrDefault(m => m.Token == token && m.Used == false && m.Created>= expiredDate);
                if(authToken == null)
                    return null;
                var membershipHelper = new Umbraco.Web.Security.MembershipHelper(Umbraco);
                return new Member(membershipHelper.GetById(authToken.Member));
            }
        }

        public void TryInvalidateToken(string token)
        {
            if (token.IsNullOrEmpty())
                return;
            using (var db = new DataContext())
            {
                var authToken = db.AuthorisationTokens.SingleOrDefault(m => m.Token == token);
                if (authToken == null)
                    return;
                authToken.Used = true;
                db.SaveChanges();
            }
        }

        public bool DoesUserHaveCurrentMembership(int memberId)
        {
            using (var db = new DataContext())
            {
                var date = DateTime.UtcNow;
                return db.Memberships.Any(m => m.Member == memberId && m.StartDate <= date && m.EndDate >= date);
            }
        }

        public List<string> GetMostUsedRoles(int amount = 6)
        {
            using (var db = new DataContext())
            {
                var list = db.MemberRolesInShows.GroupBy(m => m.Group)
                    .OrderByDescending(g => g.Count())
                    .Take(amount)
                    .Select(g => g.Key).ToList();
                return list;
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
