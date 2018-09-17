using GraveDefensor.Engine.Services.Abstract;
using GraveDefensor.Engine.Services.Implementation;

namespace GraveDefensor.Shared
{
    public static class Globals
    {
        public static IObjectPool ObjectPool { get; } = new ObjectPool();
        public static bool ShowPaths { get; set; } = true;
        public static void Init()
        {

        }
    }
}
