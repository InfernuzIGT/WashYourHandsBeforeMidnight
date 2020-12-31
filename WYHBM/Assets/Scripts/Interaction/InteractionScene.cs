using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionScene : Interaction, IInteractable
{
    [Header("Cutscene")]
    [SerializeField] private SceneSO sceneData;

    private List<AsyncOperation> _listScenes;

    private void Start()
    {
        _listScenes = new List<AsyncOperation>();
    }

    public void OnInteractionEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.AddListener<InteractionEvent>(OnInteractScene);
        }
    }

    public void OnInteractionExit(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player))
        {
            EventController.RemoveListener<InteractionEvent>(OnInteractScene);
        }
    }

    private void OnInteractScene(InteractionEvent evt)
    {
        EventController.RemoveListener<InteractionEvent>(OnInteractScene);
        
        // TODO Mariano: Remover Input al comenzar la carga
        
        LoadScenes();

        // TODO Mariano: Devolver Input al cargar la escena
    }

    private void LoadScenes()
    {
        _listScenes.Add(SceneManager.LoadSceneAsync(sceneData.sceneMain));

        if (sceneData.scenesAdditive.Length == 0)return;

        for (int i = 0; i < sceneData.scenesAdditive.Length; i++)
        {
            _listScenes.Add(SceneManager.LoadSceneAsync(sceneData.scenesAdditive[i], LoadSceneMode.Additive));
        }
    }
    
    private IEnumerator LoadingProgress ()
    {
        float currentProgress = 0;
        float totalProgress = 0;
        
        for (int i = 0; i < _listScenes.Count; i++)
        {
            while (!_listScenes[i].isDone)
            {
                totalProgress += _listScenes[i].progress;
                currentProgress = totalProgress/_listScenes.Count;
                yield return null;
            }
        }
    }
    
}