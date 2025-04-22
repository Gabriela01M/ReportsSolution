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
    public class ReportDataEditable : ReportDataV2
    { 
        public ReportDataEditable(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
            this.HideReport = false;
        }


    bool hideReport;
    [Browsable(false)]
    public bool HideReport
    {
      get => hideReport;
      set => SetPropertyValue(nameof(HideReport), ref hideReport, value);
    }

    [Association("ReportDataEditable-Parameters"), DevExpress.Xpo.Aggregated]
        public XPCollection<ReportParameterEditable> Parameters
        {
            get
            {
                return GetCollection<ReportParameterEditable>(nameof(Parameters));
            }
        }
    }
}