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
	public static QuintessentialMod self;
	public override Type SettingsType => typeof(Settei);
	public override void Load() {
        self = this;
        Settings = new Settei();
	}
	public override void LoadPuzzleContent() {
		if (QuintessentialLoader.CodeMods.Any(mod => mod.Meta.Name == "HalvingMetallurgy"))
		{
			Atoms.PTableAtoms[3] = HalvingMetallurgy.Atoms.Beryl;
			Atoms.PTableIgnore[3] = true;
			Atoms.PTableAtoms[27] = HalvingMetallurgy.Atoms.Nickel;
			Atoms.PTableIgnore[27] = true;
			Atoms.PTableAtoms[29] = HalvingMetallurgy.Atoms.Zinc;
			Atoms.PTableIgnore[29] = true;
			Atoms.PTableAtoms[75] = HalvingMetallurgy.Atoms.Osmium;
			Atoms.PTableIgnore[75] = true;
		}
		if (QuintessentialLoader.CodeMods.Any(mod => mod.Meta.Name == "UnstableElements"))
		{
			Atoms.PTableAtoms[91] = FindModAtom("UnstableElements:uranium"); //why it is private
			Atoms.PTableIgnore[91] = true;
			//Flexibility.addExtraAtomicException(FindModAtom("UnstableElements:uranium_0_1"),91); //it is inaccessible to this atom 
			//Flexibility.addExtraAtomicException(FindModAtom("UnstableElements:uranium_0_2"),91); //and it has strict type checking
			//Flexibility.addExtraAtomicException(FindModAtom("UnstableElements:uranium_1_0"),91); //so it won't detect isotopes
			//Flexibility.addExtraAtomicException(FindModAtom("UnstableElements:uranium_1_1"),91);
			//Flexibility.addExtraAtomicException(FindModAtom("UnstableElements:uranium_1_2"),91);
			//Flexibility.addExtraAtomicException(FindModAtom("UnstableElements:uranium_2_0"),91);
			//Flexibility.addExtraAtomicException(FindModAtom("UnstableElements:uranium_2_1"),91);
			//Flexibility.addExtraAtomicException(FindModAtom("UnstableElements:uranium_2_2"),91);
		}
		if (QuintessentialLoader.CodeMods.Any(mod => mod.Meta.Name == "MetalQuintessence"))
		{
			Atoms.PTableAtoms[23] = MetalQuintessence.MetalQuintessenceAtoms.Chromium;
			Atoms.PTableIgnore[23] = true;
		}
		Atoms.AddNewContent();
		Parts.AddNewContent();
		//On.MoleculeEditorScreen.method_50 += AddElementsToMoleculeEditor;
		if (QuintessentialLoader.CodeMods.Any(mod => mod.Meta.Name == "HalvingMetallurgy"))
		{
			Flexibility.addMetallificationRule(HalvingMetallurgy.Atoms.Quickcopper,HalvingMetallurgy.Atoms.Beryl);
			Flexibility.addDemetallificationRule(HalvingMetallurgy.Atoms.Beryl,HalvingMetallurgy.Atoms.Quickcopper);
		}
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
    }
	public override void Unload() {
		Atoms.Unload();
		Parts.Unload();
	}
	//------------------------- END HOOKING -------------------------//
	public override void PostLoad() { }
}