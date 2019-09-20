using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    [Header("Action")]
    public ACTION_TYPE actualActionType;

    [Header("Menu")]
    public bool isOpenActionMenu = false;
    public bool isOpenActionRadialMenu = false;
    public GameObject menuWeapon;
    public GameObject menuDefense;
    public GameObject menuItem;

    [Header("Layers")]
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    public LayerMask ignoreLayer;

    [Header("GameObjects")]
    public GameObject WIN;
    public GameObject LOSE;
    public GameObject Action1;
    public GameObject Action2;
    public GameObject Action3;
    public GameObject UI;

    public Image barHealthEnemy;
    public Image barHealthPlayer;
    private bool inAction = false;
    private LayerMask _actualLayer;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            OnMouseDown();
        }
        if (Input.GetMouseButtonDown(0) && inAction)
        {
            InputAttack();
            InputHeal();
        }
    }
    private void Start()
    {
        _actualLayer = ignoreLayer;
    }

    public void CloseActionMenu()
    {
        if (isOpenActionMenu)
        {
            isOpenActionMenu = false;
            menuWeapon.SetActive(false);
            menuItem.SetActive(false);
            menuDefense.SetActive(false);
        }
    }

    public void CloseActionRadialMenu()
    {
        if (isOpenActionRadialMenu)
        {
            isOpenActionRadialMenu = false;
            UI.SetActive(false);
        }
    }

    // Actions Selector -------------------------------------------------------------------------

    public void SelectAction(ACTION_TYPE actionType)
    {
        actualActionType = actionType;
        isOpenActionMenu = true;

        switch (actionType)
        {
            case ACTION_TYPE.weapon:
                menuWeapon.SetActive(true);
                menuItem.SetActive(false);
                menuDefense.SetActive(false);
                break;

            case ACTION_TYPE.item:
                menuWeapon.SetActive(false);
                menuItem.SetActive(true);
                menuDefense.SetActive(false);
                break;

            case ACTION_TYPE.defense:
                menuWeapon.SetActive(false);
                menuItem.SetActive(false);
                menuDefense.SetActive(true);
                break;

            default:
                break;
        }
    }

    public void SelectOption(OPTION_TYPE optionType)
    {

        switch (actualActionType)
        {
            case ACTION_TYPE.weapon:
                OptionWeapon(optionType);
                break;

            case ACTION_TYPE.item:
                OptionItem(optionType);
                break;

            case ACTION_TYPE.defense:
                OptionDefense(optionType);
                break;

            default:
                break;
        }

        isOpenActionMenu = false;
    }

    private void OptionWeapon(OPTION_TYPE optionType)
    {
        switch (optionType)
        {
            case OPTION_TYPE.option1:
                Debug.Log($"Weapon - Option 1");
                break;

            case OPTION_TYPE.option2:
                Debug.Log($"Weapon - Option 2");
                break;

            case OPTION_TYPE.option3:
                Debug.Log($"Weapon - Option 3");
                break;

            default:
                break;
        }

        menuWeapon.SetActive(false);
    }

    private void OptionDefense(OPTION_TYPE optionType)
    {
        switch (optionType)
        {
            case OPTION_TYPE.option1:
                Debug.Log($"Defense - Option 1");
                break;

            case OPTION_TYPE.option2:
                Debug.Log($"Defense - Option 2");
                break;

            case OPTION_TYPE.option3:
                Debug.Log($"Defense - Option 3");
                break;

            default:
                break;
        }

        menuDefense.SetActive(false);
    }

    private void OptionItem(OPTION_TYPE optionType)
    {
        switch (optionType)
        {
            case OPTION_TYPE.option1:
                Debug.Log($"Item - Option 1");
                break;

            case OPTION_TYPE.option2:
                Debug.Log($"Item - Option 2");
                break;

            case OPTION_TYPE.option3:
                Debug.Log($"Item - Option 3");
                break;

            default:
                break;
        }

        menuItem.SetActive(false);
    }

    //-------------------------------------------------------------------------

    public void Attack()
    {
        inAction = true;
        UI.SetActive(false);
        Action1.SetActive(false);
        Action2.SetActive(true);
        isOpenActionRadialMenu = false;
        _actualLayer = enemyLayer;
    }
    public void Defense()
    {
        Debug.Log("Defensa");
        UI.SetActive(false);
        isOpenActionRadialMenu = false;

    }
    public void Item()
    {
        inAction = true;
        Debug.Log("Player usó item");
        UI.SetActive(false);
        Action1.SetActive(false);
        Action3.SetActive(true);
        isOpenActionRadialMenu = false;
        _actualLayer = playerLayer;
    }

    public void Run()
    {
        Debug.Log("Escapanding");
        UI.SetActive(false);
        isOpenActionRadialMenu = false;
    }
    public void Exit()
    {
        Debug.Log("Cancel Action");
        UI.SetActive(false);
        isOpenActionRadialMenu = false;
        Action1.SetActive(false);
        return;
    }
    private void OnMouseDown()
    {
        isOpenActionRadialMenu = true;

        UI.transform.position = Input.mousePosition;
        UI.SetActive(true);
        Action1.SetActive(true);
        Action2.SetActive(false);
        Action3.SetActive(false);

    }
    private void InputAttack()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, _actualLayer);

        if (hit.collider != null)
        {
            inAction = false;
            _actualLayer = ignoreLayer;
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            enemy.ActionReceiveDamage(10);
        }
    }
    public void UpdateBarEnemy(float actualHealthEnemy)
    {
        barHealthEnemy.fillAmount = actualHealthEnemy;
        if (actualHealthEnemy == 0)
        {
            WIN.SetActive(true);
        }
    }
    public void InputHeal()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, _actualLayer);

        if (hit.collider != null)
        {
            inAction = false;
            _actualLayer = ignoreLayer;
            Player player = hit.collider.GetComponent<Player>();
            player.PlayerHeal();
        }
    }
    public void UpdateBarPlayer(float actualHealthPlayer)
    {
        barHealthEnemy.fillAmount = actualHealthPlayer;
        if (actualHealthPlayer == 0)
        {
            LOSE.SetActive(true);
        }

    }

}