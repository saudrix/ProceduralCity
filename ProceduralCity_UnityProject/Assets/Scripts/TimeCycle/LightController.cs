using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private Light lamp;
    // Start is called before the first frame update
    void Start()
    {
        lamp = transform.Find("lamp").GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (WorldManager.timeOfDay < 6 || WorldManager.timeOfDay > 18)
            lamp.enabled = true;
        
        else lamp.enabled = false;
    }
}
