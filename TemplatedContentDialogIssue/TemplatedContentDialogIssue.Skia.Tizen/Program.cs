using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace TemplatedContentDialogIssue.Skia.Tizen
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new TizenHost(() => new TemplatedContentDialogIssue.App(), args);
            host.Run();
        }
    }
}
