using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.DataAccess.EntityFramework;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.ReportsV2.Blazor;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.ReportsV2;
using DevExpress.Persistent.Validation;
using DevExpress.PivotGrid.DataBinding;
using DevExpress.Utils.Filtering;
using DevExpress.Xpo;
using DevExpress.XtraReports;
using DevExpress.XtraReports.Expressions;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using DevExpress.XtraRichEdit.Import.Html;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Reports_Solution.Module.BusinessObjects;

namespace Reports_Solution.Blazor.Server.Controllers
{
  // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ViewController.
 
    public partial class ParametersController : ObjectViewController<DetailView, ReportDataEditable>
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        ReportDataEditable reportData;
    IJSRuntime jsRuntime;
    ReportDataEditable CurrentObject;
    public ParametersController()
    {
            InitializeComponent();
      
    }

    protected override void OnActivated()
    {
      base.OnActivated();
      // Perform various tasks depending on the target View.

      CurrentObject = View.CurrentObject as ReportDataEditable;

      ExecuteReportSubmit();


      var modificationsController = Frame.GetController<ModificationsController>();
      if (modificationsController != null)
      {
        modificationsController.SaveAction.Executing += SaveAction_Executing;
      }
    }



    private async void ExecuteReportSubmit()
    {
      if (jsRuntime == null)
      {
        jsRuntime = (IJSRuntime)((BlazorApplication)Application).ServiceProvider.GetService(typeof(IJSRuntime));
      }

      if (jsRuntime != null)
      {
        await jsRuntime.InvokeVoidAsync("autoSubmitReport");
      }
    }

    private void SaveAction_Executing(object sender, CancelEventArgs e)
    {

      reportData = View.CurrentObject as ReportDataEditable;
      Session session = ((XPObjectSpace)ObjectSpace).Session;
      StringBuilder filterBuilder = new StringBuilder();
      if (reportData != null)
      {
        using (MemoryStream ms = new MemoryStream(reportData.Content))
        {
          XtraReport xtraReport = new XtraReport();
          xtraReport.LoadLayoutFromXml(ms);

          if (xtraReport.Parameters.Count > 0)
          {
            for (int i = xtraReport.Parameters.Count - 1; i >= 0; i--)
            {
              xtraReport.Parameters.RemoveAt(i);
            }

            xtraReport.FilterString = string.Empty;

            ObjectSpace.SetModified(xtraReport);
            ObjectSpace.CommitChanges();
          }

          foreach (var param in reportData.Parameters)
          {
            DevExpress.XtraReports.Parameters.Parameter parameter = null;

            DevExpress.XtraReports.Parameters.Parameter NewParameter = new DevExpress.XtraReports.Parameters.Parameter();
            NewParameter.Name = param.Name;

            if (param.ParameterType == Module.BusinessObjects.ParameterType.List)
            {
              NewParameter.Type = typeof(string);
              DynamicListLookUpSettings lookUpSettings = new DynamicListLookUpSettings
              {
                DataSource = xtraReport.DataSource,
                ValueMember = param.PropertyToFilter,
                DisplayMember = param.PropertyToFilter
              };

              NewParameter.LookUpSettings = lookUpSettings;


              if (param.SingleSelect)
              {
                NewParameter.MultiValue = false;
                NewParameter.Value = param.PropertyParameterValue;
              }
              else
              {
                NewParameter.MultiValue = true;
                NewParameter.Value = new string[] { param.PropertyParameterValue };
              }
            }
            else
            {
              NewParameter.Value = param.PropertyParameterValue;
              if (param.ParameterType == Module.BusinessObjects.ParameterType.YesNo)
                NewParameter.Type = typeof(bool);
              if (param.ParameterType == Module.BusinessObjects.ParameterType.String)
                NewParameter.Type = typeof(string);
              if (param.ParameterType == Module.BusinessObjects.ParameterType.Number)
                NewParameter.Type = typeof(int);
              if (param.ParameterType == Module.BusinessObjects.ParameterType.DateTime)
                NewParameter.Type = typeof(DateTime);
              NewParameter.AllowNull = param.Required;
            }

            xtraReport.Parameters.Add(NewParameter);

            if (!string.IsNullOrEmpty(param.FilterExpression))
            {
              string filterCondition;

              if (param.FilterExpression.Contains("[Value]"))
              {
                filterCondition = param.FilterExpression.Replace("[Value]", $"(?{param.Name})");
              }
              else
              {
                filterCondition = $"{param.FilterExpression} ?{param.Name}";
              }

              if (filterBuilder.Length > 0)
              {
                filterBuilder.Append(" AND ");
              }
              filterBuilder.Append(filterCondition);
            }
            if (filterBuilder.Length > 0)
            {
              xtraReport.FilterString = filterBuilder.ToString();
              xtraReport.RequestParameters = false;
            }
          }

          using (MemoryStream outputMs = new MemoryStream())
          {
            xtraReport.SaveLayoutToXml(outputMs);
            reportData.Content = outputMs.ToArray();
          }
          ObjectSpace.CommitChanges();

        }
      }
    }
        


        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
