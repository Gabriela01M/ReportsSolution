using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Services;
using DevExpress.Drawing.Printing;
using System.Drawing;
using System.Linq;
using DevExpress.Persistent.BaseImpl;
using System.Drawing.Printing;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.Drawing;


public class ReporteMaestro : XtraReport
{
  public static XtraReport CreateReport()
  {

    XtraReport report = new XtraReport()
    {
      Name = "SimpleStaticReport",
      DisplayName = "Simple Static Report",
      PaperKind = DXPaperKind.Letter,
      Margins = new DXMargins(100, 100, 100, 100)
    };

    DetailBand detailBand = new DetailBand()
    {
      HeightF = 25
    };
    report.Bands.Add(detailBand);

    XRLabel helloWordLabel = new XRLabel()
    {
      Text = "Hello, World!",
      Font = new DXFont("Tahoma", 20f, DXFontStyle.Bold),
      BoundsF = new RectangleF(0, 0, 250, 50),
    };
    detailBand.Controls.Add(helloWordLabel);
    return report;
  }
}

