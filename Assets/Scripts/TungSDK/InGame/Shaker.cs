using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("WASD/FX/Shaker")]
public class Shaker : TickBehaviour
{
    // >>> Inspector Properties
    public Transform ShakeTarget;
    [Range(0, 60)] public float MaxAngle = 5;
    [Range(0, 1)] public float ShakeIntensity = 1;
    [Range(0, 20)] public float ShakeStartIntensity = 3;
    [Range(0, 20)] public float ShakeEndIntensity = 3f;
    [Range(0, 20)] public float ShakeSpeed = 2f;
    public float ShakeDuration = 0.5f;
    public bool AwaysShaking;
    private float currentTime;

    //Shaking Runtime Properties
    private float currentShakeIntensity;
    [HideInInspector] public bool IsShaking;

    //Perlin Noise Cordinates
    private float coordX, coordY, coordZ;

    //Local Euler Rotation
    private float rotX, rotY, rotZ;
    private Vector3 shakingEulerRotation;

    public Vector3 GetShakeLocalEulerRotation { get => shakingEulerRotation; }
    public Quaternion GetShakeLocalRotation { get => Quaternion.Euler(shakingEulerRotation); }

    void Start()
    {
        coordX = Random.Range(-1000, 1000);
        coordY = Random.Range(-1000, 1000);
        coordZ = Random.Range(-1000, 1000);

        if(ShakeTarget == null) ShakeTarget = transform;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        //Clamp intensity value
        currentShakeIntensity = Mathf.Clamp(currentShakeIntensity, 0, 1);

        //Modify shake curve
        float IntensityQuadratic = currentShakeIntensity * currentShakeIntensity;

        float time = Time.time * ShakeSpeed;

        rotX = (ShakeIntensity * IntensityQuadratic) * MaxAngle * PerlinNoise(coordX, time);
        rotY = (ShakeIntensity * IntensityQuadratic) * MaxAngle * PerlinNoise(coordY, time);
        rotZ = (ShakeIntensity * IntensityQuadratic) * MaxAngle * PerlinNoise(coordZ, time);

        shakingEulerRotation.Set(rotX, rotY, rotZ);
        ShakeTarget.localEulerAngles = shakingEulerRotation;

        if(!AwaysShaking)
        {
            switch(IsShaking)
            {
                case true:
                    StartShaking();
                    break;
                case false:
                    EndShaking();
                    break;
            }
        }
        else
        {
            StartShaking();
        }

        if(currentTime < ShakeDuration)
        {
            currentTime += Time.deltaTime;
            IsShaking = true;
        }
        else
        {
            IsShaking = false;
        }
    }

    private void EndShaking()
    {
        currentShakeIntensity -= ShakeEndIntensity * Time.deltaTime;
    }

    private void StartShaking()
    {
        currentShakeIntensity += ShakeStartIntensity * Time.deltaTime;
    }

    [Button]
    public void Shake()
    {
        currentTime = 0;
        ShakeDuration = 0.5f;
    }
    /// <summary>
    /// Shake...
    /// </summary>
    public void Shake(float speed = 3, float duration = 0.5f, float startIntensity = 15, float endIntensity = 3, float maxRotationAngle = 5, float intensity = 1)
    {
        currentTime = 0;
        ShakeSpeed = speed;
        ShakeDuration = duration;
        ShakeStartIntensity = startIntensity;
        ShakeEndIntensity = endIntensity;
        MaxAngle = maxRotationAngle;
        ShakeIntensity = intensity;
    }
    public float PerlinNoise(float coordinate, float time)
    {
        return (1 - 2 * Mathf.PerlinNoise(coordinate + time, coordinate + time));
    }
}