﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Speech.Recognition;
using System.Speech.Recognition.SrgsGrammar;
using System.Diagnostics;
using System.Globalization;

namespace Articulate
{
    static class CommandPool
    {
        private static Subject subjectObject;
        private static Dictionary<string, Command> commandObjects;

        public static SrgsDocument BuildSrgsGrammar(CultureInfo cultureInfo)
        {
            SrgsDocument document = new SrgsDocument();
            document.Culture = cultureInfo;

            // make a new subject item and then add all of it's rules to the document
            subjectObject = new Subject();
            foreach(SrgsRule rule in subjectObject.RuleList)
            {
                document.Rules.Add(rule);
            }

            SrgsRuleRef subjectRef = new SrgsRuleRef(subjectObject.RootRule);

            // go through and add all of the commands
            commandObjects = new Dictionary<string, Command>();
            SrgsOneOf commandSet = new SrgsOneOf();

            // add just the subjects
            SrgsItem select = new SrgsItem();
            select.Add(new SrgsItem(subjectRef));
            select.Add(new SrgsSemanticInterpretationTag("out.subject=rules.subject;"));
            commandSet.Add(select);

            #region Commands
            #region Utilities

            // Open Map (M)
            Command map = new Command("MAP", new string[] { "map", "toggle map", "show map", "hide map", "open map", "close map" }, new[] { DirectInputEmulator.KeyPress(DirectInputKeys.M) });
            commandObjects.Add("MAP", map);
            commandSet.Add(map.Item);

            // Open Inventory (I)
            Command inventory = new Command("INVENTORY", new string[] { "inventory", "toggle inventory", "show inventory", "hide inventory", "open inventory", "close inventory" }, new[] { DirectInputEmulator.KeyPress(DirectInputKeys.I) });
            commandObjects.Add("INVENTORY", inventory);
            commandSet.Add(inventory.Item);

            // Lights (L)
            Command lights = new Command("LIGHTS", new string[] { "lights", "light", "flashlight", "torch", "laser" }, new[] { DirectInputEmulator.KeyPress(DirectInputKeys.L) });
            commandObjects.Add("LIGHTS", lights);
            commandSet.Add(lights.Item);


            #endregion

            #region Move (1)
            // return to formation (1)
            Command returnToFormation = new Command("FORMUP", new string[] { "return to formation", "form up", "fallback", "fall back", "regroup", "join up", "rally on me", "rally to me", "with me" }, new [] { DirectInputEmulator.KeyPress(DirectInputKeys.One), DirectInputEmulator.KeyPress(DirectInputKeys.One) }, subjectRef);
            commandObjects.Add("FORMUP", returnToFormation);
            commandSet.Add(returnToFormation.Item);

            // advance (2)
			Command advance = new Command("ADVANCE", new string[] { "advance", "move up" }, new[] { DirectInputEmulator.KeyPress(DirectInputKeys.One), DirectInputEmulator.KeyPress(DirectInputKeys.Two) }, subjectRef);
            commandObjects.Add("ADVANCE", advance);
            commandSet.Add(advance.Item);

            // stay back (3)
            Command stayBack = new Command("STAYBACK", new string[] { "stay back", "go back", "back up" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.One),  DirectInputEmulator.KeyPress(DirectInputKeys.Three) }, subjectRef);
            commandObjects.Add("STAYBACK", stayBack);
            commandSet.Add(stayBack.Item);

            // flank left (4)
            Command flankLeft = new Command("FLANKLEFT", new string[] { "flank left", "go left" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.One),  DirectInputEmulator.KeyPress(DirectInputKeys.Four) }, subjectRef);
            commandObjects.Add("FLANKLEFT", flankLeft);
            commandSet.Add(flankLeft.Item);

            // flank right (5)
            Command flankRight = new Command("FLANKRIGHT", new string[] { "flank right", "go right" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.One),  DirectInputEmulator.KeyPress(DirectInputKeys.Five) }, subjectRef);
            commandObjects.Add("FLANKRIGHT", flankRight);
            commandSet.Add(flankRight.Item);

            // stop (6)
            Command stop = new Command("STOP", new string[] { "stop", "hold position", "halt", "stay there", "stay here", "stay put" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.One),  DirectInputEmulator.KeyPress(DirectInputKeys.Six) }, subjectRef);
            commandObjects.Add("STOP", stop);
            commandSet.Add(stop.Item);

            // Wait for me (7)
            Command waitForMe = new Command("WAIT", new string[] { "wait for me", "wait up", "wait" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.One),  DirectInputEmulator.KeyPress(DirectInputKeys.Six) }, subjectRef);
            commandObjects.Add("WAIT", waitForMe);
            commandSet.Add(waitForMe.Item);

            // Find cover (8)
            Command cover = new Command("COVER", new string[] { "go for cover", "look for cover", "cover", "find cover", "get to cover", "hide", "take cover" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.One),  DirectInputEmulator.KeyPress(DirectInputKeys.Seven) }, subjectRef);
            commandObjects.Add("COVER", cover);
            commandSet.Add(cover.Item);

            // Next waypoint (9)
            Command nextWaypoint = new Command("NEXTWAYPOINT", new string[] { "next waypoint", "go to the next waypoint" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.One),  DirectInputEmulator.KeyPress(DirectInputKeys.Eight) }, subjectRef);
            commandObjects.Add("NEXTWAYPOINT", nextWaypoint);
            commandSet.Add(nextWaypoint.Item);

            // Move to (Space)
            Command moveTo = new Command("MOVETO", new string[] { "move to", "move", "move there", "move up" }, new[] { DirectInputEmulator.KeyPress(DirectInputKeys.Space) }, subjectRef);
            commandObjects.Add("MOVETO", moveTo);
            commandSet.Add(moveTo.Item);
            #endregion

            #region Target (2)
            // open menu
            Command openTargetMenu = new Command("OPENTARGET", new string[] { "show targets", "target menu", "open target menu", "targets" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Two) }, subjectRef);
            commandObjects.Add("OPENTARGET", openTargetMenu);
            commandSet.Add(openTargetMenu.Item);

            // cancel target (1)
            Command cancelTarget = new Command("CANCELTARGET", new string[] { "cancel target", "cancel targets", "no target" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Two),  DirectInputEmulator.KeyPress(DirectInputKeys.One) }, subjectRef);
            commandObjects.Add("CANCELTARGET", cancelTarget);
            commandSet.Add(cancelTarget.Item);
            #endregion

            #region Engage (3)
            // open fire (1)
            Command openFire = new Command("OPENFIRE", new string[] { "open fire", "go loud", "fire at will" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Three),  DirectInputEmulator.KeyPress(DirectInputKeys.One) }, subjectRef);
            commandObjects.Add("OPENFIRE", openFire);
            commandSet.Add(openFire.Item);

            // hold fire (2)
            Command holdFire = new Command("HOLDFIRE", new string[] { "hold fire", "go quiet", "cease fire" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Three),  DirectInputEmulator.KeyPress(DirectInputKeys.Two) }, subjectRef);
            commandObjects.Add("HOLDFIRE", holdFire);
            commandSet.Add(holdFire.Item);

            // fire (3)
            Command fire = new Command("FIRE", new string[] { "fire", "take the shot" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Three),  DirectInputEmulator.KeyPress(DirectInputKeys.Three) }, subjectRef);
            commandObjects.Add("FIRE", fire);
            commandSet.Add(fire.Item);

            // engage (4)
            Command engage = new Command("ENGAGE", new string[] { "engage", "move to engage" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Three),  DirectInputEmulator.KeyPress(DirectInputKeys.Four) }, subjectRef);
            commandObjects.Add("ENGAGE", engage);
            commandSet.Add(engage.Item);

            // engage at will (5)
            Command enageAtWill = new Command("ENGAGEATWILL", new string[] { "engage at will" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Three),  DirectInputEmulator.KeyPress(DirectInputKeys.Five) }, subjectRef);
            commandObjects.Add("ENGAGEATWILL", enageAtWill);
            commandSet.Add(enageAtWill.Item);

            // disengage (6)
            Command disengage = new Command("DISENGAGE", new string[] { "disengage" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Three),  DirectInputEmulator.KeyPress(DirectInputKeys.Six) }, subjectRef);
            commandObjects.Add("DISENGAGE", disengage);
            commandSet.Add(disengage.Item);

            // scan horizon (7)
            Command scanHorizon = new Command("SCANHORIZON", new string[] { "scan horizon", "scan the horizon" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Three),  DirectInputEmulator.KeyPress(DirectInputKeys.Seven) }, subjectRef);
            commandObjects.Add("SCANHORIZON", scanHorizon);
            commandSet.Add(scanHorizon.Item);

            // watch direction (8)
            // team direct object
            DirectObject direction = new DirectObject("directionDO");
            direction.Add(new string[] { "north" }, "NORTH", new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.One) });
            direction.Add(new string[] { "north east" }, "NORTHEAST", new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Two) });
            direction.Add(new string[] { "east" }, "EAST", new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Three) });
            direction.Add(new string[] { "south east" }, "SOUTHEAST", new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Four) });
            direction.Add(new string[] { "south" }, "SOUTH", new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Five) });
            direction.Add(new string[] { "south west" }, "SOUTHWEST", new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Six) });
            direction.Add(new string[] { "west" }, "WEST", new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Seven) });
            direction.Add(new string[] { "north west" }, "NORTHWEST", new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Eight) });

            foreach (SrgsRule rule in direction.RuleList)
            {
                document.Rules.Add(rule);
            }

            Command watch = new Command("WATCH", new string[] { "watch", "watch the"}, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Three),  DirectInputEmulator.KeyPress(DirectInputKeys.Eight) }, subjectRef, direction);
            commandObjects.Add("WATCH", watch);
            commandSet.Add(watch.Item);

            // suppressive fire (9) -Now works in-game
            Command suppresiveFire = new Command("SUPRESS", new string[] { "suppresive fire", "suppress" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Three),  DirectInputEmulator.KeyPress(DirectInputKeys.Nine) }, subjectRef);
            commandObjects.Add("SUPRESS", suppresiveFire);
            commandSet.Add(suppresiveFire.Item);
            #endregion

            #region Mount (4)
            // open mount menu
            Command openMountMenu = new Command("OPENMOUNT", new string[] { "show mount menu", "open mount menu", "show vehicles", "mount menu", "get in vehicle", "get in that vehicle"}, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Four) }, subjectRef);
            commandObjects.Add("OPENMOUNT", openMountMenu);
            commandSet.Add(openMountMenu.Item);

            // dismount (1)
            Command dismount = new Command("DISMOUNT", new string[] { "dismount", "get out" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Four),  DirectInputEmulator.KeyPress(DirectInputKeys.One) }, subjectRef);
            commandObjects.Add("DISMOUNT", dismount);
            commandSet.Add(dismount.Item);
            #endregion

            #region Status (5)
            
            // Low fuel
            Command lowfuel = new Command("LOWFUEL", new string[] { "fuel low", "bingo fuel", "be advised bingo fuel", "be advised low fuel", "be advised fuel low" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Five),  DirectInputEmulator.KeyPress(DirectInputKeys.Two) }, subjectRef);
            commandObjects.Add("LOWFUEL", lowfuel);
            commandSet.Add(lowfuel.Item);
            
            // Low Ammo
            Command lowammo = new Command("LOWAMMO", new string[] { "I'm running low on ammo", "running low on ammo", "ammo low" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Five),  DirectInputEmulator.KeyPress(DirectInputKeys.Three) }, subjectRef);
            commandObjects.Add("LOWAMMO", lowammo);
            commandSet.Add(lowammo.Item);
            
            // Injured
            Command injured = new Command("INJURED", new string[] { "I'm injured", "injured", "medic", "I need a medic", "I need some help", "I need some help now", "somebody help me", "wounded", "I'm hit", "fuck I'm hurt" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Five),  DirectInputEmulator.KeyPress(DirectInputKeys.Four) }, subjectRef);
            commandObjects.Add("INJURED", injured);
            commandSet.Add(injured.Item);
            
            // SITREP, Report Status
            Command sitrep = new Command("SITREP", new string[] { "report in over", "sitrep", "report status", "report in" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Five),  DirectInputEmulator.KeyPress(DirectInputKeys.Five) }, subjectRef);
            commandObjects.Add("SITREP", sitrep);
            commandSet.Add(sitrep.Item);
            
            // Under Fire
            Command underfire = new Command("underfire", new string[] { "I'm under fire", "taking fire", "under fire", "enemy fire" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Five),  DirectInputEmulator.KeyPress(DirectInputKeys.Six) }, subjectRef);
            commandObjects.Add("underfire", underfire);
            commandSet.Add(underfire.Item);
            
            // Target Neutralized
            Command hostiledown = new Command("HOSTILEDOWN", new string[] { "hostile down", "target down", "scratch one", "he is down", "target neutralized", "tango down" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Five),  DirectInputEmulator.KeyPress(DirectInputKeys.Seven) }, subjectRef);
            commandObjects.Add("HOSTILEDOWN", hostiledown);
            commandSet.Add(hostiledown.Item);
            
            // KIA, Lost one
            Command friendlydown = new Command("KIA", new string[] { "we have lost one", "man down", "he is hit", "shit we have got a man down", "we have got a man down" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Five),  DirectInputEmulator.KeyPress(DirectInputKeys.Eight) }, subjectRef);
            commandObjects.Add("KIA", friendlydown);
            commandSet.Add(friendlydown.Item);

            #endregion
            
            #region Action (6)
            // open menu
            Command openActionMenu = new Command("OPENACTION", new string[] { "show actions", "action menu", "perform action", "do action", "open action menu", "actions" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Six) }, subjectRef);
            commandObjects.Add("OPENACTION", openActionMenu);
            commandSet.Add(openActionMenu.Item);
            #endregion

            #region Combat Mode (7)
            // stealth (1)
            Command stealth = new Command("STEALTH", new string[] { "stealth", "stealthy", "stealth mode" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Seven),  DirectInputEmulator.KeyPress(DirectInputKeys.One) }, subjectRef);
            commandObjects.Add("STEALTH", stealth);
            commandSet.Add(stealth.Item);

            // combat (2)
            Command combat = new Command("COMBAT", new string[] { "combat", "danger", "combat mode" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Seven),  DirectInputEmulator.KeyPress(DirectInputKeys.Two) }, subjectRef);
            commandObjects.Add("COMBAT", combat);
            commandSet.Add(combat.Item);

            // aware (3)
			Command aware = new Command("AWARE", new string[] { "aware", "alert", "aware mode", "stay sharp", "stay frosty", "stay alert" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Seven),  DirectInputEmulator.KeyPress(DirectInputKeys.Three) }, subjectRef);
            commandObjects.Add("AWARE", aware);
            commandSet.Add(aware.Item);

            // relax (4)
            Command relax = new Command("RELAX", new string[] { "relax", "relaxed mode", "safe" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Seven),  DirectInputEmulator.KeyPress(DirectInputKeys.Four) }, subjectRef);
            commandObjects.Add("RELAX", relax);
            commandSet.Add(relax.Item);

            // stand up (6)
            Command standUp = new Command("STANDUP", new string[] { "stand up", "get up", "stand" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Seven),  DirectInputEmulator.KeyPress(DirectInputKeys.Six) }, subjectRef);
            commandObjects.Add("STANDUP", standUp);
            commandSet.Add(standUp.Item);
            
            // stay crouched (7)
            Command stayCrouched = new Command("CROUCH", new string[] { "get low", "crouch", "stay crouched", "stay low" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Seven),  DirectInputEmulator.KeyPress(DirectInputKeys.Seven) }, subjectRef);
            commandObjects.Add("CROUCH", stayCrouched);
            commandSet.Add(stayCrouched.Item);

            // go prone (8)
            Command goProne = new Command("PRONE", new string[] { "go prone", "get down", "prone", "hit the dirt", "down" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Seven),  DirectInputEmulator.KeyPress(DirectInputKeys.Eight) }, subjectRef);
            commandObjects.Add("PRONE", goProne);
            commandSet.Add(goProne.Item);

            // copy my stance (9)
            Command copyMyStance = new Command("COPYMYSTANCE", new string[] { "copy my stance", "default stance" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Seven),  DirectInputEmulator.KeyPress(DirectInputKeys.Nine) }, subjectRef);
            commandObjects.Add("COPYMYSTANCE", copyMyStance);
            commandSet.Add(copyMyStance.Item);
            #endregion

            #region Formation (8)
            // column (1)
            Command column = new Command("COLUMN", new string[] { "formation column", "form column"}, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Eight),  DirectInputEmulator.KeyPress(DirectInputKeys.One) }, subjectRef);
            commandObjects.Add("COLUMN", column);
            commandSet.Add(column.Item);

            // staggered column (2)
            Command staggeredColumn = new Command("STAGGEREDCOLUMN", new string[] { "formation staggered column", "form staggered column" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Eight),  DirectInputEmulator.KeyPress(DirectInputKeys.Two) }, subjectRef);
            commandObjects.Add("STAGGEREDCOLUMN", staggeredColumn);
            commandSet.Add(staggeredColumn.Item);

            // wedge (3)
            Command wedge = new Command("WEDGE", new string[] { "formation wedge", "form wedge" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Eight),  DirectInputEmulator.KeyPress(DirectInputKeys.Three) }, subjectRef);
            commandObjects.Add("WEDGE", wedge);
            commandSet.Add(wedge.Item);

            // echelon left (4)
            Command echelonLeft = new Command("ECHELONLEFT", new string[] { "formation echelon left", "form echelon left" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Eight),  DirectInputEmulator.KeyPress(DirectInputKeys.Four) }, subjectRef);
            commandObjects.Add("ECHELONLEFT", echelonLeft);
            commandSet.Add(echelonLeft.Item);

            // echelon right (5)
            Command echeloneRight = new Command("ECHELONRIGHT", new string[] { "formation echelon right", "form echelon right" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Eight),  DirectInputEmulator.KeyPress(DirectInputKeys.Five) }, subjectRef);
            commandObjects.Add("ECHELONRIGHT", echeloneRight);
            commandSet.Add(echeloneRight.Item);

            // vee (6)
            Command vee = new Command("VEE", new string[] { "formation vee", "form vee" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Eight),  DirectInputEmulator.KeyPress(DirectInputKeys.Six) }, subjectRef);
            commandObjects.Add("VEE", vee);
            commandSet.Add(vee.Item);

            // line (7)
            Command line = new Command("LINE", new string[] { "formation line", "form line" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Eight),  DirectInputEmulator.KeyPress(DirectInputKeys.Seven) }, subjectRef);
            commandObjects.Add("LINE", line);
            commandSet.Add(line.Item);

            // file (8)
            Command file = new Command("FILE", new string[] { "formation file", "form file" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Eight),  DirectInputEmulator.KeyPress(DirectInputKeys.Eight) }, subjectRef);
            commandObjects.Add("FILE", file);
            commandSet.Add(file.Item);

            // diamond (9)
            Command diamond = new Command("DIAMOND", new string[] { "formation diamond", "form diamond" }, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Eight),  DirectInputEmulator.KeyPress(DirectInputKeys.Nine) }, subjectRef);
            commandObjects.Add("DIAMOND", diamond);
            commandSet.Add(diamond.Item);
            #endregion

            #region Assign Team (9)
            // team direct object
            DirectObject team = new DirectObject("teamDO");
            team.Add(new string[] { "team red", "red" }, "RED", new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.One) });
            team.Add(new string[] { "team green", "green" }, "GREEN", new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Two) });
            team.Add(new string[] { "team blue", "blue" }, "BLUE", new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Three) });
            team.Add(new string[] { "team yellow", "yellow" }, "YELLOW", new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Four) });
            team.Add(new string[] { "team white", "white" }, "WHITE", new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Five) });

            foreach (SrgsRule rule in team.RuleList)
            {
                document.Rules.Add(rule);
            }

            Command assignTeam = new Command("ASSIGN", new string[] { "assign", "assign to", "add to", "switch to", "you're"}, new [] {  DirectInputEmulator.KeyPress(DirectInputKeys.Nine) }, subjectRef, team);
            commandObjects.Add("ASSIGN", assignTeam);
            commandSet.Add(assignTeam.Item);
            #endregion

            // not implemented
            #region WW_AiMenu
            #region Infantry Commands (1)
            // heal up (1)

            // rearm (2)

            // inventory (3)

            // fire on my lead (4)

            // garrison building (5)

            // clear building (6)

            // follow target (7)
            #endregion
            
            #region WayPoints (4)
            // set waypoints (1)

            // add waypoints (2)

            // move waypoints (3)

            // delete waypoints (4)

            // wait on waypoint (5)

            // cycle waypoint (6)
            #endregion
            #endregion
            #endregion
            
            // set the root rule to 
            SrgsRule commands = new SrgsRule("commands");
            commands.Add(commandSet);

            document.Rules.Add(commands);
            document.Root = commands;

            return document;
        }
        
        public static async Task ExecuteAsync(SemanticValue semantics)
        {
            // get the subject pieces
            List<string> subjects = parseSemanticList("subject", semantics);

            // get the command
            string command = parseSemantic("command", semantics);

            // get the directObject, if any
            string directObject = parseSemantic("directObject", semantics);
            
            if (subjects != null)
            {
                // execute subject keypresses
                foreach (string subject in subjects)
                {
                    Trace.WriteLine(subject);
                    if (subjectObject.KeyLookup.ContainsKey(subject))
                    {
						await DirectInputEmulator.SendInputAsync(subjectObject.KeyLookup[subject].SpaceOperations(Core.Instance.Configuration.KeyPressDelay));
                    }
                }
            }

            if (command != null && command != "")
            {
                // execute command keypresses
                Trace.WriteLine(command);
				await DirectInputEmulator.SendInputAsync(commandObjects[command].KeyLookup[command].SpaceOperations(Core.Instance.Configuration.KeyPressDelay));

                // execute direct object keypresses (if needed)
                if (directObject != null && directObject != "")
                {
                    Trace.WriteLine(directObject);
					await DirectInputEmulator.SendInputAsync(commandObjects[command].KeyLookup[directObject].SpaceOperations(Core.Instance.Configuration.KeyPressDelay));
                }
            }
        }

        private static string parseSemantic(string key, SemanticValue semantic)
        {
            if (semantic.ContainsKey(key))
            {
                // remove weird object Object from the ToString
                int index = semantic[key].Value.ToString().IndexOf("[object Object]");
                string result = (index < 0)
                    ? semantic[key].Value.ToString()
                    : semantic[key].Value.ToString().Remove(index, "[object Object]".Length);

                string[] resultPieces = result.Split('{', '}', ' ');

                foreach (string piece in resultPieces)
                {
                    // go through each piece of the semantic and add the non-trivial pieces to our list
                    if (piece != "")
                    {
                        // return the first valid piece, shouldn't be a list
                        return piece;
                    }
                }
            }

            return null;
        }

        private static List<string> parseSemanticList(string key, SemanticValue semantic)
        {
            if (semantic.ContainsKey(key))
            {
                List<string> list = new List<string>();

                // remove weird object Object from the ToString
                int index = semantic[key].Value.ToString().IndexOf("[object Object]");
                string result = (index < 0)
                    ? semantic[key].Value.ToString()
                    : semantic[key].Value.ToString().Remove(index, "[object Object]".Length);

                string[] resultPieces = result.Split('{', '}', ' ');

                foreach (string piece in resultPieces)
                {
                    // go through each piece of the semantic and add the non-trivial pieces to our list
                    if (piece != "")
                    {
                        list.Add(piece);
                    }
                }

                return list;
            }

            return null;
        }
    }
}
