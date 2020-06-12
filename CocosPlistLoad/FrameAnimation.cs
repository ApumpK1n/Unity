using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace CocosPlistParser{


    /// <summary>
    /// frame animation res from cocos2d-x
    /// </summary>
    public class FrameAnimation
    {
        private string name;
        private int currentframe = 0;
        private float delay = 0.2f;
        private float updateTime = 0;
        private List<SpriteFrame> spriteFrames = new List<SpriteFrame>();
        private Action updateAction;

        public Image Image { get; set; }

        // deepcopy
        public FrameAnimation DeepClone()
        {
            FrameAnimation frameAnimation = new FrameAnimation();
            frameAnimation.currentframe = this.currentframe;
            frameAnimation.name = this.name;
            frameAnimation.Image = null;
            frameAnimation.delay = delay;
            frameAnimation.updateTime = updateTime;
            frameAnimation.spriteFrames = spriteFrames;
            frameAnimation.updateAction = updateAction;
            return frameAnimation;
        }

        public void SetUpdateAction(Action update)
        {
            this.updateAction = update;
        }

        public Sprite GetFirstFrameSprite()
        {
            if (this.spriteFrames != null)
            {
                return this.spriteFrames[0].GetSprite();
            }
            return null;
        }

        public int FrameCount()
        {
            if (this.spriteFrames != null)
            {
                return this.spriteFrames.Count;
            }
            return 0;
        }

        public static Dictionary<string, FrameAnimation> CreateWithFile(string plist, string textureType)
        {
            string path = Path.GetDirectoryName(plist) + "/" + Path.GetFileNameWithoutExtension(plist);
            Debug.Log("CreateWithFile: " + path);
            List<SpriteFrame> lstSpriteFrame = SpriteFrameMgr.AddSpriteFrameWithPlist(path, textureType);
            if (lstSpriteFrame == null || lstSpriteFrame.Count == 0)
            {
                Debug.LogError("frames is empty:" + path);
                return null;
            }

            Dictionary<string, FrameAnimation> animations = new Dictionary<string, FrameAnimation>();

            foreach (SpriteFrame spriteFrame in lstSpriteFrame)
            {
                string animationName = GetAnimationName(spriteFrame.name);
                if (!animations.ContainsKey(animationName))
                {
                    FrameAnimation animation = new FrameAnimation();
                    animation.name = animationName;
                    animation.spriteFrames.Add(spriteFrame);
                    animations[animationName] = animation;
                }
                else
                {
                    FrameAnimation animation = animations[animationName];
                    animation.spriteFrames.Add(spriteFrame);
                }
            }
            return animations;
        }

        public static string GetAnimationName(string sFrame)
        {
            if (sFrame.IndexOf("_") != -1)
            {
                sFrame = sFrame.Substring(0, sFrame.IndexOf("_"));
            }
            return sFrame;
        }

        public void SetUpdateTime(float updateTime)
        {
            this.delay = updateTime;
            this.updateTime = updateTime;
        }

        public void Update()
        {
            updateTime += Time.deltaTime;
            if (updateTime >= delay)
            {
                updateTime = 0;
                if (currentframe >= spriteFrames.Count)
                {
                    currentframe = 0;
                }
                SpriteFrame spriteFrame = spriteFrames[currentframe];
                this.SetSprite(spriteFrame.sprite);
                currentframe++;
                if (this.updateAction != null)
                {
                    this.updateAction();
                }
            }
        }

        private void SetSprite(Sprite sprite)
        {
            if (this.Image != null)
            {
                this.Image.sprite = sprite;
                int sizeX = sprite.texture.width;
                int sizeY = sprite.texture.height;
                this.Image.gameObject.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX, sizeY);
            }
        }
    }

}