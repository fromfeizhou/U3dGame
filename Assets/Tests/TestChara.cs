using UnityEngine;  
using System.Collections;  
using System.Collections.Generic;  
  
public class LoadTest : MonoBehaviour {  
  
    // Use this for initialization  
    void Start () {  
  
        ////加载模型;  
        //GameObject head = Instantiate(Resources.Load("anim/character/warrior/fashion/head_1")) as GameObject;  
        //GameObject jack = Instantiate(Resources.Load("anim/character/warrior/fashion/jacket_1")) as GameObject;  
        //GameObject pant = Instantiate(Resources.Load("anim/character/warrior/fashion/pants_1")) as GameObject;  
        //GameObject weapon = Instantiate(Resources.Load("anim/character/warrior/fashion/warrior_10l")) as GameObject;  
  
        ////加载动作数据;  
        //Animation mation = Resources.Load("anim/character/warrior/warrior", typeof(Animation)) as Animation;  
  
        ////获取所有的动作;  
        //List<string> animList = new List<string>();  
        //foreach (AnimationState state in mation)  
        //{  
        //    Debug.Log(state.name);  
        //    animList.Add(state.name);  
  
        //    //添加到四个部位;  
        //    head.animation.AddClip(mation.GetClip(state.name),state.name);  
        //    jack.animation.AddClip(mation.GetClip(state.name), state.name);  
        //    pant.animation.AddClip(mation.GetClip(state.name), state.name);  
        //    weapon.animation.AddClip(mation.GetClip(state.name), state.name);  
        //}  
  
        //head.animation.Play("run");  
        //jack.animation.Play("run");  
        //pant.animation.Play("run");  
        //weapon.animation.Play("run");  
  
  
    }  
      
    // Update is called once per frame  
    void Update () {  
      
    }  
}  