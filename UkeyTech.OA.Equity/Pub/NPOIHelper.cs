using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.UserModel;

namespace UkeyTech.OA.Warranty
{
    /// <summary>
    /// Excel 帮助类
    /// </summary>
    internal class NPOIHelper
    {

        /// <summary>
        /// 设置数据有效性(数字)
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="checkDecimalColumns"></param>
        /// <param name="startRow"></param>
        /// <param name="lastRow"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public static void SetColDecimalValidation(ISheet sheet, int[] checkDecimalColumns, int startRow, int lastRow, string minValue, string maxValue)
        {
            //设置数据有效性
            foreach (var i in checkDecimalColumns)
            {
                CellRangeAddressList regions1 = new CellRangeAddressList(startRow, lastRow, i, i);
                DVConstraint constraint1 = DVConstraint.CreateNumericConstraint(ValidationType.DECIMAL, OperatorType.LESS_OR_EQUAL, minValue, maxValue);
                HSSFDataValidation dataValidate1 = new HSSFDataValidation(regions1, constraint1);
                dataValidate1.CreateErrorBox("error", "You must input a decimal.");
                sheet.AddValidationData(dataValidate1);
            }
        }
        /// <summary>
        /// 获取指定行的合并信息
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="sourceRowIndex"></param>
        /// <returns></returns>
        public static Dictionary<int,int> GetRowMerageSetting(ISheet sheet, int sourceRowIndex)
        {
              //get row merage regions 
            Dictionary<int, int> meragesetting = new Dictionary<int, int>();
            for (var j = 0; j < sheet.NumMergedRegions; j++) {
                var region = sheet.GetMergedRegion(j);
                if (region.FirstRow == sourceRowIndex)
                {
                    if (meragesetting.ContainsKey(region.FirstColumn) && meragesetting.ContainsValue(region.LastColumn))
                        continue;
                    meragesetting.Add(region.FirstColumn, region.LastColumn);
                }
            }
            return meragesetting;
        }

        /// <summary>
        /// 复制行格式并插入指定行数
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="startRowIndex"></param>
        /// <param name="sourceRowIndex"></param>
        /// <param name="insertCount"></param>
        public static void CopyRow(ISheet sheet, int startRowIndex, int sourceRowIndex, int insertCount)
        {
            var setting = GetRowMerageSetting(sheet, sourceRowIndex);
            CopyRow(sheet, startRowIndex, sourceRowIndex, insertCount, setting);
        }

        /// <summary>
        /// 复制行格式并插入指定行数
        /// </summary>
        /// <param name="sheet">当前sheet</param>
        /// <param name="startRowIndex">起始行位置</param>
        /// <param name="sourceRowIndex">模板行位置</param>
        /// <param name="insertCount">插入行数</param>
        public static void CopyRow(ISheet sheet, int startRowIndex, int sourceRowIndex, int insertCount, Dictionary<int, int> meragesetting)
        {            
            IRow sourceRow = sheet.GetRow(sourceRowIndex);
            int sourceCellCount = sourceRow.Cells.Count;

           
                //1. 批量移动行,清空插入区域
            sheet.ShiftRows(startRowIndex, //开始行
                                sheet.LastRowNum, //结束行
                                insertCount, //插入行总数
                                true,        //是否复制行高
                                false        //是否重置行高
                                );

            for (int i = startRowIndex; i < startRowIndex + insertCount; i++)
            {
                IRow targetRow = null;
                ICell sourceCell = null;
                ICell targetCell = null;

                targetRow = sheet.CreateRow(i);
                targetRow.Height = sourceRow.Height;//复制行高

                for (int m = sourceRow.FirstCellNum; m < sourceRow.LastCellNum; m++)
                {
                    sourceCell = sourceRow.GetCell(m);
                    if (sourceCell == null)
                        continue;
                    targetCell = targetRow.CreateCell(m);
                    targetCell.CellStyle = sourceCell.CellStyle;//赋值单元格格式
                    targetCell.SetCellType(sourceCell.CellType);

                  
                    //以下为复制模板行的单元格合并格式
                    if (sourceCell.IsMergedCell && meragesetting !=null &&  meragesetting.ContainsKey(sourceCell.ColumnIndex))
                    {

                        sheet.AddMergedRegion(new CellRangeAddress(i, i, sourceCell.ColumnIndex, meragesetting[sourceCell.ColumnIndex]));
                    }
                }
            }
        }
    }
}
