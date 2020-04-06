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
    }
    
    public class ChangePlayerPositionEvent : GameEvent
    {
        public Vector3 newPosition;
    }

    #endregion

    public class ShakeEvent : GameEvent { }

    public class InfoTextEvent : GameEvent
    {
        public string text;
        public Vector2 position;
        public Color color;
    }

}