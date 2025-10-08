using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

namespace task3_elParabaloid;

class Program
{
    public static void Main()
    {
        var nativeSettings = new NativeWindowSettings()
        {
            Size = new Vector2i(800, 800),
            Title = "task3 el Parabaloid",
        };

        using var window = new Window(GameWindowSettings.Default, nativeSettings);
        window.Run();
    }
}