using Filemanager.Infrastructure;

namespace Filemanager{
    internal class UI{
        private  readonly ICommandProvider _commandProvider;
        private readonly IHost _host;

        public UI(ICommandProvider commandProvider, IHost host){
            _commandProvider = commandProvider;
            _host = host;
            
        }

        public void Run(){
            _host.WriteLine("FileManager is running.");
            _host.WriteLine("> ");
        }
    }
}