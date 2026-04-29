using MonoMod.RuntimeDetour;
using MonoMod.Utils;
using Quintessential;
using Quintessential.Settings;
using SDL2;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

namespace MagicalParcherPlus;

using PartType = class_139;
using PartTypes = class_191;
using Permissions = enum_149;
using AtomTypes = class_175;
using Texture = class_256;

public static class AirWave
{
	public static Sound PockEight;
	public static Sound[] Exploder = new Sound[3];

	private static bool ContentLoaded = false;
	public static void AddNewContent() {
		if (ContentLoaded) return;
		ContentLoaded = true;
		PockEight = new (){
			field_4060 = Path.GetFileNameWithoutExtension(MagicalParcherPlus.self.Meta.PathDirectory + "/Content/sounds/magical_parcher/pock_eight.wav"),
			field_4061 = class_158.method_375(MagicalParcherPlus.self.Meta.PathDirectory + "/Content/sounds/magical_parcher/pock_eight.wav")
		};
		Exploder[0] = new (){
			field_4060 = Path.GetFileNameWithoutExtension(MagicalParcherPlus.self.Meta.PathDirectory + "/Content/sounds/magical_parcher/explode3.wav"),
			field_4061 = class_158.method_375(MagicalParcherPlus.self.Meta.PathDirectory + "/Content/sounds/magical_parcher/explode3.wav")
		};
		Exploder[1] = new (){
			field_4060 = Path.GetFileNameWithoutExtension(MagicalParcherPlus.self.Meta.PathDirectory + "/Content/sounds/magical_parcher/explode4.wav"),
			field_4061 = class_158.method_375(MagicalParcherPlus.self.Meta.PathDirectory + "/Content/sounds/magical_parcher/explode4.wav")
		};
		Exploder[2] = new (){
			field_4060 = Path.GetFileNameWithoutExtension(MagicalParcherPlus.self.Meta.PathDirectory + "/Content/sounds/magical_parcher/explode5.wav"),
			field_4061 = class_158.method_375(MagicalParcherPlus.self.Meta.PathDirectory + "/Content/sounds/magical_parcher/explode5.wav")
		};
		FieldInfo field = typeof(class_11).GetField("field_52", BindingFlags.Static | BindingFlags.NonPublic);
		Dictionary<string, float> volumeDictionary = (Dictionary<string, float>)field.GetValue(null);

		volumeDictionary.Add("pock_eight", 0.2f);
		volumeDictionary.Add("explode3", 0.2f);
		volumeDictionary.Add("explode4", 0.2f);
		volumeDictionary.Add("explode5", 0.2f);
		On.class_201.method_540 += AirWave.Method_540;
	}

	public static void Unload()
	{
		On.class_201.method_540 -= AirWave.Method_540;
	}

	public static void Method_540(On.class_201.orig_method_540 orig, class_201 self)
	{
		orig(self);
		PockEight.field_4062 = false;
		Exploder[0].field_4062 = false;
		Exploder[1].field_4062 = false;
		Exploder[2].field_4062 = false;
	}
}