using Avalonia;
using Avalonia.Markup.Xaml;

namespace GraveDefensor.GameDesigner
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
