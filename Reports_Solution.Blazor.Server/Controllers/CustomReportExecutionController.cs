﻿using DevExpress.Data.Filtering;
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
