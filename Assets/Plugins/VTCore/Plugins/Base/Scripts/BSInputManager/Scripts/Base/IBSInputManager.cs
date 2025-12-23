using HuaRuXR.Base;
using System;
using UnityEngine;
namespace HuaRuXR.BSInput
{
    /// <summary>
    /// 输入管理器接口
    /// 实际应该叫物品切换及操作接口
    /// </summary>
    public interface IBSInputManager : BSimCoreModule
    {

        /// <summary>
        /// 注册按键、事件
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="input"></param>
        /// <param name="action"></param>
        void RegistBSInput(KeyCode keyCode, InputType input,Action<InputType> action);

        /// <summary>
        /// 注销按键、事件
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="input"></param>
        /// <param name="action"></param>
        void UnRegistBSInput(KeyCode keyCode);

    }
    /// <summary>
    /// 输入类型枚举
    /// </summary>
    public enum InputType
    {
        /// <summary>
        /// 开火操作输入
        /// </summary>
        Fire = 0,

        /// <summary>
        /// 切换为主武器
        /// </summary>
        MainWeapon = 1,

        /// <summary>
        /// 切换为副武器
        /// </summary>
        SubWeapon = 2,

        /// <summary>
        /// 切换为冷兵器
        /// </summary>
        CodeWeapon = 3,

        /// <summary>
        /// 切换为投掷武器
        /// </summary>
        ThrowWeapon = 4,

        /// <summary>
        /// 切换为操作类延时、触发武器
        /// 如地雷、定时炸弹等
        /// </summary>
        DelayWeapon = 5,
        
        /// <summary>
        /// 卸载主武器，丢弃到地面
        /// </summary>
        DiscarMainWeapon=6,

        /// <summary>
        /// 拾取主武器
        /// 放进背包还是装配到手上？？
        /// 手上没有武器，可捡起
        /// 手上有武器，不可捡起
        /// </summary>
        PickUpMainWeapon
    }
}
