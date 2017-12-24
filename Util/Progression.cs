using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FourPartHarmony2
{
    /// <summary>
    /// 2声部の同時-継時関係
    /// </summary>
    public enum Motion
    {
        Sustain,  //< 同時保留
        Oblique,  //< 斜行
        Parallel, //< 平行
        Contrary  //< 反行
    }

    public enum D定型進行BasType {
        a下行,
        a上行,
        b,
        c,
        d,
        e,
        f,
        g
    }

    public class D諸和音定型進行Info {
        public string Sop定型進行名 { get { return sop定型進行Idx.ToString(); } }
        public string Bas定型進行名 {
            get {
                switch (bas定型進行Type) {
                case D定型進行BasType.a下行: return "a↓";
                case D定型進行BasType.a上行: return "a↑";
                case D定型進行BasType.b: return "b";
                case D定型進行BasType.c: return "c";
                case D定型進行BasType.d: return "d";
                case D定型進行BasType.e: return "e";
                case D定型進行BasType.f: return "f";
                case D定型進行BasType.g: return "g";
                default:
                    System.Diagnostics.Debug.Assert(false);
                    return "";
                }
            }
        }

        public int Sop定型進行Idx { get { return sop定型進行Idx; } }
        public D定型進行BasType Bas定型進行Type { get { return bas定型進行Type; } }

        private int sop定型進行Idx;
        private D定型進行BasType bas定型進行Type;

        private int sopPreDeg;
        private int sopProgInterval;
        private int basPreDeg;
        private int basProgInterval;

        public D諸和音定型進行Info(
            int sop定型進行idx,
            D定型進行BasType bas定型進行Type,
            int sopPreDeg,
            int sopProgInterval,
            int basPreDeg,
            int basProgInterval) {
            this.sop定型進行Idx = sop定型進行idx;
            this.bas定型進行Type = bas定型進行Type;
            this.sopPreDeg = sopPreDeg;
            this.sopProgInterval = sopProgInterval;
            this.basPreDeg = basPreDeg;
            this.basProgInterval = basProgInterval;
        }

        /// <summary>
        /// この定型進行か？
        /// </summary>
        public bool Is(
            int sopPreDeg,
            int sopProgInterval,
            int basPreDeg,
            int basProgInterval) {
            return
                this.sopPreDeg == sopPreDeg &&
                this.sopProgInterval == sopProgInterval &&
                this.basPreDeg == basPreDeg &&
                this.basProgInterval == basProgInterval;
        }

        /// <summary>
        /// 同一の定型進行か？
        /// </summary>
        public bool Is(D諸和音定型進行Info rhs) {
            return
                this.sopPreDeg == rhs.sopPreDeg &&
                this.sopProgInterval == rhs.sopProgInterval &&
                this.basPreDeg == rhs.basPreDeg &&
                this.basProgInterval == rhs.basProgInterval;
        }

    };

    /// <summary>
    /// 終止定式の判断に使用する和音種類情報。
    /// </summary>
    public enum 定型和音Type {
        不定,
        I基,
        I1転,
        I2転,
        V基,
        V7基,
        V基またはV7基,
        VI基,
        IV基,
        IV2転,
        S和音K1K2,
        S和音K3,
        S和音またはT和音,
        T和音,
    };

    /// <summary>
    /// 定型和音Typeの文字列表現を得るための構造体。
    /// </summary>
    public struct 定型和音Info {
        private 定型和音Type type;

        public 定型和音Info(定型和音Type type) {
            this.type = type;
        }
        public string GetString() {
            switch (type) {
            case 定型和音Type.不定: return "不定";
            case 定型和音Type.I基: return "[基]I";
            case 定型和音Type.I1転: return "[1転]I";
            case 定型和音Type.I2転: return "[2転]I";
            case 定型和音Type.V基: return "[基]V";
            case 定型和音Type.V7基: return "[基]V7";
            case 定型和音Type.V基またはV7基: return "[基]Vまたは[基]V7";
            case 定型和音Type.VI基: return "[基]VI";
            case 定型和音Type.IV基: return "[基]IV";
            case 定型和音Type.IV2転: return "[2転]IV";
            case 定型和音Type.S和音K1K2: return "S和音(K3専用S和音除く)";
            case 定型和音Type.S和音K3: return "K3に使用可能なS和音";
            case 定型和音Type.S和音またはT和音: return "S和音またはT和音";
            case 定型和音Type.T和音: return "T和音";
            default:
                System.Diagnostics.Debug.Assert(false);
                return "";
            }
        }

        /// <summary>
        /// 和音Cがこの和音種類に該当するか
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool Match(Chord c) {
            switch (type) {
            case 定型和音Type.不定: return true;
            case 定型和音Type.I基: return c.Is("[基]I");
            case 定型和音Type.I1転: return c.Is("[1転]I");
            case 定型和音Type.I2転: return c.Is("[2転]I");
            case 定型和音Type.V基: return c.Is("[基]V");
            case 定型和音Type.V7基: return c.Is("[基]V7");
            case 定型和音Type.V基またはV7基: return c.Is("[基]V") || c.Is("[基]V7");
            case 定型和音Type.VI基: return c.Is("[基]VI");
            case 定型和音Type.IV基: return c.Is("[基]IV");
            case 定型和音Type.IV2転: return c.Is("[2転]IV");
            case 定型和音Type.S和音K1K2: return (c.ChordDegree == CD.II || c.ChordDegree == CD.IV || c.ChordDegree == CD.V_V) && c.AddedTone == AddedToneType.None;
            case 定型和音Type.S和音K3: return c.ChordDegree == CD.II || c.ChordDegree == CD.IV;
            case 定型和音Type.S和音またはT和音: return c.ChordDegree != CD.V;
            case 定型和音Type.T和音: return c.Is("[基]I") || c.Is("[1転]I") || c.Is("[基]VI");
            default:
                System.Diagnostics.Debug.Assert(false);
                return false;
            }
        }
    }

    /// <summary>
    /// 単一パートの音度と、定型和音のペア。
    /// </summary>
    public struct Part音度and定型和音 {
        public int partDegree;
        public 定型和音Type chordTypeEnum;

        public Part音度and定型和音(
            int partDegree,
            定型和音Type chordTypeEnum) {
            this.partDegree = partDegree;
            this.chordTypeEnum = chordTypeEnum;
        }

        public string Get定型和音Name() {
            return new 定型和音Info(chordTypeEnum).GetString();
        }
    };

    /// <summary>
    /// Bas終止定式またはSop終止定式。
    /// </summary>
    public class Part終止定式Info {
        public string Name {get; set;}
        public TerminationType 終止Type { get; set; }

        private Part音度and定型和音[] m_Part終止定式;
        private int idx;

        /// <summary>
        /// Sop終止定式の場合のみ使用。
        /// </summary>
        private List<int> m_対応Bas終止定式IdxList;

        public string GetTerminationString() {
            return new TerminationInfo(終止Type).GetString();
        }
        public string GetTerminationStringShort() {
            return new TerminationInfo(終止Type).GetStringShort();
        }

        public Part終止定式Info(int idx, string name, TerminationType type,
            int partDegree0, 定型和音Type chordTypeEnum0,
            int partDegree1, 定型和音Type chordTypeEnum1,
            int 対応Bas終止定式Idx0 = -1,
            int 対応Bas終止定式Idx1 = -1,
            int 対応Bas終止定式Idx2 = -1,
            int 対応Bas終止定式Idx3 = -1,
            int 対応Bas終止定式Idx4 = -1,
            int 対応Bas終止定式Idx5 = -1) {
            this.idx = idx;
            this.Name = name;
            this.終止Type = type;
            m_Part終止定式 = new Part音度and定型和音[2];
            m_Part終止定式[0] = new Part音度and定型和音(partDegree0, chordTypeEnum0);
            m_Part終止定式[1] = new Part音度and定型和音(partDegree1, chordTypeEnum1);

            m_対応Bas終止定式IdxList = new List<int>();
            if (対応Bas終止定式Idx0 != -1) { m_対応Bas終止定式IdxList.Add(対応Bas終止定式Idx0); }
            if (対応Bas終止定式Idx1 != -1) { m_対応Bas終止定式IdxList.Add(対応Bas終止定式Idx1); }
            if (対応Bas終止定式Idx2 != -1) { m_対応Bas終止定式IdxList.Add(対応Bas終止定式Idx2); }
            if (対応Bas終止定式Idx3 != -1) { m_対応Bas終止定式IdxList.Add(対応Bas終止定式Idx3); }
            if (対応Bas終止定式Idx4 != -1) { m_対応Bas終止定式IdxList.Add(対応Bas終止定式Idx4); }
            if (対応Bas終止定式Idx5 != -1) { m_対応Bas終止定式IdxList.Add(対応Bas終止定式Idx5); }
        }

        public Part終止定式Info(int idx, string name, TerminationType type,
            int partDegree0, 定型和音Type chordTypeEnum0,
            int partDegree1, 定型和音Type chordTypeEnum1,
            int partDegree2, 定型和音Type chordTypeEnum2,
            int 対応Bas終止定式Idx0 = -1,
            int 対応Bas終止定式Idx1 = -1,
            int 対応Bas終止定式Idx2 = -1,
            int 対応Bas終止定式Idx3 = -1,
            int 対応Bas終止定式Idx4 = -1,
            int 対応Bas終止定式Idx5 = -1) {
            this.idx = idx;
            this.Name = name;
            this.終止Type = type;
            m_Part終止定式 = new Part音度and定型和音[3];
            m_Part終止定式[0] = new Part音度and定型和音(partDegree0, chordTypeEnum0);
            m_Part終止定式[1] = new Part音度and定型和音(partDegree1, chordTypeEnum1);
            m_Part終止定式[2] = new Part音度and定型和音(partDegree2, chordTypeEnum2);

            m_対応Bas終止定式IdxList = new List<int>();
            if (対応Bas終止定式Idx0 != -1) { m_対応Bas終止定式IdxList.Add(対応Bas終止定式Idx0); }
            if (対応Bas終止定式Idx1 != -1) { m_対応Bas終止定式IdxList.Add(対応Bas終止定式Idx1); }
            if (対応Bas終止定式Idx2 != -1) { m_対応Bas終止定式IdxList.Add(対応Bas終止定式Idx2); }
            if (対応Bas終止定式Idx3 != -1) { m_対応Bas終止定式IdxList.Add(対応Bas終止定式Idx3); }
            if (対応Bas終止定式Idx4 != -1) { m_対応Bas終止定式IdxList.Add(対応Bas終止定式Idx4); }
            if (対応Bas終止定式Idx5 != -1) { m_対応Bas終止定式IdxList.Add(対応Bas終止定式Idx5); }
        }

        public Part終止定式Info(int idx, string name, TerminationType type,
            int partDegree0, 定型和音Type chordTypeEnum0,
            int partDegree1, 定型和音Type chordTypeEnum1,
            int partDegree2, 定型和音Type chordTypeEnum2,
            int partDegree3, 定型和音Type chordTypeEnum3,
            int 対応Bas終止定式Idx0 = -1,
            int 対応Bas終止定式Idx1 = -1,
            int 対応Bas終止定式Idx2 = -1,
            int 対応Bas終止定式Idx3 = -1,
            int 対応Bas終止定式Idx4 = -1,
            int 対応Bas終止定式Idx5 = -1) {
            this.idx = idx;
            this.Name = name;
            this.終止Type = type;
            m_Part終止定式 = new Part音度and定型和音[4];
            m_Part終止定式[0] = new Part音度and定型和音(partDegree0, chordTypeEnum0);
            m_Part終止定式[1] = new Part音度and定型和音(partDegree1, chordTypeEnum1);
            m_Part終止定式[2] = new Part音度and定型和音(partDegree2, chordTypeEnum2);
            m_Part終止定式[3] = new Part音度and定型和音(partDegree3, chordTypeEnum3);

            m_対応Bas終止定式IdxList = new List<int>();
            if (対応Bas終止定式Idx0 != -1) { m_対応Bas終止定式IdxList.Add(対応Bas終止定式Idx0); }
            if (対応Bas終止定式Idx1 != -1) { m_対応Bas終止定式IdxList.Add(対応Bas終止定式Idx1); }
            if (対応Bas終止定式Idx2 != -1) { m_対応Bas終止定式IdxList.Add(対応Bas終止定式Idx2); }
            if (対応Bas終止定式Idx3 != -1) { m_対応Bas終止定式IdxList.Add(対応Bas終止定式Idx3); }
            if (対応Bas終止定式Idx4 != -1) { m_対応Bas終止定式IdxList.Add(対応Bas終止定式Idx4); }
            if (対応Bas終止定式Idx5 != -1) { m_対応Bas終止定式IdxList.Add(対応Bas終止定式Idx5); }
        }

        public int Count {
            get {return m_Part終止定式.Length;}
        }

        public Part音度and定型和音 Get(int idx) {
            return m_Part終止定式[idx];
        }

        /// <summary>
        /// この終止定式のindexを戻す。
        /// </summary>
        public int Idx {
            get { return idx; }
        }

        /// <summary>
        /// Sop終止定式の場合のみ使用。対応Bas終止定式Idxの総数を戻す。
        /// </summary>
        public int 対応Bas終止定式IdxCount() {
            return m_対応Bas終止定式IdxList.Count();
        }

        /// <summary>
        /// Sop終止定式の場合のみ使用。
        /// </summary>
        /// <param name="idx">0～対応Bas終止定式IdxCount-1</param>
        /// <returns>対応Bas終止定式番号</returns>
        public int 対応Bas終止定式IdxGet(int idx) {
            return m_対応Bas終止定式IdxList[idx];
        }

    };

    public class Progression
    {
        private int m_pos;
        private Chord pre3C;
        private Chord prepreC;
        private Chord preC;
        private Chord nowC;
        public int Pos {
            get { return m_pos; }
        }
        public Chord Pre3C {
            get { return pre3C; }
        }
        public Chord PrepreC {
            get { return prepreC; }
        }
        public Chord PreC {
            get { return preC; }
        }
        public Chord NowC {
            get { return nowC; }
        }

        public Progression(List<Chord> chordList, int pos, Chord aNowC) {
            System.Diagnostics.Debug.Assert(0 <= pos);

            preC = null;
            if (1 <= pos) {
                preC = chordList[pos - 1];
            }

            prepreC = null;
            if (2 <= pos) {
                prepreC = chordList[pos - 2];
            }

            pre3C = null;
            if (3 <= pos) {
                pre3C = chordList[pos - 3];
            }

            this.m_pos = pos;
            this.nowC = aNowC;
        }

        public Progression(List<Chord> chordList, int pos) {
            System.Diagnostics.Debug.Assert(0 <= pos);
            System.Diagnostics.Debug.Assert(pos < chordList.Count);

            preC = null;
            if (1 <= pos) {
                preC = chordList[pos - 1];
            }

            prepreC = null;
            if (2 <= pos) {
                prepreC = chordList[pos - 2];
            }

            pre3C = null;
            if (3 <= pos) {
                pre3C = chordList[pos - 3];
            }

            this.m_pos = pos;
            this.nowC = chordList[pos];
        }

        private static List<D諸和音定型進行Info> m_D諸和音定型進行List;
        private static Part終止定式Info [] m_Bas終止定式List;
        private static Part終止定式Info[]  m_Sop終止定式List;

        public enum Bas終止定式Type {
            Invalid=-1,
            a1, a2, a3, a4, a5, a6,
            b1, b2, b3, b4, b5, b6, b7, b8, b9,
            c1, c2, c3, c4, c5, c6, c7, c8, c9,
            d1, d2, d3, d4,
            NUM
        }

        public enum Sop終止定式Type {
            Invalid =-1,
            a1, a1p,
            a2, a2p,
            a3, a3p,
            a4, a4p,
            a5, a5p,
            a6, a6p,
            a7, a7p,
            a8, a8p,

            b1, b1p,
            b2, b2p,
            b3, b3p,
            b4, b4p,
            b5, b5p,
            b6, b6p,
            b7, b7p,
            b8,

            c1, c1p,
            c2, c2p,
            c3, c3p,
            c4, c4p,
            c5, c5p,
            c6, c6p,

            NUM
        }

        static Progression() {
            m_Bas終止定式List = new Part終止定式Info[(int)Bas終止定式Type.NUM];
            m_Bas終止定式List[(int)Bas終止定式Type.a1] = new Part終止定式Info((int)Bas終止定式Type.a1, "a1", TerminationType.Perfect, 2, 定型和音Type.S和音K1K2, 5, 定型和音Type.V基またはV7基, 1, 定型和音Type.I基);
            m_Bas終止定式List[(int)Bas終止定式Type.a2] = new Part終止定式Info((int)Bas終止定式Type.a2, "a2", TerminationType.Perfect, 4, 定型和音Type.S和音K1K2, 5, 定型和音Type.V基またはV7基, 1, 定型和音Type.I基);
            m_Bas終止定式List[(int)Bas終止定式Type.a3] = new Part終止定式Info((int)Bas終止定式Type.a3, "a3", TerminationType.Perfect, 6, 定型和音Type.S和音K1K2, 5, 定型和音Type.V基またはV7基, 1, 定型和音Type.I基);
            m_Bas終止定式List[(int)Bas終止定式Type.a4] = new Part終止定式Info((int)Bas終止定式Type.a4, "a4", TerminationType.Perfect, 2, 定型和音Type.S和音K1K2, 5, 定型和音Type.I2転, 5, 定型和音Type.V基またはV7基, 1, 定型和音Type.I基);
            m_Bas終止定式List[(int)Bas終止定式Type.a5] = new Part終止定式Info((int)Bas終止定式Type.a5, "a5", TerminationType.Perfect, 4, 定型和音Type.S和音K1K2, 5, 定型和音Type.I2転, 5, 定型和音Type.V基またはV7基, 1, 定型和音Type.I基);
            m_Bas終止定式List[(int)Bas終止定式Type.a6] = new Part終止定式Info((int)Bas終止定式Type.a6, "a6", TerminationType.Perfect, 6, 定型和音Type.S和音K1K2, 5, 定型和音Type.I2転, 5, 定型和音Type.V基またはV7基, 1, 定型和音Type.I基);

            m_Bas終止定式List[(int)Bas終止定式Type.b1] = new Part終止定式Info((int)Bas終止定式Type.b1, "b1", TerminationType.Deceptive, 2, 定型和音Type.S和音K1K2, 5, 定型和音Type.V基またはV7基, 6, 定型和音Type.VI基);
            m_Bas終止定式List[(int)Bas終止定式Type.b2] = new Part終止定式Info((int)Bas終止定式Type.b2, "b2", TerminationType.Deceptive, 4, 定型和音Type.S和音K1K2, 5, 定型和音Type.V基またはV7基, 6, 定型和音Type.VI基);
            m_Bas終止定式List[(int)Bas終止定式Type.b3] = new Part終止定式Info((int)Bas終止定式Type.b3, "b3", TerminationType.Deceptive, 6, 定型和音Type.S和音K1K2, 5, 定型和音Type.V基またはV7基, 6, 定型和音Type.VI基);
            m_Bas終止定式List[(int)Bas終止定式Type.b4] = new Part終止定式Info((int)Bas終止定式Type.b4, "b4", TerminationType.Deceptive, 2, 定型和音Type.S和音K1K2, 5, 定型和音Type.I2転, 5, 定型和音Type.V基またはV7基, 6, 定型和音Type.VI基);
            m_Bas終止定式List[(int)Bas終止定式Type.b5] = new Part終止定式Info((int)Bas終止定式Type.b5, "b5", TerminationType.Deceptive, 4, 定型和音Type.S和音K1K2, 5, 定型和音Type.I2転, 5, 定型和音Type.V基またはV7基, 6, 定型和音Type.VI基);
            m_Bas終止定式List[(int)Bas終止定式Type.b6] = new Part終止定式Info((int)Bas終止定式Type.b6, "b6", TerminationType.Deceptive, 6, 定型和音Type.S和音K1K2, 5, 定型和音Type.I2転, 5, 定型和音Type.V基またはV7基, 6, 定型和音Type.VI基);
            m_Bas終止定式List[(int)Bas終止定式Type.b7] = new Part終止定式Info((int)Bas終止定式Type.b7, "b7", TerminationType.Deceptive, 1, 定型和音Type.I基, 5, 定型和音Type.V基またはV7基, 6, 定型和音Type.VI基);
            m_Bas終止定式List[(int)Bas終止定式Type.b8] = new Part終止定式Info((int)Bas終止定式Type.b8, "b8", TerminationType.Deceptive, 3, 定型和音Type.I1転, 5, 定型和音Type.V基またはV7基, 6, 定型和音Type.VI基);
            m_Bas終止定式List[(int)Bas終止定式Type.b9] = new Part終止定式Info((int)Bas終止定式Type.b9, "b9", TerminationType.Deceptive, 6, 定型和音Type.VI基, 5, 定型和音Type.V基またはV7基, 6, 定型和音Type.VI基);

            m_Bas終止定式List[(int)Bas終止定式Type.c1] = new Part終止定式Info((int)Bas終止定式Type.c1, "c1", TerminationType.Half, 2, 定型和音Type.S和音K1K2, 5, 定型和音Type.V基);
            m_Bas終止定式List[(int)Bas終止定式Type.c2] = new Part終止定式Info((int)Bas終止定式Type.c2, "c2", TerminationType.Half, 4, 定型和音Type.S和音K1K2, 5, 定型和音Type.V基);
            m_Bas終止定式List[(int)Bas終止定式Type.c3] = new Part終止定式Info((int)Bas終止定式Type.c3, "c3", TerminationType.Half, 6, 定型和音Type.S和音K1K2, 5, 定型和音Type.V基);
            m_Bas終止定式List[(int)Bas終止定式Type.c4] = new Part終止定式Info((int)Bas終止定式Type.c4, "c4", TerminationType.Half, 2, 定型和音Type.S和音K1K2, 5, 定型和音Type.I2転, 5, 定型和音Type.V基);
            m_Bas終止定式List[(int)Bas終止定式Type.c5] = new Part終止定式Info((int)Bas終止定式Type.c5, "c5", TerminationType.Half, 4, 定型和音Type.S和音K1K2, 5, 定型和音Type.I2転, 5, 定型和音Type.V基);
            m_Bas終止定式List[(int)Bas終止定式Type.c6] = new Part終止定式Info((int)Bas終止定式Type.c6, "c6", TerminationType.Half, 6, 定型和音Type.S和音K1K2, 5, 定型和音Type.I2転, 5, 定型和音Type.V基);
            m_Bas終止定式List[(int)Bas終止定式Type.c7] = new Part終止定式Info((int)Bas終止定式Type.c7, "c7", TerminationType.Half, 1, 定型和音Type.I基, 5, 定型和音Type.V基);
            m_Bas終止定式List[(int)Bas終止定式Type.c8] = new Part終止定式Info((int)Bas終止定式Type.c8, "c8", TerminationType.Half, 3, 定型和音Type.I1転, 5, 定型和音Type.V基);
            m_Bas終止定式List[(int)Bas終止定式Type.c9] = new Part終止定式Info((int)Bas終止定式Type.c9, "c9", TerminationType.Half, 6, 定型和音Type.VI基, 5, 定型和音Type.V基);

            m_Bas終止定式List[(int)Bas終止定式Type.d1] = new Part終止定式Info((int)Bas終止定式Type.d1, "d1", TerminationType.Plagal, 1, 定型和音Type.I基, 4, 定型和音Type.S和音K3, 1, 定型和音Type.I基);
            m_Bas終止定式List[(int)Bas終止定式Type.d2] = new Part終止定式Info((int)Bas終止定式Type.d2, "d2", TerminationType.Plagal, 3, 定型和音Type.I1転, 4, 定型和音Type.S和音K3, 1, 定型和音Type.I基);
            m_Bas終止定式List[(int)Bas終止定式Type.d3] = new Part終止定式Info((int)Bas終止定式Type.d3, "d3", TerminationType.Plagal, 6, 定型和音Type.VI基, 4, 定型和音Type.S和音K3, 1, 定型和音Type.I基);
            m_Bas終止定式List[(int)Bas終止定式Type.d4] = new Part終止定式Info((int)Bas終止定式Type.d4, "d4", TerminationType.Plagal, 1, 定型和音Type.I基, 4, 定型和音Type.IV2転, 1, 定型和音Type.I基);

            m_Sop終止定式List = new Part終止定式Info[(int)Sop終止定式Type.NUM];
            m_Sop終止定式List[(int)Sop終止定式Type.a1]  = new Part終止定式Info((int)Sop終止定式Type.a1,  "a1",   TerminationType.Perfect, 2, 定型和音Type.S和音K1K2,                       7, 定型和音Type.V基またはV7基, 1, 定型和音Type.I基, (int)Bas終止定式Type.a1, (int)Bas終止定式Type.a2);
            m_Sop終止定式List[(int)Sop終止定式Type.a1p] = new Part終止定式Info((int)Sop終止定式Type.a1p, "a1’", TerminationType.Perfect, 2, 定型和音Type.S和音K1K2, 1, 定型和音Type.I2転, 7, 定型和音Type.V基またはV7基, 1, 定型和音Type.I基, (int)Bas終止定式Type.a4, (int)Bas終止定式Type.a5);
            m_Sop終止定式List[(int)Sop終止定式Type.a2]  = new Part終止定式Info((int)Sop終止定式Type.a2,  "a2",   TerminationType.Perfect, 1, 定型和音Type.S和音K1K2,                       7, 定型和音Type.V基またはV7基, 1, 定型和音Type.I基, (int)Bas終止定式Type.a1, (int)Bas終止定式Type.a2, (int)Bas終止定式Type.a3);
            m_Sop終止定式List[(int)Sop終止定式Type.a2p] = new Part終止定式Info((int)Sop終止定式Type.a2p, "a2’", TerminationType.Perfect, 1, 定型和音Type.S和音K1K2, 1, 定型和音Type.I2転, 7, 定型和音Type.V基またはV7基, 1, 定型和音Type.I基, (int)Bas終止定式Type.a4, (int)Bas終止定式Type.a5, (int)Bas終止定式Type.a6);
            m_Sop終止定式List[(int)Sop終止定式Type.a3]  = new Part終止定式Info((int)Sop終止定式Type.a3,  "a3",   TerminationType.Perfect, 2, 定型和音Type.S和音K1K2,                       2, 定型和音Type.V基またはV7基, 1, 定型和音Type.I基, (int)Bas終止定式Type.a1, (int)Bas終止定式Type.a2, (int)Bas終止定式Type.a3);
            m_Sop終止定式List[(int)Sop終止定式Type.a3p] = new Part終止定式Info((int)Sop終止定式Type.a3p, "a3’", TerminationType.Perfect, 2, 定型和音Type.S和音K1K2, 3, 定型和音Type.I2転, 2, 定型和音Type.V基またはV7基, 1, 定型和音Type.I基, (int)Bas終止定式Type.a4, (int)Bas終止定式Type.a5, (int)Bas終止定式Type.a6);
            m_Sop終止定式List[(int)Sop終止定式Type.a4]  = new Part終止定式Info((int)Sop終止定式Type.a4,  "a4",   TerminationType.Perfect, 4, 定型和音Type.S和音K1K2,                       2, 定型和音Type.V基またはV7基, 1, 定型和音Type.I基, (int)Bas終止定式Type.a1, (int)Bas終止定式Type.a2);
            m_Sop終止定式List[(int)Sop終止定式Type.a4p] = new Part終止定式Info((int)Sop終止定式Type.a4p, "a4’", TerminationType.Perfect, 4, 定型和音Type.S和音K1K2, 3, 定型和音Type.I2転, 2, 定型和音Type.V基またはV7基, 1, 定型和音Type.I基, (int)Bas終止定式Type.a4, (int)Bas終止定式Type.a5, (int)Bas終止定式Type.a6);
            m_Sop終止定式List[(int)Sop終止定式Type.a5]  = new Part終止定式Info((int)Sop終止定式Type.a5,  "a5",   TerminationType.Perfect, 3, 定型和音Type.S和音K1K2,                       2, 定型和音Type.V基またはV7基, 1, 定型和音Type.I基, (int)Bas終止定式Type.a1, (int)Bas終止定式Type.a2);
            m_Sop終止定式List[(int)Sop終止定式Type.a5p] = new Part終止定式Info((int)Sop終止定式Type.a5p, "a5’", TerminationType.Perfect, 3, 定型和音Type.S和音K1K2, 3, 定型和音Type.I2転, 2, 定型和音Type.V基またはV7基, 1, 定型和音Type.I基, (int)Bas終止定式Type.a4, (int)Bas終止定式Type.a5, (int)Bas終止定式Type.a6);
            m_Sop終止定式List[(int)Sop終止定式Type.a6]  = new Part終止定式Info((int)Sop終止定式Type.a6,  "a6",   TerminationType.Perfect, 1, 定型和音Type.S和音K1K2,                       2, 定型和音Type.V基またはV7基, 1, 定型和音Type.I基, (int)Bas終止定式Type.a3);
            m_Sop終止定式List[(int)Sop終止定式Type.a6p] = new Part終止定式Info((int)Sop終止定式Type.a6p, "a6’", TerminationType.Perfect, 1, 定型和音Type.S和音K1K2, 3, 定型和音Type.I2転, 2, 定型和音Type.V基またはV7基, 1, 定型和音Type.I基, (int)Bas終止定式Type.a6);
            m_Sop終止定式List[(int)Sop終止定式Type.a7]  = new Part終止定式Info((int)Sop終止定式Type.a7,  "a7",   TerminationType.Perfect, 2, 定型和音Type.S和音K1K2,                       2, 定型和音Type.V基またはV7基, 3, 定型和音Type.I基, (int)Bas終止定式Type.a1, (int)Bas終止定式Type.a2, (int)Bas終止定式Type.a3);
            m_Sop終止定式List[(int)Sop終止定式Type.a7p] = new Part終止定式Info((int)Sop終止定式Type.a7p, "a7’", TerminationType.Perfect, 2, 定型和音Type.S和音K1K2, 3, 定型和音Type.I2転, 2, 定型和音Type.V基またはV7基, 3, 定型和音Type.I基, (int)Bas終止定式Type.a4, (int)Bas終止定式Type.a5, (int)Bas終止定式Type.a6);
            m_Sop終止定式List[(int)Sop終止定式Type.a8]  = new Part終止定式Info((int)Sop終止定式Type.a8,  "a8",   TerminationType.Perfect, 4, 定型和音Type.S和音K1K2,                       2, 定型和音Type.V基またはV7基, 3, 定型和音Type.I基, (int)Bas終止定式Type.a1, (int)Bas終止定式Type.a2);
            m_Sop終止定式List[(int)Sop終止定式Type.a8p] = new Part終止定式Info((int)Sop終止定式Type.a8p, "a8’", TerminationType.Perfect, 4, 定型和音Type.S和音K1K2, 3, 定型和音Type.I2転, 2, 定型和音Type.V基またはV7基, 3, 定型和音Type.I基, (int)Bas終止定式Type.a4, (int)Bas終止定式Type.a5, (int)Bas終止定式Type.a6);

            m_Sop終止定式List[(int)Sop終止定式Type.b1]  = new Part終止定式Info((int)Sop終止定式Type.b1,  "b1",   TerminationType.Deceptive, 2, 定型和音Type.S和音K1K2,                       7, 定型和音Type.V基またはV7基, 1, 定型和音Type.VI基, (int)Bas終止定式Type.b1, (int)Bas終止定式Type.b2);
            m_Sop終止定式List[(int)Sop終止定式Type.b1p] = new Part終止定式Info((int)Sop終止定式Type.b1p, "b1’", TerminationType.Deceptive, 2, 定型和音Type.S和音K1K2, 1, 定型和音Type.I2転, 7, 定型和音Type.V基またはV7基, 1, 定型和音Type.VI基, (int)Bas終止定式Type.b4, (int)Bas終止定式Type.b5);
            m_Sop終止定式List[(int)Sop終止定式Type.b2]  = new Part終止定式Info((int)Sop終止定式Type.b2,  "b2",   TerminationType.Deceptive, 1, 定型和音Type.S和音またはT和音,                7, 定型和音Type.V基またはV7基, 1, 定型和音Type.VI基, (int)Bas終止定式Type.b1, (int)Bas終止定式Type.b2, (int)Bas終止定式Type.b3, (int)Bas終止定式Type.b7, (int)Bas終止定式Type.b8, (int)Bas終止定式Type.b9);
            m_Sop終止定式List[(int)Sop終止定式Type.b2p] = new Part終止定式Info((int)Sop終止定式Type.b2p, "b2’", TerminationType.Deceptive, 1, 定型和音Type.S和音K1K2, 1, 定型和音Type.I2転, 7, 定型和音Type.V基またはV7基, 1, 定型和音Type.VI基, (int)Bas終止定式Type.b4, (int)Bas終止定式Type.b5, (int)Bas終止定式Type.b6);
            m_Sop終止定式List[(int)Sop終止定式Type.b3]  = new Part終止定式Info((int)Sop終止定式Type.b3,  "b3",   TerminationType.Deceptive, 2, 定型和音Type.S和音K1K2,                       2, 定型和音Type.V基またはV7基, 1, 定型和音Type.VI基, (int)Bas終止定式Type.b1, (int)Bas終止定式Type.b2, (int)Bas終止定式Type.b3);
            m_Sop終止定式List[(int)Sop終止定式Type.b3p] = new Part終止定式Info((int)Sop終止定式Type.b3p, "b3’", TerminationType.Deceptive, 2, 定型和音Type.S和音K1K2, 3, 定型和音Type.I2転, 2, 定型和音Type.V基またはV7基, 1, 定型和音Type.VI基, (int)Bas終止定式Type.b4, (int)Bas終止定式Type.b5, (int)Bas終止定式Type.b6);
            m_Sop終止定式List[(int)Sop終止定式Type.b4]  = new Part終止定式Info((int)Sop終止定式Type.b4,  "b4",   TerminationType.Deceptive, 4, 定型和音Type.S和音K1K2,                       2, 定型和音Type.V基またはV7基, 1, 定型和音Type.VI基, (int)Bas終止定式Type.b1, (int)Bas終止定式Type.b2);
            m_Sop終止定式List[(int)Sop終止定式Type.b4p] = new Part終止定式Info((int)Sop終止定式Type.b4p, "b4’", TerminationType.Deceptive, 4, 定型和音Type.S和音K1K2, 3, 定型和音Type.I2転, 2, 定型和音Type.V基またはV7基, 1, 定型和音Type.VI基, (int)Bas終止定式Type.b4, (int)Bas終止定式Type.b5, (int)Bas終止定式Type.b6);
            m_Sop終止定式List[(int)Sop終止定式Type.b5]  = new Part終止定式Info((int)Sop終止定式Type.b5,  "b5",   TerminationType.Deceptive, 3, 定型和音Type.S和音またはT和音,                2, 定型和音Type.V基またはV7基, 1, 定型和音Type.VI基, (int)Bas終止定式Type.b1, (int)Bas終止定式Type.b2, (int)Bas終止定式Type.b3, (int)Bas終止定式Type.b7, (int)Bas終止定式Type.b8, (int)Bas終止定式Type.b9);
            m_Sop終止定式List[(int)Sop終止定式Type.b5p] = new Part終止定式Info((int)Sop終止定式Type.b5p, "b5’", TerminationType.Deceptive, 3, 定型和音Type.S和音K1K2, 3, 定型和音Type.I2転, 2, 定型和音Type.V基またはV7基, 1, 定型和音Type.VI基, (int)Bas終止定式Type.b4, (int)Bas終止定式Type.b5, (int)Bas終止定式Type.b6);
            m_Sop終止定式List[(int)Sop終止定式Type.b6]  = new Part終止定式Info((int)Sop終止定式Type.b6,  "b6",   TerminationType.Deceptive, 4, 定型和音Type.S和音K1K2,                       4, 定型和音Type.V7基,          3, 定型和音Type.VI基, (int)Bas終止定式Type.b1, (int)Bas終止定式Type.b2, (int)Bas終止定式Type.b3);
            m_Sop終止定式List[(int)Sop終止定式Type.b6p] = new Part終止定式Info((int)Sop終止定式Type.b6p, "b6’", TerminationType.Deceptive, 4, 定型和音Type.S和音K1K2, 5, 定型和音Type.I2転, 4, 定型和音Type.V7基,          3, 定型和音Type.VI基, (int)Bas終止定式Type.b6);
            m_Sop終止定式List[(int)Sop終止定式Type.b7]  = new Part終止定式Info((int)Sop終止定式Type.b7,  "b7",   TerminationType.Deceptive, 6, 定型和音Type.S和音K1K2,                       5, 定型和音Type.V基,           3, 定型和音Type.VI基, (int)Bas終止定式Type.b1, (int)Bas終止定式Type.b2);
            m_Sop終止定式List[(int)Sop終止定式Type.b7p] = new Part終止定式Info((int)Sop終止定式Type.b7p, "b7’", TerminationType.Deceptive, 6, 定型和音Type.S和音K1K2, 5, 定型和音Type.I2転, 4, 定型和音Type.V7基,          3, 定型和音Type.VI基, (int)Bas終止定式Type.b4, (int)Bas終止定式Type.b5);
            m_Sop終止定式List[(int)Sop終止定式Type.b8]  = new Part終止定式Info((int)Sop終止定式Type.b8,  "b8",   TerminationType.Deceptive, 5, 定型和音Type.T和音,                           4, 定型和音Type.V7基,          3, 定型和音Type.VI基, (int)Bas終止定式Type.b7, (int)Bas終止定式Type.b8);

            m_Sop終止定式List[(int)Sop終止定式Type.c1]  = new Part終止定式Info((int)Sop終止定式Type.c1,  "c1",   TerminationType.Half, 2, 定型和音Type.S和音K1K2,                       7, 定型和音Type.V基, (int)Bas終止定式Type.c1, (int)Bas終止定式Type.c2);
            m_Sop終止定式List[(int)Sop終止定式Type.c1p] = new Part終止定式Info((int)Sop終止定式Type.c1p, "c1’", TerminationType.Half, 2, 定型和音Type.S和音K1K2, 1, 定型和音Type.I2転, 7, 定型和音Type.V基, (int)Bas終止定式Type.c4, (int)Bas終止定式Type.c5);
            m_Sop終止定式List[(int)Sop終止定式Type.c2]  = new Part終止定式Info((int)Sop終止定式Type.c2,  "c2",   TerminationType.Half, 1, 定型和音Type.S和音またはT和音,                7, 定型和音Type.V基, (int)Bas終止定式Type.c1, (int)Bas終止定式Type.c2, (int)Bas終止定式Type.c3, (int)Bas終止定式Type.c7, (int)Bas終止定式Type.c8, (int)Bas終止定式Type.c9);
            m_Sop終止定式List[(int)Sop終止定式Type.c2p] = new Part終止定式Info((int)Sop終止定式Type.c2p, "c2’", TerminationType.Half, 1, 定型和音Type.S和音K1K2, 1, 定型和音Type.I2転, 7, 定型和音Type.V基, (int)Bas終止定式Type.c4, (int)Bas終止定式Type.c5, (int)Bas終止定式Type.c6);
            m_Sop終止定式List[(int)Sop終止定式Type.c3]  = new Part終止定式Info((int)Sop終止定式Type.c3,  "c3",   TerminationType.Half, 2, 定型和音Type.S和音K1K2,                       2, 定型和音Type.V基, (int)Bas終止定式Type.c1, (int)Bas終止定式Type.c2, (int)Bas終止定式Type.c3);
            m_Sop終止定式List[(int)Sop終止定式Type.c3p] = new Part終止定式Info((int)Sop終止定式Type.c3p, "c3’", TerminationType.Half, 2, 定型和音Type.S和音K1K2, 3, 定型和音Type.I2転, 2, 定型和音Type.V基, (int)Bas終止定式Type.c4, (int)Bas終止定式Type.c5, (int)Bas終止定式Type.c6);
            m_Sop終止定式List[(int)Sop終止定式Type.c4]  = new Part終止定式Info((int)Sop終止定式Type.c4,  "c4",   TerminationType.Half, 4, 定型和音Type.S和音K1K2,                       2, 定型和音Type.V基, (int)Bas終止定式Type.c1, (int)Bas終止定式Type.c2);
            m_Sop終止定式List[(int)Sop終止定式Type.c4p] = new Part終止定式Info((int)Sop終止定式Type.c4p, "c4’", TerminationType.Half, 4, 定型和音Type.S和音K1K2, 3, 定型和音Type.I2転, 2, 定型和音Type.V基, (int)Bas終止定式Type.c4, (int)Bas終止定式Type.c5, (int)Bas終止定式Type.c6);
            m_Sop終止定式List[(int)Sop終止定式Type.c5]  = new Part終止定式Info((int)Sop終止定式Type.c5,  "c5",   TerminationType.Half, 3, 定型和音Type.S和音またはT和音,                2, 定型和音Type.V基, (int)Bas終止定式Type.c1, (int)Bas終止定式Type.c2, (int)Bas終止定式Type.c3, (int)Bas終止定式Type.c7, (int)Bas終止定式Type.c8, (int)Bas終止定式Type.c9);
            m_Sop終止定式List[(int)Sop終止定式Type.c5p] = new Part終止定式Info((int)Sop終止定式Type.c5p, "c5’", TerminationType.Half, 3, 定型和音Type.S和音K1K2, 3, 定型和音Type.I2転, 2, 定型和音Type.V基, (int)Bas終止定式Type.c4, (int)Bas終止定式Type.c5, (int)Bas終止定式Type.c6);
            m_Sop終止定式List[(int)Sop終止定式Type.c6]  = new Part終止定式Info((int)Sop終止定式Type.c6,  "c6",   TerminationType.Half, 4, 定型和音Type.S和音K1K2,                       5, 定型和音Type.V基, (int)Bas終止定式Type.c3);
            m_Sop終止定式List[(int)Sop終止定式Type.c6p] = new Part終止定式Info((int)Sop終止定式Type.c6p, "c6’", TerminationType.Half, 6, 定型和音Type.S和音K1K2,                       5, 定型和音Type.V基, (int)Bas終止定式Type.c1, (int)Bas終止定式Type.c2);
            
            m_D諸和音定型進行List = new List<D諸和音定型進行Info>();
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(1, D定型進行BasType.a下行, 7, 1, 5, -4));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(1, D定型進行BasType.a上行, 7, 1, 5, 3));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(1, D定型進行BasType.b, 7, 1, 5, 1));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(1, D定型進行BasType.c, 7, 1, 5, -2));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(1, D定型進行BasType.e, 7, 1, 2, -1));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(1, D定型進行BasType.f, 7, 1, 2, 1));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(1, D定型進行BasType.g, 7, 1, 4, -1));

            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(2, D定型進行BasType.a下行, 2, -1, 5, -4));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(2, D定型進行BasType.a上行, 2, -1, 5, 3));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(2, D定型進行BasType.b, 2, -1, 5, 1));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(2, D定型進行BasType.c, 2, -1, 5, -2));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(2, D定型進行BasType.d, 2, -1, 7, 1));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(2, D定型進行BasType.f, 2, -1, 2, 1));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(2, D定型進行BasType.g, 2, -1, 4, -1));

            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(3, D定型進行BasType.a下行, 2, 1, 5, -4));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(3, D定型進行BasType.a上行, 2, 1, 5, 3));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(3, D定型進行BasType.d, 2, 1, 7, 1));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(3, D定型進行BasType.e, 2, 1, 2, -1));

            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(4, D定型進行BasType.a下行, 4, -1, 5, -4));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(4, D定型進行BasType.a上行, 4, -1, 5, 3));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(4, D定型進行BasType.b, 4, -1, 5, 1));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(4, D定型進行BasType.d, 4, -1, 7, 1));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(4, D定型進行BasType.e, 4, -1, 2, -1));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(4, D定型進行BasType.f, 4, -1, 2, 1));

            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(5, D定型進行BasType.a下行, 6, -1, 5, -4));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(5, D定型進行BasType.a上行, 6, -1, 5, 3));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(5, D定型進行BasType.d, 6, -1, 7, 1));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(5, D定型進行BasType.f, 6, -1, 2, 1));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(5, D定型進行BasType.g, 6, -1, 4, -1));

            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(6, D定型進行BasType.a下行, 5, 0, 5, -4));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(6, D定型進行BasType.a上行, 5, 0, 5, 3));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(6, D定型進行BasType.c, 5, 0, 5, -2));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(6, D定型進行BasType.d, 5, 0, 7, 1));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(6, D定型進行BasType.e, 5, 0, 2, -1));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(6, D定型進行BasType.f, 5, 0, 2, 1));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(6, D定型進行BasType.g, 5, 0, 4, -1));

            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(7, D定型進行BasType.a上行, 5, -2, 5, 3));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(7, D定型進行BasType.a下行, 5, -2, 5, -4));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(7, D定型進行BasType.b, 5, -2, 5, 1));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(7, D定型進行BasType.d, 5, -2, 7, 1));

            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(8, D定型進行BasType.c, 5, 3, 5, -2));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(8, D定型進行BasType.g, 5, 3, 4, -1));

            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(9, D定型進行BasType.b, 5, -4, 5, 1));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(9, D定型進行BasType.c, 5, -4, 5, -2));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(9, D定型進行BasType.d, 5, -4, 7, 1));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(9, D定型進行BasType.g, 5, -4, 4, -1));

            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(10, D定型進行BasType.c, 2, 3, 5, -2));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(10, D定型進行BasType.g, 2, 3, 4, -1));

            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(11, D定型進行BasType.c, 2, -4, 5, -2));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(11, D定型進行BasType.d, 2, -4, 7, 1));
            m_D諸和音定型進行List.Add(new D諸和音定型進行Info(11, D定型進行BasType.g, 2, -4, 4, -1));
        }

        /// <summary>
        /// 終止定式情報の和音進行どおりになっているか。
        /// </summary>
        public bool CheckChordProgression(Part終止定式Info t) {
            switch (t.Count) {
            case 2:
                if (preC == null) {
                    return false;
                }

                if (new 定型和音Info(t.Get(0).chordTypeEnum).Match(preC) &&
                    new 定型和音Info(t.Get(1).chordTypeEnum).Match(nowC)) {
                    return true;
                }
                return false;
            case 3:
                if (prepreC == null || preC == null) {
                    return false;
                }
                if (new 定型和音Info(t.Get(0).chordTypeEnum).Match(prepreC) &&
                    new 定型和音Info(t.Get(1).chordTypeEnum).Match(preC) &&
                    new 定型和音Info(t.Get(2).chordTypeEnum).Match(nowC)) {
                    return true;
                }
                return false;
            case 4:
                if (pre3C == null || prepreC == null || preC == null) {
                    return false;
                }
                if (new 定型和音Info(t.Get(0).chordTypeEnum).Match(pre3C) &&
                    new 定型和音Info(t.Get(1).chordTypeEnum).Match(prepreC) &&
                    new 定型和音Info(t.Get(2).chordTypeEnum).Match(preC) &&
                    new 定型和音Info(t.Get(3).chordTypeEnum).Match(nowC)) {
                    return true;
                }
                return false;
            default:
                System.Diagnostics.Debug.Assert(false);
                return false;
            }
        }

        /// <summary>
        /// この進行に該当するD諸和音定型進行を戻す。II巻p114
        /// </summary>
        public D諸和音定型進行Info GetD諸和音定型進行() {
            if (null == preC) {
                return null;
            }
            if (preC.ChordDegree != CD.V ||
                nowC.ChordDegree != CD.I) {
                return null;
            }

            foreach (D諸和音定型進行Info d in m_D諸和音定型進行List) {
                int sopPreDeg = preC.GetPitch(Part.Sop).Degree;
                int basPreDeg = preC.GetPitch(Part.Bas).Degree;
                int sopProgInterval = PartProgressionHigherInterval(Part.Sop);
                int basProgInterval = PartProgressionHigherInterval(Part.Bas);

                if (d.Is(sopPreDeg, sopProgInterval, basPreDeg, basProgInterval)) {
                    return d;
                }
            }
            return null;
        }

        private List<Part終止定式Info> GetPart終止定式(Part part, Part終止定式Info[] 終止定式list) {
            var result = new List<Part終止定式Info>();

            if (preC == null) {
                return result;
            }
            foreach (Part終止定式Info b in 終止定式list) {
                switch (b.Count) {
                case 2: {
                        int preDeg = preC.GetPitch(part).Degree;
                        int nowDeg = nowC.GetPitch(part).Degree;
                        if (b.Get(0).partDegree == preDeg &&
                            b.Get(1).partDegree == nowDeg) {
                            result.Add(b);
                        }
                    }
                    break;
                case 3: {
                        if (prepreC == null) {
                            continue;
                        }
                        int pre2Deg = prepreC.GetPitch(part).Degree;
                        int preDeg = preC.GetPitch(part).Degree;
                        int nowDeg = nowC.GetPitch(part).Degree;
                        if (b.Get(0).partDegree == pre2Deg &&
                            b.Get(1).partDegree == preDeg &&
                            b.Get(2).partDegree == nowDeg) {
                            result.Add(b);
                        }
                    }
                    break;
                case 4: {
                        if (pre3C == null || prepreC == null) {
                            continue;
                        }
                        int pre3Deg = pre3C.GetPitch(part).Degree;
                        int pre2Deg = prepreC.GetPitch(part).Degree;
                        int preDeg = preC.GetPitch(part).Degree;
                        int nowDeg = nowC.GetPitch(part).Degree;
                        if (b.Get(0).partDegree == pre3Deg &&
                            b.Get(1).partDegree == pre2Deg &&
                            b.Get(2).partDegree == preDeg &&
                            b.Get(3).partDegree == nowDeg) {
                            result.Add(b);
                        }
                    }
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// この進行に該当するSop終止定式を戻す。II巻p117
        /// </summary>
        /// <returns></returns>
        public List<Part終止定式Info> GetSop終止定式List() {
            return GetPart終止定式(Part.Sop, m_Sop終止定式List);
        }

        /// <summary>
        /// この進行に該当するBas終止定式進行を戻す。II巻p95
        /// </summary>
        public List<Part終止定式Info> GetBas終止定式List() {
            return GetPart終止定式(Part.Bas, m_Bas終止定式List);
        }

        public void UpdateVerdict(Verdict v) {
            nowC.UpdateVerdict(v);
        }

        /// <summary>
        /// 転調進行の場合true
        /// </summary>
        public bool Is転調進行() {
            if (preC != null && preC.KeyRelation != nowC.KeyRelation) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 配分一致か配分移行の場合trueを戻す。
        /// Oct→Octもtrue
        /// 「配分転換が生じていない」というのとは、また異なる？？？同じに見えるので同じかも。
        /// </summary>
        public bool PositionOfAChordIsTheSameOrTransfered() {
            switch (preC.PositionOfAChord) {
            case PositionOfAChord.Oct:
                return true;
            case PositionOfAChord.密:
                return
                    nowC.PositionOfAChord == PositionOfAChord.密 ||
                    nowC.PositionOfAChord == PositionOfAChord.Oct;
            case PositionOfAChord.開:
                return
                    nowC.PositionOfAChord == PositionOfAChord.開 ||
                    nowC.PositionOfAChord == PositionOfAChord.Oct;
            default:
                System.Diagnostics.Debug.Assert(false);
                return false;
            }
        }

        /// <summary>
        /// 配分転換が生じているかどうか。
        /// </summary>
        public bool PositionOfAChordChanged() {
            return
                (preC.PositionOfAChord == PositionOfAChord.開 && nowC.PositionOfAChord == PositionOfAChord.密) ||
                (preC.PositionOfAChord == PositionOfAChord.密 && nowC.PositionOfAChord == PositionOfAChord.開);
        }

        public delegate bool PartPredicate(Part part);

        /// <summary>
        /// predicateを全パートに対して実行し、それぞれの実行でtrueを戻した場合、それぞれverdictを後続和音にセットする。
        /// </summary>
        /// <param name="verdict">trueを戻した場合に後続和音にセットするVerdict。verdict.part0にtrueを戻したpartがセットされる。</param>
        /// <returns>CheckがFailした数。</returns>
        public int AllPartCheckAndIfTrueSetVerdict(PartPredicate pred, Verdict verdict) {
            int result = 0;
            for (int i=(int)Part.Bas; i <= (int)Part.Sop; ++i) {
                Part part = (Part)i;
                if (pred(part)) {
                    Verdict v = new Verdict(verdict);
                    v.part0 = part;
                    UpdateVerdict(v);
                    ++result;
                }
            }
            return result;
        }

        /// <summary>
        /// predicateを全パートに対して実行し、全てのパートがtrueを戻した場合verdictを後続和音にセットする。
        /// </summary>
        /// <returns>true: 全パートがtrueを戻し、verdictを後続和音にセットした。</returns>
        public bool AllPartCheckAndIfAllTrueSetVerdictOnce(PartPredicate pred, Verdict verdict) {
            for (int i=(int)Part.Bas; i <= (int)Part.Sop; ++i) {
                Part part = (Part)i;
                if (!pred(part)) {
                    return false;
                }
            }
            UpdateVerdict(verdict);
            return true;
        }

        /// <summary>
        /// predicateを上3声各パートに対して実行し、全ての上3声パートがtrueを戻した場合verdictを後続和音にセットする。
        /// </summary>
        public bool Upper3CheckAndIfAllTrueSetVerdictOnce(PartPredicate pred, Verdict verdict) {
            for (int i=(int)Part.Ten; i <= (int)Part.Sop; ++i) {
                Part part = (Part)i;
                if (!pred(part)) {
                    return false;
                }
            }
            UpdateVerdict(verdict);
            return true;
        }

        /// <summary>
        /// predicateを上3声各パートに対して実行し、1つでもtrueがあればverdictを後続和音にセットする。
        /// </summary>
        public bool Upper3CheckAndIfAnyTrueSetVerdictOnce(PartPredicate pred, Verdict verdict) {
            for (int i=(int)Part.Ten; i <= (int)Part.Sop; ++i) {
                Part part = (Part)i;
                if (pred(part)) {
                    UpdateVerdict(verdict);
                    return true;
                }
            }
            return false;
        }

        public List<Part> Upper3CheckAndAccumulate(PartPredicate pred) {
            var result = new List<Part>();
            for (int i = (int)Part.Ten; i <= (int)Part.Sop; ++i) {
                Part part = (Part)i;
                if (pred(part)) {
                    result.Add(part);
                }
            }
            return result;
        }

        /// <summary>
        /// partの進行音度度数-1
        /// </summary>
        /// <returns>
        /// C4→D4の場合 1
        /// D4→C4の場合 -1
        /// C4→E4の場合 2
        /// </returns>
        public int PartProgressionHigherInterval(Part part) {
            return nowC.GetPitch(part).HigherIntervalTo(preC.GetPitch(part));
        }
        /// <summary>
        /// prepreC→preCの進行音度度数-1。あらかじめPrepreC!=nullを確認してから呼ぶこと。
        /// </summary>
        /// <returns>
        /// C4→D4の場合 1
        /// D4→C4の場合 -1
        /// C4→E4の場合 2
        /// </returns>
        public int PrePartProgressionHigherInterval(Part part) {
            return preC.GetPitch(part).HigherIntervalTo(prepreC.GetPitch(part));
        }

        public int PartProgressionAbsInterval(Part part) {
            return nowC.GetPitch(part).AbsIntervalNumberWith(preC.GetPitch(part));
        }

        public bool IsPartProgression上行限定進行(Part part) {
            return PartProgressionHigherInterval(part) == 1 &&
                PartProgressionHigherPitch(part) >= 1;
        }

        public bool IsPartProgression下行限定進行(Part part) {
            return PartProgressionHigherInterval(part) == -1 &&
                PartProgressionHigherPitch(part) <= -1;
        }

        public void DebugProgression() {
            for (int i=0; i < 4; ++i) {
                Part part = (Part)i;
                System.Console.WriteLine("{0} prePitch={1} higherInterval={2}, higherPitch={3}",
                    part.ToString(),
                    PreC.GetPitch(part).Inversion,
                    PartProgressionHigherInterval(part),
                    PartProgressionHigherPitch(part));
            }
        }

        /// <summary>
        /// partの進行 半音の数
        /// C4→D4の場合 2
        /// D4→C4の場合 -2
        /// C4→E4の場合 4
        /// </summary>
        public int PartProgressionHigherPitch(Part part) {
            return nowC.GetPitch(part).HigherPitchTo(preC.GetPitch(part));
        }

        public int PartProgressionAbsPitch(Part part) {
            return nowC.GetPitch(part).AbsNumberOfSemitonesWith(preC.GetPitch(part));
        }

        public IntervalType PartProgressionIntervalType(Part part) {
            return preC.GetPitch(part).GetIntervalType(nowC.GetPitch(part));
        }
        /// <summary>
        /// prepreC→preCのPartの進行の種類。
        /// </summary>
        public IntervalType PrePrePartProgressionIntervalType(Part part) {
            return prepreC.GetPitch(part).GetIntervalType(preC.GetPitch(part));
        }

        public bool Is(string preCProp, string nowCProp, string progression) {
            if (preC == null) {
                return false;
            }
            if (!preC.Is(preCProp)) {
                return false;
            }
            if (!nowC.Is(nowCProp)) {
                return false;
            }
            if (!CheckProgression(progression)) {
                return false;
            }
            return true;
        }

        public bool ChordsAre(string preCProp, string nowCProp) {
            if (preC == null) {
                return false;
            }
            if (!preC.Is(preCProp)) {
                return false;
            }
            if (!nowC.Is(nowCProp)) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 2声部の同時-継時関係が同時保留か、斜行か、平行か、反行か。
        /// </summary>
        public Motion TwoPartMotion(Part part0, Part part1) {
            if (part0 == part1) {
                return Motion.Sustain;
            }
            int part0Progression = PartProgressionHigherInterval(part0);
            int part1Progression = PartProgressionHigherInterval(part1);
            if ((part0Progression > 0 && part1Progression > 0) ||
                (part0Progression < 0 && part1Progression < 0)) {
                return Motion.Parallel;
            }
            if ((part0Progression > 0 && part1Progression < 0) ||
                (part0Progression < 0 && part1Progression > 0)) {
                return Motion.Contrary;
            }
            if ((part0Progression == 0) &&
                (part1Progression == 0)) {
                return Motion.Sustain;
            }
            return Motion.Oblique;
        }

        enum ProgressionDirection
        {
            Any,      //< 特に決めない
            Up,       //< 上行
            Down,     //< 下行
            Sustain,  //< 保留または増一度連結。
            Contrary, //< 2つの声部が反行
            Parallel, //< 2つの声部が平行
        }

        enum ProgressionIntervalType
        {
            Degree,   //< 度数(1==同度)
            Semitone, //< 半音の個数
        }

        enum ProgressionPart
        {
            Bas, //< Part.Basと同じ番号
            Ten, //< Part.Tenと同じ番号
            Alt, //< Part.Altと同じ番号
            Sop, //< Part.Sopと同じ番号
            Upper3
        }

        private class ProgressionInfo
        {
            public int n;
            public ProgressionDirection    direction;
            public ProgressionIntervalType intervalType;
            public ProgressionPart part0;
            public ProgressionPart part1;

            public ProgressionInfo() {
                Clear();
            }

            public void Clear() {
                n = 0;
                direction = ProgressionDirection.Any;
                intervalType = ProgressionIntervalType.Degree;
            }
        }

        private bool CheckOnePartProgression(ProgressionInfo pinfo) {
            switch (pinfo.direction) {
            case ProgressionDirection.Sustain:
                switch (pinfo.part0) {
                case ProgressionPart.Bas: if (PartProgressionHigherInterval(Part.Bas) != 0) { return false; } break;
                case ProgressionPart.Ten: if (PartProgressionHigherInterval(Part.Ten) != 0) { return false; } break;
                case ProgressionPart.Alt: if (PartProgressionHigherInterval(Part.Alt) != 0) { return false; } break;
                case ProgressionPart.Sop: if (PartProgressionHigherInterval(Part.Sop) != 0) { return false; } break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
                break;
            case ProgressionDirection.Up:
                if (pinfo.n == 0) {
                    // とにかく上行していればよい
                    switch (pinfo.part0) {
                    case ProgressionPart.Bas: if (PartProgressionHigherInterval(Part.Bas) <= 0) { return false; } break;
                    case ProgressionPart.Ten: if (PartProgressionHigherInterval(Part.Ten) <= 0) { return false; } break;
                    case ProgressionPart.Alt: if (PartProgressionHigherInterval(Part.Alt) <= 0) { return false; } break;
                    case ProgressionPart.Sop: if (PartProgressionHigherInterval(Part.Sop) <= 0) { return false; } break;
                    case ProgressionPart.Upper3:
                        if (PartProgressionHigherInterval(Part.Ten) <= 0) { return false; }
                        if (PartProgressionHigherInterval(Part.Alt) <= 0) { return false; }
                        if (PartProgressionHigherInterval(Part.Sop) <= 0) { return false; }
                        break;
                    }
                } else {
                    if (pinfo.intervalType == ProgressionIntervalType.Degree) {
                        // 度数指定(n度上行)
                        int nInterval = pinfo.n-1;
                        switch (pinfo.part0) {
                        case ProgressionPart.Bas: if (PartProgressionHigherInterval(Part.Bas) != nInterval) { return false; } break;
                        case ProgressionPart.Ten: if (PartProgressionHigherInterval(Part.Ten) != nInterval) { return false; } break;
                        case ProgressionPart.Alt: if (PartProgressionHigherInterval(Part.Alt) != nInterval) { return false; } break;
                        case ProgressionPart.Sop: if (PartProgressionHigherInterval(Part.Sop) != nInterval) { return false; } break;
                        default:
                            System.Diagnostics.Debug.Assert(false);
                            break;
                        }
                    } else {
                        // 半音の数指定(n個上行)
                        switch (pinfo.part0) {
                        case ProgressionPart.Bas: if (PartProgressionHigherPitch(Part.Bas) != pinfo.n) { return false; } break;
                        case ProgressionPart.Ten: if (PartProgressionHigherPitch(Part.Ten) != pinfo.n) { return false; } break;
                        case ProgressionPart.Alt: if (PartProgressionHigherPitch(Part.Alt) != pinfo.n) { return false; } break;
                        case ProgressionPart.Sop: if (PartProgressionHigherPitch(Part.Sop) != pinfo.n) { return false; } break;
                        default:
                            System.Diagnostics.Debug.Assert(false);
                            break;
                        }
                    }
                }
                break;
            case ProgressionDirection.Down:
                if (pinfo.n == 0) {
                    // とにかく下行していればよい
                    switch (pinfo.part0) {
                    case ProgressionPart.Bas: if (PartProgressionHigherInterval(Part.Bas) >= 0) { return false; } break;
                    case ProgressionPart.Ten: if (PartProgressionHigherInterval(Part.Ten) >= 0) { return false; } break;
                    case ProgressionPart.Alt: if (PartProgressionHigherInterval(Part.Alt) >= 0) { return false; } break;
                    case ProgressionPart.Sop: if (PartProgressionHigherInterval(Part.Sop) >= 0) { return false; } break;
                    case ProgressionPart.Upper3:
                        if (PartProgressionHigherInterval(Part.Ten) >= 0) { return false; }
                        if (PartProgressionHigherInterval(Part.Alt) >= 0) { return false; }
                        if (PartProgressionHigherInterval(Part.Sop) >= 0) { return false; }
                        break;
                    }
                } else {
                    if (pinfo.intervalType == ProgressionIntervalType.Degree) {
                        // 度数指定(n度下行)
                        int nInterval = -(pinfo.n - 1);
                        switch (pinfo.part0) {
                        case ProgressionPart.Bas: if (PartProgressionHigherInterval(Part.Bas) != nInterval) { return false; } break;
                        case ProgressionPart.Ten: if (PartProgressionHigherInterval(Part.Ten) != nInterval) { return false; } break;
                        case ProgressionPart.Alt: if (PartProgressionHigherInterval(Part.Alt) != nInterval) { return false; } break;
                        case ProgressionPart.Sop: if (PartProgressionHigherInterval(Part.Sop) != nInterval) { return false; } break;
                        default:
                            System.Diagnostics.Debug.Assert(false);
                            break;
                        }
                    } else {
                        // 半音の数指定(n個下行)
                        switch (pinfo.part0) {
                        case ProgressionPart.Bas: if (PartProgressionHigherPitch(Part.Bas) != -pinfo.n) { return false; } break;
                        case ProgressionPart.Ten: if (PartProgressionHigherPitch(Part.Ten) != -pinfo.n) { return false; } break;
                        case ProgressionPart.Alt: if (PartProgressionHigherPitch(Part.Alt) != -pinfo.n) { return false; } break;
                        case ProgressionPart.Sop: if (PartProgressionHigherPitch(Part.Sop) != -pinfo.n) { return false; } break;
                        default:
                            System.Diagnostics.Debug.Assert(false);
                            break;
                        }
                    }
                }
                break;
            default:
                System.Diagnostics.Debug.Assert(false);
                break;
            }
            return true;
        }

        private bool CheckTwoPartProgression(ProgressionInfo pinfo) {
            System.Diagnostics.Debug.Assert(pinfo.part0 != ProgressionPart.Upper3); 

            switch (pinfo.direction) {
            case ProgressionDirection.Contrary:
                // pinfo.part0とpinfo.part1が反行
                if (pinfo.part1 == ProgressionPart.Upper3) {
                    System.Diagnostics.Debug.Assert(pinfo.part0 == ProgressionPart.Bas);
                    // Basと上3声が反行
                    int basInterval = nowC.GetPitch(Part.Bas).HigherIntervalTo(preC.GetPitch(Part.Bas));
                    if (0 == basInterval) {
                        return false;
                    }

                    if (0 < basInterval) {
                        if (PartProgressionHigherInterval(Part.Ten) >= 0) { return false; }
                        if (PartProgressionHigherInterval(Part.Alt) >= 0) { return false; }
                        if (PartProgressionHigherInterval(Part.Sop) >= 0) { return false; }
                    } else { // basInterval < 0
                        if (PartProgressionHigherInterval(Part.Ten) <= 0) { return false; }
                        if (PartProgressionHigherInterval(Part.Alt) <= 0) { return false; }
                        if (PartProgressionHigherInterval(Part.Sop) <= 0) { return false; }
                    }
                } else {
                    // part0とpart1が反行
                    Part part0 = (Part)pinfo.part0;
                    Part part1 = (Part)pinfo.part1;
                    int interval0 = PartProgressionHigherInterval(part0);
                    int interval1 = PartProgressionHigherInterval(part1);
                    if (0 <= interval0 * interval1) {
                        return false;
                    }
                }
                break;
            case ProgressionDirection.Parallel:
                // pinfo.part0とpinfo.part1が平行
                if (pinfo.part1 == ProgressionPart.Upper3) {
                    System.Diagnostics.Debug.Assert(pinfo.part0 == ProgressionPart.Bas);
                    // Basと上3声が平行
                    int basInterval = nowC.GetPitch(Part.Bas).HigherIntervalTo(preC.GetPitch(Part.Bas));
                    if (0 == basInterval) {
                        return false;
                    }

                    if (0 < basInterval) {
                        if (PartProgressionHigherInterval(Part.Ten) <= 0) { return false; }
                        if (PartProgressionHigherInterval(Part.Alt) <= 0) { return false; }
                        if (PartProgressionHigherInterval(Part.Sop) <= 0) { return false; }
                    } else { // basInterval < 0
                        if (PartProgressionHigherInterval(Part.Ten) >= 0) { return false; }
                        if (PartProgressionHigherInterval(Part.Alt) >= 0) { return false; }
                        if (PartProgressionHigherInterval(Part.Sop) >= 0) { return false; }
                    }
                } else {
                    // part0とpart1が平行
                    Part part0 = (Part)pinfo.part0;
                    Part part1 = (Part)pinfo.part1;
                    int interval0 = PartProgressionHigherInterval(part0);
                    int interval1 = PartProgressionHigherInterval(part1);
                    if (0 >= interval0 * interval1) {
                        return false;
                    }
                }
                break;
            default:
                System.Diagnostics.Debug.Assert(false);
                break;
            }
            return true;
        }

        private bool CheckProgression1(string s, ref int pos, ProgressionInfo pinfo) {
            switch (pinfo.direction) {
            case ProgressionDirection.Up:
            case ProgressionDirection.Down:
            case ProgressionDirection.Sustain:
                if (!CheckOnePartProgression(pinfo)) {
                    return false;
                }
                break;
            case ProgressionDirection.Contrary:
            case ProgressionDirection.Parallel:
                switch (s[pos]) {
                case 'B':
                    System.Diagnostics.Debug.Assert(s[pos + 1] == 'a' && s[pos + 2] == 's');
                    pinfo.part1 = ProgressionPart.Bas;
                    pos += 3;
                    if (!CheckTwoPartProgression(pinfo)) {
                        return false;
                    }
                    break;
                case 'T':
                    System.Diagnostics.Debug.Assert(s[pos + 1] == 'e' && s[pos + 2] == 'n');
                    pinfo.part1 = ProgressionPart.Ten;
                    pos += 3;
                    if (!CheckTwoPartProgression(pinfo)) {
                        return false;
                    }
                    break;
                case 'A':
                    System.Diagnostics.Debug.Assert(s[pos + 1] == 'l' && s[pos + 2] == 't');
                    pinfo.part1 = ProgressionPart.Alt;
                    pos += 3;
                    if (!CheckTwoPartProgression(pinfo)) {
                        return false;
                    }
                    break;
                case 'S':
                    System.Diagnostics.Debug.Assert(s[pos + 1] == 'o' && s[pos + 2] == 'p');
                    pinfo.part1 = ProgressionPart.Sop;
                    pos += 3;
                    if (!CheckTwoPartProgression(pinfo)) {
                        return false;
                    }
                    break;
                case '上':
                    System.Diagnostics.Debug.Assert(s[pos + 1] == '3' && s[pos + 2] == '声');
                    pinfo.part1 = ProgressionPart.Upper3;
                    pos += 3;
                    if (!CheckTwoPartProgression(pinfo)) {
                        return false;
                    }
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
                break;
            default:
                System.Diagnostics.Debug.Assert(false);
                break;
            }

            return true;
        }

        public bool CheckProgression(string s) {
            // ↑Bas→Sop  Bas上行Sop保留(増一度連結も可)
            // ↑上3声
            // ↑H3Bas     Bas半音3つ上げ
            // ↑2Bas      Bas2度上げ
            // 反Bas上3声  Basと上3声が反行
            ProgressionInfo pinfo = new ProgressionInfo();

            for (int pos=0; pos < s.Length; ) {
                switch (s[pos]) {
                case '↑':
                    pinfo.direction = ProgressionDirection.Up;
                    ++pos;
                    break;
                case '↓':
                    pinfo.direction = ProgressionDirection.Down;
                    ++pos;
                    break;
                case '→':
                    pinfo.direction = ProgressionDirection.Sustain;
                    ++pos;
                    break;
                case '反':
                    pinfo.direction = ProgressionDirection.Contrary;
                    ++pos;
                    break;
                case '平':
                    pinfo.direction = ProgressionDirection.Parallel;
                    ++pos;
                    break;
                case 'B':
                    System.Diagnostics.Debug.Assert(s[pos + 1] == 'a' && s[pos + 2] == 's');
                    pinfo.part0 = ProgressionPart.Bas;
                    pos += 3;
                    if (!CheckProgression1(s, ref pos, pinfo)) {
                        return false;
                    }
                    pinfo.Clear();
                    break;
                case 'T':
                    System.Diagnostics.Debug.Assert(s[pos + 1] == 'e' && s[pos + 2] == 'n');
                    pinfo.part0 = ProgressionPart.Ten;
                    pos += 3;
                    if (!CheckProgression1(s, ref pos, pinfo)) {
                        return false;
                    }
                    pinfo.Clear();
                    break;
                case 'A':
                    System.Diagnostics.Debug.Assert(s[pos + 1] == 'l' && s[pos + 2] == 't');
                    pinfo.part0 = ProgressionPart.Alt;
                    pos += 3;
                    if (!CheckProgression1(s, ref pos, pinfo)) {
                        return false;
                    }
                    pinfo.Clear();
                    break;
                case 'S':
                    System.Diagnostics.Debug.Assert(s[pos + 1] == 'o' && s[pos + 2] == 'p');
                    pinfo.part0 = ProgressionPart.Sop;
                    pos += 3;
                    if (!CheckProgression1(s, ref pos, pinfo)) {
                        return false;
                    }
                    pinfo.Clear();
                    break;
                case '上':
                    System.Diagnostics.Debug.Assert(s[pos + 1] == '3' && s[pos + 2] == '声');
                    pinfo.part0 = ProgressionPart.Upper3;
                    pos += 3;
                    if (!CheckProgression1(s, ref pos, pinfo)) {
                        return false;
                    }
                    pinfo.Clear();
                    break;
                case 'H':
                    pinfo.intervalType = ProgressionIntervalType.Semitone;
                    ++pos;
                    break;
                case '1': pinfo.n = 1; ++pos; break;
                case '2': pinfo.n = 2; ++pos; break;
                case '3': pinfo.n = 3; ++pos; break;
                case '4': pinfo.n = 4; ++pos; break;
                case '5': pinfo.n = 5; ++pos; break;
                case '6': pinfo.n = 6; ++pos; break;
                case '7': pinfo.n = 7; ++pos; break;
                case '8': pinfo.n = 8; ++pos; break;
                case '9': pinfo.n = 9; ++pos; break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
            }
            return true;
        }
    }
}
