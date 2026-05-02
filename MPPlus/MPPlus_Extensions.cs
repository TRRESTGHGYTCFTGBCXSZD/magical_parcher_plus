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

using AtomTypes = class_175;

internal class MPPlusExtensions
{
	public static void AddHalvingMetallurgy() {
		Atoms.PTableAtoms[3] = HalvingMetallurgy.Atoms.Beryl;
		Atoms.PTableIgnore[3] = true;
		Atoms.PTableAtoms[27] = HalvingMetallurgy.Atoms.Nickel;
		Atoms.PTableIgnore[27] = true;
		Atoms.PTableAtoms[29] = HalvingMetallurgy.Atoms.Zinc;
		Atoms.PTableIgnore[29] = true;
		Atoms.PTableAtoms[73] = HalvingMetallurgy.Atoms.Wolfram;
		Atoms.PTableIgnore[73] = true;
		Atoms.PTableAtoms[75] = HalvingMetallurgy.Atoms.Osmium;
		Atoms.PTableIgnore[75] = true;
		Flexibility.addMetallificationRule(HalvingMetallurgy.Atoms.Quickcopper,HalvingMetallurgy.Atoms.Beryl);
		Flexibility.addDemetallificationRule(HalvingMetallurgy.Atoms.Beryl,HalvingMetallurgy.Atoms.Quickcopper);
		Flexibility.DemetallificationExplosionMeta.Add(HalvingMetallurgy.Atoms.Nickel);
		Flexibility.DemetallificationExplosionMeta.Add(HalvingMetallurgy.Atoms.Zinc);
		Flexibility.DemetallificationExplosionMeta.Add(HalvingMetallurgy.Atoms.Wolfram);
		Flexibility.DemetallificationExplosionMeta.Add(HalvingMetallurgy.Atoms.Osmium);
		Flexibility.DemetallificationExplosionMeta.Add(HalvingMetallurgy.Atoms.Sednum);
		Flexibility.DemetallificationExplosionMeta.Add(HalvingMetallurgy.Atoms.Vulcan);
	}
	public static void AddHalvingMetallurgyLater() {
		Flexibility.addTriplexCondition(Atoms.EZGG,HalvingMetallurgy.Atoms.Vulcan);
	}
	public static void AddUnstableElements() {
		Atoms.PTableAtoms[91] = MagicalParcherPlus.FindModAtom("UnstableElements:uranium"); //why it is private
		Atoms.PTableIgnore[91] = true;
		Flexibility.DemetallificationExplosionMeta.Add(Atoms.PTableAtoms[91]);
		//Flexibility.addExtraAtomicException(FindModAtom("UnstableElements:uranium_0_1"),91); //it is inaccessible to this atom 
		//Flexibility.addExtraAtomicException(FindModAtom("UnstableElements:uranium_0_2"),91); //and it has strict type checking
		//Flexibility.addExtraAtomicException(FindModAtom("UnstableElements:uranium_1_0"),91); //so it won't detect isotopes
		//Flexibility.addExtraAtomicException(FindModAtom("UnstableElements:uranium_1_1"),91);
		//Flexibility.addExtraAtomicException(FindModAtom("UnstableElements:uranium_1_2"),91);
		//Flexibility.addExtraAtomicException(FindModAtom("UnstableElements:uranium_2_0"),91);
		//Flexibility.addExtraAtomicException(FindModAtom("UnstableElements:uranium_2_1"),91);
		//Flexibility.addExtraAtomicException(FindModAtom("UnstableElements:uranium_2_2"),91);
	}
	public static void AddMetalQuintessence() {
		Atoms.PTableAtoms[23] = MetalQuintessence.MetalQuintessenceAtoms.Chromium;
		Atoms.PTableIgnore[23] = true;
		Flexibility.DemetallificationExplosionMeta.Add(MetalQuintessence.MetalQuintessenceAtoms.Chromium);
	}
	public static void AddAlchemicalInversions() {
		Atoms.PTableAtoms[38] = AlchemicalInversions.Atoms.Yttrium;
		Atoms.PTableIgnore[38] = true;
	}
	public static void AddNeuvolics() {
		Atoms.PTableAtoms[76] = Neuvolics.Atoms.Iridium;
		Atoms.PTableIgnore[76] = true;
	}
	public static void AddVacancy_ExtHalvingMetallurgy() {
		Flexibility.addMetallificationRule(HalvingMetallurgy.Atoms.Quicklime,Vaca.MainClass.VacaAtom);
		Flexibility.addDemetallificationRule(Vaca.MainClass.VacaAtom,HalvingMetallurgy.Atoms.Quicklime);
	}
}
public class SwitcherooRecipe : Recipe
{
	protected MultipleMatcher Catalyst;
	protected Dictionary<AtomType,int> Input;
	public MultipleMatcher GetCatalyst() {
		return Catalyst;
	}
	public Dictionary<AtomType,int> GetInput() {
		return Input;
	}
	public override bool CheckRecipe(AtomType[] Ingredients){ //partital match unsupported
		if (Catalyst != Ingredients[0]) return false;
		Ingredients = Ingredients.Skip(1).ToArray();
		Dictionary<AtomType,int> Dim = new(Input);
		foreach (AtomType atoms in Ingredients){
			if (atoms is null) {
				continue;
			} else if (Dim.ContainsKey(atoms)) {
				Dim[atoms]--;
				if (Dim[atoms] <= 0) {
					Dim.Remove(atoms);
				}
			} else {
				return false;
			}
		}
		return Dim.Count <= 0;
	}
	public SwitcherooRecipe(MultipleMatcher Cata,Dictionary<AtomType,int> Targ,AtomType[] Dispel){
		Catalyst = Cata;
		Input = Targ;
		Output = Dispel;
	}
}