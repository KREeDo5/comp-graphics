using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Threading.Tasks;

namespace task2;

class Program
{
    static void Main(string[] args)
    {
        var nativeWindowSettings = new NativeWindowSettings()
        {
            ClientSize = new Vector2i(1000, 600),
            Title = "lab7 task2",
            Flags = ContextFlags.Default,
            Profile = ContextProfile.Compatability,
            API = ContextAPI.OpenGL,
        };

        var gameWindowSettings = new GameWindowSettings()
        {
            UpdateFrequency = 60.0
        };

        var window = new Window(gameWindowSettings, nativeWindowSettings);
        window.Run();
    }
}