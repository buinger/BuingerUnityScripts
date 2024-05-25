using UnityEngine;

public class GongSi : MonoBehaviour
{
    private Ren xx;

    private void Start()
    {
        xx = new Ren();
        Debug.Log(xx.xiangFa);
        xx.ShuoChuXiaoMiMi();
        xx.ShuoChuDaMiMi();
    }

}



//abstract class ShengWu {

//    private abstract void xxx();

//}


//interface xx
//{

//    protected void xxx();
//    public void xxx2();
//}


class Ren
{
    //秘密一定是私有的
    private string mimi = "点赞，收藏，转发";

    //受保护的秘密可以让继承者用
    protected string mimi2 = "坚决不恰饭";

    //公开的想法可以直接说
    public string xiangFa = "给我投硬币";

    public void ShuoChuXiaoMiMi()
    {
        Debug.Log(mimi2);
    }

    public void ShuoChuDaMiMi()
    {
        Debug.Log(mimi);
    }


    protected virtual void Mengxiang()
    {
        Debug.Log("我想成为超人");
    }

}


class NvRen : Ren
{

    protected override void Mengxiang()
    {
        Debug.Log("我想成为公主");
    }
}


