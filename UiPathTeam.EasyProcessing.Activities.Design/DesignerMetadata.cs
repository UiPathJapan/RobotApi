using System.Activities.Presentation.Metadata;
using System.ComponentModel;

namespace UiPathTeam.EasyProcessing.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.AddCustomAttributes(type: typeof(InvokeProcess), attributes: new DesignerAttribute(typeof(InvokeProcessDesigner)));
            var category = new CategoryAttribute("UiPathTeam.EasyProcessing");
            builder.AddCustomAttributes(typeof(InvokeProcess), category);
            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
