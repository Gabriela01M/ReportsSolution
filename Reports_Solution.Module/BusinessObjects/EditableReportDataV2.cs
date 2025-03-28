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
using DevExpress.XtraReports.UI;

namespace Reports_Solution.Module.BusinessObjects
{
    [DomainComponent]
    public class EditableReportParameter : NonPersistentBaseObject
    {
        public Type ParameterType { get; set; }
        public string Name { get; set; }
        public string PropertyToFilter { get; set; }

        public string FilterExpression { get; set; }

        public string DefaultValue { get; set; }
        public bool Required { get; set; }
        public bool SingleSelect { get; set; }
    }


    [DomainComponent, NonPersistent]
    public class EditableReportDataV2
    {
        [Browsable(false)]
        public Guid OriginalReportOid { get; set; }
        public string DisplayName { get; set; }

        private List<EditableReportParameter> parameters;
        public List<EditableReportParameter> Parameters
        {
            get => parameters ?? (parameters = new List<EditableReportParameter>());
            set => parameters = value;
        }

        public EditableReportDataV2(ReportDataV2 reportData)
        {
            OriginalReportOid = reportData.Oid;
            DisplayName = reportData.DisplayName;
            Parameters = LoadReportParameters(reportData.Content);
        }

        private List<EditableReportParameter> LoadReportParameters(byte[] content)
        {
            if (content == null || content.Length == 0)
                return new List<EditableReportParameter>();

            List<EditableReportParameter> parametersList = new List<EditableReportParameter>();

            using (MemoryStream ms = new MemoryStream(content))
            {
                XtraReport xtraReport = new XtraReport();
                xtraReport.LoadLayoutFromXml(ms);

                foreach (var param in xtraReport.Parameters)
                {
                    parametersList.Add(new EditableReportParameter
                    {
                        ParameterType = param.Type,
                        Name = param.Name,
                        DefaultValue = param.Value?.ToString()
                    });
                }
            }

            return parametersList;
        }
    }
}