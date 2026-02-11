using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] Light flashlight;
    bool isOn = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && flashlight)
        {
            isOn = !isOn;
            flashlight.enabled = isOn;
        }
    }
}