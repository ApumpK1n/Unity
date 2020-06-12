using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CocosPlistParser 
{
	public class SpriteFrameMgr
    {
        public static List<SpriteFrame> AddSpriteFrameWithPlist(string plist, string textureType)
        {
            PlistDictionary dPlist = new PlistDictionary();
            dPlist.LoadWithFile(plist);

            return LoadWithFrameDict(dPlist, plist, textureType);
        }

        private static PlistMetaData ParseMetaData(PlistDictionary dMetaData)
        {
            PlistMetaData metaData = new PlistMetaData();
            metaData.format = (int)dMetaData["format"];
            metaData.realTextureFileName = dMetaData["realTextureFileName"] as string;
            metaData.size = PlistDictionary.ParseVector2(dMetaData["size"] as string);
            metaData.smartupdate = dMetaData["smartupdate"] as string;
            metaData.textureFileName = dMetaData["textureFileName"] as string;
            return metaData;
        }

        private static List<FrameDataDict> ParseFrames(PlistDictionary dFrames, int format)
        {
            List<FrameDataDict> frames = new List<FrameDataDict>();
            foreach( KeyValuePair<string, object> kv in dFrames)
            {
                if (kv.Value is PlistDictionary)
                {
                    FrameDataDict frameDataDict = new FrameDataDict();
                    frameDataDict.name = kv.Key;
                    PlistDictionary frameDict = kv.Value as PlistDictionary;
                    if (format == 2)
                    {
                        RectInt frame = PlistDictionary.ParseRectInt(frameDict["frame"] as string);
                        frameDataDict.x = frame.x;
                        frameDataDict.y = frame.y;
                        frameDataDict.width = frame.width;
                        frameDataDict.height = frame.height;
                    }
                    else
                    {
                        RectInt frame = PlistDictionary.ParseRectInt(frameDict["textureRect"] as string);
                        frameDataDict.x = frame.x;
                        frameDataDict.y = frame.y;
                        frameDataDict.width = frame.width;
                        frameDataDict.height = frame.height;
                    }

                    if (format == 2)
                    {
                        Vector2 offset = PlistDictionary.ParseVector2(frameDataDict["offset"] as string);
                        frameDataDict.offsetHeight = (int)offset.y;
                        frameDataDict.offsetWidth = (int)offset.x;
                    }
                    else
                    {
                        Vector2 offset = PlistDictionary.ParseVector2(frameDataDict["spriteOffset"] as string);
                        frameDataDict.offsetHeight = (int)offset.y;
                        frameDataDict.offsetWidth = (int)offset.x;
                    }

                    if (format == 2)
                    {
                        frameDataDict.rotated = (bool)frameDict["rotated"];
                    }
                    else
                    {
                        frameDataDict.rotated = (bool)frameDict["textureRotated"];
                    }

                    if (format == 2)
                    {
                        Vector2 size = PlistDictionary.ParseVector2(frameDict["sourceSize"] as string);
                        frameDataDict.sourceSizeWidth = (int)size.x;
                        frameDataDict.sourceSizeHeight = (int)size.y;
                    }
                    else
                    {
                        Vector2 size = PlistDictionary.ParseVector2(frameDict["spriteSourceSize"] as string);
                        frameDataDict.sourceSizeWidth = (int)size.x;
                        frameDataDict.sourceSizeHeight = (int)size.y;
                    }

                    frames.Add(frameDataDict);
                }
            }
            return frames;
        }

        private static List<SpriteFrame> LoadWithFrameDict(PlistDictionary dPlist, string plist, string textureType)
        {
            // parse metadata
            var meta = dPlist["metadata"] as PlistDictionary;
            PlistMetaData metaData = ParseMetaData(meta);
            
            // parse frames
            List<FrameDataDict> frames = ParseFrames(dPlist["frames"] as PlistDictionary, metaData.format);              
            string name = metaData.realTextureFileName.SubString(0, metaData.realTextureFileName.Length - 4);
            string plistPath = Path.GetDirectoryName(plist);
            string parentPath = plistPath.Substring(0, plistPath.LastIndexOf('/'));

            //load png
            Texture2D bigTexture = (Texture2D)Resources.Load(parentPath + "/png/" + name);
            if (bigTexture == null) return null;
            List<SpriteFrame> lstSpriteFrame = new List<SpriteFrame>();
            foreach(FrameDataDict frameDataDict in frames){
                SpriteFrame spriteFrame = SpriteFrame.CreateWithFrameDict(frameDataDict, bigTexture, textureType);
                lstSpriteFrame.Add(spriteFrame);
            }
            return lstSpriteFrame; 
        }
    }
}
