﻿using System.IO;
using System.Text;
using System.Web;
using N2.Engine;
using N2.Tests.Web.Items;
using NUnit.Framework;
using System.Configuration;
using System.Collections;
using System;
using N2.Engine.MediumTrust;
using N2.Engine.Castle;

namespace N2.Tests.Web.WebControls
{
	public abstract class WebControlTestsBase : ItemTestsBase
	{
		protected const string ZoneName = "TheZone";
		
		protected PageItem page;
		protected DataItem data;

		[TestFixtureSetUp]
		public virtual void TestFixtureSetUp()
		{
			var engine = new ContentEngine(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None),
				"n2nodb",
				new WindsorServiceContainer(),
				EventBroker.Instance,
				new ContainerConfigurer());
			N2.Context.Replace(engine);
			engine.Initialize();
			engine.Host.CurrentSite.RootItemID = 1;
			engine.Host.CurrentSite.StartPageID = 1;

			page = CreateOneItem<PageItem>(1, "page", null);
		}

		[SetUp]
		public override void SetUp()
		{
			base.SetUp();

			data = CreateOneItem<DataItem>(2, "data", page);
			data.ZoneName = ZoneName;
		}

		protected override T CreateOneItem<T>(int id, string name, ContentItem parent)
		{
			var item = base.CreateOneItem<T>(id, name, parent);
			N2.Context.Current.Persister.Repository.SaveOrUpdate(item);
			return item;
		}

		protected void Initialize(string queryString)
		{
			var request = new HttpRequest("/Default.aspx", "http://localhost/", queryString);
            request.Browser = new HttpBrowserCapabilities();
            request.Browser.Capabilities = new Hashtable();
            request.Browser.Capabilities["ecmascriptversion"] = "1.7";
            request.Browser.Capabilities["w3cdomversion"] = "2.0";
            var response = new HttpResponse(new StringWriter(new StringBuilder()));
			HttpContext.Current = new HttpContext(request, response)
			{
				ApplicationInstance = new HttpApplication(), 
				User = SecurityUtilities.CreatePrincipal("admin")
			};
		}
	}
}
