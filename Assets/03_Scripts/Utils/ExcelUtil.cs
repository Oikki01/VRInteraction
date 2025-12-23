using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;
using UnityEngine;

public class ExcelUtil
{
    public static object[,] ReadExcelRange(string path)
    {
        using ExcelPackage excelPackage = new ExcelPackage(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read));
        int row = excelPackage.Workbook.Worksheets[1].Dimension.End.Row;
        int column = excelPackage.Workbook.Worksheets[1].Dimension.End.Column;
        object[,] array = new object[row - 1, column];
        ExcelRange cells = excelPackage.Workbook.Worksheets[1].Cells;
        for (int i = 2; i <= row; i++)
        {
            for (int j = 1; j <= column; j++)
            {
                array[i - 2, j - 1] = cells[i, j].Value;
            }
        }

        return array;
    }

    public static Vector3 ExcelDataToVector3(string dataStr)
    {
        Vector3 zero = Vector3.zero;
        if (string.IsNullOrEmpty(dataStr))
        {
            return zero;
        }

        string[] array = dataStr.Split(',', '，');
        if (array.Length != 3)
        {
            return zero;
        }

        zero.x = ParseFloat(array[0]);
        zero.y = ParseFloat(array[1]);
        zero.z = ParseFloat(array[2]);
        return zero;
    }

    public static List<int> ExcelDataToIntList(string dataStr)
    {
        List<int> list = new List<int>();
        if (string.IsNullOrEmpty(dataStr))
        {
            return list;
        }

        string[] array = dataStr.Split(',', '，');
        for (int i = 0; i < array.Length; i++)
        {
            list.Add(ParseInt(array[i]));
        }

        return list;
    }

    public static int ParseInt(string str)
    {
        if (!int.TryParse(str, out var result))
        {
            return 0;
        }

        return result;
    }

    public static float ParseFloat(string str)
    {
        if (!float.TryParse(str, out var result))
        {
            return 0f;
        }

        return result;
    }

    public static Vector3 StringToVector3(string str1, string str2, string str3, int digits = -1)
    {
        Vector3 zero = Vector3.zero;
        zero.x = ParseFloat(str1);
        zero.y = ParseFloat(str2);
        zero.z = ParseFloat(str3);
        if (digits >= 0)
        {
            zero.x = (float)Math.Round(zero.x, digits);
            zero.y = (float)Math.Round(zero.y, digits);
            zero.z = (float)Math.Round(zero.z, digits);
        }

        return zero;
    }
}