using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class FootstepController : MonoBehaviour
{
    public LayerMask layerMask;

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
                    _player.footstepSound.EventInstance.setParameterByName(FMODParameters.GroundType, 2);
                    break;

                case Tags.Ground_Dirt:
                    _player.footstepSound.EventInstance.setParameterByName(FMODParameters.GroundType, 1);
                    break;

                case Tags.Ground_Wood:
                    _player.footstepSound.EventInstance.setParameterByName(FMODParameters.GroundType, 3);
                    break;

                case Tags.Ground_Cement:
                    _player.footstepSound.EventInstance.setParameterByName(FMODParameters.GroundType, 0);
                    break;

                case Tags.Ground_Ceramic:
                    _player.footstepSound.EventInstance.setParameterByName(FMODParameters.GroundType, 3);
                    break;

                default:
                    _player.footstepSound.EventInstance.setParameterByName(FMODParameters.GroundType, 1);
                    break;
            }
        }
    }
}