using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public GameObject Exit;
    public GameObject MenuActions;
    public bool isPanelActive;


  void Update()
  {
      if(isPanelActive)
      {
          Exit.SetActive(false);
          MenuActions.SetActive(false);
      }
      else
      {
          Exit.SetActive(true);
          MenuActions.SetActive(true);
      }

  }
  
}
