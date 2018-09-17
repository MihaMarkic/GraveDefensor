using GraveDefensor.Engine.Services.Abstract;
using GraveDefensor.Shared.Service.Abstract;

namespace GraveDefensor.Shared.Services.Implementation
{
    public sealed class InitContext : IInitContext, IObjectPoolContext
    {
        public IObjectPool ObjectPool { get; }
        public InitContext(IObjectPool objectPool)
        {
            ObjectPool = objectPool;
        }
    }
}
