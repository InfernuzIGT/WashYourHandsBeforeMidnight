using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool isSmooth;
    [Space]

    [Header ("Velocity")]
    public float speed = 5f;
    public float speedSmooth = 10f;
    [Space]
    public DialogManager dialogManager;

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

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSmooth = true;
        }

        transform.position += new Vector3(_posX, 0, _posZ);
    }

    private void Interaction()
    {
        if (Input.GetKeyDown(KeyCode.E) && dialogManager.isTriggerArea)
        {
            if (dialogManager.isPass)
            {
                dialogManager.CompleteText();
                Debug.Log ($"<b> Texto salteado </b>");
                return;
            }
            if (dialogManager.isEndConversation)
            {
                dialogManager.textUI.SetActive(false);
                dialogManager.isEndConversation = false;                
            }
            else
            {
                dialogManager.SetText();
                dialogManager.textUI.SetActive(true);
            }
        }

    }
}