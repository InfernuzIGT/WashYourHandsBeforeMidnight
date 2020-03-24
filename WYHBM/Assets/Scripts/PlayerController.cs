using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public bool isSmooth;
    [Space]
    public float speed = 5f;
    public float speedSmooth = 10f;
    public DialogManager dialogManager;
    public static BoxCollider NPCBox;

    private float _moveHorizontal;
    private float _moveVertical;

    private float _posX;
    private float _posZ;

    private void Update()
    {
        Movement();
        Interaction();
    }

    private void Movement()
    {
        if (isSmooth)
        {
            _moveHorizontal = Input.GetAxis("Horizontal");
            _moveVertical = Input.GetAxis("Vertical");

            _posX = _moveHorizontal * speedSmooth * Time.deltaTime;
            _posZ = _moveVertical * speedSmooth * Time.deltaTime;
        }
        else
        {
            _moveHorizontal = Input.GetAxisRaw("Horizontal");
            _moveVertical = Input.GetAxisRaw("Vertical");

            _posX = _moveHorizontal * speed * Time.deltaTime;
            _posZ = _moveVertical * speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSmooth = true;
        }

        transform.position += new Vector3(_posX, 0, _posZ);
    }

    private void Interaction()
    {
        // TODO Marco: Remove possibilty of spam interact and break code
        if (Input.GetKey(KeyCode.E))
        {
            dialogManager.SetText();
        }
    }

    public void OnTriggerEnter(Collider NPCBox)
    {
        Interaction();
    }
}