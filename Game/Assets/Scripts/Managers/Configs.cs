using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Configs : MonoBehaviour
{
    public static Configs main;
    void Awake() {
        main = this;
    }

    public InputConfig InputConfig;
}
