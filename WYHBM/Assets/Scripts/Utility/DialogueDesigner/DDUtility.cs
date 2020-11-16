using DD;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InteractionNPC))]
public class DDUtility : MonoBehaviour
{
    public enum DDExecuteScript
    {
        GiveReward
    }

    public enum DDEvaluateCondition
    {
        HaveAmount
    }

    private InteractionNPC _interactionNPC;
    private Dialogue m_loadedDialogue;
    private DialoguePlayer m_dialoguePlayer;

    private void Awake()
    {
        _interactionNPC = GetComponent<InteractionNPC>();
    }

    private void Start()
    {
        m_loadedDialogue = Dialogue.FromAsset(_interactionNPC.data.dialogDD);

        if (m_loadedDialogue == null)
        {
            Debug.LogError($"<color=red><b>[ERROR]</b></color> No DialogDD", gameObject);
            return;
        }

        m_dialoguePlayer = new DialoguePlayer(m_loadedDialogue);

        DialoguePlayer.GlobalOnShowMessage += OnShowMessage;
        DialoguePlayer.GlobalOnEvaluateCondition += OnEvaluateCondition;
        DialoguePlayer.GlobalOnExecuteScript += OnExecuteScript;

        // if you want to handle a particular dialogue differently, you can use these instead
        //m_dialoguePlayer.OverrideOnShowMessage += OnShowMessageSpecial;
        //m_dialoguePlayer.OverrideOnEvaluateCondition += OnEvaluateConditionSpecial;
        //m_dialoguePlayer.OverrideOnExecuteScript += OnExecuteScriptSpecial;

        m_dialoguePlayer.OnDialogueEnded += OnDialogueEnded;

        m_dialoguePlayer.Play();
    }

    private void OnDialogueEnded(DialoguePlayer sender)
    {
        Debug.Log($"<color=green><b> DIALOGUE ENDED </b></color>");
    }

    private void Update()
    {
        // wait for player input on any Show Message Node, then advance
        ShowMessageNode showMessageNode = m_dialoguePlayer.CurrentNode as ShowMessageNode;
        if (showMessageNode != null)
        {
            ShowMessageNodeChoice choiceNode = showMessageNode as ShowMessageNodeChoice;
            if (choiceNode != null)
            {
                int numberInput = GetAlphaNumberDown();
                if (numberInput >= 1)
                {
                    m_dialoguePlayer.AdvanceMessage(numberInput - 1);
                }
            }
            else
            {
                // Close
                if (Keyboard.current.digit1Key.wasPressedThisFrame)
                {
                    m_dialoguePlayer.AdvanceMessage(0);
                }
            }
        }
    }

    private void OnShowMessage(DialoguePlayer sender, ShowMessageNode node)
    {
        Debug.Log($"<b>[{node.Character}] Dialog: </b>{node.GetText(GameData.Instance.GetLanguage())}");

        ShowMessageNodeChoice choiceNode = node as ShowMessageNodeChoice;
        if (choiceNode != null)
        {
            for (int i = 0; i < choiceNode.Choices.Length; i++)
            {
                Debug.LogFormat("[OPTION {0}] {1}", i + 1, choiceNode.GetChoiceText(i, GameData.Instance.GetLanguage()));
            }
        }
        else
        {
            Debug.Log($"<b> NO CHOICE / ONE OPTION</b>");
        }
    }

    private bool OnEvaluateCondition(DialoguePlayer sender, string script)
    {
        if (System.Enum.TryParse<DDEvaluateCondition>(script, out DDEvaluateCondition scriptParsed))
        {
            switch (scriptParsed)
            {
                case DDEvaluateCondition.HaveAmount:
                    return _interactionNPC.DDHaveAmount();

                default:
                    Debug.LogError($"<color=red><b>[ERROR]</b></color> No DDEvaluateCondition", gameObject);
                    return false;
            }
        }

        Debug.LogError($"<color=red><b>[ERROR]</b></color> Can't parse DDEvaluateCondition \"{script}\"", gameObject);
        return false;
    }

    private void OnExecuteScript(DialoguePlayer sender, string script)
    {
        if (System.Enum.TryParse<DDExecuteScript>(script, out DDExecuteScript scriptParsed))
        {
            switch (scriptParsed)
            {
                case DDExecuteScript.GiveReward:
                    _interactionNPC.DDGiveReward();
                    break;

                default:
                    Debug.LogError($"<color=red><b>[ERROR]</b></color> No DDExecuteScript", gameObject);
                    break;
            }
        }
        else
        {
            Debug.LogError($"<color=red><b>[ERROR]</b></color> Can't parse DDExecuteScript \"{script}\"", gameObject);
        }
    }

    private int GetAlphaNumberDown()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)return 1;
        if (Keyboard.current.digit2Key.wasPressedThisFrame)return 2;
        if (Keyboard.current.digit3Key.wasPressedThisFrame)return 3;
        if (Keyboard.current.digit4Key.wasPressedThisFrame)return 4;

        return -1;
    }

}