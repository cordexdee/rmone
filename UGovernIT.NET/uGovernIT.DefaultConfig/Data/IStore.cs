
namespace uGovernIT.DefaultConfig
{
    public interface IStore
    {
        IDefault LoaduGovernITDefault();
        IModule LoadPRSModule();
        IModule LoadTSRModule();
        IModule LoadACRModule();
        IModule LoadDRQModule();
        IModule LoadBTSModule();
        IModule LoadNPRModule();
        IPMMModule LoadPMMModule();
        IModule LoadRMMModule();
        IModule LoadINCModule();
        IModule LoadITGModule();
        IModule LoadTSKModule();
        IModule LoadSVCModule();
        IModule LoadCMTModule();
        IModule LoadAPPModule();
        IModule LoadINVModule();
        IModule LoadVNDModule();
        IModule LoadVPMModule();
        IModule LoadVFMModule();
        IModule LoadVSWModule();
        IModule LoadVSLModule();
        IModule LoadCRMModule();
        IModule LoadPLCModule();
        IModule LoadRCAModule();
        IModule LoadOPMModule();
        IModule LoadVCCModule();
        IAssetModule LoadAssetModule();
        IWIKIModule LoadWIKIModule();
        IDashboard LoadDashboard();
    }
}
