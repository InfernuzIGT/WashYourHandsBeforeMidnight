using System.Collections;
using Cinemachine;
using Events;
using UnityEngine;

namespace GameMode.Combat
{
	public class ShakeCamera : MonoBehaviour
	{
		private float _shakeElapsedTime = 0f;
		private CinemachineVirtualCamera _virtualCamera;
		private CinemachineBasicMultiChannelPerlin _virtualCameraNoise;

		private void Awake()
		{
			_virtualCamera = GetComponent<CinemachineVirtualCamera>();
			_virtualCameraNoise = _virtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();
		}

		private void OnEnable()
		{
			EventController.AddListener<ShakeEvent>(Shake);
		}

		private void OnDisable()
		{
			EventController.RemoveListener<ShakeEvent>(Shake);
		}

		private void Shake(ShakeEvent evt)
		{
			StartCoroutine(cShake());
		}

		private IEnumerator cShake()
		{
			_shakeElapsedTime = GameData.Instance.combatConfig.shakeDuration;

			while (_shakeElapsedTime > 0)
			{
				_virtualCameraNoise.m_AmplitudeGain = GameData.Instance.combatConfig.shakeAmplitude;
				_virtualCameraNoise.m_FrequencyGain = GameData.Instance.combatConfig.shakeFrequency;

				_shakeElapsedTime -= Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}

			_virtualCameraNoise.m_AmplitudeGain = 0f;
			_shakeElapsedTime = 0f;
			StopCoroutine(cShake());
		}

	}
}