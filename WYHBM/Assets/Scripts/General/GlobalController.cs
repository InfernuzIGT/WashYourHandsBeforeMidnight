using Cinemachine;
using UnityEditor;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    [Header("Spawn")]
    public bool customSpawn;
    public Transform spawnPoint;

    [Header("Cheats")]
    public bool infiniteStamina;
    public ItemSO[] items;

    [Header("Settings")]
    public PlayerController player;
    public Camera mainCamera;
    public CinemachineVirtualCamera virtualCamera;

    private CinemachineVirtualCamera _newVirtualCamera;

    private float _offsetPlayer = 1.505f;

    private void Start()
    {
        SpawnPlayer();
        SetCamera();
        AddItems();
    }

    private void SpawnPlayer()
    {
        RaycastHit hit;

#if UNITY_EDITOR

        if (customSpawn)
        {
            if (Physics.Raycast(spawnPoint.position, Vector3.down, out hit, Mathf.Infinity))
            {
                Vector3 spawnPosition = hit.point + new Vector3(0, _offsetPlayer, 0);
                player = Instantiate(player, spawnPosition, Quaternion.identity, this.transform);
            }
            else
            {
                Debug.LogWarning($"<color=yellow><b>[WARNING]</b></color> Can't detect surface to spawn!");

                player = Instantiate(player, spawnPoint.position, Quaternion.identity, this.transform);
            }
        }
        else
        {
            SceneView sceneView = SceneView.lastActiveSceneView;
            Vector3 sceneCameraPosition = sceneView.pivot - sceneView.camera.transform.position;

            if (Physics.Raycast(sceneCameraPosition, Vector3.down, out hit, Mathf.Infinity))
            {
                Vector3 spawnPosition = hit.point + new Vector3(0, _offsetPlayer, 0);
                player = Instantiate(player, spawnPosition, Quaternion.identity, this.transform);
            }
            else
            {
                Debug.LogWarning($"<color=yellow><b>[WARNING]</b></color> Can't detect surface to spawn!");

                player = Instantiate(player, sceneCameraPosition, Quaternion.identity, this.transform);
            }
        }
#else

        if (Physics.Raycast(spawnPoint.position, Vector3.down, out hit, Mathf.Infinity))
        {
            Vector3 spawnPosition = hit.point + new Vector3(0, _offsetPlayer, 0);
            player = Instantiate(player, spawnPosition, Quaternion.identity, this.transform);
        }
        else
        {
            Debug.LogWarning($"<color=yellow><b>[WARNING]</b></color> Can't detect surface to spawn!");

            player = Instantiate(player, spawnPoint.position, Quaternion.identity, this.transform);
        }

#endif
        player.gameObject.name = "Sam";
    }

    private void SetCamera()
    {
        virtualCamera.m_Follow = player.transform;
        virtualCamera.m_LookAt = player.transform;
        virtualCamera.transform.position = player.transform.position;
        
        DetectTargetBehind detectTargetBehind = mainCamera.GetComponent<DetectTargetBehind>();
        detectTargetBehind.SetTarget(player.transform);
    }

    public void ChangeCamera(CinemachineVirtualCamera newCamera)
    {
        if (newCamera == null)
        {
            _newVirtualCamera.gameObject.SetActive(false);
            virtualCamera.gameObject.SetActive(true);
            _newVirtualCamera = null;
        }
        else
        {
            newCamera.gameObject.SetActive(true);
            virtualCamera.gameObject.SetActive(false);
            _newVirtualCamera = newCamera;
        }
    }

    private void AddItems()
    {
        if (items.Length != 0)
        {
            for (int i = 0; i < items.Length; i++)
            {
                AddItem(i);
            }
        }
    }

    private void AddItem(int index)
    {
        if (GameManager.Instance.IsInventoryFull)
        {
            GameManager.Instance.worldUI.ShowPopup(GameData.Instance.textConfig.popupInventoryFull);
            return;
        }

        Slot newSlot = Instantiate(GameData.Instance.worldConfig.slotPrefab, GameManager.Instance.worldUI.itemParents);
        newSlot.AddItem(items[index]);
    }

    private void Update()
    {
        Cheats();
    }

    private void Cheats()
    {
        player.InfiniteStamina = infiniteStamina;
    }

}