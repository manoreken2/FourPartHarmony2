using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace FourPartHarmony2
{
    /// <summary>
    /// 転回形種類(Inversion)
    /// </summary>
    public enum Inversion
    {
        Unspecified = -1, ///< 特に定めない。
        根音 = 0,    //< Basの場合基本形
        第3音 = 1, //< Basの場合第1転回形
        第5音 = 2, //< Basの場合第2転回形
        第7音 = 3, //< Basの場合第3転回形
        第9音 = 4, //< Basの場合第4転回形 1オクターブ +2度
        第4音 = 5, //< ? 1オクターブ +4度 (微妙なバランスによって成り立っているので順番を入れ替えないこと!)
        第6音 = 6, //< ? 1オクターブ +6度 同上
    }

    /// <summary>
    /// 2つの音が形成する音程
    /// </summary>
    public enum IntervalType
    {
        Invalid = -1,      //< 無効
        PerfectUnison,     //< 1度(調号と臨時記号が同じ場合完全1度)
        DiminishedSecond,  //< 減2度
        DoublyDiminishedSecond,  //< 重減2度
        MinorSecond,       //< 短2度
        AugmentedUnison,   //< 増1度
        DoublyAugmentedUnison, //< 重増1度

        MajorSecond,       //< 長2度
        DiminishedThird,   //< 減3度
        DoublyDiminishedThird,   //< 重減3度
        MinorThird,        //< 短3度
        AugmentedSecond,   //< 増2度
        DoublyAugmentedSecond,   //< 重増2度

        MajorThird,        //< 長3度
        DiminishedFourth,  //< 減4度
        DoublyDiminishedFourth,  //< 重減4度

        PerfectFourth,     //< 完全4度
        AugmentedThird,    //< 増3度
        DoublyAugmentedThird,    //< 重増3度

        AugmentedFourth,   //< 3全音(増4度)
        DoublyAugmentedFourth,   //< 重増4度

        DiminishedFifth,   //< 減5度
        DoublyDiminishedFifth,   //< 重減5度
        PerfectFifth,      //< 完全5度

        DiminishedSixth,   //< 減6度
        DoublyDiminishedSixth,   //< 重減6度
        MinorSixth,        //< 短6度
        AugmentedFifth,    //< 増5度
        DoublyAugmentedFifth,    //< 重増5度

        MajorSixth,        //< 長6度
        DiminishedSeventh, //< 減7度
        DoublyDiminishedSeventh, //< 重減7度

        MinorSeventh,      //< 短7度
        AugmentedSixth,    //< 増6度
        DoublyAugmentedSixth,    //< 重増6度

        MajorSeventh,      //< 長7度
        DiminishedOctave,  //< 減8度
        DoublyDiminishedOctave, //< 重減8度

        PerfectOctave,     //< 8度(調号と臨時記号が同じ場合完全8度)

        AugmentedSeventh,   //< 増7度
        DoublyAugmentedSeventh   //< 重増7度
    }

    /// <summary>
    /// 音名LetterNameと音度Degree。不変クラス
    /// </summary>
    public struct LnDeg
    {
        private readonly LetterName letterName;
        private readonly int        degree;     //< 音度。1=主音。1～9が音度を表す。0の場合音度情報が無意味。-1の場合無効。

        public LnDeg(LN aLetterName, int aDegree) {
            letterName = new LetterName(aLetterName);
            degree = aDegree;
        }

        /// <summary>
        /// 無効な状況を表す。
        /// </summary>
        public static readonly LnDeg INVALID = new LnDeg(LN.C, -1);

        public LetterName LetterName {
            get { return letterName; }
        }

        public int Degree {
            get { return degree; }
        }
    }

    /// <summary>
    /// 音名、音度、転回形
    /// </summary>
    public struct LnDegInversion
    {
        private readonly LnDeg ld;
        private readonly Inversion inversion;

        public LnDegInversion(LN ln, int degree, Inversion inversion) {
            this.ld = new LnDeg(ln, degree);
            this.inversion = inversion;
        }
        public LnDegInversion(LnDeg ld, Inversion inversion) {
            this.ld = ld;
            this.inversion = inversion;
        }
        /// <summary>
        /// C#4.0になったらデフォルト引数でまとめる。
        /// </summary>
        public LnDegInversion(LN ln) {
            this.ld = new LnDeg(ln, 0);
            this.inversion = Inversion.Unspecified;
        }

        /// <summary>
        /// 無効な状況を表す。
        /// </summary>
        public static readonly LnDegInversion INVALID = new LnDegInversion(LN.C, -1, Inversion.Unspecified);

        public LnDeg LnDeg { get { return ld; } }
        public LetterName LetterName { get { return ld.LetterName; } }
        public int Degree { get { return ld.Degree; } }
        public Inversion Inversion { get { return inversion; } }
    }

    /// <summary>
    /// シリアライズするピッチ情報
    /// </summary>
    public class PitchSave
    {
        [XmlAttribute] public LN        letterName;
        [XmlAttribute] public int       degree;
        [XmlAttribute] public Inversion inversion;
        [XmlAttribute] public int       octave;

        public PitchSave() {
        }

        public PitchSave(Pitch pitch) {
            letterName = pitch.LetterName.LN;
            degree     = pitch.Degree;
            inversion  = pitch.Inversion;
            octave     = pitch.Octave;
        }

        public Pitch ToPitch() {
            return new Pitch(new LnDegInversion(letterName, degree, inversion), octave);
        }
    }

    /// <summary>
    /// ピッチ(音名＋オクターブ＋その調における音度1～9＋転回形)
    /// </summary>
    public struct Pitch
    {
        private readonly LnDegInversion  ldi;
        private readonly int octave;
        public LnDegInversion LnDegInversion { get { return ldi; } }
        public LnDeg LnDeg { get { return ldi.LnDeg; } }
        public LetterName LetterName { get { return ldi.LetterName; } }
        /// <summary>
        /// 音度。1=主音。1～9が音度を表す。0の場合音度情報が無意味。-1の場合無効。
        /// </summary>
        public int Degree { get { return ldi.Degree; } }
        public Inversion Inversion { get { return ldi.Inversion; } }
        public int Octave { get { return octave; } }

        static Dictionary<KeyValuePair<int, int>, IntervalType> intervalTypeTable
            = new Dictionary<KeyValuePair<int, int>, IntervalType>();

        static Pitch() {
            intervalTypeTable.Add(new KeyValuePair<int, int>(0, 0), IntervalType.PerfectUnison);
            intervalTypeTable.Add(new KeyValuePair<int, int>(1, 0), IntervalType.DiminishedSecond);

            intervalTypeTable.Add(new KeyValuePair<int, int>(0, 1), IntervalType.AugmentedUnison);
            intervalTypeTable.Add(new KeyValuePair<int, int>(1, 1), IntervalType.MinorSecond);
            intervalTypeTable.Add(new KeyValuePair<int, int>(2, 1), IntervalType.DoublyDiminishedThird);

            intervalTypeTable.Add(new KeyValuePair<int, int>(0, 2), IntervalType.DoublyAugmentedUnison);
            intervalTypeTable.Add(new KeyValuePair<int, int>(1, 2), IntervalType.MajorSecond);
            intervalTypeTable.Add(new KeyValuePair<int, int>(2, 2), IntervalType.DiminishedThird);

            intervalTypeTable.Add(new KeyValuePair<int, int>(1, 3), IntervalType.AugmentedSecond);
            intervalTypeTable.Add(new KeyValuePair<int, int>(2, 3), IntervalType.MinorThird);
            intervalTypeTable.Add(new KeyValuePair<int, int>(3, 3), IntervalType.DoublyDiminishedFourth);

            intervalTypeTable.Add(new KeyValuePair<int, int>(1, 4), IntervalType.DoublyAugmentedSecond);
            intervalTypeTable.Add(new KeyValuePair<int, int>(2, 4), IntervalType.MajorThird);
            intervalTypeTable.Add(new KeyValuePair<int, int>(3, 4), IntervalType.DiminishedFourth);

            intervalTypeTable.Add(new KeyValuePair<int, int>(2, 5), IntervalType.AugmentedThird);
            intervalTypeTable.Add(new KeyValuePair<int, int>(3, 5), IntervalType.PerfectFourth);
            intervalTypeTable.Add(new KeyValuePair<int, int>(4, 5), IntervalType.DoublyDiminishedFifth);

            intervalTypeTable.Add(new KeyValuePair<int, int>(2, 6), IntervalType.DoublyAugmentedThird);
            intervalTypeTable.Add(new KeyValuePair<int, int>(3, 6), IntervalType.AugmentedFourth);
            intervalTypeTable.Add(new KeyValuePair<int, int>(4, 6), IntervalType.DiminishedFifth);
            intervalTypeTable.Add(new KeyValuePair<int, int>(5, 6), IntervalType.DoublyDiminishedSixth);

            intervalTypeTable.Add(new KeyValuePair<int, int>(3, 7), IntervalType.DoublyAugmentedFourth);
            intervalTypeTable.Add(new KeyValuePair<int, int>(4, 7), IntervalType.PerfectFifth);
            intervalTypeTable.Add(new KeyValuePair<int, int>(5, 7), IntervalType.DiminishedSixth);

            intervalTypeTable.Add(new KeyValuePair<int, int>(4, 8), IntervalType.AugmentedFifth);
            intervalTypeTable.Add(new KeyValuePair<int, int>(5, 8), IntervalType.MinorSixth);
            intervalTypeTable.Add(new KeyValuePair<int, int>(6, 8), IntervalType.DoublyDiminishedSeventh);

            intervalTypeTable.Add(new KeyValuePair<int, int>(4, 9), IntervalType.DoublyAugmentedFifth);
            intervalTypeTable.Add(new KeyValuePair<int, int>(5, 9), IntervalType.MajorSixth);
            intervalTypeTable.Add(new KeyValuePair<int, int>(6, 9), IntervalType.DiminishedSeventh);

            intervalTypeTable.Add(new KeyValuePair<int, int>(5, 10), IntervalType.AugmentedSixth);
            intervalTypeTable.Add(new KeyValuePair<int, int>(6, 10), IntervalType.MinorSeventh);
            intervalTypeTable.Add(new KeyValuePair<int, int>(7, 10), IntervalType.DoublyDiminishedOctave);
            intervalTypeTable.Add(new KeyValuePair<int, int>(0, 10), IntervalType.DoublyDiminishedOctave);

            intervalTypeTable.Add(new KeyValuePair<int, int>(5, 11), IntervalType.DoublyAugmentedSixth);
            intervalTypeTable.Add(new KeyValuePair<int, int>(6, 11), IntervalType.MajorSeventh);
            intervalTypeTable.Add(new KeyValuePair<int, int>(7, 11), IntervalType.DiminishedOctave);
            intervalTypeTable.Add(new KeyValuePair<int, int>(0, 11), IntervalType.DiminishedOctave);
            intervalTypeTable.Add(new KeyValuePair<int, int>(8, 11), IntervalType.DoublyDiminishedSecond);
            intervalTypeTable.Add(new KeyValuePair<int, int>(1, 11), IntervalType.DoublyDiminishedSecond);

            intervalTypeTable.Add(new KeyValuePair<int, int>(6, 12), IntervalType.AugmentedSeventh);
            intervalTypeTable.Add(new KeyValuePair<int, int>(6, 0), IntervalType.AugmentedSeventh);
            intervalTypeTable.Add(new KeyValuePair<int, int>(7, 12), IntervalType.PerfectOctave);

            intervalTypeTable.Add(new KeyValuePair<int, int>(6, 13), IntervalType.DoublyAugmentedSeventh);
            intervalTypeTable.Add(new KeyValuePair<int, int>(6, 1), IntervalType.DoublyAugmentedSeventh);
        }

        public Pitch(Pitch rhs) {
            ldi = rhs.ldi;
            octave = rhs.octave;
        }

        public Pitch(LnDegInversion aLdi, int aOctave) {
            ldi = aLdi;
            octave = aOctave;
        }

        public LN NaturalLetterName() {
            return LetterName.NaturalLetterName();
        }

        /// <summary>
        /// 1オクターブ高い同一ピッチの音を戻す。
        /// </summary>
        /// <returns>1オクターブ高い同一ピッチの音</returns>
        public Pitch NextOctave() {
            return new Pitch(ldi, octave + 1);
        }

        public bool IsFlat() {
            return ldi.LetterName.IsFlat();
        }

        public bool IsSharp() {
            return ldi.LetterName.IsSharp();
        }
        public bool IsNatural() {
            return ldi.LetterName.IsNatural();
        }
        public bool IsDoubleSharp() {
            return ldi.LetterName.IsDoubleSharp();
        }
        public bool IsDoubleFlat() {
            return ldi.LetterName.IsDoubleFlat();
        }

        private static Pitch BAS_LOWEST  = new Pitch(new LnDegInversion(LN.ES), 2);
        private static Pitch BAS_HIGHEST = new Pitch(new LnDegInversion(LN.E), 4); //< 別間課題の実施II巻補充課題8-6
        private static Pitch TEN_LOWEST  = new Pitch(new LnDegInversion(LN.C), 3);
        private static Pitch TEN_HIGHEST = new Pitch(new LnDegInversion(LN.A), 4);
        private static Pitch ALT_LOWEST  = new Pitch(new LnDegInversion(LN.GES), 3);
        private static Pitch ALT_HIGHEST = new Pitch(new LnDegInversion(LN.E), 5);
        private static Pitch SOP_LOWEST  = new Pitch(new LnDegInversion(LN.C), 4);
        private static Pitch SOP_HIGHEST = new Pitch(new LnDegInversion(LN.A), 5);

        public static Pitch INVALID_PITCH = new Pitch(new LnDegInversion(LN.C), 0);

        public static int BasLowestOctave() { return BAS_LOWEST.Octave; }
        public static int BasHighestOctave() { return BAS_HIGHEST.Octave; }
        public static int TenLowestOctave() { return TEN_LOWEST.Octave; }
        public static int TenHighestOctave() { return TEN_HIGHEST.Octave; }
        public static int AltLowestOctave() { return ALT_LOWEST.Octave; }
        public static int AltHighestOctave() { return ALT_HIGHEST.Octave; }
        public static int SopLowestOctave() { return SOP_LOWEST.Octave; }
        public static int SopHighestOctave() { return SOP_HIGHEST.Octave; }

        /// <summary>
        /// 音程がパートの音域の上限を超えているかどうか。
        /// </summary>
        /// <returns>true: 音域の上限を超えている。</returns>
        public bool IsAboveTheRange(Part part) {
            switch (part) {
            case Part.Bas:
                if (0 < HigherPitchTo(BAS_HIGHEST) || 0 < HigherIntervalTo(BAS_HIGHEST)) {
                    return true;
                }
                break;
            case Part.Ten:
                if (0 < HigherPitchTo(TEN_HIGHEST) || 0 < HigherIntervalTo(TEN_HIGHEST)) {
                    return true;
                }
                break;
            case Part.Alt:
                if (0 < HigherPitchTo(ALT_HIGHEST) || 0 < HigherIntervalTo(ALT_HIGHEST)) {
                    return true;
                }
                break;
            case Part.Sop:
                if (0 < HigherPitchTo(SOP_HIGHEST) || 0 < HigherIntervalTo(SOP_HIGHEST)) {
                    return true;
                }
                break;
            default:
                System.Diagnostics.Debug.Assert(false);
                break;
            }
            return false;
        }

        /// <summary>
        /// 音程がパートの音域の下限を下回っているかどうか。
        /// </summary>
        /// <returns>true: 音域の下限を下回っている。</returns>
        public bool IsBelowTheRange(Part part) {
            switch (part) {
            case Part.Bas:
                if (HigherPitchTo(BAS_LOWEST) < 0) {
                    return true;
                }
                break;
            case Part.Ten:
                if (HigherPitchTo(TEN_LOWEST) < 0) {
                    return true;
                }
                break;
            case Part.Alt:
                if (HigherPitchTo(ALT_LOWEST) < 0) {
                    return true;
                }
                break;
            case Part.Sop:
                if (HigherPitchTo(SOP_LOWEST) < 0) {
                    return true;
                }
                break;
            default:
                System.Diagnostics.Debug.Assert(false);
                break;
            }
            return false;
        }

        public bool IsWithinTheRange(Part part) {
            // 範囲の上限以下、しかも、範囲の下限以上の場合、範囲内。
            return (!IsAboveTheRange(part)) && (!IsBelowTheRange(part));
        }

        /// <summary>
        /// オクターブを考慮したC-1からの半音の数。
        /// </summary>
        public int GetMidiPitchValue() {
            return (octave + 1) * 12 + LetterName.ToFreqIndex();
        }

        /// <summary>
        /// MIDIで発音したときのピッチを比較する。
        /// 正: rhsのほうが低い 数字は半音の数。
        /// 負: rhsのほうが高い 数字は半音の数。
        /// 例:  ten.HigherPitchTo(bas) > 0
        /// </summary>
        public int HigherPitchTo(Pitch rhs) {
            return GetMidiPitchValue() - rhs.GetMidiPitchValue();
        }

        /// <summary>
        /// rhsとの半音の数(絶対値)。0==同音、12==1オクターブ
        /// </summary>
        /// <param name="rhs">比較対象。</param>
        public int AbsNumberOfSemitonesWith(Pitch rhs) {
            return Math.Abs(HigherPitchTo(rhs));
        }

        /// <summary>
        /// Cとの「度」(同度==0) 0～6
        /// ただしオクターブは考慮していない
        /// </summary>
        public int NoteDistanceFromC() {
            return LetterName.NoteDistanceFromC();
        }

        /// <summary>
        /// C0との絶対音程。この値を使用して他のピッチと比較する場合、同度=0、1オクターブ=7
        /// </summary>
        public int GetIntervalNumberFromC0() {
            return NoteDistanceFromC() + octave * 7;
        }

        /// <summary>
        /// 楽譜の上でrhsの上に来るか下に来るか。
        /// 正: rhsのほうが低い 数字は度。
        /// 負: rhsのほうが高い 数字は度。
        /// 例:  ten.HigherIntervalTo(bas) > 0
        /// 注：この比較は(フラットやシャープを考慮しているが)あくまで楽譜の上で
        /// 上か下かを判断するものでCes4よりもHis3のほうが低いと判断する。
        /// </summary>
        /// <param name="rhs">比較対象のピッチ。</param>
        public int HigherIntervalTo(Pitch rhs) {
            return GetIntervalNumberFromC0() - rhs.GetIntervalNumberFromC0();
        }

        /// <summary>
        /// rhsとの音程(1度=0) 絶対値。同度=0、1オクターブ=7
        /// </summary>
        /// <returns>
        /// 0: 完全1度、増一度、重増一度
        /// 1: 短二度、長二度、減二度、増二度、重増二度
        /// 2: 短3度、長3度、減3度、重減3度、増3度、重増3度
        /// 3: 完全4度、増4度、減4度、…
        /// 4: 5度
        /// 7: 8度
        /// 8: 9度
        /// 11: 12度
        /// </returns>
        public int AbsIntervalNumberWith(Pitch rhs) {
            return Math.Abs(HigherIntervalTo(rhs));
        }

        public IntervalType GetIntervalType(Pitch rhs) {
            int intervalNumber = AbsIntervalNumberWith(rhs) % 7;
            int numberOfSemitones = AbsNumberOfSemitonesWith(rhs) % 12;
            return intervalTypeTable[new KeyValuePair<int, int>(intervalNumber, numberOfSemitones)];
        }
    }
}
