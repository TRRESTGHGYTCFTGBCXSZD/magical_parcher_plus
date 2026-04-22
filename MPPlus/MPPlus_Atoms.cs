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

//using PartType = class_139;
//using Permissions = enum_149;
using BondType = enum_126;
using BondSite = class_222;
using AtomTypes = class_175;
//using PartTypes = class_191;
//using Texture = class_256;

internal static class Atoms
{
	public static AtomType Wordexis, Erabukun;
	public static List<AtomType> atomsToAdd;
	private static IDetour hook_Sim_method_1832;

	public static void AddNewContent() {
		//
		//sound = QApi.loadSound("sounds/activate01",0.2f); API never existed, i think - oh well

		Erabukun = new AtomType()
		{
			/*ID, byte*/field_2283 = 254,
			/*Non-local Name*/field_2284 = class_134.method_254("Erabukun"),
			/*Atomic Name*/field_2285 = class_134.method_253("Erabukun", string.Empty),
			/*Local name*/field_2286 = class_134.method_253("Erabukun", string.Empty),
			/*Symbol*/field_2287 = class_235.method_615("textures/atoms/erabukun_face"),
			/*Shadow*/field_2288 = class_238.field_1989.field_81.field_599,
			/*Default Graphics struct*/field_2290 = new class_106()
			{
				field_994 = class_235.method_615("textures/atoms/erabukun_base"),//salt_diffuse
				field_995 = class_235.method_615("textures/atoms/salt_shade")//salt_shade
			},
        	QuintAtomType = "magicalparcherplus:erabukun"
		};

		Wordexis = new AtomType()
		{
			/*ID, byte*/field_2283 = 255,
			/*Non-local Name*/field_2284 = class_134.method_254("Wordexis"),
			/*Atomic Name*/field_2285 = class_134.method_253("Wordexis", string.Empty),
			/*Local name*/field_2286 = class_134.method_253("Wordexis", string.Empty),
			/*Symbol*/field_2287 = class_235.method_615("textures/atoms/wordexis_face"),
			/*Shadow*/field_2288 = class_238.field_1989.field_81.field_599,
			/*Default Graphics struct*/field_2290 = new class_106()
			{
				field_994 = class_235.method_615("textures/atoms/wordexis_base"),//salt_diffuse
				field_995 = class_235.method_615("textures/atoms/salt_shade")//salt_shade
			},
        	QuintAtomType = "magicalparcherplus:wordexis"
		};

		atomsToAdd = new List<AtomType>() { Wordexis, Erabukun, };

		//add atoms to internal dictionary
		var old_len = AtomTypes.field_1691.Length;
		Array.Resize(ref AtomTypes.field_1691, AtomTypes.field_1691.Length + atomsToAdd.Count());

		for (int i = 0; i < atomsToAdd.Count(); i++)
		{
			AtomTypes.field_1691[old_len + i] = atomsToAdd[i];
		}
	}

	public static void Unload() { }
}