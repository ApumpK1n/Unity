﻿using UnityEngine;
using System.Collections;


public abstract class Layers
{
	public const int DefaultMask = 1;
	public const int TransparentFXMask = 1 << 1;
	public const int IgnoreRaycastMask = 1 << 2;
	public const int WaterMask = 1 << 4;
	public const int UIMask = 1 << 5;
	public const int PlaneMask = 1 << 8;

}