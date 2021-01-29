using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Playables;

namespace Events
{
    public class GameEvent { }

    #region Interaction

    public class InteractionEvent : GameEvent
    {
        public bool isStart;
        public Vector3 lastPlayerPosition;
    }

    public class EnableDialogEvent : GameEvent
    {
        public bool enable;
        public NPCController npc;
        public TextAsset dialogue;
        public IDialogueable dialogueable;
        public PlayerSO playerData;
    }

    public class EnableMovementEvent : GameEvent
    {
        public bool canMove;
    }

    public class LadderEvent : GameEvent
    {
        public LADDER_EXIT ladderExit;
    }

    public class EnterCombatEvent : GameEvent
    {
        public NPCSO npc;
        public NPCController currentNPC;
    }

    public class ExitCombatEvent : GameEvent
    {
        public bool isWin;
    }

    #endregion

    public class ShakeEvent : GameEvent { }

    public class FadeEvent : GameEvent
    {
        public bool instant;
        public TweenCallback callbackStart;
        public TweenCallback callbackMid;
        public TweenCallback callbackEnd;
    }

    public class CustomFadeEvent : GameEvent
    {
        public bool instant;
        public bool fadeIn;
        public TweenCallback callbackFadeIn;
    }

    public class CutsceneEvent : GameEvent
    {
        public PlayableAsset cutscene;
        public bool show;
        public PlayerSO playerData;
    }

    public class ChangeSceneEvent : GameEvent
    {
        public bool load;
        public bool isLoadAdditive;
        public SceneSO sceneData;
        public Vector3 newPlayerPosition;
        public bool instantFade;
    }

    public class ChangePositionEvent : GameEvent
    {
        public Vector3 newPosition;
    }

    public class SessionEvent : GameEvent
    {
        public SESSION_OPTION option;
    }
    
    public class SaveAnimationEvent : GameEvent
    {
    }

    public class QuestEvent : GameEvent
    {
        public QuestSO data;
        public QUEST_STATE state;
    }

    public class MainMenuEvent : GameEvent
    {
        public UI_TYPE menuType;
    }

    public class DeviceChangeEvent : GameEvent
    {
        public bool showPopup;
        public DEVICE device;
        public Gamepad gamepad;
    }

    public class PauseEvent : GameEvent
    {
        public PAUSE_TYPE pauseType;
    }

    public class InfoTextEvent : GameEvent
    {
        public string text;
        public Vector2 position;
        public Color color;
    }

    public class EventSystemEvent : GameEvent
    {
        public GameObject objectSelected;
    }

    public class UpdateLanguageEvent : GameEvent
    {
        public string language;
        public Locale locale;
    }

    public class ChangeInputEvent : GameEvent
    {
        public bool enable;
    }
    
    public class CurrentInteractEvent : GameEvent
    {
        public Interaction currentInteraction;
    }

    public class ShowInteractionHintEvent : GameEvent
    {
        public bool show;
    }

}