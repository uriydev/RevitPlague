using System;
using System.IO;
using System.Linq;
using WixSharp;
using File = WixSharp.File;

//1. declaring an instance of the WixSharp.Project
var project = new Project();
//2. defining the compilation action with WixSharp


//3. executing the build script

#region test
// class Installer
// {
//     static void Main()
//     {
//         // Название проекта и выходной файл
//         const string projectName = "RevitPlagueInstaller";
//         const string outputFileName = "RevitPlagueInstaller.msi";
//
//         // Путь к папке с файлами плагина
//         string sourceDir = @"C:\_code\Repos\RevitPlague\source\RevitPlague\bin\Release R24\publish\Revit 2024 Release R24 addin\RevitPlague";
//
//         // Путь к файлу .addin
//         string addinFilePath = @"C:\_code\Repos\RevitPlague\source\RevitPlague\bin\Release R24\publish\Revit 2024 Release R24 addin\RevitPlague.addin";
//
//         // Получаем все файлы из папки
//         var files = Directory.GetFiles(sourceDir, "*.*", SearchOption.AllDirectories)
//             .Select(filePath => new File(filePath))
//             .ToArray();
//
//         // Добавляем файл .addin
//         var addinFile = new File(addinFilePath);
//
//         // Путь к папке Revit Addins (куда нужно установить файлы)
//         var revitAddinsFolder = new Dir(@"%AppData%\Autodesk\Revit\Addins\2024\RevitPlague", files);
//         revitAddinsFolder.Files = revitAddinsFolder.Files.Combine(addinFile); // Используем Combine для добавления файла .addin
//
//         // Создание проекта установщика
//         var project = new Project(projectName, revitAddinsFolder);
//
//         // Настройка свойств проекта
//         project.GUID = new Guid("1531e4b7-1325-424b-9b05-073ce7ae0177"); // Уникальный GUID для проекта
//         project.Version = new Version("1.0.0"); // Версия установщика
//         project.OutFileName = outputFileName; // Имя выходного файла
//         project.InstallScope = InstallScope.perUser; // Установка для текущего пользователя
//
//         // Сборка установщика
//         project.BuildMsi();
//     }
// }
#endregion