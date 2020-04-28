using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class FootstepController : MonoBehaviour
{
    public LayerMask layerMask;

    private GROUND_TYPE groundType;
    private RaycastHit _hit;
    private PlayerController _player;

    private void Awake()
    {
        _player = GetComponent<PlayerController>();
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
                    break;

                case "Ground/Dirt":
                    groundType = GROUND_TYPE.Dirt;
                    break;

                case "Ground/Wood":
                    groundType = GROUND_TYPE.Wood;
                    break;

                case "Ground/Cement":
                    groundType = GROUND_TYPE.Cement;
                    break;

                case "Ground/Ceramic":
                    groundType = GROUND_TYPE.Ceramic;
                    break;

                default:
                    groundType = GROUND_TYPE.none;
                    break;
            }
        }
    }

    public void Play()
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
    }
}