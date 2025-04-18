using DevExpress.Data.Filtering;
using DevExpress.Drawing.Printing;
using DevExpress.Drawing;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using Microsoft.JSInterop;
using Reports_Solution.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Reports_Solution.Blazor.Server.Controllers
{
  // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ViewController.
  public partial class ViewController3 : WindowController
  {
    IJSRuntime jsRuntime;
    // Use CodeRush to create Controllers and Actions with a few keystrokes.
    // https://docs.devexpress.com/CodeRushForRoslyn/403133/
    public ViewController3()
    {
      TargetWindowType = WindowType.Main;
      //InitializeComponent();
    }
    protected override void OnActivated()
    {
      base.OnActivated();
      // Perform various tasks depending on the target View.

      //if (View.ObjectTypeInfo.Type == typeof(DevExpress.XtraReports.UI.XtraReport))
      //{
      //  ExecuteReportSubmit();
      //}
      Frame.GetController<ShowNavigationItemController>().CustomShowNavigationItem += OnCustomShowNavigationItem;
    }

    private void ExecuteHostReport()
    {
      const string masterReportName = "Simple Static Report";
      IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(ReportDataEditable));

      // Obtener o crear el ReportDataEditable
      var masterReportData = objectSpace.GetObjects<ReportDataEditable>()
          .FirstOrDefault(r => r.DisplayName == masterReportName)
          ?? objectSpace.CreateObject<ReportDataEditable>();

      // Crear nuevo reporte
      XtraReport masterReport = new XtraReport()
      {
        DisplayName = masterReportName,
        Name = masterReportName.Replace(" ", string.Empty),
        PaperKind = DXPaperKind.Letter,
        Margins = new DXMargins(100, 100, 100, 100)
      };

      var detailBand = new DetailBand() { HeightF = 100 };
      masterReport.Bands.Add(detailBand);

      float currentTop = 0;

      // Obtener todos los subreportes (excluyendo el maestro)
      var allReports = objectSpace.GetObjects<ReportDataEditable>()
          .Where(r => r.DisplayName != masterReportName)
          .ToList();

      foreach (var reportData in allReports)
      {
        using var subStream = new MemoryStream(reportData.Content);
        XtraReport subreport = new XtraReport();
        subreport.LoadLayoutFromXml(subStream);

        // Crear una banda separada para cada subreporte
        var groupBand = new GroupHeaderBand()
        {
          HeightF = 300,
          PageBreak = PageBreak.AfterBand // Aquí está el salto de página correcto
        };

        XRSubreport subreportControl = new XRSubreport()
        {
          ReportSource = subreport,
          BoundsF = new RectangleF(0, 0, masterReport.PageWidth - masterReport.Margins.Left - masterReport.Margins.Right, 300)
        };

        groupBand.Controls.Add(subreportControl);
        masterReport.Bands.Add(groupBand);
      }


      // Guardar nuevo layout en el objeto existente
      masterReportData.DisplayName = masterReport.DisplayName;
      masterReportData.IsInplaceReport = false;

      using var saveStream = new MemoryStream();
      masterReport.SaveLayoutToXml(saveStream);
      masterReportData.Content = saveStream.ToArray();

      objectSpace.CommitChanges();

      // Mostrar vista previa
      var reportStorage = Application.ServiceProvider.GetRequiredService<IReportStorage>();
      string handle = reportStorage.GetReportContainerHandle(masterReportData);

      var reportService = Frame.GetController<ReportServiceController>();
      reportService?.ShowPreview(handle);
    }


    //protected override void OnViewControlsCreated()
    //{
    //  base.OnViewControlsCreated();
    //  // Access and customize the target View control.
    //}
    protected override void OnDeactivated()
    {
      // Unsubscribe from previously subscribed events and release other references and resources.
      base.OnDeactivated();
      
    }

    private void OnCustomShowNavigationItem(object sender, CustomShowNavigationItemEventArgs e)
    {
      if (e.ActionArguments.SelectedChoiceActionItem.Id == "ShowAllReports")
      {
        // Evita que se muestre el ListView
        e.Handled = true;
        ExecuteHostReport();
      }
    }
  }
}
