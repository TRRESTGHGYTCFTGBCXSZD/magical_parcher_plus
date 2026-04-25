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

public class Flexibility
{
	private static Dictionary<AtomType, AtomType> CardinalificationMeta = new();
	private static Dictionary<AtomType, AtomType> LiquidationMeta = new();
	private static Dictionary<AtomType, AtomType> MetallificationMeta = new();
	private static Dictionary<AtomType, AtomType> DemetallificationMeta = new();
	public static List<AtomType> DemetallificationExplosionMeta = new();
	private static Dictionary<AtomType, int> ExtraAtomicExceptions = new();
	private static Dictionary<AtomType, int> PlaceholderPTableReplacements = new();
	private static Dictionary<AtomType,List<AtomType>> CanThisAtomTriplex = new();
    //reductive metal code
	public static bool applyCardinalificationRule(AtomType input, out AtomType output) => applyTRule(input, CardinalificationMeta, out output);
	public static bool applyLiquidationRule(AtomType input, out AtomType output) => applyTRule(input, LiquidationMeta, out output);
	public static bool applyMetallificationRule(AtomType input, out AtomType output) => applyTRule(input, MetallificationMeta, out output);
	public static bool applyDemetallificationRule(AtomType input, out AtomType output) => applyTRule(input, DemetallificationMeta, out output);
	public static bool applyExtraAtomicException(AtomType input, out int output) => applyTRule(input, ExtraAtomicExceptions, out output);
	public static bool applyPlaceholderPTableReplacement(AtomType input, out int output) => applyTRule(input, PlaceholderPTableReplacements, out output);

	public static void addCardinalificationRule(AtomType hi, AtomType lo) => addTRule("cardinalificationMeta", hi, lo, CardinalificationMeta, new List<AtomType> {});
	public static void addLiquidationRule(AtomType hi, AtomType lo) => addTRule("liquidationMeta", hi, lo, LiquidationMeta, new List<AtomType> {});
	public static void addMetallificationRule(AtomType hi, AtomType lo) => addTRule("metallification", hi, lo, MetallificationMeta, new List<AtomType> {});
	public static void addDemetallificationRule(AtomType hi, AtomType lo) => addTRule("demetallification", hi, lo, DemetallificationMeta, new List<AtomType> {});
	public static void addExtraAtomicException(AtomType atomictype, int atomicnumber) => addTRule("extraatomicexceptions", atomictype, atomicnumber, ExtraAtomicExceptions, new List<AtomType> {});
	public static void addPlaceholderPTableReplacement(AtomType atomictype, int atomicnumber) => addTRule("placeholderptablereplacements", atomictype, atomicnumber, PlaceholderPTableReplacements, new List<AtomType> {});

	public static bool checkTriplexCondition(AtomType triplexcatalyst, AtomType triplexattach){
		if (!CanThisAtomTriplex.ContainsKey(triplexcatalyst)) //safeguard to prevent uninitialized data
		{
			return false;
		}
		foreach (AtomType atomie in CanThisAtomTriplex[triplexcatalyst])
		{
			if (atomie == triplexattach) return true; // this is found
		}
		return false;
	}
	public static void addTriplexCondition(AtomType triplexcatalyst, AtomType triplexattach){
		if (!CanThisAtomTriplex.ContainsKey(triplexcatalyst)) //safeguard to prevent uninitialized data
		{
			CanThisAtomTriplex.Add(triplexcatalyst,new List<AtomType>());
		}
		CanThisAtomTriplex[triplexcatalyst].Add(triplexattach);
	}

	private static bool applyTRule<T>(AtomType hi, Dictionary<AtomType, T> dict, out T lo)
	{
		bool ret = dict.ContainsKey(hi);
		lo = ret ? dict[hi] : default(T);
		return ret;
	}
	private static string ToString(AtomType A) => A.field_2284;

	private static string ruleToString<T>(AtomType hi, T lo)
	{
		if (typeof(T) == typeof(AtomType))
		{
			return ToString(hi) + " => " + ToString((AtomType)(object)lo);
		}
		else if (typeof(T) == typeof(Pair<AtomType, AtomType>))
		{
			return ToString(hi) + " => ( " + ToString(((Pair<AtomType, AtomType>)(object)lo).Left) + ", " + ToString(((Pair<AtomType, AtomType>)(object)lo).Right) + " )";
		}
		return "";
	}
	private static bool TEquality<T>(T A, T B)
	{
		if (typeof(T) == typeof(AtomType))
		{
			return (AtomType)(object)A == (AtomType)(object)B;
		}
		else if (typeof(T) == typeof(Pair<AtomType, AtomType>))
		{
			return (Pair<AtomType, AtomType>)(object)A == (Pair<AtomType, AtomType>)(object)B;
		}
		return false;
	}
	private static void addTRule<T>(string Tname, AtomType input, T output, Dictionary<AtomType, T> dict, List<AtomType> forbiddenInputs)
	{
		string TNAME = Tname.First().ToString().ToUpper() + Tname.Substring(1);
		//check if rule is forbidden
		if (forbiddenInputs.Contains(input))
		{
			Logger.Log("[MP+] ERROR: A " + Tname + " rule for " + ToString(input) + " is not permitted.");
			throw new Exception("add" + TNAME + "Rule: Cannot add rule '" + ruleToString(input, output) + "'.");
		}
		//try to add rule
		bool flag = dict.ContainsKey(input);
		if (flag && !TEquality(dict[input], output))
		{
			//throw an error
			string msg = "[MP+] ERROR: Preparing debug dump.";
			msg += "\n  Current list of " + TNAME + " Rules:";
			foreach (var kvp in dict) msg += "\n\t" + ruleToString(kvp.Key, kvp.Value);
			msg += "\n\n  AtomType '" + ToString(input) + "' already has a " + Tname + " rule: '" + ruleToString(input, dict[input]) + "'.";
			Logger.Log(msg);
			throw new Exception("add" + TNAME + "Rule: Cannot add rule '" + ruleToString(input, output) + "'.");
		}
		else if (!flag)
		{
			dict.Add(input, output);
		}
	}
}