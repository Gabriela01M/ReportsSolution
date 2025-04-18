using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using DevExpress.ExpressApp.Editors;

namespace Reports_Solution.Module.BusinessObjects
{
    public enum ParameterType
    {
        [XafDisplayName("Yes/No")]
        YesNo = 1,
        List = 2,
        String = 3,
        Number = 4,
        DateTime = 5,

    }

    public class ReportParameterEditable : BaseObject
    { 
        public ReportParameterEditable(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
        }


    string options;
    string propertyParameterValue;
    ReportDataEditable report;
    bool singleSelect;
    bool required;
    string filterExpression;
    string propertyToFilter;
    string name;
    ParameterType parameterType;

    [Association("ReportDataEditable-Parameters")]
    public ReportDataEditable Report
    {
      get => report;
      set => SetPropertyValue(nameof(Report), ref report, value);
    }

    public ParameterType ParameterType
    {
      get => parameterType;
      set => SetPropertyValue(nameof(ParameterType), ref parameterType, value);
    }


    [Size(SizeAttribute.DefaultStringMappingFieldSize)]
    public string Name
    {
      get => name;
      set => SetPropertyValue(nameof(Name), ref name, value);
    }


    [Size(SizeAttribute.DefaultStringMappingFieldSize)]
    public string PropertyParameterValue
    {
      get => propertyParameterValue;
      set => SetPropertyValue(nameof(PropertyParameterValue), ref propertyParameterValue, value);
    }

    [Size(SizeAttribute.DefaultStringMappingFieldSize)]
    [DataSourceProperty("AvailableProperties")]
    [EditorAlias("ComboBoxEditor")]
    public string PropertyToFilter
    {
      get => propertyToFilter;
      set => SetPropertyValue(nameof(PropertyToFilter), ref propertyToFilter, value);
    }


    [Size(SizeAttribute.DefaultStringMappingFieldSize)]
    public string FilterExpression
    {
      get => filterExpression;
      set => SetPropertyValue(nameof(FilterExpression), ref filterExpression, value);
    }

    public bool Required
    {
      get => required;
      set => SetPropertyValue(nameof(Required), ref required, value);
    }


    public bool SingleSelect
    {
      get => singleSelect;
      set => SetPropertyValue(nameof(SingleSelect), ref singleSelect, value);
    }


    [NonPersistent]

    public List<string> AvailableProperties
    {
      get
      {
        if (Report == null || string.IsNullOrEmpty(Report.DataTypeName))
          return new List<string>();

        Type targetType = Type.GetType(Report.DataTypeName)
            ?? AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.FullName == Report.DataTypeName);

        if (targetType == null)
          return new List<string>();

        return targetType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Where(p =>
                p.PropertyType == typeof(string) ||
                p.PropertyType == typeof(DateTime) ||
                p.PropertyType == typeof(int) ||
                p.PropertyType == typeof(bool))
            .Select(p => p.Name)
            .ToList();
      }
    }
  }
}