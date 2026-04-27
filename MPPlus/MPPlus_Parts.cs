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

public static class Parts
{
	public static MethodInfo PrivateMethod<T>(string method) => typeof(T).GetMethod(method, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
	public static bool FindBerloAtom(Sim simulation, HexIndex HexPos, out AtomType wee)
	{
		var SEB = simulation.field_3818;
		var solution = SEB.method_502();
		var partList = solution.field_3919;
		var partSimStates = simulation.field_3821;

		foreach (var wheel in partList)
		{
			if (wheel.method_1159().field_1544 is not null)
			{
				var partSimState = partSimStates[wheel];
				if (wheel.method_1159().field_1544.ContainsKey((HexPos-partSimState.field_2724).Rotated(partSimState.field_2726.Negative())))
				{
					wee = wheel.method_1159().field_1544[(HexPos-partSimState.field_2724).Rotated(partSimState.field_2726.Negative())];
					return true;
				}
			}
		}
		wee = AtomTypes.field_1675; //salt as fallback
		return false;
	}

	public static bool IsHalvingLoaded = false;
	//season 1 glyphs
	public static PartType Cardinalification, Liquidation, Gerioification, Metallification, Demetallification;
	//season 2 glyphs
	public static PartType AnimisStabilizer, AtomicProjection, AtomicRejection, MagneticDepolarizer, MagneticPolarizer, WordexisWheel;
    static Molecule WordexisWheelWhatTheFuck()
    {
        Molecule molecule = new();
        molecule.method_1105(new Atom(Atoms.Gabs), new HexIndex(0, 1));
        molecule.method_1105(new Atom(Atoms.Erabukun), new HexIndex(1, 0));
        molecule.method_1105(new Atom(Atoms.Tric), new HexIndex(1, -1));
        molecule.method_1105(new Atom(Atoms.EZGG), new HexIndex(0, -1));
        molecule.method_1105(new Atom(Atoms.Wordexis), new HexIndex(-1, 0));
        molecule.method_1105(new Atom(Atoms.Modrenity), new HexIndex(-1, 1));
        return molecule;
    }

	public static readonly Texture ProjectionBase = class_238.field_1989.field_90.field_255.field_288;
	public static readonly Texture ProjectionGlow = class_235.method_615("textures/select/double_glow");
	public static readonly Texture ProjectionBowl = class_238.field_1989.field_90.field_255.field_292;
	public static readonly Texture ProjectionHole = class_238.field_1989.field_90.field_255.field_293;
	
	public static readonly Texture Wordexis_Input = class_235.method_615("textures/parts/magicalparcher/inputs/wordexis_input");
	public static readonly Texture Tric_Input = class_235.method_615("textures/parts/magicalparcher/inputs/tric_input");
	public static readonly Texture QuicksilverSymbol = class_238.field_1989.field_90.field_255.field_294;
	public static readonly Texture SaltSymbol = class_238.field_1989.field_90.field_228.field_275;
	public static readonly Texture QuintessenceSymbol = class_238.field_1989.field_90.field_238.field_341;
	public static readonly Texture LeadSymbol = class_238.field_1989.field_90.field_255.field_291;

	public static readonly Texture DiamondBase = class_238.field_1989.field_90.field_228.field_265;

	public static readonly Texture CalcinatorBase = class_238.field_1989.field_90.field_169;
	public static readonly Texture RejectionBowl = class_238.field_1989.field_90.field_170;

	private static bool ContentLoaded = false;
	public static void AddNewContent() {
		if (ContentLoaded) return;
		ContentLoaded = true;

		Cardinalification = new(){
			field_1528 = "magical-parcher-plus-cardinalification", // ID
			field_1529 = class_134.method_253("Haxior Cardinalification", string.Empty), // Name
			field_1530 = class_134.method_253("Transforms Quicksilver into Salt Using Wordexis.", string.Empty), // Description
			field_1531 = 40, // Cost
			field_1539 = true, // Is a glyph (?)
			field_1549 = class_238.field_1989.field_97.field_374, // Shadow/glow
			field_1550 = class_238.field_1989.field_97.field_375, // Stroke/outline
			field_1547 = SaltSymbol, // Panel icon
			field_1548 = SaltSymbol, // Hovered panel icon
			field_1540 = new HexIndex[]{
				new(0, 0),
				new(1, 0)
			}, // Spaces used
			field_1551 = Permissions.None,
			CustomPermissionCheck = perms => perms.Contains("magicalparcherplus:cardinalification")
		};

		QApi.AddPartType(Cardinalification, (part, pos, editor, renderer) => {
			Vector2 vector2 = new(41f, 48f);
			renderer.method_523(ProjectionBase, new Vector2(0.0f, 0.0f), vector2, 0.0f);
			foreach(HexIndex idx in part.method_1159().field_1540){
				if(idx is { Q: 0, R: 0 }){
					renderer.method_530(class_238.field_1989.field_90.field_164 /*bonder_shadow*/, idx, 0);
					renderer.method_528(ProjectionBowl, idx, Vector2.Zero);
					renderer.method_529(QuicksilverSymbol /*quicksilver_symbol*/, idx, Vector2.Zero);
				}
				else{
					renderer.method_530(class_238.field_1989.field_90.field_164 /*bonder_shadow*/, idx, 0);
					renderer.method_530(class_238.field_1989.field_90.field_255.field_293 /*quicksilver_input*/, idx, 0);
					// should be 272?
					renderer.method_529(Wordexis_Input /*quicksilver_symbol*/, idx, Vector2.Zero);
				}
			}

			for(var i = 0; i < part.method_1159().field_1540.Length; i++){
				HexIndex hexIndex = part.method_1159().field_1540[i];
				if(hexIndex != new HexIndex(0, 0)){
					int index = i - 1;
					float num = new HexRotation(index * 2).ToRadians();
					renderer.method_522(class_238.field_1989.field_90.field_255.field_289 /*bond*/, new Vector2(-30f, 12f), num);
				}
			}
		});
		QApi.AddPartTypeToPanel(Cardinalification, PartTypes.field_1775);
		
		QApi.AddPuzzlePermission("magicalparcherplus:cardinalification", "Haxior Cardinalification", "Magical Parcher+");

		Liquidation = new(){
			field_1528 = "magical-parcher-plus-liquidation", // ID
			field_1529 = class_134.method_253("Haxior Liquidation", string.Empty), // Name
			field_1530 = class_134.method_253("Transforms Salt into Quicksilver Using Wordexis.", string.Empty), // Description
			field_1531 = 40, // Cost
			field_1539 = true, // Is a glyph (?)
			field_1549 = class_238.field_1989.field_97.field_374, // Shadow/glow
			field_1550 = class_238.field_1989.field_97.field_375, // Stroke/outline
			field_1547 = QuicksilverSymbol, // Panel icon
			field_1548 = QuicksilverSymbol, // Hovered panel icon
			field_1540 = new HexIndex[]{
				new(0, 0),
				new(1, 0)
			}, // Spaces used
			field_1551 = Permissions.None,
			CustomPermissionCheck = perms => perms.Contains("magicalparcherplus:liquidation")
		};

		QApi.AddPartType(Liquidation, (part, pos, editor, renderer) => {
			Vector2 vector2 = new(41f, 48f);
			renderer.method_523(ProjectionBase, new Vector2(0.0f, 0.0f), vector2, 0.0f);
			foreach(HexIndex idx in part.method_1159().field_1540){
				if(idx is { Q: 0, R: 0 }){
					renderer.method_530(class_238.field_1989.field_90.field_164 /*bonder_shadow*/, idx, 0);
					renderer.method_528(ProjectionBowl, idx, Vector2.Zero);
					renderer.method_529(SaltSymbol /*quicksilver_symbol*/, idx, Vector2.Zero);
				}
				else{
					renderer.method_530(class_238.field_1989.field_90.field_164 /*bonder_shadow*/, idx, 0);
					renderer.method_530(class_238.field_1989.field_90.field_255.field_293 /*quicksilver_input*/, idx, 0);
					// should be 272?
					renderer.method_529(Wordexis_Input /*quicksilver_symbol*/, idx, Vector2.Zero);
				}
			}

			for(var i = 0; i < part.method_1159().field_1540.Length; i++){
				HexIndex hexIndex = part.method_1159().field_1540[i];
				if(hexIndex != new HexIndex(0, 0)){
					int index = i - 1;
					float num = new HexRotation(index * 2).ToRadians();
					renderer.method_522(class_238.field_1989.field_90.field_255.field_289 /*bond*/, new Vector2(-30f, 12f), num);
				}
			}
		});
		QApi.AddPartTypeToPanel(Liquidation, PartTypes.field_1775);
		
		QApi.AddPuzzlePermission("magicalparcherplus:liquidation", "Haxior Liquidation", "Magical Parcher+");

		Gerioification = new(){
			field_1528 = "magical-parcher-plus-gerioification", // ID
			field_1529 = class_134.method_253("Haxior Gerioification", string.Empty), // Name
			field_1530 = class_134.method_253("Transforms Salt or Quicksilver Into Wordexis Using Quintessence.", string.Empty), // Description
			field_1531 = 40, // Cost
			field_1539 = true, // Is a glyph (?)
			field_1549 = class_238.field_1989.field_97.field_374, // Shadow/glow
			field_1550 = class_238.field_1989.field_97.field_375, // Stroke/outline
			field_1547 = Wordexis_Input, // Panel icon
			field_1548 = Wordexis_Input, // Hovered panel icon
			field_1540 = new HexIndex[]{
				new(0, 0),
				new(1, 0)
			}, // Spaces used
			field_1551 = Permissions.None,
			CustomPermissionCheck = perms => perms.Contains("magicalparcherplus:gerioification")
		};

		QApi.AddPartType(Gerioification, (part, pos, editor, renderer) => {
			Vector2 vector2 = new(41f, 48f);
			renderer.method_523(ProjectionBase, new Vector2(0.0f, 0.0f), vector2, 0.0f);
			foreach(HexIndex idx in part.method_1159().field_1540){
				if(idx is { Q: 0, R: 0 }){
					renderer.method_530(class_238.field_1989.field_90.field_164 /*bonder_shadow*/, idx, 0);
					renderer.method_528(ProjectionBowl, idx, Vector2.Zero);
					renderer.method_529(SaltSymbol /*quicksilver_symbol*/, idx, Vector2.Zero);
					renderer.method_529(QuicksilverSymbol /*quicksilver_symbol*/, idx, Vector2.Zero);
				}
				else{
					renderer.method_530(class_238.field_1989.field_90.field_164 /*bonder_shadow*/, idx, 0);
					renderer.method_530(class_238.field_1989.field_90.field_255.field_293 /*quicksilver_input*/, idx, 0);
					// should be 272?
					renderer.method_529(QuintessenceSymbol /*quicksilver_symbol*/, idx, Vector2.Zero);
				}
			}

			for(var i = 0; i < part.method_1159().field_1540.Length; i++){
				HexIndex hexIndex = part.method_1159().field_1540[i];
				if(hexIndex != new HexIndex(0, 0)){
					int index = i - 1;
					float num = new HexRotation(index * 2).ToRadians();
					renderer.method_522(class_238.field_1989.field_90.field_255.field_289 /*bond*/, new Vector2(-30f, 12f), num);
				}
			}
		});
		QApi.AddPartTypeToPanel(Gerioification, PartTypes.field_1775);
		
		QApi.AddPuzzlePermission("magicalparcherplus:gerioification", "Haxior Gerioification", "Magical Parcher+");

		Metallification = new(){
			field_1528 = "magical-parcher-plus-metallification", // ID
			field_1529 = class_134.method_253("Glyph of Metallification", string.Empty), // Name
			field_1530 = class_134.method_253("Transforms Quicksilver to Lead.", string.Empty), // Description
			field_1531 = 30, // Cost
			field_1539 = true, // Is a glyph (?)
			field_1549 = class_238.field_1989.field_97.field_382, // Shadow/glow
			field_1550 = class_238.field_1989.field_97.field_383, // Stroke/outline
			field_1547 = Wordexis_Input, // Panel icon
			field_1548 = Wordexis_Input, // Hovered panel icon
			field_1540 = new HexIndex[]{
				new(0, 0)
			}, // Spaces used
			field_1551 = Permissions.None,
			CustomPermissionCheck = perms => perms.Contains("magicalparcherplus:metallification")
		};

		QApi.AddPartType(Metallification, (part, pos, editor, renderer) => {
			Vector2 vector2 = new(41f, 48f);
			renderer.method_523(CalcinatorBase, new Vector2(0.0f, 0.0f), vector2, 0.0f);
			renderer.method_528(ProjectionBowl, new HexIndex(0, 0), Vector2.Zero);
			renderer.method_529(QuicksilverSymbol /*quicksilver_symbol*/, new HexIndex(0, 0), Vector2.Zero);
		});
		QApi.AddPartTypeToPanel(Metallification, PartTypes.field_1775);
		
		QApi.AddPuzzlePermission("magicalparcherplus:metallification", "Glyph of Metallification", "Magical Parcher+");

		Demetallification = new(){
			field_1528 = "magical-parcher-plus-demetallification", // ID
			field_1529 = class_134.method_253("Glyph of Demetallification", string.Empty), // Name
			field_1530 = class_134.method_253("Transforms Lead to Quicksilver.", string.Empty), // Description
			field_1531 = 30, // Cost
			field_1539 = true, // Is a glyph (?)
			field_1549 = class_238.field_1989.field_97.field_382, // Shadow/glow
			field_1550 = class_238.field_1989.field_97.field_383, // Stroke/outline
			field_1547 = Wordexis_Input, // Panel icon
			field_1548 = Wordexis_Input, // Hovered panel icon
			field_1540 = new HexIndex[]{
				new(0, 0)
			}, // Spaces used
			field_1551 = Permissions.None,
			CustomPermissionCheck = perms => perms.Contains("magicalparcherplus:demetallification")
		};

		QApi.AddPartType(Demetallification, (part, pos, editor, renderer) => {
			Vector2 vector2 = new(41f, 48f);
			renderer.method_523(CalcinatorBase, new Vector2(0.0f, 0.0f), vector2, 0.0f);
			renderer.method_528(ProjectionBowl, new HexIndex(0, 0), Vector2.Zero);
			renderer.method_529(LeadSymbol /*quicksilver_symbol*/, new HexIndex(0, 0), Vector2.Zero);
		});
		QApi.AddPartTypeToPanel(Demetallification, PartTypes.field_1775);
		
		QApi.AddPuzzlePermission("magicalparcherplus:demetallification", "Glyph of Demetallification", "Magical Parcher+");

		QApi.AddPuzzlePermission("magicalparcherplus:unsafedemetallification", "Unsafe Demetallification", "Magical Parcher+");



		AnimisStabilizer = new(){
			field_1528 = "magical-parcher-plus-animisstabilizer", // ID
			field_1529 = class_134.method_253("Glyph of Anti-Animismus", string.Empty), // Name
			field_1530 = class_134.method_253("Takes Vitae and Mors and expells 2 Salt Atoms.", string.Empty), // Description
			field_1531 = 40, // Cost
			field_1539 = true, // Is a glyph (?)
			field_1549 = class_238.field_1989.field_97.field_368, // Shadow/glow
			field_1550 = class_238.field_1989.field_97.field_369, // Stroke/outline
			field_1547 = SaltSymbol, // Panel icon
			field_1548 = SaltSymbol, // Hovered panel icon
			field_1540 = new HexIndex[]{
				new(0, 0),
				new(1, 0),
				new(0, 1),
				new(1, -1)
			}, // Spaces used
			field_1551 = Permissions.None,
			CustomPermissionCheck = perms => perms.Contains("magicalparcherplus:animisstabilizer")
		};

		QApi.AddPartType(AnimisStabilizer, (part, pos, editor, renderer) => {
			Vector2 vector2 = new(42f, 120f);
			renderer.method_523(DiamondBase, new Vector2(0.0f, 0.0f), vector2, 0.0f);
			renderer.method_528(class_238.field_1989.field_90.field_255.field_293, new HexIndex(0, 1), Vector2.Zero);
			renderer.method_528(class_238.field_1989.field_90.field_255.field_293, new HexIndex(1, -1), Vector2.Zero);
			renderer.method_528(class_238.field_1989.field_90.field_255.field_293, new HexIndex(0, 0), Vector2.Zero);
			renderer.method_528(class_238.field_1989.field_90.field_255.field_293, new HexIndex(1, 0), Vector2.Zero);
			
			PartSimState pss = editor.method_507().method_481(part);
			class_236 uco = editor.method_1989(part, pos);
			float time = editor.method_504();
			Vector2 risingOffset = uco.field_1984 + class_187.field_1742.method_492(new HexIndex(0, 0)).Rotated(uco.field_1985);
			Vector2 risingOffset2 = uco.field_1984 + class_187.field_1742.method_492(new HexIndex(1, 0)).Rotated(uco.field_1985);

			if (pss.field_2743)
			{
				Molecule VitaeMolecule = Molecule.method_1121(pss.field_2744[0]);
				Molecule MorsMolecule = Molecule.method_1121(pss.field_2744[0]);
				Editor.method_925(VitaeMolecule, risingOffset, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
				Editor.method_925(MorsMolecule, risingOffset2, new HexIndex(0, 0), 0f, 1f, time, 1f, false, null);
			}
		});
		QApi.AddPartTypeToPanel(AnimisStabilizer, PartTypes.field_1775);
		
		QApi.AddPuzzlePermission("magicalparcherplus:animisstabilizer", "Glyph of Anti-Animismus", "Magical Parcher+");

		AtomicProjection = new(){
			field_1528 = "magical-parcher-plus-atomicprojection", // ID
			field_1529 = class_134.method_253("Haxior Projection", string.Empty), // Name
			field_1530 = class_134.method_253("Promotes the atom into next atomic number using Proton.", string.Empty), // Description
			field_1531 = 40, // Cost
			field_1539 = true, // Is a glyph (?)
			field_1549 = class_238.field_1989.field_97.field_374, // Shadow/glow
			field_1550 = class_238.field_1989.field_97.field_375, // Stroke/outline
			field_1547 = SaltSymbol, // Panel icon
			field_1548 = SaltSymbol, // Hovered panel icon
			field_1540 = new HexIndex[]{
				new(0, 0),
				new(1, 0)
			}, // Spaces used
			field_1551 = Permissions.None,
			CustomPermissionCheck = perms => perms.Contains("magicalparcherplus:atomicprojection")
		};

		QApi.AddPartType(AtomicProjection, (part, pos, editor, renderer) => {
			Vector2 vector2 = new(41f, 48f);
			renderer.method_523(ProjectionBase, new Vector2(0.0f, 0.0f), vector2, 0.0f);
			foreach(HexIndex idx in part.method_1159().field_1540){
				if(idx is { Q: 0, R: 0 }){
					renderer.method_530(class_238.field_1989.field_90.field_164 /*bonder_shadow*/, idx, 0);
					renderer.method_528(ProjectionBowl, idx, Vector2.Zero);
				}
				else{
					renderer.method_530(class_238.field_1989.field_90.field_164 /*bonder_shadow*/, idx, 0);
					renderer.method_530(class_238.field_1989.field_90.field_255.field_293 /*quicksilver_input*/, idx, 0);
					// should be 272?
					renderer.method_529(Tric_Input /*quicksilver_symbol*/, idx, Vector2.Zero);
				}
			}

			for(var i = 0; i < part.method_1159().field_1540.Length; i++){
				HexIndex hexIndex = part.method_1159().field_1540[i];
				if(hexIndex != new HexIndex(0, 0)){
					int index = i - 1;
					float num = new HexRotation(index * 2).ToRadians();
					renderer.method_522(class_238.field_1989.field_90.field_255.field_289 /*bond*/, new Vector2(-30f, 12f), num);
				}
			}
		});
		QApi.AddPartTypeToPanel(AtomicProjection, PartTypes.field_1775);
		
		QApi.AddPuzzlePermission("magicalparcherplus:atomicprojection", "Haxior Projection", "Magical Parcher+");

		AtomicRejection = new(){
			field_1528 = "magical-parcher-plus-atomicrejection", // ID
			field_1529 = class_134.method_253("Haxior Rejection", string.Empty), // Name
			field_1530 = class_134.method_253("Demotes the atom into previous atomic number, Expelling Proton.", string.Empty), // Description
			field_1531 = 40, // Cost
			field_1539 = true, // Is a glyph (?)
			field_1549 = class_238.field_1989.field_97.field_374, // Shadow/glow
			field_1550 = class_238.field_1989.field_97.field_375, // Stroke/outline
			field_1547 = SaltSymbol, // Panel icon
			field_1548 = SaltSymbol, // Hovered panel icon
			field_1540 = new HexIndex[]{
				new(0, 0),
				new(1, 0)
			}, // Spaces used
			field_1551 = Permissions.None,
			CustomPermissionCheck = perms => perms.Contains("magicalparcherplus:atomicrejection")
		};

		QApi.AddPartType(AtomicRejection, (part, pos, editor, renderer) => {
			Vector2 vector2 = new(41f, 48f);
			renderer.method_523(ProjectionBase, new Vector2(0.0f, 0.0f), vector2, 0.0f);
			foreach(HexIndex idx in part.method_1159().field_1540){
				if(idx is { Q: 0, R: 0 }){
					renderer.method_530(class_238.field_1989.field_90.field_164 /*bonder_shadow*/, idx, 0);
					renderer.method_528(ProjectionBowl, idx, Vector2.Zero);
				}
				else{
					renderer.method_530(class_238.field_1989.field_90.field_164 /*bonder_shadow*/, idx, 0);
					renderer.method_528(RejectionBowl /*quicksilver_input*/, idx, Vector2.Zero);
					// should be 272?
					//renderer.method_529(Wordexis_Input /*quicksilver_symbol*/, idx, Vector2.Zero);
				}
			}

			for(var i = 0; i < part.method_1159().field_1540.Length; i++){
				HexIndex hexIndex = part.method_1159().field_1540[i];
				if(hexIndex != new HexIndex(0, 0)){
					int index = i - 1;
					float num = new HexRotation(index * 2).ToRadians();
					renderer.method_522(class_238.field_1989.field_90.field_255.field_289 /*bond*/, new Vector2(-30f, 12f), num);
				}
			}
		});
		QApi.AddPartTypeToPanel(AtomicRejection, PartTypes.field_1775);
		
		QApi.AddPuzzlePermission("magicalparcherplus:atomicrejection", "Haxior Rejection", "Magical Parcher+");

		MagneticPolarizer = new(){
			field_1528 = "magical-parcher-plus-magneticpolarizer", // ID
			field_1529 = class_134.method_253("Glyph Of Polarizer", string.Empty), // Name
			field_1530 = class_134.method_253("Polarizes 2 Zenas into Red and Blue Zena.", string.Empty), // Description
			field_1531 = 40, // Cost
			field_1539 = true, // Is a glyph (?)
			field_1549 = class_238.field_1989.field_97.field_374, // Shadow/glow
			field_1550 = class_238.field_1989.field_97.field_375, // Stroke/outline
			field_1547 = SaltSymbol, // Panel icon
			field_1548 = SaltSymbol, // Hovered panel icon
			field_1540 = new HexIndex[]{
				new(0, 0),
				new(1, 0)
			}, // Spaces used
			field_1551 = Permissions.None,
			CustomPermissionCheck = perms => perms.Contains("magicalparcherplus:magneticpolarizer")
		};

		QApi.AddPartType(MagneticPolarizer, (part, pos, editor, renderer) => {
			Vector2 vector2 = new(41f, 48f);
			renderer.method_523(ProjectionBase, new Vector2(0.0f, 0.0f), vector2, 0.0f);
			renderer.method_528(ProjectionBowl, new HexIndex(0, 0), Vector2.Zero);
			renderer.method_528(ProjectionBowl, new HexIndex(1, 0), Vector2.Zero);
		});
		QApi.AddPartTypeToPanel(MagneticPolarizer, PartTypes.field_1775);
		

		MagneticDepolarizer = new(){
			field_1528 = "magical-parcher-plus-magneticdepolarizer", // ID
			field_1529 = class_134.method_253("Glyph Of Depolarizer", string.Empty), // Name
			field_1530 = class_134.method_253("Depolarized Red And Blue Zena into 2 Zenas.", string.Empty), // Description
			field_1531 = 40, // Cost
			field_1539 = true, // Is a glyph (?)
			field_1549 = class_238.field_1989.field_97.field_374, // Shadow/glow
			field_1550 = class_238.field_1989.field_97.field_375, // Stroke/outline
			field_1547 = SaltSymbol, // Panel icon
			field_1548 = SaltSymbol, // Hovered panel icon
			field_1540 = new HexIndex[]{
				new(0, 0),
				new(1, 0)
			}, // Spaces used
			field_1551 = Permissions.None,
			CustomPermissionCheck = perms => perms.Contains("magicalparcherplus:magneticpolarizer")
		};

		QApi.AddPartType(MagneticDepolarizer, (part, pos, editor, renderer) => {
			Vector2 vector2 = new(41f, 48f);
			renderer.method_523(ProjectionBase, new Vector2(0.0f, 0.0f), vector2, 0.0f);
			renderer.method_528(RejectionBowl, new HexIndex(0, 0), Vector2.Zero);
			renderer.method_528(RejectionBowl, new HexIndex(1, 0), Vector2.Zero);
		});
		QApi.AddPartTypeToPanel(MagneticDepolarizer, PartTypes.field_1775);
		
		QApi.AddPuzzlePermission("magicalparcherplus:magneticpolarizer", "Glyphs Of Magnets", "Magical Parcher+");
		WordexisWheel = new(){
			field_1528 = "magical-parcher-plus-wordexiswheel",
			field_1529 = class_134.method_253("Wordexis' Wheel", string.Empty),
			field_1530 = class_134.method_253("GERIO. DO YOU WANNA TRANSFORM?", string.Empty),
			field_1547 = class_238.field_1989.field_90.field_245.field_335,
			field_1548 = class_238.field_1989.field_90.field_245.field_336,
			field_1531 = 30,
			field_1552 = true,
			field_1532 = (enum_2)1,
			field_1533 = true,
			field_1544 = new Dictionary<HexIndex, AtomType>
			{
				{
					new HexIndex(-1, 0),
					Atoms.Wordexis
				},
				{
					new HexIndex(1, 0),
					Atoms.Erabukun
				},
				{
					new HexIndex(-1, 1),
					Atoms.Modrenity
				},
				{
					new HexIndex(0, 1),
					Atoms.Gabs
				},
				{
					new HexIndex(0, -1),
					Atoms.EZGG
				},
				{
					new HexIndex(1, -1),
					Atoms.Tric
				}
			},
			field_1536 = true,
			field_1551 = Permissions.None,
			CustomPermissionCheck = perms => perms.Contains("magicalparcherplus:wordexiswheel")
		};

		QApi.AddPartType(WordexisWheel, (part, pos, editor, renderer) => {
		
			class_236 uco = editor.method_1989(part, pos);
			float time = (float)(Math.Sin(new struct_27(Time.Now().Ticks).method_603()*4f)/2f)+0.5f;
			Vector2 risingOffset = uco.field_1984 + class_187.field_1742.method_492(new HexIndex(0, 0)).Rotated(uco.field_1985);

			Editor.method_925(WordexisWheelWhatTheFuck(), risingOffset, new HexIndex(0, 0), uco.field_1985, 1f, time, 1f, false, null);
		});
		QApi.AddPartTypeToPanel(WordexisWheel, PartTypes.field_1775);
		
		QApi.AddPuzzlePermission("magicalparcherplus:wordexiswheel", "Wordexis' Wheel", "Magical Parcher+");
		// the that code runs below
		QApi.RunAfterCycle((sim, first) => {
			var seb = sim.field_3818;
			List<Part> allParts = seb.method_502().field_3919;
			Dictionary<Part, PartSimState> partSimStates = sim.field_3821;
			var simStates = sim.field_3821;
			var moleculeList = sim.field_3823;
			void YOUARENOTAPRIVATEEYENOWPLAYSOUND(Sound S)
			{
				S.method_28(seb.method_506());
			}

			foreach(var part in allParts){
				PartSimState pss = partSimStates[part];
				var type = part.method_1159();
				// look for 3 unheld QSs and free gold
				if(type == Cardinalification){
					// if all the atoms exist...
					AtomType Demetal = default(AtomType);
					if(sim.FindAtomRelative(part, new HexIndex(0, 0)).method_99(out AtomReference qs)
					   && sim.FindAtomRelative(part, new(1, 0)).method_99(out AtomReference word)){
						// and are the right types...
						if(Flexibility.applyCardinalificationRule(qs.field_2280,out Demetal)
						   && word.field_2280 == Atoms.Wordexis){
							// and the quicksilver is not being consumed or held...
							if(!word.field_2281 && !word.field_2282){
								// transmute the gold and destroy the quicksilver
								qs.field_2277.method_1106(Demetal, qs.field_2278);
								word.field_2277.method_1107(word.field_2278);
								// show the removal effects for qs
								seb.field_3937.Add(new class_286(seb, word.field_2278, Atoms.Wordexis));
								// upgrade effect for gold -> uranium
								qs.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, qs.field_2280, class_238.field_1989.field_81.field_614, 30f);
								YOUARENOTAPRIVATEEYENOWPLAYSOUND(class_238.field_1991.field_1843);
							}
						}
					}
				}else if(type == Liquidation){
					// if all the atoms exist...
					AtomType Demetal = default(AtomType);
					if(sim.FindAtomRelative(part, new HexIndex(0, 0)).method_99(out AtomReference qs)
					   && sim.FindAtomRelative(part, new(1, 0)).method_99(out AtomReference word)){
						// and are the right types...
						if(Flexibility.applyLiquidationRule(qs.field_2280,out Demetal)
						   && word.field_2280 == Atoms.Wordexis){
							// and the quicksilver is not being consumed or held...
							if(!word.field_2281 && !word.field_2282){
								// transmute the gold and destroy the quicksilver
								qs.field_2277.method_1106(Demetal, qs.field_2278);
								word.field_2277.method_1107(word.field_2278);
								// show the removal effects for qs
								seb.field_3937.Add(new class_286(seb, word.field_2278, Atoms.Wordexis));
								// upgrade effect for gold -> uranium
								qs.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, qs.field_2280, class_238.field_1989.field_81.field_614, 30f);
								YOUARENOTAPRIVATEEYENOWPLAYSOUND(class_238.field_1991.field_1843);
							}
						}
					}
				}else if(type == Gerioification){
					// if all the atoms exist...
					if(sim.FindAtomRelative(part, new HexIndex(0, 0)).method_99(out AtomReference qs)
					   && sim.FindAtomRelative(part, new(1, 0)).method_99(out AtomReference word)){
						// and are the right types...
						if((qs.field_2280 == AtomTypes.field_1675 || qs.field_2280 == AtomTypes.field_1680)
						   && word.field_2280 == AtomTypes.field_1690){
							// and the quicksilver is not being consumed or held...
							if(!word.field_2281 && !word.field_2282){
								// transmute the gold and destroy the quicksilver
								qs.field_2277.method_1106(Atoms.Wordexis, qs.field_2278);
								word.field_2277.method_1107(word.field_2278);
								// show the removal effects for qs
								seb.field_3937.Add(new class_286(seb, word.field_2278, AtomTypes.field_1690));
								// upgrade effect for gold -> uranium
								qs.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, qs.field_2280, class_238.field_1989.field_81.field_614, 30f);
								YOUARENOTAPRIVATEEYENOWPLAYSOUND(class_238.field_1991.field_1843);
							}
						}
					}
				}else if(type == Metallification){
					// if all the atoms exist...
					AtomType Demetal = default(AtomType);
					if(sim.FindAtomRelative(part, new HexIndex(0, 0)).method_99(out AtomReference bowl)){
						// and are the right types...
						if(Flexibility.applyMetallificationRule(bowl.field_2280,out Demetal)){
							// transmute the gold and destroy the quicksilver
							bowl.field_2277.method_1106(Demetal, bowl.field_2278);
							// upgrade effect for gold -> uranium
							bowl.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, bowl.field_2280, class_238.field_1989.field_81.field_614, 30f);
							YOUARENOTAPRIVATEEYENOWPLAYSOUND(class_238.field_1991.field_1845);
						}
					}
				}else if(type == Demetallification){
					// if all the atoms exist...
					AtomType Demetal = default(AtomType);
					if(sim.FindAtomRelative(part, new HexIndex(0, 0)).method_99(out AtomReference bowl)){
						// and are the right types...
						if(Flexibility.applyDemetallificationRule(bowl.field_2280,out Demetal)){
							// transmute the gold and destroy the quicksilver
							bowl.field_2277.method_1106(Demetal, bowl.field_2278);
							// upgrade effect for gold -> uranium
							bowl.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, bowl.field_2280, class_238.field_1989.field_81.field_614, 30f);
							YOUARENOTAPRIVATEEYENOWPLAYSOUND(class_238.field_1991.field_1845);
						}
						if(false && Flexibility.DemetallificationExplosionMeta.Contains(bowl.field_2280)){ //make it as part of rule
							seb.method_518(first ? 0f : 1f, class_134.method_253("Spontaneous quicksilver explosion is not allowed.", string.Empty), new Vector2[1] { class_187.field_1742.method_492(part.method_1184(new(0, 0))) });
						}
					}
				}else if(type == AtomicProjection){
					// if all the atoms exist...
					if(sim.FindAtomRelative(part, new HexIndex(0, 0)).method_99(out AtomReference qs)
					   && sim.FindAtomRelative(part, new(1, 0)).method_99(out AtomReference word)){
						// and are the right types...
						if(Atoms.IsAndGetPTableAtom(qs.field_2280, out int WinosPrime)
						   && word.field_2280 == Atoms.Proton){
							// and the quicksilver is not being consumed or held...
							if(!word.field_2281 && !word.field_2282 && WinosPrime < 118-1){
								// transmute the gold and destroy the quicksilver
								qs.field_2277.method_1106(Atoms.PTableAtoms[WinosPrime+1], qs.field_2278);
								word.field_2277.method_1107(word.field_2278);
								// show the removal effects for qs
								seb.field_3937.Add(new class_286(seb, word.field_2278, Atoms.Proton));
								// upgrade effect for gold -> uranium
								qs.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, qs.field_2280, class_238.field_1989.field_81.field_614, 30f);
								YOUARENOTAPRIVATEEYENOWPLAYSOUND(class_238.field_1991.field_1844);
							}
						}
					}
				}else if(type == AtomicRejection){
					// if all the atoms exist...
					if(sim.FindAtomRelative(part, new HexIndex(0, 0)).method_99(out AtomReference qs)
					   && !sim.FindAtomRelative(part, new(1, 0)).method_99(out _)){
						// and are the right types...
						if(Atoms.IsAndGetPTableAtom(qs.field_2280, out int WinosPrime)){
							// and the quicksilver is not being consumed or held...
							if(WinosPrime > 0){
								// transmute the gold and destroy the quicksilver
								qs.field_2277.method_1106(Atoms.PTableAtoms[WinosPrime-1], qs.field_2278);
								Molecule molecule = new Molecule();
								molecule.method_1105(new Atom(Atoms.Proton), part.method_1184(new(1, 0)));
								moleculeList.Add(molecule);
								// show the removal effects for qs
								//seb.field_3937.Add(new class_286(seb, word.field_2278, Atoms.Wordexis));
								// upgrade effect for gold -> uranium
								qs.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, qs.field_2280, class_238.field_1989.field_81.field_614, 30f);
								YOUARENOTAPRIVATEEYENOWPLAYSOUND(class_238.field_1991.field_1845);
							}
						}
					}
				}

				else if(type == AnimisStabilizer){
					if (first)
					{
						// if all the atoms exist...
						if(sim.FindAtomRelative(part, new HexIndex(0, 1)).method_99(out AtomReference zena1)
						   && sim.FindAtomRelative(part, new (1, -1)).method_99(out AtomReference zena2)
						   && !sim.FindAtomRelative(part, new (0, 0)).method_99(out _)
						   && !sim.FindAtomRelative(part, new (1, 0)).method_99(out _)){
							// and are the right types...
							if(ZenaLib.RecipeManager.CheckRecipes(new AtomType[2]{zena1.field_2280,zena2.field_2280},Flexibility.AntiAnimismusRecipes.ToArray(),out AtomType[] Dumper)){
								// and the quicksilver is not being consumed or held...
								if(!zena1.field_2281 && !zena1.field_2282
								&& !zena2.field_2281 && !zena2.field_2282
								){
									// transmute the gold and destroy the quicksilver
									zena1.field_2277.method_1107(zena1.field_2278);
									zena2.field_2277.method_1107(zena2.field_2278);
									// show the removal effects for qs
									seb.field_3937.Add(new class_286(seb, zena1.field_2278, zena1.field_2280));
									seb.field_3937.Add(new class_286(seb, zena2.field_2278, zena2.field_2280));
									sim.field_3826.Add(new()
									{
										field_3850 = (Sim.enum_190)0,
										field_3851 = class_187.field_1742.method_492(part.method_1184(new(0, 0))),
										field_3852 = 15f
									});
									sim.field_3826.Add(new()
									{
										field_3850 = (Sim.enum_190)0,
										field_3851 = class_187.field_1742.method_492(part.method_1184(new(1, 0))),
										field_3852 = 15f
									});
									// upgrade effect for gold -> uranium
									pss.field_2743 = true;
									pss.field_2744 = Dumper;
									YOUARENOTAPRIVATEEYENOWPLAYSOUND(class_238.field_1991.field_1838);
								}
							}
						}
					}
					else if (pss.field_2743)
					{
						Molecule molecule = new Molecule();
						molecule.method_1105(new Atom(pss.field_2744[0]), part.method_1184(new(0, 0)));
						moleculeList.Add(molecule);
						molecule = new Molecule();
						molecule.method_1105(new Atom(pss.field_2744[1]), part.method_1184(new(1, 0)));
						moleculeList.Add(molecule);
					}
				}else if(type == MagneticPolarizer){
					// if all the atoms exist...
					if(sim.FindAtomRelative(part, new HexIndex(0, 0)).method_99(out AtomReference zena1)
					   && sim.FindAtomRelative(part, new (1, 0)).method_99(out AtomReference zena2)){
						// and are the right types...
						if(zena1.field_2280 == Atoms.Zena && zena2.field_2280 == Atoms.Zena){
							// transmute the gold and destroy the quicksilver
							zena1.field_2277.method_1106(Atoms.RedZena, zena1.field_2278);
							zena2.field_2277.method_1106(Atoms.BlueZena, zena2.field_2278);
							// show the removal effects for qs
							//seb.field_3937.Add(new class_286(seb, word.field_2278, Atoms.Wordexis));
							// upgrade effect for gold -> uranium
							Texture[] disposalFlashAnimation = class_238.field_1989.field_90.field_240;
							seb.field_3936.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new(0, 0))+new HexIndex(1, 0)), disposalFlashAnimation, 30f, Vector2.Zero, 0f));
							seb.field_3936.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new(1, 0))+new HexIndex(1, 0)), disposalFlashAnimation, 30f, Vector2.Zero, 0f));
							YOUARENOTAPRIVATEEYENOWPLAYSOUND(class_238.field_1991.field_1840);
						}
					}
				}else if(type == MagneticDepolarizer){
					// if all the atoms exist...
					if(sim.FindAtomRelative(part, new HexIndex(0, 0)).method_99(out AtomReference zena1)
					   && sim.FindAtomRelative(part, new (1, 0)).method_99(out AtomReference zena2)){
						// and are the right types...
						if((zena1.field_2280 == Atoms.RedZena && zena2.field_2280 == Atoms.BlueZena)
						||(zena2.field_2280 == Atoms.RedZena && zena1.field_2280 == Atoms.BlueZena)){
							// transmute the gold and destroy the quicksilver
							zena1.field_2277.method_1106(Atoms.Zena, zena1.field_2278);
							zena2.field_2277.method_1106(Atoms.Zena, zena2.field_2278);
							// show the removal effects for qs
							//seb.field_3937.Add(new class_286(seb, word.field_2278, Atoms.Wordexis));
							// upgrade effect for gold -> uranium
							Texture[] disposalFlashAnimation = class_238.field_1989.field_90.field_240;
							seb.field_3936.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new(0, 0))+new HexIndex(1, 0)), disposalFlashAnimation, 30f, Vector2.Zero, 0f));
							seb.field_3936.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new(1, 0))+new HexIndex(1, 0)), disposalFlashAnimation, 30f, Vector2.Zero, 0f));
							YOUARENOTAPRIVATEEYENOWPLAYSOUND(class_238.field_1991.field_1840);
						}
					}
				}else if(type == class_191.field_1775)/* Triplex bonder */{
					foreach (class_222 bonder in type.field_1538)
					{
						if (!sim.FindAtomRelative(part, bonder.field_1920).method_99(out AtomReference leftAtom) || !sim.FindAtomRelative(part, bonder.field_1921).method_99(out AtomReference rightAtom))
						{
							continue;
						}
						if (Flexibility.checkTriplexCondition(leftAtom.field_2280,rightAtom.field_2280) || Flexibility.checkTriplexCondition(rightAtom.field_2280,leftAtom.field_2280))
						{
							if (leftAtom.field_2277 != rightAtom.field_2277){
								sim.field_3823.Remove(leftAtom.field_2277);
								sim.field_3823.Remove(rightAtom.field_2277);
								sim.field_3823.Add(leftAtom.field_2277.method_1119(rightAtom.field_2277));
							}
							BondEffect bondEffect = new BondEffect(sim.field_3818, (enum_7)1, bonder.field_1922.method_779().field_1817, 60f, bonder.field_1922.method_779().field_1818);
							if (leftAtom.field_2277.method_1112(bonder.field_1922.method_779().field_1814, part.method_1184(bonder.field_1920), part.method_1184(bonder.field_1921), bondEffect))
								YOUARENOTAPRIVATEEYENOWPLAYSOUND(bonder.field_1922.method_779().field_1820);
						}
					}
				}else if(type == class_191.field_1780)/* Animismus */{
					// vanilla code runs first, so what if i do a hacky workaround
					if (first && pss.field_2743)
					{
						try
						{
							if (pss.field_2744.Length < 2)
								pss.field_2744 = new AtomType[2] { class_175.field_1687, class_175.field_1688 };
						}
						catch (Exception _)
						{
							pss.field_2744 = new AtomType[2] { class_175.field_1687, class_175.field_1688 };
						}
					}
					else if (first)
					{
						if(!sim.FindAtomRelative(part, new HexIndex(1, -1)).method_99(out _)
						   && !sim.FindAtomRelative(part, new (0, 1)).method_99(out _)
						   && sim.FindAtomRelative(part, new (0, 0)).method_99(out AtomReference Input1)
						   && sim.FindAtomRelative(part, new (1, 0)).method_99(out AtomReference Input2)){
							// and are the right types...
							if(ZenaLib.RecipeManager.CheckRecipes(new AtomType[2]{Input1.field_2280,Input2.field_2280},Flexibility.AnimismusRecipes.ToArray(),out AtomType[] Dumper)){
								// and the quicksilver is not being consumed or held...
								if(!Input1.field_2281 && !Input1.field_2282
								&& !Input2.field_2281 && !Input2.field_2282){
									// transmute the gold and destroy the quicksilver
									Input1.field_2277.method_1107(Input1.field_2278);
									Input2.field_2277.method_1107(Input2.field_2278);
									//Molecule molecule = new Molecule();
									//molecule.method_1105(new Atom(Atoms.Abomination), part.method_1184(new(0, 0)));
									//moleculeList.Add(molecule);
									// show the removal effects for qs
									Texture[] disposalFlashAnimation = class_238.field_1989.field_90.field_240;
									//seb.field_3936.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new(1, 1))), disposalFlashAnimation, 30f, Vector2.Zero, 0f));
									//seb.field_3936.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new(1, -1))), disposalFlashAnimation, 30f, Vector2.Zero, 0f));
									//seb.field_3936.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new(0, 1))), disposalFlashAnimation, 30f, Vector2.Zero, 0f));
									//seb.field_3936.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new(2, -1))), disposalFlashAnimation, 30f, Vector2.Zero, 0f));
									sim.field_3826.Add(new()
									{
										field_3850 = (Sim.enum_190)0,
										field_3851 = class_187.field_1742.method_492(part.method_1184(new(0, 1))),
										field_3852 = 15f
									});
									sim.field_3826.Add(new()
									{
										field_3850 = (Sim.enum_190)0,
										field_3851 = class_187.field_1742.method_492(part.method_1184(new(1, -1))),
										field_3852 = 15f
									});
									YOUARENOTAPRIVATEEYENOWPLAYSOUND(class_238.field_1991.field_1838);
									pss.field_2743 = true;
									pss.field_2744 = Dumper;
									//seb.field_3937.Add(new class_286(seb, word.field_2278, Atoms.Wordexis));
									// upgrade effect for gold -> uranium
									//qs.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, qs.field_2280, class_238.field_1989.field_81.field_614, 30f);
								}
							}
						}
					}
					else if (pss.field_2743)
					{
						if(sim.FindAtomRelative(part, new HexIndex(1, -1)).method_99(out AtomReference ToBeRemoved)) 
							ToBeRemoved.field_2277.method_1107(ToBeRemoved.field_2278);
						if(sim.FindAtomRelative(part, new HexIndex(0, 1)).method_99(out AtomReference ToBeRemoved2)) 
							ToBeRemoved2.field_2277.method_1107(ToBeRemoved2.field_2278);
						Molecule molecule = new Molecule();
						molecule.method_1105(new Atom(pss.field_2744[0]), part.method_1184(new(0, 1)));
						moleculeList.Add(molecule);
						molecule = new Molecule();
						molecule.method_1105(new Atom(pss.field_2744[1]), part.method_1184(new(1, -1)));
						moleculeList.Add(molecule);
					}
				}else if(type == class_191.field_1783)/* Unification */{
					// vanilla code runs first, so what if i do a hacky workaround
					if (first && pss.field_2743)
					{
						if (pss.field_2744.Length < 5)
							pss.field_2744 = new AtomType[5] { pss.field_2744[0], pss.field_2744[1], pss.field_2744[2], pss.field_2744[3], class_175.field_1690 };
					}
					else if (first)
					{
						if(sim.FindAtomRelative(part, new HexIndex(0, 1)).method_99(out AtomReference Input1)
						   && sim.FindAtomRelative(part, new (0, -1)).method_99(out AtomReference Input2)
						   && sim.FindAtomRelative(part, new (-1, 1)).method_99(out AtomReference Input3)
						   && sim.FindAtomRelative(part, new (1, -1)).method_99(out AtomReference Input4)
						   && !sim.FindAtomRelative(part, new (0, 0)).method_99(out _)){
							// and are the right types...
							if(ZenaLib.RecipeManager.CheckRecipes(new AtomType[4]{Input1.field_2280,Input2.field_2280,Input3.field_2280,Input4.field_2280},Flexibility.UnifyRecipes.ToArray(),out AtomType[] Dumper)){
								// and the quicksilver is not being consumed or held...
								if(!Input1.field_2281 && !Input1.field_2282
								&& !Input2.field_2281 && !Input2.field_2282
								&& !Input3.field_2281 && !Input3.field_2282
								&& !Input4.field_2281 && !Input4.field_2282
								){
									// transmute the gold and destroy the quicksilver
									Input1.field_2277.method_1107(Input1.field_2278);
									Input2.field_2277.method_1107(Input2.field_2278);
									Input3.field_2277.method_1107(Input3.field_2278);
									Input4.field_2277.method_1107(Input4.field_2278);
									//Molecule molecule = new Molecule();
									//molecule.method_1105(new Atom(Atoms.Abomination), part.method_1184(new(0, 0)));
									//moleculeList.Add(molecule);
									// show the removal effects for qs
									Texture[] disposalFlashAnimation = class_238.field_1989.field_90.field_240;
									//seb.field_3936.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new(1, 1))), disposalFlashAnimation, 30f, Vector2.Zero, 0f));
									//seb.field_3936.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new(1, -1))), disposalFlashAnimation, 30f, Vector2.Zero, 0f));
									//seb.field_3936.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new(0, 1))), disposalFlashAnimation, 30f, Vector2.Zero, 0f));
									//seb.field_3936.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new(2, -1))), disposalFlashAnimation, 30f, Vector2.Zero, 0f));
									sim.field_3826.Add(new()
									{
										field_3850 = (Sim.enum_190)0,
										field_3851 = class_187.field_1742.method_492(part.method_1184(new(0, 0))),
										field_3852 = 15f
									});
									YOUARENOTAPRIVATEEYENOWPLAYSOUND(class_238.field_1991.field_1850);
									pss.field_2743 = true;
									pss.field_2744 = new AtomType[5] { Input3.field_2280, Input1.field_2280, Input2.field_2280, Input4.field_2280, Dumper[0] };
									//seb.field_3937.Add(new class_286(seb, word.field_2278, Atoms.Wordexis));
									// upgrade effect for gold -> uranium
									//qs.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, qs.field_2280, class_238.field_1989.field_81.field_614, 30f);
								}
							}
						}
					}
					else if (pss.field_2743)
					{
						if(sim.FindAtomRelative(part, new HexIndex(0, 0)).method_99(out AtomReference ToBeRemoved)) 
							ToBeRemoved.field_2277.method_1107(ToBeRemoved.field_2278);
						Molecule molecule = new Molecule();
						molecule.method_1105(new Atom(pss.field_2744[4]), part.method_1184(new(0, 0)));
						moleculeList.Add(molecule);
					}
				}else if(type == class_191.field_1784)/* Dispersion */{
					// vanilla code runs first, so what if i do a hacky workaround
					if (first && pss.field_2743)
					{
						try
						{
							if (pss.field_2744.Length < 4)
								pss.field_2744 = new AtomType[4] { class_175.field_1676, class_175.field_1678, class_175.field_1679, class_175.field_1677 };
						}
						catch (Exception _)
						{
							pss.field_2744 = new AtomType[4] { class_175.field_1676, class_175.field_1678, class_175.field_1679, class_175.field_1677 };
						}
					}
					else if (first)
					{
						if(!sim.FindAtomRelative(part, new HexIndex(-1, 0)).method_99(out _)
						   && !sim.FindAtomRelative(part, new (0, -1)).method_99(out _)
						   && !sim.FindAtomRelative(part, new (1, -1)).method_99(out _)
						   && !sim.FindAtomRelative(part, new (1, 0)).method_99(out _)
						   && sim.FindAtomRelative(part, new (0, 0)).method_99(out AtomReference Input)){
							// and are the right types...
							if(ZenaLib.RecipeManager.CheckRecipes(new AtomType[1]{Input.field_2280},Flexibility.DisperseRecipes.ToArray(),out AtomType[] Dumper)){
								// and the quicksilver is not being consumed or held...
								if(!Input.field_2281 && !Input.field_2282){
									// transmute the gold and destroy the quicksilver
									Input.field_2277.method_1107(Input.field_2278);
									//Molecule molecule = new Molecule();
									//molecule.method_1105(new Atom(Atoms.Abomination), part.method_1184(new(0, 0)));
									//moleculeList.Add(molecule);
									// show the removal effects for qs
									Texture[] disposalFlashAnimation = class_238.field_1989.field_90.field_240;
									//seb.field_3936.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new(1, 1))), disposalFlashAnimation, 30f, Vector2.Zero, 0f));
									//seb.field_3936.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new(1, -1))), disposalFlashAnimation, 30f, Vector2.Zero, 0f));
									//seb.field_3936.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new(0, 1))), disposalFlashAnimation, 30f, Vector2.Zero, 0f));
									//seb.field_3936.Add(new class_228(seb, (enum_7)1, class_187.field_1742.method_492(part.method_1184(new(2, -1))), disposalFlashAnimation, 30f, Vector2.Zero, 0f));
									sim.field_3826.Add(new()
									{
										field_3850 = (Sim.enum_190)0,
										field_3851 = class_187.field_1742.method_492(part.method_1184(new(-1, 0))),
										field_3852 = 15f
									});
									sim.field_3826.Add(new()
									{
										field_3850 = (Sim.enum_190)0,
										field_3851 = class_187.field_1742.method_492(part.method_1184(new(0, -1))),
										field_3852 = 15f
									});
									sim.field_3826.Add(new()
									{
										field_3850 = (Sim.enum_190)0,
										field_3851 = class_187.field_1742.method_492(part.method_1184(new(1, -1))),
										field_3852 = 15f
									});
									sim.field_3826.Add(new()
									{
										field_3850 = (Sim.enum_190)0,
										field_3851 = class_187.field_1742.method_492(part.method_1184(new(1, 0))),
										field_3852 = 15f
									});
									YOUARENOTAPRIVATEEYENOWPLAYSOUND(class_238.field_1991.field_1841);
									pss.field_2743 = true;
									pss.field_2744 = Dumper;
									//seb.field_3937.Add(new class_286(seb, word.field_2278, Atoms.Wordexis));
									// upgrade effect for gold -> uranium
									//qs.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, qs.field_2280, class_238.field_1989.field_81.field_614, 30f);
								}
							}
						}
					}
					else if (pss.field_2743)
					{
						if(sim.FindAtomRelative(part, new HexIndex(-1, 0)).method_99(out AtomReference ToBeRemoved)) 
							ToBeRemoved.field_2277.method_1107(ToBeRemoved.field_2278);
						if(sim.FindAtomRelative(part, new HexIndex(0, -1)).method_99(out AtomReference ToBeRemoved2)) 
							ToBeRemoved2.field_2277.method_1107(ToBeRemoved2.field_2278);
						if(sim.FindAtomRelative(part, new HexIndex(1, -1)).method_99(out AtomReference ToBeRemoved3)) 
							ToBeRemoved3.field_2277.method_1107(ToBeRemoved3.field_2278);
						if(sim.FindAtomRelative(part, new HexIndex(1, 0)).method_99(out AtomReference ToBeRemoved4)) 
							ToBeRemoved4.field_2277.method_1107(ToBeRemoved4.field_2278);
						Molecule molecule = new Molecule();
						molecule.method_1105(new Atom(pss.field_2744[0]), part.method_1184(new(-1, 0)));
						moleculeList.Add(molecule);
						molecule = new Molecule();
						molecule.method_1105(new Atom(pss.field_2744[1]), part.method_1184(new(0, -1)));
						moleculeList.Add(molecule);
						molecule = new Molecule();
						molecule.method_1105(new Atom(pss.field_2744[2]), part.method_1184(new(1, -1)));
						moleculeList.Add(molecule);
						molecule = new Molecule();
						molecule.method_1105(new Atom(pss.field_2744[3]), part.method_1184(new(1, 0)));
						moleculeList.Add(molecule);
					}
				}else if(type == class_191.field_1776)/* Calcinator */{
					AtomType Demetal = default(AtomType);
					if(sim.FindAtomRelative(part, new HexIndex(0, 0)).method_99(out AtomReference bowl)){
						// and are the right types...
						if(Flexibility.applyCalcinatorRule(bowl.field_2280,out Demetal)){
							// transmute the gold and destroy the quicksilver
							bowl.field_2277.method_1106(Demetal, bowl.field_2278);
							// upgrade effect for gold -> uranium
							bowl.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, bowl.field_2280, class_238.field_1989.field_81.field_614, 30f);
							YOUARENOTAPRIVATEEYENOWPLAYSOUND(class_238.field_1991.field_1840);
						}
					}
				}else if(type == class_191.field_1777)/* Duplicator */{
					if(sim.FindAtomRelative(part, new HexIndex(1, 0)).method_99(out AtomReference bowl)){
						//atom
						if (sim.FindAtomRelative(part, new HexIndex(0, 0)).method_99(out AtomReference dupe)){
							// and are the right types...
							if(Flexibility.checkDuplicatorCastable(bowl.field_2280,dupe.field_2280)){
								// transmute the gold and destroy the quicksilver
								bowl.field_2277.method_1106(dupe.field_2280, bowl.field_2278);
								// upgrade effect for gold -> uranium
								bowl.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, bowl.field_2280, class_238.field_1989.field_81.field_614, 30f);
								YOUARENOTAPRIVATEEYENOWPLAYSOUND(class_238.field_1991.field_1843);
							}
						}
						//baron
						if (FindBerloAtom(sim, part.method_1184(new(0, 0)), out AtomType dupe2)){
							// and are the right types...
							if(Flexibility.checkDuplicatorCastable(bowl.field_2280,dupe2)){
								// transmute the gold and destroy the quicksilver
								bowl.field_2277.method_1106(dupe2, bowl.field_2278);
								// upgrade effect for gold -> uranium
								bowl.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, bowl.field_2280, class_238.field_1989.field_81.field_614, 30f);
								YOUARENOTAPRIVATEEYENOWPLAYSOUND(class_238.field_1991.field_1843);
							}
						}
					}
				}
			}
		});
	}

	public static void Unload() { }
}