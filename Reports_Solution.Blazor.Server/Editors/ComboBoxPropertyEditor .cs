using DevExpress.Blazor.Legacy;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using Microsoft.AspNetCore.Components;
using Reports_Solution.Module.BusinessObjects;
using DevExpress.Blazor;
using DevExpress.ExpressApp.Blazor.Components;
using System.Reflection;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.XtraReports;

namespace Reports_Solution.Blazor.Server.Editors
{
  [PropertyEditor(typeof(string), "ComboBoxEditor",false)]
  public class CheckBoxComboBoxPropertyEditor : BlazorPropertyEditorBase
    {
    public CheckBoxComboBoxPropertyEditor(Type objectType, IModelMemberViewItem model)
       : base(objectType, model) { }

    protected override IComponentModel CreateComponentModel()
    {
      var model = new DxComboBoxModel<string, string>
      {
        Data = GetAvailableValues(),
        AllowUserInput = false,
        Value = (string)PropertyValue,
        ValueChanged = EventCallback.Factory.Create<string>(this, newValue =>
        {
          PropertyValue = newValue;
        })
      };

      return model;
    }

    private List<string> GetAvailableValues()
    {
      var reportParameter = CurrentObject as ReportParameterEditable;

      if (reportParameter?.Report == null || string.IsNullOrEmpty(reportParameter.Report.DataTypeName))
      {
        return new List<string> { "No se encontró tipo de datos" };
      }

      Type targetType = Type.GetType(reportParameter.Report.DataTypeName)
            ?? AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName == reportParameter.Report.DataTypeName);

      if (targetType == null)
        return new List<string>();

      var declaredProperties = targetType
          .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
          .Where(p =>
              p.PropertyType == typeof(string) ||
              p.PropertyType == typeof(DateTime) ||
              p.PropertyType == typeof(int) ||
              p.PropertyType == typeof(bool))
          .Select(p => p.Name)
          .ToList();

      return declaredProperties;
    }
  }
}
