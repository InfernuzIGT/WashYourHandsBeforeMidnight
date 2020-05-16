using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSFX : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    //Para multiples animaciones en un mismo objeto se puede usar:

    public void MyAnimationEventHandler(AnimationEvent zombieAttack)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/NPC/Zombi 1 y 2/Zombi Attack", GetComponent<Transform>().position);
    }

    public void MyAnimation2EventHandler(AnimationEvent zombieHurt)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/NPC/Zombi 1 y 2/Zombi Hurt", GetComponent<Transform>().position);
    }

    public void MyAnimation3EventHandler(AnimationEvent zombieDeath)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/NPC/Zombi 1 y 2/Zombi Death", GetComponent<Transform>().position);
    }

    public void MyAnimation4EventHandler(AnimationEvent zombieWalk)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/NPC/Zombi 1 y 2/Zombi Roam", GetComponent<Transform>().position);

        // Etc.
    }

    //Etc.

    // Update is called once per frame
    void Update()
    {
        
    }
}
