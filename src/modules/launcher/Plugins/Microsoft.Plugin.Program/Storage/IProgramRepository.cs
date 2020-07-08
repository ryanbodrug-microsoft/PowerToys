using System.Runtime.CompilerServices;
using Windows.ApplicationModel;

[assembly: InternalsVisibleTo("Microsoft.Plugin.Program.UnitTests")]
namespace Microsoft.Plugin.Program.Storage
{
    
    internal interface IProgramRepository
    {
        void IndexPrograms();
        void Load();
        void Save();
    }
}