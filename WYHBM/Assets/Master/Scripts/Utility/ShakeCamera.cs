using System.Collections;
using Cinemachine;
using Events;
using UnityEngine;

namespace GameMode.Combat
{
	public class ShakeCamera : MonoBehaviour
	{
		[SerializeField] private CombatConfig _combatConfig = null;

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
				_virtualCameraNoise.m_AmplitudeGain = _combatConfig.shakeAmplitude;
				_virtualCameraNoise.m_FrequencyGain = _combatConfig.shakeFrequency;

				_shakeElapsedTime -= Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}

			_virtualCameraNoise.m_AmplitudeGain = 0f;
			_shakeElapsedTime = 0f;
			StopCoroutine(cShake());
		}

	}
}