﻿using System;
using System.Collections.Generic;

namespace N2.Web
{
	public interface IHost
	{
		Site CurrentSite { get; }
		Site DefaultSite { get; set; }
		IList<Site> Sites { get; }
	}
}