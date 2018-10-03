using System;
using System.Collections.Generic;
using System.Data;
using Ecard.Models;
using Ecard.Services;
using Microsoft.Practices.Unity;

namespace Ecard.Mvc.ViewModels
{
    public class EcardModelReportRequest : EcardModel, IQueryRequest, IPageOfList
    {
        private Dictionary<string, object> _parameters = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public EcardModelReportRequest()
        {
            PageIndex = 1;
            PageSize = 10;
        }

        [NoRender, Dependency]
        public DatabaseInstance DatabaseInstance { get; set; }
        //[Dependency, NoRender]
        //public IReportService ReportService { get; set; }
        protected virtual void OnReady()
        {

        }
        public void Ready()
        {
            OnReady();
            //this._table = GetTable();
        }

        //private DataTable GetTable()
        //{
        //    var columns = GetColumns();

        //    var table = ReportService.GetReport(GetSql("sql"), _parameters, (PageIndex - 1) * PageSize, PageSize, OrderBy);

        //    int summaryCount = Convert.ToInt32(Localize("summary.count", "0"));
        //    for (int i = 0; i < summaryCount; i++)
        //    {
        //        MergeSummary("summary" + (i + 1), table);
        //    }
        //    this.TotalItemCount = ReportService.GetCount(GetSql("sql"), _parameters);
        //    this.TotalPageCount = (int)Math.Ceiling(TotalItemCount / (double)PageSize);

        //    for (int i = table.Columns.Count - 1; i >= 0; i--)
        //    {
        //        if (columns.Length <= i || string.IsNullOrWhiteSpace(columns[i]))
        //            table.Columns.RemoveAt(i);
        //        else
        //            table.Columns[i].Caption = columns[i];
        //    }
        //    return table;
        //}

        //private void MergeSummary(string sqlName, DataTable theTable)
        //{
        //    var sql = GetSql(sqlName + ".sql");
        //    if (!string.IsNullOrWhiteSpace(sql))
        //    {
        //        var table = ReportService.GetReport(sql, _parameters, 0, 1, OrderBy);
        //        if (table.Rows.Count == 0)
        //            return;
        //        var row = theTable.NewRow();
        //        for (int i = 0; i < theTable.Columns.Count; i++)
        //        {
        //            row[i] = table.Rows[0][i];
        //            if (i == 0)
        //                row[i] = Localize(sqlName + ".title");
        //        }
        //        theTable.Rows.Add(row);
        //    }
        //}

        protected virtual string[] GetColumns()
        {
            var columns = Localize("columns");
            return columns.Split(new[] { ',' }, StringSplitOptions.None);
        }

        protected virtual string GetSql(string sqlName)
        {
            return Localize(sqlName);
        }

        [Hidden]
        public string OrderBy { get; set; }

        [Hidden]
        public int PageIndex { get; set; }

        [Hidden]
        public int PageSize { get; set; }


        [NoRender]
        public int TotalItemCount { get; set; }

        [NoRender]
        public int TotalPageCount { get; set; }

        private DataTable _table;

        [NoRender]
        public DataTable Table
        {
            get
            {

                return _table;
            }
        }

        protected void SetParameter(string k, object v)
        {
            _parameters.Add(k, v);
        }
    }
}