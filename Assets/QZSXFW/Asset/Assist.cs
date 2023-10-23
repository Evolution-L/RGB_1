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
    /// AB包类型
    /// </summary>
    public enum AssetBundleType
    {
        packet = 1,
        monofile = 2,
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
