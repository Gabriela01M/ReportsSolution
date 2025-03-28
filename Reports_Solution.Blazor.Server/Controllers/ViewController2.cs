using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.XtraReports.UI;
using Reports_Solution.Module.BusinessObjects;

namespace Reports_Solution.Blazor.Server.Controllers
{
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ViewController.
    public partial class ViewController2 : ObjectViewController<DetailView, EditableReportDataV2>
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/

        SimpleAction SaveRecord;
        public ViewController2()
        {
            InitializeComponent();
            SaveRecord = new SimpleAction(this, "SaveChanges", PredefinedCategory.View);
            SaveRecord.Caption = "Save Changes";
            SaveRecord.Execute += SendEmail_Execute;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
        }

        private void SendEmail_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View.CurrentObject is EditableReportDataV2 currentObject)
            {
                var parameterObject = View.CurrentObject as EditableReportDataV2;
                IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(ReportDataV2));
                ReportDataV2 originalReport = objectSpace.GetObjectByKey<ReportDataV2>(currentObject.OriginalReportOid);

                if (originalReport == null) return;

                XtraReport report = new XtraReport();
                using (MemoryStream ms = new MemoryStream(originalReport.Content))
                {
                    report.LoadLayoutFromXml(ms);
                }

                foreach (var parameter in report.Parameters)
                {
                    var updatedParam = currentObject.Parameters.FirstOrDefault(x => x.Name == parameter.Name);
                    if (updatedParam != null)
                    {
                        parameter.Value = updatedParam.DefaultValue;
                    }
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    report.Name = parameterObject.DisplayName;
                    report.SaveLayoutToXml(ms);
                    originalReport.Content = ms.ToArray();
                }

                objectSpace.SetModified(originalReport);
                objectSpace.CommitChanges();
            }
        }

        private void SaveAction_Executing(object sender, CancelEventArgs e)
        {
            
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
