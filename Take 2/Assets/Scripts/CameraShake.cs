using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private float shakeDecayStart = 0.06f;
    [SerializeField]
    private float shakeIntensityStart = 0.03f;

    private float shakeDecay;
    private float shakeIntensity;

    private Vector3 originPosition;
    private Quaternion originRotation;
    private bool shaking;

    public IEnumerator Shake()
    {
        originPosition = Vector3.zero + Vector3.back * 10f;
        originRotation = Quaternion.identity;

        shakeDecay = shakeDecayStart;
        shakeIntensity = shakeIntensityStart;

        while (shakeIntensity > 0f)
        {
            transform.localPosition = originPosition + Random.insideUnitSphere * shakeIntensity;

            transform.localRotation = new Quaternion(
                originRotation.x + Random.Range(-shakeIntensity, shakeIntensity) * .2f,
                originRotation.y + Random.Range(-shakeIntensity, shakeIntensity) * .2f,
                originRotation.z + Random.Range(-shakeIntensity, shakeIntensity) * .2f,
                originRotation.w + Random.Range(-shakeIntensity, shakeIntensity) * .2f
            );

            shakeIntensity -= shakeDecay * Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        transform.localPosition = originPosition;
        transform.localRotation = originRotation;
    }
}