using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Stage_FSM;

public static class Extension //옆에는 정적 클래스이다. 아래에 있는 메서드들은 확장 메서드라고 하는데 static키워드와 함께 정적 클래스 내에 정의돼있어야되며 this라는 키워드를 써야된다. 그러면 해당 타입과 같은 변수 뒤에서 바로 해당 (확장)메서드를 쓸수있다.
{
    public static T GetOrAddComponent<T>(this GameObject go)where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

    /*public static void BindEvent(this GameObject go
        ,Action action = null
        ,Action<BaseEventData> dragAction = null
        ,Define.UIEvent type = Define.UIEvent.Click)
    {
        UIBase.BindEvent(go, action, dragAction, type);
    }*/

    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }

    public static void DestoryChilds(this GameObject go)
    {
        Transform[] children = new Transform[go.transform.childCount];
        for (int i = 0; i < go.transform.childCount; i++)
        {
            children[i] = go.transform.GetChild(i);
        }

        foreach(Transform child in children)    //모든 자식 오브젝트 삭제 
        {
            Managers.Resource.Destroy(child.gameObject);
        }
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while( n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    public static void Anim_processing(this Dictionary<string, Anim_stage_state> dic, ref Animator anim, sbyte simplae_pattern_num, sbyte hard_pattern_num )
    {           //FIX : 나중에 여기 리팩토링 해야됨
        if (anim == null)
        {
            Debug.LogError("애니메이션이 없어");
        }
        Animator temp = anim;
        dic.Add("1_phase_idle", new Phase1_idle(anim));
        dic.Add("2_phase_idle", new Phase2_idle(anim));
        for (int i = 0; i < simplae_pattern_num; i++)
        {
            dic.Add($"simple_pattern{i}", new Simple_pattern($"simple_pattern{i}", anim));
        }
        for (int i = 0; i < hard_pattern_num; i++)
        {
            dic.Add($"hard_pattern{i}", new Hard_pattern($"hard_pattern{i}", anim));

        }
    }
    public static void Anim_processing2(this Dictionary<string, Anim_stage_state> dic, ref Animator anim, string[] anims_name)
    {           //배열 형식으로 바꿔서 애니메이션 이름을 패턴과 동일하게 되도록 변경
        if (anim == null)
        {
            Debug.LogError("애니메이션이 없어");
        }
        Animator temp = anim;
        foreach (string s in anims_name)
        {
            Debug.Log(s);
            dic.Add(s, new Pattern_anim(s, anim));
        }
    }
}
