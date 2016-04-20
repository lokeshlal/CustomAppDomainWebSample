using System;
using System.Collections.Generic;
using System.IO;

namespace CustomAppDomainWebSample
{
    public class PluginManager
    {
        private List<string> plugins;
        private static PluginManager instance;
        private static object lockObj = new object();
        private List<string> dllsToIgnore;
        private string directoryToMonitor;
        private PluginManager()
        {
            plugins = new List<string>();
            dllsToIgnore = new List<string>();
            directoryToMonitor = string.Empty;

            // Add dll to ignore via configuration or hard code here
            dllsToIgnore.Add("InterfaceCommon.dll");
            // Add directory to watch via configuration or hard code here
            directoryToMonitor = @"E:\Github\CustomAppDomainWebSample\CustomAppDomainWebSample\DLLs";
        }

        public static PluginManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                        {
                            instance = new PluginManager();
                        }
                    }
                }
                return instance;
            }
        }

        public List<string> Plugins
        {
            get
            {
                return plugins;
            }
        }

        private void PreloadAssemblies()
        {
            DirectoryInfo directory = new DirectoryInfo(directoryToMonitor);
            foreach (FileInfo file in directory.GetFiles("*.dll", SearchOption.TopDirectoryOnly))
            {
                if (dllsToIgnore.Contains(Path.GetFileName(file.FullName)))
                    continue;
                if (!plugins.Contains(file.FullName))
                {
                    plugins.Add(file.FullName);
                }
            }
        }

        public void StartMonitoring()
        {
            PreloadAssemblies();

            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = directoryToMonitor;

            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Created += new FileSystemEventHandler(OnCreated);
            watcher.Deleted += new FileSystemEventHandler(OnDeleted);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);

            // Begin watching
            watcher.EnableRaisingEvents = true;
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            if (dllsToIgnore.Contains(Path.GetFileName(e.FullPath)))
                return;

            if (plugins.Contains(e.FullPath))
            {
                plugins.Remove(e.FullPath);
            }
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            if (dllsToIgnore.Contains(Path.GetFileName(e.FullPath)))
                return;

            if (!plugins.Contains(e.FullPath))
            {
                plugins.Add(e.FullPath);
            }
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            if (plugins.Contains(e.OldFullPath))
            {
                plugins.Remove(e.OldFullPath);
            }

            if (dllsToIgnore.Contains(Path.GetFileName(e.FullPath)))
                return;

            if (!plugins.Contains(e.FullPath))
            {
                plugins.Add(e.FullPath);
            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            /* Do nothing, as this dll is already part of plugins list */
        }
    }
}