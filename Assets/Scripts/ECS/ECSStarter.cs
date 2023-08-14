using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

public class ECSStarter : MonoBehaviour
{
    private IEcsSystems _updateSystems;
    private IEcsSystems _initSystems;
    private IEcsSystems _fixedUpdateSystems;
    private DIContainer _dIContainer;

    private EcsWorld _world;

    void Start()
    {
        _dIContainer = GetComponent<DIContainer>();
        _world = new EcsWorld();

        _initSystems = new EcsSystems(_world);
        _initSystems
            .AddInitOnlySystems()
            .AddDebugSystems("Init")
            .Inject(_dIContainer.MazeSettings)
            .Inject(_dIContainer.PlayerSettings)
            .Init();

        _updateSystems = new EcsSystems(_world);
        _updateSystems
            .AddRunSystems()
            .AddDebugSystems("Run")
            .Inject(_dIContainer.MazeSettings)
            .Init();

        _fixedUpdateSystems = new EcsSystems(_world);
        _fixedUpdateSystems
            .AddFixedUpdateSystems()
            .AddDebugSystems("FixedUpdate")
            .Inject(_dIContainer.PeaseSettings)
            .Init();
    }

    public void Update()
    {
        _updateSystems?.Run();
    }

    public void FixedUpdate()
    {
        _fixedUpdateSystems?.Run();
    }


    private void OnDestroy()
    {
        _initSystems?.Destroy();
        _updateSystems?.Destroy();
        _fixedUpdateSystems?.Destroy();

        _world?.Destroy();
    }
}
