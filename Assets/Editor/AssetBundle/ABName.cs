using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetExt
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

public static class ABName
{
    public static void SetABName(string type_, string path_)
    {
        switch (type_)
        {
            case AssetExt.ANIMSPRITE:
            case AssetExt.PREFAB:
            case AssetExt.UI:
            case AssetExt.SHADER:
            {
                    BuildSingleAsset(path_);
                    break;
                }

            case AssetExt.TEXTURE:
                {
                    BuildTextureAsset(path_);
                    break;
                }

            case AssetExt.MODEL:
                {
                    BuildModelAsset(path_);
                    break;
                }

            case AssetExt.scene:
                {
                    BuildMultiAsset(path_, ".unity");
                    break;
                }
            case AssetExt.ANIM:
                {
                    BuildAnimClipAsset(path_);
                    break;
                }
            case AssetExt.EFFECT:
                {
                    BuildSingleAsset(path_, ".prefab");
                    break;
                }

            case AssetExt.AUDIO:
                {
                    BuildAudioAsset(path_);
                    break;
                }

            case AssetExt.SMODEL:
                {
                    BuildMultiAsset(path_, ".prefab");
                    break;
                }

            case AssetExt.FONT:
                {
                    BuildFontAsset(path_);
                    break;
                }

            case AssetExt.ASSET:
                {
                    BuildSingleAsset(path_);
                    break;
                }
            case AssetExt.LuaScript:
                {
                    BuildLUAAsset(path_);
                    break;
                }
            case AssetExt.MAT:
                {
                    //BuildMaterialAsset(path_);
                    BuildSingleAsset(path_);
                    break;
                }
        }
    }

    private static void BuildAudioAsset(string path_)
    {
        throw new NotImplementedException();
    }

    private static void BuildFontAsset(string path_)
    {
        throw new NotImplementedException();
    }

    private static void BuildLUAAsset(string path_)
    {
        throw new NotImplementedException();
    }

    private static void BuildSingleAsset(string path_, string v)
    {
        throw new NotImplementedException();
    }

    private static void BuildAnimClipAsset(string path_)
    {
        throw new NotImplementedException();
    }

    private static void BuildMultiAsset(string path_, string v)
    {
        throw new NotImplementedException();
    }

    private static void BuildModelAsset(string path_)
    {
        throw new NotImplementedException();
    }

    private static void BuildTextureAsset(string path_)
    {
        throw new NotImplementedException();
    }

    private static void BuildSingleAsset(object path_)
    {
        throw new NotImplementedException();
    }
}
