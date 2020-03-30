using UnityEngine;

namespace Events
{
    public class GameEvent { }

    #region Dialogues

    public class UIEnableDialogEvent : GameEvent
    {
        public bool enable;
        public DialogSO dialog;
    }

    public class UIExecuteDialogEvent : GameEvent { }

    public class StopMovementEvent : GameEvent
    {
        public bool enable;
    }

    #endregion

    //-----------

    public class FadeEvent : GameEvent
    {
        public string text;
        public float duration;
    }

    public class FadeInEvent : FadeEvent { }
    public class FadeOutEvent : FadeEvent { }

    public class FadeInCanvasEvent : FadeEvent { }
    public class FadeOutCanvasEvent : FadeEvent { }

    public class ShakeEvent : FadeEvent { }

    public class InfoTextEvent : GameEvent
    {
        public string text;
        public Vector2 position;
        public Color color;
    }

}