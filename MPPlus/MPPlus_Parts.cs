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

internal static class Parts
{
	public static bool IsHalvingLoaded = false;
	//season 1 glyphs
	public static PartType Cardinalification, Liquidation, Gerioification, Metallification, Demetallification;
	//season 2 glyphs
	public static PartType AnimisStabilizer, AtomicProjection, AtomicRejection;
	public static Texture ProjectionBase = class_235.method_615("textures/parts/projection_glyph/base");
	public static Texture ProjectionGlow = class_235.method_615("textures/select/double_glow");
	public static Texture ProjectionBowl = class_235.method_615("textures/parts/projection_glyph/metal_bowl");
	public static Texture ProjectionHole = class_235.method_615("textures/parts/projection_glyph/quicksilver_input");
    
	public static Texture Wordexis_Input = class_235.method_615("textures/parts/magicalparcher/inputs/wordexis_input");
	public static Texture Tric_Input = class_235.method_615("textures/parts/magicalparcher/inputs/tric_input");
	public static Texture QuicksilverSymbol = class_235.method_615("textures/parts/projection_glyph/quicksilver_symbol");
	public static Texture SaltSymbol = class_235.method_615("textures/parts/animismus/symbol_salt");
	public static Texture QuintessenceSymbol = class_235.method_615("textures/parts/dispersion/symbol_quintessence");
	public static Texture LeadSymbol = class_235.method_615("textures/atoms/lead_symbol");

	public static Texture CalcinatorBase = class_235.method_615("textures/parts/calcinator_base");
	public static Texture RejectionBowl = class_235.method_615("textures/parts/rejection_glyph/metal_bowl");

	private static bool ContentLoaded = false;
	public static void AddNewContent() {
		if (ContentLoaded) return;
		ContentLoaded = true;

		Flexibility.addMetallificationRule(AtomTypes.field_1680,AtomTypes.field_1681);
		Flexibility.addDemetallificationRule(AtomTypes.field_1681,AtomTypes.field_1680);

	    Cardinalification = new(){
	    	field_1528 = "magical-parcher-plus-cardinalification", // ID
	    	field_1529 = class_134.method_253("Haxior Cardinalification", string.Empty), // Name
	    	field_1530 = class_134.method_253("Transforms Quicksilver into Salt Using Wordexis.", string.Empty), // Description
	    	field_1531 = 40, // Cost
	    	field_1539 = true, // Is a glyph (?)
	    	field_1549 = class_238.field_1989.field_97.field_382, // Shadow/glow
	    	field_1550 = class_238.field_1989.field_97.field_383, // Stroke/outline
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
	    	field_1549 = class_238.field_1989.field_97.field_382, // Shadow/glow
	    	field_1550 = class_238.field_1989.field_97.field_383, // Stroke/outline
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
	    	field_1549 = class_238.field_1989.field_97.field_382, // Shadow/glow
	    	field_1550 = class_238.field_1989.field_97.field_383, // Stroke/outline
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



	    AtomicProjection = new(){
	    	field_1528 = "magical-parcher-plus-atomicprojection", // ID
	    	field_1529 = class_134.method_253("Haxior Projection", string.Empty), // Name
	    	field_1530 = class_134.method_253("Promotes the atom into next atomic number using Tric.", string.Empty), // Description
	    	field_1531 = 40, // Cost
	    	field_1539 = true, // Is a glyph (?)
	    	field_1549 = class_238.field_1989.field_97.field_382, // Shadow/glow
	    	field_1550 = class_238.field_1989.field_97.field_383, // Stroke/outline
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
	    	field_1530 = class_134.method_253("Demotes the atom into previous atomic number, Expelling Tric.", string.Empty), // Description
	    	field_1531 = 40, // Cost
	    	field_1539 = true, // Is a glyph (?)
	    	field_1549 = class_238.field_1989.field_97.field_382, // Shadow/glow
	    	field_1550 = class_238.field_1989.field_97.field_383, // Stroke/outline
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
        // the that code runs below
		QApi.RunAfterCycle((sim, first) => {
			var seb = sim.field_3818;
			List<Part> allParts = seb.method_502().field_3919;
			var simStates = sim.field_3821;
			var moleculeList = sim.field_3823;

			foreach(var part in allParts){
				var type = part.method_1159();
				// look for 3 unheld QSs and free gold
				if(type == Cardinalification){
					// if all the atoms exist...
					if(sim.FindAtomRelative(part, new HexIndex(0, 0)).method_99(out AtomReference qs)
					   && sim.FindAtomRelative(part, new(1, 0)).method_99(out AtomReference word)){
						// and are the right types...
						if(qs.field_2280 == AtomTypes.field_1680
						   && word.field_2280 == Atoms.Wordexis){
							// and the quicksilver is not being consumed or held...
							if(!word.field_2281 && !word.field_2282){
								// transmute the gold and destroy the quicksilver
								qs.field_2277.method_1106(AtomTypes.field_1675, qs.field_2278);
								word.field_2277.method_1107(word.field_2278);
								// show the removal effects for qs
								seb.field_3937.Add(new class_286(seb, word.field_2278, Atoms.Wordexis));
								// upgrade effect for gold -> uranium
								qs.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, qs.field_2280, class_238.field_1989.field_81.field_614, 30f);
							}
						}
					}
				}else if(type == Liquidation){
					// if all the atoms exist...
					if(sim.FindAtomRelative(part, new HexIndex(0, 0)).method_99(out AtomReference qs)
					   && sim.FindAtomRelative(part, new(1, 0)).method_99(out AtomReference word)){
						// and are the right types...
						if(qs.field_2280 == AtomTypes.field_1675
						   && word.field_2280 == Atoms.Wordexis){
							// and the quicksilver is not being consumed or held...
							if(!word.field_2281 && !word.field_2282){
								// transmute the gold and destroy the quicksilver
								qs.field_2277.method_1106(AtomTypes.field_1680, qs.field_2278);
								word.field_2277.method_1107(word.field_2278);
								// show the removal effects for qs
								seb.field_3937.Add(new class_286(seb, word.field_2278, Atoms.Wordexis));
								// upgrade effect for gold -> uranium
								qs.field_2279.field_2276 = new class_168(seb, 0, (enum_132)1, qs.field_2280, class_238.field_1989.field_81.field_614, 30f);
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
							}
						}
					}
				}
			}
		});
	}

	public static void Unload() { }
}