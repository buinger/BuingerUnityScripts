using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class BloodBar : MonoBehaviour
{
    protected Image bloodBarBG;
    protected Image bloodBar;


    public  void Ini()
    {
        bloodBarBG = transform.GetComponent<Image>();
        bloodBar = transform.GetChild(0).GetComponent<Image>();
        CheckColor();
    }

    private void Start()
    {
        Ini();
    }


    protected  void Update()
    {

        CheckColor();

    }

    // Update is called once per frame
    public  void ChangeValue(float percent)
    {
        float aimValue = percent;
        bloodBar.fillAmount = aimValue;
    }

    [ContextMenu("测试随机血量")]
    void SetRandomPercent()
    {
        float percent = Random.Range(0, 1f) / 1f;
        ChangeValue(percent);

    }


    private void CheckColor()
    {
        if (bloodBar.fillAmount > 0.3f)
        {
            bloodBar.color = Color.green;
        }
        else
        {
            bloodBar.color = Color.red;
        }
    }
}
