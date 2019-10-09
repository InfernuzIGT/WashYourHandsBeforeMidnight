using UnityEngine;

namespace Events
{
    public class GameEvent { }
    public class FadeEvent : GameEvent
    {
        public string text;
        public float duration;
    }

    public class FadeInEvent : FadeEvent { }
    public class FadeOutEvent : FadeEvent { }
    
    public class FadeInCanvasEvent : FadeEvent { }
    public class FadeOutCanvasEvent : FadeEvent { }
}