﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using N2.Web.Rendering;
using N2.Definitions.Runtime;
using N2.Web.Mvc;
using System.IO;
using N2.Definitions;
using N2.Web.Mvc.Html;
using System.Web.Mvc;

namespace N2.Web.Mvc
{
	public class RegisteringDisplayRenderer<T> : EditableBuilder<T>, IHtmlString, IDisplayRenderer where T : IEditable
	{
		public RenderingContext Context { get; set; }

		public RegisteringDisplayRenderer(RenderingContext context, ContentRegistration registration)
			: base(context.PropertyName, registration)
		{
			this.Context = context;
		}

		public RegisteringDisplayRenderer(HtmlHelper html, string propertyName, ContentRegistration registration)
			: base(propertyName, registration)
		{
			Context = new RenderingContext();
			Context.Content = html.CurrentItem();
			var providers = html.ResolveServices<ITemplateProvider>();
			var template = providers.GetTemplate(Context.Content);
			if (template != null)
				Context.Displayable = template.Definition.Displayables.FirstOrDefault(d => d.Name == propertyName);
			Context.Html = html;
			Context.PropertyName = propertyName;
		}

		#region IHtmlString Members

		public string ToHtmlString()
		{
			return ToString();
		}

		#endregion

		public void Render()
		{
			if (Context.Displayable == null)
				return;

			WriteTo(Context.Html.ViewContext.Writer);
		}

		public override string ToString()
		{
			using (var sw = new StringWriter())
			{
				WriteTo(sw);
				return sw.ToString();
			}
		}

		public void WriteTo(TextWriter writer)
		{
			if (Context.Displayable == null)
				return;

			Context.Html.ResolveService<DisplayableRendererSelector>()
				.Render(Context, writer);
		}
	}

}