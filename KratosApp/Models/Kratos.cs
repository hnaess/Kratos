using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KratosApp.Models
{
    public class Kratos
    {
        public double PowerOutput { get; private set; }
        public double SpeedOutput { get; private set; }

        public double cad { get; set; }

        public string bike { private get; set; }
        /// <summary>Slope of Road</summary>
        public double fstg { get; set; }
        public double fP { get; set; }
        public double fV { get; set; }
        /// <summary>Wind Speed (headwind positive, tailwind negative values)</summary>
        public double fW { get; set; }

        /// <summary>Height of rider</summary>
        public double fh { get; set; }
        /// <summary>Weight of rider</summary>
        public double fM { get; set; }
        /// <summary>Weight of bike</summary>
        public double fmr { get; set; }
        /// <summary>Air Temperature (not Kelvin)</summary>
        public double fT { get; set; }
        /// <summary>Height above SeaLevel</summary>
        public double fHn { get; set; }

        /// <summary>Effective Drag Area Cd*A</summary>
        public double fCwA { get; private set; }
        /// <summary>Rolling Resistance Coeff. Cr</summary>
        public double CrEff { get; private set; }

        public string Ext { get; set; }



        bool isTandem { get { return bike == "tandem"; } }

        static bool imperial = false;

        private int bikeI { get { return asBike.IndexOf(bike); } }


        static List<String> asBike = new List<String>() { "roadster", "mtb", "tandem", "racetops", "racedrops", "tria", "superman", "lwbuss", "swbuss", "swbass", "ko4", "ko4tailbox", "whitehawk", "questclosed", "handtrike" };
        static List<double> afCd = new List<double>() { .95, .79, .35, .82, .60, .53, .47, .85, .67, .60, .50, .41, 0, 0, .62 };
        static List<double> afSin = new List<double>() { .95, .85, .7, .89, .67, .64, .55, .64, .51, .44, .37, .37, 0, 0, .55 };
        static List<double> afCdBike = new List<double>() { 2.0, 1.5, 1.7, 1.5, 1.5, 1.25, .90, 1.7, 1.6, 1.25, 1.2, 1.15, .036, .090, 1.5 };
        static List<double> afAFrame = new List<double>() { .06, .052, .06, .048, .048, .048, .044, .039, .036, .031, .023, .026, 1, 1, .046 };
        static List<double> afCATireV = new List<double>() { 1.1, 1.1, 1.1, 1.1, 1.1, 1.1, .9, .66, .8, .85, .77, .77, .1, .26, .9 };
        static List<double> afCATireH = new List<double>() { .9, .9, .9, .9, .9, .7, .7, .9, .80, .84, .49, .3, .13, .16, 2 };
        static List<double> afLoadV = new List<double>() { .33, .45, .5, .40, .45, .47, .48, .32, .55, .55, .63, .63, .55, .72, .5 };
        static List<double> afCCrV = new List<double>() { 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.25, 1.25, 1.25, 1.25, 1.25, 1.25, 1.5, 1.5 };
        static List<double> afCm = new List<double>() { 1.03, 1.025, 1.05, 1.025, 1.025, 1.025, 1.025, 1.04, 1.04, 1.04, 1.05, 1.05, 1.06, 1.09, 1.03 };
        static List<double> afMBikeDef = new List<double>() { 18, 12, 17.8, 9.5, 9.5, 9.5, 8, 18, 15.5, 11.5, 11.8, 13.5, 18, 32, 18 };
        static List<int> aiTireFDef = new List<int>() { 3, 5, 1, 0, 0, 0, 0, 1, 2, 0, 0, 0, 0, 0, 0 };
        static List<int> aiTireRDef = new List<int>() { 3, 5, 1, 0, 0, 0, 0, 3, 3, 0, 0, 0, 0, 0, 0 };

        static List<double> Cr = new List<double>() { .0033, .0031, .0029, .0050, .0016, .0046 };
        static List<double> ATire = new List<double>() { .021, .031, .042, .048, .042, .055 };

        public enum CalcMethod { Power, Speed };


        public Kratos ShallowCopy()
        {
            return (Kratos)this.MemberwiseClone();
        }

        public void Calc()
        {
            if (fP > 0)
                Calc(CalcMethod.Power);
            else if (fV > 0)
                Calc(CalcMethod.Speed);
            else
                throw new Exception("Power or Speed has to be bigger than zero");
        }

        public void Calc(CalcMethod calcMethod)
        {
            const double cCad = .002;

            int fVRSelectedIndex = 0; // 0 = narrow racing tire (high pressurE)
            int fHRSelectedIndex = 0;

            //var fmr = afMBikeDef[bikeI] * (imperial ? 2.2 : 1);

            var hRider = fh * (imperial ? .0254 : .01) * (isTandem ? 2 : 1);
            var M = fM * (imperial ? .453592 : 1) * (isTandem ? 2 : 1);
            var MBik = fmr * (imperial ? .453592 : 1);
            var T = imperial ? 5 * (fT - 32) / 9 : fT;
            var Hn = fHn * (imperial ? .3048 : 1);
            var Slope = Math.Atan(fstg * .01);
            var W = fW * (imperial ? .44704 : .27778);
            var P = fP * (isTandem ? 2 : 1);
            var V = fV * (imperial ? .44704 : .27778);

            var CrDyn = 0.1 * Math.Cos(Slope);
            double CrV = Cr[fVRSelectedIndex], ATireV = ATire[fVRSelectedIndex];
            double CrH = Cr[fHRSelectedIndex], ATireH = ATire[fHRSelectedIndex];
            CrEff = afLoadV[bikeI] * afCCrV[bikeI] * CrV + (1.0 - afLoadV[bikeI]) * CrH;

            var adipos = (bike == "whitehawk" || bike == "questclosed") ? 0 : Math.Sqrt(M / (hRider * 750));
            var CwaBike = afCdBike[bikeI] * (afCATireV[bikeI] * ATireV + afCATireH[bikeI] * ATireH + afAFrame[bikeI]);
            double CwaRider = 0;
            var Frg = 9.81 * (MBik + M) * (CrEff * Math.Cos(Slope) + Math.Sin(Slope));

            CwaRider = (1 + cad * cCad) * afCd[bikeI] * adipos * (((hRider - adipos) * afSin[bikeI]) + adipos);
            double Ka = 176.5 * Math.Exp(-Hn * .0001253) * (CwaRider + CwaBike) / (273 + T);

            //double outputSpeed = 0;
            if (calcMethod == CalcMethod.Power)
            {
                double cardB = (3 * Frg - 4 * W * CrDyn) / (9 * Ka) - Math.Pow(CrDyn, 2) / (9 * Math.Pow(Ka, 2)) - (W * W) / 9;
                double cardA = -((Math.Pow(CrDyn, 3) - Math.Pow(W, 3)) / 27
                            + W * (5 * W * CrDyn + 4 * Math.Pow(CrDyn, 2) / Ka - 6 * Frg) / (18 * Ka)
                            - P / (2 * Ka * afCm[bikeI])
                            - CrDyn * Frg / (6 * Math.Pow(Ka, 2))),
                sqrt = Math.Pow(cardA, 2) + Math.Pow(cardB, 3),
                ire = cardA - Math.Sqrt(sqrt); //, Vms;
                double Vms;
                if (sqrt >= 0)
                    Vms = Math.Pow(cardA + Math.Sqrt(sqrt), 1 / (double)3) + ((ire < 0) ? -Math.Pow(-ire, 1 / (double)3) : Math.Pow(ire, 1 / (double)3));
                else
                    Vms = 2 * Math.Sqrt(-cardB) * Math.Cos(Math.Acos(cardA / Math.Sqrt(Math.Pow(-cardB, 3))) / (double)3);
                Vms -= 2 * W / (double)3 + CrDyn / (double)(3 * Ka);

                SpeedOutput = (imperial ? 2.2369 : 3.6) * Vms;

                if (Vms < -W)
                {
                    throw new Exception("Probably wrong speed result.\n"
                        + "Tail-wind speed is greater than bicycle-speed.\nTherefore the air drag could not be evaluated correctly.\nTry it again with a less negative wind speed.");
                }
            }


            // Caclculate based on Speed
            if (calcMethod == CalcMethod.Speed)
            {
                var vw = V + W;
                PowerOutput = afCm[bikeI] * V * (Ka * (vw * ((vw < 0) ? -vw : vw)) + Frg + V * CrDyn);
                if (isTandem)
                {
                    PowerOutput *= 0.5;
                }
            }

            fCwA = (CwaBike + CwaRider) * (imperial ? 10.764 : 1);
        }
    }
}