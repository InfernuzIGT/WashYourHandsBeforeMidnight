using DG.Tweening;
using UnityEngine;
using UnityEngine.Playables;

namespace Events
{
    public class GameEvent { }

    #region Interaction

    public class InteractionEvent : GameEvent
    {
        public bool isStart;
        public Vector3 lastPlayerPosition;
        public bool isRunning;
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

    public class ChangePlayerPositionEvent : GameEvent
    {
        public Vector3 newPosition;
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
        public bool fadeFast;
        public TweenCallback callbackStart;
        public TweenCallback callbackMid;
        public TweenCallback callbackEnd;
    }

    public class CutsceneEvent : GameEvent
    {
        public PlayableAsset cutscene;
        public bool show;
        public PlayerSO playerData;
    }

    public class SessionEvent : GameEvent
    {
        public SESSION_OPTION option;
    }

    public class QuestEvent : GameEvent
    {
        public QuestSO data;
        public QUEST_STATE state;
    }

    public class MainMenuEvent : GameEvent
    {
        public MENU_TYPE menuType;
    }

    public class DeviceChangeEvent : GameEvent
    {
        public DEVICE device;
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
    }

    public class ChangeInputEvent : GameEvent
    {
        public bool enable;
    }

    public class ShowInteractionHintEvent : GameEvent
    {
        public bool show;
    }

}