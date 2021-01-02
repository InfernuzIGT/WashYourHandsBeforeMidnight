using System.Collections;
using Cinemachine;
using Events;
using UnityEngine;

public class ShakeFreeLookCamera : MonoBehaviour
{
    [SerializeField] private CombatConfig _combatConfig = null;

    private float _shakeElapsedTime = 0f;
    private CinemachineFreeLook _freeLookCamera;
    private CinemachineBasicMultiChannelPerlin _freeLookCameraNoiseA;
    private CinemachineBasicMultiChannelPerlin _freeLookCameraNoiseB;
    // private CinemachineBasicMultiChannelPerlin _freeLookCameraNoiseC;

    private void Awake()
    {
        _freeLookCamera = GetComponent<CinemachineFreeLook>();
        _freeLookCameraNoiseA = _freeLookCamera.GetRig(0).GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        _freeLookCameraNoiseB = _freeLookCamera.GetRig(1).GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
        // _freeLookCameraNoiseC = _freeLookCamera.GetRig(2).GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
    }

    private void OnEnable()
    {
        EventController.AddListener<ShakeEvent>(Shake);
    }

    private void OnDisable()
    {
        EventController.RemoveListener<ShakeEvent>(Shake);
    }

    [ContextMenu("Shake")]
    public void Shake()
    {
        StartCoroutine(cShake());
    }

    private void Shake(ShakeEvent evt)
    {
        StartCoroutine(cShake());
    }

    private IEnumerator cShake()
    {
        _shakeElapsedTime = _combatConfig.shakeDuration;

        while (_shakeElapsedTime > 0)
        {
            _freeLookCameraNoiseA.m_AmplitudeGain = _combatConfig.shakeAmplitude;
            _freeLookCameraNoiseA.m_FrequencyGain = _combatConfig.shakeFrequency;

            _freeLookCameraNoiseB.m_AmplitudeGain = _combatConfig.shakeAmplitude;
            _freeLookCameraNoiseB.m_FrequencyGain = _combatConfig.shakeFrequency;

            // _freeLookCameraNoiseC.m_AmplitudeGain = _combatConfig.shakeAmplitude;
            // _freeLookCameraNoiseC.m_FrequencyGain = _combatConfig.shakeFrequency;

            _shakeElapsedTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _freeLookCameraNoiseA.m_AmplitudeGain = 0f;
        _freeLookCameraNoiseB.m_AmplitudeGain = 0f;
        // _freeLookCameraNoiseC.m_AmplitudeGain = 0f;

        _shakeElapsedTime = 0f;
        StopCoroutine(cShake());
    }

}