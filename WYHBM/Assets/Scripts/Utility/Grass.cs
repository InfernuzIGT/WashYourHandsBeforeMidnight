using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Grass : MonoBehaviour
{
    private Material _material;
    private Coroutine _coroutine;
    private int hash_Position = Shader.PropertyToID("_Position");
    private WaitForEndOfFrame _wait;

    private void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
        _wait = new WaitForEndOfFrame();
    }

    // private void Update()
    // {
    //     _material.SetVector(hash_Position, GameManager.Instance.globalController.player.transform.position);
    // }

    public void EnableMovement(bool isMoving)
    {
        if (isMoving)
        {
            _coroutine = StartCoroutine(Move());
        }
        else
        {
            StopCoroutine(_coroutine);
        }
    }

    private IEnumerator Move()
    {
        while (true)
        {
            _material.SetVector(hash_Position, GameManager.Instance.globalController.player.transform.position);
            yield return _wait;
        }
    }
}