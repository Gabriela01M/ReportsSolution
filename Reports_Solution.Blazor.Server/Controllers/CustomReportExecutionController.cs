using DevExpress.Data.Filtering;
using DevExpress.Drawing.Printing;
using DevExpress.Drawing;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.ReportsV2.Blazor;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using Reports_Solution.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using DevExpress.Xpo;

namespace Reports_Solution.Blazor.Server.Controllers
{
  public class RunReportPlaceholder : BaseObject
  {
    public RunReportPlaceholder(Session session) : base(session) { }
  }

  // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ViewController.
  public partial class CustomReportExecutionController : ViewController<ListView>
  {
    // Use CodeRush to create Controllers and Actions with a few keystrokes.
    // https://docs.devexpress.com/CodeRushForRoslyn/403133/
    public CustomReportExecutionController()
    {
      InitializeComponent();
      //TargetObjectType = typeof(RunReportPlaceholder);
      // Target required Views (via the TargetXXX properties) and create their Actions.
      //var showReportAction = new SimpleAction(this, "MostrarReporteMaestro", PredefinedCategory.View)
      //{
      //  Caption = "Mostrar Reporte Maestro",
      //  ImageName = "Action_Export_ToPDF"
      //};
      //showReportAction.Execute += ShowReportAction_Execute;
    }

    private void ShowReportAction_Execute(object sender, SimpleActionExecuteEventArgs e)
    {
      const string masterReportName = "Simple Static Report";
      IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(ReportDataEditable));

      // Buscar si ya existe el reporte maestro
      var existingMasterReport = objectSpace.GetObjects<ReportDataEditable>()
          .FirstOrDefault(r => r.DisplayName == masterReportName);

      ReportDataV2 masterReportData;
      XtraReport masterReport;

      if (existingMasterReport != null)
      {
        // Si ya existe, cargarlo
        masterReportData = existingMasterReport;
        using var stream = new MemoryStream(masterReportData.Content);
        masterReport = new XtraReport();
        masterReport.LoadLayoutFromXml(stream);
      }
      else
      {
        // Crear reporte nuevo
        masterReport = new XtraReport()
        {
          DisplayName = masterReportName,
          Name = masterReportName.Replace(" ", string.Empty),
          PaperKind = DXPaperKind.Letter,
          Margins = new DXMargins(100, 100, 100, 100)
        };

        var detailBand = new DetailBand() { HeightF = 100 };
        masterReport.Bands.Add(detailBand);

        float currentTop = 0;

        // Obtener todos los ReportDataV2 menos el maestro
        var allReports = objectSpace.GetObjects<ReportDataEditable>()
            .Where(r => r.DisplayName != masterReportName)
            .ToList();

        foreach (var reportData in allReports)
        {
          using var subStream = new MemoryStream(reportData.Content);
          XtraReport subreport = new XtraReport();
          subreport.LoadLayoutFromXml(subStream);


          XRSubreport subreportControl = new XRSubreport()
          {
            ReportSource = subreport,
            BoundsF = new RectangleF(0, currentTop, masterReport.PageWidth - masterReport.Margins.Left - masterReport.Margins.Right, 300)
          };

          detailBand.Controls.Add(subreportControl);
          currentTop += 320; // Ajusta espacio entre subreportes
        }

        // Crear objeto ReportDataV2 y guardar
        masterReportData = objectSpace.CreateObject<ReportDataEditable>();
        masterReportData.DisplayName = masterReport.DisplayName;
        masterReportData.IsInplaceReport = false;

        using var saveStream = new MemoryStream();
        masterReport.SaveLayoutToXml(saveStream);
        masterReportData.Content = saveStream.ToArray();

        objectSpace.CommitChanges();
      }

      // Mostrar vista previa
      var reportStorage = Application.ServiceProvider.GetRequiredService<IReportStorage>();
      string handle = reportStorage.GetReportContainerHandle(masterReportData);

      var reportService = Frame.GetController<ReportServiceController>();
      reportService?.ShowPreview(handle);
    }


    protected override void OnActivated()
    {
      base.OnActivated();

      //ExecuteHostReport();

      // Perform various tasks depending on the target View.
      //var reportsController = Frame.GetController<ReportsController>();
      //if (reportsController != null)
      //{
      //  // Interceptar la acción de ejecución del reporte
      //  reportsController.ExecuteReportAction.Execute -= ExecuteReport;
      //  reportsController.ExecuteReportAction.Execute += ExecuteReport;

      //}
    }

    private void ExecuteHostReport()
    {
      const string masterReportName = "Simple Static Report";
      IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(ReportDataEditable));

      // Buscar si ya existe el reporte maestro
      var existingMasterReport = objectSpace.GetObjects<ReportDataEditable>()
          .FirstOrDefault(r => r.DisplayName == masterReportName);

      ReportDataV2 masterReportData;
      XtraReport masterReport;

      if (existingMasterReport != null)
      {
        // Si ya existe, cargarlo
        masterReportData = existingMasterReport;
        using var stream = new MemoryStream(masterReportData.Content);
        masterReport = new XtraReport();
        masterReport.LoadLayoutFromXml(stream);
      }
      else
      {
        // Crear reporte nuevo
        masterReport = new XtraReport()
        {
          DisplayName = masterReportName,
          Name = masterReportName.Replace(" ", string.Empty),
          PaperKind = DXPaperKind.Letter,
          Margins = new DXMargins(100, 100, 100, 100)
        };

        var detailBand = new DetailBand() { HeightF = 100 };
        masterReport.Bands.Add(detailBand);

        float currentTop = 0;

        // Obtener todos los ReportDataV2 menos el maestro
        var allReports = objectSpace.GetObjects<ReportDataEditable>()
            .Where(r => r.DisplayName != masterReportName)
            .ToList();

        foreach (var reportData in allReports)
        {
          using var subStream = new MemoryStream(reportData.Content);
          XtraReport subreport = new XtraReport();
          subreport.LoadLayoutFromXml(subStream);


          XRSubreport subreportControl = new XRSubreport()
          {
            ReportSource = subreport,
            BoundsF = new RectangleF(0, currentTop, masterReport.PageWidth - masterReport.Margins.Left - masterReport.Margins.Right, 300)
          };

          detailBand.Controls.Add(subreportControl);
          currentTop += 320; // Ajusta espacio entre subreportes
        }

        // Crear objeto ReportDataV2 y guardar
        masterReportData = objectSpace.CreateObject<ReportDataEditable>();
        masterReportData.DisplayName = masterReport.DisplayName;
        masterReportData.IsInplaceReport = false;

        using var saveStream = new MemoryStream();
        masterReport.SaveLayoutToXml(saveStream);
        masterReportData.Content = saveStream.ToArray();

        objectSpace.CommitChanges();
      }

      // Mostrar vista previa
      var reportStorage = Application.ServiceProvider.GetRequiredService<IReportStorage>();
      string handle = reportStorage.GetReportContainerHandle(masterReportData);

      var reportService = Frame.GetController<ReportServiceController>();
      reportService?.ShowPreview(handle);
    }

    //private void ExecuteReport(object sender, SimpleActionExecuteEventArgs e)
    //{
    //  var reportData = e.SelectedObjects.OfType<ReportDataV2>().FirstOrDefault();
    //  var editableReportData = reportData as ReportDataEditable;
    //  if (editableReportData != null)
    //  {
    //    var reportService = Frame.GetController<ReportServiceController>();
    //    if (reportService != null)
    //    {
    //      XtraReport report = ReportDataProvider.ReportsStorage.LoadReport(editableReportData);
    //      if (report != null)
    //      {
    //        ApplyFilters(report, editableReportData);

    //        var reportStorage = Application.ServiceProvider.GetRequiredService<IReportStorage>();
    //        string handle = reportStorage.GetReportContainerHandle(editableReportData);

    //        reportService.ShowPreview(handle);
    //      }
    //    }
    //  }
    //}

    private void ApplyFilters(XtraReport report, ReportDataEditable reportData)
    {
      if (reportData != null && report != null)
      {
        StringBuilder filterBuilder = new StringBuilder();

        foreach (var param in reportData.Parameters)
        {
          if (!string.IsNullOrEmpty(param.FilterExpression))
          {
            string filterCondition;

            if (param.FilterExpression.Contains("[Value]"))
            {
              filterCondition = $"{param.FilterExpression.Replace("[Value]", $"#{param.PropertyParameterValue}#")}";
            }
            else
            {
              filterCondition = $"{param.FilterExpression} '{param.PropertyParameterValue}'";
            }

            if (filterBuilder.Length > 0)
            {
              filterBuilder.Append(" AND ");
            }
            filterBuilder.Append(filterCondition);
          }
        }

        if (filterBuilder.Length > 0)
        {
          report.FilterString = filterBuilder.ToString();
          report.RequestParameters = false;

          using (MemoryStream outputMs = new MemoryStream())
          {
            report.SaveLayoutToXml(outputMs);
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
