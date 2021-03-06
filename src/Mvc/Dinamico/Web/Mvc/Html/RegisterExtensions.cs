﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using N2.Definitions.Runtime;
using N2.Details;
using System.Web.Mvc;
using N2.Definitions;

namespace N2.Web.Mvc.Html
{
	public static class RegisterExtensions
	{
		public static ContentRegistration Define(this ContentHelper content)
		{
			return content.Define(null);
		}

		public static ContentRegistration Define(this ContentHelper content, Action<ContentRegistration> registration)
		{
			var re = RegistrationExtensions.GetRegistrationExpression(content.Html);
			if (re != null)
			{
				FakeModel(content, re);

				re.GlobalSortOffset = 0;
				if (content.Html.GetType().IsGenericType)
				{
					var contentType = content.Html.GetType().GetGenericArguments().First();
					if (typeof(ContentItem).IsAssignableFrom(contentType) && re.ContentType == null)
					{
						re.ContentType = contentType;
						re.Title = contentType.Name;
					}
				}
				re.IsDefined = true;
				if(registration != null)
					registration(re);
			}
			return re;
		}

		private static void FakeModel(ContentHelper content, ContentRegistration re)
		{
			if (content.Html.ViewContext.ViewData.Model == null)
			{
				var modelType = content.Html.GetType().GetGenericArguments()[0];
				object model;
				if (modelType.IsAssignableFrom(re.ContentType) && !re.ContentType.IsAbstract)
					model = Activator.CreateInstance(re.ContentType);
				else if (typeof(ContentItem).IsAssignableFrom(modelType) && !modelType.IsAbstract)
					model = Activator.CreateInstance(modelType);
				else if (modelType.IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(modelType.GetGenericTypeDefinition()))
					model = Array.CreateInstance(modelType.GetGenericTypeDefinition().MakeArrayType(), 0);
				else
					return;

				content.Html.ViewContext.ViewData.Model = content.Html.ViewData.Model = model;
			}
		}

		public static ContentRegistration AppendDefinition(this ContentHelper content)
		{
			return content.AppendDefinition(null);
		}

		public static ContentRegistration AppendDefinition(this ContentHelper content, Action<ContentRegistration> registration)
		{
			var re = RegistrationExtensions.GetRegistrationExpression(content.Html);
			if (re != null)
			{
				re.GlobalSortOffset = 0;
				registration(re);
			}
			return re;
		}

		public static ContentRegistration PrependDefinition(this ContentHelper content)
		{
			return content.PrependDefinition(null);
		}

		public static ContentRegistration PrependDefinition(this ContentHelper content, Action<ContentRegistration> registration = null)
		{
			var re = RegistrationExtensions.GetRegistrationExpression(content.Html);
			if (re != null)
			{
				re.GlobalSortOffset = -1000;
				registration(re);
			}
			return re;
		}

		//// containables

		public static ContentRegistration Tab(this ContentRegistration re, string containerName, string tabText, Action<ContentRegistration> registration, int? sortOrder = null)
		{
			if (re == null) return re;

			re.Add(new N2.Web.UI.TabContainerAttribute(containerName, tabText, re.NextSortOrder(sortOrder)));

			string previousContainerName = re.ContainerName;
			re.ContainerName = containerName;
			if (registration != null)
			{
				registration(re);
				re.ContainerName = previousContainerName;
			}

			return re;
		}

		public static ContentRegistration FieldSet(this ContentRegistration re, string containerName, string legend, Action<ContentRegistration> registration, int? sortOrder = null)
		{
			if (re == null) return re;

			string previousContainerName = re.ContainerName;
			re.ContainerName = containerName;
			re.Add(new N2.Web.UI.FieldSetContainerAttribute(containerName, legend, re.NextSortOrder(sortOrder)));

			if (registration != null)
			{
				registration(re);
				re.ContainerName = previousContainerName;
			}

			return re;
		}

		public static ContentRegistration Container(this ContentRegistration re, string name, Action<ContentRegistration> registration)
		{
			if (re == null) return re;

			string previousContainerName = re.ContainerName;
			re.ContainerName = name;
			if (registration != null)
			{
				registration(re);
				re.ContainerName = previousContainerName;
			}

			return re;
		}

		public static ContentRegistration EndContainer(this ContentRegistration re)
		{
			if (re == null) return re;

			re.ContainerName = null;
			return re;
		}

		public static IDisplayRenderer Display<T>(this EditableBuilder<T> builder) where T : IEditable
		{
			return builder as IDisplayRenderer;
		}
	}

}