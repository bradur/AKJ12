using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{

    public static Crosshair main;
    private void Awake() {
        main = this;
    }
    void Start()
    {
        Lock();
    }

    void Update()
    {
        if (Cursor.visible) {
            return;
        }
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = worldPos;
    }

    public void Lock() {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    public void Unlock() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
