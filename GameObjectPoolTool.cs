using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectPoolTool : MonoBehaviour
{
    public static GameObject GetFromPool(bool active, string fullname)
    {
        string aimFullName = fullname;
        aimFullName = GameObjectPool.FormatPath4ResourceLoad(aimFullName);

        GameObject aimGameObj;
        aimGameObj = GameObjectPool.OutPool(aimFullName);
        if (aimGameObj == null)
        {
            return null;
        }
        else
        {
            //aimGameObj.transform.SetParent(null);
            aimGameObj.SetActive(active);
            return aimGameObj;
        }
    }

    public static GameObject GetFromPool(string fullName, Vector3 postion, Transform father, bool isLocalSet = false)
    {

        GameObject aimGameObj = GetFromPool(true, fullName);
        aimGameObj.transform.SetParent(father);
        if (isLocalSet)
        {
            aimGameObj.transform.localPosition = postion;
        }
        else
        {
            aimGameObj.transform.position = postion;
        }
        return aimGameObj;
    }
    public static async Task<GameObject> GetFromPoolForceAsync(bool active, string fullname)
    {
        string aimFullName = fullname;
        aimFullName = GameObjectPool.FormatPath4ResourceLoad(aimFullName);
        GameObject aimGameObj;
        aimGameObj = GameObjectPool.OutPool(aimFullName);

        if (aimGameObj == null)
        {
            Task getResource = GameObjectPool.PreLoadPrefabToPoolAsync(aimFullName);
            await getResource;
            aimGameObj = GameObjectPool.OutPool(aimFullName);
            aimGameObj.SetActive(active);
            return aimGameObj;
        }
        else
        {
            //aimGameObj.transform.SetParent(null);
            aimGameObj.SetActive(active);
            return aimGameObj;
        }
    }

    public static GameObject GetFromPoolForce(bool active, string fullname)
    {
        string aimFullName = fullname;
        aimFullName = GameObjectPool.FormatPath4ResourceLoad(aimFullName);
        GameObject aimGameObj;
        aimGameObj = GameObjectPool.OutPool(aimFullName);

        if (aimGameObj == null)
        {
            GameObjectPool.PreLoadPrefabToPool(aimFullName);
            aimGameObj = GameObjectPool.OutPool(aimFullName);
            aimGameObj.SetActive(active);
            return aimGameObj;
        }
        else
        {
            //aimGameObj.transform.SetParent(null);
            aimGameObj.SetActive(active);
            return aimGameObj;
        }
    }

    public static GameObject GetFromPool(bool active, string fullname, Vector3 postion)
    {
        GameObject aimGameObj = GetFromPool(active, fullname);
        if (aimGameObj == null)
        {
            return null;
        }
        aimGameObj.transform.position = postion;
        return aimGameObj;
    }

    public static GameObject GetFromPool(bool active, string fullname, Vector3 postion, Transform father, bool isLocalSet = false)
    {
        GameObject aimGameObj = GetFromPool(active, fullname);
        if (aimGameObj == null)
        {
            return null;
        }
        aimGameObj.transform.SetParent(father);
        aimGameObj.transform.localScale = Vector3.one;
        if (isLocalSet)
        {
            aimGameObj.transform.localPosition = postion;
        }
        else
        {
            aimGameObj.transform.position = postion;
        }
        return aimGameObj;
    }

    //public GameObject GetFromPoolLikeFather(string fullname, Transform tempTrans, bool beSon = false)
    //{
    //    GameObject aimGameObj = GetFromPool(fullname);
    //    if (aimGameObj == null)
    //    {
    //        return null;
    //    }
    //    if (beSon == true)
    //    {
    //        aimGameObj.transform.SetParent(tempTrans);
    //        aimGameObj.transform.position = Vector3.zero;
    //        aimGameObj.transform.rotation = Quaternion.identity;
    //    }
    //    else
    //    {
    //        aimGameObj.transform.position = tempTrans.position;
    //        aimGameObj.transform.rotation = tempTrans.rotation;
    //    }
    //    return aimGameObj;
    //}

    public static void PutInPool(GameObject gameObj)
    {
        GameObjectPool.InPool(gameObj);
    }

    public static void Release(bool releaseAll = false)
    {
        if (releaseAll)
        {
            GameObjectPool.ReleaseAll();
        }
        else
        {
            GameObjectPool.ReleaseEmptyObj();
        }
    }


    //-------------------------------Plot池

    public class GameObjectPool
    {
        //池字典
        public static Dictionary<string, List<GameObject>> itemPool = new Dictionary<string, List<GameObject>>();
        //加载过的资源
        public static Dictionary<string, GameObject> loadedPrefabs = new Dictionary<string, GameObject>();

        public static void ReleaseAll()
        {
            foreach (var item in itemPool)
            {
                foreach (GameObject obj in item.Value)
                {
                    if (obj != null)
                    {
                        Destroy(obj);
                    }
                }
            }
            itemPool.Clear();
        }

        public static void ReleaseEmptyObj()
        {
            List<GameObject> allEmptyObj = new List<GameObject>();
            foreach (var item in itemPool)
            {
                List<GameObject> tempEmptyObj = new List<GameObject>();
                foreach (GameObject obj in item.Value)
                {
                    if (obj == null)
                    {
                        tempEmptyObj.Add(obj);
                        allEmptyObj.Add(obj);
                    }
                }
                foreach (GameObject obj in tempEmptyObj)
                {
                    item.Value.Remove(obj);
                }
            }
        }


        public static void InPool(GameObject gameObj)
        {
            //Button but = gameObj.GetComponent<Button>();
            //if (but != null)
            //{
            //    but.onClick.RemoveAllListeners();
            //}

            if (itemPool.ContainsKey(gameObj.name) == false)
            {
                itemPool.Add(gameObj.name, new List<GameObject>());
            }

            gameObj.SetActive(false);

            if (!itemPool[gameObj.name].Contains(gameObj))
            {
                itemPool[gameObj.name].Add(gameObj);
            }
        }

        public static GameObject OutPool(string assetPath)
        {
            if (itemPool.ContainsKey(assetPath) == false)
            {
                itemPool.Add(assetPath, new List<GameObject>());
            }

            if (itemPool[assetPath].Count == 0)
            {
                return null;
            }
            else
            {
                GameObject outGo = itemPool[assetPath][0];
                itemPool[assetPath].Remove(outGo);
                if (outGo != null)
                {
                    //这里有竞争条件，可能要Lock      
                    outGo.SetActive(true);
                    return outGo;
                }
                else
                {
                    return OutPool(assetPath);
                }
            }
        }


        /// <summary>
        /// 通过路径获取预制件名字
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string GetNameFromPrefabPath(string path)
        {
            string target = "";
            string[] strs = path.Split('/');
            if (strs.Length != 0)
            {
                string temp = strs[strs.Length - 1];
                target = FormatPath4ResourceLoad(temp);
            }
            return target;
        }


        public static string FormatPath4ResourceLoad(string oringnal)
        {
            string target = oringnal;
            if (target.Contains(".prefab"))
            {
                target = target.Replace(".prefab", "");
            }
            if (target.Contains("Assets/Resources/"))
            {
                target = target.Replace("Assets/Resources/", "");
            }

            return target;
        }


        /// <summary>
        /// 协程
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IEnumerator PreLoadPrefabToPoolIE(string path)
        {
            string assetPath = path;
            assetPath = FormatPath4ResourceLoad(assetPath);

            Task<GameObject> loadTask = LoadGameObjectAsync(path);
            while (!loadTask.IsCompleted)
            {
                yield return null;
            }

            GameObject loadedObject = Instantiate(loadTask.Result);
            loadedObject.name = path;
            InPool(loadedObject);

            yield return loadedObject;

        }




        /// <summary>
        /// 异步
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public async static Task<GameObject> PreLoadPrefabToPoolAsync(string path)
        {
            string assetPath = path;
            assetPath = FormatPath4ResourceLoad(assetPath);
            Task<GameObject> loadTask = LoadGameObjectAsync(path);
            await loadTask;
            GameObject loadedObject = Instantiate(loadTask.Result);
            loadedObject.name = path;
            InPool(loadedObject);

            return loadedObject;
        }


        public static GameObject PreLoadPrefabToPool(string path)
        {
            string assetPath = path;
            assetPath = FormatPath4ResourceLoad(assetPath);
            GameObject gameObj = LoadGameObject(path);
            GameObject loadedObject = Instantiate(gameObj);
            loadedObject.name = path;
            InPool(loadedObject);

            return loadedObject;
        }


        static async Task<GameObject> LoadGameObjectAsync(string path)
        {
            if (loadedPrefabs.ContainsKey(path))
            {
                return loadedPrefabs[path];
            }
            else
            {
                ResourceRequest rr = Resources.LoadAsync<GameObject>(path);
                while (rr.isDone == false) await Task.Delay(100);
                GameObject gameObj = rr.asset as GameObject;
                loadedPrefabs.Add(path, gameObj);
                return gameObj;
            }
        }

        static GameObject LoadGameObject(string path)
        {
            if (loadedPrefabs.ContainsKey(path))
            {
                return loadedPrefabs[path];
            }
            else
            {
                GameObject gameObj = Resources.Load<GameObject>(path);
                loadedPrefabs.Add(path, gameObj);
                return gameObj;
            }
        }
    }
}
