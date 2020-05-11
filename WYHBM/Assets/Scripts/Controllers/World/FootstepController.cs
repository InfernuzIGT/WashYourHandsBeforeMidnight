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
                case Tags.Ground_Grass:
                    groundType = GROUND_TYPE.Grass;
                    _player.footstepSound.EventInstance.setParameterByName(FMODParameters.GroundType, 2);
                    break;

                case Tags.Ground_Dirt:
                    groundType = GROUND_TYPE.Dirt;
                    _player.footstepSound.EventInstance.setParameterByName(FMODParameters.GroundType, 1);
                    break;

                case Tags.Ground_Wood:
                    groundType = GROUND_TYPE.Wood;
                    _player.footstepSound.EventInstance.setParameterByName(FMODParameters.GroundType, 3);
                    break;

                case Tags.Ground_Cement:
                    groundType = GROUND_TYPE.Cement;
                    _player.footstepSound.EventInstance.setParameterByName(FMODParameters.GroundType, 0);
                    break;

                case Tags.Ground_Ceramic:
                    groundType = GROUND_TYPE.Ceramic;
                    _player.footstepSound.EventInstance.setParameterByName(FMODParameters.GroundType, 3);
                    break;

                default:
                    groundType = GROUND_TYPE.none;
                    _player.footstepSound.EventInstance.setParameterByName(FMODParameters.GroundType, 0);
                    break;
            }
        }

    }
}