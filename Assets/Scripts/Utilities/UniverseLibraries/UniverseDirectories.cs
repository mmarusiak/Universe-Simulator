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
    
    public static void RenameDirectory(string oldPath, string newPath) => Directory.Move(oldPath, newPath);
}
