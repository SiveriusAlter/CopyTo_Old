using System;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;

internal class CopyToOld
{
    private static void Main(string[] args)
    {
        var Path = @ConfigurationManager.AppSettings.Get("DirectoryPath");
        var CurrentVerDirectory = @ConfigurationManager.AppSettings.Get("CurrentVersionDirectory");
        var ArchiveDirectory = @ConfigurationManager.AppSettings.Get("ArchiveDirectory");
        foreach (string SourcePath in Directory.GetDirectories(Path, CurrentVerDirectory + "*"))
        {

            //Собрать новые папки
            var DestinationPath = CreateDestDir(SourcePath, ArchiveDirectory, CurrentVerDirectory);

            //Создать идентичное дерево каталогов
            CreateTread(SourcePath, DestinationPath);

            //Скопировать все файлы. И перезаписать(если такие существуют)
            CopyFileToNewDir(SourcePath, DestinationPath);
        }
        //Переименовать старые директории, если они не от текущей даты
        RenCurDirectory(Path, CurrentVerDirectory);
    }


    static string CreateDestDir(string SourcePath, string ArchiveDirectory, string CurrentVerDirectory)
    {
        string DestinationPath = SourcePath.Replace(CurrentVerDirectory, ArchiveDirectory + "\\");
        //Проверить папку бэкапа на наличие созданных папок и создать новые папки
        string[] BackupDir = Directory.GetDirectories(ArchiveDirectory);
        if (BackupDir.Contains(DestinationPath))
        {
            for (int i = 0; BackupDir.Contains(DestinationPath)==false; i++)
            {
                DestinationPath = DestinationPath + "-" + i.ToString();
            }
        }
        Directory.CreateDirectory(DestinationPath);
        return DestinationPath;

    }


    static void CreateTread(string SourcePath, string DestinationPath)
    {
        //Создать идентичное дерево каталогов
        foreach (string dirPath in Directory.GetDirectories(SourcePath, "*", SearchOption.AllDirectories))
            Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));
    }

    static void CopyFileToNewDir(string SourcePath, string DestinationPath)
    {
        //Скопировать все файлы. И перезаписать(если такие существуют)
        foreach (string newPath in Directory.GetFiles(SourcePath, "*.*", SearchOption.AllDirectories))
            File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);
    }


    static void RenCurDirectory(string Path, string CurrentVerDirectory)
    {
        //Переименовать старые директории
        string[] CurrentDirectories = Directory.GetDirectories(Path, CurrentVerDirectory + "*");
        if (CurrentDirectories.Contains(CurrentVerDirectory.Remove(Path.Length + CurrentVerDirectory.Length + 1) + DateTime.Today.ToString("d") + "*") == false)
        {
            for (int i = 0; i < CurrentDirectories.Length; i++)
            {
                if (i == 0)
                {
                    Directory.Move(CurrentDirectories[i], CurrentDirectories[i].Remove(Path.Length + CurrentVerDirectory.Length + 1) + DateTime.Today.ToString("d"));
                }
                else
                {
                    Directory.Move(CurrentDirectories[i], CurrentDirectories[i].Remove(Path.Length + CurrentVerDirectory.Length + 1) + DateTime.Today.ToString("d") + "-" + i);

                }
            }
        }
        else
        {
            Console.WriteLine("Текущая дирректория сегодня уже создавалась!");
        }
    }
}