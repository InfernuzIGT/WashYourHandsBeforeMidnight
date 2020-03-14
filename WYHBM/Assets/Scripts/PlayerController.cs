using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 15f;

    //private AnimatorController _animatorController;

    // Movement Values
    private float _moveHorizontal;
    private float _moveVertical;
    private float _posX;
    private float _posZ;

    //Movement Input
    private string _inputHorizontal = "Horizontal";
    private string _inputVertical= "Vertical";
    private string _inputInteraction = "E";

    //private void Awake ()
    //{
    //    _animatorController = GetComponent<AnimatorController> ();
    //}

    private void Update ()
    {
        Movement ();
        Interaction ();
    }

    private void Movement ()
    {
        _moveHorizontal = Input.GetAxisRaw (_inputHorizontal);
        _moveVertical = Input.GetAxisRaw (_inputVertical);

        _posX = _moveHorizontal * speed * Time.deltaTime;
        _posZ = _moveVertical * speed * Time.deltaTime;

        transform.Translate (_posX, 0, _posZ);

        //_animatorController.Movement (_posX, _posZ);
    }

    private void Interaction ()
    {
        if (Input.GetKey (_inputInteraction))
        {
            // sphere gizmo interact w quad to open chest w colliders
            // Debug.Log ($"<b> Chest Open </b>");
        }
    }
}