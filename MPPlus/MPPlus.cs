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

public class MagicalParcherPlus : QuintessentialMod
{
	public static QuintessentialMod self;
	public override Type SettingsType => typeof(Settei);
	public override void Load() {
        self = this;
        Settings = new Settei();
	}
	public override void LoadPuzzleContent() {
		Atoms.AddNewContent();
		Parts.AddNewContent();
		On.MoleculeEditorScreen.method_50 += AddElementsToMoleculeEditor;
		if (QuintessentialLoader.CodeMods.Any(mod => mod.Meta.Name == "HalvingMetallurgy"))
		{
			Flexibility.addMetallificationRule(HalvingMetallurgy.Atoms.Quickcopper,HalvingMetallurgy.Atoms.Beryl);
			Flexibility.addDemetallificationRule(HalvingMetallurgy.Atoms.Beryl,HalvingMetallurgy.Atoms.Quickcopper);
		}
	}
	static MethodInfo method_1130_info = typeof(MoleculeEditorScreen).GetMethod("method_1130", BindingFlags.Instance | BindingFlags.NonPublic);
	private void AddElementsToMoleculeEditor(On.MoleculeEditorScreen.orig_method_50 orig, MoleculeEditorScreen self, float param_4858)
	{
		orig(self, param_4858);
		// add new atoms to the editor palette
		for (int i = 0; i < Atoms.atomsToAdd.Count(); i++) method_1130_info.Invoke(self, new object[] { new Vector2(560, 440 - 30 * i), Atoms.atomsToAdd[i], true });
	}
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