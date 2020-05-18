using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXOnAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public void EventHandlerMethod()
    {
        // las instrucciones necesarias, sea un playoneshot o un start, etc.
    }*/


    //Para multiples animaciones en un mismo objeto se puede usar:

    public void MyAnimationEventHandler(AnimationEvent animationEvent)
    {
        string stringParm = animationEvent.stringParameter;
        float floatParam = animationEvent.floatParameter;
        int intParam = animationEvent.intParameter;

        // Etc.
    }

    public void MyAnimation2EventHandler(AnimationEvent animationEvent)
    {
        string stringParm = animationEvent.stringParameter;
        float floatParam = animationEvent.floatParameter;
        int intParam = animationEvent.intParameter;

        // Etc.
    }

    //Etc.
}
