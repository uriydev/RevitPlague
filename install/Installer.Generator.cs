using System;
using System.Collections.Generic;
using System.IO;
using WixSharp;
using WixSharp.CommonTasks;
using File = WixSharp.File;

namespace Installer;

public static class Generator
{
    public static WixEntity[] GenerateWixEntities(IEnumerable<string> args)
    {
        var entities = new List<WixEntity>();
        foreach (var directory in args)
        {
            if (Directory.Exists(directory))
            {
                Console.WriteLine($"Обработка директории: {directory}");
                GenerateRootEntities(directory, entities);
            }
            else
            {
                Console.WriteLine($"Директория не существует: {directory}");
            }
        }
    
        return entities.ToArray();
    }

    public static void GenerateRootEntities(string directory, ICollection<WixEntity> entities)
    {
        foreach (var file in Directory.GetFiles(directory))
        {
            Console.WriteLine($"Найден файл: {file}");
            entities.Add(new File(file));
        }

        foreach (var folder in Directory.GetDirectories(directory))
        {
            var folderName = Path.GetFileName(folder);
            var entity = new Dir(folderName);
            entities.Add(entity);
            
            GenerateSubEntities(folder, entity);
        }
    }

    private static void GenerateSubEntities(string directory, Dir parent)
    {
        foreach (var file in Directory.GetFiles(directory))
        {
            Console.WriteLine($"Найден файл: {file}");
            parent.AddFile(new File(file));
        }

        foreach (var subFolder in Directory.GetDirectories(directory))
        {
            var folderName = Path.GetFileName(subFolder);
            var entity = new Dir(folderName);
            parent.AddDir(entity);
            
            GenerateSubEntities(subFolder, entity);
        }
    }
}