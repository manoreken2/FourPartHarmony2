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

        private void GenerateIdurChordMidi(ChordType ct, string desc, bool completeDeg, ref int[] n) {
            var keys = new KeyAndName[] {
                new KeyAndName(MusicKey.Cdur, "C" ),
                new KeyAndName(MusicKey.CISdur, "Cis" ),
                new KeyAndName(MusicKey.Ddur, "D" ),
                new KeyAndName(MusicKey.ESdur, "Es" ),
                new KeyAndName(MusicKey.Edur, "E" ),
                new KeyAndName(MusicKey.Fdur, "F" ),
                new KeyAndName(MusicKey.FISdur, "Fis" ),
                new KeyAndName(MusicKey.Gdur, "G" ),
                new KeyAndName(MusicKey.ASdur, "As" ),
                new KeyAndName(MusicKey.Adur, "A" ),
                new KeyAndName(MusicKey.Bdur, "B" ),
                new KeyAndName(MusicKey.Hdur, "H" ),
            };

            ct.chordDegree = CD.I;

            int ik = 0;
            foreach (var k in keys) {
                ct.musicKey = k.k;

                var pos = new PositionOfAChord[] { PositionOfAChord.密, PositionOfAChord.開, PositionOfAChord.Oct};
                foreach (var p in pos) {
                    ct.positionOfAChord = p;

                    ChordListGenerator clg = new ChordListGenerator(ct);
                    clg.CompleteDeg = completeDeg;
                    var chords = clg.Generate();
                    for (int i = 0; i < chords.Count(); ++i) {
                        List<Chord> chord1 = new List<Chord>();
                        chord1.Add(chords[i]);
                        using (var bw = new BinaryWriter(File.Open(string.Format("midi/{0}{1}_{2}.mid", k.name, desc, n[ik]), FileMode.Create))) {
                            var mm = Chord.ChordListToMidiFile(chord1, 60);
                            mm.Write(bw);
                        }
                        ++n[ik];
                    }
                }

                ++ik;
            }
        }
        private void GenerateImollChordMidi(ChordType ct, string desc, bool completeDeg, ref int[] n) {
            var keys = new KeyAndName[] {
                new KeyAndName(MusicKey.Cmoll, "C" ),
                new KeyAndName(MusicKey.CISmoll, "Cis" ),
                new KeyAndName(MusicKey.Dmoll, "D" ),
                new KeyAndName(MusicKey.DISmoll, "Dis" ),
                new KeyAndName(MusicKey.Emoll, "E" ),
                new KeyAndName(MusicKey.Fmoll, "F" ),
                new KeyAndName(MusicKey.FISmoll, "Fis" ),
                new KeyAndName(MusicKey.Gmoll, "G" ),
                new KeyAndName(MusicKey.GISmoll, "Gis" ),
                new KeyAndName(MusicKey.Amoll, "A" ),
                new KeyAndName(MusicKey.Bmoll, "B" ),
                new KeyAndName(MusicKey.Hmoll, "H" ),
            };

            ct.chordDegree = CD.I;

            int ik = 0;
            foreach (var k in keys) {
                ct.musicKey = k.k;

                var pos = new PositionOfAChord[] { PositionOfAChord.密, PositionOfAChord.開, PositionOfAChord.Oct};
                foreach (var p in pos) {
                    ct.positionOfAChord = p;

                    ChordListGenerator clg = new ChordListGenerator(ct);
                    clg.CompleteDeg = completeDeg;
                    var chords = clg.Generate();
                    for (int i = 0; i < chords.Count(); ++i) {
                        List<Chord> chord1 = new List<Chord>();
                        chord1.Add(chords[i]);
                        using (var bw = new BinaryWriter(File.Open(string.Format("midi/{0}{1}_{2}.mid", k.name, desc, n[ik]), FileMode.Create))) {
                            var mm = Chord.ChordListToMidiFile(chord1, 60);
                            mm.Write(bw);
                        }
                        ++n[ik];
                    }
                }

                ++ik;
            }
        }

        private void GenerateVdurChordMidi(ChordType ct, string desc, ref int[] n) {
            var keys = new KeyAndName[] {
                new KeyAndName(MusicKey.Cdur, "G" ),
                new KeyAndName(MusicKey.CISdur, "As" ),
                new KeyAndName(MusicKey.Ddur, "A" ),
                new KeyAndName(MusicKey.ESdur, "B" ),
                new KeyAndName(MusicKey.Edur, "H" ),
                new KeyAndName(MusicKey.Fdur, "C" ),
                new KeyAndName(MusicKey.FISdur, "Cis" ),
                new KeyAndName(MusicKey.Gdur, "D" ),
                new KeyAndName(MusicKey.ASdur, "Es" ),
                new KeyAndName(MusicKey.Adur, "E" ),
                new KeyAndName(MusicKey.Bdur, "F" ),
                new KeyAndName(MusicKey.Hdur, "Fis" ),
            };

            ct.chordDegree = CD.V;

            int ik=0;
            foreach (var k in keys) {
                ct.musicKey = k.k;

                var pos = new PositionOfAChord[] { PositionOfAChord.密, PositionOfAChord.開, PositionOfAChord.Oct};
                foreach (var p in pos) {
                    ct.positionOfAChord = p;

                    ChordListGenerator clg = new ChordListGenerator(ct);
                    clg.CompleteDeg = true;
                    var chords = clg.Generate();
                    for (int i = 0; i < chords.Count(); ++i) {
                        List<Chord> chord1 = new List<Chord>();
                        chord1.Add(chords[i]);
                        using (var bw = new BinaryWriter(File.Open(string.Format("midi/{0}{1}_{2}.mid", k.name, desc, n[ik]), FileMode.Create))) {
                            var mm = Chord.ChordListToMidiFile(chord1, 60);
                            mm.Write(bw);
                        }

                        ++n[ik];
                    }
                }
                ++ik;
            }
        }

        private void GenerateVmollChordMidi(ChordType ct, string desc, ref int [] n) {
            var keys = new KeyAndName[] {
                new KeyAndName(MusicKey.Cmoll, "G" ),
                new KeyAndName(MusicKey.CISmoll, "As" ),
                new KeyAndName(MusicKey.Dmoll, "A" ),
                new KeyAndName(MusicKey.ESmoll, "B" ),
                new KeyAndName(MusicKey.Emoll, "H" ),
                new KeyAndName(MusicKey.Fmoll, "C" ),
                new KeyAndName(MusicKey.FISmoll, "Cis" ),
                new KeyAndName(MusicKey.Gmoll, "D" ),
                new KeyAndName(MusicKey.ASmoll, "Es" ),
                new KeyAndName(MusicKey.Amoll, "E" ),
                new KeyAndName(MusicKey.Bmoll, "F" ),
                new KeyAndName(MusicKey.Hmoll, "Fis" ),
            };

            ct.chordDegree = CD.V;

            int ik=0;
            foreach (var k in keys) {
                ct.musicKey = k.k;

                var pos = new PositionOfAChord[] { PositionOfAChord.密, PositionOfAChord.開, PositionOfAChord.Oct};
                foreach (var p in pos) {
                    ct.positionOfAChord = p;

                    ChordListGenerator clg = new ChordListGenerator(ct);
                    clg.CompleteDeg = true;
                    var chords = clg.Generate();
                    for (int i = 0; i < chords.Count(); ++i) {
                        List<Chord> chord1 = new List<Chord>();
                        chord1.Add(chords[i]);
                        using (var bw = new BinaryWriter(File.Open(string.Format("midi/{0}{1}_{2}.mid", k.name, desc, n[ik]), FileMode.Create))) {
                            var mm = Chord.ChordListToMidiFile(chord1, 60);
                            mm.Write(bw);
                        }

                        ++n[ik];
                    }
                }

                ++ik;
            }
        }
        private void GenerateVIIdurChordMidi(ChordType ct, string desc, ref int[] n) {
            var keys = new KeyAndName[] {
                new KeyAndName(MusicKey.Cdur, "H" ),
                new KeyAndName(MusicKey.CISdur, "C" ),
                new KeyAndName(MusicKey.Ddur, "Cis" ),
                new KeyAndName(MusicKey.ESdur, "D" ),
                new KeyAndName(MusicKey.Edur, "Es" ),
                new KeyAndName(MusicKey.Fdur, "E" ),
                new KeyAndName(MusicKey.FISdur, "F" ),
                new KeyAndName(MusicKey.Gdur, "Fis" ),
                new KeyAndName(MusicKey.ASdur, "G" ),
                new KeyAndName(MusicKey.Adur, "As" ),
                new KeyAndName(MusicKey.Bdur, "A" ),
                new KeyAndName(MusicKey.Hdur, "B" ),
            };

            ct.chordDegree = CD.VII;

            int ik=0;
            foreach (var k in keys) {
                ct.musicKey = k.k;

                var pos = new PositionOfAChord[] { PositionOfAChord.密, PositionOfAChord.開, PositionOfAChord.Oct};
                foreach (var p in pos) {
                    ct.positionOfAChord = p;

                    ChordListGenerator clg = new ChordListGenerator(ct);
                    clg.CompleteDeg = true;
                    var chords = clg.Generate();
                    for (int i = 0; i < chords.Count(); ++i) {
                        List<Chord> chord1 = new List<Chord>();
                        chord1.Add(chords[i]);
                        using (var bw = new BinaryWriter(File.Open(string.Format("midi/{0}{1}_{2}.mid", k.name, desc, n[ik]), FileMode.Create))) {
                            var mm = Chord.ChordListToMidiFile(chord1, 60);
                            mm.Write(bw);
                        }
                        ++n[ik];
                    }
                }

                ++ik;
            }
        }

        private void GenerateVmollOmitRChordMidi(ChordType ct, string desc, ref int []n) {
            var keys = new KeyAndName[] {
                new KeyAndName(MusicKey.Cmoll, "H" ),
                new KeyAndName(MusicKey.CISmoll, "C" ),
                new KeyAndName(MusicKey.Dmoll, "Cis" ),
                new KeyAndName(MusicKey.ESmoll, "D" ),
                new KeyAndName(MusicKey.Emoll, "Es" ),
                new KeyAndName(MusicKey.Fmoll, "E" ),
                new KeyAndName(MusicKey.FISmoll, "F" ),
                new KeyAndName(MusicKey.Gmoll, "Fis" ),
                new KeyAndName(MusicKey.ASmoll, "G" ),
                new KeyAndName(MusicKey.Amoll, "As" ),
                new KeyAndName(MusicKey.Bmoll, "A" ),
                new KeyAndName(MusicKey.Hmoll, "B" ),
            };

            ct.chordDegree = CD.V;
            System.Diagnostics.Debug.Assert(ct.omission == Omission.First);

            int ik=0;
            foreach (var k in keys) {
                ct.musicKey = k.k;

                var pos = new PositionOfAChord[] { PositionOfAChord.密, PositionOfAChord.開, PositionOfAChord.Oct};
                foreach (var p in pos) {
                    ct.positionOfAChord = p;

                    ChordListGenerator clg = new ChordListGenerator(ct);
                    clg.CompleteDeg = true;
                    var chords = clg.Generate();
                    for (int i = 0; i < chords.Count(); ++i) {
                        List<Chord> chord1 = new List<Chord>();
                        chord1.Add(chords[i]);
                        using (var bw = new BinaryWriter(File.Open(string.Format("midi/{0}{1}_{2}.mid", k.name, desc, n[ik]), FileMode.Create))) {
                            var mm = Chord.ChordListToMidiFile(chord1, 60);
                            mm.Write(bw);
                        }
                        ++n[ik];
                    }
                }

                ++ik;
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

            int [] n;

            // 長三
            n = new int[12];
            ct.bassInversion = Inversion.根音;
            GenerateIdurChordMidi(ct, "", false, ref n);
            ct.bassInversion = Inversion.第3音;
            GenerateIdurChordMidi(ct, "", false, ref n);

            // 短三
            n = new int[12];
            ct.bassInversion = Inversion.根音;
            GenerateImollChordMidi(ct, "m", false, ref n);
            ct.bassInversion = Inversion.第3音;
            GenerateImollChordMidi(ct, "m", false, ref n);

            // 減三
            n = new int[12];
            ct.bassInversion = Inversion.根音;
            GenerateVIIdurChordMidi(ct, "dim", ref n);
            ct.bassInversion = Inversion.第3音;
            GenerateVIIdurChordMidi(ct, "dim", ref n);

            // 増三
            n = new int[12];
            ct.alteration = AlterationType.Raised;
            ct.bassInversion = Inversion.根音;
            GenerateVdurChordMidi(ct, "aug", ref n);
            ct.alteration = AlterationType.None;

            // 長7
            n = new int[12];
            ct.numberOfNotes = NumberOfNotes.Seventh;
            ct.bassInversion = Inversion.根音;
            GenerateIdurChordMidi(ct, "maj7", true, ref n);
            ct.bassInversion = Inversion.第3音;
            GenerateIdurChordMidi(ct, "maj7", true, ref n);
            ct.bassInversion = Inversion.第7音;
            GenerateIdurChordMidi(ct, "maj7", true, ref n);
            ct.bassInversion = Inversion.根音;

            // 短7
            n = new int[12];
            ct.numberOfNotes = NumberOfNotes.Seventh;
            ct.bassInversion = Inversion.根音;
            GenerateImollChordMidi(ct, "m7", true, ref n);
            ct.bassInversion = Inversion.第3音;
            GenerateImollChordMidi(ct, "m7", true, ref n);
            ct.bassInversion = Inversion.第7音;
            GenerateImollChordMidi(ct, "m7", true, ref n);
            ct.bassInversion = Inversion.根音;

            // 属7
            n = new int[12];
            ct.numberOfNotes = NumberOfNotes.Seventh;
            ct.bassInversion = Inversion.根音;
            GenerateVdurChordMidi(ct, "7", ref n);
            ct.bassInversion = Inversion.第3音;
            GenerateVdurChordMidi(ct, "7", ref n);
            ct.bassInversion = Inversion.第7音;
            GenerateVdurChordMidi(ct, "7", ref n);
            ct.bassInversion = Inversion.根音;

            // 減5・7
            n = new int[12];
            ct.numberOfNotes = NumberOfNotes.Seventh;
            ct.bassInversion = Inversion.根音;
            GenerateVIIdurChordMidi(ct, "m7flat5", ref n);
            ct.bassInversion = Inversion.第3音;
            GenerateVIIdurChordMidi(ct, "m7flat5", ref n);
            ct.bassInversion = Inversion.第7音;
            GenerateVIIdurChordMidi(ct, "m7flat5", ref n);
            ct.bassInversion = Inversion.根音;

            // 長9
            n = new int[12];
            ct.numberOfNotes = NumberOfNotes.Ninth;
            ct.bassInversion = Inversion.根音;
            GenerateVdurChordMidi(ct, "9", ref n);
            ct.bassInversion = Inversion.第3音;
            GenerateVdurChordMidi(ct, "9", ref n);
            ct.bassInversion = Inversion.第7音;
            GenerateVdurChordMidi(ct, "9", ref n);
            ct.bassInversion = Inversion.根音;

            // 短9
            n = new int[12];
            ct.numberOfNotes = NumberOfNotes.Ninth;
            ct.bassInversion = Inversion.根音;
            GenerateVmollChordMidi(ct, "m9", ref n);
            ct.bassInversion = Inversion.第3音;
            GenerateVmollChordMidi(ct, "m9", ref n);
            ct.bassInversion = Inversion.第7音;
            GenerateVmollChordMidi(ct, "m9", ref n);
            ct.bassInversion = Inversion.根音;

            // 減7
            n = new int[12];
            ct.numberOfNotes = NumberOfNotes.Ninth;
            ct.omission = Omission.First;
            ct.bassInversion = Inversion.第3音;
            GenerateVmollOmitRChordMidi(ct, "dim7", ref n);
            ct.bassInversion = Inversion.根音;
            ct.omission = Omission.None;

            Console.WriteLine("Done.");


        }


    }
}
