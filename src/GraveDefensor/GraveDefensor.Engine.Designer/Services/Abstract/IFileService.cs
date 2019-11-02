using GraveDefensor.Engine.Designer.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GraveDefensor.Engine.Designer.Services.Abstract
{
    public interface IFileService
    {
        Task<Configuration> LoadConfigurationAsync(CancellationToken ct = default);
        Task SaveConfigurationAsync(Configuration config, CancellationToken ct = default);
    }
}
