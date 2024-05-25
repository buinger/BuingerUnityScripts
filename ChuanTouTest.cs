using UnityEngine;

public class ChuanTouTest : MonoBehaviour
{

    public float speed = 10;


    public float realHeigth = 1.5f;
    public float bodyRadius = 0.5f;


    private void Update()
    {
        
        Move(speed);

    }


    #region//控制移动相关   

    //判断当前位置的可移动性


    private Vector3 AntiPenetrationPos(Vector3 _aimPos)
    {
        if (transform.position == _aimPos)
        {
            return transform.position;
        }
        //判定横纵的可移动性
        bool couldHmove = true;
        bool couldVmove = true;
        //获取角色横纵偏移向量
        Vector3 dirH = (new Vector3(_aimPos.x - transform.position.x, 0, 0)).normalized;
        Vector3 dirV = (new Vector3(0, 0, _aimPos.z - transform.position.z)).normalized;



        Vector3 bodyPos = transform.position;
        float tempPercent = 3;
        bodyPos.y += realHeigth / tempPercent;

        //判定横向
        if (dirH != Vector3.zero && Physics.Raycast(new Ray(bodyPos, dirH), bodyRadius))
        {
            couldHmove = false;
        }
        else
        {

            if (Physics.Raycast(new Ray(bodyPos + dirH * bodyRadius, Vector3.up), realHeigth / tempPercent))
            {
                couldHmove = false;
            }
        }
        //判定纵向
        if (dirV != Vector3.zero && Physics.Raycast(new Ray(bodyPos, dirV), bodyRadius))
        {
            couldVmove = false;
        }
        else
        {
            if (Physics.Raycast(new Ray(bodyPos + dirV * bodyRadius, Vector3.up), realHeigth / tempPercent))
            {
                couldVmove = false;

            }
        }
        //画线调试
        Debug.DrawLine(bodyPos, dirH * bodyRadius + bodyPos, Color.red);
        Debug.DrawLine(bodyPos, dirV * bodyRadius + bodyPos, Color.red);

        Vector3 temp = bodyPos + dirH * bodyRadius;
        Debug.DrawLine(temp, temp + Vector3.up * realHeigth, Color.red);
        temp = bodyPos + dirV * bodyRadius;
        Debug.DrawLine(temp, temp + Vector3.up * realHeigth, Color.red);


        Vector3 aimPos = _aimPos;
        if (!couldHmove) aimPos.x = transform.position.x;
        if (!couldVmove) aimPos.z = transform.position.z;

        return aimPos;

        //return _aimPos;
    }


    protected void Move(float speed)
    {
        Vector3 moveValue = new Vector3(Input.GetAxis("Horizontal"),
            0, Input.GetAxis("Vertical"));

        Vector3 aimPos = moveValue * speed * Time.deltaTime + transform.position;
        transform.position = AntiPenetrationPos(aimPos);
    }

    #endregion

}
