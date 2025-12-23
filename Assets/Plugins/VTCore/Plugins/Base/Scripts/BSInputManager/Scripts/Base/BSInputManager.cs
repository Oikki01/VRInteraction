using HuaRuXR.Base;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace HuaRuXR.BSInput
{
    /// <summary>
    /// 输入管理器
    /// 实际应该叫物品切换及操作管理器
    /// </summary>
    public class BSInputManager : IBSInputManager
    {
        #region 继承自接口，可供外部使用的属性
        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; set; }
        #endregion

        #region 管理器自用属性、字段，协助实现框架功能

        /// <summary>
        /// 存储按键操作、操作没记的字典
        /// </summary>
        private Dictionary<KeyCode, InputType> KeyPut { get; set ; }

        /// <summary>
        /// 存储操作枚举、回调事件的字典
        /// </summary>
        private Dictionary<InputType, Action<InputType>> InputActions { get ; set ; }

        #endregion

        #region 框架时序、流程、轮循、关闭、清理等方法，虽公开，但一般不供外部使用，自用

        /// <summary>
        /// 框架初始化
        /// </summary>
        /// <param name="module"></param>
        public void Initialize(ModuleData module)
        {
            Log.Error("初始化BSInputManager");
            KeyPut = new Dictionary<KeyCode, InputType>();
            InputActions = new Dictionary<InputType, Action<InputType>>();
        }

        /// <summary>
        /// 框架轮循
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            UpDateInput();
        }

        /// <summary>
        /// 框架轮循
        /// </summary>
        public void FixedUpdate()
        {


        }

        /// <summary>
        /// 框架关闭（退出程序时）
        /// </summary>
        public void Shutdown()
        {

        }

        /// <summary>
        /// 框架清理（退出房间时）
        /// </summary>
        public void ClearScenarioAbout()
        {
            InputActions.Clear();
            KeyPut.Clear();
        }



        #endregion

        #region  继承自接口，可供外部使用的方法
        /// <summary>
        /// 注册输入事件
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="input"></param>
        /// <param name="action"></param>
        public void RegistBSInput(KeyCode keyCode, InputType input, Action<InputType> action)
        {
            if (!KeyPut.ContainsKey(keyCode)) 
            {
                KeyPut.Add(keyCode, input);
                InputActions.Add(input, action);
            }
            else
            {
                Log.Error("已注册了按键 "+keyCode.ToString()+" 为输入 "+ KeyPut[keyCode].ToString(),"，本次注册会覆盖为 "+ input.ToString()+" 及其事件");
                //先注销事件
                InputActions.Remove(KeyPut[keyCode]);
                KeyPut[keyCode]=input;
                InputActions.Add(input,action);
            }
           
        }

        /// <summary>
        /// 注销注册输入事件
        /// </summary>
        /// <param name="keyCode"></param>
        public void UnRegistBSInput(KeyCode keyCode)
        {
            if (KeyPut.ContainsKey(keyCode))
            {
                if (InputActions.ContainsKey(KeyPut[keyCode])) 
                {
                    InputActions.Remove(KeyPut[keyCode]);
                }
                KeyPut.Remove(keyCode);
            }
        }
        #endregion


        #region 框架私有自用方法，协助实现框架功能
        /// <summary>
        /// 输入检测、轮循
        /// </summary>
        private void UpDateInput() 
        {
            if (KeyPut==null|| KeyPut.Count<1) 
            {
                return;
            }
            if (InputActions==null|| InputActions.Count<1) 
            {
                return;
            }
            foreach (var item in KeyPut)
            {
                if (UnityEngine.Input.GetKeyDown(item.Key))
                {
                    Log.Error("按下键 " + item.Key.ToString());
                    if (InputActions.ContainsKey(item.Value)) 
                    {
                        InputActions[item.Value].Invoke(item.Value);
                        Log.Error("执行输入 " + item.Value.ToString());
                    }
                }
            }
        }

       
        #endregion
    }
}
