using System.Collections.Generic;
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

    public class DialogDesignerEvent : GameEvent
    {
        public bool enable;
        public NPCController npc;
        public TextAsset dialogue;
        public IDialogueable dialogueable;
        public PlayerSO playerData;
    }

    public class DialogSimpleEvent : GameEvent
    {
        public bool enable;
        public LocalizedString localizedString;
        public QuestSO questData;
        public QUEST_STATE questState;
        public string objectName;
    }

    public class EnableMovementEvent : GameEvent
    {
        public bool canMove;
        public bool isDetected;
    }

    public class LadderEvent : GameEvent
    {
        public LADDER_TYPE type;
    }

    public class CombatEvent : GameEvent
    {
        public bool isEnter;
        public bool isWin;
        public Transform enemy;
        public List<Enemy> combatEnemies = new List<Enemy>();
    }

    public class CombatActionEvent : GameEvent
    {
        public ItemSO item;
    }

    public class CombatPlayerEvent : GameEvent
    {
        public bool canSelect;
        public int combatIndex;
    }

    public class CombatRemoveCharacterEvent : GameEvent
    {
        public CombatCharacter character;
        public bool isPlayer;
    }

    public class CombatEnemyEvent : GameEvent { }

    public class CombatCharacterGoAheadEvent : GameEvent
    {
        public CombatCharacter character;
    }

    public class CombatCreateActionsEvent : GameEvent
    {
        public Equipment equipment;
    }

    public class CombatHideActionsEvent : GameEvent
    {
        public bool canHighlight;
    }

    #endregion

    public class ShakeEvent : GameEvent { }

    public class VibrationEvent : GameEvent
    {
        public RUMBLE_TYPE type;
    }

    public class FadeEvent : GameEvent
    {
        public bool instant;
        public float delay;
        public TweenCallback callbackStart;
        public TweenCallback callbackMid;
        public TweenCallback callbackEnd;
    }

    public class CustomFadeEvent : GameEvent
    {
        public bool instant;
        public bool fadeIn;
        public TweenCallback callbackFadeIn;
        public float duration;
    }

    public class CutsceneEvent : GameEvent
    {
        public PlayableAsset cutscene;
        public bool show;
        public PlayerSO playerData;
    }

    public class ChangeSceneEvent : GameEvent
    {
        public bool onlyTeleport;
        public bool load;
        public bool useEnableMovementEvent;
        public bool isLoadAdditive;
        public SceneSO sceneData;
        public Vector3 newPlayerPosition;
        public bool instantFade;
    }

    public class ChangePositionEvent : GameEvent
    {
        public Vector3 newPosition;
        public Vector3 offset = Vector3.zero;
        public bool useY;
    }

    public class SessionEvent : GameEvent
    {
        public SESSION_OPTION option;
    }

    public class SaveAnimationEvent : GameEvent { }

    public class LoadAnimationEvent : GameEvent
    {
        public bool isLoading;
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
        public bool isPaused;
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

    // public class ShowInteractionHintEvent : GameEvent
    // {
    //     public bool show;
    // }

}