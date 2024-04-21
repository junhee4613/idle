using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_controller
{
    Dictionary<string, List<Pattern_json_date>> stage_data = new Dictionary<string, List<Pattern_json_date>>();
    Dictionary<string, Anim_stage_state> anim_state = new Dictionary<string, Anim_stage_state>();
    public void Anim_init()
    {

    }
    public void Stage_init(string Portal_name)
    {
        switch (Portal_name)
        {
            case "Stage2":
                //Fix : 여기에 스테이지 별로 데이터 가져오기
                break;
            default:
                break;
        }
    }
}
