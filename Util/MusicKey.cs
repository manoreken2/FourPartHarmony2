using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FourPartHarmony2
{
    /// <summary>
    /// 調
    /// </summary>
    public enum MusicKey
    {
        Invalid,

        // 長調

        FESESdur, CESESdur, GESESdur, DESESdur, ASESdur,  ESESdur,  BESdur,
        FESdur,   CESdur,   GESdur,   DESdur,   ASdur,    ESdur,    Bdur,
        Fdur,     Cdur,     Gdur,     Ddur,     Adur,     Edur,     Hdur,
        FISdur,   CISdur,   GISdur,   DISdur,   AISdur,   EISdur,   HISdur,
        FISISdur, CISISdur, GISISdur, DISISdur, AISISdur, EISISdur, HISISdur,

        // 短調

        FESESmoll, CESESmoll, GESESmoll, DESESmoll, ASESmoll,  ESESmoll,  BESmoll,
        FESmoll,   CESmoll,   GESmoll,   DESmoll,   ASmoll,    ESmoll,    Bmoll,
        Fmoll,     Cmoll,     Gmoll,     Dmoll,     Amoll,     Emoll,     Hmoll,
        FISmoll,   CISmoll,   GISmoll,   DISmoll,   AISmoll,   EISmoll,   HISmoll,
        FISISmoll, CISISmoll, GISISmoll, DISISmoll, AISISmoll, EISISmoll, HISISmoll,

        NUM
    }

    public enum Mode
    {
        Dur,
        Moll,
        NUM
    }

    /// <summary>
    /// 内属記号。
    /// </summary>
    public enum KeyRelation {
        Unknown=-1, ///< 不明な調関係

        I調,     ///< 主調
        II調,    ///< II調 音度調(固有和音調)  (I調が長調の場合のみ。短調。)
        MII調,   ///< -II調。I調が短調の場合のIIの和音の根音を半音下げた和音がIの調。ナポリII調。長調。
        III調,   ///< III調 音度調(固有和音調) (I調が長調の場合短調。I調が短調の場合長調。)
        IV調,    ///< IV調 音度調(固有和音調)  (I調が長調の場合長調。I調が短調の場合短調。)
        V調,     ///< V調 音度調(固有和音調)  (I調が長調の場合長調。I調が短調の場合短調。)
        VI調,    ///< VI調 音度調(固有和音調)  (I調が長調の場合短調。I調が短調の場合長調。)
        VII調,   ///< VII調 音度調(固有和音調) (I調が短調の場合のみ。長調。)
        VIIU調,  ///< VII↑調。I調が長調の場合のVIIの和音の第5音を半音上げた和音がIの調。短調。

        CI調,    ///< ○I調 (準I調 I調が長調の場合のI調の同主短調。短調。)
        CMII調,  ///< ○-II調。主調が長調の場合の同主短調の固有和音副次-II調
        CIII調,  ///< ○III調 (I調が長調の場合の○I調の固有和音調III調。長調。)
        CIV調,   ///< ○IV調  (I調が長調の場合の○I調の固有和音調IV調。短調。)
        CV調,    ///< ○V調   (I調が長調の場合の○I調の固有和音調V調。短調。)
        CVI調,   ///< ○VI調  (I調が長調の場合の○I調の固有和音調VI調。長調。)
        CVII調,  ///< ○VII調 (I調が長調の場合の○I調の固有和音調VII調。長調。)

        PI調,    ///< ＋I調。△I調。(I調が短調の場合のI調を＋転旋した同主長調。長調。)
        PII調,   ///< ＋II調 (I調が長調の場合のII調を＋転旋した調。固有和音同主長調、副次同主長調。長調。)
        PIII調,  ///< ＋III調(I調が長調の場合のIII調を＋転旋した調。固有和音同主長調、副次同主長調。長調。)
        PIV調,   ///< ＋IV調。△IV調 (I調が短調の場合のIV調を＋転旋した調。固有和音同主長調。長調。)
        PV調,    ///< ＋V調。△V調  (I調が短調の場合のIV調を＋転旋した調。固有和音同主長調。長調。)
        PVI調,   ///< ＋VI調 (I調が長調の場合のVI調を＋転旋した調。固有和音同主長調、副次同主長調。長調。)
        PVIIU調, ///< ＋VII↑調 (I調が長調の場合のVII↑調を＋転旋した調。固有和音同主長調、副次同主長調。長調。)

        PIIIp調, ///< ＋IIIp調 +III調の平行調。短調。
        PVIp調,  ///< ＋VIp調 ＋VI調の平行調。短調。
        PVIIUp調, ///< ＋VII↑p調 ＋VII↑調の平行調。短調。

        TII調,   ///< △II調 (I調が短調の場合の同主固有和音副次II調。短調。)
        TIII調,  ///< △III調 (I調が短調の場合の同主固有和音副次III調。短調。)
        TVI調,   ///< △VI調 (I調が短調の場合の同主固有和音副次VI調。短調。)
        TVIIU調, ///< △VII↑調 (I調が短調の場合の同主固有和音副次VII↑調。短調。)

        PTII調,   ///< ＋△II調 (I調が短調の場合の△II調を＋転旋した調。長調。)
        PTIII調,  ///< ＋△III調 (I調が短調の場合の△III調を＋転旋した調。長調。)
        PTVI調,   ///< ＋△VI調 (I調が短調の場合の△VI調を＋転旋した調。長調。)
        PTVIIU調, ///< ＋△VII↑調 (I調が短調の場合の△VII↑調を＋転旋した調。長調。)

        PTIIIp調, ///< ＋△IIIp調 (I調が短調の場合の△III調を＋転旋した調の平行調。短調。)
        PTVIp調,  ///< ＋△VIp調 (I調が短調の場合の△VI調を＋転旋した調の平行調。短調。)
        PTVIIUp調,///< ＋△VII↑p調 (I調が短調の場合の△VII↑調を＋転旋した調の平行調。短調。)

        MMII調,  ///< －－II調 (I調が短調の場合の-II調を－転旋した調。固有和音同主短調。短調。)
        MIII調,  ///< －III調 (I調が短調の場合のIII調を－転旋した調。固有和音同主短調。短調。)
        MVI調,   ///< －VI調 (I調が短調の場合のVI調を－転旋した調。固有和音同主短調。短調。)
        MVII調,  ///< －VII調 (I調が短調の場合のVII調を－転旋した調。固有和音同主短調。短調。)

        MMIIp調, ///< －－IIp調 (I調が短調の場合の-II調を－転旋した調の平行調。長調。)
        MIIIp調, ///< －IIIp調 (I調が短調の場合のIII調を－転旋した調の平行調。長調。)
        MVIp調,  ///< －VIp調 (I調が短調の場合のVI調を－転旋した調の平行調。長調。)

        MCMII調, ///< －○－II調 (I調が長調の場合の○－II調を－転旋した調。準固有和音同主短調。短調。)
        MCIII調, ///< －○III調 (I調が長調の場合の○III調を－転旋した調。準固有和音同主短調。短調。)
        MCVI調,  ///< －○VI調 (I調が長調の場合の○VI調を－転旋した調。準固有和音同主短調。短調。)
        MCVII調, ///< －○VII調 (I調が長調の場合の○VII調を－転旋した調。準固有和音同主短調。短調。)

        MCMIIp調, ///< －○－IIp調 (I調が長調の場合の○－II調を－転旋した調の平行調。長調。)
        MCIIIp調, ///< －○IIIp調 (I調が長調の場合の○III調を－転旋した調の平行調。長調。)
        MCVIp調,  ///< －○VIp調 (I調が長調の場合の○VI調を－転旋した調の平行調。長調。)

        NUM
    }

    /// <summary>
    /// 内属記号enumをラップしユーティリティメソッドを提供する構造体
    /// </summary>
    public struct KeyRelationInfo {
        private KeyRelation keyRelation;
        public KeyRelation KeyRelation { get { return keyRelation; } set { keyRelation = value; } }
        public KeyRelationInfo(KeyRelation chordKey) {
            this.keyRelation = chordKey;
        }

        /// <summary>
        /// 内属記号表示用文字列を取得
        /// </summary>
        public string DispString() {
            switch (KeyRelation) {
            case KeyRelation.Unknown: return "?";

            case KeyRelation.I調: return "I";
            case KeyRelation.II調: return "II";
            case KeyRelation.MII調: return "-II";
            case KeyRelation.III調: return "III";
            case KeyRelation.IV調: return "IV";
            case KeyRelation.V調: return "V";
            case KeyRelation.VI調: return "VI";
            case KeyRelation.VII調: return "VII";
            case KeyRelation.VIIU調: return "VII↑";

            case KeyRelation.CI調: return "○I";
            case KeyRelation.CMII調: return "○-II";
            case KeyRelation.CIII調: return "○III";
            case KeyRelation.CIV調: return "○IV";
            case KeyRelation.CV調: return "○V";
            case KeyRelation.CVI調: return "○VI";
            case KeyRelation.CVII調: return "○VII";

            case KeyRelation.PI調: return "+I";
            case KeyRelation.PII調: return "+II";
            case KeyRelation.PIII調: return "+III";
            case KeyRelation.PIV調: return "+IV";
            case KeyRelation.PV調: return "+V";
            case KeyRelation.PVI調: return "+VI";
            case KeyRelation.PVIIU調: return "+VII↑";

            case KeyRelation.PIIIp調: return "+IIIp";
            case KeyRelation.PVIp調: return "+VIp";
            case KeyRelation.PVIIUp調: return "+VII↑p";

            case KeyRelation.TII調: return "△II";
            case KeyRelation.TIII調: return "△III";
            case KeyRelation.TVI調: return "△VI";
            case KeyRelation.TVIIU調: return "△VII↑";

            case KeyRelation.PTII調: return "+△II";
            case KeyRelation.PTIII調: return "+△III";
            case KeyRelation.PTVI調: return "+△VI";
            case KeyRelation.PTVIIU調: return "+△VII↑";

            case KeyRelation.PTIIIp調: return "+△IIIp";
            case KeyRelation.PTVIp調: return "+△VIp";
            case KeyRelation.PTVIIUp調: return "+△VII↑p";

            case KeyRelation.MMII調: return "--II";
            case KeyRelation.MIII調: return "-III";
            case KeyRelation.MVI調: return "-VI";
            case KeyRelation.MVII調: return "-VII";

            case KeyRelation.MMIIp調: return "--IIp";
            case KeyRelation.MIIIp調: return "-IIIp";
            case KeyRelation.MVIp調: return "-VIp";

            case KeyRelation.MCMII調: return "-○-II";
            case KeyRelation.MCIII調: return "-○III";
            case KeyRelation.MCVI調: return "-○VI";
            case KeyRelation.MCVII調: return "-○VII";

            case KeyRelation.MCMIIp調: return "-○-IIp";
            case KeyRelation.MCIIIp調: return "-○IIIp";
            case KeyRelation.MCVIp調: return "-○VIp";
            default:
                System.Diagnostics.Debug.Assert(false);
                return "";
            }
        }
    }

    /// <summary>
    /// 個々の調の特徴
    /// </summary>
    internal struct MusicKeyProperties
    {
        MusicKey key;           //< I調
        Mode mode;       //< Dur: 長調、Moll: 短調。
        int flatNum;  //< この調のフラットの数
        int sharpNum; //< この調のシャープの数
        MusicKey[] relatedKeys; //< 近親調(副次調) [0]=主調、[1]=II調、……、[6]=VII調(長調の場合Invalid)
        MusicKey[] parallelRelatedKeys; //< 副次同主調(主調が長調の場合準固有和音調、短調の場合副次同主長調)。[0]=同主調、[1]＝○II調または＋II調……。
        LN[] lns;      //< 各音度の音名。[0]=I度、[1]=II度、……、[6]==VII度。
        string name; //< C c Cis 等

        private static readonly LN[] sharpLNList = { LN.F, LN.C, LN.G, LN.D, LN.A, LN.E, LN.H };
        private static readonly LN[] flatLNList = { LN.H, LN.E, LN.A, LN.D, LN.G, LN.C, LN.F };

        public void Init(MusicKey key, LN ln1, string name, Mode mode, int flatNum, int sharpNum) {
            this.relatedKeys = new MusicKey[7];
            this.parallelRelatedKeys = new MusicKey[7];
            this.lns = new LN[7];

            this.key = key;
            this.lns[0] = ln1;
            this.name = name;
            this.mode = mode;
            this.flatNum = flatNum;
            this.sharpNum = sharpNum;

            this.relatedKeys[0] = key;
        }

        public void SetLnList(LN l2, LN l3, LN l4, LN l5, LN l6, LN l7) {
            lns[1] = l2;
            lns[2] = l3;
            lns[3] = l4;
            lns[4] = l5;
            lns[5] = l6;
            lns[6] = l7;
        }

        public void SetRelatedKeys(MusicKey k2, MusicKey k3, MusicKey k4, MusicKey k5, MusicKey k6, MusicKey k7) {
            relatedKeys[1] = k2;
            relatedKeys[2] = k3;
            relatedKeys[3] = k4;
            relatedKeys[4] = k5;
            relatedKeys[5] = k6;
            relatedKeys[6] = k7;
        }
        public void SetParallelRelatedKeys(MusicKey parallelKey, MusicKey k2, MusicKey k3, MusicKey k4, MusicKey k5, MusicKey k6, MusicKey k7) {
            parallelRelatedKeys[0] = parallelKey;
            parallelRelatedKeys[1] = k2;
            parallelRelatedKeys[2] = k3;
            parallelRelatedKeys[3] = k4;
            parallelRelatedKeys[4] = k5;
            parallelRelatedKeys[5] = k6;
            parallelRelatedKeys[6] = k7;
        }

        /// <summary>
        /// この調のI度の音程の音名。
        /// </summary>
        public LN GetFirstDegreeLN() {
            return lns[0];
        }

        /// <summary>
        /// degreeからLNを取得。
        /// </summary>
        /// <param name="degree">0～6。0:I度。</param>
        /// <returns>音度がdegreeのLN</returns>
        public LN GetLnFromDegree(int degree) {
            return lns[degree];
        }

        public string GetString() {
            return name;
        }

        /// <summary>
        /// 長調か。
        /// </summary>
        /// <returns>true: 長調。</returns>
        public bool IsMajor() {
            return mode == Mode.Dur;
        }

        /// <summary>
        /// 短調か。
        /// </summary>
        /// <returns>true: 短調。</returns>
        public bool IsMinor() {
            return mode == Mode.Moll;
        }

        /// <summary>
        /// 調のフラットの個数。
        /// </summary>
        /// <returns>調のフラットの個数。</returns>
        public int FlatNum() {
            return flatNum;
        }

        /// <summary>
        /// 調のシャープの個数。
        /// </summary>
        /// <returns>調のシャープの個数。</returns>
        public int SharpNum() {
            return sharpNum;
        }

        /// <summary>
        /// 調のシャープまたはフラットの個数。
        /// </summary>
        /// <returns>調のシャープまたはフラットの個数。</returns>
        public int FlatSharpNum() {
            return FlatNum() + SharpNum();
        }

        /// <summary>
        /// この調でシャープが付く音名のリスト
        /// </summary>
        public List<LN> CreateSharpLetterNameList() {
            List<LN> result = new List<LN>();
            if (SharpNum() <= 7) {
                for (int i = 0; i < SharpNum(); ++i) {
                    result.Add(sharpLNList[i]);
                }
            } else {
                for (int i = SharpNum() % 7; i < 7; ++i) {
                    result.Add(sharpLNList[i]);
                }
            }
            return result;
        }

        /// <summary>
        /// この調でフラットが付く音名のリスト
        /// </summary>
        public List<LN> CreateFlatLetterNameList() {
            List<LN> result = new List<LN>();
            if (FlatNum() <= 7) {
                for (int i = 0; i < FlatNum(); ++i) {
                    result.Add(flatLNList[i]);
                }
            } else {
                for (int i = FlatNum() % 7; i < 7; ++i) {
                    result.Add(flatLNList[i]);
                }
            }
            return result;
        }

        /// <summary>
        /// この調でダブルシャープが付く音名のリスト
        /// </summary>
        public List<LN> CreateDoubleSharpLetterNameList() {
            var result = new List<LN>();
            for (int i = 0; i < SharpNum() - 7; ++i) {
                result.Add(sharpLNList[i]);
            }
            return result;
        }

        /// <summary>
        /// 近親調を戻す。
        /// degree=CD.V…属調(V調)を戻す
        /// </summary>
        public MusicKey RelatedKey(CD degree) {
            return relatedKeys[(int)degree];
        }
    }

    /// <summary>
    /// 調の情報
    /// </summary>
    public struct MusicKeyInfo
    {
        private readonly MusicKey internalKey;
        private readonly MusicKey mainKey;

        /// <summary>
        /// 内部調
        /// </summary>
        public MusicKey InternalKey { get { return internalKey; } }

        /// <summary>
        /// 主調
        /// </summary>
        public MusicKey MainKey { get { return mainKey; } }

        static MusicKeyProperties[] keyProps;

        /// <summary>
        /// I度の音名と旋法→調keyを得る表。
        /// </summary>
        static MusicKey[] lnModeKey;

        /// <summary>
        /// 主調keyと内部調internalKey→内属記号を得る表。
        /// </summary>
        static KeyRelation [] mainKeyInternalKeyToKeyRelation;

        public MusicKeyInfo(MusicKey mainKey, MusicKey internalKey) {
            this.mainKey = mainKey;
            this.internalKey = internalKey;
        }

        /// <summary>
        /// こちらのctorはあまり用いない方が良い。
        /// </summary>
        public MusicKeyInfo(MusicKey mainKey, KeyRelation keyRelation) {
            this.mainKey = mainKey;
            this.internalKey = MainKeyAndKeyRelationToKey(mainKey, keyRelation);
        }

        /// <summary>
        /// この調のI度の音程の音名。
        /// </summary>
        public LN GetFirstDegreeLN() {
            return keyProps[(int)internalKey].GetFirstDegreeLN();
        }
        
        /// <summary>
        /// degreeからLNを取得。
        /// </summary>
        /// <param name="degree">0～6。0:I度。</param>
        /// <returns>音度がdegreeのLN</returns>
        public LN GetLnFromDegree(int degree) {
            return keyProps[(int)internalKey].GetLnFromDegree(degree);
        }

        public string GetString() {
            return keyProps[(int)internalKey].GetString();
        }

        /// <summary>
        /// 長調か。
        /// </summary>
        public bool IsMajor() {
            return keyProps[(int)internalKey].IsMajor();
        }

        /// <summary>
        /// 短調か。
        /// </summary>
        public bool IsMinor() {
            return keyProps[(int)internalKey].IsMinor();
        }

        /// <summary>
        /// 調のフラットの個数。
        /// </summary>
        /// <returns>調のフラットの個数。</returns>
        public int FlatNum() {
            return keyProps[(int)internalKey].FlatNum();
        }

        /// <summary>
        /// 調のシャープの個数。
        /// </summary>
        /// <returns>調のシャープの個数。</returns>
        public int SharpNum() {
            return keyProps[(int)internalKey].SharpNum();
        }

        /// <summary>
        /// 調のシャープの個数＋フラットの個数。
        /// </summary>
        /// <returns>調のシャープまたはフラットの個数。</returns>
        public int FlatSharpNum() {
            return FlatNum() + SharpNum();
        }

        /// <summary>
        /// この調でシャープが付く音名のリスト
        /// </summary>
        public List<LN> CreateSharpLetterNameList() {
            return keyProps[(int)internalKey].CreateSharpLetterNameList();
        }

        /// <summary>
        /// この調でフラットが付く音名のリスト
        /// </summary>
        public List<LN> CreateFlatLetterNameList() {
            return keyProps[(int)internalKey].CreateFlatLetterNameList();
        }

        /// <summary>
        /// この調でダブルシャープが付く音名のリスト
        /// </summary>
        public List<LN> CreateDoubleSharpLetterNameList() {
            return keyProps[(int)internalKey].CreateDoubleSharpLetterNameList();
        }

        /// <summary>
        /// 近親調を戻す。
        /// degree=CD.V…属調(V調)を戻す
        /// </summary>
        public MusicKey RelatedKey(CD degree) {
            return keyProps[(int)internalKey].RelatedKey(degree);
        }

        // 主調mainKeyと内属記号keyRelationのペアから調keyを決定する
        public static MusicKey MainKeyAndKeyRelationToKey(MusicKey mainKey, KeyRelation keyRelation) {
            for (int internalKey = 0; internalKey < (int)MusicKey.NUM; ++internalKey) {
                if (keyRelation == mainKeyInternalKeyToKeyRelation[(int)mainKey * (int)MusicKey.NUM + internalKey]) {
                    return (MusicKey)internalKey;
                }
            }

            System.Console.WriteLine("MainKeyAndKeyRelationToKey not found {0} {1}", mainKey, keyRelation);
            return MusicKey.Invalid;
        }

        public static KeyRelation MusicKeyAndChordDegreeToKeyRelation(MusicKey key, CD chordDegree) {
            MusicKeyInfo mki = new MusicKeyInfo(key, KeyRelation.I調);

            KeyRelation kr = KeyRelation.Unknown;
            switch (chordDegree) {
            case CD.I: kr = KeyRelation.I調; break;
            case CD.II: /* 長調の時はII調、短調の時はない。 */
                if (mki.IsMajor()) { kr = KeyRelation.II調; }
                break; 
            case CD.III: kr = KeyRelation.III調; break;
            case CD.IV: kr = KeyRelation.IV調; break;
            case CD.V: kr = KeyRelation.V調; break;
            case CD.VI: kr = KeyRelation.VI調; break;
            case CD.VII: /* 短調の時はVII調、長調の時はない。*/
                if (mki.IsMinor()) { kr = KeyRelation.VII調; }
                break;
            case CD.V_V:/* 長調のV調のV調は＋II調、短調のV調のV調は△II調 */
                if (mki.IsMajor()) { kr = KeyRelation.PII調; }
                else { kr = KeyRelation.TII調; }
                break; 
            default: System.Diagnostics.Debug.Assert(false); break;
            }
            return kr;
        }

        /// <summary>
        /// 和音の属する内部調と、和音の音度から、その和音がIの和音である調を得る。
        /// </summary>
        public static MusicKey MusicKeyAndChordDegreeToInternalKey(MusicKey key, CD chordDegree) {
            System.Diagnostics.Debug.Assert(key != MusicKey.Invalid);
            
            KeyRelation desirableKR = MusicKeyAndChordDegreeToKeyRelation(key, chordDegree);

            MusicKey result = MusicKey.Invalid;
            for (int internalKey = 0; internalKey < (int)MusicKey.NUM; ++internalKey) {
                if (desirableKR == mainKeyInternalKeyToKeyRelation[(int)key * (int)MusicKey.NUM + internalKey ]) {
                    result = (MusicKey)internalKey;
                    break;
                }
            }
            
            return result;
        }

        // 主調musicKeyと内部調の調名chordKeyのペアから内属記号を得る。
        public static KeyRelation MainKeynameAndChordKeynametoKeyRelation(MusicKey mainKey, MusicKey chordKey) {
            System.Diagnostics.Debug.Assert(mainKey != MusicKey.Invalid);
            System.Diagnostics.Debug.Assert(chordKey != MusicKey.Invalid);

            return mainKeyInternalKeyToKeyRelation[ (int)mainKey * (int)MusicKey.NUM + (int)chordKey ];
        }

        static void CreateMainKeyInternalKeyToKeyRelationTable() {
            mainKeyInternalKeyToKeyRelation = new KeyRelation[(int)MusicKey.NUM * (int)MusicKey.NUM];
            for (int i = 0; i < (int)MusicKey.NUM; ++i) {
                for (int j = 0; j < (int)MusicKey.NUM; ++j) {
                    mainKeyInternalKeyToKeyRelation[i * (int)MusicKey.NUM + j] = KeyRelation.Unknown;
                }
            }

            KeyRelation[] majorList = { KeyRelation.MCMIIp調,
                                        KeyRelation.Unknown,
                                        KeyRelation.MCVIp調,
                                        KeyRelation.Unknown,
                                        KeyRelation.MCIIIp調,
                                        KeyRelation.Unknown,
                                        KeyRelation.CMII調,
                                        KeyRelation.MCMII調,
                                        KeyRelation.CVI調,
                                        KeyRelation.MCVI調,
                                        KeyRelation.CIII調,
                                        KeyRelation.MCIII調,
                                        KeyRelation.CVII調,
                                        KeyRelation.MCVII調,
                                        KeyRelation.IV調,
                                        KeyRelation.CIV調,
                                        KeyRelation.I調,
                                        KeyRelation.CI調,
                                        KeyRelation.V調,
                                        KeyRelation.CV調,
                                        KeyRelation.PII調,
                                        KeyRelation.II調,
                                        KeyRelation.PVI調,
                                        KeyRelation.VI調,
                                        KeyRelation.PIII調,
                                        KeyRelation.III調,
                                        KeyRelation.PVIIU調,
                                        KeyRelation.VIIU調,
                                        KeyRelation.Unknown,
                                        KeyRelation.PVIp調,
                                        KeyRelation.Unknown,
                                        KeyRelation.PIIIp調,
                                        KeyRelation.Unknown,
                                        KeyRelation.PVIIUp調};

            KeyRelation[] minorList = { KeyRelation.MMIIp調,
                                        KeyRelation.Unknown,
                                        KeyRelation.MVIp調,
                                        KeyRelation.Unknown,
                                        KeyRelation.MIIIp調,
                                        KeyRelation.Unknown,
                                        KeyRelation.MII調,
                                        KeyRelation.MMII調,
                                        KeyRelation.VI調,
                                        KeyRelation.MVI調,
                                        KeyRelation.III調,
                                        KeyRelation.MIII調,
                                        KeyRelation.VII調,
                                        KeyRelation.MVII調,
                                        KeyRelation.PIV調,
                                        KeyRelation.IV調,
                                        KeyRelation.PI調,
                                        KeyRelation.I調,
                                        KeyRelation.PV調,
                                        KeyRelation.V調,
                                        KeyRelation.PTII調,
                                        KeyRelation.TII調,
                                        KeyRelation.PTVI調,
                                        KeyRelation.TVI調,
                                        KeyRelation.PTIII調,
                                        KeyRelation.TIII調,
                                        KeyRelation.PTVIIU調,
                                        KeyRelation.TVIIU調,
                                        KeyRelation.Unknown,
                                        KeyRelation.PTVIp調,
                                        KeyRelation.Unknown,
                                        KeyRelation.PTIIIp調,
                                        KeyRelation.Unknown,
                                        KeyRelation.PTVIIUp調 };

            int majorToMinorOffset = ((int)MusicKey.FESESmoll - (int)MusicKey.FESESdur);
            {   // 主調が長調
                int startPos = 16;
                for (int mainKey = (int)MusicKey.FESESdur; mainKey <= (int)MusicKey.HISISdur; ++mainKey) {
                    int startPosI = startPos;
                    for (int internalKey = (int)MusicKey.FESESdur; internalKey <= (int)MusicKey.HISISdur; ++internalKey) {
                        if (0 <= startPosI && startPosI < majorList.Length) {
                            //                                 主調                        内部調(長調)              内属記号
                            mainKeyInternalKeyToKeyRelation[mainKey * (int)MusicKey.NUM + internalKey] = majorList[startPosI];

                            //                                 主調                        内部調(短調)                      内属記号
                            mainKeyInternalKeyToKeyRelation[mainKey * (int)MusicKey.NUM + internalKey + majorToMinorOffset] = majorList[startPosI+1];
                        }
                        startPosI+=2;
                    }
                    startPos -= 2;
                }
            }
            {   // 主調が短調
                int startPos = 16;
                for (int mainKey = (int)MusicKey.FESESmoll; mainKey <= (int)MusicKey.HISISmoll; ++mainKey) {
                    int startPosI = startPos;
                    for (int internalKey = (int)MusicKey.FESESdur; internalKey <= (int)MusicKey.HISISdur; ++internalKey) {
                        if (0 <= startPosI && startPosI < minorList.Length) {
                            //                                 主調                        内部調(長調)              内属記号
                            mainKeyInternalKeyToKeyRelation[mainKey * (int)MusicKey.NUM + internalKey] = minorList[startPosI];

                            //                                 主調                        内部調(短調)                      内属記号
                            mainKeyInternalKeyToKeyRelation[mainKey * (int)MusicKey.NUM + internalKey + majorToMinorOffset] = minorList[startPosI+1];
                        }
                        startPosI+=2;
                    }
                    startPos -= 2;
                }
            }

            /*
            // 調関係デバッグ表示。
            for (int mainKey = (int)MusicKey.FESESdur; mainKey <= (int)MusicKey.HISISdur; ++mainKey) {
                Console.Write("{0}: ", (MusicKey)mainKey);
                for (int internalKey = (int)MusicKey.FESESdur; internalKey <= (int)MusicKey.HISISdur; ++internalKey) {
                    System.Console.Write("{0} ", mainKeyInternalKeyToKeyRelation[mainKey * (int)MusicKey.NUM + internalKey]);
                }
                Console.WriteLine("");
            }
            for (int mainKey = (int)MusicKey.FESESdur; mainKey <= (int)MusicKey.HISISdur; ++mainKey) {
                Console.Write("{0}: ", (MusicKey)mainKey);
                for (int internalKey = (int)MusicKey.FESESmoll; internalKey <= (int)MusicKey.HISISmoll; ++internalKey) {
                    System.Console.Write("{0} ", mainKeyInternalKeyToKeyRelation[mainKey * (int)MusicKey.NUM + internalKey]);
                }
                Console.WriteLine("");
            }
            for (int mainKey = (int)MusicKey.FESESmoll; mainKey <= (int)MusicKey.HISISmoll; ++mainKey) {
                Console.Write("{0}: ", (MusicKey)mainKey);
                for (int internalKey = (int)MusicKey.FESESdur; internalKey <= (int)MusicKey.HISISdur; ++internalKey) {
                    System.Console.Write("{0} ", mainKeyInternalKeyToKeyRelation[mainKey * (int)MusicKey.NUM + internalKey]);
                }
                Console.WriteLine("");
            }
            for (int mainKey = (int)MusicKey.FESESmoll; mainKey <= (int)MusicKey.HISISmoll; ++mainKey) {
                Console.Write("{0}: ", (MusicKey)mainKey);
                for (int internalKey = (int)MusicKey.FESESmoll; internalKey <= (int)MusicKey.HISISmoll; ++internalKey) {
                    System.Console.Write("{0} ", mainKeyInternalKeyToKeyRelation[mainKey * (int)MusicKey.NUM + internalKey]);
                }
                Console.WriteLine("");
            }
            */

        }

        static MusicKeyInfo() {
            // 主調と内部調→内属記号の表を作成。
            CreateMainKeyInternalKeyToKeyRelationTable();


            // 音名と旋法→調名
            lnModeKey = new MusicKey[(int)LN.NUM*(int)Mode.NUM];
            for (int i=0; i<(int)LN.NUM; ++i) {
                for (int j=0; j<(int)Mode.NUM; ++j) {
                    lnModeKey[i*(int)Mode.NUM + j] = MusicKey.Invalid;
                }
            }
            lnModeKey[(int)LN.FESES * (int)Mode.NUM + (int)Mode.Dur] = MusicKey.FESESdur;
            lnModeKey[(int)LN.FESES * (int)Mode.NUM + (int)Mode.Moll] = MusicKey.FESESmoll;
            lnModeKey[(int)LN.CESES*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.CESESdur;
            lnModeKey[(int)LN.CESES*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.CESESmoll;
            lnModeKey[(int)LN.GESES*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.GESESdur;
            lnModeKey[(int)LN.GESES*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.GESESmoll;
            lnModeKey[(int)LN.DESES*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.DESESdur;
            lnModeKey[(int)LN.DESES*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.DESESmoll;
            lnModeKey[(int)LN.ASES*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.ASESdur;
            lnModeKey[(int)LN.ASES*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.ASESmoll;
            lnModeKey[(int)LN.ESES*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.ESESdur;
            lnModeKey[(int)LN.ESES*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.ESESmoll;
            lnModeKey[(int)LN.HESES*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.BESdur;
            lnModeKey[(int)LN.HESES*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.BESmoll;

            lnModeKey[(int)LN.FES*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.FESdur;
            lnModeKey[(int)LN.FES*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.FESmoll;
            lnModeKey[(int)LN.CES*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.CESdur;
            lnModeKey[(int)LN.CES*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.CESmoll;
            lnModeKey[(int)LN.GES*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.GESdur;
            lnModeKey[(int)LN.GES*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.GESmoll;
            lnModeKey[(int)LN.DES*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.DESdur;
            lnModeKey[(int)LN.DES*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.DESmoll;
            lnModeKey[(int)LN.AS*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.ASdur;
            lnModeKey[(int)LN.AS*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.ASmoll;
            lnModeKey[(int)LN.ES*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.ESdur;
            lnModeKey[(int)LN.ES*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.ESmoll;
            lnModeKey[(int)LN.B*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.Bdur;
            lnModeKey[(int)LN.B*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.Bmoll;

            lnModeKey[(int)LN.F*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.Fdur;
            lnModeKey[(int)LN.F*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.Fmoll;
            lnModeKey[(int)LN.C*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.Cdur;
            lnModeKey[(int)LN.C*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.Cmoll;
            lnModeKey[(int)LN.G*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.Gdur;
            lnModeKey[(int)LN.G*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.Gmoll;
            lnModeKey[(int)LN.D*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.Ddur;
            lnModeKey[(int)LN.D*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.Dmoll;
            lnModeKey[(int)LN.A*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.Adur;
            lnModeKey[(int)LN.A*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.Amoll;
            lnModeKey[(int)LN.E*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.Edur;
            lnModeKey[(int)LN.E*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.Emoll;
            lnModeKey[(int)LN.H*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.Hdur;
            lnModeKey[(int)LN.H*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.Hmoll;

            lnModeKey[(int)LN.FIS*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.FISdur;
            lnModeKey[(int)LN.FIS*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.FISmoll;
            lnModeKey[(int)LN.CIS*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.CISdur;
            lnModeKey[(int)LN.CIS*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.CISmoll;
            lnModeKey[(int)LN.GIS*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.GISdur;
            lnModeKey[(int)LN.GIS*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.GISmoll;
            lnModeKey[(int)LN.DIS*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.DISdur;
            lnModeKey[(int)LN.DIS*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.DISmoll;
            lnModeKey[(int)LN.AIS*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.AISdur;
            lnModeKey[(int)LN.AIS*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.AISmoll;
            lnModeKey[(int)LN.EIS*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.EISdur;
            lnModeKey[(int)LN.EIS*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.EISmoll;
            lnModeKey[(int)LN.HIS*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.HISdur;
            lnModeKey[(int)LN.HIS*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.HISmoll;

            lnModeKey[(int)LN.FISIS*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.FISISdur;
            lnModeKey[(int)LN.FISIS*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.FISISmoll;
            lnModeKey[(int)LN.CISIS*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.CISISdur;
            lnModeKey[(int)LN.CISIS*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.CISISmoll;
            lnModeKey[(int)LN.GISIS*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.GISISdur;
            lnModeKey[(int)LN.GISIS*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.GISISmoll;
            lnModeKey[(int)LN.DISIS*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.DISISdur;
            lnModeKey[(int)LN.DISIS*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.DISISmoll;
            lnModeKey[(int)LN.AISIS*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.AISISdur;
            lnModeKey[(int)LN.AISIS*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.AISISmoll;
            lnModeKey[(int)LN.EISIS*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.EISISdur;
            lnModeKey[(int)LN.EISIS*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.EISISmoll;
            lnModeKey[(int)LN.HISIS*(int)Mode.NUM + (int)Mode.Dur] = MusicKey.HISISdur;
            lnModeKey[(int)LN.HISIS*(int)Mode.NUM + (int)Mode.Moll] = MusicKey.HISISmoll;

            keyProps = new MusicKeyProperties[(int)MusicKey.NUM];

            //                                                                                     F  S
            keyProps[(int)MusicKey.FESESdur].Init(MusicKey.FESESdur, LN.FESES, "Feses", Mode.Dur, 15, 0);
            keyProps[(int)MusicKey.CESESdur].Init(MusicKey.CESESdur, LN.CESES, "Ceses", Mode.Dur, 14, 0);
            keyProps[(int)MusicKey.GESESdur].Init(MusicKey.GESESdur, LN.GESES, "Geses", Mode.Dur, 13, 0);
            keyProps[(int)MusicKey.DESESdur].Init(MusicKey.DESESdur, LN.DESES, "Deses", Mode.Dur, 12, 0);
            keyProps[(int)MusicKey.ASESdur].Init(MusicKey.ASESdur, LN.ASES, "Ases", Mode.Dur, 11, 0);
            keyProps[(int)MusicKey.ESESdur].Init(MusicKey.ESESdur, LN.ESES, "Eses", Mode.Dur, 10, 0);
            keyProps[(int)MusicKey.BESdur].Init(MusicKey.BESdur, LN.HESES, "Heses", Mode.Dur, 9, 0);

            //                                                                            F  S
            keyProps[(int)MusicKey.FESdur].Init(MusicKey.FESdur, LN.FES, "Fes", Mode.Dur, 8, 0);
            keyProps[(int)MusicKey.CESdur].Init(MusicKey.CESdur, LN.CES, "Ces", Mode.Dur, 7, 0);
            keyProps[(int)MusicKey.GESdur].Init(MusicKey.GESdur, LN.GES, "Ges", Mode.Dur, 6, 0);
            keyProps[(int)MusicKey.DESdur].Init(MusicKey.DESdur, LN.DES, "Des", Mode.Dur, 5, 0);
            keyProps[(int)MusicKey.ASdur].Init(MusicKey.ASdur, LN.AS, "As", Mode.Dur, 4, 0);
            keyProps[(int)MusicKey.ESdur].Init(MusicKey.ESdur, LN.ES, "Es", Mode.Dur, 3, 0);
            keyProps[(int)MusicKey.Bdur].Init(MusicKey.Bdur, LN.B, "B", Mode.Dur, 2, 0);

            //                                                                    F  S
            keyProps[(int)MusicKey.Fdur].Init(MusicKey.Fdur, LN.F, "F", Mode.Dur, 1, 0);
            keyProps[(int)MusicKey.Cdur].Init(MusicKey.Cdur, LN.C, "C", Mode.Dur, 0, 0);
            keyProps[(int)MusicKey.Gdur].Init(MusicKey.Gdur, LN.G, "G", Mode.Dur, 0, 1);
            keyProps[(int)MusicKey.Ddur].Init(MusicKey.Ddur, LN.D, "D", Mode.Dur, 0, 2);
            keyProps[(int)MusicKey.Adur].Init(MusicKey.Adur, LN.A, "A", Mode.Dur, 0, 3);
            keyProps[(int)MusicKey.Edur].Init(MusicKey.Edur, LN.E, "E", Mode.Dur, 0, 4);
            keyProps[(int)MusicKey.Hdur].Init(MusicKey.Hdur, LN.H, "H", Mode.Dur, 0, 5);

            //                                                                            F  S
            keyProps[(int)MusicKey.FISdur].Init(MusicKey.FISdur, LN.FIS, "Fis", Mode.Dur, 0, 6);
            keyProps[(int)MusicKey.CISdur].Init(MusicKey.CISdur, LN.CIS, "Cis", Mode.Dur, 0, 7);
            keyProps[(int)MusicKey.GISdur].Init(MusicKey.GISdur, LN.GIS, "Gis", Mode.Dur, 0, 8);
            keyProps[(int)MusicKey.DISdur].Init(MusicKey.DISdur, LN.DIS, "Dis", Mode.Dur, 0, 9);
            keyProps[(int)MusicKey.AISdur].Init(MusicKey.AISdur, LN.AIS, "Ais", Mode.Dur, 0, 10);
            keyProps[(int)MusicKey.EISdur].Init(MusicKey.EISdur, LN.EIS, "Eis", Mode.Dur, 0, 11);
            keyProps[(int)MusicKey.HISdur].Init(MusicKey.HISdur, LN.HIS, "His", Mode.Dur, 0, 12);

            //                                                                                    F  S
            keyProps[(int)MusicKey.FISISdur].Init(MusicKey.FISISdur, LN.FISIS, "Fisis", Mode.Dur, 0, 13);
            keyProps[(int)MusicKey.CISISdur].Init(MusicKey.CISISdur, LN.CISIS, "Cisis", Mode.Dur, 0, 14);
            keyProps[(int)MusicKey.GISISdur].Init(MusicKey.GISISdur, LN.GISIS, "Gisis", Mode.Dur, 0, 15);
            keyProps[(int)MusicKey.DISISdur].Init(MusicKey.DISISdur, LN.DISIS, "Disis", Mode.Dur, 0, 16);
            keyProps[(int)MusicKey.AISISdur].Init(MusicKey.AISISdur, LN.AISIS, "Aisis", Mode.Dur, 0, 17);
            keyProps[(int)MusicKey.EISISdur].Init(MusicKey.EISISdur, LN.EISIS, "Eisis", Mode.Dur, 0, 18);
            keyProps[(int)MusicKey.HISISdur].Init(MusicKey.HISISdur, LN.HISIS, "Hisis", Mode.Dur, 0, 19);

            //                                                                                        F  S
            keyProps[(int)MusicKey.FESESmoll].Init(MusicKey.FESESmoll, LN.FESES, "feses", Mode.Moll, 18, 0);
            keyProps[(int)MusicKey.CESESmoll].Init(MusicKey.CESESmoll, LN.CESES, "ceses", Mode.Moll, 17, 0);
            keyProps[(int)MusicKey.GESESmoll].Init(MusicKey.GESESmoll, LN.GESES, "geses", Mode.Moll, 16, 0);
            keyProps[(int)MusicKey.DESESmoll].Init(MusicKey.DESESmoll, LN.DESES, "deses", Mode.Moll, 15, 0);
            keyProps[(int)MusicKey.ASESmoll].Init(MusicKey.ASESmoll, LN.ASES, "ases", Mode.Moll, 14, 0);
            keyProps[(int)MusicKey.ESESmoll].Init(MusicKey.ESESmoll, LN.ESES, "eses", Mode.Moll, 13, 0);
            keyProps[(int)MusicKey.BESmoll].Init(MusicKey.BESmoll, LN.HESES, "heses", Mode.Moll, 12, 0);

            //                                                                                F  S
            keyProps[(int)MusicKey.FESmoll].Init(MusicKey.FESmoll, LN.FES, "fes", Mode.Moll, 11, 0);
            keyProps[(int)MusicKey.CESmoll].Init(MusicKey.CESmoll, LN.CES, "ces", Mode.Moll, 10, 0);
            keyProps[(int)MusicKey.GESmoll].Init(MusicKey.GESmoll, LN.GES, "ges", Mode.Moll, 9, 0);
            keyProps[(int)MusicKey.DESmoll].Init(MusicKey.DESmoll, LN.DES, "des", Mode.Moll, 8, 0);
            keyProps[(int)MusicKey.ASmoll].Init(MusicKey.ASmoll, LN.AS, "as", Mode.Moll, 7, 0);
            keyProps[(int)MusicKey.ESmoll].Init(MusicKey.ESmoll, LN.ES, "es", Mode.Moll, 6, 0);
            keyProps[(int)MusicKey.Bmoll].Init(MusicKey.Bmoll, LN.B, "b", Mode.Moll, 5, 0);

            //                                                                       F  S
            keyProps[(int)MusicKey.Fmoll].Init(MusicKey.Fmoll, LN.F, "f", Mode.Moll, 4, 0);
            keyProps[(int)MusicKey.Cmoll].Init(MusicKey.Cmoll, LN.C, "c", Mode.Moll, 3, 0);
            keyProps[(int)MusicKey.Gmoll].Init(MusicKey.Gmoll, LN.G, "g", Mode.Moll, 2, 0);
            keyProps[(int)MusicKey.Dmoll].Init(MusicKey.Dmoll, LN.D, "d", Mode.Moll, 1, 0);
            keyProps[(int)MusicKey.Amoll].Init(MusicKey.Amoll, LN.A, "a", Mode.Moll, 0, 0);
            keyProps[(int)MusicKey.Emoll].Init(MusicKey.Emoll, LN.E, "e", Mode.Moll, 0, 1);
            keyProps[(int)MusicKey.Hmoll].Init(MusicKey.Hmoll, LN.H, "h", Mode.Moll, 0, 2);

            //                                                                               F  S
            keyProps[(int)MusicKey.FISmoll].Init(MusicKey.FISmoll, LN.FIS, "fis", Mode.Moll, 0, 3);
            keyProps[(int)MusicKey.CISmoll].Init(MusicKey.CISmoll, LN.CIS, "cis", Mode.Moll, 0, 4);
            keyProps[(int)MusicKey.GISmoll].Init(MusicKey.GISmoll, LN.GIS, "gis", Mode.Moll, 0, 5);
            keyProps[(int)MusicKey.DISmoll].Init(MusicKey.DISmoll, LN.DIS, "dis", Mode.Moll, 0, 6);
            keyProps[(int)MusicKey.AISmoll].Init(MusicKey.AISmoll, LN.AIS, "ais", Mode.Moll, 0, 7);
            keyProps[(int)MusicKey.EISmoll].Init(MusicKey.EISmoll, LN.EIS, "eis", Mode.Moll, 0, 8);
            keyProps[(int)MusicKey.HISmoll].Init(MusicKey.HISmoll, LN.HIS, "his", Mode.Moll, 0, 9);

            //                                                                                       F  S
            keyProps[(int)MusicKey.FISISmoll].Init(MusicKey.FISISmoll, LN.FISIS, "fisis", Mode.Moll, 0, 10);
            keyProps[(int)MusicKey.CISISmoll].Init(MusicKey.CISISmoll, LN.CISIS, "cisis", Mode.Moll, 0, 11);
            keyProps[(int)MusicKey.GISISmoll].Init(MusicKey.GISISmoll, LN.GISIS, "gisis", Mode.Moll, 0, 12);
            keyProps[(int)MusicKey.DISISmoll].Init(MusicKey.DISISmoll, LN.DISIS, "disis", Mode.Moll, 0, 13);
            keyProps[(int)MusicKey.AISISmoll].Init(MusicKey.AISISmoll, LN.AISIS, "aisis", Mode.Moll, 0, 14);
            keyProps[(int)MusicKey.EISISmoll].Init(MusicKey.EISISmoll, LN.EISIS, "eisis", Mode.Moll, 0, 15);
            keyProps[(int)MusicKey.HISISmoll].Init(MusicKey.HISISmoll, LN.HISIS, "hisis", Mode.Moll, 0, 16);

            // I～VIIの各音度、固有和音調。
            for (int i = (int)MusicKey.FESESdur; i <= (int)MusicKey.HISISdur; ++i) {
                MusicKeyProperties prop = keyProps[i];
                LetterName l1, l2, l3, l4, l5, l6, l7;
                l1 = new LetterName(prop.GetFirstDegreeLN());
                l2 = l1.NextSemi2();
                l3 = l2.NextSemi2();
                l4 = l3.NextSemi1();
                l5 = l4.NextSemi2();
                l6 = l5.NextSemi2();
                l7 = l6.NextSemi2();
                keyProps[i].SetLnList(l2.LN, l3.LN, l4.LN, l5.LN, l6.LN, l7.LN);

                MusicKey k2, k3, k4, k5, k6;
                k2 = lnModeKey[(int)l2.LN*(int)Mode.NUM + (int)Mode.Moll];
                k3 = lnModeKey[(int)l3.LN*(int)Mode.NUM + (int)Mode.Moll];
                k4 = lnModeKey[(int)l4.LN*(int)Mode.NUM + (int)Mode.Dur];
                k5 = lnModeKey[(int)l5.LN*(int)Mode.NUM + (int)Mode.Dur];
                k6 = lnModeKey[(int)l6.LN*(int)Mode.NUM + (int)Mode.Moll];
                keyProps[i].SetRelatedKeys(k2, k3, k4, k5, k6, MusicKey.Invalid);

                MusicKey p1, p3, p4, p5, p6, p7;
                p1 = lnModeKey[(int)l1.LN*(int)Mode.NUM + (int)Mode.Moll];
                p3 = lnModeKey[(int)l3.LN*(int)Mode.NUM + (int)Mode.Dur];
                p4 = lnModeKey[(int)l4.LN*(int)Mode.NUM + (int)Mode.Moll];
                p5 = lnModeKey[(int)l5.LN*(int)Mode.NUM + (int)Mode.Moll];
                p6 = lnModeKey[(int)l6.LN*(int)Mode.NUM + (int)Mode.Dur];
                p7 = lnModeKey[(int)l6.LN*(int)Mode.NUM + (int)Mode.Dur];
                keyProps[i].SetParallelRelatedKeys(p1, MusicKey.Invalid, p3, p4, p5, p6, p7);
            }

            for (int i = (int)MusicKey.FESESmoll; i <= (int)MusicKey.HISISmoll; ++i) {
                MusicKeyProperties prop = keyProps[i];
                LetterName l1, l2, l3, l4, l5, l6, l7;
                l1 = new LetterName(prop.GetFirstDegreeLN());
                l2 = l1.NextSemi2();
                l3 = l2.NextSemi1();
                l4 = l3.NextSemi2();
                l5 = l4.NextSemi2();
                l6 = l5.NextSemi1();
                l7 = l6.NextSemi2();
                keyProps[i].SetLnList(l2.LN, l3.LN, l4.LN, l5.LN, l6.LN, l7.LN);

                MusicKey p3, p4, p5, p6, p7;
                p3 = lnModeKey[(int)l3.LN*(int)Mode.NUM + (int)Mode.Dur];
                p4 = lnModeKey[(int)l4.LN*(int)Mode.NUM + (int)Mode.Moll];
                p5 = lnModeKey[(int)l5.LN*(int)Mode.NUM + (int)Mode.Moll];
                p6 = lnModeKey[(int)l6.LN*(int)Mode.NUM + (int)Mode.Dur];
                p7 = lnModeKey[(int)l6.LN*(int)Mode.NUM + (int)Mode.Dur];
                keyProps[i].SetRelatedKeys(MusicKey.Invalid, p3, p4, p5, p6, p7);

                MusicKey k1, k2, k3, k4, k5, k6;
                k1 = lnModeKey[(int)l1.LN*(int)Mode.NUM + (int)Mode.Dur];
                k2 = lnModeKey[(int)l2.LN*(int)Mode.NUM + (int)Mode.Moll];
                k3 = lnModeKey[(int)l3.LN*(int)Mode.NUM + (int)Mode.Moll];
                k4 = lnModeKey[(int)l4.LN*(int)Mode.NUM + (int)Mode.Dur];
                k5 = lnModeKey[(int)l5.LN*(int)Mode.NUM + (int)Mode.Dur];
                k6 = lnModeKey[(int)l6.LN*(int)Mode.NUM + (int)Mode.Moll];
                keyProps[i].SetParallelRelatedKeys(k1, k2, k3, k4, k5, k6, MusicKey.Invalid);
            }
        }
    }
}
