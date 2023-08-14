using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SystemRegisterExtentions 
{
    public static IEcsSystems AddInitOnlySystems(this IEcsSystems systems)
    {
        return systems
            .Add(new MazeGenerateSystem())
            .Add(new PlayerSpawnSystem())
#if UNITY_EDITOR
            .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem());
#endif
    }

    public static IEcsSystems AddRunSystems(this IEcsSystems systems)
    {
        return systems
            .Add(new PlayerInputSystem())
            .Add(new InputEventsExecuteSystem())
            .Add(new MoveSystem())
            .Add(new PeaseSpawnSystem())
            .Add(new PeaseRemoveSystem())
            .Add(new CellStatusChangeSystem());
    }

    public static IEcsSystems AddFixedUpdateSystems(this IEcsSystems systems)
    {
        return systems
            .Add(new PeaseSpawnTimerSystem());
    }

    /// <summary>
    /// Системы запускаются только в Unity Editor. Используются для дебага.
    /// Добавлять только после инициализации всех остальных систем!
    /// </summary>
    /// <param name="systems"></param>
    /// <param name="systemName">Название системы</param>
    /// <returns></returns>
    public static IEcsSystems AddDebugSystems(this IEcsSystems systems, string systemName)
    {
#if UNITY_EDITOR
        systems.Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem(systemName));
#endif
        return systems;
    }
}
