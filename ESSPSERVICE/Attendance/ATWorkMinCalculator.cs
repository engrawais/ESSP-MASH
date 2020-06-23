using ESSPCORE.Attendance;
using ESSPCORE.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSPSERVICE.Attendance
{
    public static class ATWorkMinCalculator
    {
        #region -- Calculate Work Times --
        static DateTime Trim(this DateTime date, long roundTicks)
        {
            return new DateTime(date.Ticks - date.Ticks % roundTicks);
        }
        public static void CalculateShiftTimes(DailyAttendance attendanceRecord, MyShift shift, OTPolicy otPolicy)
        {
            try
            {
                attendanceRecord.TimeIn = attendanceRecord.TimeIn.Value.Trim(TimeSpan.TicksPerMinute);
                attendanceRecord.TimeOut = attendanceRecord.TimeOut.Value.Trim(TimeSpan.TicksPerMinute);
                // Break start and End Times
                DateTime ts = attendanceRecord.TimeIn.Value.Date + new TimeSpan(13, 0, 0);
                DateTime te = attendanceRecord.TimeIn.Value.Date + new TimeSpan(14, 0, 0);
                //Calculate Difference between Times
                #region --- For Time Difference Calculations--
                TimeSpan mins = new TimeSpan();
                if (shift.CalDiffOnly == true)
                {
                    TimeSpan? min4 = new TimeSpan();
                    TimeSpan? min5 = new TimeSpan();

                    TimeSpan min1 = new TimeSpan();
                    TimeSpan min2 = new TimeSpan();
                    TimeSpan min3 = new TimeSpan();
                    if (attendanceRecord.Tout0 != null && attendanceRecord.Tin0 != null)
                    {
                        attendanceRecord.Tin0 = attendanceRecord.Tin0.Value.Trim(TimeSpan.TicksPerMinute);
                        attendanceRecord.Tout0 = attendanceRecord.Tout0.Value.Trim(TimeSpan.TicksPerMinute);
                        min1 = (TimeSpan)(attendanceRecord.Tout0 - attendanceRecord.Tin0);
                    }
                    if (attendanceRecord.Tout1 != null && attendanceRecord.Tin1 != null)
                    {
                        attendanceRecord.Tin1 = attendanceRecord.Tin1.Value.Trim(TimeSpan.TicksPerMinute);
                        attendanceRecord.Tout1 = attendanceRecord.Tout1.Value.Trim(TimeSpan.TicksPerMinute);
                        min2 = (TimeSpan)(attendanceRecord.Tout1 - attendanceRecord.Tin1);
                    }
                    if (attendanceRecord.Tout2 != null && attendanceRecord.Tin2 != null)
                    {
                        min3 = (TimeSpan)(attendanceRecord.Tout2 - attendanceRecord.Tin2);
                    }
                    mins = min1 + min2 + min3;
                    if (min4 != null)
                        mins = mins + (TimeSpan)min4;
                    if (min5 != null)
                        mins = mins + (TimeSpan)min5;
                }
                else
                {
                    mins = (TimeSpan)(attendanceRecord.TimeOut - attendanceRecord.TimeIn);
                }
                #endregion
                double _workHours = mins.TotalHours;
                attendanceRecord.WorkMin = (short)(mins.TotalMinutes);

                if (attendanceRecord.WorkMin > 0)
                {
                    if (attendanceRecord.Remarks != null)
                    {
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[Absent]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[Manual]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LI]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[M]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[N-OT]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LO]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LO]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[EI]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[EO]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[R-OT]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[G-OT]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[HA]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[DO]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[GZ]", "");
                    }
                    else
                    {
                        attendanceRecord.Remarks = "";
                    }
                    if (attendanceRecord.StatusMN == true)
                        attendanceRecord.Remarks = "[M]" + attendanceRecord.Remarks;
                    //Check if GZ holiday then place all WorkMin in GZOTMin
                    if (attendanceRecord.StatusGZ == true && attendanceRecord.DutyCode == "G")
                    {
                        #region -- GZ Calculation--
                        if (otPolicy.CalculateGZOT == true)
                        {
                            if (otPolicy.PerDayGOTLimitHour >= _workHours)
                            {
                                int hour = (int)(mins.TotalMinutes / 60);
                                int min = hour * 60;
                                int remainingmin = (int)mins.TotalMinutes - min;
                                if (remainingmin >= otPolicy.MinMinutesForOneHour)
                                {
                                    attendanceRecord.GZOTMin = (short)((hour + 1) * 60);
                                }
                                else
                                {
                                    attendanceRecord.GZOTMin = (short)((hour) * 60);
                                }
                            }
                            else
                            {
                                int policyOTLimitMin = (int)(otPolicy.PerDayGOTLimitHour * 60.0);
                                attendanceRecord.GZOTMin = (short)policyOTLimitMin;
                            }
                        }
                        else
                        {
                            attendanceRecord.WorkMin = 0;
                            attendanceRecord.ExtraMin = (short)mins.TotalMinutes;
                        }
                        if (attendanceRecord.GZOTMin > 0)
                        {
                            attendanceRecord.StatusGZOT = true;
                            attendanceRecord.Remarks = attendanceRecord.Remarks + "[G-OT]";
                        }
                        #endregion
                    }
                    //if Rest day then place all WorkMin in OTMin
                    else if (attendanceRecord.StatusDO == true && attendanceRecord.DutyCode == "R")
                    {
                        #region -- Rest Calculation --
                        if (otPolicy.CalculateRestOT == true)
                        {
                            if (otPolicy.PerDayROTLimitHour >= _workHours)
                            {
                                if (mins.TotalMinutes < otPolicy.MinMinutesForOneHour)
                                    attendanceRecord.OTMin = 0;
                                else if (attendanceRecord.OTMin >= otPolicy.MinMinutesForOneHour && attendanceRecord.OTMin <= 61)
                                {
                                    attendanceRecord.OTMin = 60;
                                }
                                else
                                {
                                    int hour = (int)(mins.TotalMinutes / 60);
                                    int min = hour * 60;
                                    int remainingmin = (int)mins.TotalMinutes - min;
                                    if (remainingmin >= otPolicy.MinMinutesForOneHour)
                                    {
                                        attendanceRecord.OTMin = (short)((hour + 1) * 60);
                                    }
                                    else
                                    {
                                        attendanceRecord.OTMin = (short)((hour) * 60);
                                    }
                                }
                            }
                            else if (otPolicy.PerDayROTLimitHour == 0)
                            {
                                int hour = (int)(mins.TotalMinutes / 60);
                                int min = hour * 60;
                                int remainingmin = (int)mins.TotalMinutes - min;
                                if (remainingmin >= otPolicy.MinMinutesForOneHour)
                                {
                                    attendanceRecord.OTMin = (short)((hour + 1) * 60);
                                }
                                else
                                {
                                    attendanceRecord.OTMin = (short)((hour) * 60);
                                }
                            }
                            else
                            {
                                int policyOTLimitMin = (int)(otPolicy.PerDayROTLimitHour * 60.0);
                                attendanceRecord.OTMin = (short)policyOTLimitMin;
                            }
                        }
                        else
                        {
                            attendanceRecord.WorkMin = 0;
                            attendanceRecord.ExtraMin = (short)mins.TotalMinutes;
                        }
                        if (attendanceRecord.OTMin > 0)
                        {
                            attendanceRecord.StatusOT = true;
                            attendanceRecord.Remarks = attendanceRecord.Remarks + "[R-OT]";
                        }
                        #endregion
                    }
                    else
                    {

                        attendanceRecord.StatusAB = false;
                        attendanceRecord.StatusP = true;
                        attendanceRecord.ExtraMin = 0;
                        #region -- Margins--
                        //Calculate Late IN, Compare margin with Shift Late In
                        if (attendanceRecord.TimeIn.Value.TimeOfDay > attendanceRecord.DutyTime)
                        {
                            TimeSpan lateMinsSpan = (TimeSpan)(attendanceRecord.TimeIn.Value.TimeOfDay - attendanceRecord.DutyTime);
                            if (lateMinsSpan.Minutes > shift.LateIn)
                            {

                                attendanceRecord.LateIn = (short)lateMinsSpan.TotalMinutes;
                                attendanceRecord.StatusLI = true;
                                attendanceRecord.EarlyIn = 0;
                                attendanceRecord.Remarks = attendanceRecord.Remarks + "[LI]";
                            }
                            else
                            {
                                attendanceRecord.StatusLI = null;
                                attendanceRecord.LateIn = 0;
                                attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LI]", "");
                            }
                        }
                        else
                        {
                            attendanceRecord.StatusLI = null;
                            attendanceRecord.LateIn = 0;
                            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LI]", "");
                        }

                        //Calculate Early In, Compare margin with Shift Early In
                        if (attendanceRecord.TimeIn.Value.TimeOfDay < attendanceRecord.DutyTime)
                        {
                            TimeSpan EarlyInMinsSpan = (TimeSpan)(attendanceRecord.DutyTime - attendanceRecord.TimeIn.Value.TimeOfDay);
                            if (EarlyInMinsSpan.TotalMinutes > shift.EarlyIn)
                            {
                                attendanceRecord.EarlyIn = (short)EarlyInMinsSpan.TotalMinutes;
                                attendanceRecord.StatusEI = true;
                                attendanceRecord.LateIn = 0;
                                attendanceRecord.Remarks = attendanceRecord.Remarks + "[EI]";
                            }
                            else
                            {
                                attendanceRecord.StatusEI = null;
                                attendanceRecord.EarlyIn = 0;
                                attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[EI]", "");
                            }
                        }
                        else
                        {
                            attendanceRecord.StatusEI = null;
                            attendanceRecord.EarlyIn = 0;
                            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[EI]", "");
                        }

                        // CalculateShiftEndTime = ShiftStart + DutyHours
                        DateTime shiftEnd = ATAssistant.CalculateShiftEndTimeWithAttData(attendanceRecord.AttDate.Value, attendanceRecord.DutyTime.Value, (short)(attendanceRecord.ShifMin + attendanceRecord.BreakMin));

                        //Calculate Early Out, Compare margin with Shift Early Out
                        if (attendanceRecord.TimeOut < shiftEnd)
                        {
                            TimeSpan EarlyOutMinsSpan = (TimeSpan)(shiftEnd - attendanceRecord.TimeOut);
                            if (EarlyOutMinsSpan.TotalMinutes > shift.EarlyOut)
                            {
                                attendanceRecord.EarlyOut = (short)EarlyOutMinsSpan.TotalMinutes;
                                attendanceRecord.StatusEO = true;
                                attendanceRecord.LateOut = 0;
                                attendanceRecord.Remarks = attendanceRecord.Remarks + "[EO]";
                            }
                            else
                            {
                                attendanceRecord.StatusEO = null;
                                attendanceRecord.EarlyOut = 0;
                                attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[EO]", "");
                            }
                        }
                        else
                        {
                            attendanceRecord.StatusEO = null;
                            attendanceRecord.EarlyOut = 0;
                            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[EO]", "");
                        }
                        //Calculate Late Out, Compare margin with Shift Late Out
                        if (attendanceRecord.TimeOut > shiftEnd)
                        {
                            TimeSpan LateOutMinsSpan = (TimeSpan)(attendanceRecord.TimeOut - shiftEnd);
                            if (LateOutMinsSpan.TotalMinutes > shift.LateOut)
                            {
                                attendanceRecord.LateOut = (short)LateOutMinsSpan.TotalMinutes;
                                // Late Out cannot have an early out, In case of poll at multiple times before and after shiftend
                                attendanceRecord.EarlyOut = 0;
                                attendanceRecord.StatusLO = true;
                                attendanceRecord.Remarks = attendanceRecord.Remarks + "[LO]";
                            }
                            else
                            {
                                attendanceRecord.StatusLO = null;
                                attendanceRecord.LateOut = 0;
                                attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LO]", "");
                            }
                        }
                        else
                        {
                            attendanceRecord.StatusLO = null;
                            attendanceRecord.LateOut = 0;
                            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LO]", "");
                        }
                        #endregion

                        if (otPolicy.CalculateNOT == true && attendanceRecord.LateOut > 0)
                            attendanceRecord.OTMin = (short)attendanceRecord.LateOut;
                        else if (otPolicy.CalculateNOT == false)
                        {
                            if (attendanceRecord.LateOut > 0)
                                attendanceRecord.ExtraMin = (short)(attendanceRecord.LateOut + attendanceRecord.ExtraMin);
                            if (attendanceRecord.EarlyIn > 0)
                                attendanceRecord.ExtraMin = (short)(attendanceRecord.EarlyIn + attendanceRecord.ExtraMin);
                        }
                        #region -- Shift Things
                        //Subtract EarlyIn and LateOut from Work Minutes
                        if (shift.SubtractEIFromWork == true)
                        {
                            if (attendanceRecord.EarlyIn != null && attendanceRecord.EarlyIn > shift.EarlyIn)
                            {
                                attendanceRecord.WorkMin = (short)(attendanceRecord.WorkMin - attendanceRecord.EarlyIn);
                            }
                        }
                        if (shift.SubtractOTFromWork == true)
                        {
                            if (attendanceRecord.LateOut != null && attendanceRecord.LateOut > shift.LateOut)
                            {
                                attendanceRecord.WorkMin = (short)(attendanceRecord.WorkMin - attendanceRecord.LateOut);
                            }
                        }
                        if (shift.AddEIInOT == true && otPolicy.CalculateNOT == true)
                        {
                            if (attendanceRecord.EarlyIn != null)
                            {
                                if (attendanceRecord.OTMin != null)
                                {
                                    attendanceRecord.OTMin = (short)(attendanceRecord.OTMin + attendanceRecord.EarlyIn);
                                }
                                else
                                {
                                    if (attendanceRecord.EarlyIn != null && attendanceRecord.EarlyIn > shift.OverTimeMin)
                                        attendanceRecord.OTMin = (short)attendanceRecord.EarlyIn;
                                }
                            }
                        }
                        // Deduct break
                        if (attendanceRecord.DutyCode == "D")
                        {
                            //Normal
                            if (attendanceRecord.TimeIn != null && attendanceRecord.TimeOut != null)
                            {
                                if (attendanceRecord.TimeIn < ts && attendanceRecord.TimeOut > te)
                                {
                                    attendanceRecord.WorkMin = (short)(attendanceRecord.WorkMin - attendanceRecord.BreakMin);
                                }
                                else
                                {
                                    if (attendanceRecord.TotalShortMin > 0)
                                        attendanceRecord.TotalShortMin = (short)(attendanceRecord.TotalShortMin + attendanceRecord.BreakMin);
                                }
                            }
                        }
                        //RoundOff Work Minutes
                        //if (shift.RoundOffWorkMin == true)
                        //{
                        //    if (attendanceRecord.LateOut != null || attendanceRecord.EarlyIn != null)
                        //    {
                        //        if (attendanceRecord.WorkMin > (attendanceRecord.ShifMin - shift.LateIn) && (attendanceRecord.WorkMin <= ((attendanceRecord.ShifMin + attendanceRecord.BreakMin) + 2)))
                        //        {
                        //            attendanceRecord.WorkMin = (short)(attendanceRecord.ShifMin);
                        //        }
                        //    }
                        //}
                        #endregion
                        #region -- OT Calculation --
                        if ((attendanceRecord.StatusGZ != true || attendanceRecord.StatusDO != true))
                        {
                            if (attendanceRecord.OTMin != null)
                            {
                                if (otPolicy.CalculateNOT == true)
                                {
                                    float otHour = (float)(attendanceRecord.OTMin / 60.0);
                                    if (otPolicy.PerDayOTLimitHour >= otHour)
                                    {
                                        if (otPolicy.MinMinutesForOneHour == 0)
                                        {
                                            attendanceRecord.OTMin = (short)attendanceRecord.OTMin;
                                        }
                                        else
                                        {
                                            if (attendanceRecord.OTMin < otPolicy.MinMinutesForOneHour)
                                                attendanceRecord.OTMin = 0;
                                            else if (attendanceRecord.OTMin >= otPolicy.MinMinutesForOneHour && attendanceRecord.OTMin <= 61)
                                            {
                                                attendanceRecord.OTMin = 60;
                                            }
                                            else
                                            {
                                                if (attendanceRecord.OTMin != null)
                                                {
                                                    int hour = (int)(attendanceRecord.OTMin / 60);
                                                    int min = hour * 60;
                                                    int remainingmin = (int)attendanceRecord.OTMin - min;
                                                    if (remainingmin >= otPolicy.MinMinutesForOneHour)
                                                    {
                                                        attendanceRecord.OTMin = (short)((hour + 1) * 60);
                                                    }
                                                    else
                                                    {
                                                        attendanceRecord.OTMin = (short)min;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        int policyOTLimitMin = (int)(otPolicy.PerDayOTLimitHour * 60.0);
                                        attendanceRecord.OTMin = (short)policyOTLimitMin;
                                    }
                                }
                                else
                                {
                                    attendanceRecord.WorkMin = (short)mins.TotalMinutes;
                                }
                                if (attendanceRecord.OTMin > 0)
                                {
                                    attendanceRecord.StatusOT = true;
                                    attendanceRecord.Remarks = attendanceRecord.Remarks + "[N-OT]";
                                }
                            }
                        }
                        #endregion
                        #region --- Half Absent and Short Time ---
                        if (attendanceRecord.StatusHL == true)
                        {
                            attendanceRecord.TotalShortMin = 0;
                            attendanceRecord.EarlyOut = 0;
                            attendanceRecord.LateIn = 0;
                            attendanceRecord.LateOut = 0;
                            attendanceRecord.OTMin = 0;
                            attendanceRecord.ExtraMin = 0;
                            attendanceRecord.TotalShortMin = 0;
                            attendanceRecord.StatusLI = false;
                            attendanceRecord.StatusEO = false;
                            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LI]", "");
                            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[EO]", "");
                            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[N-OT]", "");
                            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LO]", "");
                            attendanceRecord.PDays = 0.5;
                            attendanceRecord.AbDays = 0;
                            attendanceRecord.LeaveDays = 0.5;
                            // update if lateout

                        }
                        else
                        {
                            attendanceRecord.PDays = 1;
                            attendanceRecord.AbDays = 0;
                            attendanceRecord.LeaveDays = 0;
                            short totalshortMin = 0;
                            if (attendanceRecord.LateIn > 0)
                                totalshortMin = (short)attendanceRecord.LateIn;
                            if (attendanceRecord.EarlyOut > 0)
                                totalshortMin = (short)(totalshortMin + attendanceRecord.EarlyOut);
                            attendanceRecord.TotalShortMin = totalshortMin;
                            int marginForST = 10;
                            if (shift.LateIn > 0)
                                marginForST = shift.LateIn;
                            if (attendanceRecord.WorkMin < (attendanceRecord.ShifMin - marginForST))
                            {
                                attendanceRecord.TotalShortMin = (Int16)(attendanceRecord.TotalShortMin + (attendanceRecord.ShifMin - (attendanceRecord.WorkMin + totalshortMin)));
                            }
                            if (otPolicy.CalculateNOT == true)
                            {
                                if (attendanceRecord.OTMin > 0)
                                {
                                    if (attendanceRecord.TotalShortMin > 0)
                                        attendanceRecord.ActualOT = (short)(attendanceRecord.OTMin - attendanceRecord.TotalShortMin);
                                }
                            }
                        }

                        #endregion
                        #region -- Mark Absent --
                        //Mark Absent if less than 4 hours
                        if (attendanceRecord.AttDate.Value.DayOfWeek != DayOfWeek.Friday && attendanceRecord.StatusDO != true && attendanceRecord.StatusGZ != true)
                        {
                            if (attendanceRecord.StatusHL != true)
                            {
                                short MinShiftMin = (short)shift.MinHrs;
                                if (attendanceRecord.WorkMin < MinShiftMin)
                                {
                                    attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[Absent]", "");
                                    attendanceRecord.StatusAB = true;
                                    attendanceRecord.StatusP = false;
                                    attendanceRecord.PDays = 0;
                                    attendanceRecord.AbDays = 1;
                                    attendanceRecord.LeaveDays = 0;
                                    attendanceRecord.Remarks = attendanceRecord.Remarks + "[Absent]";
                                }
                                else
                                {
                                    attendanceRecord.StatusAB = false;
                                    attendanceRecord.StatusP = true;
                                    if (attendanceRecord.StatusHL == true)
                                    {
                                        attendanceRecord.PDays = 0.5;
                                        attendanceRecord.AbDays = 0;
                                        attendanceRecord.LeaveDays = 0.5;
                                    }
                                    else
                                    {
                                        attendanceRecord.PDays = 1;
                                        attendanceRecord.AbDays = 0;
                                        attendanceRecord.LeaveDays = 0;
                                    }
                                    attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[Absent]", "");
                                }
                            }
                        }
                        #endregion

                        //RoundOff Work Minutes
                        if (shift.RoundOffWorkMin == true)
                        {
                            if (attendanceRecord.WorkMin >= (attendanceRecord.ShifMin - shift.LateIn) && (attendanceRecord.WorkMin <= ((attendanceRecord.ShifMin) + shift.LateIn)))
                            {
                                attendanceRecord.WorkMin = (short)(attendanceRecord.ShifMin);
                            }

                            if (attendanceRecord.WorkMin > 0 && attendanceRecord.StatusHL != true)
                            {
                                //if (attendanceRecord.ShifMin <= attendanceRecord.WorkMin + attendanceRecord.TotalShortMin)
                                //{
                                //    attendanceRecord.WorkMin = attendanceRecord.ShifMin;
                                //}
                            }
                            if (attendanceRecord.WorkMin > 0 && attendanceRecord.StatusHL == true)
                            {
                                //attendanceRecord.WorkMin = (short)(attendanceRecord.ShifMin);
                            }
                        }
                    }
                    #region -- Break for GZ, Rest and Normal Day
                    //GZ Break
                    if (attendanceRecord.DutyCode == "G")
                    {
                        if (attendanceRecord.TimeIn != null && attendanceRecord.TimeOut != null)
                        {
                            if (attendanceRecord.TimeIn < ts && attendanceRecord.TimeOut > te)
                            {
                                if (attendanceRecord.GZOTMin > 0)
                                {
                                    attendanceRecord.GZOTMin = (short)(attendanceRecord.GZOTMin - attendanceRecord.BreakMin);
                                }
                                if (attendanceRecord.ExtraMin > 0)
                                {
                                    attendanceRecord.ExtraMin = (short)(attendanceRecord.ExtraMin - attendanceRecord.BreakMin);
                                }
                            }
                        }
                    }
                    //Rest
                    else if (attendanceRecord.DutyCode == "R")
                    {
                        if (attendanceRecord.TimeIn != null && attendanceRecord.TimeOut != null)
                        {
                            if (attendanceRecord.TimeIn < ts && attendanceRecord.TimeOut > te)
                            {
                                if (attendanceRecord.OTMin > 0)
                                {
                                    attendanceRecord.OTMin = (short)(attendanceRecord.OTMin - attendanceRecord.BreakMin);
                                }
                                if (attendanceRecord.ExtraMin > 0)
                                {
                                    attendanceRecord.ExtraMin = (short)(attendanceRecord.ExtraMin - attendanceRecord.BreakMin);
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {

            }
        }

        public static void CalculateOpenShiftTimes(DailyAttendance attendanceRecord, MyShift shift, OTPolicy otPolicy)
        {
            try
            {
                //Calculate WorkMin
                if (attendanceRecord != null)
                {
                    //Calculate WorkMin
                    if (attendanceRecord.Remarks != null)
                    {
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[Absent]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LI]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[[Manual]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[N-OT]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LI]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LO]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[EI]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[EO]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[R-OT]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[G-OT]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[HA-HCL]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[HA-HSL]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[HA-HAL]", "");
                        attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[DO]", "");
                    }
                    else
                    {
                        attendanceRecord.Remarks = "";
                    }
                    if (attendanceRecord.TimeOut != null && attendanceRecord.TimeIn != null)
                    {
                        attendanceRecord.Remarks = "";
                        TimeSpan mins = (TimeSpan)(attendanceRecord.TimeOut - attendanceRecord.TimeIn);
                        attendanceRecord.WorkMin = (short)mins.TotalMinutes;
                        double _workHours = mins.TotalHours;
                        //Check if GZ holiday then place all WorkMin in GZOTMin
                        if (attendanceRecord.StatusGZ == true && attendanceRecord.DutyCode == "G")
                        {
                            if (otPolicy.CalculateGZOT == true)
                            {
                                if (otPolicy.PerDayOTLimitHour > _workHours)
                                {

                                    attendanceRecord.GZOTMin = (short)mins.TotalMinutes;
                                }
                                else
                                {
                                    int policyOTLimitMin = (int)(otPolicy.PerDayOTLimitHour * 60.0);
                                    attendanceRecord.GZOTMin = (short)policyOTLimitMin;
                                }
                                attendanceRecord.StatusGZOT = true;
                                attendanceRecord.Remarks = attendanceRecord.Remarks + "[G-OT]";
                            }
                            else
                            {
                                attendanceRecord.WorkMin = (short)mins.TotalMinutes;
                            }
                        }
                        else if (attendanceRecord.StatusDO == true && attendanceRecord.DutyCode == "R")
                        {
                            if (otPolicy.CalculateRestOT == true)
                            {
                                if (otPolicy.PerDayOTLimitHour > _workHours)
                                {
                                    attendanceRecord.OTMin = (short)mins.TotalMinutes;
                                }
                                else
                                {
                                    int policyOTLimitMin = (int)(otPolicy.PerDayOTLimitHour * 60.0);
                                    attendanceRecord.OTMin = (short)policyOTLimitMin;
                                }
                                attendanceRecord.StatusOT = true;
                                attendanceRecord.Remarks = attendanceRecord.Remarks + "[R-OT]";
                            }
                            else
                            {
                                attendanceRecord.WorkMin = (short)mins.TotalMinutes;
                            }
                        }
                        else
                        {
                            attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[Absent]", "");
                            attendanceRecord.StatusAB = false;
                            attendanceRecord.StatusP = true;
                            if (mins.TotalMinutes < ((attendanceRecord.ShifMin + attendanceRecord.BreakMin) - shift.EarlyOut))
                            {
                                Int16 EarlyoutMin = (Int16)((attendanceRecord.ShifMin + attendanceRecord.BreakMin) - Convert.ToInt16(mins.TotalMinutes));
                                if (EarlyoutMin > shift.EarlyOut)
                                {
                                    attendanceRecord.EarlyOut = EarlyoutMin;
                                    attendanceRecord.StatusEO = true;
                                    attendanceRecord.Remarks = attendanceRecord.Remarks + "[EO]";
                                }
                                else
                                {
                                    attendanceRecord.StatusEO = null;
                                    attendanceRecord.EarlyOut = 0;
                                    attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[EO]", "");
                                }
                            }
                            else
                            {
                                attendanceRecord.StatusEO = null;
                                attendanceRecord.EarlyOut = 0;
                                attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[EO]", "");
                            }
                            if (attendanceRecord.WorkMin > (attendanceRecord.ShifMin + attendanceRecord.BreakMin))
                            {
                                short LateOutMins = (short)(attendanceRecord.WorkMin - (attendanceRecord.ShifMin + attendanceRecord.BreakMin));
                                if (LateOutMins > shift.LateOut)
                                {
                                    attendanceRecord.LateOut = (short)LateOutMins;
                                    // Late Out cannot have an early out, In case of poll at multiple times before and after shiftend
                                    attendanceRecord.EarlyOut = 0;
                                    attendanceRecord.StatusLO = true;
                                    attendanceRecord.Remarks = attendanceRecord.Remarks + "[LO]";
                                }
                                else
                                {
                                    attendanceRecord.StatusLO = null;
                                    attendanceRecord.LateOut = 0;
                                    attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LO]", "");
                                }
                            }
                            else
                            {
                                attendanceRecord.StatusLO = null;
                                attendanceRecord.LateOut = 0;
                                attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[LO]", "");
                            }
                            if ((attendanceRecord.StatusGZ != true || attendanceRecord.StatusDO != true))
                            {
                                if (attendanceRecord.LateOut != null)
                                {
                                    if (otPolicy.CalculateNOT == true)
                                    {
                                        float otHour = (float)(attendanceRecord.LateOut / 60.0);
                                        if (otPolicy.PerDayOTLimitHour > otHour)
                                        {
                                            attendanceRecord.OTMin = (short)attendanceRecord.LateOut;
                                        }
                                        else
                                        {
                                            int policyOTLimitMin = (int)(otPolicy.PerDayOTLimitHour * 60.0);
                                            attendanceRecord.OTMin = (short)policyOTLimitMin;
                                        }
                                        attendanceRecord.StatusOT = true;
                                        attendanceRecord.Remarks = attendanceRecord.Remarks + "[N-OT]";
                                    }
                                    else
                                    {
                                        attendanceRecord.WorkMin = (short)mins.TotalMinutes;
                                    }
                                    attendanceRecord.StatusOT = true;
                                    attendanceRecord.Remarks = attendanceRecord.Remarks + "[N-OT]";
                                }
                            }
                            //Subtract EarlyIn and LateOut from Work Minutes
                            if (shift.SubtractEIFromWork == true)
                            {
                                if (attendanceRecord.EarlyIn != null && attendanceRecord.EarlyIn > shift.EarlyIn)
                                {
                                    attendanceRecord.WorkMin = (short)(attendanceRecord.WorkMin - attendanceRecord.EarlyIn);
                                }
                            }
                            if (shift.SubtractOTFromWork == true)
                            {
                                if (attendanceRecord.OTMin != null && attendanceRecord.OTMin > shift.OverTimeMin)
                                {
                                    attendanceRecord.WorkMin = (short)(attendanceRecord.WorkMin - attendanceRecord.OTMin);
                                }
                            }
                            if (shift.AddEIInOT == true)
                            {
                                if (attendanceRecord.EarlyIn != null)
                                {
                                    if (attendanceRecord.OTMin != null)
                                    {
                                        attendanceRecord.OTMin = (short)(attendanceRecord.OTMin + attendanceRecord.EarlyIn);
                                    }
                                    else
                                    {
                                        if (attendanceRecord.EarlyIn != null && attendanceRecord.EarlyIn > shift.OverTimeMin)
                                            attendanceRecord.OTMin = (short)attendanceRecord.EarlyIn;
                                    }
                                }
                            }
                            if (shift.RoundOffWorkMin == true)
                            {
                                if (attendanceRecord.LateOut != null || attendanceRecord.EarlyIn != null)
                                {
                                    if (attendanceRecord.WorkMin > (attendanceRecord.ShifMin + attendanceRecord.BreakMin) && (attendanceRecord.WorkMin <= (attendanceRecord.ShifMin + attendanceRecord.BreakMin + shift.OverTimeMin)))
                                    {
                                        attendanceRecord.WorkMin = attendanceRecord.ShifMin;
                                    }
                                }
                            }
                            //Mark Absent if less than 4 hours
                            if (attendanceRecord.AttDate.Value.DayOfWeek != DayOfWeek.Friday && attendanceRecord.StatusDO != true && attendanceRecord.StatusGZ != true)
                            {
                                short MinShiftMin = (short)shift.MinHrs;
                                if (attendanceRecord.WorkMin < MinShiftMin)
                                {
                                    attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[Absent]", "");
                                    attendanceRecord.StatusAB = true;
                                    attendanceRecord.StatusP = false;
                                    attendanceRecord.Remarks = attendanceRecord.Remarks + "[Absent]";
                                }
                                else
                                {
                                    attendanceRecord.StatusAB = false;
                                    attendanceRecord.StatusP = true;
                                    attendanceRecord.Remarks = attendanceRecord.Remarks.Replace("[Absent]", "");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        #endregion
    }
}
