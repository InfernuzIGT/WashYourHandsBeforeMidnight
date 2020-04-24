using UnityEngine;

[RequireComponent(typeof(Character))]
public class FootstepController : MonoBehaviour
{
    public LayerMask layerMask;

    private GROUND_TYPE groundType;

    // Raycast
    private Ray _ray;
    private RaycastHit _hit;

    private void Update()
    {
        // TODO Mariano: IF is in the Ground and IF is moving
        DetectGround();
    }

    private void DetectGround()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _hit, 2, layerMask))
        {
            // TODO Mariano: MOVE STRINGS TO CONFIG
            switch (_hit.collider.gameObject.tag)
            {
                case "A":
                    groundType = GROUND_TYPE.A;
                    break;
                case "B":
                    groundType = GROUND_TYPE.B;
                    break;
                case "C":
                    groundType = GROUND_TYPE.C;
                    break;

                default:
                    break;
            }
        }
    }

    public void Play()
    {
        switch (groundType)
        {
            case GROUND_TYPE.A:
                // Sound A
                break;

            case GROUND_TYPE.B:
                // Sound B
                break;

            case GROUND_TYPE.C:
                // Sound C
                break;

            default:
                break;
        }
    }
}