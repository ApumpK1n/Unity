using System;
using System.Security.Cryptography;
using UnityEngine;

namespace CocosPlistParser
{
	public class SpriteFrame
    {
		public Sprite sprite = null;
		public string name;
		public Vector2 size;
		public Rect rect;

		public Sprite GetSprite()
        {
			return this.sprite;
        }

		public static SpriteFrame CreateWithFrameDict(FrameDataDict frameDataDict, Texture2D bigTexture, string textureType)
        {
			Texture2D texture;
			if(textureType == TextureType.Restore)
            {
				texture = Restore(frameDataDict, bigTexture);
            }
            else
            {
				texture = JustSplit(frameDataDict, bigTexture);
            }

			Rect rect = new Rect(new Vector2(0, 0), new Vector2(texture.width, texture.height));
			Sprite sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
			sprite.name = frameDataDict.name;

			SpriteFrame spriteFrame = new SpriteFrame();
			spriteFrame.sprite = sprite;
			spriteFrame.name = frameDataDict.name;
			spriteFrame.rect = rect;
			spriteFrame.size = new Vector2(frameDataDict.width, frameDataDict.height);

			return spriteFrame;
        }

		// 仅从大图中裁剪出小图
		public static Texture2D JustSplit(FrameDataDict frameDataDict, Texture2D bigTexture)
        {
			int sampleWidth = frameDataDict.width;
			int sampleHeight = frameDataDict.height;
			int destWidth = sampleWidth;
			int destHeight = sampleHeight;

			Texture2D destTexture = new Texture2D(destWidth, destHeight, bigTexture.format, false);
			//旋转时，宽高互换
			if (frameDataDict.rotated)
            {
				sampleWidth = frameDataDict.height;
				sampleHeight = frameDataDict.width;
            }

			//起始位置(Y轴需变换, 受旋转影响)
			int startPosX = frameDataDict.x;
			int startPosY = bigTexture.height - (frameDataDict.y + sampleHeight);

			//(x,y)对应 y*width + x
			Color[] colors = bigTexture.GetPixels(startPosX, startPosY, sampleWidth, sampleHeight);

			// 设置像素，采样
			for(int x = 0; x < destWidth; x++)
            {
				for(int y=0; y<destHeight; y++)
                {
					if (frameDataDict.rotated) // 顺时针旋转
					{
						// 旋转时，目标图中的坐标(x, y) 对应采样区坐标为(y, height-1-x)
						int index = (sampleHeight - 1 - x) * sampleHeight + y;
						destTexture.SetPixel(x, y, colors[index]);
					}
                    else
                    {
						//没有旋转时，目标图中的坐标(x, y), 对应采样区坐标为(x, y)
						int index = y * sampleWidth + x;
						destTexture.SetPixel(x, y, colors[index]);
                    }
                }
            }

			destTexture.Apply();
			return destTexture;
        }

		// 从大图裁剪出小图，并还原到原始大小(恢复其四周被裁剪的透明像素)
		public static Texture2D Restore(FrameDataDict frameDataDict, Texture2D bigTexture)
        {
			int sampleWidth = frameDataDict.width;
			int sampleHeight = frameDataDict.height;
			int destWidth = frameDataDict.sourceSizeWidth;
			int destHeight = frameDataDict.sourceSizeHeight;

			//计算偏移值(不受旋转影响)
			int offsetLX = frameDataDict.offsetWidth + frameDataDict.sourceSizeWidth / 2 - frameDataDict.width / 2;
			int offsetBY = -(-frameDataDict.offsetHeight + frameDataDict.height / 2 - frameDataDict.sourceSizeHeight / 2);

			Texture2D destTexture = new Texture2D(destWidth, destHeight, bigTexture.format, false);

			if (frameDataDict.rotated)
            {
				sampleWidth = frameDataDict.height;
				sampleHeight = frameDataDict.width;
            }

			// 起始位置(Y轴需变换，受旋转影响)
			int startPosx = frameDataDict.x;
			int startPosY = bigTexture.height - (frameDataDict.y + sampleHeight);

			Color[] colors = bigTexture.GetPixels(startPosx, startPosY, sampleWidth, sampleHeight);

			// 设置像素，采样
			for(int x=0; x< destWidth; x++)
            {
				for(int y=0; y<destHeight; y++)
                {
					if (x >= offsetLX && x < frameDataDict.width + offsetLX && y >= offsetBY && y < frameDataDict.height + offsetBY)
                    {
						if (frameDataDict.rotated)
                        {
							//旋转时，目标图中的坐标(x, y),对应采样区坐标为(y-offsetY, height - 1- (x-offsetLX))
							int index = (sampleHeight - 1 - (x - offsetLX)) * sampleWidth + (y - offsetBY);
							destTexture.SetPixel(x, y, colors[index]);
						}
                        else
                        {
							// 没有旋转时，目标图中(x, y) 对应采样区(x-offsetLX, y-offsetBY)
							int index = (y - offsetBY) * sampleWidth + (x - offsetLX);
							destTexture.SetPixel(x, y, colors[index]);
                        }
                    }
                    else
                    {
						//四周颜色透明
						destTexture.SetPixel(x, y, new Color(0, 0, 0, 0));
                    }
                }
            }
			destTexture.Apply();
			return destTexture;
        }
    }
}