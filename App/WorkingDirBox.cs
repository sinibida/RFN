using System;
using System.IO;

namespace Rfn.App
{
    public class WorkingDirBox : IDisposable
    {
        public string SavedWorkingDirectory { get; }

        public static WorkingDirBox Push(string path)
        {
            var box = new WorkingDirBox();
            Directory.SetCurrentDirectory(path);
            return box;
        }

        public WorkingDirBox()
        {
            SavedWorkingDirectory = Directory.GetCurrentDirectory();
        }

        public void Dispose()
        {
            Directory.SetCurrentDirectory(SavedWorkingDirectory);
        }
    }
}