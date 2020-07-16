using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Grass : MonoBehaviour
{
    private Material _material;
    private Coroutine _coroutine;
    private string _position = "_Position";
    private WaitForEndOfFrame _wait;

    private void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
        _wait = new WaitForEndOfFrame();
    }

    // private void Update()
    // {
    //     _material.SetVector(_position, GameManager.Instance.globalController.player.transform.position);
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
            _material.SetVector(_position, GameManager.Instance.globalController.player.transform.position);
            yield return _wait;
        }
    }
}