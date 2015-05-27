using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KratosApp.Models
{
    public class Kratos
    {
        public int Power { get; set; }
        public double Speed { get; set; }


    }

    public static class Speed
    {
        static int iLang = 0;
        static bool engl = true;

        public static void Start()
        {
            double prefCad = 90, cCad = .002;

            string bike = "racetops";
            double fstg = 7;               // grade;
            double fCadc = 90;              // cadence
            double fP = 500;                // power
            double fV = 9.3;                // speed
            double fW = 1;                  // wind

            //vr = Front Wheel
            //hr = Rear Wheel


            var asBike = new List<String>() { "roadster", "mtb", "tandem", "racetops", "racedrops", "tria", "superman", "lwbuss", "swbuss", "swbass", "ko4", "ko4tailbox", "whitehawk", "questclosed", "handtrike" };
            var afCd = new List<double>() { .95, .79, .35, .82, .60, .53, .47, .85, .67, .60, .50, .41, 0, 0, .62 };
            var afSin = new List<double>() { .95, .85, .7, .89, .67, .64, .55, .64, .51, .44, .37, .37, 0, 0, .55 };
            var afCdBike = new List<double>() { 2.0, 1.5, 1.7, 1.5, 1.5, 1.25, .90, 1.7, 1.6, 1.25, 1.2, 1.15, .036, .090, 1.5 };
            var afAFrame = new List<double>() { .06, .052, .06, .048, .048, .048, .044, .039, .036, .031, .023, .026, 1, 1, .046 };
            var afCATireV = new List<double>() { 1.1, 1.1, 1.1, 1.1, 1.1, 1.1, .9, .66, .8, .85, .77, .77, .1, .26, .9 };
            var afCATireH = new List<double>() { .9, .9, .9, .9, .9, .7, .7, .9, .80, .84, .49, .3, .13, .16, 2 };
            var afLoadV = new List<double>() { .33, .45, .5, .40, .45, .47, .48, .32, .55, .55, .63, .63, .55, .72, .5 };
            var afCCrV = new List<double>() { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.25, 1.25, 1.25, 1.25, 1.25, 1.25, 1.5, 1.5 };
            var afCm = new List<double>() { 1.03, 1.025, 1.05, 1.025, 1.025, 1.025, 1.025, 1.04, 1.04, 1.04, 1.05, 1.05, 1.06, 1.09, 1.03 };
            var afMBikeDef = new List<double>() { 18, 12, 17.8, 9.5, 9.5, 9.5, 8, 18, 15.5, 11.5, 11.8, 13.5, 18, 32, 18 };
            var aiTireFDef = new List<int>() { 3, 5, 1, 0, 0, 0, 0, 1, 2, 0, 0, 0, 0, 0, 0 };
            var aiTireRDef = new List<int>() { 3, 5, 1, 0, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0, 0 };


            double fBh, fh;
            double fBm, fM;
            double fT;
            double fHn;

            fh = fBh = (iLang == 1) ? 67.7 : 172;
            fM = fBm = (iLang == 1) ? 157.2 : 71.3;
            fT = (iLang == 1) ? 68 : 20;                    // Air Temperature;
            fHn = (iLang == 1) ? 1150 : 350;                // Height above SeaLevel;


            // change bike
            int i = 3; // racetops
            var fmr = afMBikeDef[i] * ((iLang == 1) ? 2.2 : 1);
            int fVRSelectedIndex = aiTireFDef[i];
            int fHRSelectedIndex = aiTireRDef[i];

            // rest

            var bikeI = 3;                                  // hands on top. TODO, change to bike type
            //var bikeR = 0;

            var hRider = fh * (engl ? ((iLang == 1) ? .0254 : .01) : 1) * ((bike == "tandem") ? 2 : 1);
            var M = fM * ((iLang == 1) ? .453592 : 1) * ((bike == "tandem") ? 2 : 1);
            var MBik = fmr * ((iLang == 1) ? .453592 : 1);
            var T = (iLang == 1) ? 5 * (fT - 32) / 9 : fT;
            var Hn = fHn * ((iLang == 1) ? .3048 : 1);
            var Slope = Math.Atan(fstg * .01);
            var W = fW * ((iLang == 1) ? .44704 : .27778);
            var P = fP * ((bike == "tandem") ? 2 : 1);
            var V = fV * ((iLang == 1) ? .44704 : .27778);
            var cad = fCadc;
            double diagrCad;

            if (!engl) hRider /= ((hRider < 10) ? 1 : ((10 <= hRider && hRider < 400) ? 100 : 1000));
            fCadc = diagrCad = cad = Math.Abs(cad);
            if (cad > 10) prefCad = cad;

            var Cr = new List<double>() { .0033, .0031, .0029, .0050, .0016, .0046 };
            var ATire = new List<double>() { .021, .031, .042, .048, .042, .055 };
            var CrDyn = 0.1 * Math.Cos(Slope);
            var j = fVRSelectedIndex;
            double CrV = Cr[j], ATireV = ATire[j];
            j = fHRSelectedIndex;
            double CrH = Cr[j], ATireH = ATire[j];
            var CrEff = afLoadV[bikeI] * afCCrV[bikeI] * CrV + (1.0 - afLoadV[bikeI]) * CrH;

            var adipos = (bike == "whitehawk" || bike == "questclosed") ? 0 : Math.Sqrt(M / (hRider * 750));
            var CwaBike = afCdBike[bikeI] * (afCATireV[bikeI] * ATireV + afCATireH[bikeI] * ATireH + afAFrame[bikeI]);
            double CwaRider, Ka, cardB;
            var Frg = 9.81 * (MBik + M) * (CrEff * Math.Cos(Slope) + Math.Sin(Slope));

            var windtxt = "Tail-wind speed is greater than bicycle-speed.\nTherefore the air drag could not be evaluated correctly.\nTry it again with a less negative wind speed.";
            var P0txt = "Rider\"s Power <= 0";
            var P1txt = "Rider\"s Power > 0";
            var Ptxt = " Watt.\nThe program has presumed to reset the Cadence to ";


            // Calculate based on Power
            if (fP > 0)
            {
                if (P > 0 && cad == 0)
                {
                    cad = prefCad;
                    WRITE_OUTPUT(fCadc, 0, cad);
                    alert(P1txt + Ptxt + cad + ".");
                }
                if (P <= 0 && cad > 0)
                {
                    WRITE_OUTPUT(fCadc, 0, 0);
                    cad = 0;
                    alert(P0txt + Ptxt + "0.");
                }
                CwaRider = (1 + cad * cCad) * afCd[bikeI] * adipos * (((hRider - adipos) * afSin[bikeI]) + adipos);
                Ka = 176.5 * Math.Exp(-Hn * .0001253) * (CwaRider + CwaBike) / (273 + T);
                cardB = (3 * Frg - 4 * W * CrDyn) / (9 * Ka) - Math.Pow(CrDyn, 2) / (9 * Math.Pow(Ka, 2)) - (W * W) / 9;
                double cardA = -((Math.Pow(CrDyn, 3) - Math.Pow(W, 3)) / 27
                            + W * (5 * W * CrDyn + 4 * Math.Pow(CrDyn, 2) / Ka - 6 * Frg) / (18 * Ka)
                            - P / (2 * Ka * afCm[bikeI])
                            - CrDyn * Frg / (6 * Math.Pow(Ka, 2))),
                sqrt = Math.Pow(cardA, 2) + Math.Pow(cardB, 3),
                    //ire = cardA - Math.Sqrt(sqrt), Vms;
                ire = cardA - Math.Sqrt(sqrt); //, Vms;
                double Vms;
                if (sqrt >= 0)
                    Vms = Math.Pow(cardA + Math.Sqrt(sqrt), 1 / (double)3) + ((ire < 0) ? -Math.Pow(-ire, 1 / (double)3) : Math.Pow(ire, 1 / (double)3));
                else
                    Vms = 2 * Math.Sqrt(-cardB) * Math.Cos(Math.Acos(cardA / Math.Sqrt(Math.Pow(-cardB, 3))) / (double)3);
                Vms -= 2 * W / (double)3 + CrDyn / (double)(3 * Ka);

                WRITE_OUTPUT(fV, 1, ((iLang == 1) ? 2.2369 : 3.6) * Vms);
                double fVoutput = ((iLang == 1) ? 2.2369 : 3.6) * Vms;

                if (Vms < -W)
                {
                    alert("Probably wrong speed result.\n" + windtxt);
                    return;
                }
            }


            // Caclculate based on Speed
            if (fV > 0)
            {
                double y = 7979;
                while (y == 7979 || Math.Round((double)y) <= 0 && cad > 0 || Math.Round((double)y) > 0 && cad == 0)
                {
                    if (y != 7979 && Math.Round((double)y) <= 0 && cad > 0)
                    {
                        cad = 0;
                        WRITE_OUTPUT(fCadc, 0, cad);
                        alert(P0txt + Ptxt + "0.");
                    }
                    if (y != 7979 && Math.Round((double)y) > 0 && cad == 0)
                    {
                        cad = prefCad;
                        WRITE_OUTPUT(fCadc, 0, cad);
                        alert(P1txt + Ptxt + cad + ".");
                    }
                    var vw = V + W;
                    CwaRider = (1 + cad * cCad) * afCd[bikeI] * adipos * (((hRider - adipos) * afSin[bikeI]) + adipos);
                    Ka = 176.5 * Math.Exp(-Hn * .0001253) * (CwaRider + CwaBike) / (273 + T);
                    y = afCm[bikeI] * V * (Ka * (vw * ((vw < 0) ? -vw : vw)) + Frg + V * CrDyn);
                    if (bike == "tandem")
                        y *= 0.5;

                    var fPoutput = y;
                }
            }
        }

        public static void alert(string s)
        {
            //Debug.WriteLine("ALERT: " + s);
        }

        public static void WRITE_OUTPUT(double d, int i, double d2)
        {
            //Debug.WriteLine(String.Format("{0} {1} {2}", d, i, d2));
        }
    }

}