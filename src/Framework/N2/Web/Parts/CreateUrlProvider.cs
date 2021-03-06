using System;
using System.Collections.Specialized;
using System.Web;
using N2.Definitions;
using N2.Edit;
using N2.Persistence;
using N2.Web;
using N2.Engine;

namespace N2.Web.Parts
{
	[Service]
	public class CreateUrlProvider : PartsAjaxService
	{
        readonly IPersister persister;
		readonly IEditUrlManager managementPaths;
		readonly ContentActivator activator;
		readonly IDefinitionManager definitions;
		readonly ITemplateProvider[] templates;
        readonly Navigator navigator;

		public CreateUrlProvider(IPersister persister, IEditUrlManager editUrlManager, IDefinitionManager definitions, ITemplateProvider[] templates, ContentActivator activator, AjaxRequestDispatcher dispatcher, Navigator navigator)
			: base(dispatcher)
		{
            this.persister = persister;
			this.managementPaths = editUrlManager;
			this.definitions = definitions;
			this.templates = templates;
			this.activator = activator;
            this.navigator = navigator;
		}

		public override string Name
		{
			get { return "create"; }
		}

		public override NameValueCollection HandleRequest(NameValueCollection request)
		{
			NameValueCollection response = new NameValueCollection();

			var template = GetTemplate(request["discriminator"], request["template"]);
			if (template.Definition.Editables.Count > 0)
			{
				response["redirect"] = GetRedirectUrl(template, request);
				response["dialog"] = "yes";
			}
			else
			{
				response["redirect"] = request["returnUrl"];
				response["dialog"] = "no";
				CreateItem(template, request);
			}

			return response;
		}

		private void CreateItem(TemplateDefinition template, NameValueCollection request)
        {
            ContentItem parent = navigator.Navigate(request["below"]);
			
			ContentItem item = activator.CreateInstance(template.Definition.ItemType, parent);
            item.ZoneName = request["zone"];
			item["TemplateName"] = template.Name;
            
            string before = request["before"];
            if (!string.IsNullOrEmpty(before))
			{
				ContentItem beforeItem = navigator.Navigate(before);
                int newIndex = parent.Children.IndexOf(beforeItem);
                Utility.Insert(item, parent, newIndex);

                foreach (var sibling in Utility.UpdateSortOrder(parent.Children))
                    persister.Repository.Save(sibling);
			}

            persister.Save(item);
        }

		private string GetRedirectUrl(TemplateDefinition template, NameValueCollection request)
		{
			string zone = request["zone"];

			string before = request["before"];
			string below = request["below"];

			Url url;
			if (!string.IsNullOrEmpty(before))
			{
                ContentItem beforeItem = navigator.Navigate(before);
				url = managementPaths.GetEditNewPageUrl(beforeItem, template.Definition, zone, CreationPosition.Before);
			}
			else
			{
                ContentItem parent = navigator.Navigate(below);
				url = managementPaths.GetEditNewPageUrl(parent, template.Definition, zone, CreationPosition.Below);
			}

			if (!string.IsNullOrEmpty(request["returnUrl"]))
				url = url.AppendQuery("returnUrl", request["returnUrl"]);
			return url;
		}

		private TemplateDefinition GetTemplate(string discriminator, string templateName)
		{
			return templates.GetTemplate(definitions.GetDefinition(discriminator).ItemType, templateName);
			//foreach (ItemDefinition definition in definitions.GetDefinitions())
			//{
			//    if (definition.Discriminator == discriminator)
			//    {
			//        return definition;
			//    }
			//}
			//throw new N2Exception("Definition not found: " + discriminator);
		}
	}
}