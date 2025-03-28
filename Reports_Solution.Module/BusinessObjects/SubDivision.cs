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
    
    public class SubDivision : BaseObject
    { 
        public SubDivision(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
        }


        ReportDataSource1 report;
        bool inReport;
        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }


        public bool InReport
        {
            get => inReport;
            set => SetPropertyValue(nameof(InReport), ref inReport, value);
        }

        [Association("ReportDataSource1-SubDivisions")]
        public ReportDataSource1 Report
        {
            get => report;
            set => SetPropertyValue(nameof(Report), ref report, value);
        }
    }
}