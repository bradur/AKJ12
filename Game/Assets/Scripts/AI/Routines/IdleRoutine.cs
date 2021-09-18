using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IdleRoutine : MonoBehaviour
{

    public enum Type
    {
        PATROL,
        SEEK_PLAYER,
        SEEK_CENTER,
    }

    public static IdleRoutine addToGameObject(AIController controller, Type type)
    {
        switch (type)
        {
            case Type.PATROL:
                var patrolRoutine = controller.gameObject.AddComponent<PatrolRoutine>();
                patrolRoutine.Init(controller, defaultPatrolConfig());
                return patrolRoutine;
            case Type.SEEK_PLAYER:
                var seekPlayerRoutine = controller.gameObject.AddComponent<SeekTransformRoutine>();
                seekPlayerRoutine.Init(controller, AIManager.Main.Player);
                return seekPlayerRoutine;
            case Type.SEEK_CENTER:
                var seekCenterRoutine = controller.gameObject.AddComponent<SeekTransformRoutine>();
                seekCenterRoutine.Init(controller, AIManager.Main.CenterOfArena);
                return seekCenterRoutine;
            default:
                Debug.LogError("Unimplemented IdleRoutine type: " + type);
                return null;
        }
    }

    private static PatrolRoutine.PatrolConfig defaultPatrolConfig()
    {
        var config = new PatrolRoutine.PatrolConfig();
        config.PatrolAreaCenter = AIManager.Main.CenterOfArena;
        config.PatrolRadius = 10.0f;
        config.MinIdleTime = 5.0f;
        config.MaxIdleTime = 8.0f;
        return config;
    }
}

