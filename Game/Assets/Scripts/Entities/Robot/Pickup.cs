using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pickup : MonoBehaviour
{
    private float value;
    private UnityAction destroyPart;
    private RobotPartType robotPartType;

    private Character player;

    public void Initialize(float value, UnityAction destroyPart, RobotPartType robotPartType)
    {
        this.value = value;
        this.destroyPart = destroyPart;
        this.robotPartType = robotPartType;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            if (robotPartType == RobotPartType.HealthPack)
            {
                player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
                bool healthWasPicked = player.AddHealth((int)value);
                if (healthWasPicked) {
                    PoppingTextOptions poppingTextOptions = new PoppingTextOptions();
                    poppingTextOptions.Position = transform.position;
                    poppingTextOptions.Text = $"+{value} HP";
                    poppingTextOptions.Color = Color.green;
                    WorldUI.main.ShowPoppingText(poppingTextOptions);
                    destroyPart.Invoke();
                }
            }
            else if (robotPartType == RobotPartType.RobotPart)
            {

                PartPicker picker = collider.gameObject.GetComponent<PartPicker>();
                if (picker != null)
                {
                    bool collected = picker.CollectPart(value);
                    if (collected)
                        {
                        PoppingTextOptions poppingTextOptions = new PoppingTextOptions();
                        poppingTextOptions.Position = transform.position;
                        poppingTextOptions.Text = $"+1 Robot part";
                        poppingTextOptions.Color = Color.cyan;
                        WorldUI.main.ShowPoppingText(poppingTextOptions);
                        destroyPart.Invoke();
                    }
                }
            }
        }
    }
}