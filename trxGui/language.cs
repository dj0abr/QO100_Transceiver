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
            "Pluto TX power [dBm], allowed values: -40 to 0",
            "TX power:",
            "Screen-Size:",
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
            "Pluto TX Ausgangsleistung [dBm], erlaubte Werte: -40 bis 0",
            "TX Leistung:",
            "Bildgröße:",
        };

        public static String[] fr = new String[]
        {
            "Adresse Pluto :",
            "USB local",
            "entrer l'adresse IP du Pluto",
            "Loadspeaker/Headphone :",
            "Microphone :",
            "Fréquence du récepteur :",
            "Fréquence de l'émetteur :",
            "Spectre/Vitesse de chute d'eau :",
            "Annuler",
            "MHz (entrer la fréquence centrale correspondant à 10489,750 MHz)",
            "MHz (entrer la fréquence centrale correspondant à 2400,250 MHz)",
            "Hz (valeur de correction pour le TCXO de Pluto, ou utiliser la fonction de calibration CAL)",
            "Hz (valeur de correction pour le LNB, ou utiliser la fonction de calibrage CAL)",
            "CPU normal",
            "pour réduire la charge du CPU sur les ordinateurs plus lents",
            "Aide",
            "Langue :",
            "Ouvrir le manuel",
            "Souris :",
            "Palette de couleurs :",
            "Puissance TX Pluto [dBm], valeurs autorisées : -40 à 0",
            "Puissance TX :",
            "Taille de l'écran :",
        };

        public static String[] es = new String[]
        {
            "Dirección de Pluto:",
            "USB local",
            "introduzca la dirección IP del Pluto",
            "Altavoz/Auricular:",
            "Micrófono:",
            "Frecuencia del receptor:",
            "Frecuencia del transmisor:",
            "Espectro/Velocidad de caída:",
            "Cancelar",
            "MHz ( introduzca la frecuencia central correspondiente a 10489,750 MHz )",
            "MHz ( introduzca la frecuencia central correspondiente a 2400,250 MHz )",
            "Hz (valor de corrección para el TCXO de Plutón, o utilice la función de calibración CAL)",
            "Hz (valor de corrección para el LNB, o utilice la función de calibración CAL)",
            "CPU normal",
            "para reducir la carga de la CPU en ordenadores más lentos",
            "Ayuda",
            "Idioma:",
            "Abrir manual",
            "Ratón:",
            "Paleta de colores:",
            "Potencia de transmisión de Plutón [dBm], valores permitidos: -40 a 0",
            "Potencia TX:",
            "Tamaño de pantalla:",
        };

        public static String[] pt = new String[]
        {
            "Endereço de Plutão:",
            "USB local",
            "introduzir o endereço IP do Plutão",
            "Loadspeaker/Headphone:",
            "Microfone:",
            "Receptor Frequência:",
            "Frequência do Transmissor:",
            "Spectrum/Waterfall Speed:",
            "Cancelar",
            "MHz ( introduzir a frequência central correspondente a 10489.750 MHz )",
            "MHz ( introduzir a frequência central correspondente a 2400,250 MHz )",
            "Hz (valor de correcção para o Pluto TCXO, ou utilizar a função de calibração CAL)",
            "Hz (valor de correcção para o LNB, ou utilizar a função de calibração CAL)",
            "CPU normal",
            "para reduzir a carga da CPU em computadores mais lentos",
            "Ajuda",
            "Língua:",
            "Manual Aberto",
            "Rato:",
            "Paleta de cores:",
            "Pluto TX potência [dBm], valores permitidos: -40 a 0",
            "TX power:",
            "Tamanho de ecrã:",
        };

        public static String[] it = new String[]
        {
            "Indirizzo Pluto:",
            "USB locale",
            "inserire l'indirizzo IP del Pluto",
            "Altoparlante/Cuffia:",
            "Microfono:",
            "Frequenza del ricevitore:",
            "Frequenza del trasmettitore:",
            "Spettro/Velocità della cascata:",
            "Annulla",
            "MHz ( inserire frequenza centrale corrispondente a 10489,750 MHz )",
            "MHz ( inserire frequenza centrale corrispondente a 2400,250 MHz )",
            "Hz (valore di correzione per il TCXO Pluto, o usare la funzione di calibrazione CAL)",
            "Hz (valore di correzione per l'LNB, o usare la funzione di calibrazione CAL)",
            "CPU normale",
            "per ridurre il carico della CPU sui computer più lenti",
            "Aiuto",
            "Lingua:",
            "Aprire il manuale",
            "Mouse:",
            "Tavolozza dei colori:",
            "Potenza TX Pluto [dBm], valori ammessi: -40 a 0",
            "Potenza TX:",
            "Dimensione dello schermo:",
        };



        public static String GetText(String s)
        {
            for(int i=0; i<en.Length; i++)
            {
                if(s == en[i] || s==de[i] || s==fr[i] || s==es[i] || s==pt[i] || s==it[i])
                {
                    if (statics.language == 0) return en[i];
                    if (statics.language == 1) return de[i];
                    if (statics.language == 2) return fr[i];
                    if (statics.language == 3) return es[i];
                    if (statics.language == 4) return pt[i];
                    if (statics.language == 5) return it[i];
                }
            }

            return s;
        }
    }
}
