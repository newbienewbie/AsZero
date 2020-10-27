using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AsZero.Core.Services
{
    internal static class ExcelSheetParser
    {
        public static IList<T> Parse<T>(ISheet chSht)
            where T : IValidatableObject, new()
        {

            var headerNames = new List<string>();
            var header = chSht.GetRow(0);
            int cellCount = header.LastCellNum;
            for (var j = 0; j < cellCount; j++)
            {
                ICell cell = header.GetCell(j);
                if (cell == null || string.IsNullOrWhiteSpace(cell.ToString()))
                {
                    headerNames.Add("");  // add empty string if cell is empty
                }
                else
                {
                    headerNames.Add(cell.ToString());
                }
            }

            var records = new List<T>();
            // Read Excel File
            for (int i = (chSht.FirstRowNum + 1); i <= chSht.LastRowNum; i++)
            {
                IRow row = chSht.GetRow(i);
                if (row == null) continue;
                if (row.Cells.All(d => d.CellType == CellType.Blank)) continue;
                var record = new T();
                var type = typeof(T);
                for (int j = 0; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        var field = row.GetCell(j).ToString();
                        var propName = headerNames[j];
                        if (String.IsNullOrWhiteSpace(propName))
                        {
                            throw new Exception($"There's a value in Cell({i},{j}) but has no header !");
                        }
                        var pi = type.GetProperty(propName);
                        if (pi == null)
                        {
                            // skip
                            continue;
                        }
                        if (pi.PropertyType.IsAssignableFrom(typeof(double)))
                        {
                            if (double.TryParse(field, out var intFiled))
                            {
                                pi.SetValue(record, intFiled);
                            }
                        }
                        else if (pi.PropertyType.IsAssignableFrom(typeof(Int32)))
                        {
                            if (int.TryParse(field, out var intFiled))
                            {
                                pi.SetValue(record, intFiled);
                            }
                        }
                        // for other colun : string
                        else
                        {
                            pi.SetValue(record, field);
                        }
                    }
                }
                var validationCtx = new ValidationContext(record);
                var validationResults = record.Validate(validationCtx).ToList();
                if (validationResults.Count > 0)
                {
                    throw new Exception($"第{i}行错误：{string.Join(";", validationResults.Select(r => $"{string.Join(",", r.MemberNames)}:{r.ErrorMessage}"))}");
                }
                records.Add(record);
            }
            return records;
        }
    }
}
