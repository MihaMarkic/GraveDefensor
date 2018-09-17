using GraveDefensor.Engine.Services.Abstract;

namespace GraveDefensor.Shared.Service.Abstract
{
    public interface IInitContext
    {
        IObjectPool ObjectPool { get; }
    }
}
