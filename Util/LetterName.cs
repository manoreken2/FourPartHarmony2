using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FourPartHarmony2
{
    /// <summary>
    /// 音名 Letter Name do re mi
    /// </summary>
    public enum LN
    {
        NA,

        // triple flat

        FESESES,
        CESESES,
        GESESES,
        DESESES,
        ASESES,
        ESESES,
        HESESES,

        // double flat

        FESES,
        CESES,
        GESES,
        DESES,
        ASES,
        ESES,
        HESES,

        // flat

        FES,
        CES,
        GES,
        DES,
        AS,
        ES,
        B,

        // natural

        F,
        C,
        G,
        D,
        A,
        E,
        H,

        // sharp

        FIS,
        CIS,
        GIS,
        DIS,
        AIS,
        EIS,
        HIS,

        // double sharp

        FISIS,
        CISIS,
        GISIS,
        DISIS,
        AISIS,
        EISIS,
        HISIS,

        // triple sharp

        FISISIS,
        CISISIS,
        GISISIS,
        DISISIS,
        AISISIS,
        EISISIS,
        HISISIS,

        NUM
    };

    public enum KeySigType
    {
        TripleFlat,
        DoubleFlat,
        Flat,
        Natural,
        Sharp,
        DoubleSharp,
        TripleSharp,
    }

    internal struct LnProperty
    {
        public LN Ln { get; set; }
        public LN Natural { get; set; }
        public KeySigType Kst { get; set; }
        public int NoteDistanceFromC { get; set; }
        public int FreqIndexFromC { get; set; }
        public LN Sharp { get; set; }
        public LN Flat { get; set; }
        public LN NextSemi1 { get; set; }
        public LN NextSemi2 { get; set; }

        public void Init(LN ln, LN naturalLN, LN sharp, LN flat,
            KeySigType kst, int noteDistanceFromC, int freqIndexFromC, LN nextSemi1, LN nextSemi2) {
            this.Ln = ln;
            this.Natural = naturalLN;
            this.Sharp = sharp;
            this.Flat = flat;
            this.Kst = kst;
            this.NoteDistanceFromC = noteDistanceFromC;
            this.FreqIndexFromC = freqIndexFromC;
            this.NextSemi1 = nextSemi1;
            this.NextSemi2 = nextSemi2;
        }
    }

    public struct LetterName
    {
        private readonly LN ln;

        private static readonly LnProperty [] properties;
        static LetterName() {
            properties = new LnProperty[(int)LN.NUM];
            int n=0;
            properties[n++].Init(LN.NA, LN.NA, LN.NA, LN.NA, KeySigType.Natural, 0, 0, LN.NA, LN.NA);

            properties[n++].Init(LN.FESESES, LN.F, LN.FESES, LN.NA, KeySigType.TripleFlat, 3, 2, LN.NA, LN.GESESES);
            properties[n++].Init(LN.CESESES, LN.C, LN.CESES, LN.NA, KeySigType.TripleFlat, 0, -3, LN.NA, LN.DESESES);
            properties[n++].Init(LN.GESESES, LN.G, LN.GESES, LN.NA, KeySigType.TripleFlat, 4, 4, LN.NA, LN.ASESES);
            properties[n++].Init(LN.DESESES, LN.D, LN.DESES, LN.NA, KeySigType.TripleFlat, 1, -1, LN.NA, LN.ESESES);
            properties[n++].Init(LN.ASESES, LN.A, LN.ASES, LN.NA, KeySigType.TripleFlat, 5, 6, LN.NA, LN.HESESES);
            properties[n++].Init(LN.ESESES, LN.E, LN.ESES, LN.NA, KeySigType.TripleFlat, 2, 1, LN.FESESES, LN.FESES);
            properties[n++].Init(LN.HESESES, LN.H, LN.HESES, LN.NA, KeySigType.TripleFlat, 6, 8, LN.CESESES, LN.CESES);

            properties[n++].Init(LN.FESES, LN.F, LN.FES, LN.FESESES, KeySigType.DoubleFlat, 3, 3, LN.GESESES, LN.GESES);
            properties[n++].Init(LN.CESES, LN.C, LN.CES, LN.CESESES, KeySigType.DoubleFlat, 0, -2, LN.DESESES, LN.DESES);
            properties[n++].Init(LN.GESES, LN.G, LN.GES, LN.GESESES, KeySigType.DoubleFlat, 4, 5, LN.ASESES, LN.ASES);
            properties[n++].Init(LN.DESES, LN.D, LN.DES, LN.DESESES, KeySigType.DoubleFlat, 1, 0, LN.ESESES, LN.ESES);
            properties[n++].Init(LN.ASES, LN.A, LN.AS, LN.ASESES, KeySigType.DoubleFlat, 5, 7, LN.HESESES, LN.HESES);
            properties[n++].Init(LN.ESES, LN.E, LN.ES, LN.ESESES, KeySigType.DoubleFlat, 2, 2, LN.FESES, LN.FES);
            properties[n++].Init(LN.HESES, LN.H, LN.B, LN.HESESES, KeySigType.DoubleFlat, 6, 9, LN.CESES, LN.CES);

            properties[n++].Init(LN.FES, LN.F, LN.F, LN.FESES, KeySigType.Flat, 3, 4, LN.GESES, LN.GES);
            properties[n++].Init(LN.CES, LN.C, LN.C, LN.CESES, KeySigType.Flat, 0, -1, LN.DESES, LN.DES);
            properties[n++].Init(LN.GES, LN.G, LN.G, LN.GESES, KeySigType.Flat, 4, 6, LN.ASES, LN.AS);
            properties[n++].Init(LN.DES, LN.D, LN.D, LN.DESES, KeySigType.Flat, 1, 1, LN.ESES, LN.ES);
            properties[n++].Init(LN.AS, LN.A, LN.A, LN.ASES, KeySigType.Flat, 5, 8, LN.HESES, LN.B);
            properties[n++].Init(LN.ES, LN.E, LN.E, LN.ESES, KeySigType.Flat, 2, 3, LN.FES, LN.F);
            properties[n++].Init(LN.B, LN.H, LN.H, LN.HESES, KeySigType.Flat, 6, 10, LN.CES, LN.C);

            properties[n++].Init(LN.F, LN.F, LN.FIS, LN.FES, KeySigType.Natural, 3, 5, LN.GES, LN.G);
            properties[n++].Init(LN.C, LN.C, LN.CIS, LN.CES, KeySigType.Natural, 0, 0, LN.DES, LN.D);
            properties[n++].Init(LN.G, LN.G, LN.GIS, LN.GES, KeySigType.Natural, 4, 7, LN.AS, LN.A);
            properties[n++].Init(LN.D, LN.D, LN.DIS, LN.DES, KeySigType.Natural, 1, 2, LN.ES, LN.E);
            properties[n++].Init(LN.A, LN.A, LN.AIS, LN.AS, KeySigType.Natural, 5, 9, LN.B, LN.H);
            properties[n++].Init(LN.E, LN.E, LN.EIS, LN.ES, KeySigType.Natural, 2, 4, LN.F, LN.FIS);
            properties[n++].Init(LN.H, LN.H, LN.HIS, LN.B, KeySigType.Natural, 6, 11, LN.C, LN.CIS);

            properties[n++].Init(LN.FIS, LN.F, LN.FISIS, LN.F, KeySigType.Sharp, 3, 6, LN.G, LN.GIS);
            properties[n++].Init(LN.CIS, LN.C, LN.CISIS, LN.C, KeySigType.Sharp, 0, 1, LN.D, LN.DIS);
            properties[n++].Init(LN.GIS, LN.G, LN.GISIS, LN.G, KeySigType.Sharp, 4, 8, LN.A, LN.AIS);
            properties[n++].Init(LN.DIS, LN.D, LN.DISIS, LN.D, KeySigType.Sharp, 1, 3, LN.E, LN.EIS);
            properties[n++].Init(LN.AIS, LN.A, LN.AISIS, LN.A, KeySigType.Sharp, 5, 10, LN.H, LN.HIS);
            properties[n++].Init(LN.EIS, LN.E, LN.EISIS, LN.E, KeySigType.Sharp, 2, 5, LN.FIS, LN.FISIS);
            properties[n++].Init(LN.HIS, LN.H, LN.HISIS, LN.H, KeySigType.Sharp, 6, 12, LN.CIS, LN.CISIS);

            properties[n++].Init(LN.FISIS, LN.F, LN.FISISIS, LN.FIS, KeySigType.DoubleSharp, 3, 7, LN.GIS, LN.GISIS);
            properties[n++].Init(LN.CISIS, LN.C, LN.CISISIS, LN.CIS, KeySigType.DoubleSharp, 0, 2, LN.DIS, LN.DISIS);
            properties[n++].Init(LN.GISIS, LN.G, LN.GISISIS, LN.GIS, KeySigType.DoubleSharp, 4, 9, LN.AIS, LN.AISIS);
            properties[n++].Init(LN.DISIS, LN.D, LN.DISISIS, LN.DIS, KeySigType.DoubleSharp, 1, 4, LN.EIS, LN.EISIS);
            properties[n++].Init(LN.AISIS, LN.A, LN.AISISIS, LN.AIS, KeySigType.DoubleSharp, 5, 11, LN.HIS, LN.HISIS);
            properties[n++].Init(LN.EISIS, LN.E, LN.EISISIS, LN.EIS, KeySigType.DoubleSharp, 2, 6, LN.FISIS, LN.FISISIS);
            properties[n++].Init(LN.HISIS, LN.H, LN.HISISIS, LN.HIS, KeySigType.DoubleSharp, 6, 13, LN.CISIS, LN.CISISIS);

            properties[n++].Init(LN.FISISIS, LN.F, LN.NA, LN.FISIS, KeySigType.TripleSharp, 3, 8, LN.GISIS, LN.GISISIS);
            properties[n++].Init(LN.CISISIS, LN.C, LN.NA, LN.CISIS, KeySigType.TripleSharp, 0, 3, LN.DISIS, LN.DISISIS);
            properties[n++].Init(LN.GISISIS, LN.G, LN.NA, LN.GISIS, KeySigType.TripleSharp, 4, 10, LN.AISIS, LN.AISISIS);
            properties[n++].Init(LN.DISISIS, LN.D, LN.NA, LN.DISIS, KeySigType.TripleSharp, 1, 5, LN.EISIS, LN.EISISIS);
            properties[n++].Init(LN.AISISIS, LN.A, LN.NA, LN.AISIS, KeySigType.TripleSharp, 5, 12, LN.HISIS, LN.HISISIS);
            properties[n++].Init(LN.EISISIS, LN.E, LN.NA, LN.EISIS, KeySigType.TripleSharp, 2, 7, LN.FISISIS, LN.NA);
            properties[n++].Init(LN.HISISIS, LN.H, LN.NA, LN.HISIS, KeySigType.TripleSharp, 6, 14, LN.CISISIS, LN.NA);
        }

        public LetterName(LN ln) {
            this.ln = ln;
        }

        public LN LN {
            get { return ln; }
        }

        public bool Is(LN ln) {
            return this.ln == ln;
        }

        /// <summary>
        /// フラットやシャープを取った音名。
        /// </summary>
        /// <returns></returns>
        public LN NaturalLetterName() {
            return properties[(int)ln].Natural;
        }

        /// <summary>
        /// シャープやフラットの情報
        /// </summary>
        public KeySigType GetKeySigType() {
            return properties[(int)ln].Kst;
        }

        /// <summary>
        /// フラットか。
        /// </summary>
        public bool IsFlat() {
            return properties[(int)ln].Kst == KeySigType.Flat;
        }

        /// <summary>
        /// シャープか。
        /// </summary>
        public bool IsSharp() {
            return properties[(int)ln].Kst == KeySigType.Sharp;
        }

        /// <summary>
        /// ダブルシャープか
        /// </summary>
        public bool IsDoubleSharp() {
            return properties[(int)ln].Kst == KeySigType.DoubleSharp;
        }

        /// <summary>
        /// ダブルフラットか
        /// </summary>
        public bool IsDoubleFlat() {
            return properties[(int)ln].Kst == KeySigType.DoubleFlat;
        }

        /// <summary>
        /// トリプルシャープか
        /// </summary>
        public bool IsTripleSharp() {
            return properties[(int)ln].Kst == KeySigType.TripleSharp;
        }

        /// <summary>
        /// トリプルフラットか
        /// </summary>
        public bool IsTripleFlat() {
            return properties[(int)ln].Kst == KeySigType.TripleFlat;
        }

        public bool IsNatural() {
            return properties[(int)ln].Kst == KeySigType.Natural;
        }

        /// <summary>
        /// 半音高い音を戻す。楽譜上の高さはそのままにする。
        /// </summary>
        /// <returns>lnを半音高くした音。</returns>
        public LN Sharp() {
            return properties[(int)ln].Sharp;
        }

        /// <summary>
        /// 半音低い音を戻す。楽譜上の高さはそのままにする。
        /// </summary>
        /// <returns>lnを半音低くした音。</returns>
        public LN Flat() {
            return properties[(int)ln].Flat;
        }

        /// <summary>
        /// Cとの「度」(同度==0) 0～6
        /// ただしオクターブは考慮していない
        /// </summary>
        public int NoteDistanceFromC() {
            return properties[(int)ln].NoteDistanceFromC;
        }

        /// <summary>
        /// ピッチをrhsと比較する。
        /// </summary>
        /// <param name="rhs"></param>
        /// <returns>
        /// rhsよりも自分のほうが低い: 負
        /// rhsよりも自分のほうが高い: 正
        /// 自分とrhsとが同じピッチ: 0
        /// </returns>
        public int CompareTo(LetterName rhs) {
            int me = ToFreqIndex();
            int you = rhs.ToFreqIndex();
            if (me < you) {
                return -1;
            } else if (you < me) {
                return 1;
            } else {
                return 0;
            }
        }

        /// <summary>
        /// Cからの相対の音の高さ(半音の数)。C==0、CES==-1
        /// </summary>
        /// <returns></returns>
        public int ToFreqIndex() {
            return properties[(int)ln].FreqIndexFromC;
        }

        /// <summary>
        /// 半音高く、音名が1つ進んだLetterNameを戻す。
        /// </summary>
        public LetterName NextSemi1() {
            return new LetterName(properties[(int)ln].NextSemi1);
        }

        /// <summary>
        /// 全音高く、音名が1つ進んだLetterNameを戻す。
        /// </summary>
        public LetterName NextSemi2() {
            return new LetterName(properties[(int)ln].NextSemi2);
        }
    }
}
