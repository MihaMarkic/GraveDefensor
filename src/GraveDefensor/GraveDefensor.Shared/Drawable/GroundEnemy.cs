using GraveDefensor.Shared.Service.Abstract;
using Settings = GraveDefensor.Engine.Settings;

namespace GraveDefensor.Shared.Drawable
{
    public class GroundEnemy : Enemy
    {
        public void Init(IInitContext context, Settings.GroundEnemy settings, Settings.Path path)
        {
            base.Init(context, settings, path);
        }
    }
}
