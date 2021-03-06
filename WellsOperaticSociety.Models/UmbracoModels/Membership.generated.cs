//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder v3.0.7.99
//
//   Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;
using Umbraco.ModelsBuilder;
using Umbraco.ModelsBuilder.Umbraco;

namespace WellsOperaticSociety.Models.UmbracoModels
{
	/// <summary>Membership</summary>
	[PublishedContentModel("membership")]
	public partial class Membership : Memberships
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "membership";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public Membership(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<Membership, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// End Date: The date the membership will run out
		///</summary>
		[ImplementPropertyType("endDate")]
		public DateTime EndDate
		{
			get { return this.GetPropertyValue<DateTime>("endDate"); }
		}

		///<summary>
		/// Member: The member the membership belongs to
		///</summary>
		[ImplementPropertyType("member")]
		public IPublishedContent Member
		{
			get { return this.GetPropertyValue<IPublishedContent>("member"); }
		}

		///<summary>
		/// Membership Type: The type of membership
		///</summary>
		[ImplementPropertyType("membershipType")]
		public string MembershipType
		{
			get { return this.GetPropertyValue<string>("membershipType"); }
		}

		///<summary>
		/// Start Date: The date the membership starts from
		///</summary>
		[ImplementPropertyType("startDate")]
		public DateTime StartDate
		{
			get { return this.GetPropertyValue<DateTime>("startDate"); }
		}
	}
}
