using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.DocumentServices.ServiceModel.DataContracts;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.ReportsV2.Blazor;
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
    public partial class ViewController1 : ObjectViewController<ListView, ReportDataV2>
    {
        // Use CodeRush to create Controllers and Actions with a few keystrokes.
        // https://docs.devexpress.com/CodeRushForRoslyn/403133/
        public ViewController1()
        {
            InitializeComponent();
           
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            BlazorEditReportController editReportController = Frame.GetController<BlazorEditReportController>();
            if (editReportController != null)
            {
                editReportController.EditReportAction.Execute += EditAction_Execute;
            }

           
        }

        private void EditAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (e.SelectedObjects.Count == 0) return;

            ReportDataV2 selectedReport = e.SelectedObjects.Cast<ReportDataV2>().FirstOrDefault();
            if (selectedReport == null) return;

            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(ReportDataV2));

            // Crear el objeto no persistente basado en el reporte original
            EditableReportDataV2 editableReport = new EditableReportDataV2(selectedReport);

            DetailView detailView = Application.CreateDetailView(objectSpace, editableReport, true);
            e.ShowViewParameters.CreatedView = detailView;
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
