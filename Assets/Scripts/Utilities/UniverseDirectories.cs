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

    public static void CreateNewDirectory(string path, string directoryName)
    {
        if (Directory.Exists(path + "/" + directoryName))
        {
            DeleteFilesAndDirectoriesInPath(path + "/" + directoryName);
            return;
        }

        Directory.CreateDirectory(path + "/" + directoryName);
    }
}
