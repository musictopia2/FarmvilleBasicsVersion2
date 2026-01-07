using Phase01IntroduceBarnAndSilo.Services.Core;

namespace Phase01IntroduceBarnAndSilo.Services.Trees;
public interface ITreeFactory
{
    TreeServicesContext GetTreeServices(FarmKey farm);
}