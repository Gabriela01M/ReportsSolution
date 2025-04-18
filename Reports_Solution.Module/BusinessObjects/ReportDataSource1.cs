using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;

namespace Reports_Solution.Module.BusinessObjects
{
  [DefaultClassOptions]

  public class ReportDataSource1 : BaseObject
  {
    public ReportDataSource1(Session session)
        : base(session)
    {
    }
    public override void AfterConstruction()
    {
      base.AfterConstruction();
      // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
    }


    string subDivisionName;
    SubDivision subDivision;
    DateTime date;

    public DateTime Date
    {
      get => date;
      set => SetPropertyValue(nameof(Date), ref date, value);
    }


    public SubDivision SubDivision
    {
      get => subDivision;
      set => SetPropertyValue(nameof(SubDivision), ref subDivision, value);
    }

    [NonPersistent]
    public string SubDivisionName => SubDivision?.Name;
  }
}