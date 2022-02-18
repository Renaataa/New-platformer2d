using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CastedFunction();

public class Progress{
    public static void StartNewProgress(GameObject obj, float timeSec, CastedFunction func){
        Progress.StartNewProgress(obj, timeSec, true, func);
    }

    public static void StartNewProgress(GameObject obj, float timeSec, bool repeat, CastedFunction func){
        if(obj == null) return;

        CorutineLoader prevComp = obj.GetComponent<CorutineLoader>();

        if(prevComp != null){
            prevComp.StopAllCoroutines();
            prevComp.StartCast(timeSec, repeat, func);
        }
        else{
            obj.AddComponent<CorutineLoader>().StartCast(timeSec, repeat, func);
        }
    }
    private Progress() {}
}

public class CorutineLoader : MonoBehaviour
{
    private float timeSec = 0;
    private bool repeat = false;
    private CastedFunction func = null;

    public void StartCast(float time, bool repeat, CastedFunction function){
        timeSec = time;
        this.repeat = repeat;
        func = function;

        if(timeSec > 0 && this.isActiveAndEnabled) 
            StartCoroutine(Corutine());
        else Destroy(this);
    }

    private IEnumerator Corutine(){
        yield return new WaitForSeconds(timeSec);
        CastDone();
    }

    private void CastDone(){
        if(func != null) func();

        if(repeat && this.isActiveAndEnabled)
            StartCoroutine(Corutine());
        else {
            StopAllCoroutines();
            Destroy(this);
        }
    }

    private void OnDestroy(){
        StopAllCoroutines();
    }
}
