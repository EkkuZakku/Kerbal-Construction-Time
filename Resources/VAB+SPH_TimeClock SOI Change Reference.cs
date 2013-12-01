		private void GlobalSOICatchAll(double SecondsTillNextUpdate)
        {
            foreach (Vessel tmpVessel in FlightGlobals.Vessels)
            {
                //only track vessels, not debris, EVA, etc
                //and not the current vessel
                //and no SOI alarm for it within the threshold - THIS BIT NEEDS TUNING
                if (Settings.VesselTypesForSOI.Contains(tmpVessel.vesselType) 
					&&  	(tmpVessel!=KACWorkerGameState.CurrentVessel) 
					&&  	(Settings.Alarms.FirstOrDefault(a => (a.VesselID == tmpVessel.id.ToString()
							&& 	(a.TypeOfAlarm == KACAlarm.AlarmType.SOIChange)
							&&	(Math.Abs(a.Remaining.UT) < SecondsTillNextUpdate + Settings.AlarmAddSOIAutoThreshold)
							)) == null)
					)

                {
                    if (lstVessels.ContainsKey(tmpVessel.id.ToString()) == false)
                    {
                        //Add new Vessels
                        DebugLogFormatted(String.Format("Adding {0}-{1}-{2}-{3}", tmpVessel.id, tmpVessel.vesselName, tmpVessel.vesselType, tmpVessel.mainBody.bodyName));
                        lstVessels.Add(tmpVessel.id.ToString(), new KACVesselSOI(tmpVessel.vesselName, tmpVessel.mainBody.bodyName));
                    }
                    else
                    {
                        //get this vessel from the memory array we are keeping and compare to its SOI
                        if (lstVessels[tmpVessel.id.ToString()].SOIName != tmpVessel.mainBody.bodyName)
                        {
                            //Set a new alarm to display now
                            KACAlarm newAlarm = new KACAlarm(FlightGlobals.ActiveVessel.id.ToString(), tmpVessel.vesselName + "- SOI Catch",
                                tmpVessel.vesselName + " Has entered a new Sphere of Influence\r\n" +
                                "     Old SOI: " + lstVessels[tmpVessel.id.ToString()].SOIName + "\r\n" +
                                "     New SOI: " + tmpVessel.mainBody.bodyName,
                                 KACWorkerGameState.CurrentTime.UT, 0, KACAlarm.AlarmType.SOIChange,
                                (Settings.AlarmOnSOIChange_Action > 0), (Settings.AlarmOnSOIChange_Action > 1));
                            Settings.Alarms.Add(newAlarm);

                            DebugLogFormatted("Triggering SOI Alarm - " + newAlarm.Name);
                            newAlarm.Triggered = true;
                            newAlarm.Actioned = true;
                            if (Settings.AlarmOnSOIChange_Action > 1)
                            {
                                DebugLogFormatted(String.Format("{0}-Pausing Game", newAlarm.Name));
                                TimeWarp.SetRate(0, true);
                                FlightDriver.SetPause(true);
                            }
                            else if (Settings.AlarmOnSOIChange_Action > 0)
                            {
                                DebugLogFormatted(String.Format("{0}-Halt Warp", newAlarm.Name));
                                TimeWarp.SetRate(0, true);
                            }

                            //reset the name String for next check
                            lstVessels[tmpVessel.id.ToString()].SOIName = tmpVessel.mainBody.bodyName;
                        }
                    }
                }
            }
        }