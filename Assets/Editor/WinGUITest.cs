using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class WinGUITest : EditorWindow
{
    private int select = 0;

    [MenuItem("MyTool/WinGUITest")]

    static void ShowWindow()
    {
        //创建窗口
        EditorWindow.GetWindowWithRect(typeof(WinGUITest),new Rect(300,300,500,600));

    }

    void OnGUI()
    {
        #region GUILayout布局
        ////GUILayout采用线性布局，类似于StackPanel，默认是纵向布局。通过GUILayout.BeginHorizontal();
        ////开启和GUILayout.EndHorizontal()结束一个横向排列区域，同理BeginVertical() 、EndVertical()。
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Button1", GUILayout.Width(100), GUILayout.Height(50)))
        {
            Debug.Log("Button1");
        }
        GUILayout.Button("Button2", GUILayout.Width(100), GUILayout.Height(50));
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();
        //如果嫌控件太挤，可以使用GUILayout.Space(30);增加若干像素的间隙。
        GUILayout.Space(30);//Button3和Button1在垂直方向上面就会增加30个像素的间隙
        GUILayout.Button("Button3", GUILayout.Width(100), GUILayout.Height(50));
        GUILayout.Button("Button4", GUILayout.Width(100), GUILayout.Height(50));
        GUILayout.EndVertical();
        #endregion

        #region 常用的GUI控件
        #region GUI.Button
        //GUI.Button(new Rect(20, 20, 150, 30), "这是一个文字按钮");

        ////绘制纹理按钮
        //GUI.Button(new Rect(20, 60, 150, 30), texture);//texture是在unity上面Script脚本上面拖上图片进行赋值的
        ////绘制一个带图片和文字按钮
        //GUIContent guic = new GUIContent("按钮", texture);
        //GUI.Button(new Rect(20, 100, 150, 30), guic);

        ////设置按钮的样式
        //GUIStyle guis = new GUIStyle();
        //guis.fontSize = 23;
        //guis.alignment = TextAnchor.MiddleCenter;

        ////设置状态样式
        //GUIStyleState guiss = new GUIStyleState();
        //guiss.textColor = Color.white;
        //guiss.background = texture2D;//设置按钮背景图片，texture2D在编辑器上拖图片赋值
        //guis.normal = guiss;//设置按钮正常显示的状态
        //GUIStyleState guissActive = new GUIStyleState();
        //guissActive.textColor = Color.white;
        //guissActive.background = texture2DActive;//设置按钮背景图片，texture2D在编辑器上拖图片赋值
        //guis.active = guissActive;//设置鼠标按下去按钮上显示的状态
        //guis.hover = guissActive;//设置鼠标放在按钮上显示的状态
        //if (GUI.Button(new Rect(20, 140, 150, 30), "样式按钮", guis))//点击后返回true
        //{
        //    Debug.Log("点击了按钮");
        //} 
        #endregion

        #region GUI.Label
        //GUI.color = Color.red;//全局设置颜色，设置后后面的控件都变为红色，直到重新设置颜色
        //GUI.Label(new Rect(20, 180, 100, 50), "label1");
        //GUI.color = Color.blue;
        //GUI.Label(new Rect(20, 200, 100, 50), "label2"); 
        #endregion

        #region GUI.TextField GUI.PasswordField GUI.TextArea
        //userName = GUI.TextField(new Rect(10, 10, 100, 30), userName);
        //password = GUI.PasswordField(new Rect(10, 50, 100, 30), password,'*');
        //remark = GUI.TextArea(new Rect(10, 100, 100, 30),remark);
        //if (GUI.Button(new Rect(10,150,50,30),"登录"))
        //{
        //    Debug.Log(userName + "-"+password+"-"+remark);
        //    if (userName.Equals("admin")&&password.Equals("123"))
        //    {
        //        isSuccess = true;
        //    }
        //    else
        //    {
        //        isSuccess = false;
        //    }
        //}
        //if (isSuccess)
        //{
        //    GUI.Label(new Rect(10, 200, 100, 30), "登录成功！");
        //}
        //else
        //{
        //    GUI.Label(new Rect(10, 200, 100, 30), "登录失败！");
        //}
        #endregion

        #region GUI.Toolbar GUI.Toggle  GUI.HorizontalSlider
        //Tab页，返回值为激活的按钮的序号,三个按钮并排，select为0选中第一个按钮
        select = GUI.Toolbar(new Rect(0, 300, 300, 50), select, new string[] { "功能一", "功能二", "功能三" });
        Debug.Log(select);

        //单选按钮
        //GUIStyle gs = new GUIStyle();
        //GUIStyleState gss = new GUIStyleState();
        //gss.textColor = Color.white;
        //gs.normal = gss;
        //gs.active = gss;
        //GUIContent contenxt = new GUIContent("开关", bug1);
        //if (toggle1)
        //{
        //    contenxt.image = bug2;
        //}
        //// toggle = GUI.Toggle(new Rect(10, 10, 100, 30), toggle, "是否开启声音");
        //toggle1 = GUI.Toggle(new Rect(10, 10, 50, 50), toggle1, contenxt, gs);
        //GUI.Label(new Rect(10, 80, 100, 30), toggle1 + "");

        //水平拖动的Slider,h为Slider赋值
        //h = GUI.HorizontalSlider(new Rect(0, 0, 100, 100), h, 0, 100);
        //Debug.Log(h); 
        #endregion

        #region GUI.BeginScrollView GUI.BeginGroup GUI.Window GUI.SelectionGrid
        //开始滚动视图
        //  public static Vector2 BeginScrollView(Rect position, Vector2 scrollPosition, Rect viewRect, bool alwaysShowHorizontal, bool alwaysShowVertical);
        //position 用于滚动视图在屏幕上矩形的位置
        //scrollPosition 用来显示滚动位置
        //viewRect 滚动视图内使用的矩形
        //vector2 = GUI.BeginScrollView(new Rect(0, 0, 200, 200), vector2, new Rect(0, 0, 200, 200), true, true);
        //GUI.Button(new Rect(0, 0, 50, 50),"Button");
        //GUI.EndScrollView();

        //开始组 将控件都放在一组中，只要组变动，里面的控件都跟着变
        //GUI.BeginGroup(new Rect(10, 100, 200, 400));
        //GUI.Label(new Rect(10, 100, 100, 30), "群组视图1");
        //GUI.Button(new Rect(10, 130, 100, 30), "按钮");
        //GUI.EndGroup();
        //GUI.BeginGroup(new Rect(200, 0, 300, 400));
        //GUI.Label(new Rect(10, 100, 100, 30), "群组视图2");
        //GUI.Button(new Rect(10, 130, 100, 30), "按钮");
        //GUI.EndGroup();

        //弹出窗口
        //必须要把窗口的位置设置成全局变量,窗口里面内容在回调函数里面写
        //rect1 = GUI.Window(0, rect1, win, "窗口");
        //rect2 = GUI.Window(1, rect2, win, "窗口");

        //选择表格
        //selGridId = GUI.SelectionGrid(new Rect(10, 10, 300, 200), selGridId, selString, 2);
        //Debug.Log(selGridId); 
        #endregion

        #region GUILayout.BeginArea
        //区域就是无边框的窗口,Button控件随着区域移动
        //GUILayout.BeginArea(new Rect(0, 50, 200, 200), "Area");
        //GUI.Button(new Rect(0,0,100,50),"Button");
        //GUILayout.EndArea(); 
        #endregion

        #endregion
    }
}
