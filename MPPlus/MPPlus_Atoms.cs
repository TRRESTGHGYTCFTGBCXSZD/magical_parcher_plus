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

using ZenaLib;

namespace MagicalParcherPlus;

//using PartType = class_139;
//using Permissions = enum_149;
using BondType = enum_126;
using BondSite = class_222;
using AtomTypes = class_175;
//using PartTypes = class_191;
using Texture = class_256;

public static class Atoms
{
	public static bool GerioificationVanilla = true;
	public static bool GerioHasHat = false;
	public static bool TricHasHeadphones = true;
	private static byte NewCharactersAtomID = 255;
	private static byte OriginalCharactersAtomID = 254;
	private static byte PTableAtomID = 253;
	private static byte ProtonID = 252;
	private static byte NormalCardinalsID = 251;
	private static byte SwitcherooID = 250;
	private static byte NumberAtomID = 249;
	//speaki is atom id 248

	public static T AndOr<T>(bool dind, T truedind, T falsedind) {
		return dind ? truedind : falsedind;
	}
	//atoms for atomic operations
	public static AtomType Proton;
	public static AtomType[] PTableAtoms = new AtomType[118];
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
	public static bool[] PTableDoNotAdd = {
		false,false,
		false,false,false,false,false,false,false,false,
		false,false,false,false,false,false,false,false,
		false,false,false,false,false,false,false,true, false,false,true, false,false,false,false,false,false,false,
		false,false,false,false,false,false,false,false,false,false,true, false,false,true, false,false,false,false,
		false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true, true, false,true, false,false,false,false,
		false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,
	};
	//public static Dictionary<AtomType, int> PTableAtomsReverse = new();
	//number atoms
	public static AtomType[] NumberAtoms = new AtomType[256];
	//atoms that don't categorize
	public static AtomType Wood, Rock, Limbo, Switcheroo, NumberAtom;
	//character/alternative atoms
	public static AtomType Wordexis, Erabukun, Zena, AnxietyBot;
	public static AtomType Gerio, EZGG, Gabs, Tric, Modrenity, CeminratesBestie, RedZena, BlueZena, Abomination;

	public static List<AtomType> atomsToAdd;
	
	public static Texture WordexisBackAccessories = class_235.method_615("textures/atoms/magicalparcher/wordexis_accessoriesback");
	public static Texture WordexisVisor = class_235.method_615("textures/atoms/magicalparcher/wordexis_accessoriesfront");
	public static Texture TricEars = class_235.method_615("textures/atoms/magicalparcher/tric_ears");
	public static Texture TricBackAccessories = class_235.method_615("textures/atoms/magicalparcher/tric_accessoriesback");
	public static Texture GerioHair = class_235.method_615("textures/atoms/magicalparcher/gerio_hair");
	public static Texture GerioHat = class_235.method_615("textures/atoms/magicalparcher/gerio_hat");
	public static Texture GabsSpikes = class_235.method_615("textures/atoms/magicalparcher/gabs_spikes");
	public static Texture EZGGHat = class_235.method_615("textures/atoms/magicalparcher/ezgg_hat");
	public static Texture ModrenityControlSticks = class_235.method_615("textures/atoms/magicalparcher/modrenity_controlsticks");
	public static Texture ZenaMagnets = class_235.method_615("textures/atoms/magicalparcher/zena_magnets");
	public static Texture RedZenaMagnets = class_235.method_615("textures/atoms/magicalparcher/redzena_magnets");
	public static Texture BlueZenaMagnets = class_235.method_615("textures/atoms/magicalparcher/bluezena_magnets");
	public static Texture CeminratesBestieEars = class_235.method_615("textures/atoms/magicalparcher/ceminratesbestie_ears");
	public static Texture CeminratesBestieFrontAccessories = class_235.method_615("textures/atoms/magicalparcher/ceminratesbestie_accessoriesfront");
	public static Texture AnxietyBotAccessories = class_235.method_615("textures/atoms/magicalparcher/anxietybot_antennas");
	public static Texture saltcircle = class_235.method_615("textures/atoms/salt_diffuse");

	private static readonly Random DoItShakyShaky = new();

	internal static void CreateSwitcherooRecipe(AtomType catalyst,Dictionary<AtomType,int> input,AtomType output){
		Flexibility.SwitcherooRecipes.Add(new SwitcherooRecipe(new MultipleMatcher(catalyst),input,new AtomType[1]{output}));
		Flexibility.SwitcherooRecipes.Add(new SwitcherooRecipe(new MultipleMatcher(output),input,new AtomType[1]{catalyst}));
	}

	public static void AddNewContent() {
		//
		//sound = QApi.loadSound("sounds/activate01",0.2f); API never existed, i think - oh well

		Erabukun = new AtomType()
		{
			/*ID, byte*/field_2283 = NewCharactersAtomID,
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
			/*ID, byte*/field_2283 = NewCharactersAtomID,
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

		Zena = new AtomType()
		{
			/*ID, byte*/field_2283 = NewCharactersAtomID,
			/*Non-local Name*/field_2284 = class_134.method_254("Zena"),
			/*Atomic Name*/field_2285 = class_134.method_253("Zena", string.Empty),
			/*Local name*/field_2286 = class_134.method_253("Zena", string.Empty),
			/*Symbol*/field_2287 = class_235.method_615("textures/atoms/magicalparcher/zena_face"),
			/*Shadow*/field_2288 = class_238.field_1989.field_81.field_599,
			/*Default Graphics struct*/field_2290 = new class_106()
			{
				field_994 = class_235.method_615("textures/atoms/magicalparcher/zena_base"),//salt_diffuse
				field_995 = class_235.method_615("textures/atoms/salt_shade")//salt_shade
			},
			QuintAtomType = "magicalparcherplus:zena"
		};

		Tric = new AtomType()
		{
			/*ID, byte*/field_2283 = OriginalCharactersAtomID,
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

		Gerio = new AtomType()
		{
			/*ID, byte*/field_2283 = OriginalCharactersAtomID,
			/*Non-local Name*/field_2284 = class_134.method_254("Gerio"),
			/*Atomic Name*/field_2285 = class_134.method_253("Gerio", string.Empty),
			/*Local name*/field_2286 = class_134.method_253("Gerio", string.Empty),
			/*Symbol*/field_2287 = class_235.method_615("textures/atoms/magicalparcher/gerio_face"),
			/*Shadow*/field_2288 = class_238.field_1989.field_81.field_599,
			/*Default Graphics struct*/field_2290 = new class_106()
			{
				field_994 = class_235.method_615("textures/atoms/magicalparcher/gerio_base"),//salt_diffuse
				field_995 = class_235.method_615("textures/atoms/salt_shade")//salt_shade
			},
			QuintAtomType = "magicalparcherplus:gerio"
		};

		Gabs = new AtomType()
		{
			/*ID, byte*/field_2283 = OriginalCharactersAtomID,
			/*Non-local Name*/field_2284 = class_134.method_254("Gabs"),
			/*Atomic Name*/field_2285 = class_134.method_253("Gabs", string.Empty),
			/*Local name*/field_2286 = class_134.method_253("Gabs", string.Empty),
			/*Symbol*/field_2287 = class_235.method_615("textures/atoms/magicalparcher/gabs_face"),
			/*Shadow*/field_2288 = class_238.field_1989.field_81.field_599,
			/*Default Graphics struct*/field_2290 = new class_106()
			{
				field_994 = class_235.method_615("textures/atoms/magicalparcher/gabs_base"),//salt_diffuse
				field_995 = class_235.method_615("textures/atoms/salt_shade")//salt_shade
			},
			QuintAtomType = "magicalparcherplus:gabs"
		};

		EZGG = new AtomType()
		{
			/*ID, byte*/field_2283 = OriginalCharactersAtomID,
			/*Non-local Name*/field_2284 = class_134.method_254("EZGG"),
			/*Atomic Name*/field_2285 = class_134.method_253("EZGG", string.Empty),
			/*Local name*/field_2286 = class_134.method_253("EZGG", string.Empty),
			/*Symbol*/field_2287 = class_235.method_615("textures/atoms/magicalparcher/ezgg_face"),
			/*Shadow*/field_2288 = class_238.field_1989.field_81.field_599,
			/*Default Graphics struct*/field_2290 = new class_106()
			{
				field_994 = class_235.method_615("textures/atoms/magicalparcher/ezgg_base"),//salt_diffuse
				field_995 = class_235.method_615("textures/atoms/salt_shade")//salt_shade
			},
			QuintAtomType = "magicalparcherplus:ezgg"
		};

		Modrenity = new AtomType()
		{
			/*ID, byte*/field_2283 = OriginalCharactersAtomID,
			/*Non-local Name*/field_2284 = class_134.method_254("Modrenity"),
			/*Atomic Name*/field_2285 = class_134.method_253("Modrenity", string.Empty),
			/*Local name*/field_2286 = class_134.method_253("Modrenity", string.Empty),
			/*Symbol*/field_2287 = class_235.method_615("textures/atoms/magicalparcher/modrenity_face"),
			/*Shadow*/field_2288 = class_238.field_1989.field_81.field_599,
			/*Default Graphics struct*/field_2290 = new class_106()
			{
				field_994 = class_235.method_615("textures/atoms/magicalparcher/modrenity_base"),//salt_diffuse
				field_995 = class_235.method_615("textures/atoms/salt_shade")//salt_shade
			},
			QuintAtomType = "magicalparcherplus:modrenity"
		};

		Abomination = new AtomType()
		{
			/*ID, byte*/field_2283 = OriginalCharactersAtomID,
			/*Non-local Name*/field_2284 = class_134.method_254("Abomination"),
			/*Atomic Name*/field_2285 = class_134.method_253("Abomination", string.Empty),
			/*Local name*/field_2286 = class_134.method_253("Abomination", string.Empty),
			/*Symbol*/field_2287 = class_235.method_615("textures/atoms/magicalparcher/abomination_face"),
			/*Shadow*/field_2288 = class_238.field_1989.field_81.field_599,
			/*Default Graphics struct*/field_2290 = new class_106()
			{
				field_994 = class_235.method_615("textures/atoms/magicalparcher/modrenity_base"),//salt_diffuse
				field_995 = class_235.method_615("textures/atoms/salt_shade")//salt_shade
			},
			QuintAtomType = "magicalparcherplus:abomination"
		};

		RedZena = new AtomType()
		{
			/*ID, byte*/field_2283 = OriginalCharactersAtomID,
			/*Non-local Name*/field_2284 = class_134.method_254("Red Zena"),
			/*Atomic Name*/field_2285 = class_134.method_253("Red Zena", string.Empty),
			/*Local name*/field_2286 = class_134.method_253("Red Zena", string.Empty),
			/*Symbol*/field_2287 = class_235.method_615("textures/atoms/magicalparcher/redzena_face"),
			/*Shadow*/field_2288 = class_238.field_1989.field_81.field_599,
			/*Default Graphics struct*/field_2290 = new class_106()
			{
				field_994 = class_235.method_615("textures/atoms/magicalparcher/zena_base"),//salt_diffuse
				field_995 = class_235.method_615("textures/atoms/salt_shade")//salt_shade
			},
			QuintAtomType = "magicalparcherplus:redzena"
		};

		BlueZena = new AtomType()
		{
			/*ID, byte*/field_2283 = OriginalCharactersAtomID,
			/*Non-local Name*/field_2284 = class_134.method_254("Blue Zena"),
			/*Atomic Name*/field_2285 = class_134.method_253("Blue Zena", string.Empty),
			/*Local name*/field_2286 = class_134.method_253("Blue Zena", string.Empty),
			/*Symbol*/field_2287 = class_235.method_615("textures/atoms/magicalparcher/bluezena_face"),
			/*Shadow*/field_2288 = class_238.field_1989.field_81.field_599,
			/*Default Graphics struct*/field_2290 = new class_106()
			{
				field_994 = class_235.method_615("textures/atoms/magicalparcher/zena_base"),//salt_diffuse
				field_995 = class_235.method_615("textures/atoms/salt_shade")//salt_shade
			},
			QuintAtomType = "magicalparcherplus:bluezena"
		};

		CeminratesBestie = new AtomType()
		{
			/*ID, byte*/field_2283 = OriginalCharactersAtomID,
			/*Non-local Name*/field_2284 = class_134.method_254("Qix"),
			/*Atomic Name*/field_2285 = class_134.method_253("Qix", string.Empty),
			/*Local name*/field_2286 = class_134.method_253("Qix", string.Empty),
			/*Symbol*/field_2287 = class_235.method_615("textures/atoms/magicalparcher/ceminratesbestie_face"),
			/*Shadow*/field_2288 = class_238.field_1989.field_81.field_599,
			/*Default Graphics struct*/field_2290 = new class_106()
			{
				field_994 = class_235.method_615("textures/atoms/magicalparcher/ceminratesbestie_base"),//salt_diffuse
				field_995 = class_235.method_615("textures/atoms/salt_shade")//salt_shade
			},
			QuintAtomType = "magicalparcherplus:CeminratesBestie"
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

		Switcheroo = new AtomType()
		{
			/*ID, byte*/field_2283 = SwitcherooID,
			/*Non-local Name*/field_2284 = class_134.method_254("Switcheroo"),
			/*Atomic Name*/field_2285 = class_134.method_253("Switcheroo", string.Empty),
			/*Local name*/field_2286 = class_134.method_253("Switcheroo", string.Empty),
			/*Symbol*/field_2287 = class_235.method_615("textures/atoms/magicalparcher/switcheroo_symbol"),
			/*Shadow*/field_2288 = class_238.field_1989.field_81.field_599,
			/*Default Graphics struct*/field_2290 = new class_106()
			{
				field_994 = class_235.method_615("textures/atoms/salt_diffuse"),//salt_diffuse
				field_995 = class_235.method_615("textures/atoms/salt_shade")//salt_shade
			},
			QuintAtomType = "magicalparcherplus:switcheroo"
		};

		Wood = new AtomType()
		{
			/*ID, byte*/field_2283 = NormalCardinalsID,
			/*Non-local Name*/field_2284 = class_134.method_254("Wood"),
			/*Atomic Name*/field_2285 = class_134.method_253("Elemental Wood", string.Empty),
			/*Local name*/field_2286 = class_134.method_253("Wood", string.Empty),
			/*Symbol*/field_2287 = class_235.method_615("textures/atoms/magicalparcher/simple/wordexis"),
			/*Shadow*/field_2288 = class_238.field_1989.field_81.field_599,
			/*Default Graphics struct*/field_2290 = new class_106()
			{
				field_994 = class_235.method_615("textures/atoms/magicalparcher/zena_base"),//salt_diffuse
				field_995 = class_235.method_615("textures/atoms/salt_shade")//salt_shade
			},
			QuintAtomType = "magicalparcherplus:wood"
		};

		Rock = new AtomType()
		{
			/*ID, byte*/field_2283 = NormalCardinalsID,
			/*Non-local Name*/field_2284 = class_134.method_254("Rock"),
			/*Atomic Name*/field_2285 = class_134.method_253("Elemental Rock", string.Empty),
			/*Local name*/field_2286 = class_134.method_253("Rock", string.Empty),
			/*Symbol*/field_2287 = class_235.method_615("textures/atoms/magicalparcher/simple/erabukun"),
			/*Shadow*/field_2288 = class_238.field_1989.field_81.field_599,
			/*Default Graphics struct*/field_2290 = new class_106()
			{
				field_994 = class_235.method_615("textures/atoms/magicalparcher/zena_base"),//salt_diffuse
				field_995 = class_235.method_615("textures/atoms/salt_shade")//salt_shade
			},
			QuintAtomType = "magicalparcherplus:rock"
		};

		Limbo = new AtomType()
		{
			/*ID, byte*/field_2283 = NormalCardinalsID,
			/*Non-local Name*/field_2284 = class_134.method_254("Limbo"),
			/*Atomic Name*/field_2285 = class_134.method_253("Elemental Limbo", string.Empty),
			/*Local name*/field_2286 = class_134.method_253("Limbo", string.Empty),
			/*Symbol*/field_2287 = class_235.method_615("textures/atoms/magicalparcher/simple/zena"),
			/*Shadow*/field_2288 = class_238.field_1989.field_81.field_599,
			/*Default Graphics struct*/field_2290 = new class_106()
			{
				field_994 = class_235.method_615("textures/atoms/magicalparcher/zena_base"),//salt_diffuse
				field_995 = class_235.method_615("textures/atoms/salt_shade")//salt_shade
			},
			QuintAtomType = "magicalparcherplus:limbo"
		};

		AnxietyBot = new AtomType()
		{
			/*ID, byte*/field_2283 = NewCharactersAtomID,
			/*Non-local Name*/field_2284 = class_134.method_254("Anxiety Bot"),
			/*Atomic Name*/field_2285 = class_134.method_253("Anxiety Bot", string.Empty),
			/*Local name*/field_2286 = class_134.method_253("Anxiety Bot", string.Empty),
			/*Symbol*/field_2287 = class_235.method_615("textures/atoms/magicalparcher/anxietybot_face"),
			/*Shadow*/field_2288 = class_238.field_1989.field_81.field_599,
			/*Default Graphics struct*/field_2290 = new class_106()
			{
				field_994 = class_235.method_615("textures/atoms/magicalparcher/anxietybot_base"),//salt_diffuse
				field_995 = class_235.method_615("textures/atoms/salt_shade")//salt_shade
			},
			QuintAtomType = "magicalparcherplus:anxietybot"
		};

		//NumberAtom = new AtomType()
		//{
		//	/*ID, byte*/field_2283 = ProtonID,
		//	/*Non-local Name*/field_2284 = class_134.method_254("Number"),
		//	/*Atomic Name*/field_2285 = class_134.method_253("Number", string.Empty),
		//	/*Local name*/field_2286 = class_134.method_253("Number", string.Empty),
		//	/*Symbol*/field_2287 = class_235.method_615("textures/atoms/magicalparcher/number_symbol"),
		//	/*Shadow*/field_2288 = class_238.field_1989.field_81.field_599,
		//	/*Default Graphics struct*/field_2290 = new class_106()
		//	{
		//		field_994 = class_235.method_615("textures/atoms/magicalparcher/number_base"),//salt_diffuse
		//		field_995 = class_235.method_615("textures/atoms/salt_shade")//salt_shade
		//	},
		//	QuintAtomType = "magicalparcherplus:numberatom"
		//};

		atomsToAdd = new List<AtomType>() { Wood, Rock, Limbo, Proton, Switcheroo, Gerio, Gabs, Tric, EZGG, Modrenity, CeminratesBestie, RedZena, BlueZena, Abomination, Wordexis, Erabukun, Zena, AnxietyBot, };

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
			if (!PTableDoNotAdd[PTableAtomIndex]) {
				bool ThisImageShouldBeTextured = MagicalParcherPlus.DoesThisTextureExists("textures/atoms/magicalparcher/ptableatoms/"+PTableNaming[PTableAtomIndex].ToLower()+"_diffuse");
				AtomType Whatwiyo = new AtomType()
				{
					/*ID, byte*/field_2283 = PTableAtomID,
					/*Non-local Name*/field_2284 = class_134.method_254(PTableNaming[PTableAtomIndex]),
					/*Atomic Name*/field_2285 = class_134.method_253("Elemental "+PTableNaming[PTableAtomIndex], string.Empty),
					/*Local name*/field_2286 = class_134.method_253(PTableNaming[PTableAtomIndex], string.Empty),
					/*Symbol*/field_2287 = class_235.method_615("textures/atoms/magicalparcher/ptableatoms/"+PTableNaming[PTableAtomIndex].ToLower()+"_symbol"),
					/*Shadow*/field_2288 = class_238.field_1989.field_81.field_599,
					/*Default Graphics struct*/field_2290 = new class_106()
					{
						field_994 = AndOr(ThisImageShouldBeTextured,class_235.method_615("textures/atoms/magicalparcher/ptableatoms/"+PTableNaming[PTableAtomIndex].ToLower()+"_diffuse"),class_235.method_615("textures/atoms/salt_diffuse")),//salt_diffuse
						field_995 = class_235.method_615("textures/atoms/salt_shade")//salt_shade
					},
					QuintAtomType = "magicalparcherplus:"+PTableNaming[PTableAtomIndex].ToLower()
				};
				if (!PTableIgnore[PTableAtomIndex]) {
					PTableAtoms[PTableAtomIndex] = Whatwiyo;
					atomsToAdd.Add(Whatwiyo);
				} else {
					Flexibility.addPlaceholderPTableReplacement(Whatwiyo,PTableAtomIndex);
					atomsToAdd.Add(Whatwiyo);
				}
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

		for (int PTableAtomIndex = 0; PTableAtomIndex <= 255; PTableAtomIndex++)
		{
			AtomType Whatwiyo = new AtomType()
			{
				/*ID, byte*/field_2283 = NumberAtomID,
				/*Non-local Name*/field_2284 = class_134.method_254("Number "+(sbyte)PTableAtomIndex),
				/*Atomic Name*/field_2285 = class_134.method_253("Number "+(sbyte)PTableAtomIndex, string.Empty),
				/*Local name*/field_2286 = class_134.method_253("Number "+(sbyte)PTableAtomIndex, string.Empty),
				/*Symbol*/field_2287 = class_235.method_615("textures/atoms/magicalparcher/number_symbol"),
				/*Shadow*/field_2288 = class_238.field_1989.field_81.field_599,
				/*Default Graphics struct*/field_2290 = new class_106()
				{
					field_994 = class_235.method_615("textures/atoms/magicalparcher/number_base"),//salt_diffuse
					field_995 = class_235.method_615("textures/atoms/salt_shade")//salt_shade
				},
				QuintAtomType = "magicalparcherplus:number_"+PTableAtomIndex
			};
			NumberAtoms[PTableAtomIndex] = Whatwiyo;
			QApi.AddAtomType(Whatwiyo);
		}
		//assign ptable atoms manually
		PTableAtoms[25] = AtomTypes.field_1684;
		PTableAtoms[28] = AtomTypes.field_1682;
		PTableAtoms[46] = AtomTypes.field_1685;
		PTableAtoms[49] = AtomTypes.field_1683;
		PTableAtoms[78] = AtomTypes.field_1686;
		PTableAtoms[79] = AtomTypes.field_1680;
		PTableAtoms[81] = AtomTypes.field_1681;
		Flexibility.addTriplexCondition(EZGG,AtomTypes.field_1678);
		Flexibility.addTriplexCondition(EZGG,EZGG);
		//Flexibility.addTriplexCondition(Abomination,EZGG);
		//Flexibility.addTriplexCondition(Abomination,Abomination);
		//rules disabled to be consistent with other atom set
		Flexibility.addCardinalificationRule(AtomTypes.field_1680,AtomTypes.field_1675);
		Flexibility.addLiquidationRule(AtomTypes.field_1675,AtomTypes.field_1680);
		Flexibility.addCardinalificationRule(CeminratesBestie,Gerio);
		Flexibility.addLiquidationRule(Gerio,CeminratesBestie);
		Flexibility.addMetallificationRule(AtomTypes.field_1680,AtomTypes.field_1681);
		Flexibility.addDemetallificationRule(AtomTypes.field_1681,AtomTypes.field_1680);
		Flexibility.DemetallificationExplosionMeta.Add(AtomTypes.field_1684);
		Flexibility.DemetallificationExplosionMeta.Add(AtomTypes.field_1682);
		Flexibility.DemetallificationExplosionMeta.Add(AtomTypes.field_1685);
		Flexibility.DemetallificationExplosionMeta.Add(AtomTypes.field_1683);
		Flexibility.DemetallificationExplosionMeta.Add(AtomTypes.field_1686);
		Flexibility.addCalcinatorRule(Atoms.Gabs,Atoms.Gerio);
		Flexibility.addCalcinatorRule(Atoms.Modrenity,Atoms.Gerio);
		Flexibility.addCalcinatorRule(Atoms.Tric,Atoms.Gerio);
		Flexibility.addCalcinatorRule(Atoms.EZGG,Atoms.Gerio);
		Flexibility.addCalcinatorRule(Atoms.Wordexis,Atoms.Gerio);
		Flexibility.addCalcinatorRule(Atoms.Erabukun,Atoms.Gerio);
		Flexibility.addCalcinatorRule(Atoms.Wood,AtomTypes.field_1675);
		Flexibility.addCalcinatorRule(Atoms.Rock,AtomTypes.field_1675);
		Flexibility.addDuplicatorCastable(Atoms.Gerio,Atoms.Gabs);
		Flexibility.addDuplicatorCastable(Atoms.Gerio,Atoms.Modrenity);
		Flexibility.addDuplicatorCastable(Atoms.Gerio,Atoms.Tric);
		Flexibility.addDuplicatorCastable(Atoms.Gerio,Atoms.EZGG);
		Flexibility.addDuplicatorCastable(Atoms.Gerio,Atoms.Wordexis);
		Flexibility.addDuplicatorCastable(Atoms.Gerio,Atoms.Erabukun);
		Flexibility.addDuplicatorCastable(AtomTypes.field_1675,Atoms.Wood);
		Flexibility.addDuplicatorCastable(AtomTypes.field_1675,Atoms.Rock);
		Dictionary<AtomType,int> wanafo = new Dictionary<AtomType,int>();
		wanafo.Add(Atoms.Modrenity,1);
		wanafo.Add(Atoms.EZGG,1);
		wanafo.Add(Atoms.Gabs,1);
		wanafo.Add(Atoms.Tric,1);
		Flexibility.UnifyRecipes.Add(new UnshapedRecipe(wanafo,new AtomType[1] { Atoms.Abomination }));
		wanafo = new Dictionary<AtomType,int>();
		wanafo.Add(Atoms.Abomination,1);
		Flexibility.DisperseRecipes.Add(new UnshapedRecipe(wanafo,new AtomType[4] { Atoms.Modrenity, Atoms.EZGG, Atoms.Gabs, Atoms.Tric }));
		wanafo = new Dictionary<AtomType,int>();
		wanafo.Add(Atoms.Gerio,2);
		Flexibility.AnimismusRecipes.Add(new UnshapedRecipe(wanafo,new AtomType[2] { Atoms.RedZena, Atoms.BlueZena }));
		wanafo = new Dictionary<AtomType,int>();
		wanafo.Add(Atoms.RedZena,1);
		wanafo.Add(Atoms.BlueZena,1);
		Flexibility.AntiAnimismusRecipes.Add(new UnshapedRecipe(wanafo,new AtomType[2] { Atoms.Gerio, Atoms.Gerio }));
		wanafo = new Dictionary<AtomType,int>();
		wanafo.Add(AtomTypes.field_1687,1);
		wanafo.Add(AtomTypes.field_1688,1);
		Flexibility.AntiAnimismusRecipes.Add(new UnshapedRecipe(wanafo,new AtomType[2] { AtomTypes.field_1675, AtomTypes.field_1675 }));
		Flexibility.GerioificationRecipes.Add(
			new ShapedRecipe(
				new MultipleMatcher[]{
					new (new AtomType[2] { AtomTypes.field_1675, AtomTypes.field_1680 }),
					new (AtomTypes.field_1690),
				}
				,new AtomType[1] { Atoms.Switcheroo }
			)
		);
		Flexibility.GerioificationRecipes.Add(
			new ShapedRecipe(
				new MultipleMatcher[]{
					new (new AtomType[2] { Atoms.Gerio, Atoms.CeminratesBestie }),
					new (Atoms.Abomination),
				}
				,new AtomType[1] { Atoms.Switcheroo }
			)
		);
		wanafo = new Dictionary<AtomType,int>();
		wanafo.Add(Atoms.Switcheroo,1);
		CreateSwitcherooRecipe(
			AtomTypes.field_1680,
			wanafo,
			Atoms.Gerio
		);
		wanafo = new Dictionary<AtomType,int>();
		wanafo.Add(Atoms.Switcheroo,1);
		CreateSwitcherooRecipe(
			AtomTypes.field_1685,
			wanafo,
			Atoms.CeminratesBestie
		);
		wanafo = new Dictionary<AtomType,int>();
		wanafo.Add(Atoms.Switcheroo,3);
		CreateSwitcherooRecipe(
			AtomTypes.field_1676,
			wanafo,
			Atoms.Modrenity
		);
		wanafo = new Dictionary<AtomType,int>();
		wanafo.Add(Atoms.Switcheroo,3);
		CreateSwitcherooRecipe(
			AtomTypes.field_1677,
			wanafo,
			Atoms.Tric
		);
		wanafo = new Dictionary<AtomType,int>();
		wanafo.Add(Atoms.Switcheroo,3);
		CreateSwitcherooRecipe(
			AtomTypes.field_1678,
			wanafo,
			Atoms.EZGG
		);
		wanafo = new Dictionary<AtomType,int>();
		wanafo.Add(Atoms.Switcheroo,3);
		CreateSwitcherooRecipe(
			AtomTypes.field_1679,
			wanafo,
			Atoms.Gabs
		);
		wanafo = new Dictionary<AtomType,int>();
		wanafo.Add(Atoms.Switcheroo,3);
		CreateSwitcherooRecipe(
			AtomTypes.field_1687,
			wanafo,
			Atoms.RedZena
		);
		wanafo = new Dictionary<AtomType,int>();
		wanafo.Add(Atoms.Switcheroo,3);
		CreateSwitcherooRecipe(
			AtomTypes.field_1688,
			wanafo,
			Atoms.BlueZena
		);
		wanafo = new Dictionary<AtomType,int>();
		wanafo.Add(Atoms.Switcheroo,3);
		CreateSwitcherooRecipe(
			AtomTypes.field_1681,
			wanafo,
			Atoms.NumberAtoms[1]
		);
		wanafo = new Dictionary<AtomType,int>();
		wanafo.Add(Atoms.Switcheroo,3);
		CreateSwitcherooRecipe(
			AtomTypes.field_1682,
			wanafo,
			Atoms.NumberAtoms[2]
		);
		wanafo = new Dictionary<AtomType,int>();
		wanafo.Add(Atoms.Switcheroo,3);
		CreateSwitcherooRecipe(
			AtomTypes.field_1683,
			wanafo,
			Atoms.NumberAtoms[3]
		);
		wanafo = new Dictionary<AtomType,int>();
		wanafo.Add(Atoms.Switcheroo,3);
		CreateSwitcherooRecipe(
			AtomTypes.field_1684,
			wanafo,
			Atoms.NumberAtoms[4]
		);
		wanafo = new Dictionary<AtomType,int>();
		wanafo.Add(Atoms.Switcheroo,3);
		CreateSwitcherooRecipe(
			AtomTypes.field_1685,
			wanafo,
			Atoms.NumberAtoms[5]
		);
		wanafo = new Dictionary<AtomType,int>();
		wanafo.Add(Atoms.Switcheroo,3);
		CreateSwitcherooRecipe(
			AtomTypes.field_1686,
			wanafo,
			Atoms.NumberAtoms[6]
		);
		wanafo = new Dictionary<AtomType,int>();
		wanafo.Add(Atoms.Switcheroo,3);
		CreateSwitcherooRecipe(
			AtomTypes.field_1690,
			wanafo,
			Atoms.Abomination
		);
		wanafo = new Dictionary<AtomType,int>();
		wanafo.Add(Atoms.Switcheroo,3);
		CreateSwitcherooRecipe(
			Atoms.Wood,
			wanafo,
			Atoms.Wordexis
		);
		wanafo = new Dictionary<AtomType,int>();
		wanafo.Add(Atoms.Switcheroo,3);
		CreateSwitcherooRecipe(
			Atoms.Rock,
			wanafo,
			Atoms.Erabukun
		);
		wanafo = new Dictionary<AtomType,int>();
		wanafo.Add(Atoms.Switcheroo,3);
		CreateSwitcherooRecipe(
			Atoms.Limbo,
			wanafo,
			Atoms.Zena
		);
		On.Editor.method_927 += RenderExtraStuff;
		QApi.RunAfterCycle((sim, first) => {
			List<Molecule> moleculeList = sim.field_3823;
			var seb = sim.field_3818;

			foreach(var molecule in moleculeList){
				foreach(KeyValuePair<HexIndex, Atom> entry in molecule.method_1100()){
					if (Flexibility.applyPlaceholderPTableReplacement(entry.Value.field_2275,out int WinosPrime))
								molecule.method_1106(PTableAtoms[WinosPrime], entry.Key);
				}
			}
		});
	}
	//param_4582 = scale
	//param_4583 = transparency
	//param_4584 = drop %
	//param_4585 = unknown
	//param_4586 = angle
	//param_4587 = unknown
	internal static void RenderExtraStuff(On.Editor.orig_method_927 orig, AtomType type, Vector2 position, float param_4582, float param_4583, float param_4584, float param_4585, float param_4586, float param_4587, Texture overrideShadow, Texture maskM, bool param_4590)
	{
		if (GerioificationVanilla)
		{
			if (type == AtomTypes.field_1677) //visuallize earth as tric
			{
				RenderExtraStuff(orig, Tric, position, param_4582, param_4583, param_4584, param_4585, param_4586, param_4587, overrideShadow, maskM, param_4590);
				return;
			}
			if (type == AtomTypes.field_1679) //visuallize water as gabs
			{
				RenderExtraStuff(orig, Gabs, position, param_4582, param_4583, param_4584, param_4585, param_4586, param_4587, overrideShadow, maskM, param_4590);
				return;
			}
			if (type == AtomTypes.field_1678) //visuallize fire as ezgg
			{
				RenderExtraStuff(orig, EZGG, position, param_4582, param_4583, param_4584, param_4585, param_4586, param_4587, overrideShadow, maskM, param_4590);
				return;
			}
			if (type == AtomTypes.field_1676) //visuallize air as modrenity
			{
				RenderExtraStuff(orig, Modrenity, position, param_4582, param_4583, param_4584, param_4585, param_4586, param_4587, overrideShadow, maskM, param_4590);
				return;
			}
			if (type == AtomTypes.field_1675) //visuallize salt as gerio
			{
				RenderExtraStuff(orig, Gerio, position, param_4582, param_4583, param_4584, param_4585, param_4586, param_4587, overrideShadow, maskM, param_4590);
				return;
			}
			if (type == AtomTypes.field_1687) //visuallize vitae as red zena
			{
				RenderExtraStuff(orig, RedZena, position, param_4582, param_4583, param_4584, param_4585, param_4586, param_4587, overrideShadow, maskM, param_4590);
				return;
			}
			if (type == AtomTypes.field_1688) //visuallize mors as blue zena
			{
				RenderExtraStuff(orig, BlueZena, position, param_4582, param_4583, param_4584, param_4585, param_4586, param_4587, overrideShadow, maskM, param_4590);
				return;
			}
			if (type == AtomTypes.field_1690) //visuallize quint as abomination
			{
				RenderExtraStuff(orig, Abomination, position, param_4582, param_4583, param_4584, param_4585, param_4586, param_4587, overrideShadow, maskM, param_4590);
				return;
			}
		}
		Color tecolor = Color.White.WithAlpha(param_4583*param_4584);
		//if (type == Switcheroo)
		//{
		//	float Red = (float)(Math.Sin(new struct_27(Time.Now().Ticks).method_603()*4)+1)/2f;
		//	float Green = (float)(Math.Sin((new struct_27(Time.Now().Ticks).method_603()+(Math.PI*4/3))*4)+1)/2f;
		//	float Blue = (float)(Math.Sin((new struct_27(Time.Now().Ticks).method_603()+(Math.PI*2/3))*4)+1)/2f;
		//	class_135.method_263(saltcircle, new Color(Red,Green,Blue,param_4583*param_4584), position - new Vector2(35, 35) * param_4582, new Vector2(70, 70) * param_4582);
		//}

		if (type == Wordexis)
		{
			class_135.method_263(WordexisBackAccessories, tecolor, position - new Vector2(60, 60) * param_4582, new Vector2(120, 120) * param_4582);
		}
		if (type == Tric)
		{
			class_135.method_263(TricEars, tecolor, position - new Vector2(60, 60) * param_4582, new Vector2(120, 120) * param_4582);
			if (TricHasHeadphones)
				class_135.method_263(TricBackAccessories, tecolor, position - new Vector2(60, 60) * param_4582, new Vector2(120, 120) * param_4582);
		}
		if (type == CeminratesBestie)
		{
			class_135.method_263(CeminratesBestieEars, tecolor, position - new Vector2(60, 60) * param_4582, new Vector2(120, 120) * param_4582);
		}
		if (type == Gabs)
		{
			class_135.method_263(GabsSpikes, tecolor, position - new Vector2(60, 60) * param_4582, new Vector2(120, 120) * param_4582);
		}
		if (type == Modrenity)
		{
			class_135.method_263(ModrenityControlSticks, tecolor, position - new Vector2(60, 60) * param_4582, new Vector2(120, 120) * param_4582);
		}
		if (type == Zena)
		{
			class_135.method_263(ZenaMagnets, tecolor, position - new Vector2(60, 60) * param_4582, new Vector2(120, 120) * param_4582);
		}
		if (type == RedZena)
		{
			class_135.method_263(RedZenaMagnets, tecolor, position - new Vector2(60, 60) * param_4582, new Vector2(120, 120) * param_4582);
		}
		if (type == BlueZena)
		{
			class_135.method_263(BlueZenaMagnets, tecolor, position - new Vector2(60, 60) * param_4582, new Vector2(120, 120) * param_4582);
		}
		if (type == Abomination)
		{
			class_135.method_263(TricEars, tecolor, position - new Vector2(60, 60) * param_4582, new Vector2(120, 120) * param_4582);
			class_135.method_263(GabsSpikes, tecolor, position - new Vector2(60, 60) * param_4582, new Vector2(120, 120) * param_4582);
			class_135.method_263(ModrenityControlSticks, tecolor, position - new Vector2(60, 60) * param_4582, new Vector2(120, 120) * param_4582);
		}

		if (type == AnxietyBot)
		{
			position += new Vector2((DoItShakyShaky.Next(9) - 4) / 4f, (DoItShakyShaky.Next(9) - 4) / 4f);
			class_135.method_263(AnxietyBotAccessories, tecolor, position - new Vector2(60, 60) * param_4582, new Vector2(120, 120) * param_4582);
		}
		//behind atom
		orig(type, position, param_4582, param_4583, param_4584, param_4585, param_4586, param_4587, overrideShadow, maskM, param_4590);
		//in front of atom
		if (type == Wordexis)
		{
			class_135.method_263(WordexisVisor, tecolor, position - new Vector2(30, 30) * param_4582, new Vector2(60, 60) * param_4582);
		}
		if (type == Gerio)
		{
			class_135.method_263(GerioHair, tecolor, position - new Vector2(60, 60) * param_4582, new Vector2(120, 120) * param_4582);
			if (GerioHasHat)
				class_135.method_263(GerioHat, tecolor, position - new Vector2(60, 30) * param_4582, new Vector2(120, 120) * param_4582);
		}
		if (type == EZGG)
		{
			class_135.method_263(EZGGHat, tecolor, position - new Vector2(60, 30) * param_4582, new Vector2(120, 120) * param_4582);
		}
		if (type == CeminratesBestie)
		{
			class_135.method_263(CeminratesBestieFrontAccessories, tecolor, position - new Vector2(0, 30) * param_4582, new Vector2(60, 60) * param_4582);
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