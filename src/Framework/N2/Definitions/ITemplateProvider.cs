﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N2.Definitions
{
	/// <summary>
	/// Provides the ability to create items with alternative editors, and pre-canned content.
	/// </summary>
	public interface ITemplateProvider
	{
		/// <summary>Gets all templates for the given content type</summary>
		/// <param name="contentType">The type of content item whose templates to get.</param>
		/// <returns>An emumeration of templates of the given type.</returns>
		IEnumerable<TemplateDefinition> GetTemplates(Type contentType);

		/// <summary>Gives the template definition for items created by this template.</summary>
		/// <param name="item">The item whose template definition to get.</param>
		/// <returns>The corresponding template definition or null if the item doesn't derive from this provider.</returns>
		TemplateDefinition GetTemplate(ContentItem item);
	}

	public static class TemplateProviderExtensions
	{
		public static IEnumerable<TemplateDefinition> GetTemplates(this IEnumerable<ITemplateProvider> providers, Type contentType)
		{
			if (contentType == null) return new TemplateDefinition[0];

			var templates = providers.SelectMany(tp => tp.GetTemplates(contentType)).ToList();
			if (!templates.Any(t => t.ReplaceDefault))
				return templates;
			return templates.Where(t => t.Name != null).ToList();
		}

		public static TemplateDefinition GetTemplate(this IEnumerable<ITemplateProvider> providers, Type contentType, string templateName)
		{
			if (contentType == null) return null;

			return providers
				.SelectMany(tp => tp.GetTemplates(contentType))
				.FirstOrDefault(td => td.Name == templateName);
		}

		public static TemplateDefinition GetTemplate(this IEnumerable<ITemplateProvider> providers, ContentItem item)
		{
			if (item == null) return null;

			return providers.Select(tp => tp.GetTemplate(item)).FirstOrDefault(t => t != null);
		}

		public static ItemDefinition GetDefinition(this IEnumerable<ITemplateProvider> providers, ContentItem item)
		{
			if (item == null) return null;

			var template = providers.GetTemplate(item);
			if (template == null)
				return null;
			return template.Definition;
		}
	}
}
