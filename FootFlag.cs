
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class FootFlag : MonoBehaviour
{
    public Vector3 posOffset = Vector3.zero;
    public Vector3 boxSize = Vector3.one;
    [HideInInspector]
    public UnityEvent onBeginAir = new UnityEvent();
    private bool couldJump = false;
    public LayerMask groundMask;
    public bool CouldJump
    {
        set
        {
            if (couldJump != value && value == false)
            {
                if (lastPos.y < transform.position.y)
                {
                    onBeginAir.Invoke();
                }
            }
            couldJump = value;
        }
        get
        {
            return couldJump;
        }
    }
   
    //private Vector3 sizeOffsetForGroundCheck;
    // [Header("地面检测的碰撞体")]
    public List<Collider> groundColliders = new List<Collider>();
    // Start is called before the first frame update
    void Awake()
    {
        couldJump = false;
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
    }





    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.color = new Color(Gizmos.color.r, Gizmos.color.g, Gizmos.color.b, 0.5f);
        Vector3 center = transform.position + posOffset;
        // center.y = center.y - sizeCollider.y / 2;
        Vector3 cubeScale = boxSize;
        Gizmos.DrawCube(center, cubeScale);
    }

    Vector3 lastPos;

    private void CheckGround()
    {
        groundColliders.Clear();
        Vector3 center = transform.position + posOffset;
        Vector3 cubeScale = boxSize;

        groundColliders = Physics.OverlapBox(center, cubeScale / 2,
            transform.rotation, groundMask).ToList();


        CouldJump = groundColliders.Count > 0;
        if (couldJump == true)
        {
            lastPos = transform.position;
        }
        // CheckLift(groundColliders);
    }



    private Collider nowLift;
    private Vector3 lastLiftPos;
    void CheckLift(List<Collider> colliders)
    {
        if (colliders.Count != 0)
        {
            if (nowLift != null)
            {
                if (colliders.Contains(nowLift))
                {
                    Vector3 offSet = nowLift.transform.position - lastLiftPos;
                    transform.position += offSet;
                    lastLiftPos = nowLift.transform.position;
                }
                else
                {
                    nowLift = null;
                    foreach (var item in colliders)
                    {
                        if (item.gameObject.layer == LayerMask.NameToLayer("Lift"))
                        {
                            nowLift = item;
                            break;
                        }
                    }
                    if (nowLift != null)
                    {
                        lastLiftPos = nowLift.transform.position;
                    }
                }
            }
            else
            {
                nowLift = null;
                foreach (var item in colliders)
                {
                    if (item.gameObject.layer == LayerMask.NameToLayer("Lift"))
                    {
                        nowLift = item;
                        break;
                    }
                }
                if (nowLift != null)
                {
                    lastLiftPos = nowLift.transform.position;
                }
            }

        }
        else
        {
            nowLift = null;
        }

    }
}