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
using Texture = class_256;

internal static class Atoms
{
    public static bool GerioificationVanilla = true;
    private static byte ErabukunAtomID = 255;
    private static byte WordexisAtomID = 254;
    private static byte TricAtomID = 253;
    private static byte PTableAtomID = 252;
    private static byte ProtonID = 251;
    private static string[] PTableNaming = {
		"Hydrogen","Helium",
		"Lithium","Berylium","Boron","Carbon","Nitrogen","Oxygen","Fluorine","Neon",
		"Sodium","Magnesium","Aluminium","Phosphorus","Silicon","Sulfur","Chlorine","Argon",
		"Potassium","Calcium","Scandium","Titanium","Vanadium","Chromium","Manganese","Iron","Cobalt","Nickel","Copper","Zinc","Gallium","Germanium","Arsenic","Selenium","Bromine","Krypton",
		"Rubidium","Strontium","Yttrium","Zirconium","Niobium","Molybdenum","Technetium","Ruthenium","Rhodium","Palladium","Silver","Cadmium","Indium","Tin","Antimony","Tellurium","Iodine","Xenon",
		"Caesium","Barium","Lanthanum","Cerium","Praseodymium","Neodymium","Promethium","Samarium","Europium","Gadolinium","Terbium","Dysprosium","Holmium","Erbium","Thulium","Ytterbium","Lutetium","Hafnium","Tantalum","Tungsten","Rhenium","Osmium","Iridium","Platinum","Gold","Mercury","Thallium","Lead","Bismuth","Polonium","Astatine","Radon",
		"Francium","Radium","Actinium","Thorium","Protactinium","Uranium","Neptunium","Plutonium","Americium","Curium","Berkelium","Californium","Einsteinium","Fermium","Mendelevium","Nobelium","Lawrencium","Rutherfordium","Dubnium","Seaborgium","Bohrium","Hassium","Meitnerium","Darmstadtium","Roentgenium","Copernicium","Nihonium","Flerovium","Moscovium","Livermorium","Tennessine","Oganesson"
	};
    public static bool[] PTableIgnore = {
		false,false,
		false,false,false,false,false,false,false,false,
		false,false,false,false,false,false,false,false,
		false,false,false,false,false,false,false,true, false,false,true, false,false,false,false,false,false,false,
		false,false,false,false,false,false,false,false,false,false,true, false,false,true, false,false,false,false,
		false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true, true, false,true, false,false,false,false,
		false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
	};
    public static bool[] PTableTextured = {
		false,true, 
		false,false,false,false,false,false,true, false,
		false,false,false,false,false,false,false,false,
		false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
		false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
		false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
		false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
	};

	public static T AndOr<T>(bool dind, T truedind, T falsedind) {
        if (dind) return truedind;
		return falsedind;
	}
    public static AtomType[] PTableAtoms = new AtomType[118];
	public static Dictionary<AtomType, int> PTableAtomsReverse = new();
    //atoms that don't categorize
	public static AtomType Wordexis, Erabukun, Tric, Proton;
	public static List<AtomType> atomsToAdd;
	public static Texture WordexisBackAccessories = class_235.method_615("textures/atoms/magicalparcher/wordexis_accessoriesback");
	public static Texture WordexisVisor = class_235.method_615("textures/atoms/magicalparcher/wordexis_accessoriesfront");
	public static Texture TricBackAccessories = class_235.method_615("textures/atoms/magicalparcher/tric_accessoriesback");

	public static void AddNewContent() {
		//
		//sound = QApi.loadSound("sounds/activate01",0.2f); API never existed, i think - oh well

		Erabukun = new AtomType()
		{
			/*ID, byte*/field_2283 = ErabukunAtomID,
			/*Non-local Name*/field_2284 = class_134.method_254("Erabukun"),
			/*Atomic Name*/field_2285 = class_134.method_253("Erabukun", string.Empty),
			/*Local name*/field_2286 = class_134.method_253("Erabukun", string.Empty),
			/*Symbol*/field_2287 = class_235.method_615("textures/atoms/magicalparcher/erabukun_face"),
			/*Shadow*/field_2288 = class_238.field_1989.field_81.field_599,
			/*Default Graphics struct*/field_2290 = new class_106()
			{
				field_994 = class_235.method_615("textures/atoms/magicalparcher/erabukun_combined"),//salt_diffuse
				field_995 = class_235.method_615("textures/atoms/salt_shade")//salt_shade
			},
        	QuintAtomType = "magicalparcherplus:erabukun"
		};

		Wordexis = new AtomType()
		{
			/*ID, byte*/field_2283 = WordexisAtomID,
			/*Non-local Name*/field_2284 = class_134.method_254("Wordexis"),
			/*Atomic Name*/field_2285 = class_134.method_253("Wordexis", string.Empty),
			/*Local name*/field_2286 = class_134.method_253("Wordexis", string.Empty),
			/*Symbol*/field_2287 = class_235.method_615("textures/atoms/magicalparcher/wordexis_face"),
			/*Shadow*/field_2288 = class_238.field_1989.field_81.field_599,
			/*Default Graphics struct*/field_2290 = new class_106()
			{
				field_994 = class_235.method_615("textures/atoms/magicalparcher/wordexis_base"),//salt_diffuse
				field_995 = class_235.method_615("textures/atoms/salt_shade")//salt_shade
			},
        	QuintAtomType = "magicalparcherplus:wordexis"
		};

		Tric = new AtomType()
		{
			/*ID, byte*/field_2283 = TricAtomID,
			/*Non-local Name*/field_2284 = class_134.method_254("Tric"),
			/*Atomic Name*/field_2285 = class_134.method_253("Tric", string.Empty),
			/*Local name*/field_2286 = class_134.method_253("Tric", string.Empty),
			/*Symbol*/field_2287 = class_235.method_615("textures/atoms/magicalparcher/tric_face"),
			/*Shadow*/field_2288 = class_238.field_1989.field_81.field_599,
			/*Default Graphics struct*/field_2290 = new class_106()
			{
				field_994 = class_235.method_615("textures/atoms/magicalparcher/tric_base"),//salt_diffuse
				field_995 = class_235.method_615("textures/atoms/salt_shade")//salt_shade
			},
        	QuintAtomType = "magicalparcherplus:tric"
		};

		Proton = new AtomType()
		{
			/*ID, byte*/field_2283 = ProtonID,
			/*Non-local Name*/field_2284 = class_134.method_254("Proton"),
			/*Atomic Name*/field_2285 = class_134.method_253("Proton", string.Empty),
			/*Local name*/field_2286 = class_134.method_253("Proton", string.Empty),
			/*Symbol*/field_2287 = class_235.method_615("textures/atoms/magicalparcher/proton_symbol"),
			/*Shadow*/field_2288 = class_238.field_1989.field_81.field_599,
			/*Default Graphics struct*/field_2290 = new class_106()
			{
				field_994 = class_235.method_615("textures/atoms/magicalparcher/proton_diffuse"),//salt_diffuse
				field_995 = class_235.method_615("textures/atoms/salt_shade")//salt_shade
			},
        	QuintAtomType = "magicalparcherplus:proton"
		};

		atomsToAdd = new List<AtomType>() { Wordexis, Erabukun, Tric, Proton, };

		//add atoms to internal dictionary
		//var old_len = AtomTypes.field_1691.Length;
		//Array.Resize(ref AtomTypes.field_1691, AtomTypes.field_1691.Length + atomsToAdd.Count());

		for (int i = 0; i < atomsToAdd.Count(); i++)
		{
			//AtomTypes.field_1691[old_len + i] = atomsToAdd[i];
			QApi.AddAtomType(atomsToAdd[i]);
		}

		atomsToAdd = new List<AtomType>() {};

		for (int PTableAtomIndex = 0; PTableAtomIndex < 118; PTableAtomIndex++)
		{
			if (!PTableIgnore[PTableAtomIndex]) {
				PTableAtoms[PTableAtomIndex] = new AtomType()
				{
					/*ID, byte*/field_2283 = PTableAtomID,
					/*Non-local Name*/field_2284 = class_134.method_254(PTableNaming[PTableAtomIndex]),
					/*Atomic Name*/field_2285 = class_134.method_253("Elemental "+PTableNaming[PTableAtomIndex], string.Empty),
					/*Local name*/field_2286 = class_134.method_253(PTableNaming[PTableAtomIndex], string.Empty),
					/*Symbol*/field_2287 = class_235.method_615("textures/atoms/magicalparcher/ptableatoms/"+PTableNaming[PTableAtomIndex].ToLower()+"_symbol"),
					/*Shadow*/field_2288 = class_238.field_1989.field_81.field_599,
					/*Default Graphics struct*/field_2290 = new class_106()
					{
						field_994 = AndOr(PTableTextured[PTableAtomIndex],class_235.method_615("textures/atoms/magicalparcher/ptableatoms/"+PTableNaming[PTableAtomIndex].ToLower()+"_diffuse"),class_235.method_615("textures/atoms/salt_diffuse")),//salt_diffuse
						field_995 = class_235.method_615("textures/atoms/salt_shade")//salt_shade
					},
        			QuintAtomType = "magicalparcherplus:"+PTableNaming[PTableAtomIndex].ToLower()
				};
				atomsToAdd.Add(PTableAtoms[PTableAtomIndex]);
			}
		}

		//add atoms to internal dictionary
		//old_len = AtomTypes.field_1691.Length;
		//Array.Resize(ref AtomTypes.field_1691, AtomTypes.field_1691.Length + atomsToAdd.Count());

		for (int i = 0; i < atomsToAdd.Count(); i++)
		{
			//AtomTypes.field_1691[old_len + i] = atomsToAdd[i];
			QApi.AddAtomType(atomsToAdd[i]);
		}
		//assign ptable atoms manually
		PTableAtoms[25] = AtomTypes.field_1684;
		PTableAtoms[28] = AtomTypes.field_1682;
		PTableAtoms[46] = AtomTypes.field_1685;
		PTableAtoms[49] = AtomTypes.field_1683;
		PTableAtoms[78] = AtomTypes.field_1686;
		PTableAtoms[79] = AtomTypes.field_1680;
		PTableAtoms[81] = AtomTypes.field_1681;
		On.Editor.method_927 += RenderExtraStuff;
	}
    internal static void RenderExtraStuff(On.Editor.orig_method_927 orig, AtomType type, Vector2 position, float param_4582, float param_4583, float param_4584, float param_4585, float param_4586, float param_4587, Texture overrideShadow, Texture maskM, bool param_4590)
    {
        if (GerioificationVanilla && type == AtomTypes.field_1677) //visuallize earth as tric
        {
            RenderExtraStuff(orig, Tric, position, param_4582, param_4583, param_4584, param_4585, param_4586, param_4587, overrideShadow, maskM, param_4590);
            return;
        }
        Color tecolor = Color.White.WithAlpha(param_4583 * param_4585);
        if (type.QuintAtomType == "magicalparcherplus:wordexis")
        {
	        class_135.method_263(WordexisBackAccessories, tecolor, position - new Vector2(60, 60), new Vector2(120, 120));
        }
        if (type.QuintAtomType == "magicalparcherplus:tric")
        {
	        class_135.method_263(TricBackAccessories, tecolor, position - new Vector2(60, 60), new Vector2(120, 120));
        }
        orig(type, position, param_4582, param_4583, param_4584, param_4585, param_4586, param_4587, overrideShadow, maskM, param_4590);
        if (type.QuintAtomType == "magicalparcherplus:wordexis")
        {
	        class_135.method_263(WordexisVisor, tecolor, position - new Vector2(30, 30), new Vector2(60, 60));
        }
    }

	public static bool IsAndGetPTableAtom(AtomType WhatAtom, out int AtomIndex) {
		for (int i = 0; i < 118; i++)
		{
			if (WhatAtom == PTableAtoms[i]) {
				AtomIndex = i;
				return true;
			}
		}
		if (Flexibility.applyExtraAtomicException(WhatAtom,out int AtomIndexD)) {
			AtomIndex = AtomIndexD;
			return true;
		}
		AtomIndex = 0;
		return false;
	}

	public static void Unload()
    {
		On.Editor.method_927 -= RenderExtraStuff;
    }
}