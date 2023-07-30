using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ECSStarter : MonoBehaviour
{
    private IEcsSystems _updateSystems;
    private IEcsSystems _fixedUpdateSystems;
    private IEcsSystems _lateUpdateSystems;
    private IEcsSystems _initSystems;
    private DIContainer _dIContainer;

    private EcsWorld _world;

    void Start()
    {
        _dIContainer = GetComponent<DIContainer>();
         _world = new EcsWorld();

        _initSystems = new EcsSystems(_world);
        _initSystems
            .Add(new MazeGenerateSystem())
#if UNITY_EDITOR
            .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
            .Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem("InitOnly"))
#endif
            .Inject(_dIContainer.MazeSettings)
            .Init();

        _updateSystems = new EcsSystems(_world);
        _updateSystems
#if UNITY_EDITOR 
            .Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem("Update"))
#endif
            .Inject()
            .Init();

        _fixedUpdateSystems = new EcsSystems(_world);
        _fixedUpdateSystems
#if UNITY_EDITOR
            .Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem("FixedUpdate"))
#endif
            .Inject()
            .Init();

        _lateUpdateSystems = new EcsSystems(_world);
        _lateUpdateSystems
#if UNITY_EDITOR
            .Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem("LateUpdate"))
#endif
            .Inject()
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
    public void LateUpdate()
    {
        _lateUpdateSystems?.Run();
    }
    private void OnDestroy()
    {
        _initSystems.Destroy();
        _updateSystems.Destroy();
        _fixedUpdateSystems.Destroy();
        _lateUpdateSystems.Destroy();

        _world.Destroy();
    }
}
