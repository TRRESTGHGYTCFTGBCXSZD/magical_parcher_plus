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
    }
	public static void AddHalvingMetallurgyLater() {
		Flexibility.addTriplexCondition(Atoms.EZGG,HalvingMetallurgy.Atoms.Vulcan);
    }
	public static void AddUnstableElements() {
		Atoms.PTableAtoms[91] = MagicalParcherPlus.FindModAtom("UnstableElements:uranium"); //why it is private
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
	public static void AddMetalQuintessence() {
		Atoms.PTableAtoms[23] = MetalQuintessence.MetalQuintessenceAtoms.Chromium;
		Atoms.PTableIgnore[23] = true;
    }
	public static void AddAlchemicalInversions() {
		Atoms.PTableAtoms[38] = AlchemicalInversions.Atoms.Yttrium;
		Atoms.PTableIgnore[38] = true;
    }
	public static void AddNeuvolics() {
		Atoms.PTableAtoms[76] = Neuvolics.Atoms.Iridium;
		Atoms.PTableIgnore[76] = true;
    }
}