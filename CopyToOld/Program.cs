﻿using System;
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
        foreach (string SourcePath in Directory.GetDirectories(Path, CurrentVerDirectory + "*")) {
            DestinationPath(SourcePath, Path, ArchiveDirectory, CurrentVerDirectory);
            DestPath(Path, SourcePath, DestinationPath(SourcePath, Path, ArchiveDirectory, CurrentVerDirectory));
            CreateTread(SourcePath, DestinationPath(SourcePath, Path, ArchiveDirectory, CurrentVerDirectory));
            CopyFileToNewDir(SourcePath, DestinationPath(SourcePath, Path, ArchiveDirectory, CurrentVerDirectory));
        }

    }


    static string DestinationPath(string SourcePath, string Path, string ArchiveDirectory, string CurrentVerDirectory)
    {
        //Собрать адрес для новых папок       
        //DateTime ThisDay = DateTime.Today;
        return Path + "\\" + ArchiveDirectory + "\\" + SourcePath.Replace(Path + "\\" + CurrentVerDirectory, "");// + ThisDay.ToString("d");
    }

    static void DestPath(string Path, string SourcePath, string DestinationPath)
    {
        //Собрать новые папки
        Directory.CreateDirectory(Path.Replace(SourcePath, DestinationPath));
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
    static void RenDirectory(string Path, string CurrentVerDirectory)
    {
       string[] CurrentDirectories = Directory.GetDirectories(Path, CurrentVerDirectory + "*");

    }
    }