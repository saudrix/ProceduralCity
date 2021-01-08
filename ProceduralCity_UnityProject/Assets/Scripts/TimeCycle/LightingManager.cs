using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light sun;
    [SerializeField] private LightingPresets presets;
    
    private void UpdateLighting(float timeOfDay)
    {
        RenderSettings.ambientLight = presets.AmbientColor.Evaluate(timeOfDay);
        RenderSettings.fogColor = presets.FogColor.Evaluate(timeOfDay);
        sun.color = presets.DirectionalColor.Evaluate(timeOfDay);
        sun.transform.localRotation = Quaternion.Euler(new Vector3((timeOfDay * 360f) - 90f, 170, 0));
    }

    void Update()
    {
        UpdateLighting(WorldManager.timeOfDay / 24f);
    }
}
