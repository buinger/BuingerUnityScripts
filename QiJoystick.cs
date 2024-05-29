
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class QiJoystick : MonoBehaviour
{
    public  Vector2 value;
    public  Vector3 nowControlPos;
    //public  BarValueEvent onBarValueUpdate;
    float raduis;
    RectTransform bar;
    RectTransform range;

    public static int bindFingerIndex = -1;

    void OnDisable()
    {
        if (range != null)
        {
            bindFingerIndex = -1;
            nowControlPos = range.position;
            bar.position = range.position;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        range = transform.GetChild(0) as RectTransform;
        bar = range.GetChild(0) as RectTransform;
        bar.localPosition = Vector3.zero;
        Image rangeImage = range.GetComponent<Image>();
        raduis = range.rect.width / 2;




        void OnBeginDrag(PointerEventData peDate)
        {
            bindFingerIndex = Input.touchCount - 1;
            Vector2 aimPos = peDate.position;
            nowControlPos = aimPos;
            Vector3 dir = ((Vector3)aimPos - range.position);
            float changDu = dir.magnitude;
            if (changDu <= raduis)
            {
                bar.position = aimPos;
            }
        }

        void OnDrag(PointerEventData peDate)
        {

            Vector2 aimPos = peDate.position;
            nowControlPos = aimPos;
            Vector3 dir = ((Vector3)aimPos - range.position);
            float changDu = dir.magnitude;


            if (changDu <= raduis)
            {
                bar.position = aimPos;
            }
            else
            {
                bar.position = range.position + dir.normalized * raduis;
            }
        }

        void OnEndDrag(PointerEventData peDate)
        {
            bindFingerIndex = -1;
            nowControlPos = range.position;
            bar.position = range.position;
        }

        SetTriggerEvent(rangeImage, EventTriggerType.BeginDrag, (bed) =>
        {
            OnBeginDrag(bed as PointerEventData);
        });

        SetTriggerEvent(rangeImage, EventTriggerType.PointerDown, (bed) =>
        {
            OnBeginDrag(bed as PointerEventData);
        });


        SetTriggerEvent(rangeImage, EventTriggerType.Drag, (bed) =>
        {
            OnDrag(bed as PointerEventData);

        });

        SetTriggerEvent(rangeImage, EventTriggerType.EndDrag, (bed) =>
        {

            OnEndDrag(bed as PointerEventData);

        });


        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 dir = (bar.position - range.position);
        dir.x /= raduis;
        dir.y /= raduis;
        value = dir;

        if (Input.touchCount == 0 && Input.GetMouseButton(0) == false)
        {
            if (bar.localPosition != Vector3.zero)
            {
                bindFingerIndex = -1;
                nowControlPos = range.position;
                bar.position = range.position;
            }
        }

    }




    private void SetTriggerEvent(MaskableGraphic ui, EventTriggerType type, UnityAction<BaseEventData> doSth)
    {

        EventTrigger eventTrigger = ui.gameObject.GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = ui.gameObject.AddComponent<EventTrigger>();

        }
        //定义所要绑定的事件类型   
        EventTrigger.Entry entry = new EventTrigger.Entry();
        //设置事件类型    
        entry.eventID = type;
        //定义回调函数    
        UnityAction<BaseEventData> callback = doSth;
        //设置回调函数    
        entry.callback.AddListener(callback);
        //添加事件触发记录到GameObject的事件触发组件    
        eventTrigger.triggers.Add(entry);

    }
}
