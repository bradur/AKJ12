using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackRoutine : MonoBehaviour
{

    public enum Type
    {
        DUMB_BLASTER,
        STRAFER
    }

    public static AttackRoutine addToGameObject(AIController controller, Type type)
    {
        switch (type)
        {
            case Type.DUMB_BLASTER:
                var blasterRoutine = controller.gameObject.AddComponent<DumbBlasterRoutine>();
                return blasterRoutine;
            case Type.STRAFER:
                Debug.LogError("Unimplemented AttackRoutine type: " + type);
                return null;
            default:
                Debug.LogError("Unimplemented AttackRoutine type: " + type);
                return null;
        }
    }
}
