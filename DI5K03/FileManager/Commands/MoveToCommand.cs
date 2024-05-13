using Filemanager.Infrastructure;
using Filemanager.Model;

namespace Filemanager.Commands
{
    internal class MoveToCommand : ICommand
    {
        public string Name => "move";

        public async void ExecuteAsync(IHost host, string[] args, Cache cache)
        {
            string valid_format = "move <extension> to <folder_name>";
            if (args.Length < 4)
            {
                host.WriteLine("Not enough arguments: command must be: " + valid_format);
            }
            else
            {
                if (!args[2].Equals("to"))
                {
                    host.WriteLine("Invalid argument: command must be: " + valid_format);
                }
                else
                {
                    if (cache.Target_dir.Equals(""))
                    {
                        host.WriteLine("Can't execute command: no target directory selected yet");
                    }
                    else
                    {
                        string foldername = args[3];
                        string extension = args[1];
                        host.WriteLine("... Updating fm_config.json");
                        bool cache_was_modified = false;
                        bool folderdef_exists = false;

                        foreach (FolderDef folderdef in cache.Stored_folderdefs)
                        {
                            if (folderdef.Name.Equals(foldername))
                            {
                                if (!folderdef.Types.Contains<string>(extension))
                                {
                                    folderdef.Types = [.. folderdef.Types, extension];
                                    cache_was_modified = true;
                                }
                                folderdef_exists = true;
                                break;
                            }
                        }

                        if (!folderdef_exists)
                        {
                            FolderDef folderDef = new(foldername, [extension]);
                            cache.Stored_folderdefs = [.. cache.Stored_folderdefs, folderDef];
                            cache_was_modified = true;
                        }

                        string target = cache.Target_dir + "/fm_config.json";

                        if (cache_was_modified)
                        {
                            try
                            {
                                using (FileStream config_stream = File.Open(target, FileMode.Truncate))
                                {
                                    Serializer serializer = new();
                                    await serializer.SerializeToJson(config_stream, cache.Stored_folderdefs);
                                }
                                host.WriteLine("Config file cleared");
                            }
                            catch (Exception file_exception)
                            {
                                host.WriteLine("Exception occured while trying to open fm_config.json: " + file_exception.Message);
                            }
                        }

                    }
                }
            }
        }
    }
}