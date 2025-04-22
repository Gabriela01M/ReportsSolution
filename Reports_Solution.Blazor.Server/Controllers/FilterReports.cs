using DevExpress.CodeParser;
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
using DevExpress.Persistent.Validation;
using Reports_Solution.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reports_Solution.Blazor.Server.Controllers
{
  // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ViewController.
  public partial class FilterReports : ViewController<ListView>
  {
    // Use CodeRush to create Controllers and Actions with a few keystrokes.
    // https://docs.devexpress.com/CodeRushForRoslyn/403133/
    public FilterReports()
    {
      InitializeComponent();
      // Target required Views (via the TargetXXX properties) and create their Actions.
      TargetObjectType = typeof(ReportDataEditable);
    }
    protected override void OnActivated()
    {
      base.OnActivated();
      // Perform various tasks depending on the target View.
      if (View is ListView listView && listView.CollectionSource is CollectionSourceBase collectionSource)
      {

        collectionSource.Criteria["CustomFilter"] = CriteriaOperator.Parse("HideReport = false");
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
