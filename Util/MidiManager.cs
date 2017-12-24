﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace FourPartHarmony2
{
    public class MidiWriter
    {
        public MidiWriter(BinaryWriter a)
        {
            bw = a;
        }

        BinaryWriter bw;

        public void WriteByte(byte b)
        {
            bw.Write(b);
        }

        public void WriteBE4(int v)
        {
            bw.Write((byte)((v >> 24) & 0xff));
            bw.Write((byte)((v >> 16) & 0xff));
            bw.Write((byte)((v >> 8) & 0xff));
            bw.Write((byte)(v & 0xff));
        }

        public void WriteBE2(short v)
        {
            bw.Write((byte)((v >> 8) & 0xff));
            bw.Write((byte)(v & 0xff));
        }

        public static int CountMidiNumberBytes(ushort v)
        {
            if (128 * 128 <= v) {
                return 3;
            } else if (128 <= v) {
                return 2;
            } else {
                return 1;
            }
        }

        public void WriteMidiNumber(ushort v)
        {
            if (128 * 128 <= v) {
                int v2 = v / 128 * 128;
                int v1 = v - v2 * 128 * 128;
                v1 = v1 / 128;
                int v0 = v - v2 * 128 * 128 - v1 * 128;

                v2 += 128;
                v1 += 128;

                bw.Write((byte)v2);
                bw.Write((byte)v1);
                bw.Write((byte)v0);
            } else if (128 <= v) {
                int v1 = v / 128;
                int v0 = v - v1 * 128;

                v1 += 128;

                bw.Write((byte)v1);
                bw.Write((byte)v0);
            } else {
                bw.Write((byte)v);
            }
        }
    }

    public struct Note
    {
        private int        offset;
        private int        length;
        private LetterName letterName;
        private int        octave;
        private int        volume;

        public Note(int offset, int length, LetterName letterName, int octave, int volume)
        {
            this.offset     = offset;
            this.length     = length;
            this.letterName = letterName;
            this.octave     = octave;
            this.volume     = volume;
        }

        public int CountMidiBytes(ref int timeCursorRW)
        {
            if (letterName.Is(LN.NA)) {
                return 0;
            }

            int bytes =
                MidiWriter.CountMidiNumberBytes((ushort)(offset - timeCursorRW))
                + 3 +
                MidiWriter.CountMidiNumberBytes((ushort)(length))
                + 3;
            timeCursorRW = offset + length;
            return bytes;
        }

        private int GetMidiPitchValue()
        {
            return letterName.ToFreqIndex() + 12 * (octave+1);
        }

        // returns new time cursor
        public int Write(int timeCursor, MidiWriter mw)
        {
            if (letterName.Is(LN.NA)) {
                return timeCursor;
            }

            // pitch
            int p = GetMidiPitchValue();

            mw.WriteMidiNumber((ushort)(offset - timeCursor));
            mw.WriteByte(0x90);

            System.Diagnostics.Debug.Assert(p <= 0x7f);
            mw.WriteByte((byte)p);

            System.Diagnostics.Debug.Assert(volume <= 0x7f);
            mw.WriteByte((byte)volume);

            // stop note
            mw.WriteMidiNumber((ushort)(length));
            mw.WriteByte(0x90);
            mw.WriteByte((byte)p);
            mw.WriteByte((byte)0);

            return offset + length;
        }
    }

    public class MidiTrackInfo
    {
        public MidiTrackInfo()
        {
            noteList = new System.Collections.Generic.List<Note>();
        }

        public void AddNote(Note note)
        {
            noteList.Add(note);
        }

        public void Write(BinaryWriter a)
        {
            MidiWriter mw = new MidiWriter(a);

            int timeCursor = 0;
            foreach (Note n in noteList) {
                timeCursor = n.Write(timeCursor, mw);
            }
        }

        public int CountMidiBytes()
        {
            int bytes = 0;
            int timeCursor = 0;
            foreach (Note n in noteList) {
                bytes += n.CountMidiBytes(ref timeCursor);
            }

            return bytes;
        }

        System.Collections.Generic.List<Note> noteList;
    }

    class MidiHeaderInfo
    {
        short tempo;
        short nTrack;

        public MidiHeaderInfo(int tempo, short nTrack)
        {
            this.tempo = (short)tempo;
            this.nTrack = nTrack;
        }

        public void WriteMidiHeader(BinaryWriter a)
        {
            MidiWriter mw = new MidiWriter(a);

            mw.WriteByte((byte)'M');
            mw.WriteByte((byte)'T');
            mw.WriteByte((byte)'h');
            mw.WriteByte((byte)'d');

            mw.WriteBE4(6);
            mw.WriteBE2(1);
            mw.WriteBE2(nTrack);
            mw.WriteBE2(tempo);
        }
    }

    class MidiTrackHeaderInfo
    {
        public void WriteTrackHeader(int partDataBytes, BinaryWriter a)
        {
            MidiWriter mw = new MidiWriter(a);

            mw.WriteByte((byte)'M');
            mw.WriteByte((byte)'T');
            mw.WriteByte((byte)'r');
            mw.WriteByte((byte)'k');

            // trackData = partData + trackFooter(4 bytes)
            mw.WriteBE4(partDataBytes + 4);
        }

        public void WriteTrackFooter(BinaryWriter a)
        {
            MidiWriter mw = new MidiWriter(a);
            mw.WriteBE4(0x00ff2f00);
        }
    }

    public class MidiManager
    {
        MidiTrackInfo[] tracks;
        int tempo;

        public MidiManager(int nTrack)
        {
            tempo = (short)75;
            tracks = new MidiTrackInfo[nTrack];
            for (int i = 0; i < nTrack; ++i) {
                tracks[i] = new MidiTrackInfo();
            }
        }

        public MidiTrackInfo GetTrack(int n)
        {
            return tracks[n];
        }

        public void SetTempo(int tempo)
        {
            this.tempo = tempo;
        }

        public void Write(BinaryWriter bw)
        {
            MidiHeaderInfo mhi = new MidiHeaderInfo(tempo*2, (short)tracks.Length);
            MidiTrackHeaderInfo thi = new MidiTrackHeaderInfo();

            mhi.WriteMidiHeader(bw);

            for (int i = 0; i < tracks.Length; ++i) {
                MidiTrackInfo p = GetTrack(i);
                thi.WriteTrackHeader(p.CountMidiBytes(), bw);
                p.Write(bw);
                thi.WriteTrackFooter(bw);
            }
        }

        private static string tempPath = string.Empty;

        public static void CreateTempFile() {
            // いけてない
            tempPath = Path.GetTempPath() + Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".mid";
            System.Console.WriteLine("tempPath={0}", tempPath);
        }

        public static void DeleteTempFile() {
            Stop();
            if (File.Exists(tempPath)) {
                File.Delete(tempPath);
            }
        }

        [DllImport("winmm.dll")]
        static extern Int32 mciSendString(String command, StringBuilder buffer,
                                          Int32 bufferSize, IntPtr hwndCallback);

        public static void Stop()
        {
            mciSendString("stop track", new StringBuilder(), 0, new IntPtr(0));
            mciSendString("close track", new StringBuilder(), 0, new IntPtr(0));
        }

        private static void SetDevice(int deviceId)
        {
            mciSendString("set port "+deviceId, new StringBuilder(), 0, new IntPtr(0));
        }
        private static void SetPortMapper() => mciSendString("set port mapper", new StringBuilder(), 0, new IntPtr(0));

        public static void Play(string path)
        {
            mciSendString("open " + path + " alias track", new StringBuilder(), 0, new IntPtr(0));
            mciSendString("play track", new StringBuilder(), 0, new IntPtr(0));
        }

        //static int deviceId = 0;

        public void PlayNow()
        {
            Stop();

            using (BinaryWriter bw
                = new BinaryWriter(File.Open(tempPath, FileMode.Create))) {
                Write(bw);
            }

            /*
            SetDevice(deviceId);

            Console.WriteLine("deviceId=" + deviceId);
            ++deviceId;

            SetPortMapper();
            */


            Play(tempPath);
        }

    }
}
