using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class 旋转度和坐标序列化 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Info info = new Info();
        info.xx1 = Vector3.back;
        info.xx2 = transform.rotation;


        JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
        jsonSerializerSettings.Converters.Add(new Vector3Converter());
        jsonSerializerSettings.Converters.Add(new QuaternionConverter());
   
        string xxstring=JsonConvert.SerializeObject(info, jsonSerializerSettings);
        Debug.Log(xxstring);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


public class Info
{
    public Vector3 xx1;
    public Quaternion xx2;

}


