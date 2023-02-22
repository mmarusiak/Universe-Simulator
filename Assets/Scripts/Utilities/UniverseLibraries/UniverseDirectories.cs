using System;
using System.Collections.Generic;
using System.IO;

public static class UniverseDirectories 
{
    public static void DeleteFilesAndDirectoriesInPath(string path)
    {
        DirectoryInfo di = new DirectoryInfo(path);

        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete(); 
        }
        foreach (DirectoryInfo dir in di.GetDirectories())
        {
            dir.Delete(true); 
        }
    }

    public static void CreateNewDirectory(string path, string directoryName = "")
    {
        string fullPath = path;
        if (directoryName != "") fullPath += "/" + directoryName;
        if (Directory.Exists(fullPath))
        {
            DeleteFilesAndDirectoriesInPath(fullPath);
            return;
        }

        Directory.CreateDirectory(fullPath);
    }

    public static void CreateDirectoryIfNotExists(string path, string directoryName = "")
    {
        string fullPath = path;
        if (directoryName != "") fullPath += "/" + directoryName;
        if (Directory.Exists(fullPath)) return;
        Directory.CreateDirectory(fullPath);
    }
    
    public static void RenameDirectory(string oldPath, string newPath) => Directory.Move(oldPath, newPath);
    
    // https://stackoverflow.com/a/18321162/13786856
    public static String[] GetFilesFromDirectory(String searchFolder, String[] filters, bool isRecursive)
    {
        List<String> filesFound = new List<String>();
        var searchOption = isRecursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        foreach (var filter in filters)
        {
            filesFound.AddRange(Directory.GetFiles(searchFolder, String.Format("*.{0}", filter), searchOption));
        }
        return filesFound.ToArray();
    }

    public static String[] GetFoldersInDirectory(string path)
    {
        string[] folders = Directory.GetDirectories(path);
        for (int i = 0; i < folders.Length; i++)
        {
            folders[i] = Path.GetFileName(folders[i]);
        }
        return folders;
    }

    public static DateTime LastTimeModified(string filePath)
    {
        return File.GetLastWriteTime(filePath);
    }
}
