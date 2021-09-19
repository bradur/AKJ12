using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{

    [SerializeField]
    private Transform player;
    private Vector2 previousJoyAim = Vector2.left;
    private Vector3 previousMousePos = Vector2.zero;
    private float idleToCheck = 1f;
    private float idleTimeStarted = 0;
    private bool aimingMouse = true;
    private bool checkDelayInProgress = false;

    public static Crosshair main;
    private void Awake()
    {
        main = this;
    }
    void Start()
    {
        Lock();
    }

    void Update()
    {
        if (Cursor.visible)
        {
            return;
        }

        float deltaMouse = (Input.mousePosition - previousMousePos).magnitude;
        float horz = Input.GetAxis("Horizontal2");
        float vert = -Input.GetAxis("Vertical2");
        Vector2 aiming = new Vector2(horz, vert);

        if (aiming.magnitude < 0.2f && !aimingMouse && !checkDelayInProgress)
        {
            checkDelayInProgress = true;
            idleTimeStarted = Time.time;
        }
        else if (aiming.magnitude > 0.2f && aimingMouse && !checkDelayInProgress)
        {
            checkDelayInProgress = true;
            idleTimeStarted = Time.time;
        }

        if (Time.time - idleTimeStarted > idleToCheck && checkDelayInProgress)
        {
            if (!aimingMouse && deltaMouse > 1f)
            {
                aimingMouse = true;
            }
            else if (aimingMouse && aiming.magnitude > 0.2f)
            {
                aimingMouse = false;
            }
            checkDelayInProgress = false;
        }

        if (!aimingMouse)
        {
            if (aiming.magnitude < 0.5f) aiming = previousJoyAim;
            aiming = aiming.normalized; // aiming.magnitude > 1 ? aiming.normalized : aiming;
            previousJoyAim = aiming;
            Vector2 playerPos = new Vector2(player.position.x, player.position.y);
            transform.position = playerPos + aiming * 2;
        }
        else
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = worldPos;
            previousMousePos = Input.mousePosition;
        }
    }

    public void Lock()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    public void Unlock()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
