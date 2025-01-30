using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;

namespace RevitPlague.Commands;

[Transaction(TransactionMode.Manual)]
public class FamilyHashCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        try
        {
            // Получаем доступ к UIApplication
            UIApplication uiApp = commandData.Application;
            Application app = uiApp.Application;

            // Открываем файл семейства
            string familyPath = @"C:\_bim\_temp\RevitHashSumTest\Бойлер_01.rfa";
            Document familyDoc = app.OpenDocumentFile(familyPath);

            if (familyDoc == null)
            {
                TaskDialog.Show("Ошибка", "Не удалось открыть семейство.");
                return Result.Failed;
            }

            // Проверяем, является ли документ семейством
            FamilyManager familyManager = familyDoc.FamilyManager;
            if (familyManager == null)
            {
                TaskDialog.Show("Ошибка", "Документ не является семейством.");
                familyDoc.Close(false);
                return Result.Failed;
            }

            // Собираем параметры в список
            List<string> paramList = new List<string>();
            foreach (FamilyParameter param in familyManager.Parameters)
            {
                if (param.IsShared || param.IsReadOnly) // Исключаем ненужные параметры
                    continue;

                string paramValue = GetParameterValueAsString(param, familyManager);
                paramList.Add($"{param.Definition.Name}: {paramValue} (Type: {param.StorageType})");
            }

            // Сортируем параметры для стабильного хэша
            paramList.Sort();
            string paramData = string.Join("\n", paramList);

            // Вычисляем хэш-сумму
            string parametersHash = CalculateHash(paramData);
            TaskDialog.Show("Hash", $"Хэш параметров семейства:\n{parametersHash}");

            familyDoc.Close(false);
            return Result.Succeeded;
        }
        catch (Exception ex)
        {
            TaskDialog.Show("Ошибка", ex.Message);
            return Result.Failed;
        }
    }

    private string GetParameterValueAsString(FamilyParameter param, FamilyManager familyManager)
    {
        if (param == null) return "Null";

        switch (param.StorageType)
        {
            case StorageType.Double:
                return Math.Round(familyManager.CurrentType.AsDouble(param) ?? 0.0, 6).ToString();
            case StorageType.ElementId:
                return familyManager.CurrentType.AsElementId(param).IntegerValue.ToString();
            case StorageType.Integer:
                return familyManager.CurrentType.AsInteger(param).ToString();
            case StorageType.String:
                return familyManager.CurrentType.AsString(param) ?? "Null";
            default:
                return "Unknown";
        }
    }

    private string CalculateHash(string input)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    }
}

