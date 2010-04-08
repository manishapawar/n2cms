using System;
using N2.Web.UI;
using N2.Web.UI.WebControls;
using System.Web.Mvc;
using System.Text;
using System.IO;
using N2.Definitions;

namespace N2.Web.Mvc.Html
{
    internal static class TextWriterExtensions
    {
        public static TextWriter WriteAttribute(this TextWriter writer, string attributeName, string value)
        {
            writer.Write(" {0}=\"{1}\"", attributeName, value);
            return writer;
        }
    }

	public class DroppableZoneHelper : ZoneHelper
	{

        ControlPanelState state = ControlPanelState.Hidden;

        public DroppableZoneHelper(HtmlHelper helper, string zoneName, ContentItem currentItem)
            : base(helper, zoneName, currentItem)
		{
            state = helper.ViewContext.HttpContext.ControlPanelState();
        }

        public override void Render(TextWriter writer)
        {
			if (state == ControlPanelState.DragDrop)
			{
				if (ZoneName.IndexOfAny(new[] { '.', ',', ' ', '\'', '"', '\t', '\r', '\n' }) >= 0) throw new N2Exception("Zone '" + ZoneName + "' contains illegal characters.");

				writer.Write("<div class='" + ZoneName + " dropZone'");
				writer.WriteAttribute(PartUtilities.PathAttribute, CurrentItem.Path)
					.WriteAttribute(PartUtilities.ZoneAttribute, ZoneName)
					.WriteAttribute(PartUtilities.AllowedAttribute, PartUtilities.GetAllowedNames(ZoneName, PartsAdapter.GetAllowedDefinitions(CurrentItem, ZoneName, Html.ViewContext.HttpContext.User)))
					.WriteAttribute("title", DroppableZone.GetToolTip(Context.Current.Definitions.GetDefinition(CurrentItem.GetType()), ZoneName))
					.Write(">");
				
				base.Render(writer);
				
				writer.Write("</div>");
			}
			else
				base.Render(writer);
        }

        protected override void RenderTemplate(TextWriter writer, ContentItem model)
        {			
            if (state == ControlPanelState.DragDrop)
            {
                ItemDefinition definition = Context.Current.Definitions.GetDefinition(model.GetType());

                writer.Write("<div class='" + definition.Discriminator + " zoneItem'");
                writer.WriteAttribute(PartUtilities.PathAttribute, model.Path)
                    .WriteAttribute(PartUtilities.TypeAttribute, definition.Discriminator)
                    .Write(">");

                PartUtilities.WriteTitleBar(writer, Context.Current.EditManager, definition, model);
                
                base.RenderTemplate(writer, model);

                writer.Write("</div>");
            }
            else
                base.RenderTemplate(writer, model);
        }

        public DroppableZoneHelper AllowExternalManipulation()
        {
            // TODO: Not implemented
            return this;
        }
	}
}