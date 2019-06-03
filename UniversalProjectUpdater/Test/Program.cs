using Moonbyte.Net.UniversalProjectUpdater;
using System.Windows.Forms;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            UniversalProjectUpdater Updater = new UniversalProjectUpdater(Application.ProductName);
            Updater.CreateProject("1.0.0.0", "https://moonbyte.net/Download/Vermeer/Vermeer.zip");
        }
    }
}
