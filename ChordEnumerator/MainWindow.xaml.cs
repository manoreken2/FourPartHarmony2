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

            ChordListGenerator clg = new ChordListGenerator(ct);
            var chords = clg.Generate();

            var mm = Chord.ChordListToMidiFile(chords, 60);

            using (var bw = new BinaryWriter(File.Open("CMajorTriad.mid", FileMode.Create))) {
                mm.Write(bw);
            }

            Console.WriteLine("Done.");


        }


    }
}
