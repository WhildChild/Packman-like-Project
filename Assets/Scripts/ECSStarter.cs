using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ECSStarter : MonoBehaviour
{
    IEcsSystems _updateSystems;
    IEcsSystems _fixedUpdateSystems;
    IEcsSystems _lateUpdateSystems;

    void Start()
    {
        EcsWorld world = new EcsWorld();

        _updateSystems = new EcsSystems(world);
        _updateSystems
            .Init();

        _fixedUpdateSystems = new EcsSystems(world);
        _fixedUpdateSystems
            .Init();

        _lateUpdateSystems = new EcsSystems(world);
        _lateUpdateSystems
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
}
