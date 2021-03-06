﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace N2.Configuration
{
	/// <summary>
	/// Section configuring host settings such as root, and start node ids and 
	/// multiple sites.
	/// </summary>
	public class HostSection : ConfigurationSectionBase
	{
		[ConfigurationProperty("rootID", DefaultValue = 1)]
		public int RootID
		{
			get { return (int)base["rootID"]; }
			set { base["rootID"] = value; }
		}

		[ConfigurationProperty("startPageID", DefaultValue = 1)]
		public int StartPageID
		{
			get { return (int)base["startPageID"]; }
			set { base["startPageID"] = value; }
		}

		[ConfigurationProperty("serverAddress")]
		public string ServerAddress
		{
			get { return (string)base["serverAddress"]; }
			set { base["serverAddress"] = value; }
		}

		/// <summary>Enable multiple sites functionality.</summary>
		[ConfigurationProperty("multipleSites", DefaultValue = false)]
		public bool MultipleSites
		{
			get { return (bool)base["multipleSites"]; }
			set { base["multipleSites"] = value; }
		}

		/// <summary>Examine content nodes to find items that are site providers.</summary>
		[ConfigurationProperty("dynamicSites", DefaultValue = true)]
		public bool DynamicSites
		{
			get { return (bool)base["dynamicSites"]; }
			set { base["dynamicSites"] = value; }
		}

		/// <summary>Use wildcard matching for hostnames, e.g. both n2cms.com and www.n2cms.com.</summary>
		[ConfigurationProperty("wildcards", DefaultValue = false)]
		public bool Wildcards
		{
			get { return (bool)base["wildcards"]; }
			set { base["wildcards"] = value; }
		}

		[ConfigurationProperty("sites")]
		public SiteElementCollection Sites
		{
			get { return (SiteElementCollection)base["sites"]; }
			set { base["sites"] = value; }
		}

		[ConfigurationProperty("web")]
		public WebElement Web
		{
			get { return (WebElement)base["web"]; }
			set { base["web"] = value; }
		}

		/// <summary>Configures output cache for the templates.</summary>
		[ConfigurationProperty("outputCache")]
		public OutputCacheElement OutputCache
		{
			get { return (OutputCacheElement)base["outputCache"]; }
			set { base["outputCache"] = value; }
		}

		/// <summary>Web resource related config options.</summary>
		[ConfigurationProperty("resources")]
		public ResourcesElement Resources
		{
			get { return (ResourcesElement)base["resources"]; }
			set { base["resources"] = value; }
		}

		/// <summary>Virtual path provider related config options.</summary>
		[ConfigurationProperty("vpp")]
		public VppElement Vpp
		{
			get { return (VppElement)base["vpp"]; }
			set { base["vpp"] = value; }
		}
	}
}