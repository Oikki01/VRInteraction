using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using NAudio.Wave;
using UnityEngine;
using UnityEngine.UI;

public class FileIOUtil
{
    public static bool LoadSpriteByIO(string url, out Sprite sprite, out Vector2 imgSize)
    {
        imgSize = Vector2.zero;
        sprite = null;
        if (!new FileInfo(url).Exists)
        {
            UnityEngine.Debug.Log("文件不存在");
            return false;
        }

        FileStream fileStream = new FileStream(url, FileMode.Open, FileAccess.Read);
        byte[] array = new byte[fileStream.Length];
        fileStream.Read(array, 0, (int)fileStream.Length);
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;
        Texture2D texture2D = new Texture2D(0, 0);
        texture2D.LoadImage(array);
        imgSize = new Vector2(texture2D.width, texture2D.height);
        sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
        return true;
    }

    public static bool LoadSpriteByIO(string url, Image img, int width, int height)
    {
        if (!new FileInfo(url).Exists)
        {
            UnityEngine.Debug.Log("文件不存在");
            return false;
        }

        FileStream fileStream = new FileStream(url, FileMode.Open, FileAccess.Read);
        byte[] array = new byte[fileStream.Length];
        fileStream.Read(array, 0, (int)fileStream.Length);
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;
        Texture2D texture2D = new Texture2D(width, height);
        texture2D.LoadImage(array);
        Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
        img.sprite = sprite;
        return true;
    }

    public static void SaveLargeImage(Texture2D texture2D, string fileName)
    {
        byte[] bytes = texture2D.EncodeToPNG();
        File.WriteAllBytes(fileName, bytes);
    }

    public static void SaveLargeImageToJGP(Texture2D texture2D, string fileName)
    {
        byte[] bytes = texture2D.EncodeToJPG();
        File.WriteAllBytes(fileName, bytes);
    }

    public static IEnumerator LoadMusicMp3(string path_Music, string cachePath, Action<AudioClip> loadComplete)
    {
        Mp3FileReader sourceProvider = new Mp3FileReader(File.Open(path_Music, FileMode.Open));
        WaveFileWriter.CreateWaveFile(cachePath, sourceProvider);
        WWW w = new WWW("file://" + cachePath);
        yield return w;
        loadComplete(w.GetAudioClip());
    }

    public static IEnumerator LoadMusicPaths(string path_Music, Action<AudioClip> loadComplete)
    {
        WWW www = new WWW(path_Music);
        yield return www;
        www.GetAudioClip();
        loadComplete(www.GetAudioClip());
    }

    public static bool CopyToFile(string sourceName, string folderPath)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string fileName = Path.GetFileName(sourceName);
        string destFileName = Path.Combine(folderPath, fileName);
        FileInfo fileInfo = new FileInfo(sourceName);
        if (!fileInfo.Exists)
        {
            return false;
        }

        fileInfo.CopyTo(destFileName, overwrite: true);
        return true;
    }

    public static void DeleteFlle(FileAttributes fileAttributes, string fullName)
    {
        switch (fileAttributes)
        {
            case FileAttributes.Archive:
                {
                    FileInfo fileInfo = new FileInfo(fullName);
                    if (fileInfo.Exists)
                    {
                        fileInfo.Delete();
                    }

                    break;
                }
            case FileAttributes.Directory:
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(fullName);
                    if (directoryInfo.Exists)
                    {
                        directoryInfo.Delete(recursive: true);
                    }

                    break;
                }
        }
    }

    public static bool CreateFolder(string fullName, string newFolderName, bool isExistsDelete = false)
    {
        if (!Directory.Exists(fullName))
        {
            return false;
        }

        string path = fullName + "/" + newFolderName;
        if (Directory.Exists(path))
        {
            if (isExistsDelete)
            {
                Directory.Delete(path, recursive: true);
                Directory.CreateDirectory(path);
            }
        }
        else
        {
            Directory.CreateDirectory(path);
        }

        return true;
    }

    public static void ReNameFolder(string folderFullPath, string newName)
    {
        string text = folderFullPath.Replace("/", "\\");
        Process.Start("cmd", "/c ren " + text + " " + newName);
    }

    public static bool CopyFilesToDir(string srcPath, string destPath)
    {
        try
        {
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            FileSystemInfo[] fileSystemInfos = new DirectoryInfo(srcPath).GetFileSystemInfos();
            foreach (FileSystemInfo fileSystemInfo in fileSystemInfos)
            {
                if (fileSystemInfo is DirectoryInfo)
                {
                    if (!Directory.Exists(destPath + "\\" + fileSystemInfo.Name))
                    {
                        Directory.CreateDirectory(destPath + "\\" + fileSystemInfo.Name);
                    }

                    CopyFilesToDir(fileSystemInfo.FullName, destPath + "\\" + fileSystemInfo.Name);
                }
                else
                {
                    File.Copy(fileSystemInfo.FullName, destPath + "\\" + fileSystemInfo.Name, overwrite: true);
                }
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError(ex.Message);
            return false;
        }

        return true;
    }

    public static FileSystemInfo[] GetAllFileSystemOnFolder(string folderPath)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
        if (!directoryInfo.Exists)
        {
            return null;
        }

        return directoryInfo.GetFileSystemInfos();
    }

    public static DirectoryInfo[] GetAllFolderOnFolder(string folderPath)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
        if (!directoryInfo.Exists)
        {
            return null;
        }

        return directoryInfo.GetDirectories();
    }

    public static FileInfo[] GetAllFileOnFolder(string folderPath)
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
        if (!directoryInfo.Exists)
        {
            return null;
        }

        return directoryInfo.GetFiles();
    }

    public static string ConvertFilePath(string[] paths)
    {
        string text = "";
        if (paths.Length == 0)
        {
            return "";
        }

        text = "";
        foreach (string text2 in paths)
        {
            text += text2;
        }

        return text;
    }

    public static string GetFileOrDirectorySize(string filePath)
    {
        long num = 0L;
        num = ((!new FileInfo(filePath).Exists) ? GetDirectorySize(filePath) : GetFileSize(filePath));
        float num2 = num % 1024;
        float num3 = (float)num / 1024f;
        float num4 = (float)num / 1024f / 1024f;
        float num5 = (float)num / 1024f / 1024f / 1024f;
        if (num5 >= 1f)
        {
            return num5.ToString("F3") + " GB";
        }

        if (num4 >= 1f)
        {
            return num4.ToString("F3") + " MB";
        }

        if (num3 >= 1f)
        {
            return num3.ToString("F3") + " KB";
        }

        return num2 + " 字节";
    }

    private static long GetDirectorySize(string filePath)
    {
        long num = 0L;
        DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
        if (!directoryInfo.Exists)
        {
            return 0L;
        }

        FileInfo[] files = directoryInfo.GetFiles();
        foreach (FileInfo fileInfo in files)
        {
            num += fileInfo.Length;
        }

        DirectoryInfo[] directories = directoryInfo.GetDirectories();
        foreach (DirectoryInfo directoryInfo2 in directories)
        {
            num += GetDirectorySize(directoryInfo2.FullName);
        }

        return num;
    }

    private static long GetFileSize(string filePath)
    {
        long num = 0L;
        FileInfo fileInfo = new FileInfo(filePath);
        if (!fileInfo.Exists)
        {
            return 0L;
        }

        return num + fileInfo.Length;
    }
}