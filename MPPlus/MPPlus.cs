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

using AtomTypes = class_175;

public class MagicalParcherPlus : QuintessentialMod
{
	internal static AtomType FindModAtom(string QuintAtom){
		foreach(AtomType don in QApi.ModAtomTypes)
		{
			if (don.QuintAtomType == QuintAtom) return don;
		}	
		return AtomTypes.field_1675; //salt as fallback
	}
	public static bool DoesThisTextureExists(string WhatTexture){
		WhatTexture = Path.Combine("Content", WhatTexture);
		class_256 TextureFuck = new class_256
		{
			field_2056 = Index2.Zero,
			field_2057 = IntPtr.Zero,
			field_2058 = false,
			field_2059 = (enum_3)2,
			field_2062 = WhatTexture,
			field_2063 = DateTime.MinValue
		};
		string text = null;
		if (TextureFuck.field_2062.method_1085())
		{
			text = TextureFuck.field_2062.method_1087();
			if (TextureFuck.field_2062.method_1087().StartsWith("Content"))
			{
				for (int num = QuintessentialLoader.ModContentDirectories.Count - 1; num >= 0; num--)
				{
					string path = QuintessentialLoader.ModContentDirectories[num];
					try
					{
						TextureFuck.field_2062 = Path.Combine(path, text);
						return Renderer.orig_method_1339(TextureFuck);
					}
					catch (Exception e)
					{}
					finally
					{
						TextureFuck.field_2062 = text;
					}
				}
			}
		}
		try
		{
			return Renderer.orig_method_1339(TextureFuck);
		}
		catch (Exception e2)
		{}
		return false;
	}
	public static MethodInfo PrivateMethod<T>(string method) => typeof(T).GetMethod(method, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
	public static QuintessentialMod self;
	public override Type SettingsType => typeof(Settei);
	public override void Load() {
		self = this;
		Settings = new Settei();
	}
	public override void LoadPuzzleContent() {
		//foreach(var mod in QuintessentialLoader.CodeMods){Logger.Log("[MP+] Mod Name: " + mod.Meta.Name);}
		if (QuintessentialLoader.CodeMods.Any(mod => mod.Meta.Name == "HalvingMetallurgy"))
		{
			Logger.Log("[MP+] Detected Halving Metallurgy, Adding 5 Elements");
			MPPlusExtensions.AddHalvingMetallurgy();
			if (QuintessentialLoader.CodeMods.Any(mod => mod.Meta.Name == "Vacancy"))
			{
				Logger.Log("[MP+] Detected Vacancy Extension for Halving Metallurgy");
				MPPlusExtensions.AddVacancy_ExtHalvingMetallurgy();
			}
		}
		if (QuintessentialLoader.CodeMods.Any(mod => mod.Meta.Name == "UnstableElements"))
		{
			Logger.Log("[MP+] Detected Unstable Elements, Adding Uranium");
			MPPlusExtensions.AddUnstableElements();
		}
		if (QuintessentialLoader.CodeMods.Any(mod => mod.Meta.Name == "MetalQuintessence"))
		{
			Logger.Log("[MP+] Detected Metal Quintessence, Adding Chromium");
			MPPlusExtensions.AddMetalQuintessence();
		}
		if (QuintessentialLoader.CodeMods.Any(mod => mod.Meta.Name == "Alchemical_Inversions")) // thanks quintessential for replacing spaces with underscores
		{
			Logger.Log("[MP+] Detected Alchemical Inversions, Adding Yttrium");
			MPPlusExtensions.AddAlchemicalInversions();
		}
		if (QuintessentialLoader.CodeMods.Any(mod => mod.Meta.Name == "Neuvolics"))
		{
			Logger.Log("[MP+] Detected Neuvolics, Adding Iridium");
			MPPlusExtensions.AddNeuvolics();
		}
		AirWave.AddNewContent();
		Atoms.AddNewContent();
		Parts.AddNewContent();
		if (QuintessentialLoader.CodeMods.Any(mod => mod.Meta.Name == "HalvingMetallurgy"))
		{
			MPPlusExtensions.AddHalvingMetallurgyLater();
		}
		//On.MoleculeEditorScreen.method_50 += AddElementsToMoleculeEditor;
	}
	static MethodInfo method_1130_info = typeof(MoleculeEditorScreen).GetMethod("method_1130", BindingFlags.Instance | BindingFlags.NonPublic);
	//private void AddElementsToMoleculeEditor(On.MoleculeEditorScreen.orig_method_50 orig, MoleculeEditorScreen self, float param_4858)
	//{
	//	orig(self, param_4858);
	//	// add new atoms to the editor palette
	//	for (int i = 0; i < Atoms.atomsToAdd.Count(); i++) method_1130_info.Invoke(self, new object[] { new Vector2(560, 440 - 30 * i), Atoms.atomsToAdd[i], true });
	//}
	public override void ApplySettings()
	{
		base.ApplySettings();
		Settei SET = (Settei)Settings;

		Atoms.GerioificationVanilla = SET.GerioificationVanilla;
		Atoms.GerioHasHat = SET.GerioHasHat;
		Atoms.TricHasHeadphones = SET.TricHasHeadphones;
		Parts.BypassPartRules = SET.BypassPartRules;
	}
	public override void Unload() {
		Parts.Unload();
		Atoms.Unload();
		AirWave.Unload();
	}
	//------------------------- END HOOKING -------------------------//
	public override void PostLoad() { }
}