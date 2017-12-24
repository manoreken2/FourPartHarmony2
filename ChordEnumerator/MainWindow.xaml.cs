using FourPartHarmony2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChordEnumerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        class KeyAndName {
            public MusicKey k;
            public string name;

            public KeyAndName(MusicKey aK, string aName) {
                k = aK;
                name = aName;
            }
        };

        private void GenerateChordMidi(ChordType ct, string desc) {
            var keys = new KeyAndName[] {
                new KeyAndName(MusicKey.Cdur, "CMajor" ),
                new KeyAndName(MusicKey.Ddur, "DMajor" ),
                new KeyAndName(MusicKey.Edur, "EMajor" ),
                new KeyAndName(MusicKey.Fdur, "FMajor" ),
                new KeyAndName(MusicKey.Gdur, "GMajor" ),
                new KeyAndName(MusicKey.Adur, "AMajor" ),
                new KeyAndName(MusicKey.Hdur, "BMajor" ),

                new KeyAndName(MusicKey.Cmoll, "CMinor" ),
                new KeyAndName(MusicKey.Dmoll, "DMinor" ),
                new KeyAndName(MusicKey.Emoll, "EMinor" ),
                new KeyAndName(MusicKey.Fmoll, "FMinor" ),
                new KeyAndName(MusicKey.Gmoll, "GMinor" ),
                new KeyAndName(MusicKey.Amoll, "AMinor" ),
                new KeyAndName(MusicKey.Hmoll, "BMinor" ),

                new KeyAndName(MusicKey.CISdur, "CSharpMajor" ),
                new KeyAndName(MusicKey.DESdur, "DFlatMajor" ),
                new KeyAndName(MusicKey.ESdur, "EFlatMajor" ),
                new KeyAndName(MusicKey.FISdur, "FSharpMajor" ),
                new KeyAndName(MusicKey.GESdur, "GFlatMajor" ),
                new KeyAndName(MusicKey.ASdur, "AFlatMajor" ),
                new KeyAndName(MusicKey.Bdur, "BFlatMajor" ),

                new KeyAndName(MusicKey.CISmoll, "CSharpMinor" ),
                new KeyAndName(MusicKey.DISmoll, "DSharpMinor" ),
                new KeyAndName(MusicKey.ESmoll, "EFlatMinor" ),
                new KeyAndName(MusicKey.FISmoll, "FSharpMinor" ),
                new KeyAndName(MusicKey.GISmoll, "GSharpMinor" ),
                new KeyAndName(MusicKey.ASmoll, "AFlatMinor" ),
                new KeyAndName(MusicKey.Bmoll, "BFlatMinor" ),
            };

            foreach (var k in keys) {
                ct.musicKey = k.k;
                using (var bw = new BinaryWriter(File.Open(string.Format("out/{0}{1}.mid", k.name, desc), FileMode.Create))) {
                    ChordListGenerator clg = new ChordListGenerator(ct);
                    var chords = clg.Generate();
                    var mm = Chord.ChordListToMidiFile(chords, 60);
                    mm.Write(bw);
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ChordType ct = new ChordType();
            ct.musicKey = MusicKey.Cdur;
            ct.keyRelation = KeyRelation.I調;
            ct.chordDegree = CD.I;
            ct.positionOfAChord = PositionOfAChord.密;
            ct.numberOfNotes = NumberOfNotes.Triad;
            ct.omission = Omission.None;
            ct.bassInversion = Inversion.根音;
            ct.termination = TerminationType.None;
            ct.is準固有 = false;
            ct.has固有VII = false;
            ct.alteration = AlterationType.None;
            ct.addedTone = AddedToneType.None;

            GenerateChordMidi(ct, "Triad");

            ct.numberOfNotes = NumberOfNotes.Seventh;
            GenerateChordMidi(ct, "Seventh");

            Console.WriteLine("Done.");


        }


    }
}
