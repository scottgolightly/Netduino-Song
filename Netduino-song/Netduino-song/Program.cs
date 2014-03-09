using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace Netduino_song
{
    public class Program
    {
        public static void Main()
        {
            PWM speaker = new PWM(PWMChannels.PWM_PIN_D5, 2000, 0.95, true);
            
            // Store the notes on the music scale and their associated pulse lengths
            System.Collections.Hashtable scale = new System.Collections.Hashtable();

            // low octave
            scale.Add("c", 1915u);
            scale.Add("d", 1700u);
            scale.Add("e", 1519u);
            scale.Add("f", 1432u);
            scale.Add("g", 1275u);
            scale.Add("a", 1136u);
            scale.Add("b", 1014u);

            // upper octave
            scale.Add("C", 956u);
            scale.Add("D", 851u);
            scale.Add("E", 758u);
            scale.Add("F", 671u);
            scale.Add("G", 594u);

            // hold note
            scale.Add("H", 0u);

            int beatsPerMinute = 90;
            int beatTimeInMilliseconds = 60000 / beatsPerMinute;
            int pauseTimeInMilliseconds = (int)(beatTimeInMilliseconds * 0.1);

            // Define the song (letter of note followed by length of note)
            string song = "C1C1C1g1a1a1g2E1E1D1D1C2";

            // interpret and play the song
            for (int i = 0; i < song.Length; i += 2)
            {
                //Extract each note and the length in beats
                string note = song.Substring(i, 1);
                int beatCount = int.Parse(song.Substring(i + 1, 1));

                // look up the note duration in milliseconds
                uint noteFrequency = (uint)scale[note];
                int duration = beatCount * beatTimeInMilliseconds;

                // play the note for the desired duration
                speaker.Frequency = noteFrequency;
                Debug.Print(beatCount + "\t" + beatTimeInMilliseconds + "\t" + pauseTimeInMilliseconds + "\t" + duration + "\t"  + speaker.Duration + "\t" + speaker.DutyCycle + "\t" + speaker.Frequency + "\t" + speaker.Period);
                speaker.Start();
                Thread.Sleep(duration);
                speaker.Stop();

                // Pause for 1/10th of one beat
                Thread.Sleep(pauseTimeInMilliseconds);
            }
            Thread.Sleep(Timeout.Infinite);

        }

    }
}
