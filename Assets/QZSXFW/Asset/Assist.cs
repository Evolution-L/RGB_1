/*
///此文件定义枚举, 结构体 等用于辅助资源系统的工具
*/

using System.Collections.Generic;

namespace QZSXFrameWork.Asset
{
    public static class Assist
    {
        // 加密字符串
        public static string abEncryptString = "x(~dKGz.,#iWc.3[@7CQDXtq&Z!?;3LM";
        // 数据偏移值
        public static uint abOffset = 7;
        
        // 加密列表
        public static List<string> encryptList = new List<string>
        {

        };
    }

    /// <summary>
    /// AB包内资源的类型
    /// </summary>
    public enum AssetBundleExportType
    {
        /// 独立资源（背景音乐、场景、球、球员、时装、特效、简模、配置档、字体、prefab、UI、部分UI贴图）
        Asset = 1,

        //操作型集合资源（音效、图标ICON、球员动画anim文件、动画曲线asset文件）    
        UseSet = 2,

        ///不可操作集合资源（texture依赖）
        NotUseSet = 3,
    }

    /// <summary>
    /// 加载状态
    /// </summary>
    public enum LoadState
    {
        State_None = 0,
        State_Loading = 1,
        State_Error = 2,
        State_Complete = 3
    }

    public static class AssetExt
    {
        public const string UI = "ui";
        public const string MODEL = "model";
        public const string CONFIG = "conf";
        public const string TEXTURE = "texture";
        public const string ANIMSPRITE = "animspritenew";
        public const string PREFAB = "prefab";
        public const string scene = "scene";
        public const string ANIM = "anim";
        public const string EFFECT = "effect";
        public const string AUDIO = "audio";
        public const string SMODEL = "smodel";
        public const string FONT = "font";
        public const string ASSET = "asset";
        public const string MAT = "mat";
        public const string LuaScript = "luascript";
        public const string SHADER = "shader";

    }


    public delegate void Handler(object arg_);

}
