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
	/// <summary>Standard Page</summary>
	[PublishedContentModel("standardPage")]
	public partial class StandardPage : Shared
	{
#pragma warning disable 0109 // new is redundant
		public new const string ModelTypeAlias = "standardPage";
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
#pragma warning restore 0109

		public StandardPage(IPublishedContent content)
			: base(content)
		{ }

#pragma warning disable 0109 // new is redundant
		public new static PublishedContentType GetModelContentType()
		{
			return PublishedContentType.Get(ModelItemType, ModelTypeAlias);
		}
#pragma warning restore 0109

		public static PublishedPropertyType GetModelPropertyType<TValue>(Expression<Func<StandardPage, TValue>> selector)
		{
			return PublishedContentModelUtility.GetModelPropertyType(GetModelContentType(), selector);
		}

		///<summary>
		/// Grid Content
		///</summary>
		[ImplementPropertyType("gridContent")]
		public Newtonsoft.Json.Linq.JToken GridContent
		{
			get { return this.GetPropertyValue<Newtonsoft.Json.Linq.JToken>("gridContent"); }
		}

		///<summary>
		/// Hero Image: Recommended size 1980 X 872. This will allow it to scale for mobiles or larger screens.
		///</summary>
		[ImplementPropertyType("heroImage")]
		public IEnumerable<IPublishedContent> HeroImage
		{
			get { return this.GetPropertyValue<IEnumerable<IPublishedContent>>("heroImage"); }
		}
	}
}