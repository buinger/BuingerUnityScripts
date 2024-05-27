using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMysql : MonoBehaviour
{
    SqlTool mysqltool;
    // Start is called before the first frame update
    void Start()
    {

        mysqltool = new SqlTool("你的服务器域名或者ip", "mysql账号", "mysql密码", "库名");


    }


    [ContextMenu("新建表")]
    void CreatTable()
    {
        mysqltool.CreateTable("buYingLoshi",new string[] { "canshu1","canshu2"},new string[] { "text", "text" });
    }

    [ContextMenu("插入一行")]
    void ChaRu()
    {
        mysqltool.InsertInto("buYingLoshi", new string[] { "你好", "关注我" });
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
