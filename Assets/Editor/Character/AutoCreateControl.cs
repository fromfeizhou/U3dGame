using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using System.IO;

/**
* 功 能： 
* ───────────────────────────────────
* V0.01 2017/8/10 14:55:13 leeming 
* Copyright (c) 2017 edo Corporation. All rights reserved.
*/
public class AutoCreateControl
{
    private static string ParentFold = "/Resources/Character/";
    private static string ParentPath = Application.dataPath + ParentFold;

    [MenuItem("EDOTools/Charactor/2.CreateControlAll")]
    public static void CreateControlAll()
    {
        string[] dirs = Directory.GetDirectories(ParentPath, "*", SearchOption.TopDirectoryOnly);

        foreach (string dir in dirs)
        {
            if (dir.EndsWith(".svn"))
                continue;

            int lastLineIndex = dir.LastIndexOf("/");
            if (lastLineIndex == -1)
                continue;

            string foldName = dir.Substring(lastLineIndex + 1);

            try
            {
                CreateControl(foldName);
            }
            catch (Exception e)
            {
                Debug.LogError("----create charactor control fail----name=" + foldName + ",msg=" + e.Message);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [MenuItem("EDOTools/Charactor/2.CreateControlSelect")]
    public static void CreateControlSelect()
    {
        UnityEngine.Object[] objs = Selection.GetFiltered(typeof(DefaultAsset), SelectionMode.TopLevel);
        foreach (UnityEngine.Object o in objs)
        {
            string path = ParentPath + o.name;
            if (Directory.Exists(path) && Directory.Exists(path + "/models"))
                CreateControl(o.name);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    #region 创建animator

    private static string modelPath = "";
    private static AnimatorStateMachine machine;
    private static AnimatorController m_ac;
    private static Dictionary<string, AnimatorState> m_allState = new Dictionary<string, AnimatorState>();
    public static AnimatorController CreateControl(string foldName)
    {
        string path = string.Format("Assets{0}{1}/animation", ParentFold, foldName);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        AnimatorController ac = AnimatorController.CreateAnimatorControllerAtPath(path + "/a.controller");
        AddParam(ac);
        m_ac = ac;
        modelPath = string.Format("Assets{0}{1}/models/", ParentFold, foldName);

        var animLayer = ac.layers[0];
        var stateMachine = animLayer.stateMachine;
        //machine.entryPosition = new Vector3(100, -400, 0);
        //machine.anyStatePosition = new Vector3(100, -216, 0);
        //machine.parentStateMachinePosition = new Vector3(100, -400, 0);
        machine = stateMachine;
        CreateAllState(machine);

        CreateAllRelation();

        return ac;
    }

    /************************************************************************/
    /* 增加参数                                                                     */
    /************************************************************************/
    private static string[] ParamNamesFloat = { "rotation" };
    private static string[] ParamNamesInt = { "action" };

    public static void AddParam(AnimatorController ac)
    {
        foreach (string name in ParamNamesFloat)
        {
            var parm2 = new UnityEngine.AnimatorControllerParameter();
            parm2.type = UnityEngine.AnimatorControllerParameterType.Float;
            parm2.name = name;
            parm2.defaultFloat = 0;
            ac.AddParameter(parm2);
        }

        foreach (string name in ParamNamesInt)
        {
            var parm2 = new UnityEngine.AnimatorControllerParameter();
            parm2.type = UnityEngine.AnimatorControllerParameterType.Int;
            parm2.name = name;
            parm2.defaultFloat = 0;
            ac.AddParameter(parm2);
        }
    }

    public static void CreateAllRelation()
    {
        AnimatorStateTransition ast = null;

        AddTransition("", "stand", "action", AnimatorConditionMode.Equals, 0);
        AddTransition("", "move", "action", AnimatorConditionMode.Equals, 1);
        AddTransition("", "standup", "action", AnimatorConditionMode.Equals, 2);
        AddTransition("", "atstand", "action", AnimatorConditionMode.Equals, 3);
        AddTransition("", "flyup", "action", AnimatorConditionMode.Equals, 4);
        AddTransition("", "flydown", "action", AnimatorConditionMode.Equals, 5);
        AddTransition("", "lose", "action", AnimatorConditionMode.Equals, 6);
        AddTransition("", "victory", "action", AnimatorConditionMode.Equals, 7);
        AddTransition("", "victorybreak", "action", AnimatorConditionMode.Equals, 8);
        AddTransition("", "damage01", "action", AnimatorConditionMode.Equals, 9);
        AddTransition("", "attack01", "action", AnimatorConditionMode.Equals, 10);
        AddTransition("", "attack02", "action", AnimatorConditionMode.Equals, 11);
        AddTransition("", "attack03", "action", AnimatorConditionMode.Equals, 12);
        AddTransition("", "attack04", "action", AnimatorConditionMode.Equals, 13);
        AddTransition("", "attack05", "action", AnimatorConditionMode.Equals, 14);
        AddTransition("", "attack100", "action", AnimatorConditionMode.Equals, 15);
        AddTransition("", "attack101", "action", AnimatorConditionMode.Equals, 16);
        AddTransition("", "attack102", "action", AnimatorConditionMode.Equals, 17);

    }

    private static AnimatorStateTransition AddTransition(string fromName, string toName,
        params object[] conditions)
    {
        AnimatorState from = null;
        if (m_allState.ContainsKey(fromName))
            from = m_allState[fromName];

        AnimatorState to = null;
        if (m_allState.ContainsKey(toName))
            to = m_allState[toName];

        AnimatorStateTransition transition = (from == null ? machine.AddAnyStateTransition(to) : from.AddTransition(to));

        for (int i = 0; i < conditions.Length; )
        {
            var name = (string)conditions[i++];
            var type = (AnimatorConditionMode)conditions[i++];
            if (type == AnimatorConditionMode.If || type == AnimatorConditionMode.IfNot)
            {
                i++;
                transition.AddCondition(type, 0, name);
            }
            else
            {
                var arg = (int)conditions[i++];
                transition.AddCondition(type, arg, name);
            }
        }
        if (!fromName.StartsWith("stand"))
        {
            transition.canTransitionToSelf = false;
        }
        if (fromName.StartsWith("attack"))
        {
            transition.hasExitTime = true;
        }
        return transition;
    }

    #region 创建状态
    public static void CreateAllState(AnimatorStateMachine machine)
    {
        m_allState.Clear();
        string[] states = { "stand", "move", "standup", "atstand", "flyup", "flydown", "lose", "victory", "victorybreak", "damage01", "attack100", "attack101", "attack102" };
        AnimatorState aState;
        int index = 0;
        for (int i = 0; i < states.Length; i++)
        {
            string state = states[i];
            if (i == 0)
                aState = CreateState(machine, state, new Vector3(0, -100, 0));
            else
                aState = CreateState(machine, state, new Vector3(250, -states.Length * 30 + 60 * index, 0));
            if (aState != null)
                index++;
        }

        string[] skillState = { "attack01", "attack02", "attack03", "attack04", "attack05" };
        for (int i = 0; i < skillState.Length; i++)
        {
            string state = skillState[i];
            CreateState(machine, state, new Vector3(250, 60 * i, 0));
        }
    }

    private static string Modex_ext = ".fbx";
    public static AnimatorState CreateState(AnimatorStateMachine machine
        , string name, Vector3 pos)
    {
        string fbxPath = modelPath + "action" + Modex_ext;
        string realAlcName = name;

        AnimatorState state = null;
        if (IsBlendTree(name))
        {
            AnimationClip a1c = GetAnimationClip(fbxPath, realAlcName + "_up");
            if (a1c == null)
            {
                Debug.LogError("--------动作丢失---------name=" + (fbxPath + "/" + name));
                return null;
            }
            BlendTree bt;

            state = m_ac.CreateBlendTreeInController(name, out bt);
            bt.hideFlags = HideFlags.HideInHierarchy;
            string[] btMotion = { "_up", "_rightup", "_right", "_rightdown" };
            for (int i = 0; i < btMotion.Length; i++)
            {
                a1c = GetAnimationClip(fbxPath, realAlcName + btMotion[i]);
                bt.AddChild(a1c);
            }
            bt.useAutomaticThresholds = true;
            bt.blendParameter = "rotation";
            bt.name = "Blend Tree";
            state.motion = bt;
        }
        else
        {
            AnimationClip anim = GetAnimationClip(fbxPath, realAlcName);

            if (anim == null)
            {
                Debug.LogError("--------动作丢失---------name=" + (fbxPath + "/" + name));
                return null;
            }

            state = machine.AddState(name, pos);
            state.motion = anim;
        }
        m_allState[name] = state;
        return state;
    }

    private static bool IsBlendTree(string name)
    {
        string[] treeStates = { "atstand", "attack01", "attack02", "attack03", "attack04", "attack05" };
        if (treeStates.Contains(name))
            return true;
        return false;
    }

    private static AnimationClip GetAnimationClip(string fbxPath, string name)
    {
        if (name == "victory")
        {
            fbxPath = modelPath + name + Modex_ext;
            AnimationClip a1c = AssetDatabase.LoadAssetAtPath(fbxPath, typeof(AnimationClip)) as AnimationClip;
            return a1c;
        }

        UnityEngine.Object[] objs = AssetDatabase.LoadAllAssetsAtPath(fbxPath);
        foreach (UnityEngine.Object o in objs)
        {
            if (o is AnimationClip && o.name == name)
            {
                return (o as AnimationClip);
            }
        }
        return null;
    }

    #endregion
    #endregion  -- 创建animator
}
