using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChangeCellSize : MonoBehaviour
{
    public float ratioX = 0.2f;
    public float ratioY = 0.2f;
    public GridLayoutGroup mGridLayoutGroup;
    //todo


    //temp
    float lastwidth = 0f;
    float lastheight = 0f;


    void OnEnable()
    {
        InvokeRepeating("ResolutionChange", 0.1f, 0.1f);
        if (mGridLayoutGroup == null)
        {
            mGridLayoutGroup = this.GetComponent<GridLayoutGroup>();
            if (mGridLayoutGroup == null)
            {
                this.enabled=false ;
            }
        }
    }

    //监听分辨率
    void ResolutionChange()
    {
        if (lastwidth != Screen.width || lastheight != Screen.height)
        {
            lastwidth = Screen.width;
            lastheight = Screen.height;
            //print(string.Concat("Resolution", lastheight, " ", lastwidth));

            mGridLayoutGroup.cellSize = new Vector2(ratioX * lastwidth, ratioY * lastheight); //整个屏幕的分辨率
        }
    }

    void OnDisable()
    {
        CancelInvoke("ResolutionChange");
    }

}
