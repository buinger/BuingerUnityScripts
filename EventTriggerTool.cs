
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventTriggerTool
{
    public static void SetTriggerEvent(MaskableGraphic ui, EventTriggerType type, UnityAction<BaseEventData> doSth)
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
