using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RainController : MonoBehaviour
{
    [Range(0, 1f)]
    public float masterIntensity = 1f;
    [Range(0, 1f)]
    public float rainIntensity = 1f;
    [Range(0, 1f)]
    public float windIntensity = 1f;
    [Range(0, 1f)]
    public float fogIntensity = 1f;
    [Range(0, 1f)]
    public float lightningIntensity = 1f;
    public bool autoUpdate;

    public ParticleSystem rainPart;
    public ParticleSystem windPart;
    public ParticleSystem lightningPart;
    public ParticleSystem fogPart;

    private ParticleSystem.EmissionModule rainEmission;
    private ParticleSystem.ForceOverLifetimeModule rainForce;
    private ParticleSystem.EmissionModule windEmission;
    private ParticleSystem.MainModule windMain;
    private ParticleSystem.EmissionModule lightningEmission;
    private ParticleSystem.EmissionModule fogEmission;

    void Awake()
    {
        rainEmission = rainPart.emission;
        rainForce = rainPart.forceOverLifetime;
        windEmission = windPart.emission;
        windMain = windPart.main;
        if (lightningPart != null)
        {
            lightningEmission = lightningPart.emission;
        }
        fogEmission = fogPart.emission;

        UpdateAll();
    }

    void Update()
    {
        if (autoUpdate)
            UpdateAll();
    }

    void UpdateAll(){
        rainEmission.rateOverTime = 200f * masterIntensity * rainIntensity;
        rainForce.x = new ParticleSystem.MinMaxCurve(-25f * windIntensity * masterIntensity, (-3-30f * windIntensity) * masterIntensity);
        windEmission.rateOverTime = 5f * masterIntensity * (windIntensity + fogIntensity);
        windMain.startLifetime = 2f + 5f * (1f - windIntensity);
        windMain.startSpeed = new ParticleSystem.MinMaxCurve(15f * windIntensity, 25 * windIntensity);
        fogEmission.rateOverTime = (1f + (rainIntensity + windIntensity)*0.5f) * fogIntensity * masterIntensity;
        if (lightningPart != null)
        {
            if (rainIntensity * masterIntensity < 0.7f)
                lightningEmission.rateOverTime = 0;
            else
                lightningEmission.rateOverTime = lightningIntensity * masterIntensity * 4.0f;
        }
    }

    public void OnScaleChanged(Vector3 localScale)
    {
        var rainMain = rainPart.main;

        // the smaller the scale of the gameObject, the faster it has to move to cover the distance
        // calculated from the gravity modifier (e.g. 9.8 m/s2), so we have to multiply the gravity modifier by the local scale
        // to make the physics engine work as intended
        rainMain.gravityModifier = new ParticleSystem.MinMaxCurve(rainMain.gravityModifier.constantMin * localScale.magnitude,
                                                        rainMain.gravityModifier.constantMax * localScale.magnitude);
    }

    public void OnMasterChanged(float value)
    {
        masterIntensity = value;
        UpdateAll();
    }
    public void OnRainChanged(float value)
    {
        rainIntensity = value;
        UpdateAll();
    }
    public void OnWindChanged(float value)
    {
        windIntensity = value;
        UpdateAll();
    }
    public void OnLightningChanged(float value)
    {
        lightningIntensity = value;
        UpdateAll();
    }
    public void OnFogChanged(float value)
    {
        fogIntensity = value;
        UpdateAll();
    }
}
