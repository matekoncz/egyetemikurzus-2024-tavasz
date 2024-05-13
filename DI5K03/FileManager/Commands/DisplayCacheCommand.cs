using Filemanager.Infrastructure;
using Filemanager.Model;

namespace Filemanager.Commands
{
    internal class DisplayCacheCommand : ICommand
    {
        public string Name => "cache";

        public async Task ExecuteAsync(IHost host, string[] args, Cache cache)
        {
            host.WriteLine("target_dir: " + cache.Target_dir);
            foreach (FolderDef folderDef in cache.Stored_folderdefs)
            {
                host.WriteLine("Folder " + folderDef.Name);
                foreach (string extension in folderDef.Types)
                {
                   await Task.Factory.StartNew(()=>host.WriteLine("    *." + extension));
                }
            }
        }
    }
}