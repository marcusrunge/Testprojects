using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseManager.Interfaces;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Ribbon;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;

namespace ExcelSqlAddIn
{
    public partial class Ribbon
    {
        IDataBaseService _databaseService;
        IDatabaseSettings _dataBaseSettings;
        int rowCorrection;
        private void Ribbon_Load(object sender, RibbonUIEventArgs e)
        {
            _databaseService = ServiceLocator.Current.GetInstance<IDataBaseService>();
            _dataBaseSettings = ServiceLocator.Current.GetInstance<IDatabaseSettings>();
            for (int i = 2; i <= 10; i++)
            {
                RibbonDropDownItem ribbonDropDownItem = Globals.Factory.GetRibbonFactory().CreateRibbonDropDownItem();
                ribbonDropDownItem.Label = i.ToString();
                rowCorrectionDropDown.Items.Add(ribbonDropDownItem);
            }
            rowCorrection = 2;
        }

        private async void sqlTransferButton_Click(object sender, RibbonControlEventArgs e) => await TransferWorksheetToDatabase();

        private async Task TransferWorksheetToDatabase()
        {
            var dictionary = new Dictionary<string, Tuple<int, int, Type>>();
            Microsoft.Office.Tools.Excel.Worksheet worksheet = Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet);
            var excelApplication = System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application") as Excel.Application;
            worksheet.CalculateMethod();
            Range selectedRange = (excelApplication.Selection as Range);
            var areas = selectedRange.Areas;
            for (int i = 1; i <= areas.Count; i++)
            {
                if (areas.Item[i].Rows.Count > 1 && !areas.Item[i].MergeCells)
                {
                    MessageBox.Show("Only one cell per column allowed!", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            var dataTable = new System.Data.DataTable((Globals.ThisAddIn.Application.ActiveSheet as Excel.Worksheet).Name);
            var cellCount = selectedRange.Cells.Count > selectedRange.EntireColumn.Areas.Count ? selectedRange.Cells.Count : selectedRange.EntireColumn.Areas.Count;
            var areaCount = areas.Count;
            bool useArea;
            useArea = areaCount > 1 ? true : false;
            int loops = useArea ? areaCount : cellCount;

            for (int i = 1; i <= loops; i++)
            {
                string columnName = String.Empty;
                int columnNameRow;
                int columnNameColumn;

                if (useArea)
                {
                    if (areas.Item[i].MergeCells) columnName = (areas.Item[i].Cells[1, 1]).Value;
                    else columnName = areas.Item[i].Value;
                    columnNameRow = areas.Item[i].Row;
                    columnNameColumn = areas.Item[i].Column;
                }
                else
                {
                    columnNameColumn = (selectedRange.Cells[i] as Range).Column;
                    columnNameRow = (selectedRange.Cells[i] as Range).Row;
                    columnName = (selectedRange.Cells[i] as Range).Value;
                }

                var dataType = ResolveType(worksheet.Cells[rowCorrection, columnNameColumn].Value, columnName);
                if (dataType == null) return;

                try
                {
                    dictionary.Add(columnName, new Tuple<int, int, Type>(columnNameRow, columnNameColumn, dataType));
                }
                catch (ArgumentException)
                {
                    string newColumnName = columnName + "2";
                    int counter = 2;
                    while (dictionary.ContainsKey(newColumnName))
                    {
                        newColumnName = columnName + counter;
                        counter++;
                    }
                    dictionary.Add(newColumnName, new Tuple<int, int, Type>(columnNameRow, columnNameColumn, dataType));
                    columnName = newColumnName;
                }

                dataTable.Columns.Add(new DataColumn()
                {
                    DataType = dataType,
                    ColumnName = columnName,
                    Caption = columnName,
                    AutoIncrement = false,
                    ReadOnly = false,
                    Unique = false
                });
            }

            _dataBaseSettings.TableName = (Globals.ThisAddIn.Application.ActiveSheet as Excel.Worksheet).Name;

            try
            {
                excelApplication.Cursor = XlMousePointer.xlWait;
                await _databaseService.CreateTable(dictionary);
                for (int k = rowCorrection; k <= worksheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing).Row; k++)
                {
                    var row = dataTable.NewRow();
                    for (int i = 0; i < dictionary.Count; i++)
                    {
                        var cellValue = Parse(worksheet.Cells[k, dictionary.ElementAt(i).Value.Item2].Value, dictionary.ElementAt(i).Value.Item3);
                        row[dictionary.ElementAt(i).Key] = cellValue;
                    }
                    dataTable.Rows.Add(row);
                    await _databaseService.Insert(dataTable);
                    dataTable.Clear();
                    rowCounterLlabel.Label = k.ToString();
                }
                excelApplication.Cursor = XlMousePointer.xlDefault;
            }
            catch (Exception exception)
            {
                excelApplication.Cursor = XlMousePointer.xlDefault;
                MessageBox.Show(exception.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private object Parse(object value, Type type)
        {
            try
            {
                if (type == typeof(int)) return int.Parse(value.ToString());
                if (type == typeof(decimal)) return decimal.Parse(value.ToString());
                if (type == typeof(DateTime)) return DateTime.Parse(value.ToString());
                return value.ToString();
            }
            catch (Exception)
            {
                return DBNull.Value;
            }
        }

        private async void updateSqlButton_Click(object sender, RibbonControlEventArgs e)
        {
            _dataBaseSettings.TableName = (Globals.ThisAddIn.Application.ActiveSheet as Excel.Worksheet).Name;
            var columnList = await _databaseService.GetColumnList();
        }

        private void databaseSetupButton_Click(object sender, RibbonControlEventArgs e)
        {
            var excelApplication = System.Runtime.InteropServices.Marshal.GetActiveObject("Excel.Application") as Excel.Application;
            _dataBaseSettings.TableName = (Globals.ThisAddIn.Application.ActiveSheet as Excel.Worksheet).Name;
            _dataBaseSettings.DatabaseName = Globals.ThisAddIn.Application.ActiveWorkbook.Name.Replace(".xlsx", "");
            var databaseSetupForm = new DatabaseSetupForm();
            databaseSetupForm.BusyEvent += (busyEvent, busyEventArgs) =>
            {
                if ((busyEventArgs as BusyEventArgs).IsBusy) excelApplication.Cursor = XlMousePointer.xlWait;
                else excelApplication.Cursor = XlMousePointer.xlDefault;
            };
            databaseSetupForm.Show();
        }

        Type ResolveType(object o, string columnName)
        {
            try
            {
                bool punctuationPresent = false;
                if (o.ToString().Contains(",") || o.ToString().Contains(".")) punctuationPresent = true;
                if (!punctuationPresent && int.TryParse(o.ToString(), out int intResult)) return typeof(int);
                if (punctuationPresent && decimal.TryParse(o.ToString(), out decimal doubleResult)) return typeof(decimal);
                if (DateTime.TryParse(o.ToString(), out DateTime dateTimeResult)) return typeof(DateTime);
                return typeof(String);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show($@"Fehlende Daten zur Typerkennung in Reihe {rowCorrection}, Spalte ""{columnName}""", null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void rowCorrectionDropDown_SelectionChanged(object sender, RibbonControlEventArgs e)
        {
            rowCorrection = int.Parse((sender as RibbonDropDown).SelectedItem.Label);
        }

        //private int FindPossibleFirstDataRow(Microsoft.Office.Tools.Excel.Worksheet worksheet)
        //{
        //    int[] possibleFirstDataRows = new int[10];
        //    for (int i = 1; i < 11; i++)
        //    {
        //        Type type = null;
        //        for (int j = 1; j < 12; j++)
        //        {
        //            Type tempType = null;
        //            if (worksheet.Cells[j, i].Value != null) tempType = ResolveType(worksheet.Cells[j, i].Value);
        //            if (type != null && type != tempType)
        //            {
        //                possibleFirstDataRows[i - 1] = j < 10 && worksheet.Cells[j, i].Value != null ? j : 2;
        //                break;
        //            }
        //            type = tempType;
        //        }
        //    }
        //    return possibleFirstDataRows.Max();
        //}       

        //private void findFirstDataRowButton_Click(object sender, RibbonControlEventArgs e)
        //{
        //    rowCorrectionDropDown.SelectedItemIndex = FindPossibleFirstDataRow(Globals.Factory.GetVstoObject(Globals.ThisAddIn.Application.ActiveWorkbook.ActiveSheet));
        //}

        private async void dropTableButton_Click(object sender, RibbonControlEventArgs e)
        {
            _dataBaseSettings.TableName = (Globals.ThisAddIn.Application.ActiveSheet as Excel.Worksheet).Name;
            await _databaseService.DropTable();
        }
    }
}
