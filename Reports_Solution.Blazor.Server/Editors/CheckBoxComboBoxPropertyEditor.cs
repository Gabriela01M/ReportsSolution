using DevExpress.Blazor.Legacy;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using Microsoft.AspNetCore.Components;
using Reports_Solution.Module.BusinessObjects;
using DevExpress.Blazor;

namespace Reports_Solution.Blazor.Server.Editors
{
    [PropertyEditor(typeof(string), false)]
    public class CheckBoxComboBoxPropertyEditor : BlazorPropertyEditorBase
    {
        public CheckBoxComboBoxPropertyEditor(Type objectType, IModelMemberViewItem model)
        : base(objectType, model) { }

        protected override RenderFragment CreateViewComponentCore(object dataContext)
        {
            return builder =>
            {
                builder.OpenComponent<DxComboBox<string, string>>(0);
                builder.AddAttribute(1, "Data", GetAvailableValues());
                builder.AddAttribute(2, "AllowUserInput", false);
                builder.AddAttribute(3, "ShowClearButton", true);
                builder.AddAttribute(4, "DropDownMinWidth", 300);
                builder.AddAttribute(5, "Value", PropertyValue);
                builder.AddAttribute(6, "ValueChanged", EventCallback.Factory.Create<string>(this, newValue =>
                {
                    PropertyValue = newValue; // Guarda el nuevo valor
                }));
                builder.CloseComponent();
            };
        }

        private List<string> GetAvailableValues()
        {
            return new List<string> { "Opción 1", "Opción 2", "Opción 3" }; // Datos de prueba
        }
    }
}
