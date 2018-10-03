using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moonlit.Exports;

namespace Ecard.Mvc.Results
{
    public class ExcelResult : FileResult
    {
        private readonly IEnumerable _items;
        private readonly Type _itemType;
        private readonly SecurityHelper _securityHelper;

        public ExcelResult(IEnumerable items, Type itemType, SecurityHelper securityHelper)
            : base("application/vnd.ms-excel")
        {
            _items = items;
            _itemType = itemType;
            _securityHelper = securityHelper;
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            var type = ViewModelDescriptor.GetTypeDescriptor(_itemType);
            Grid grid = new Grid();
            var user = _securityHelper.GetCurrentUser();
            var properties = type.Properties.Where(x => x.Show && (user == null || x.Permission.Check(user.CurrentUser))).OrderBy(x => x.Order).ToList();
            foreach (var propertyDescriptor in properties)
            {
                grid.Columns.Add(new GridColumn(propertyDescriptor.ShortName, 80));
            }

            foreach (var item in _items)
            {
                var row = grid.NewRow();
                for (int i = 0; i < properties.Count; i++)
                {
                    var propertyDescriptor = properties[i];
                    row[i].Text = string.Format(propertyDescriptor.ShortNameFormat, propertyDescriptor.GetValue(item));
                }
                grid.Rows.Add(row);
            }

            ExcelExport export = new ExcelExport();
            var bytes = export.Export(grid);
            response.BinaryWrite(bytes);
            response.End();
        }
    }
    public class ExcelTableResult : FileResult
    {
        private readonly DataTable _table;

        public ExcelTableResult(DataTable table)
            : base("application/vnd.ms-excel")
        {
            _table = table;
        }

        protected override void WriteFile(HttpResponseBase response)
        {
            Grid grid = new Grid();
            var properties = _table.Columns;
            foreach (DataColumn propertyDescriptor in properties)
            {
                grid.Columns.Add(new GridColumn(propertyDescriptor.Caption, 80));
            }

            foreach (DataRow item in _table.Rows)
            {
                var row = grid.NewRow();
                for (int i = 0; i < properties.Count; i++)
                {
                    var propertyDescriptor = properties[i];
                    row[i].Text = item[i];
                }
                grid.Rows.Add(row);
            }

            ExcelExport export = new ExcelExport();
            var bytes = export.Export(grid);
            response.BinaryWrite(bytes);
            response.End();
        }
    }
}