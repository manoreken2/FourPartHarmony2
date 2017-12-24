using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FourPartHarmony2
{
    /// <summary>
    /// シリアライズするMusic情報
    /// </summary>
    public class MusicSave
    {
        private readonly int FILE_VERSION = 2;

        public int fileVersion;
        public int tempo;
        public ChordSave[] chordSaves;

        public MusicSave() {
        }

        public MusicSave(Music m) {
            fileVersion = FILE_VERSION;
            tempo = m.Tempo;
            chordSaves = new ChordSave[m.GetNumOfChords()];
            for (int i = 0; i < chordSaves.Length; ++i) {
                chordSaves[i] = new ChordSave(m.GetChord(i));
            }
        }

        public Music ToMusic() {
            var ret = new Music();
            ret.Tempo = tempo;
            ret.FileVersion = fileVersion;
            for (int i = 0; i < chordSaves.Length; ++i) {
                ret.Insert(i, chordSaves[i].ToChord(fileVersion));
            }
            return ret;
        }

        public void ExportAsMusicXml(TextWriter w) {
            w.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            w.WriteLine("<!DOCTYPE score-partwise PUBLIC \"-//Recordare//DTD MusicXML 2.0 Partwise//EN\"");
            w.WriteLine("    \"http://www.musicxml.org/dtds/partwise.dtd\">");
            w.WriteLine("<score-partwise version=\"2.0\">");
            w.WriteLine("  <part-list>");
            w.WriteLine("    <part-group number=\"1\" type=\"start\">");
            w.WriteLine("      <group-symbol>bracket</group-symbol>");
            w.WriteLine("      <group-barline>yes</group-barline>");
            w.WriteLine("    </part-group>");
            w.WriteLine("    <score-part id=\"P1\">");
            w.WriteLine("      <part-name>Sop</part-name>");
            w.WriteLine("      <part-abbreviation>S</part-abbreviation>");
            w.WriteLine("    </score-part>");
            w.WriteLine("    <score-part id=\"P2\">");
            w.WriteLine("      <part-name>Alt</part-name>");
            w.WriteLine("      <part-abbreviation>A</part-abbreviation>");
            w.WriteLine("    </score-part>");
            w.WriteLine("    <score-part id=\"P3\">");
            w.WriteLine("      <part-name>Ten</part-name>");
            w.WriteLine("      <part-abbreviation>T</part-abbreviation>");
            w.WriteLine("    </score-part>");
            w.WriteLine("    <score-part id=\"P4\">");
            w.WriteLine("      <part-name>Bas</part-name>");
            w.WriteLine("      <part-abbreviation>B</part-abbreviation>");
            w.WriteLine("    </score-part>");
            w.WriteLine("    <part-group number=\"1\" type=\"stop\"/>");
            w.WriteLine("  </part-list>");
            w.WriteLine("  <!--=========================================================-->");

            // Sop
            MXOutputPartHeader(w, "P1", ClefType.G2);
            int duration = 0;
            int measure = 0;
            foreach (var chord in chordSaves) {
                if (duration !=0 && (duration % 16) == 0) {
                    ++measure;
                    w.WriteLine("  </measure>");
                    w.WriteLine("  <!--=========================================================-->");
                    w.WriteLine("  <measure number=\"{0}\">", measure);
                }
                var sopNote = new LetterName(chord.sop.letterName);
                w.WriteLine("    <note>");
                w.WriteLine("      <pitch>");
                w.WriteLine("        <step>{0}</step>", sopNote.NaturalLetterName());
                w.WriteLine("        <alter>{0}</alter>", ((int)sopNote.GetKeySigType()) - (int)KeySigType.Natural);
                w.WriteLine("        <octave>{0}</octave>", chord.sop.octave);
                w.WriteLine("      </pitch>");
                w.WriteLine("      <duration>2</duration>");
                w.WriteLine("      <type>half</type>");
                w.WriteLine("    </note>");

                duration += 8;
            }
            MXOutputPartFooter(w);

            w.WriteLine("  <!--=========================================================-->");

            // Alt
            MXOutputPartHeader(w, "P2", ClefType.G2);
            duration = 0;
            measure = 0;
            foreach (var chord in chordSaves) {
                if (duration !=0 && (duration % 16) == 0) {
                    ++measure;
                    w.WriteLine("  </measure>");
                    w.WriteLine("  <!--=========================================================-->");
                    w.WriteLine("  <measure number=\"{0}\">", measure);
                }
                var altNote = new LetterName(chord.alt.letterName);
                w.WriteLine("    <note>");
                w.WriteLine("      <pitch>");
                w.WriteLine("        <step>{0}</step>", altNote.NaturalLetterName());
                w.WriteLine("        <alter>{0}</alter>", ((int)altNote.GetKeySigType()) - (int)KeySigType.Natural);
                w.WriteLine("        <octave>{0}</octave>", chord.alt.octave);
                w.WriteLine("      </pitch>");
                w.WriteLine("      <duration>2</duration>");
                w.WriteLine("      <type>half</type>");
                w.WriteLine("    </note>");

                duration += 8;
            }
            MXOutputPartFooter(w);

            w.WriteLine("  <!--=========================================================-->");

            // へ音記号の五線 Ten
            MXOutputPartHeader(w, "P3", ClefType.F4);
            duration = 0;
            measure = 0;
            foreach (var chord in chordSaves) {
                if (duration !=0 && (duration % 16) == 0) {
                    ++measure;
                    w.WriteLine("  </measure>");
                    w.WriteLine("  <!--=========================================================-->");
                    w.WriteLine("  <measure number=\"{0}\">", measure);
                }
                // Ten
                var tenNote = new LetterName(chord.ten.letterName);
                w.WriteLine("    <note>");
                w.WriteLine("      <pitch>");
                w.WriteLine("        <step>{0}</step>", tenNote.NaturalLetterName());
                w.WriteLine("        <alter>{0}</alter>", ((int)tenNote.GetKeySigType()) - (int)KeySigType.Natural);
                w.WriteLine("        <octave>{0}</octave>", chord.ten.octave);
                w.WriteLine("      </pitch>");
                w.WriteLine("      <duration>2</duration>");
                w.WriteLine("      <type>half</type>");
                w.WriteLine("    </note>");

                duration += 8;
            }
            MXOutputPartFooter(w);

            w.WriteLine("  <!--=========================================================-->");

            // へ音記号の五線 Bas
            MXOutputPartHeader(w, "P4", ClefType.F4);
            duration = 0;
            measure = 0;
            foreach (var chord in chordSaves) {
                if (duration !=0 && (duration % 16) == 0) {
                    ++measure;
                    w.WriteLine("  </measure>");
                    w.WriteLine("  <!--=========================================================-->");
                    w.WriteLine("  <measure number=\"{0}\">", measure);
                }
                // Bas
                var basNote = new LetterName(chord.bas.letterName);
                w.WriteLine("    <note>");
                w.WriteLine("      <pitch>");
                w.WriteLine("        <step>{0}</step>", basNote.NaturalLetterName());
                w.WriteLine("        <alter>{0}</alter>", ((int)basNote.GetKeySigType()) - (int)KeySigType.Natural);
                w.WriteLine("        <octave>{0}</octave>", chord.bas.octave);
                w.WriteLine("      </pitch>");
                w.WriteLine("      <duration>2</duration>");
                w.WriteLine("      <type>half</type>");
                w.WriteLine("    </note>");

                duration += 8;
            }
            MXOutputPartFooter(w);
            w.WriteLine("  <!--=========================================================-->");
            w.WriteLine("</score-partwise>");
        }

        enum ClefType {
            G2,
            F4
        };

        private void MXOutputPartHeader(TextWriter w, string partId, ClefType clef) {
            w.WriteLine("  <part id=\"{0}\">", partId);
            w.WriteLine("    <measure number=\"0\">");
            w.WriteLine("      <attributes>");

            var mki = new MusicKeyInfo(chordSaves[0].musicKey, KeyRelation.I調);
            w.WriteLine("        <key>");
            w.WriteLine("          <fifths>{0}</fifths>", mki.FlatNum() - mki.SharpNum());
            w.WriteLine("          <mode>{0}</mode>", mki.IsMajor() ? "major" : "minor");
            w.WriteLine("        </key>");
            w.WriteLine("        <time>");
            w.WriteLine("          <beats>2</beats>");
            w.WriteLine("          <beat-type>2</beat-type>");
            w.WriteLine("        </time>");

            w.WriteLine("        <clef>");
            switch (clef) {
            case ClefType.G2:
                w.WriteLine("          <sign>G</sign>");
                w.WriteLine("          <line>2</line>");
                break;
            case ClefType.F4:
                w.WriteLine("          <sign>F</sign>");
                w.WriteLine("          <line>4</line>");
                break;
            default:
                System.Diagnostics.Debug.Assert(false);
                break;
            }
            w.WriteLine("        </clef>");

            w.WriteLine("      </attributes>");
            w.WriteLine("      <sound tempo=\"{0}\"/>", tempo);
        }

        private void MXOutputPartFooter(TextWriter w) {
            w.WriteLine("      <barline location=\"right\">");
            w.WriteLine("        <bar-style>light-heavy</bar-style>");
            w.WriteLine("      </barline>");
            w.WriteLine("    </measure>");
            w.WriteLine("  </part>");
        }
    }
}
