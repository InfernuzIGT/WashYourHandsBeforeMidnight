using DG.Tweening;
using UnityEngine;

namespace Events
{
    public class GameEvent { }

    #region Interaction

    public class InteractionEvent : GameEvent
    {
        public Vector3 lastPlayerPosition;
    }

    public class EnableDialogEvent : GameEvent
    {
        public bool enable;
        public DialogSO dialog;
    }

    public class EnableMovementEvent : GameEvent
    {
        public bool canMove;
        public bool enterToInterior;
    }

    public class ChangePlayerPositionEvent : GameEvent
    {
        public Vector3 newPosition;
    }

    public class CreateInteriorEvent : GameEvent
    {
        public bool isCreating;
        public GameObject newInterior;
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

    public class InfoTextEvent : GameEvent
    {
        public string text;
        public Vector2 position;
        public Color color;
    }

}