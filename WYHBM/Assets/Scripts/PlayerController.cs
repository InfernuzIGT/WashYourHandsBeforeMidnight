using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public DialogManager dialogManager; // TODO Mariano: REMOVE

    [Header("Velocity")]
    public float speed = 5f;

    private AnimatorController _animatorController;

    // Movement Values
    private float _moveHorizontal;
    private float _moveVertical;
    private float _posX;
    private float _posZ;

    //Movement Input
    private string _inputHorizontal = "Horizontal";
    private string _inputVertical = "Vertical";

    private void Awake()
    {
        _animatorController = GetComponent<AnimatorController>();
    }

    private void Update()
    {
        Movement();
        Interaction();
    }

    private void Movement()
    {
        _moveHorizontal = Input.GetAxisRaw(_inputHorizontal);
        _moveVertical = Input.GetAxisRaw(_inputVertical);

        _posX = _moveHorizontal * speed * Time.deltaTime;
        _posZ = _moveVertical * speed * Time.deltaTime;

        transform.Translate(_posX, 0, _posZ);

        _animatorController.Movement(_moveHorizontal, _moveVertical);
    }

    private void Interaction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractDialog();
        }
    }

    // TODO Mariano: REMOVE
    private void InteractDialog()
    {
        if (dialogManager == null)
        {
            Debug.Log($"DialogManager NULL");
            return;
        }

        if (dialogManager.isTriggerArea)
        {
            if (dialogManager.isPass)
            {
                dialogManager.CompleteText();
                Debug.Log($"<b> Texto salteado </b>");
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