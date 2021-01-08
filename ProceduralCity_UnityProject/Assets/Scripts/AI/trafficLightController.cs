using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trafficLightController : MonoBehaviour
{
    public List<GameObject> redLights_odd;
    public List<GameObject> redLights_even;
    // Start is called before the first frame update
    void Start()
    {
        LitUp(redLights_odd);
        TurnOff(redLights_even);
    }

    // Update is called once per frame
    void Update()
    {
        if(WorldManager.timeOfDay % 1 * 10 < 5)
        {
            LitUp(redLights_even);
            TurnOff(redLights_odd);
        }
        else
        {
            LitUp(redLights_odd);
            TurnOff(redLights_even);
        }
    }

    void LitUp(List<GameObject> lights)
    {
        foreach(GameObject light in lights)
        {
            Light greenLight = light.transform.Find("greenLight").GetComponent<Light>();
            greenLight.enabled = true;
            Light redLight = light.transform.Find("redLight").GetComponent<Light>();
            redLight.enabled = false;
            // Collider
            Collider box = light.GetComponent<BoxCollider>();
            box.enabled = false;
        }
    }

    void TurnOff(List<GameObject> lights)
    {
        foreach (GameObject light in lights)
        {
            Light greenLight = light.transform.Find("greenLight").GetComponent<Light>();
            greenLight.enabled = false;
            Light redLight = light.transform.Find("redLight").GetComponent<Light>();
            redLight.enabled = true;
            // Collider
            Collider box = light.GetComponent<BoxCollider>();
            box.enabled = true;
        }
    }
}
