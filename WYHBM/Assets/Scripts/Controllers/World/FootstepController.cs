using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class FootstepController : MonoBehaviour
{
    public LayerMask layerMask;

    private GROUND_TYPE groundType;
    private RaycastHit _hit;
    private PlayerController _player;

    FMODUnity.StudioEventEmitter footstepSound;


    private void Awake()
    {
        _player = GetComponent<PlayerController>();
    }

    private void Start()
    {
        footstepSound = GetComponent<FMODUnity.StudioEventEmitter>();
    }
    private void Update()
    {
        if (_player.CanPlayFootstep)
        {
            DetectGround();
        }
    }

    private void DetectGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _hit, 2, layerMask))
        {
            switch (_hit.collider.gameObject.tag)
            {
                case "Ground/Grass":
                    groundType = GROUND_TYPE.Grass;
                    footstepSound.EventInstance.setParameterByName("Ground Type", 2);
                    break;

                case "Ground/Dirt":
                    groundType = GROUND_TYPE.Dirt;
                    footstepSound.EventInstance.setParameterByName("Ground Type", 1);
                    break;

                case "Ground/Wood":
                    groundType = GROUND_TYPE.Wood;
                    footstepSound.EventInstance.setParameterByName("Ground Type", 3);
                    break;

                case "Ground/Cement":
                    groundType = GROUND_TYPE.Cement;
                    footstepSound.EventInstance.setParameterByName("Ground Type", 0);
                    break;

                case "Ground/Ceramic":
                    groundType = GROUND_TYPE.Ceramic;
                    footstepSound.EventInstance.setParameterByName("Ground Type", 3);
                    break;

                default:
                    groundType = GROUND_TYPE.none;
                    footstepSound.EventInstance.setParameterByName("Ground Type", 0);
                    break;
            }
        }

       
    }


    




    /*public void Play()
    {
        switch (groundType)
        {
            case GROUND_TYPE.Grass:
                // Sound Grass
                break;

            case GROUND_TYPE.Dirt:
                // Sound Dirt
                break;

            case GROUND_TYPE.Wood:
                // Sound Wood
                break;

            case GROUND_TYPE.Cement:
                // Sound Cement
                break;

            case GROUND_TYPE.Ceramic:
                // Sound Ceramic
                break;

            default:
                // Sound Default
                break;
        }
    }*/
}