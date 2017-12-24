using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace FourPartHarmony2
{
    /// <summary>
    /// 和音の種類。I、II、III、IV、…
    /// </summary>
    public enum CD
    {
        I   = 0,
        II  = 1,
        III = 2,
        IV  = 3,
        V   = 4,
        VI  = 5,
        VII = 6,
        // II度のI=7
        // III度のI=14
        // IV度のI=21
        // V度のI=28
        V_V = 32,
    }

    /// <summary>
    /// Close: 密集配置
    /// Open: 開離配置
    /// Octave: Oct配置
    /// 注:順番を並び替えたらいけない！
    /// </summary>
    public enum PositionOfAChord
    {
        密,
        開,
        Oct
    }

    /// <summary>
    /// Triad: 三和音
    /// Seventh: 7の和音
    /// Ninth: 9の和音
    /// </summary>
    public enum NumberOfNotes
    {
        Triad,
        Seventh,
        Ninth
    }

    /// <summary>
    /// 省略の種類
    /// None: 省略なし
    /// First: 根音省略
    /// Fifth: 第5音省略
    /// </summary>
    public enum Omission
    {
        None,
        First,
       // Fifth
    }

    /// <summary>
    /// パート。バス、テナー、アルト、ソプラノの順。
    /// </summary>
    public enum Part
    {
        Invalid = -1,
        Bas,
        Ten,
        Alt,
        Sop
    }

    /// <summary>
    /// 最適、良、可能、まれ、許、なるべく避けよ、避けよ、不良、禁、リストに載せない
    /// </summary>
    public enum VerdictValue
    {
        Delist,
        Prohibited,
        Wrong,
        Avoid,
        NotSoGood,
        Rare,
        Acceptable,
        Okay,
        Good,
        Best,
        Info
    }

    /// <summary>
    /// 和音の機能。T D S
    /// </summary>
    public enum FunctionType
    {
        Tonic,
        Dominant,
        Subdominant,
        Unknown
    }

    /// <summary>
    /// カデンツの種類。K1=TDT K2=TSDT K3=TST
    /// </summary>
    public enum CadenceType
    {
        Unspecified,
        K1,
        K2,
        K3,
        IncompleteK1, //< 不完全カデンツK1 (不完全終止imperfect cadenceではない!)
        IncompleteK2, //< 不完全カデンツK2 (不完全終止imperfect cadenceではない!)
        IncompleteK3, //< 不完全カデンツK3 (不完全終止imperfect cadenceではない!)
    }

    /// <summary>
    /// 終止型
    /// </summary>
    public enum TerminationType
    {
        None,         //< 終止型でない
        Perfect,      //< 全終止
        Plagal,       //< 変終止
        Deceptive,    //< 偽終止
        Half          //< 半終止
    }

    public struct TerminationInfo {
        public TerminationType terminationType;
        public TerminationType TerminationType {
            get { return terminationType; }
        }

        public TerminationInfo(TerminationType t) {
            terminationType = t;
        }
        public string GetString() {
            switch (terminationType) {
            case TerminationType.None: return "非終止";
            case TerminationType.Perfect: return "全終止";
            case TerminationType.Plagal: return "変終止";
            case TerminationType.Deceptive: return "偽終止";
            case TerminationType.Half: return "半終止";
            default: System.Diagnostics.Debug.Assert(false);
                return "";
            }
        }
        public string GetStringShort() {
            switch (terminationType) {
            case TerminationType.None: return "非";
            case TerminationType.Perfect: return "全";
            case TerminationType.Plagal: return "変";
            case TerminationType.Deceptive: return "偽";
            case TerminationType.Half: return "半";
            default: System.Diagnostics.Debug.Assert(false);
                return "";
            }
        }
    }

    /// <summary>
    /// 変位和音 alteration
    /// </summary>
    public enum AlterationType
    {
        None,   //< 変位なし
        Raised, //< 上方変位
        Lowered, //< 下方変位
        Dorian,  //< ドリアIV
        Naples,  //< ナポリII
    }

    public enum AddedToneType
    {
        None,    //< 付加なし
        Six,     //< 付加6
        SixFour, //< 付加4-6
    }

    /// <summary>
    /// 和音の種別
    /// </summary>
    public enum ChordConstructionType {
        長3和音,
        短3和音,
        減3和音,
        増3和音,
        長7の和音,
        属7の和音,
        短7の和音,
        減7の和音,
        減57の和音,
        長9の和音,
        短9の和音,
        変位和音,
    };

    // 和音が不良な場合、理由
    public struct Verdict {
        public VerdictValue  value;
        public VerdictReason reason;
        public Part part0;
        public Part part1;
        public int ordinal0;
        public string str0;
        public string str1;
        public string str2;
        public string str3;
        public string str4;
        public string str5;

        public Verdict(Verdict rhs) {
            this.value = rhs.value;
            this.reason = rhs.reason;
            this.part0 = rhs.part0;
            this.part1 = rhs.part1;
            this.ordinal0 = rhs.ordinal0;
            this.str0 = rhs.str0;
            this.str1 = rhs.str1;
            this.str2 = rhs.str2;
            this.str3 = rhs.str3;
            this.str4 = rhs.str4;
            this.str5 = rhs.str5;
        }

        public Verdict(VerdictValue value, VerdictReason reason) {
            this.value = value;
            this.reason = reason;
            this.part0 = Part.Invalid;
            this.part1 = Part.Invalid;
            this.ordinal0 = 0;
            this.str0 = string.Empty;
            this.str1 = string.Empty;
            this.str2 = string.Empty;
            this.str3 = string.Empty;
            this.str4 = string.Empty;
            this.str5 = string.Empty;
        }

        public Verdict(VerdictValue value, VerdictReason reason, int ordinal0) {
            this.value = value;
            this.reason = reason;
            this.part0 = Part.Invalid;
            this.part1 = Part.Invalid;
            this.ordinal0 = ordinal0;
            this.str0 = string.Empty;
            this.str1 = string.Empty;
            this.str2 = string.Empty;
            this.str3 = string.Empty;
            this.str4 = string.Empty;
            this.str5 = string.Empty;
        }

        public Verdict(VerdictValue value, VerdictReason reason, Part part0) {
            this.value = value;
            this.reason = reason;
            this.part0 = part0;
            this.part1 = Part.Invalid;
            this.ordinal0 = 0;
            this.str0 = string.Empty;
            this.str1 = string.Empty;
            this.str2 = string.Empty;
            this.str3 = string.Empty;
            this.str4 = string.Empty;
            this.str5 = string.Empty;
        }

        public Verdict(VerdictValue value, VerdictReason reason, Part part0, Part part1) {
            this.value = value;
            this.reason = reason;
            this.part0 = part0;
            this.part1 = part1;
            this.ordinal0 = 0;
            this.str0 = string.Empty;
            this.str1 = string.Empty;
            this.str2 = string.Empty;
            this.str3 = string.Empty;
            this.str4 = string.Empty;
            this.str5 = string.Empty;
        }
        public Verdict(VerdictValue value, VerdictReason reason, string str0) {
            this.value = value;
            this.reason = reason;
            this.part0 = Part.Invalid;
            this.part1 = Part.Invalid;
            this.ordinal0 = 0;
            this.str0 = str0;
            this.str1 = string.Empty;
            this.str2 = string.Empty;
            this.str3 = string.Empty;
            this.str4 = string.Empty;
            this.str5 = string.Empty;
        }
        public Verdict(VerdictValue value, VerdictReason reason, string str0, string str1) {
            this.value = value;
            this.reason = reason;
            this.part0 = Part.Invalid;
            this.part1 = Part.Invalid;
            this.ordinal0 = 0;
            this.str0 = str0;
            this.str1 = str1;
            this.str2 = string.Empty;
            this.str3 = string.Empty;
            this.str4 = string.Empty;
            this.str5 = string.Empty;
        }
        public Verdict(VerdictValue value, VerdictReason reason, string str0, string str1, string str2) {
            this.value = value;
            this.reason = reason;
            this.part0 = Part.Invalid;
            this.part1 = Part.Invalid;
            this.ordinal0 = 0;
            this.str0 = str0;
            this.str1 = str1;
            this.str2 = str2;
            this.str3 = string.Empty;
            this.str4 = string.Empty;
            this.str5 = string.Empty;
        }
        public Verdict(VerdictValue value, VerdictReason reason, string str0, string str1, string str2, string str3) {
            this.value = value;
            this.reason = reason;
            this.part0 = Part.Invalid;
            this.part1 = Part.Invalid;
            this.ordinal0 = 0;
            this.str0 = str0;
            this.str1 = str1;
            this.str2 = str2;
            this.str3 = str3;
            this.str4 = string.Empty;
            this.str5 = string.Empty;
        }
        public Verdict(VerdictValue value, VerdictReason reason, string str0, string str1, string str2, string str3, string str4) {
            this.value = value;
            this.reason = reason;
            this.part0 = Part.Invalid;
            this.part1 = Part.Invalid;
            this.ordinal0 = 0;
            this.str0 = str0;
            this.str1 = str1;
            this.str2 = str2;
            this.str3 = str3;
            this.str4 = str4;
            this.str5 = string.Empty;
        }
        public Verdict(VerdictValue value, VerdictReason reason, string str0, string str1, string str2, string str3, string str4, string str5) {
            this.value = value;
            this.reason = reason;
            this.part0 = Part.Invalid;
            this.part1 = Part.Invalid;
            this.ordinal0 = 0;
            this.str0 = str0;
            this.str1 = str1;
            this.str2 = str2;
            this.str3 = str3;
            this.str4 = str4;
            this.str5 = str5;
        }
    }

    /// <summary>
    /// 和音の種類を特定する情報
    /// </summary>
    public struct ChordType
    {
        public MusicKey         musicKey;         // 調(I調の調)
        public KeyRelation      keyRelation;      // この和音のI調との関係(内属記号)
        public CD               chordDegree;      // 和音の音度
        public PositionOfAChord positionOfAChord; // 配置　密集、開離、OCT
        public NumberOfNotes    numberOfNotes;    // 三和音、7の和音、9の和音
        public Omission         omission;         // 省略なし、根音省略、第5音省略
        public Inversion        bassInversion;    // 転回なし、第1転回、…
        public TerminationType  termination;      // 終止なし、全終止、…
        public bool             is準固有;         // 準固有
        public bool             has固有VII;       // 固有VII
        public AlterationType   alteration;       // 上方変位、下方変位、…
        public AddedToneType    addedTone;        // 付加和音
        public ChordConstructionType construction; // 和音の種別
        /// <summary>
        /// この和音の調(内部調)が長調か。 (準固有和音は長調と戻るので注意)
        /// </summary>
        public bool Is内部調は長調() {
            return new MusicKeyInfo(musicKey, keyRelation).IsMajor();
        }

        /// <summary>
        /// この和音の調(内部調)が短調か。長調のNOTで判定できるが、便利なので。(準固有和音は長調と戻るので注意)
        /// </summary>
        public bool Is内部調は短調() {
            return new MusicKeyInfo(musicKey, keyRelation).IsMinor();
        }

        /// <summary>
        /// この曲の主調は長調か？
        /// </summary>
        public bool Is主調は長調() {
            return new MusicKeyInfo(musicKey, KeyRelation.I調).IsMajor();
        }

        /// <summary>
        /// この和音が実際に属する調。主調ではなく内部調の調。
        /// </summary>
        public MusicKey ActualKey() {
            MusicKeyInfo mki = new MusicKeyInfo(musicKey, keyRelation);
            return mki.InternalKey;
        }
    }

    /// <summary>
    /// 和音生成時に同じ和音を削除するのに使用。
    /// </summary>
    public class ChordEqualityComparer : IEqualityComparer<Chord>
    {
        public ChordEqualityComparer() {
        }

        public bool Equals(Chord x, Chord y) {
            return x.PitchWeightedAccumulate() == y.PitchWeightedAccumulate();
        }

        public int GetHashCode(Chord x) {
            return x.PitchWeightedAccumulate();
        }
    }

    /// <summary>
    /// 和音一覧表示時の並び順を決めるのに使用。
    /// </summary>
    public class ChordComparer : IComparer<Chord>
    {
        Chord prevChord;
        public ChordComparer(Chord prevChord) {
            this.prevChord = prevChord;
        }

        /// <summary>
        /// xがyよりも良い、または低い場合マイナス
        /// </summary>
        public int Compare(Chord x, Chord y) {
            // 最適、良、許、なるべく避けよ、避けよ、不良、禁、和音リストに載せない →大きい
            int diffVerdict = y.Verdict - x.Verdict;
            if (diffVerdict != 0) {
                return diffVerdict;
            }

            if (x.IsStandard && !y.IsStandard) {
                return -1;
            }
            if (!x.IsStandard && y.IsStandard) {
                return 1;
            }

            if (null != prevChord) {
                // 先行和音がある場合、各パートの進行が少なければ少ないほどよい。
                int diffProgression =
                    x.AllPartProgressionDiffAccumulate(prevChord) - 
                    y.AllPartProgressionDiffAccumulate(prevChord);
                if (diffProgression != 0) {
                    return diffProgression;
                }
            }

            return x.PitchWeightedAccumulate() - y.PitchWeightedAccumulate();
        }
    }

    /// <summary>
    /// シリアライズする和音情報
    /// </summary>
    public class ChordSave
    {
        [XmlAttribute] public MusicKey musicKey;
        [XmlAttribute] public KeyRelation chordKey;
        [XmlAttribute] public bool isStandard;
        [XmlAttribute] public CD chordDegree;
        [XmlAttribute] public PositionOfAChord positionOfAChord;
        [XmlAttribute] public NumberOfNotes numberOfNotes;
        [XmlAttribute] public Omission omission;
        [XmlAttribute] public Inversion basInversion;
        [XmlAttribute] public Inversion sopInversion;
        [XmlAttribute] public TerminationType termination;
        [XmlAttribute] public bool is準固有;
        [XmlAttribute] public bool has固有VII;
        [XmlAttribute] public AlterationType alteration;
        [XmlAttribute] public AddedToneType addedTone;
        
        public PitchSave bas;
        public PitchSave ten;
        public PitchSave alt;
        public PitchSave sop;
        public ChordSave() {
        }

        public ChordSave(Chord c) {
            musicKey = c.MusicKey;
            chordKey = c.KeyRelation;
            isStandard = c.IsStandard;
            chordDegree = c.ChordDegree;
            positionOfAChord = c.PositionOfAChord;
            numberOfNotes = c.NumberOfNotes;
            omission = c.Omission;
            basInversion = c.Inversion;
            sopInversion = c.SopInversion;
            termination  = c.TerminationType;
            is準固有 = c.Is準固有和音;
            has固有VII = c.Has固有VII;
            alteration = c.AlterationType;
            addedTone = c.AddedTone;

            bas = new PitchSave(c.GetPitch(Part.Bas));
            ten = new PitchSave(c.GetPitch(Part.Ten));
            alt = new PitchSave(c.GetPitch(Part.Alt));
            sop = new PitchSave(c.GetPitch(Part.Sop));
        }

        public Chord ToChord(int fileVersion) {
            ChordType ct = new ChordType();
            ct.bassInversion    = basInversion;
            ct.chordDegree      = chordDegree;
            ct.musicKey         = musicKey;
            ct.keyRelation         = chordKey;
            ct.numberOfNotes = numberOfNotes;
            ct.omission         = omission;
            ct.positionOfAChord = positionOfAChord;
            ct.termination      = termination;
            ct.is準固有         = is準固有;
            ct.has固有VII       = has固有VII;
            ct.alteration       = alteration;
            ct.addedTone        = addedTone;

            if (fileVersion < 2) {
                // 旧ファイル形式。ドリア、ナポリの指定形式が古い(上変、下変で指定している)
                if (ct.chordDegree == CD.II && ct.alteration == AlterationType.Lowered) {
                    ct.alteration = AlterationType.Naples;
                }
                if (ct.chordDegree == CD.IV && ct.alteration == AlterationType.Raised) {
                    ct.alteration = AlterationType.Dorian;
                }
            }

            return new Chord(ct, isStandard, bas.ToPitch(), ten.ToPitch(), alt.ToPitch(), sop.ToPitch());
        }
    }

    /// <summary>
    /// 限定進行音種類
    /// </summary>
    public enum 限定進行音Type {
        導音,
        上行限定進行音,
        下行限定進行音
    }

    /// <summary>
    /// 限定進行音情報。
    /// </summary>
    public struct 限定進行音Info {
        public Part part;
        public 限定進行音Type type;

        public 限定進行音Info(Part part, 限定進行音Type type) {
            this.part = part;
            this.type = type;
        }
    }

    /// <summary>
    /// 和音
    /// </summary>
    public partial class Chord
    {
        private ChordType ct;
        private readonly bool standard;
        private readonly Pitch [] pitches;
        private VerdictValue verdictValue;
        private FunctionType functionType;         // 和音機能。
        public CadenceType CadenceType { get; set; }
        public AlterationType AlterationType { get { return ct.alteration; } }
        public AddedToneType AddedTone { get { return ct.addedTone; } }

        private List<Verdict> verdicts = new List<Verdict>();

        public int NumOfVerdicts { get { return verdicts.Count; } }

        /// <summary>
        /// 主調の調
        /// </summary>
        public MusicKey MusicKey { get { return ct.musicKey; } }
        /// <summary>
        /// 内属記号(I調、II調…不明な場合もあり)
        /// </summary>
        public KeyRelation KeyRelation { get { return ct.keyRelation; } }
        /// <summary>
        /// この和音の属する調(内部調)の調名。
        /// </summary>
        public MusicKey ActualKey { get { return ct.ActualKey(); } }

        public CD ChordDegree { get { return ct.chordDegree; } }
        public PositionOfAChord PositionOfAChord { get { return ct.positionOfAChord; } }
        public NumberOfNotes NumberOfNotes { get { return ct.numberOfNotes; } }
        public Omission Omission { get { return ct.omission; } }
        public Inversion Inversion { get { return ct.bassInversion; } }
        public bool IsStandard { get { return standard; } }
        public bool IsOK() { return standard || VerdictValue.NotSoGood < verdictValue; }
        public Inversion SopInversion { get { return GetPitch(Part.Sop).Inversion; } }
        public Pitch GetPitch(Part part) { return pitches[(int)part]; }
        public VerdictValue Verdict { get { return verdictValue; } }
        public bool IsMajor() { return ct.Is内部調は長調(); }
        public bool IsMinor() { return ct.Is内部調は短調(); }
        public FunctionType FunctionType { get { return functionType; } }
        public ChordType ChordType { get { return ct; } set { ct = value; } }
        public TerminationType TerminationType { get { return ct.termination; } }
        public bool Is準固有和音 { get { return ct.is準固有; } }
        public bool Has固有VII { get { return ct.has固有VII; } }
        public bool Is固有和音 {
            get {
                if (ct.is準固有) { return false; }
                switch (ct.chordDegree) {
                case CD.I:
                case CD.II:
                case CD.III:
                case CD.IV:
                case CD.V:
                case CD.VI:
                case CD.VII:
                    return true;
                case CD.V_V:
                    // IIp36 V_Vは固有和音ではない。(借用和音)
                    return false;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    return true;
                }
            }
        }

        public bool PartIsMiddleVoice(Part part) {
            return part == Part.Alt || part == Part.Ten;
        }
        public bool PartIsOuterVoice(Part part) {
            return part == Part.Sop || part == Part.Bas;
        }
        
        public Chord() {
        }

        public Chord(ChordType chordType, bool standard, Pitch bass, Pitch tenor, Pitch alto, Pitch soprano) {
            this.CadenceType = CadenceType.Unspecified;
            this.ct = chordType;
            this.standard = standard;

            pitches = new Pitch[4];
            pitches[(int)Part.Sop] = soprano;
            pitches[(int)Part.Alt] = alto;
            pitches[(int)Part.Ten] = tenor;
            pitches[(int)Part.Bas] = bass;

            // この時点では和音の調が内部調になっていることがある。
            // (例: V度のV度諸和音は、V度の和音になっていることがある)
            //したがって、所属調が正しいものになった時点でもう一度Reevaluateを呼び出して和音の情報を付け直す必要がある。
            ReevaluateChordVerdictAndChordFunction();
        }

        /// <summary>
        /// 和音単独の評価をやり直す。(前後の連結状況が変わったときに評価を最初から付け直す際に呼ぶ。)
        /// </summary>
        public void ReevaluateChordVerdictAndChordFunction() {
            SetChordConstructionType();

            // まずクリアーする。
            verdictValue = VerdictValue.Good;
            verdicts.Clear();

            CheckChordPosition();
            SetChordFunctionRoughly();

            AddSelfDescription();
            EvalChordVerdictInfo();
        }

        /// <summary>
        /// Verdictに和音単独のInfo情報を付加(verdictsに追加)する。
        /// </summary>
        private void EvalChordVerdictInfo() {
            if (ChordDegree == CD.VII) {
                UpdateVerdict(
                    new Verdict(VerdictValue.Info,
                        VerdictReason.InfoIIIp214_76_4_2));
            }

            if (Is("[2転]III") || Is("[2転]VII")) {
                // III VII triadは2転は使用しない。
                UpdateVerdict(
                    new Verdict(VerdictValue.Avoid,
                        VerdictReason.WrongDoNotUseChordType, "III巻p211"));
            }
            if (Is("[2転]VI")) {
                // VI triadは2転は使用しないようである。
                UpdateVerdict(
                    new Verdict(VerdictValue.Avoid,
                        VerdictReason.WrongDoNotUseChordType, "III巻p214 76 5 ?"));
            }
        }

        /// <summary>
        /// 和音機能の単独判定。前後関係を見ないで判定するため精度が低い。
        /// </summary>
        public void SetChordFunctionRoughly() {
            FunctionType f = FunctionType.Unknown;

            switch (ChordDegree) {
            case CD.I:
                if (Inversion == Inversion.第5音) {
                    f = FunctionType.Dominant;
                } else {
                    f = FunctionType.Tonic;
                }
                break;
            case CD.II:
            case CD.IV:
            case CD.V_V:
                f = FunctionType.Subdominant;
                break;
            case CD.III:
            case CD.VI:
                f = FunctionType.Tonic;
                break;
            case CD.V:
            case CD.VII:
                f = FunctionType.Dominant;
                break;
            default:
                break;
            }
            UpdateFunction(f);
        }

        /// <summary>
        /// ct.constructionをセットする。長3和音、短3和音…。
        /// </summary>
        private void SetChordConstructionType() {
            ct.construction = ChordCategory.GetConstructionTypeFromChordType(ct);
        }

        public void Debug() {
            System.Console.WriteLine("({0}{1} {2}{3} {4}{5} {6}{7})",
                pitches[0].LetterName.LN, pitches[0].Octave,
                pitches[1].LetterName.LN, pitches[1].Octave,
                pitches[2].LetterName.LN, pitches[2].Octave,
                pitches[3].LetterName.LN, pitches[3].Octave);
        }

        /// <summary>
        /// この和音の指定partは内声か？
        /// @todo 声部の停止を作ったときは修正の必要あり。
        /// </summary>
        public bool Is内声(Part part) {
            if (part == Part.Alt ||
                part == Part.Ten) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// この和音の指定partは外声か？
        /// @todo 声部の停止を作ったときはIs内声()の修正の必要あり。
        /// </summary>
        public bool Is外声(Part part) {
            return !Is内声(part);
        }

        /// <summary>
        /// 和音の配置チェック。
        /// </summary>
        private void CheckChordPosition() {
            CheckLeadingNote();
            CheckBp22_3();
        }

        /// <summary>
        /// 導音の重複チェック。
        /// falseが戻った和音はリストに載せない。
        /// </summary>
        private bool CheckLeadingNote() {
            var leadingNoteList = Get導音リスト();
            if (leadingNoteList.Count == 1) {
                return true;
            }
            // 導音の重複。
            foreach (限定進行音Info x in leadingNoteList) {
                Part part = x.part;

                UpdateVerdict(
                    new Verdict(VerdictValue.Delist,
                        VerdictReason.RuleA2_1,
                        part));
            }
            return false;
        }

        /// <summary>
        /// 導音リスト取得。1つもない場合、要素が0個のリストが戻る(nullではなく)
        /// </summary>
        public List<限定進行音Info> Get導音リスト() {
            var result = new List<限定進行音Info>();
            if (ChordDegree != CD.V &&
                ChordDegree != CD.V_V) {
                return result;
            }

            var leadingNotePartList = GetPartListByInversion(Inversion.第3音);
            foreach (Part part in leadingNotePartList) {
                result.Add(new 限定進行音Info(part, 限定進行音Type.導音));
            }
            return result;
        }

        /// <summary>
        /// この和音の限定進行音のリストを取得。
        /// @todo 新しい和音を追加したらここにも追加する。
        /// </summary>
        public List<限定進行音Info> Get限定進行音リスト() {
            var result = new List<限定進行音Info>();

            result.AddRange(Get導音リスト());

            if (NumberOfNotes == NumberOfNotes.Seventh) {
                switch (ChordDegree) {
                case CD.V:
                case CD.II:
                case CD.IV:
                case CD.V_V:
                    // 第7音は下行限定。
                    {
                        var part7List = GetPartListByInversion(Inversion.第7音);
                        foreach (Part part in part7List) {
                            result.Add(new 限定進行音Info(part, 限定進行音Type.下行限定進行音));
                        }
                    }
                    break;
                default:
                    break;
                }
            }
            if (NumberOfNotes == NumberOfNotes.Ninth) {
                switch (ChordDegree) {
                case CD.V:
                case CD.V_V:
                    // 第7音は下行限定。
                    {
                        var part7List = GetPartListByInversion(Inversion.第7音);
                        foreach (Part part in part7List) {
                            result.Add(new 限定進行音Info(part, 限定進行音Type.下行限定進行音));
                        }
                    }
                    // 第9音は下行限定。
                    {
                        var part9List = GetPartListByInversion(Inversion.第9音);
                        foreach (Part part in part9List) {
                            result.Add(new 限定進行音Info(part, 限定進行音Type.下行限定進行音));
                        }
                    }
                    break;
                default:
                    break;
                }
            }

            if (ChordDegree == CD.V_V && AlterationType == AlterationType.Lowered) {
                // V_V下方変位和音。第5音が下行限定。
                var part5 = GetPartListByInversion(Inversion.第5音);
                foreach (Part part in part5) {
                    result.Add(new 限定進行音Info(part, 限定進行音Type.下行限定進行音));
                }
            }

            if (AlterationType == AlterationType.Raised &&
                    (ChordDegree == CD.V || ChordDegree == CD.V_V)) {
                // V上方変位和音。第5音が上行限定。
                var part5 = GetPartListByInversion(Inversion.第5音);
                foreach (Part part in part5) {
                    result.Add(new 限定進行音Info(part, 限定進行音Type.上行限定進行音));
                }
            }
            if (AlterationType == AlterationType.Raised &&
                    ChordDegree == CD.IV) {
                // IV+6、IV+46上方変位和音。第6音が上行限定。
                var part6 = GetPartListByInversion(Inversion.第6音);
                foreach (Part part in part6) {
                    result.Add(new 限定進行音Info(part, 限定進行音Type.上行限定進行音));
                }
            }

            if (ChordDegree == CD.IV && AddedTone == AddedToneType.Six) {
                // IV付加6の和音。第6音が上行限定。
                var part6List = GetPartListByInversion(Inversion.第6音);
                foreach (Part part in part6List) {
                    result.Add(new 限定進行音Info(part, 限定進行音Type.上行限定進行音));
                }
            }
            if (ChordDegree == CD.IV && AddedTone == AddedToneType.SixFour) {
                // IV付加46の和音。第6音、第4音が上行限定。
                {
                    var part6List = GetPartListByInversion(Inversion.第6音);
                    foreach (Part part in part6List) {
                        result.Add(new 限定進行音Info(part, 限定進行音Type.上行限定進行音));
                    }
                }
                {
                    var part4List = GetPartListByInversion(Inversion.第4音);
                    foreach (Part part in part4List) {
                        result.Add(new 限定進行音Info(part, 限定進行音Type.上行限定進行音));
                    }
                }
            }

            if (ChordDegree == CD.II && AlterationType == AlterationType.Naples) {
                // ナポリのII。特に無し。
            }
            if (ChordDegree == CD.IV && AlterationType == AlterationType.Dorian) {
                // ドリアIV7。第3音が上行限定
                var part3List = GetPartListByInversion(Inversion.第3音);
                foreach (Part part in part3List) {
                    result.Add(new 限定進行音Info(part, 限定進行音Type.上行限定進行音));
                }
            }
            return result;
        }

        /// <summary>
        /// 変位和音の構成音チェック。この関数は、CheckChordPositionOnGenerate()では呼べない。
        /// V_Vが、生成時にはVになっているため。
        /// </summary>
        /// <returns>true: delistする</returns>
        public bool CheckAlteredChord() {
            if (AlterationType == AlterationType.Lowered) {
                if (ChordDegree == CD.V_V) {
                    // 第5音が1個だけ含まれている必要がある。
                    var part5List = GetPartListByInversion(Inversion.第5音);
                    var part3List = GetPartListByInversion(Inversion.第3音);
                    if (part5List.Count != 1 ||
                        part3List.Count != 1) {
                        UpdateVerdict(
                            new Verdict(VerdictValue.Delist, VerdictReason.InvalidAlteredChord));
                        return true;
                    }
                    if (NumberOfNotes == NumberOfNotes.Triad) {
                        // 3和音はほとんど用いない
                        UpdateVerdict(
                            new Verdict(VerdictValue.Avoid, VerdictReason.UnusedLoweredV));
                    }

                    var pitch3 = GetPitch(part3List[0]);
                    var pitch5 = GetPitch(part5List[0]);
                    if (pitch3.AbsNumberOfSemitonesWith(pitch5) == 2 &&
                        pitch3.AbsIntervalNumberWith(pitch5) == 2) {
                        // 変位第5音と第3音が単音程減3度をなしてはならない。
                        UpdateVerdict(
                            new Verdict(VerdictValue.Wrong, VerdictReason.RuleA5, part5List[0], part3List[0]));
                    }

                    if (IsMajor() &&
                        !Is準固有和音 && //< ここをIs固有和音とするとバグる。
                        NumberOfNotes == NumberOfNotes.Ninth &&
                        Omission == Omission.First) {
                        // II巻p62,23 3) 長調の固有V_V9根省下変和音の使用は稀である。
                        UpdateVerdict(
                            new Verdict(VerdictValue.Rare, VerdictReason.RareIIp62_23_3));
                    }
                }
            }
            if (AlterationType == AlterationType.Raised &&
                    ChordDegree == CD.IV) {
                // IV+6 IV+46上方変位
                if (IsMinor()) {
                    UpdateVerdict(
                        new Verdict(VerdictValue.Info, VerdictReason.RareRaisedIV));
                }
            }

            if (AlterationType == AlterationType.Raised &&
                    (ChordDegree == CD.V || ChordDegree == CD.V_V)) {
                // V上方変位

                if (IsMinor()) {
                    UpdateVerdict(
                        new Verdict(VerdictValue.Info, VerdictReason.RareRaisedV));
                }

                if (NumberOfNotes == NumberOfNotes.Seventh && Omission == Omission.First) {
                    UpdateVerdict(
                        new Verdict(VerdictValue.Avoid, VerdictReason.UnusedRaisedV7));
                }

                if (ChordDegree == CD.V_V) {
                    if (NumberOfNotes != NumberOfNotes.Triad) {
                        // 3和音以外は用いない。
                        UpdateVerdict(
                            new Verdict(VerdictValue.Avoid, VerdictReason.UnusedRaisedV_V));
                    }
                }

                {
                    var part5 = GetPartListByInversion(Inversion.第5音);
                    var part7 = GetPartListByInversion(Inversion.第7音);
                    if (part5.Count == 1 && part7.Count == 1) {
                        var pitch5 = GetPitch(part5[0]);
                        var pitch7 = GetPitch(part7[0]);
                        if (pitch5.AbsNumberOfSemitonesWith(pitch7) == 2 &&
                                pitch5.AbsIntervalNumberWith(pitch7) == 2) {
                            // 変位第5音と第7音が単音程減3度をなしてはならない。
                            UpdateVerdict(
                                new Verdict(VerdictValue.Wrong, VerdictReason.RuleA5_2, part5[0], part7[0]));
                        }
                    }
                }
                switch (Inversion) {
                case Inversion.根音:
                case Inversion.第3音:
                    if (GetPitch(Part.Sop).Inversion != Inversion.第5音 ||
                            PositionOfAChord == PositionOfAChord.Oct) {
                        // 最適ではない配置。
                        UpdateVerdict(
                            new Verdict(VerdictValue.Okay, VerdictReason.OkayRaisedV));
                    }
                    break;
                case Inversion.第5音:
                case Inversion.第7音:
                    if ((NumberOfNotes == NumberOfNotes.Seventh && !Is("密(3)")) ||
                            (NumberOfNotes == NumberOfNotes.Ninth && IsMajor() && !Is("密(9)"))) {
                        // 最適ではない配置。
                        UpdateVerdict(
                            new Verdict(VerdictValue.Okay, VerdictReason.OkayRaisedV));
                    }
                    break;
                default:
                    break;
                }
            }
            return false;
        }

        /// <summary>
        /// 別巻p22の注3チェック。
        /// </summary>
        private void CheckBp22_3() {
            if (NumberOfNotes == NumberOfNotes.Triad &&
                Inversion == Inversion.第3音 &&
                PositionOfAChord == PositionOfAChord.開 &&
                SopInversion == Inversion.第5音 &&
                0 == Upper3CountByInversion(Inversion.第3音) &&
                2 == Upper3CountByInversion(Inversion.根音)) {
                UpdateVerdict(
                    new Verdict(VerdictValue.NotSoGood,
                        VerdictReason.NotSoGoodBp22_3));
            }
        }

        public void UpdateFunction(FunctionType f) {
            this.functionType = f;
        }

        public void RenumberNoteDegrees() {
            for (int i = 0; i < 4; ++i) {
                // 調とLNからdegreeを出す
                var mki = new MusicKeyInfo(MusicKey, KeyRelation);
                var ln1 = new LetterName(mki.GetFirstDegreeLN());
                var ln        = pitches[i].LetterName;
                Inversion inv = pitches[i].Inversion;
                int octave    = pitches[i].Octave;

                // degreeはこの調の主音からの度数で、主音と同音=1
                int deg = 1 + ((ln.NoteDistanceFromC() + 7 - ln1.NoteDistanceFromC()) % 7);
                pitches[i] = new Pitch(new LnDegInversion(ln.LN, deg, inv), octave);
            }
        }

        /// <summary>
        /// 和音の良否判定verdictValueを更新する。
        /// 基本的には悪い方向にしか更新しないが
        /// 良→最適や、良→標結、良→許へは遷移する。
        /// </summary>
        /// <param name="newVerdict"></param>
        public void UpdateVerdict(Verdict v) {
            verdicts.Add(v);

            if (v.value == VerdictValue.Info) {
                // 情報の場合、良否は更新しない
                return;
            }

            if (verdictValue < VerdictValue.Good &&
                (VerdictValue.Good < v.value)) {
                // 良未満の状況下で、最適や標準がきた場合無視する。
                return;
            }
            if (VerdictValue.Good < v.value) {
                // 良以上→最適
                // 良以上→標結
                // 良以上→許
                verdictValue = v.value;
                return;
            }

            // ここには、v.valueが良以下の場合に来る。
            // 悪い方向には更新する。
            if (v.value < verdictValue) {
                verdictValue = v.value;
            }
        }

        /// <summary>
        /// 減7の和音か？
        /// </summary>
        public bool IsDim7() {
            /*
            if (ChordDegree == CD.II && NumberOfNotes == NumberOfNotes.Seventh &&
                (!IsMajor() || Is準固有)) {
                // 短調のII7の和音
                return true;
            }
            if (ChordDegree == CD.VII && NumberOfNotes == NumberOfNotes.Seventh &&
                (IsMajor()&&!Is準固有)) {
                // 長調のVII7の和音
                return true;
            }
            */
            if ((ChordDegree == CD.V || ChordDegree == CD.V_V) &&
                NumberOfNotes == NumberOfNotes.Ninth &&
                Omission == Omission.First && (!IsMajor() || Is準固有和音)) {
                // 短調のV9根省の和音
                return true;
            }
            if (AddedTone == AddedToneType.SixFour &&
                ChordDegree == CD.IV &&
                (IsMinor() || Is準固有和音)) {
                // 短調のIV付加4-6の和音
                return true;
            }
            return false;
        }

        public string VerdictValueToString(VerdictValue v) {
            switch (v) {
            case VerdictValue.Best: return "最適";
            case VerdictValue.Good: return "良";
            case VerdictValue.Okay: return "可能";
            case VerdictValue.Acceptable: return "許";
            case VerdictValue.Rare: return "まれ";
            case VerdictValue.Avoid: return "避けよ";
            case VerdictValue.NotSoGood: return "なるべくさけよ";
            case VerdictValue.Prohibited: return "禁";
            case VerdictValue.Wrong: return "不良";
            case VerdictValue.Info: return "注";
            default:
                System.Diagnostics.Debug.Assert(false);
                return "unknown";
            }
        }

        public Pitch GetPitchByInversion(Inversion inv) {
            foreach (Pitch p in pitches) {
                if (p.Inversion == inv) {
                    return p;
                }
            }
            return Pitch.INVALID_PITCH;
        }

        /// <summary>
        /// 転回指数がinvのパート
        /// </summary>
        /// <param name="inv"></param>
        /// <returns></returns>
        public List<Part> GetPartListByInversion(Inversion inv) {
            List<Part> result = new List<Part>();

            int part = 0;
            foreach (Pitch p in pitches) {
                if (p.Inversion == inv) {
                    result.Add((Part)part);
                }
                ++part;
            }
            return result;
        }

        /// <summary>
        /// 音度がdegreeのパート。
        /// </summary>
        /// <param name="degree">音度。1==主音。</param>
        /// <returns></returns>
        public List<Part> GetPartListByDegree(int degree) {
            List<Part> result = new List<Part>();

            int part = 0;
            foreach (Pitch p in pitches) {
                if (p.Degree == degree) {
                    result.Add((Part)part);
                }
                ++part;
            }
            return result;
        }

        public List<Part> GetUpper3PartListByInversion(Inversion inv) {
            List<Part> result = new List<Part>();

            int part = 0;
            foreach (Pitch p in pitches) {
                if (0 != part) {
                    if (p.Inversion == inv) {
                        result.Add((Part)part);
                    }
                }
                ++part;
            }
            return result;
        }

        /// <summary>
        /// 音度がdegreeの上3声パート。
        /// </summary>
        /// <param name="degree">音度。1==主音。</param>
        /// <returns></returns>
        public List<Part> GetUpper3PartListByDegree(int degree) {
            List<Part> result = new List<Part>();

            int part = 0;
            foreach (Pitch p in pitches) {
                if (0 != part) {
                    if (p.Degree == degree) {
                        result.Add((Part)part);
                    }
                }
                ++part;
            }
            return result;
        }

        /// <summary>
        /// 上3声に特定の音が含まれる個数を戻す。
        /// </summary>
        /// <returns>0: 含まれない。1: 1個含まれる。2: 2個含まれる。3: 3個含まれる。</returns>
        public int Upper3CountByInversion(Inversion inversion) {
            int result = 0;
            for (int i=(int)Part.Ten; i <= (int)Part.Sop; ++i) {
                Part part = (Part)i;
                Pitch pitch = GetPitch(part);

                if (pitch.Inversion == inversion) {
                    ++result;
                }
            }
            return result;
        }

        /// <summary>
        /// 上3声のInversionの種類を数える。
        /// </summary>
        /// <returns></returns>
        public int Upper3NumOfInversionVariation() {
            var inversionSet = new HashSet<Inversion>();
            for (int i = (int)Part.Ten; i <= (int)Part.Sop; ++i) {
                Part part = (Part)i;
                Pitch pitch = GetPitch(part);

                inversionSet.Add(pitch.Inversion);
            }
            return inversionSet.Count;
        }

        /// <summary>
        /// 全ての構成音が、各パートの音域の範囲内にあるかどうか。
        /// (バス～テノール間の距離が12音以内かどうか。許容条件があるためテストやめた)
        /// テノール～アルト間の距離が8音以内かどうか。(II巻補充課題9-2に10音の離隔あり)
        /// アルト～ソプラノ間の距離が8音以内かどうか。
        /// </summary>
        /// <returns>true: 全ての構成音が、各パートの音域の範囲内にある。false:ない。</returns>
        public bool IsWithinTheRange() {
            Pitch bas = GetPitch(Part.Bas);
            Pitch ten = GetPitch(Part.Ten);
            Pitch alt = GetPitch(Part.Alt);
            Pitch sop = GetPitch(Part.Sop);

            if (!sop.IsWithinTheRange(Part.Sop)) {
                return false;
            }
            if (!alt.IsWithinTheRange(Part.Alt)) {
                return false;
            }
            if (!ten.IsWithinTheRange(Part.Ten)) {
                return false;
            }
            if (!bas.IsWithinTheRange(Part.Bas)) {
                return false;
            }

            /*
            if (11 < bas.AbsIntervalNumberWith(ten)) {
                return false;
            }
             * */
            if (11 < ten.AbsIntervalNumberWith(alt)) {
                return false;
            }
            if (11 < alt.AbsIntervalNumberWith(sop)) {
                return false;
            }
            if (bas.HigherPitchTo(ten) > 0
                || ten.HigherPitchTo(alt) > 0
                || alt.HigherPitchTo(sop) > 0) {
                return false;
            }

            return true;
        }

        public bool Upper3PositionIsCorrect() {
            int tenToSop = GetPitch(Part.Sop).AbsNumberOfSemitonesWith(GetPitch(Part.Ten));
            if (tenToSop < 12) {
                return PositionOfAChord.密 == ct.positionOfAChord;
            }
            if (12 < tenToSop) {
                return PositionOfAChord.開 == ct.positionOfAChord;
            }
            return PositionOfAChord.Oct == ct.positionOfAChord;
        }

        private static MidiManager ChordToMidiFile(Chord chord) {
            List<Chord> playChords = new List<Chord>();
            playChords.Add(chord);
            return ChordListToMidiFile(playChords, 60);
        }

        public static MidiManager ChordListToMidiFile(List<Chord> playChords, int tempo) {
            MidiManager r = new MidiManager(4);
            r.SetTempo(tempo);
            MidiTrackInfo s = r.GetTrack(0);
            MidiTrackInfo a = r.GetTrack(1);
            MidiTrackInfo t = r.GetTrack(2);
            MidiTrackInfo b = r.GetTrack(3);

            int now = 120;
            int interval = 240;
            int volume = 100;
            
            Chord prevChord = null;
            foreach (Chord chord in playChords) {
                int nowInterval = interval;
                if (chord.TerminationType != TerminationType.None) {
                    nowInterval *= 2;
                }
                /*
                if (chord.ChordDegree == CD.IV &&
                    chord.CadenceType == CadenceType.K3 &&
                    prevChord.TerminationType == TerminationType.Perfect) {
                    nowInterval *= 2;
                }
                 */

                s.AddNote(new Note(now, (int)(nowInterval * 0.8f), chord.GetPitch(Part.Sop).LetterName, chord.GetPitch(Part.Sop).Octave, volume));
                a.AddNote(new Note(now, (int)(nowInterval * 0.8f), chord.GetPitch(Part.Alt).LetterName, chord.GetPitch(Part.Alt).Octave, volume));
                t.AddNote(new Note(now, (int)(nowInterval * 0.8f), chord.GetPitch(Part.Ten).LetterName, chord.GetPitch(Part.Ten).Octave, volume));
                b.AddNote(new Note(now, (int)(nowInterval * 0.8f), chord.GetPitch(Part.Bas).LetterName, chord.GetPitch(Part.Bas).Octave, volume));
                now += nowInterval;

                prevChord = chord;
            }
            prevChord = null;

            s.AddNote(new Note(now, interval * 4, new LetterName(LN.C), 4, 0));
            a.AddNote(new Note(now, interval * 4, new LetterName(LN.C), 4, 0));
            t.AddNote(new Note(now, interval * 4, new LetterName(LN.C), 4, 0));
            b.AddNote(new Note(now, interval * 4, new LetterName(LN.C), 4, 0));
            return r;
        }

        public void PlayNow() {
            MidiManager mm = ChordToMidiFile(this);
            mm.PlayNow();
        }

        /// <summary>
        /// 全パートのピッチを重みをつけて足した物。ソートに使用する。
        /// </summary>
        /// <returns></returns>
        public int PitchWeightedAccumulate() {
            return GetPitch(Part.Bas).GetMidiPitchValue() * (1 << 24)
                + GetPitch(Part.Ten).GetMidiPitchValue() * (1 << 16)
                + GetPitch(Part.Alt).GetMidiPitchValue() * (1 << 8)
                + GetPitch(Part.Sop).GetMidiPitchValue();
        }

        /// <summary>
        /// 一覧表示のソートに使用する。
        /// 進行における各パートの絶対音程を足した物だが、いけてない和音の場合大きな値になる。
        /// </summary>
        /// <param name="prevChord"></param>
        /// <returns></returns>
        public int AllPartProgressionDiffAccumulate(Chord prevChord) {
            int accum = 0;
            for (int i=(int)Part.Bas; i<=(int)Part.Sop; ++i) {
                Part part = (Part)i;
                accum += GetPitch(part).AbsIntervalNumberWith(prevChord.GetPitch(part));
            }

            // 現在のChordに完全一度を形成するパートが存在する場合、
            // k足す(他の選択肢が少し優先表示されるようにする)
            for (int i=(int)Part.Bas; i <= (int)Part.Alt; ++i) {
                Part part0 = (Part)i;
                Part part1 = (Part)(i+1);
                if (0 == GetPitch(part0).AbsNumberOfSemitonesWith(GetPitch(part1))) {
                    accum += 3;
                }
            }
            return accum;
        }

        public List<Pitch> CreatePitchListFromPartList(List<Part> partList) {
            var pitchList = new List<Pitch>();
            foreach (Part p in partList) {
                pitchList.Add(GetPitch(p));
            }
            return pitchList;
        }

        /// <summary>
        /// 文字列sを読み取って、ChordDegreeを戻す
        /// </summary>
        private CD StringRomanNumber1234567ToCD(string s, int pos, out int length) {
            switch (s[pos]) {
            case 'I':
                if (s.Length <= pos + 1) {
                    // I
                    length = 1;
                    return CD.I;
                }
                switch (s[pos+1]) {
                case 'I':
                    if (s.Length <= pos + 2) {
                        // II
                        length = 2;
                        return CD.II;
                    }
                    switch (s[pos+2]) {
                    case 'I':
                        // III
                        length = 3;
                        return CD.III;
                    default:
                        break;
                    }
                    // II
                    length = 2;
                    return CD.II;
                case 'V':
                    // IV
                    length = 2;
                    return CD.IV;
                default:
                    break;
                }
                // I
                length = 1;
                return CD.I;
            case 'V':
                if (s.Length <= pos + 1) {
                    // V
                    length = 1;
                    return CD.V;
                }
                switch (s[pos + 1]) {
                case '_':
                    if (s.Length <= pos + 2) {
                        System.Diagnostics.Debug.Assert(false);
                    }
                    switch (s[pos + 2]) {
                    case 'V':
                        // V_V
                        length = 3;
                        return CD.V_V;
                    default:
                        break;
                    }
                    System.Diagnostics.Debug.Assert(false);
                    break;
                case 'I':
                    if (s.Length <= pos + 2) {
                        // VI
                        length = 2;
                        return CD.VI;
                    }
                    switch (s[pos + 2]) {
                    case 'I':
                        // VII
                        length = 3;
                        return CD.VII;
                    default:
                        break;
                    }
                    // VI
                    length = 2;
                    return CD.VI;
                default:
                    break;
                }
                // V
                length = 1;
                return CD.V;
            default:
                break;
            }
            // 数字なし
            length = 0;
            return CD.I;
        }

        /// <summary>
        /// この和音のChordDegreeとproperty文字列のChordDegreeを比較し、同じならtrueを戻す。
        /// </summary>
        private bool CheckDegreeStr(string property, ref int pos) {
            int degLen;

            CD cd = StringRomanNumber1234567ToCD(property, pos, out degLen);
            System.Diagnostics.Debug.Assert(0 < degLen);
            pos += degLen;

            return ChordDegree == cd;
        }

        public bool Is(string property) {
            // 標[基]I密(5)
            // 外[基]VIOct根省3重(3)
            // のような文字列をもとに、和音の性質を照合する
            for (int pos = 0; pos < property.Length; ) {
                switch (property[pos]) {
                case '○': // 準固有
                    if (!Is準固有和音) { return false; }
                    ++pos;
                    break;
                case '(': // 高音位
                    switch (property[pos + 1]) {
                    case '根': if (SopInversion != Inversion.根音) { return false; } break;
                    case '3': if (SopInversion != Inversion.第3音) { return false; } break;
                    case '5': if (SopInversion != Inversion.第5音) { return false; } break;
                    case '7': if (SopInversion != Inversion.第7音) { return false; } break;
                    case '9': if (SopInversion != Inversion.第9音) { return false; } break;
                    case '4': if (SopInversion != Inversion.第4音) { return false; } break;
                    case '6': if (SopInversion != Inversion.第6音) { return false; } break;
                    default:
                        System.Diagnostics.Debug.Assert(false);
                        break;
                    }
                    System.Diagnostics.Debug.Assert(property[pos + 2] == ')');
                    pos += 3; // skip "(3)"
                    break;
                case '揃': // 3和音の場合に、上3声にすべての構成音が揃っている
                    System.Diagnostics.Debug.Assert(NumberOfNotes == NumberOfNotes.Triad);
                    if (Upper3CountByInversion(Inversion.根音) != 1) { return false; }
                    if (Upper3CountByInversion(Inversion.第3音) != 1) { return false; }
                    if (Upper3CountByInversion(Inversion.第5音) != 1) { return false; }
                    ++pos;
                    break;
                case '密':
                    if (PositionOfAChord != PositionOfAChord.密) { return false; }
                    ++pos;
                    break;
                case '開':
                    if (PositionOfAChord != PositionOfAChord.開) { return false; }
                    ++pos;
                    break;
                case 'O':
                    if (PositionOfAChord != PositionOfAChord.Oct) { return false; }
                    System.Diagnostics.Debug.Assert(property[pos + 1] == 'c' || property[pos + 2] == 't');
                    pos += 3; // skip "Oct"
                    break;
                case '根': {
                        char pred = property[pos + 1];
                        System.Diagnostics.Debug.Assert(pred == '省' || pred == '重');
                        if (pred == '省' && Upper3CountByInversion(Inversion.根音) != 0) { return false; }
                        if (pred == '重' && Upper3CountByInversion(Inversion.根音) != 2) { return false; }
                        pos += 2; // skip 根省
                    }
                    break;
                case '3': {
                        char pred = property[pos + 1];
                        System.Diagnostics.Debug.Assert(pred == '省' || pred == '重');
                        if (pred == '省' && Upper3CountByInversion(Inversion.第3音) != 0) { return false; }
                        if (pred == '重' && Upper3CountByInversion(Inversion.第3音) != 2) { return false; }
                        pos += 2; // skip 3省
                    }
                    break;
                case '5': {
                        char pred = property[pos + 1];
                        System.Diagnostics.Debug.Assert(pred == '省' || pred == '重');
                        if (pred == '省' && Upper3CountByInversion(Inversion.第5音) != 0) { return false; }
                        if (pred == '重' && Upper3CountByInversion(Inversion.第5音) != 2) { return false; }
                        pos += 2; // skip 5省
                    }
                    break;
                case '7': {
                        char pred = property[pos + 1];
                        System.Diagnostics.Debug.Assert(pred == '省' || pred == '重');
                        if (pred == '省' && Upper3CountByInversion(Inversion.第7音) != 0) { return false; }
                        if (pred == '重' && Upper3CountByInversion(Inversion.第7音) != 2) { return false; }
                        pos += 2; // skip 7省
                    }
                    break;
                case 'I':
                case 'V':
                    if (!CheckDegreeStr(property, ref pos)) { return false; }
                    if (property.Length <= pos) {
                        if (NumberOfNotes != NumberOfNotes.Triad) { return false; }
                    } else {
                        char nn = property[pos];
                        switch (nn) {
                        case '7': if (NumberOfNotes != NumberOfNotes.Seventh) { return false; }
                            ++pos; // consume
                            break;
                        case '9': if (NumberOfNotes != NumberOfNotes.Ninth) { return false; }
                            ++pos; // consume
                            break;
                        default: if (NumberOfNotes != NumberOfNotes.Triad) { return false; }
                            break;
                        }
                    }
                    break;
                case '標': // 標準配置
                    if (!IsStandard) { return false; }
                    ++pos;
                    break;
                case '外': // 標準外配置
                    if (IsStandard) { return false; }
                    ++pos;
                    break;
                case '[': // 展開指数
                    switch (property[pos + 1]) {
                    case '基':
                        if (Inversion != Inversion.根音) { return false; }
                        System.Diagnostics.Debug.Assert(property[pos + 2] == ']');
                        pos += 3; // consume '[基]'
                        break;
                    case '1':
                        if (Inversion != Inversion.第3音) { return false; }
                        System.Diagnostics.Debug.Assert(property[pos + 2] == '転');
                        System.Diagnostics.Debug.Assert(property[pos + 3] == ']');
                        pos += 4; // consume '[1転]'
                        break;
                    case '2':
                        if (Inversion != Inversion.第5音) { return false; }
                        System.Diagnostics.Debug.Assert(property[pos + 2] == '転');
                        System.Diagnostics.Debug.Assert(property[pos + 3] == ']');
                        pos += 4; // consume '[2転]'
                        break;
                    case '3':
                        if (Inversion != Inversion.第7音) { return false; }
                        System.Diagnostics.Debug.Assert(property[pos + 2] == '転');
                        System.Diagnostics.Debug.Assert(property[pos + 3] == ']');
                        pos += 4; // consume '[3転]'
                        break;
                    case '4':
                        if (Inversion != Inversion.第9音) { return false; }
                        System.Diagnostics.Debug.Assert(property[pos + 2] == '転');
                        System.Diagnostics.Debug.Assert(property[pos + 3] == ']');
                        pos += 4; // consume '[4転]'
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false);
                        return false;
                    }
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// part0とpart1の音度-1。
        /// part0がC4、Part1がD4のとき1が戻る
        /// part0がD4、Part1がC4のとき-1が戻る
        /// part0がC4、Part1がE4のとき2が戻る
        /// </summary>
        /// <param name="part0"></param>
        /// <param name="part1"></param>
        /// <returns></returns>
        public int TwoPartIntervalNumber(Part part0, Part part1) {
            return GetPitch(part1).HigherIntervalTo(GetPitch(part0));
        }

        /// <summary>
        /// part0とpart1のピッチの差(半音の数)。
        /// part0がC4、Part1がD4のとき2が戻る
        /// part0がD4、Part1がC4のとき-2が戻る
        /// part0がC4、Part1がE4のとき4が戻る
        /// </summary>
        /// <param name="part0"></param>
        /// <param name="part1"></param>
        /// <returns></returns>
        public int TwoPartNumberOfSemitones(Part part0, Part part1) {
            return GetPitch(part1).HigherPitchTo(GetPitch(part0));
        }

        public IntervalType TwoPartIntervalType(Part part0, Part part1) {
            return GetPitch(part1).GetIntervalType(GetPitch(part0));
        }

        /// <summary>
        /// この和音についての説明を付加する。
        /// </summary>
        public void AddSelfDescription() {
            UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoChord, ct.construction.ToString(),
                ChordCategory.GetChordOverviewText(ct)));
        }
    }
}
