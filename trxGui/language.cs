using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trxGui
{
    static class language
    {
        public static String[] en = new String[]
        {
            "Pluto Address:",
            "local USB",
            "enter IP address of the Pluto",
            "Loadspeaker/Headphone:",
            "Microphone:",
            "Receiver Frequency:",
            "Transmitter Frequency:",
            "Spectrum/Waterfall Speed:",
            "Cancel",
            "MHz ( enter center frequency corresponding to 10489.750 MHz )",
            "MHz ( enter center frequency corresponding to 2400,250 MHz )",
            "Hz (correction value for the Pluto TCXO, or use CAL calibration function)",
            "Hz (correction value for the LNB, or use CAL calibration function)",
            "normal CPU",
            "to reduce CPU load on slower computers",
            "Help",
            "Language:",
            "Open Manual",
            "Mouse:",
            "Color Palette:",
        };

        public static String[] de = new String[]
        {
            "Pluto Adresse:",
            "USB lokal",
            "gebe IP Adresse des Plutos ein",
            "Lautsprecher/Kopfhörer:",
            "Mikrofon:",
            "Empfangsfrequenz:",
            "Sendefrequenz:",
            "Spekt./Wasserf. Auflösung:",
            "Abbrechen",
            "MHz ( Mittenfrequenz bezogen auf 10489,750 MHz )",
            "MHz ( Mittenfrequenz bezogen auf 2400,250 MHz )",
            "Hz (Korrekturwert für den Pluto TCXO, oder benutze die CAL Funktion)",
            "Hz (Korrekturwert für den LNB, oder benutze die CAL Funktion)",
            "normale CPU",
            "Reduziere die CPU Last auf langsamen Rechnern",
            "Hilfe",
            "Sprache:",
            "Bedienungsanleitung",
            "Maus:",
            "Farbpalette:",
        };

        public static String GetText(String s)
        {
            for(int i=0; i<en.Length; i++)
            {
                if(s == en[i])
                {
                    if (statics.language == 1) return de[i];
                }
            }

            for (int i = 0; i < de.Length; i++)
            {
                if (s == de[i])
                {
                    if (statics.language == 0) return en[i];
                }
            }

            return s;
        }
    }
}
