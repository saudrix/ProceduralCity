using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera topView;
    public Camera EarthView;
    // Start is called before the first frame update
    void Start()
    {
        EarthView.enabled = false;
        topView.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            topView.enabled = !topView.enabled;
            EarthView.enabled = !EarthView.enabled;
        }
    }
}
