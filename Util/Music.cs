using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Threading.Tasks;

namespace FourPartHarmony2
{
    /// <summary>
    /// 和声体。
    /// </summary>
    public class Music
    {
        private List<Chord> m_chordList;

        public int FileVersion { get; set; }
        public int Tempo { get; set; }

        public Music()
        {
            m_chordList = new List<Chord>();
            Tempo = 75;
        }

        // /////////////////////////////////////////////////////////////////////////////
        // こまごまとした機能

        private List<Chord> GenerateAllChordList(ChordType chordType) {
            List<Chord> result = new List<Chord>();
            ChordType ct = new ChordType();
            ct.musicKey    = chordType.musicKey;
            ct.keyRelation = chordType.keyRelation;
            ct.chordDegree = chordType.chordDegree;
            ct.omission    = Omission.None;
            ct.termination = chordType.termination;
            ct.is準固有    = chordType.is準固有;
            ct.has固有VII  = chordType.has固有VII;
            ct.alteration  = chordType.alteration;
            ct.addedTone   = chordType.addedTone;

            // 3和音
            if (ct.is準固有 && (ct.chordDegree == CD.V || ct.chordDegree == CD.V_V)) {
                // 準固有のVの3和音は存在しない
            } else {
                ct.numberOfNotes = NumberOfNotes.Triad;
                for (int inv = (int)Inversion.根音; inv <= (int)Inversion.第5音; ++inv) {
                    ct.bassInversion = (Inversion)inv;
                    if (ct.bassInversion != Inversion.根音 &&
                        ct.addedTone != AddedToneType.None) {
                        // 付加和音は[基]だけ
                        break;
                    }

                    for (int poa = (int)PositionOfAChord.密; poa <= (int)PositionOfAChord.Oct; ++poa) {
                        ct.positionOfAChord = (PositionOfAChord)poa;

                        List<Chord> ret = ChordListFactory.ObtainChordList(ct);
                        result.AddRange(ret);
                    }
                }
            }

            if (ct.chordDegree == CD.II ||
                (ct.chordDegree == CD.IV &&
                ct.addedTone == AddedToneType.None)) {
                // II7 IV7(付加和音なしの場合のみ)
                for (int inv = (int)Inversion.根音; inv <= (int)Inversion.第7音; ++inv) {
                    ct.bassInversion = (Inversion)inv;
                    ct.numberOfNotes = NumberOfNotes.Seventh;
                    for (int poa = (int)PositionOfAChord.密; poa <= (int)PositionOfAChord.Oct; ++poa) {
                        ct.positionOfAChord = (PositionOfAChord)poa;

                        List<Chord> ret = ChordListFactory.ObtainChordList(ct);
                        result.AddRange(ret);
                    }
                }
            }
            if (ct.chordDegree == CD.I ||
                ct.chordDegree == CD.III  ||
                ct.chordDegree == CD.VI  ||
                ct.chordDegree == CD.VII) {
                // I7 III7 VI7 VII7 IIIp216 77 1)
                for (int inv = (int)Inversion.根音; inv <= (int)Inversion.第7音; ++inv) {
                    ct.bassInversion = (Inversion)inv;
                    ct.numberOfNotes = NumberOfNotes.Seventh;
                    for (int poa = (int)PositionOfAChord.密; poa <= (int)PositionOfAChord.開; ++poa) {
                        ct.positionOfAChord = (PositionOfAChord)poa;

                        List<Chord> ret = ChordListFactory.ObtainChordList(ct);
                        result.AddRange(ret);
                    }
                }
            }

            if (ct.chordDegree == CD.V ||
                ct.chordDegree == CD.V_V) {
                // V7
                if (ct.is準固有) {
                    // 準固有のV7の和音は存在しない(第9音が含まれないと何も変わらないので)
                } else {
                    for (int inv = (int)Inversion.根音; inv <= (int)Inversion.第7音; ++inv) {
                        ct.bassInversion = (Inversion)inv;
                        ct.numberOfNotes = NumberOfNotes.Seventh;
                        for (int poa = (int)PositionOfAChord.密; poa <= (int)PositionOfAChord.Oct; ++poa) {
                            ct.positionOfAChord = (PositionOfAChord)poa;

                            List<Chord> ret = ChordListFactory.ObtainChordList(ct);
                            result.AddRange(ret);
                        }
                    }

                    for (int inv = (int)Inversion.第3音; inv <= (int)Inversion.第7音; ++inv) {
                        // [n転]V7根省
                        ct.omission = Omission.First;
                        ct.bassInversion = (Inversion)inv;
                        ct.numberOfNotes = NumberOfNotes.Seventh;
                        for (int poa = (int)PositionOfAChord.密; poa <= (int)PositionOfAChord.Oct; ++poa) {
                            ct.positionOfAChord = (PositionOfAChord)poa;

                            List<Chord> ret = ChordListFactory.ObtainChordList(ct);
                            result.AddRange(ret);
                        }
                    }
                }

                ct.omission = Omission.None;

                // [基]V9
                ct.bassInversion = Inversion.根音;
                ct.numberOfNotes = NumberOfNotes.Ninth;
                for (int poa = (int)PositionOfAChord.密; poa <= (int)PositionOfAChord.Oct; ++poa) {
                    ct.positionOfAChord = (PositionOfAChord)poa;

                    List<Chord> ret = ChordListFactory.ObtainChordList(ct);
                    result.AddRange(ret);
                }

                // [1転]V9根省
                ct.omission = Omission.First;
                ct.bassInversion = Inversion.第3音;
                for (int poa = (int)PositionOfAChord.密; poa <= (int)PositionOfAChord.Oct; ++poa) {
                    ct.positionOfAChord = (PositionOfAChord)poa;

                    List<Chord> ret = ChordListFactory.ObtainChordList(ct);
                    result.AddRange(ret);
                }
                // [2転]V9根省
                ct.omission = Omission.First;
                ct.bassInversion = Inversion.第5音;
                for (int poa = (int)PositionOfAChord.密; poa <= (int)PositionOfAChord.Oct; ++poa) {
                    ct.positionOfAChord = (PositionOfAChord)poa;

                    List<Chord> ret = ChordListFactory.ObtainChordList(ct);
                    result.AddRange(ret);
                }
                // [3転]V9根省
                ct.omission = Omission.First;
                ct.bassInversion = Inversion.第7音;
                for (int poa = (int)PositionOfAChord.密; poa <= (int)PositionOfAChord.Oct; ++poa) {
                    ct.positionOfAChord = (PositionOfAChord)poa;

                    List<Chord> ret = ChordListFactory.ObtainChordList(ct);
                    result.AddRange(ret);
                }
                // 心配なので戻しておく。
                ct.omission = Omission.None;
            }

            return result;
        }

        public enum GenerateChordListType
        {
            OnlySpecified,
            All,
        }

        public List<Chord> GenerateChordListAndSort(GenerateChordListType type, ChordType chordType, Chord prevChord, int chordInsertPosOnMusic) {
            List<Chord> result = null;
            if (type == GenerateChordListType.OnlySpecified) {
                result = ChordListFactory.ObtainChordList(chordType);
            } else {
                result = GenerateAllChordList(chordType);
            }
            CheckValidityAndUpdateFunction(result, chordInsertPosOnMusic);

            result.Sort(new ChordComparer(prevChord));
            return result;
        }

        public double InspectSuggestQuality() {
            int quality = 0;

            Chord prevChord = m_chordList[0];
            for (int n=1; n < m_chordList.Count; ++n) {
                Chord c = m_chordList[n];

                ChordType ct = c.ChordType;
                var cl = GenerateChordListAndSort(GenerateChordListType.OnlySpecified, ct, prevChord, n);
                int idx=0;
                for (; idx < cl.Count; ++idx) {
                    if (cl[idx].PitchWeightedAccumulate() == c.PitchWeightedAccumulate()) {
                        prevChord = c;
                        break;
                    }
                }
                System.Diagnostics.Debug.Assert(idx != cl.Count);

                Console.WriteLine("  {0} q={1}", n, idx);
                quality += idx * idx;
            }

            return (double)quality / (m_chordList.Count -1);
        }

        public void Insert(int pos, Chord nowC)
        {
            m_chordList.Insert(pos, nowC);
            AllCheckVaridityAndUpdateFunction();
        }

        public void DeleteChordAt(int pos)
        {
            if (m_chordList.Count <= pos) {
                return;
            }
            m_chordList.RemoveAt(pos);
            AllCheckVaridityAndUpdateFunction();
        }

        public int GetNumOfChords()
        {
            return m_chordList.Count;
        }

        public Chord GetChord(int pos)
        {
            return m_chordList[pos];
        }

        public List<Chord> GetChordList()
        {
            return m_chordList;
        }

        public Chord GetLastChord()
        {
            if (m_chordList.Count == 0) {
                return null;
            }
            return m_chordList[m_chordList.Count - 1];
        }

        public Music DeepCopy()
        {
            Music result = new Music();
            result.Tempo = Tempo;
            for (int i=0; i<m_chordList.Count; ++i) {
                Chord c = m_chordList[i];
                result.Insert(i, c);
            }
            return result;
        }

        public MidiManager ToMidiFile()
        {
            return Chord.ChordListToMidiFile(m_chordList, Tempo);
        }

        private bool DelistChordPred(Chord c) {
            return c.Verdict == VerdictValue.Delist;
        }
        private bool DelistProgPred(Progression p) {
            return p.NowC.Verdict == VerdictValue.Delist;
        }

        /// <summary>
        /// m_chordListのpos位置に新しいchord候補aChordListの各々を挿入したと仮定して、連結の良否を判定する。
        /// </summary>
        private void CheckValidityAndUpdateFunction(List<Chord> aChordList, int chordInsertPosOnMusic) {
            // musicのchordInsertPosOnMusicの手前にchordを挿入した場合の妥当性をチェックする。
            Parallel.For(0, aChordList.Count, delegate(int i) {
                Chord chord = aChordList[i];

                chord.SetChordFunctionRoughly();
                var nowP = new Progression(m_chordList, chordInsertPosOnMusic, chord);
                CheckValidity(nowP);
            });
            aChordList.RemoveAll(DelistChordPred);
        }

        /// <summary>
        /// 実際に挿入、削除を行った後にm_chordList全体をチェックする。
        /// </summary>
        public void AllCheckVaridityAndUpdateFunction() {
            Progression [] progressionArray = new Progression[m_chordList.Count];

            Parallel.For(0, m_chordList.Count, delegate(int i) {
                Chord c = m_chordList[i];
                c.ReevaluateChordVerdictAndChordFunction();

                var nowP = new Progression(m_chordList, i, c);
                CheckValidity(nowP);
                progressionArray[i] = nowP;
            });
            m_chordList.RemoveAll(DelistChordPred);

            var progressionList = new List<Progression>();
            progressionList.AddRange(progressionArray);
            progressionList.RemoveAll(DelistProgPred);

            Parallel.For(0, progressionList.Count, delegate(int i) {
                if (i < 3) {
                    return;
                }

                var preP = progressionList[i - 2];
                var nowP = progressionList[i];
                Check同型のバス定型の連続(preP, nowP);
            });
        }

        ///////////////////////////////////////////////////////////////////////////////
        // 2つのProgressionに関する連結妥当性のチェック

        private void Check同型のバス定型の連続(Progression pre2P, Progression nowP) {
            var d定型0 = pre2P.GetD諸和音定型進行();
            var d定型1 = nowP.GetD諸和音定型進行();

            if (null != d定型0 && null != d定型1 &&
                d定型0.Sop定型進行Idx != d定型1.Sop定型進行Idx &&
                d定型0.Bas定型進行Type == d定型1.Bas定型進行Type &&
                pre2P.NowC.KeyRelation == nowP.NowC.KeyRelation &&
                pre2P.PreC.TerminationType == TerminationType.None &&
                pre2P.NowC.TerminationType == TerminationType.None &&
                nowP.PreC.TerminationType == TerminationType.None) {
                nowP.UpdateVerdict(new Verdict(VerdictValue.NotSoGood, VerdictReason.NotSoGoodIIp121_55_2_a));
            }
        }

        ///////////////////////////////////////////////////////////////////////////////
        // 1個のProgressionに関する妥当性のチェック

        /// <summary>
        /// posの手前にnowCを挿入した場合のnowCの妥当性をチェックする
        /// </summary>
        public void CheckValidity(Progression p) {
            int failCount = 0;

            Chord nowC = p.NowC;

            if (nowC.CheckAlteredChord()) {
                return;
            }

            if (p.PreC == null) {
                // 先行和音はない。連結はまだ無い。
                // 配置の良否をチェックする。
                CheckStartChord(nowC);
                Check配置(nowC);
                return;
            }

            // これらのチェックでAcceptable等をつける場合は、このメソッドの最後に移動する。
            failCount += CheckOver9Progression(p);
            failCount += CheckConsecutiveOctaves(p);
            failCount += CheckLeadingNoteProgression(p); //< Acceptableをつけているが、都合により先行評価する。

            failCount += CheckParallelProgression1(p);
            failCount += CheckOct6toClose5(p);
            failCount += CheckCadence(p);
            failCount += CheckIIp25_13(p);
            failCount += CheckIIp37MajorProgression(p);
            failCount += CheckRuleA3A4(p);

            if (0 == failCount) {
                Check標準連結(p);
            }

            if (p.NowC.NumOfVerdicts == 0) {
                p.UpdateVerdict(new Verdict(VerdictValue.NotSoGood, VerdictReason.NotSoGoodNonStandardProgression));
            }

            // BasとTenが12度以上離れているチェックは、早めにやると「許」がついてしまい、
            // それによってNumOfVerdictsが水増しされてしまうため、いけてないが最後にチェックする
            failCount += Check配置(nowC);
            CheckBasTenInterval(p);
            Check終止定型(p);
            failCount += CheckConsecutiveFifths(p);
            failCount += CheckAugmentedUnisonProgression(p);
            failCount += Check7Progression(p);
            failCount += CheckAugmentedProgression(p);
            failCount += CheckParallelProgression85(p);
            failCount += CheckBasの6度進行(p);
            failCount += CheckBasの5度以上進行(p);
            failCount += CheckBasの2回連続の跳躍進行(p);
            failCount += CheckD諸和音定型進行(p);
            failCount += Check終止定型進行(p);
            if (p.PositionOfAChordChanged()) {
                p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoPositionOfAChordChanged));
            }
        }

        /// <summary>
        /// II巻p117、II巻p95
        /// </summary>
        private int Check終止定型進行(Progression p) {
            int result = 0;

            var lb = p.GetBas終止定式List();
            if (0 < lb.Count) {
                foreach (Part終止定式Info b in lb) {
                    switch (b.Count) {
                    case 2:
                        p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoIIp95_2, b.GetTerminationString(), b.Name,
                            b.Get(0).Get定型和音Name(), b.Get(1).Get定型和音Name()));
                        break;
                    case 3:
                        p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoIIp95_3, b.GetTerminationString(), b.Name,
                            b.Get(0).Get定型和音Name(), b.Get(1).Get定型和音Name(), b.Get(2).Get定型和音Name()));
                        break;
                    case 4:
                        p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoIIp95_4, b.GetTerminationString(), b.Name,
                            b.Get(0).Get定型和音Name(), b.Get(1).Get定型和音Name(), b.Get(2).Get定型和音Name(), b.Get(3).Get定型和音Name()));
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false);
                        break;
                    }
                }
            }

            var ls = p.GetSop終止定式List();
            if (0 == ls.Count) {
                return result;
            }

            int matchedCount = 0;

            foreach (Part終止定式Info s in ls) {
                switch (s.Count) {
                case 2:
                    p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoIIp117_2, s.GetTerminationString(), s.Name,
                        s.Get(0).Get定型和音Name(), s.Get(1).Get定型和音Name()));
                    break;
                case 3:
                    p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoIIp117_3, s.GetTerminationString(), s.Name,
                        s.Get(0).Get定型和音Name(), s.Get(1).Get定型和音Name(), s.Get(2).Get定型和音Name()));
                    break;
                case 4:
                    p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoIIp117_4, s.GetTerminationString(), s.Name,
                        s.Get(0).Get定型和音Name(), s.Get(1).Get定型和音Name(), s.Get(2).Get定型和音Name(), s.Get(3).Get定型和音Name()));
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }

                bool sop和音進行OK = p.CheckChordProgression(s);

                if (0 == lb.Count) {
                    // Basが定型進行ではないが、Sopの旋律は定型進行。
                    if (sop和音進行OK) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoIIp117chordOK, s.Name, s.GetTerminationString()));
                    } else {
                        p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoIIp117chordNotOK, s.Name, s.GetTerminationString()));
                    }
                    return result;
                }

                // Sop定型進行とBas定型進行が揃った。

                Part終止定式Info 対応Bas定型進行 = null;
                for (int i = 0; i < s.対応Bas終止定式IdxCount(); ++i) {
                    int idx = s.対応Bas終止定式IdxGet(i);
                    foreach (Part終止定式Info b in lb) {
                        if (b.Idx == idx) {
                            対応Bas定型進行 = b;
                        }
                    }
                }

                if (null != 対応Bas定型進行) {
                    if (sop和音進行OK) {
                        if (p.NowC.TerminationType == s.終止Type) {
                            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.InfoIIp117chordOKBasOK, s.Name, 対応Bas定型進行.Name, s.GetTerminationString()));
                        } else {
                            p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoIIp117chordOKBasOK, s.Name, 対応Bas定型進行.Name, s.GetTerminationString()));
                        }
                    } else {
                        p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoIIp117chordNotOKBasOK, s.Name, 対応Bas定型進行.Name, s.GetTerminationString()));
                    }
                } else {
                    if (sop和音進行OK) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoIIp117chordOKBasNotOK, s.Name, s.GetTerminationString()));
                    } else {
                        p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoIIp117chordNotOKBasNotOK, s.Name, s.GetTerminationString()));
                    }
                }

                if (sop和音進行OK && null != 対応Bas定型進行 && p.NowC.TerminationType == s.終止Type) {
                    ++matchedCount;
                }
            }

            if (p.NowC.TerminationType != TerminationType.None && 0 == matchedCount) {
                if (p.PreC.AddedTone == AddedToneType.SixFour) {
                    // 付加4-6の和音がなぜかここに来る。
                } else {
                    p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.InfoIIp117TerminationDoesNotMatch));
                    ++result;
                }
            }

            return result;
        }

        /// <summary>
        /// II巻p114
        /// </summary>
        private int CheckD諸和音定型進行(Progression p) {
            int result = 0;

            var d = p.GetD諸和音定型進行();
            if (null == d) {
                return result;
            }

            p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoIIp114, d.Sop定型進行名, d.Bas定型進行名));
            return result;
        }

        /// <summary>
        /// II巻p123
        /// </summary>
        private int CheckBasの2回連続の跳躍進行(Progression p) {
            int result = 0;

            if (p.PrepreC == null ||
                p.PrepreC.TerminationType != TerminationType.None ||
                p.PreC.TerminationType != TerminationType.None) {
                // 終止によって遮られている場合はOK。
                return result;
            }

            int preBasInterval = p.PrePartProgressionHigherInterval(Part.Bas);
            int nowBasInterval = p.PartProgressionHigherInterval(Part.Bas);
            int 合計BasInterval = Math.Abs(preBasInterval + nowBasInterval);
            if (1 < Math.Abs(preBasInterval) &&
                1 < Math.Abs(nowBasInterval) &&
                (0 < preBasInterval * nowBasInterval) && //< 同一方向
                (6 == 合計BasInterval || 8 <= 合計BasInterval)) {
                // 同一方向の跳躍進行が続き、合計7度か9度以上。だめである。
                p.UpdateVerdict(new Verdict(VerdictValue.NotSoGood, VerdictReason.NotSoGoodBasTooFarProgression, 合計BasInterval + 1));
                ++result;
            }
            return result;
        }

        /// <summary>
        /// Basの6度進行の良否チェック。II巻p122
        /// </summary>
        private int CheckBasの6度進行(Progression p) {
            int result = 0;

            int basInterval = p.PartProgressionHigherInterval(Part.Bas);
            if (Math.Abs(basInterval) == 5) {
                if (p.PreC.ChordDegree == p.NowC.ChordDegree) {
                    // 先行和音と後続和音の和音の音度が同じ場合。良いようだ。
                } else if (p.PreC.GetPitch(Part.Bas).Degree == 1) {
                    // 先行和音のBasがI度の場合は無条件にOK
                } else if (p.PreC.TerminationType != TerminationType.None) {
                    // 先行和音が終止の場合も良好。
                } else if (0 < basInterval &&
                    (p.NowC.ChordDegree == CD.V || p.NowC.ChordDegree == CD.V_V) &&
                    (p.NowC.Is("[3転]V9根省") || p.NowC.Is("[3転]V_V9根省"))) {
                    // 後続和音が[3転]V9根省か[3転]V_V9根省の場合。良いようだ。
                    p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RareBas6Progression));
                } else {
                    p.UpdateVerdict(new Verdict(VerdictValue.NotSoGood, VerdictReason.NotSoGoodBas6Progression));
                    ++result;
                }
            }

            if (p.PrepreC != null && p.PrepreC.TerminationType == TerminationType.None) {
                // 先先行和音が終止以外の場合、6度跳躍の後反対方向に進行するのが良い。

                int preBasInterval = p.PrePartProgressionHigherInterval(Part.Bas);
                if (Math.Abs(preBasInterval) == 5) {
                    var preIntervalType = p.PrePrePartProgressionIntervalType(Part.Bas);
                    if ((preBasInterval < 0 && 0 <= basInterval) ||
                        (0 < preBasInterval && basInterval <= 0)) {
                        // OK
                    } else {
                        p.UpdateVerdict(new Verdict(VerdictValue.NotSoGood, VerdictReason.NotSoGoodBas6Progression2));
                        ++result;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Basの5度進行のチェック。II巻p123 55 3) c)
        /// </summary>
        /// <returns>エラーカウント。</returns>
        private int CheckBasの5度以上進行(Progression p) {
            int result = 0;
            if (p.PreC.TerminationType != TerminationType.None) {
                // 先行和音が終止の場合は無条件にOK。
                return result;
            }

            int basInterval = p.PartProgressionHigherInterval(Part.Bas);
            if (basInterval == -4) {
                // Basが5度の下行。
                if (p.NowC.GetPitch(Part.Bas).Inversion == Inversion.第7音) {
                    p.UpdateVerdict(new Verdict(VerdictValue.NotSoGood, VerdictReason.NotSoGoodBas5ProgInv7));
                    ++result;
                }
            } else if (basInterval == 4) {
                // Basが5度の上行。
                if (p.NowC.GetPitch(Part.Bas).Inversion == Inversion.第3音 &&
                    (p.NowC.ChordDegree == CD.V || p.NowC.ChordDegree == CD.V_V)) {
                    p.UpdateVerdict(new Verdict(VerdictValue.NotSoGood, VerdictReason.NotSoGoodBas5ProgLeadingNote));
                    ++result;
                }
            }

            return result;
        }

        /// <returns>1:不良である。 0:不良でない。</returns>
        private int Check配置IV7(Chord c) {
            // IIp67
            if (0 == c.Upper3CountByInversion(Inversion.第5音)) {
                // 上部構成音 a)
                c.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.IV7Upper3A));
                return 1;
            } else {
                // 上部構成音 b)
                c.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.IV7Upper3B));
                return 0;
            }
        }

        /// <returns>1:不良である。 0:不良でない。</returns>
        private int Check配置ドリアのIV(Chord c) {
            // IIp73
            if (c.NumberOfNotes != NumberOfNotes.Seventh) {
                // 7の和音以外は、あまり用いられない
                c.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RareDorian));
                return 0;
            }
            // IIp74
            if (!c.IsStandard) {
                // 標準外的配置は、最適配置ではない。
                c.UpdateVerdict(new Verdict(VerdictValue.Okay, VerdictReason.DorianNotBestPosition));
                return 0;
            }
            switch (c.Inversion) {
            case Inversion.根音:
                if (0 == c.Upper3CountByInversion(Inversion.第5音)) {
                    // 上部構成音 a)
                    if (c.Is("密(7)")) {
                        c.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.DorianBestPosition));
                    } else {
                        c.UpdateVerdict(new Verdict(VerdictValue.Okay, VerdictReason.DorianNotBestPosition));
                    }
                } else {
                    // 上部構成音 b)
                    if (c.Is("開(7)")) {
                        c.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.DorianBestPosition));
                    } else {
                        c.UpdateVerdict(new Verdict(VerdictValue.Okay, VerdictReason.DorianNotBestPosition));
                    }
                }
                break;
            case Inversion.第3音:
            case Inversion.第5音:
                if (c.Is("密(7)") || c.Is("開(根)")) {
                    c.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.DorianBestPosition));
                } else {
                    c.UpdateVerdict(new Verdict(VerdictValue.Okay, VerdictReason.DorianNotBestPosition));
                }
                break;
            case Inversion.第7音:
                if (c.Is("密(3)") || c.Is("開(根)")) {
                    c.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.DorianBestPosition));
                } else {
                    c.UpdateVerdict(new Verdict(VerdictValue.Okay, VerdictReason.DorianNotBestPosition));
                }
                break;
            default:
                System.Diagnostics.Debug.Assert(false);
                break;
            }
            return 0;
        }

        /// <summary>
        /// 根音が下方変位したIIの和音は、ナポリのIIではなく、ナポリの六の和音と呼ばれるらしい!
        /// </summary>
        /// <returns>1:不良である。 0:不良でない。</returns>
        private int Check配置ナポリII(Chord c) {
            if (c.Inversion == Inversion.第3音 &&
                c.NumberOfNotes == NumberOfNotes.Triad &&
                c.Is("Oct(5)")) {
                c.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodOct5Napolitan));
                return 0;
            }

            if (!c.IsStandard ||
                c.Inversion != Inversion.第3音 ||
                c.NumberOfNotes != NumberOfNotes.Triad) {
                c.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RareNapolitan));
                return 0;
            }
            if (c.Is("密(根)")) {
                c.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.BestNapolitan));
                return 0;
            } else {
                c.UpdateVerdict(new Verdict(VerdictValue.Okay, VerdictReason.OkayNapolitan));
                return 0;
            }
        }

        private int Check配置IV付加6(Chord c) {
            c.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.BestAddedSix));
            return 0;
        }

        private int Check配置IV付加46(Chord c) {
            if (c.IsMajor() && !c.Is準固有和音) {
                c.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.UnusedAddedSixFour));
                return 1;
            }
            c.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.BestAddedSixFour));
            return 0;
        }

        /// <summary>
        /// 配置の良否をチェックする。
        /// </summary>
        /// <returns>1:不良である。 0:不良でない。</returns>
        private int Check配置(Chord c) {
            int failCount = 0;

            failCount += Check音域(c);
            failCount += Check不自然に聞こえることがある準固有和音(c);
            //failCount += CheckDoNotUseChord(c);

            if (c.ChordDegree == CD.IV) {
                if (c.AddedTone == AddedToneType.Six) {
                    return Check配置IV付加6(c);
                }
                if (c.AddedTone == AddedToneType.SixFour) {
                    return Check配置IV付加46(c);
                }

                if (c.AlterationType == AlterationType.None &&
                    c.Is("[基]IV7")) {
                    return Check配置IV7(c);
                }
                if (c.AlterationType == AlterationType.Dorian) {
                    return Check配置ドリアのIV(c);
                }
            }
            if (c.ChordDegree == CD.II && c.AlterationType == AlterationType.Naples) {
                return Check配置ナポリII(c);
            }
            return 0;
        }

        private int CheckRuleA3A4(Progression p) {
            int failCount = 0;
            Chord c = p.NowC;
            if (c.NumberOfNotes != NumberOfNotes.Ninth) {
                return failCount;
            }
            if (c.Omission == Omission.None) {
                // 根音が省略されていないV9の和音の場合
                // 根音の9度以上上に第9音がない場合、不良。
                Pitch p9 = c.GetPitchByInversion(Inversion.第9音);
                System.Diagnostics.Debug.Assert(p9.Octave != Pitch.INVALID_PITCH.Octave);
                Pitch p1 = c.GetPitchByInversion(Inversion.根音);
                if (p9.GetIntervalNumberFromC0() - p1.GetIntervalNumberFromC0() < 8) {
                    c.UpdateVerdict(
                        new Verdict(VerdictValue.Wrong,
                            VerdictReason.RuleA3,
                            c.GetPartListByInversion(Inversion.根音)[0],
                            c.GetPartListByInversion(Inversion.第9音)[0]));
                    ++failCount;
                }
            }

            if (c.IsMajor() && !c.Is準固有和音) {
                var v9Part9 = c.GetPartListByInversion(Inversion.第9音);
                var v9Part3 = c.GetPartListByInversion(Inversion.第3音);
                System.Diagnostics.Debug.Assert(v9Part9.Count == 1);
                System.Diagnostics.Debug.Assert(v9Part3.Count == 1);
                if (c.TwoPartIntervalNumber(v9Part3[0], v9Part9[0]) < 6) {
                    if (p.PartProgressionAbsInterval(v9Part9[0]) != 0 &&
                        p.PartProgressionAbsInterval(v9Part3[0]) != 0) {
                        c.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.RuleA4, v9Part9[0], v9Part3[0]));
                        ++failCount;
                    }
                }
            }
            return failCount;
        }

        private int Check不自然に聞こえることがある準固有和音(Chord c) {
            // 別巻課題の実施p87
            if (c.Is準固有和音 &&
                c.ChordDegree == CD.V &&
                c.NumberOfNotes == NumberOfNotes.Ninth &&
                c.SopInversion == Inversion.第9音) {
                c.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoBp87_1));
            }
            if (c.Is準固有和音 &&
                c.ChordDegree == CD.IV &&
                c.SopInversion == Inversion.第3音) {
                c.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoBp87_2));
            }
            return 0;
        }

        private bool Is準固有和音から固有和音に移行できる和音(Chord c) {
            if (c.ChordDegree == CD.I ||
                (c.ChordDegree == CD.V && (c.NumberOfNotes == NumberOfNotes.Triad || c.NumberOfNotes == NumberOfNotes.Seventh))) {
                // OK。Iの和音か、V,V7の和音
                return true;
            }
            return false;
        }

        /// <summary>
        /// 準固有和音→固有和音に連結できる条件。
        /// </summary>
         private int CheckIIp25_13(Progression p) {
            if (!p.PreC.Is準固有和音 || !p.NowC.Is固有和音) {
                return 0;
            }

            if (Is準固有和音から固有和音に移行できる和音(p.NowC)) {
                return 0;
            }

            p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIIp25_13));
            return 1;
        }

         class Aug1PartPair
         {
             public Part prePart;
             public Part nowPart;
             public bool removeFlag;

             public Aug1PartPair() {
                 prePart = Part.Invalid;
                 nowPart = Part.Invalid;
                 removeFlag = true;
             }
             public Aug1PartPair(Part prePart, Part nowPart, bool removeFlag) {
                 this.prePart = prePart;
                 this.nowPart = nowPart;
                 this.removeFlag = removeFlag;
             }
         };

        /// <summary>
        /// 増1度の音程関係のペアを取得。
        /// </summary>
         private List<Aug1PartPair> Get増1度の音程関係のペア(Progression p, bool bOnlyUpper3Part) {
             var aug1Pair = new List<Aug1PartPair>();

             int startPartIdx = (int)Part.Bas;
             if (bOnlyUpper3Part) {
                 startPartIdx = (int)Part.Ten;
             }

             for (int prePart = startPartIdx; prePart <= (int)Part.Sop; ++prePart) {
                 for (int nowPart = startPartIdx; nowPart <= (int)Part.Sop; ++nowPart) {
                     var prePitch = p.PreC.GetPitch((Part)prePart);
                     var nowPitch = p.NowC.GetPitch((Part)nowPart);

                     int semitones = prePitch.AbsNumberOfSemitonesWith(nowPitch);

                     if (((semitones % 12) == 1 || (semitones % 12) == 11) &&
                         prePitch.LetterName.NaturalLetterName() == nowPitch.LetterName.NaturalLetterName()) {
                         aug1Pair.Add(new Aug1PartPair((Part)prePart, (Part)nowPart, false));
                     }
                 }
             }
             return aug1Pair;
         }

        private int CheckAugmentedUnisonProgression(Progression p) {
            // 先行和音、後続和音の構成音に増1度の音程の2音があるか？
            // II巻p22,11
            var aug1Pair = Get増1度の音程関係のペア(p, false);
            if (0 == aug1Pair.Count) {
                return 0;
            }

            foreach (var a in aug1Pair) {
                if (a.prePart == a.nowPart) {
                    Part part = a.prePart;
                    if (p.PartProgressionAbsInterval(part) == 0) {
                        // このPartは増一度関係に連結されている。
                        // このPartがprePartやnowPartに含まれるPairにremoveFlagを立てる。
                        foreach (var b in aug1Pair) {
                            if (b.prePart == part || b.nowPart == part) {
                                b.removeFlag = true;
                            }
                        }
                    }
                }
            }

            aug1Pair.RemoveAll(pair => pair.removeFlag);

            if (aug1Pair.Count == 0) {
                return 0;
            }

            // 先行和音、後続和音の構成音に半音1個の音程の2音があり、
            // その先行和音中の声部が増一度進行していない。
            if (p.NowC.IsDim7()) {
                foreach (var a in aug1Pair) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIIp22_11, a.prePart,a.nowPart));
                }
                return 0;
            }
            if (p.PreC.ChordDegree == CD.V_V &&
                ((p.PreC.Inversion == Inversion.根音 && p.NowC.Is("[3転]V7") ||
                 (p.PreC.Inversion == Inversion.第3音 && p.NowC.Is("[基]V7"))))) {
                foreach (var a in aug1Pair) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIIp35_18, a.prePart, a.nowPart));
                }
                return 0;
            }

            int failCount = 0;
            foreach (var a in aug1Pair) {
                var prePitch = p.PreC.GetPitch(a.prePart);
                var nowPitch = p.NowC.GetPitch(a.nowPart);
                if (nowPitch.Inversion == Inversion.第9音) {
                    // 後続和音の対斜が起きている声部が第9音の場合、許容される。
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIIIp134_53c, a.prePart, a.nowPart));
                } else if (
                    prePitch.Inversion == Inversion.根音 &&
                    p.PreC.ChordDegree == CD.II &&
                    p.PreC.AlterationType == AlterationType.Naples) {
                    // 先行和音の対斜がおきている声部が-IIの和音の根音の場合、許容される。
                    p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoIIp77_30_2, a.prePart, a.nowPart));
                } else {
                    p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIIp22_11, a.prePart, a.nowPart));
                    ++failCount;
                }
            }
            return failCount;
        }

        private int Check終止定型(Progression p) {
            TerminationType tt = p.NowC.TerminationType;
            if (tt == TerminationType.None) {
                return 0;
            }

            if (p.NowC.NumberOfNotes != NumberOfNotes.Triad) {
                p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongTermination));
                return 1;
            }
            if (p.NowC.Inversion != Inversion.根音) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.ExceptionalTermination));
            }
                
            if (p.NowC.GetPitch(Part.Sop).Degree == 1) {
                p.UpdateVerdict(new Verdict(VerdictValue.Best, VerdictReason.BestTermination));
            }
            return 0;
        }

        private int Check音域(Chord c) {
            int failCount = 0;
            Pitch bas = c.GetPitch(Part.Bas);
            Pitch ten = c.GetPitch(Part.Ten);
            Pitch alt = c.GetPitch(Part.Alt);
            Pitch sop = c.GetPitch(Part.Sop);
            if (bas.HigherPitchTo(new Pitch(new LnDegInversion(LN.F), 2)) < 0) {
                c.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RarePitch, Part.Bas));
                ++failCount;
            }
            if (bas.HigherPitchTo(new Pitch(new LnDegInversion(LN.D), 4)) > 0) {
                c.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RarePitch, Part.Bas));
                ++failCount;
            }
            if (alt.HigherPitchTo(new Pitch(new LnDegInversion(LN.G), 3)) < 0) {
                c.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RarePitch, Part.Alt));
                ++failCount;
            }
            if (alt.HigherPitchTo(new Pitch(new LnDegInversion(LN.D), 5)) > 0) {
                c.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RarePitch, Part.Alt));
                ++failCount;
            }
            if (7 < ten.AbsIntervalNumberWith(alt)) {
                c.UpdateVerdict(new Verdict(VerdictValue.NotSoGood, VerdictReason.RareInterval, Part.Alt, Part.Ten));
                ++failCount;
            }
            if (7 < alt.AbsIntervalNumberWith(sop)) {
                c.UpdateVerdict(new Verdict(VerdictValue.NotSoGood, VerdictReason.RareInterval, Part.Sop, Part.Alt));
                ++failCount;
            }
            return failCount;
        }

        private int CheckBasTenInterval(Progression p) {
            if (11 < p.NowC.TwoPartIntervalNumber(Part.Bas, Part.Ten)) {
                p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoBasTenInterval));
            }

            if (p.PreC != null &&
                11 < p.PreC.TwoPartIntervalNumber(Part.Bas, Part.Ten) &&
                11 < p.NowC.TwoPartIntervalNumber(Part.Bas, Part.Ten)) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.WrongBasTenInterval));
                return 1;
            }
            return 0;
        }

        private void CheckStartChord(Chord c) {
            if (c.Is("[基]I")) {
                if (c.IsStandard) {
                    c.UpdateVerdict(new Verdict(VerdictValue.Best, VerdictReason.BestStartChord));
                } else {
                    c.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RareStartChord));
                }
                return;
            }
            // I以外で始めたいので、早くやめたい
            c.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.BestStartChord));
        }

        /// <summary>
        /// カデンツのチェック
        /// @return エラーの時1
        /// </summary>
        private int CheckCadence(Progression p) {
            switch(p.PreC.ChordDegree){
            case CD.I:
                switch (p.NowC.ChordDegree) {
                case CD.I:
                    if ((p.PreC.Inversion == Inversion.根音 && p.NowC.Inversion == Inversion.第3音)||
                        (p.PreC.Inversion == Inversion.第3音 && p.NowC.Inversion == Inversion.根音)) {
                        return 0;
                    }
                    break;
                case CD.II:
                case CD.III:
                case CD.IV:
                case CD.V:
                case CD.VI:
                case CD.V_V:
                    return 0;
                default:
                    break;
                }
                break;
            case CD.II:
            case CD.V_V:
                switch (p.NowC.ChordDegree) {
                case CD.I:
                    if (p.NowC.Inversion == Inversion.第5音) {
                        p.NowC.CadenceType = CadenceType.K2;
                        return 0;
                    }
                    break;
                case CD.V_V:
                case CD.V:
                    p.NowC.CadenceType = CadenceType.K2;
                    if (p.PreC.ChordDegree == CD.V_V && p.NowC.ChordDegree == CD.V_V) {
                        break;
                    }
                    return 0;
                default:
                    break;
                }
                break;
            case CD.III:
                switch (p.NowC.ChordDegree) {
                case CD.VI:
                    p.PreC.UpdateFunction(FunctionType.Dominant);
                    // DT進行
                    if (p.PrepreC != null && p.PrepreC.FunctionType == FunctionType.Tonic) {
                        p.PreC.CadenceType = CadenceType.K1;
                    }
                    return 0;
                case CD.IV:
                case CD.II:
                    // TS進行だが、K2かK3かは不明。
                    return 0;
                default:
                    break;
                }
                break;
            case CD.IV:
                switch (p.NowC.ChordDegree) {
                case CD.I:
                    p.PreC.CadenceType = CadenceType.K3;
                    return 0;
                case CD.II:
                case CD.III:
                    return 0;
                case CD.V_V:
                case CD.V:
                    p.NowC.CadenceType = CadenceType.K2;
                    return 0;
                case CD.VII:
                    p.NowC.CadenceType = CadenceType.K2;
                    return 0;
                default:
                    break;
                }
                break;
            case CD.V:
                switch (p.NowC.ChordDegree) {
                case CD.I:
                case CD.III:
                case CD.VI:
                    if (p.PrepreC != null && p.PrepreC.FunctionType == FunctionType.Tonic) {
                        p.PreC.CadenceType = CadenceType.K1;
                    }
                    return 0;
                default:
                    break;
                }
                break;
            case CD.VI:
                switch (p.NowC.ChordDegree) {
                case CD.I:
                    if (p.NowC.Inversion == Inversion.第5音) {
                        p.NowC.CadenceType = CadenceType.K2;
                        return 0;
                    }
                    if (p.PreC.Is("[基]VI") && p.NowC.Is("[1転]I")) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RareIIIp215_76_6_1));
                        return 0;
                    }
                    break;
                case CD.II:
                case CD.III:
                case CD.IV:
                case CD.V:
                case CD.V_V:
                    return 0;
                default:
                    break;
                }
                break;
            case CD.VII:
                switch (p.NowC.ChordDegree) {
                case CD.III:
                    // DT進行
                    if (p.PrepreC != null && p.PrepreC.FunctionType == FunctionType.Tonic) {
                        p.PreC.CadenceType = CadenceType.K1;
                    }
                    return 0;
                default:
                    break;
                }
                break;
            default:
                break;
            }

            if (p.Is転調進行()) {
                // II巻p101
                switch (p.NowC.FunctionType) {
                case FunctionType.Tonic:
                    break;
                case FunctionType.Dominant:
                    // 転入和音が不完全カデンツ。
                    p.NowC.CadenceType = CadenceType.IncompleteK1;
                    return 0;
                case FunctionType.Subdominant:
                    // 転入和音が不完全カデンツ。
                    p.NowC.CadenceType = CadenceType.IncompleteK2;
                    return 0;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
                switch (p.PreC.FunctionType) {
                case FunctionType.Tonic:
                    break;
                case FunctionType.Dominant:
                case FunctionType.Subdominant:
                    // 離脱和音preCが不完全カデンツ
                    switch (p.PreC.CadenceType) {
                    case CadenceType.K1: p.PreC.CadenceType = CadenceType.IncompleteK1; break;
                    case CadenceType.K2: p.PreC.CadenceType = CadenceType.IncompleteK2; break;
                    case CadenceType.K3: p.PreC.CadenceType = CadenceType.IncompleteK3; break;
                    default:
                        break;
                    }
                    return 0;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
                return 0;
            }

            p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIp38_Cadence));
            return 1;
        }

        /// <summary>
        /// 1個の和音nowCのなかで、n+1度(複音程含む)を形成する声部の組のリストを戻す。
        /// C# 4.0になったらTupleを使用する。
        /// </summary>
        private List<KeyValuePair<Part, Part>> GetNthTuple(Chord nowC, int n) {
            List<KeyValuePair<Part, Part>> result = new List<KeyValuePair<Part, Part>>();

            for (int low=(int)Part.Bas; low <= (int)Part.Alt; ++low) {
                for (int high=low + 1; high <= (int)Part.Sop; ++high) {
                    Part lowPart = (Part)low;
                    Part highPart = (Part)high;
                    Pitch lowPitch = nowC.GetPitch(lowPart);
                    Pitch highPitch = nowC.GetPitch(highPart);
                    int distance = lowPitch.AbsIntervalNumberWith(highPitch);
                    if ((distance % 7) == n) {
                        result.Add(new KeyValuePair<Part, Part>(lowPart, highPart));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 1個の和音nowCのなかで、半音n個(複音程含む)を形成する声部の組のリストを戻す。
        /// C# 4.0になったらTupleを使用する。
        /// </summary>
        private List<KeyValuePair<Part, Part>> GetMatchedNumberOfSemitoneTuple(Chord nowC, int n) {
            List<KeyValuePair<Part, Part>> result = new List<KeyValuePair<Part, Part>>();

            for (int low=(int)Part.Bas; low <= (int)Part.Alt; ++low) {
                for (int high=low + 1; high <= (int)Part.Sop; ++high) {
                    Part lowPart = (Part)low;
                    Part highPart = (Part)high;
                    Pitch lowPitch = nowC.GetPitch(lowPart);
                    Pitch highPitch = nowC.GetPitch(highPart);
                    int distance = lowPitch.AbsNumberOfSemitonesWith(highPitch);
                    if ((distance % 12) == n) {
                        result.Add(new KeyValuePair<Part, Part>(lowPart, highPart));
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 複音程進行
        /// </summary>
        private int CheckOver9Progression(Progression p) {
            return p.AllPartCheckAndIfTrueSetVerdict((part) => 12 < p.PartProgressionAbsPitch(part), new Verdict(VerdictValue.Wrong, VerdictReason.RuleB1_3));
        }

        /// <summary>
        /// 7度進行
        /// @todo 例外の実装 3巻p79、p96
        /// </summary>
        private int Check7Progression(Progression p) {
            // 短7度: 半音が10個
            // 長7度: 半音が11個
            return p.AllPartCheckAndIfTrueSetVerdict((part) => {
                var interval = p.PartProgressionAbsInterval(part);
                return (10 == interval) || (11 == interval);
            }, new Verdict(VerdictValue.Wrong, VerdictReason.RuleB1_1));
        }

        /// <summary>
        /// 増音程進行
        /// @todo 増2度上行に対する除外例の実装 2巻p99
        /// </summary>
        private int CheckAugmentedProgression(Progression p) {
            if (p.PrepreC != null) {
                p.AllPartCheckAndIfTrueSetVerdict(part => {
                    if (IsIIp37Progression(part, p.PrepreC, p.PreC)) {
                        if (p.NowC.GetPitch(part).Degree == 5 &&
                            p.PartProgressionIntervalType(part) == IntervalType.MinorSecond) {
                            // II巻p37 許される増2度上行の後に、許される短2度上行した場合。良好。
                            return false;
                        } else {
                            // II巻p37 許される増2度上行の後に、短2度上行以外の進行をした場合、不良。
                            return true;
                        }
                    }
                    return false;
                }, new Verdict(VerdictValue.Wrong, VerdictReason.WrongIIp37_18));
            }

            return p.AllPartCheckAndIfTrueSetVerdict(part => {
                IntervalType intervalType = p.PartProgressionIntervalType(part);
                switch (intervalType) {
                case IntervalType.AugmentedThird:
                case IntervalType.AugmentedFourth:
                case IntervalType.AugmentedFifth:
                case IntervalType.AugmentedSixth:
                case IntervalType.AugmentedSeventh:
                    return true;
                case IntervalType.AugmentedSecond:
                    if (IsIIp37Progression(part, p.PreC, p.NowC)) {
                        // II巻p37
                        p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIIp37_18, part));
                        return false;
                    }
                    if (p.Is転調進行() && p.PreC.Is内声(part)) {
                        var 導音リスト = p.NowC.Get導音リスト();
                        if (導音リスト.Count == 1 && 導音リスト[0].part == part
                            && intervalType == IntervalType.AugmentedSecond &&
                            p.PartProgressionHigherPitch(part) > 0) {
                            // II巻p99,47 d)
                            p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIIp99_47_d, part));
                            return false;
                        }
                    }
                    return true;
                default:
                    return false;
                }
            }, new Verdict(VerdictValue.Wrong, VerdictReason.RuleB1_2));
        }

        /// <summary>
        /// II巻p37,18 許される内声増2度上行先行連結か？
        /// </summary>
        private bool IsIIp37Progression(Part part, Chord preC, Chord nowC) {
            // 同じことがすぐ下にコピペされているので注意!
            if (preC.GetPitch(part).Degree == 3 &&
                nowC.GetPitch(part).Degree == 4 &&
                nowC.GetPitch(part).HigherPitchTo(preC.GetPitch(part)) == 3 &&
                preC.PartIsMiddleVoice(part) &&
                (preC.IsMinor() ||
                preC.Is準固有和音)&&
                (preC.ChordDegree == CD.I ||
                 preC.ChordDegree == CD.VI) &&
                nowC.ChordDegree == CD.V_V &&
                (nowC.NumberOfNotes == NumberOfNotes.Seventh ||
                 nowC.NumberOfNotes == NumberOfNotes.Ninth)) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// II巻p37,18 長調、内声III→↑IV→V
        /// </summary>
        private int CheckIIp37MajorProgression(Progression p) {
            int failCount = 0;
            if (p.PrepreC == null) {
                return 0;
            }

            p.AllPartCheckAndIfTrueSetVerdict(part => {
                Chord ppC = p.PrepreC;
                Chord preC = p.PreC;
                Chord nowC = p.NowC;
                if (ppC.GetPitch(part).Degree == 3 &&
                    preC.GetPitch(part).Degree == 4 &&
                    preC.GetPitch(part).HigherPitchTo(ppC.GetPitch(part)) > 0 &&
                    ppC.PartIsOuterVoice(part) &&
                    (ppC.IsMajor() && !ppC.Is準固有和音) &&
                    (ppC.ChordDegree == CD.I ||
                     ppC.ChordDegree == CD.VI) &&
                    preC.ChordDegree == CD.V_V &&
                    (preC.NumberOfNotes == NumberOfNotes.Seventh ||
                     preC.NumberOfNotes == NumberOfNotes.Ninth)) {
                    if (nowC.GetPitch(part).HigherPitchTo(preC.GetPitch(part)) > 0 &&
                        nowC.GetPitch(part).GetIntervalType(preC.GetPitch(part)) == IntervalType.MinorSecond) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.IIp37_18_2, part));
                    } else {
                        ++failCount;
                        return true;
                    }
                }
                return false;
            }, new Verdict(VerdictValue.Wrong, VerdictReason.IIp37_18_2));
            return failCount;
        }

        /// <summary>
        /// 並達1度
        /// </summary>
        private int CheckParallelProgression1(Progression p) {
            int failCount = 0;

            List<KeyValuePair<Part, Part>> octavePair = GetNthTuple(p.NowC, 0);
            foreach (KeyValuePair<Part, Part> kvp in octavePair) {
                if (Motion.Parallel != p.TwoPartMotion(kvp.Key, kvp.Value)) {
                    continue;
                }
                if (p.NowC.TwoPartNumberOfSemitones(kvp.Key, kvp.Value) == 0) {
                    if (Part.Bas != kvp.Key ||
                        Part.Ten != kvp.Value) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.RuleC3_3_1, kvp.Key, kvp.Value));
                        ++failCount;
                    } else if (
                        3 != p.PartProgressionHigherInterval(Part.Bas) ||
                        5 != p.PartProgressionHigherPitch(Part.Bas) ||
                        1 != p.PartProgressionHigherInterval(Part.Ten) ||
                        1 != p.PartProgressionHigherPitch(Part.Ten)) {
                        // バスが完全4度上行　半音5個
                        // テノールが短二度上行 半音1個
                        p.UpdateVerdict(
                            new Verdict(VerdictValue.Wrong, VerdictReason.RuleC3_3_2, kvp.Key, kvp.Value));
                        ++failCount;
                    } else {
                        // OK
                    }
                }
            }
            return failCount;
        }

        /// <summary>
        /// 並達8度、並達5度、並達15度、並達12度…
        /// </summary>
        private int CheckParallelProgression85(Progression p) {
            if (Motion.Parallel != p.TwoPartMotion(Part.Bas, Part.Sop)) {
                return 0;
            }

            // BasとSopが平行。

            IntervalType intervalType = p.NowC.TwoPartIntervalType(Part.Bas, Part.Sop);
            if (IntervalType.PerfectUnison == intervalType) {
                // 外声間並達8度 完全8度==半音12個
                // Sopが順次進行している場合はOK
                if (p.PartProgressionAbsInterval(Part.Sop) == 1) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoRuleC3_1_2));
                    return 0;
                }
                if ((p.PreC.ChordDegree == CD.VI && p.PreC.Inversion == Inversion.根音) &&
                    (p.NowC.ChordDegree == CD.II && p.NowC.Inversion == Inversion.第3音 && p.NowC.SopInversion == Inversion.第3音)) {
                    // [基]VI→[1転]II(3)の外声間並達8度
                    p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.InfoRuleC3_1_3));
                    return 0;
                }

                p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.RuleC3_1, Part.Bas, Part.Sop));
                return 1;
            } else if (IntervalType.PerfectFifth == intervalType) {
                // 外声間並達5度 完全5度
                if (p.PartProgressionAbsInterval(Part.Sop) == 1) {
                    // Sopが順次進行している場合はOK
                    p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoRuleC3_2, Part.Bas, Part.Sop));
                    return 0;
                }
                // Sopが跳躍進行している並達5度が発生している。

                if (p.NowC.Is("[2転]V9根省(9)")) {
                    // 先行和音→[2転]V9根省(9)の場合、許される(I巻p115公理C3付則)(I巻p91,57)
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.InfoRuleC3_2_2));
                    return 0;
                }
                if (p.NowC.Is("[2転]V_V9根省(9)")) {
                    // 先行和音→[2転]V_V9根省(9)の場合、許される(IIp37,18 2)
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.InfoRuleC3_2_3));
                    return 0;
                }

                p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.RuleC3_2, Part.Bas, Part.Sop));
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// C1 posの手前にnowCを挿入した場合に、連続一度、連続8度を形成するかどうか。
        /// </summary>
        private int CheckConsecutiveOctaves(Progression p) {
            List<KeyValuePair<Part, Part>> prevOctaveTuple
                = GetMatchedNumberOfSemitoneTuple(p.PreC, 0);
            if (prevOctaveTuple.Count == 0) {
                return 0;
            }

            List<KeyValuePair<Part, Part>> nowOctaveTuple
                = GetMatchedNumberOfSemitoneTuple(p.NowC, 0);
            if (nowOctaveTuple.Count == 0) {
                return 0;
            }

            int failCount = 0;
            foreach (KeyValuePair<Part, Part> prevTuple in prevOctaveTuple) {
                if (nowOctaveTuple.Contains(prevTuple)) {
                    if ((p.PartProgressionAbsPitch(prevTuple.Key) % 12) == 0) {
                        // 8度の同時保留 良好である I巻p53
                        // 1度の同時保留 良好である 別巻p21 課題19-10
                        // 1度→8度でTen保留 良好である I巻p64,37
                        continue;
                    }
                    p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.RuleC1, prevTuple.Key, prevTuple.Value));
                    ++failCount;
                }
            }
            return failCount;
        }

        /// <summary>
        /// 連続5度の許容される条件かどうかの判定。
        /// </summary>
        /// <param name="p">連続5度が生じている、問題の連結</param>
        /// <param name="part0">連続5度を生じている低い方のパート</param>
        /// <param name="part1">連続5度を生じている高い方のパート</param>
        /// <returns>0: 連続5度だが許容される。1以上: 不良である。</returns>
        private int VerdictConsecutiveFifth(Progression p, Part part0, Part part1) {
            {
                // パターン1 総合和声p.114 3 2) (III巻p.136,53 c)(別巻 p.171 注)
                var pitch0 = p.NowC.GetPitch(part0);
                var pitch1 = p.NowC.GetPitch(part1);
                if (pitch0.Inversion == Inversion.第9音 ||
                     pitch1.Inversion == Inversion.第9音) {
                    switch (p.NowC.Omission) {
                    case Omission.None:
                        p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.RuleC2_S1, part0, part1));
                        break;
                    case Omission.First:
                        p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.RuleC2_S2, part0, part1));
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false);
                        break;
                    }
                    return 0;
                }
            }

            {
                // パターン2 I巻p115 C2付則 I巻p36
                Motion motion = p.TwoPartMotion(Part.Bas, Part.Alt);
                if (p.PrepreC != null &&
                    part0 == Part.Bas &&
                    part1 == Part.Alt &&
                    p.PrepreC.ChordDegree == CD.V &&
                    p.PreC.ChordDegree == CD.VI &&
                    p.PreC.PositionOfAChord == PositionOfAChord.Oct &&
                    motion == Motion.Contrary) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.RuleC2_1, part0, part1));
                    return 0;
                }
            }

            p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.RuleC2, part0, part1));
            return 1;
        }

        /// <summary>
        /// C2 posの手前にnowCを挿入した場合に、連続5度を形成するか。
        /// @todo 内部変換に関する許容事項3巻p81の実装
        /// @todo 転位に関する許容事項3巻p135の実装
        /// </summary>
        private int CheckConsecutiveFifths(Progression p) {
            List<KeyValuePair<Part, Part>> prevFifthTuple
                = GetNthTuple(p.PreC, 4);
            if (prevFifthTuple.Count == 0) {
                return 0;
            }
            List<KeyValuePair<Part, Part>> nowFifthTuple
                = GetNthTuple(p.NowC, 4);
            if (nowFifthTuple.Count == 0) {
                return 0;
            }

            int failCount = 0;
            foreach (KeyValuePair<Part, Part> nowTuple in nowFifthTuple) {
                if (prevFifthTuple.Contains(nowTuple) &&
                    IntervalType.PerfectFifth == p.NowC.TwoPartIntervalType(nowTuple.Key, nowTuple.Value)) {
                    if ((p.PartProgressionAbsPitch(nowTuple.Key) % 12) == 0) {
                        // 同時保留の場合はOK。(別巻課題I巻19-8)
                        // 1度→8度でAlt保留 良好である I巻補充課題9-5
                        continue;
                    }
                    // 連続5度を形成。例外的に許容される場合がある。
                    failCount += VerdictConsecutiveFifth(p, nowTuple.Key, nowTuple.Value);
                }
            }
            return failCount;
        }

        /// <summary>
        /// 共通音を保留している場合trueを戻し、
        /// 共通音を保留している声部のリストをsustainPartListに戻す。
        /// I巻p51参照
        /// </summary>
        private bool CheckUpper3CommonSustain(
            Dictionary<LN, int> commonLN,
            Progression p,
            out List<Part> sustainPartList) {
            sustainPartList = new List<Part>();
            
            Dictionary<LN, int> sustainLN = new Dictionary<LN, int>();
            for (int i=(int)Part.Ten; i<= (int)Part.Sop; ++i) {
                Part part = (Part)i;
                Pitch pitchL = p.PreC.GetPitch(part);
                LN ln = pitchL.LetterName.LN;
                if (!commonLN.ContainsKey(ln)) {
                    continue;
                }

                Pitch pitchR = p.NowC.GetPitch(part);
                if (0 != pitchL.AbsNumberOfSemitonesWith(pitchR)) {
                    // lとrの共通音を保留していない。
                    // この時点で直ちに標準連結ではないということは出来ない。
                    continue;
                }

                // 共通音を保留している。
                if (!sustainLN.ContainsKey(ln)) {
                    sustainLN[ln] = 1;
                } else {
                    ++sustainLN[ln];
                }
                sustainPartList.Add(part);
            }

            bool result = true;
#if false
            // sustainLNとcommonLNの中身が同じ場合、可能な限り共通音を保留している。
            foreach (KeyValuePair<LN, int> kvpC in commonLN) {
                if (!sustainLN.ContainsKey(kvpC.Key)) {
                    result = false;
                    break;
                }
                if (sustainLN[kvpC.Key] != commonLN[kvpC.Key]) {
                    result = false;
                    break;
                }
            }
#else
            // 共通音が2個以上ある場合は、1個保留しているのが良い。I巻p51
            if (sustainLN.Count == 1) {
                result = true;
            }
#endif
            return result;
        }

        /// <summary>
        /// 上3声の音名と出現頻度を数える。
        /// </summary>
        Dictionary<LN, int> CountUpper3LNonChord(Chord c) {
            Dictionary<LN, int> result = new Dictionary<LN, int>();
            for (int i=(int)Part.Ten; i <= (int)Part.Sop; ++i) {
                Part part = (Part)i;
                LN ln = c.GetPitch(part).LetterName.LN;
                if (!result.ContainsKey(ln)) {
                    result[ln] = 1;
                } else {
                    ++result[ln];
                }
            }
            return result;
        }

        /// <summary>
        /// 先行和音と後続和音の上3声のなかから共通音の音名と、かぶっている数のリストを戻す。
        /// </summary>
        private Dictionary<LN, int> Get上3声の共通音のペア(Progression p) {
            Dictionary<LN, int> lnCountL = CountUpper3LNonChord(p.PreC);
            Dictionary<LN, int> lnCountR = CountUpper3LNonChord(p.NowC);

            Dictionary<LN, int> result = new Dictionary<LN, int>();
            foreach (KeyValuePair<LN, int> kvpL in lnCountL) {
                if (lnCountR.ContainsKey(kvpL.Key)) {
                    int lCount = kvpL.Value;
                    int rCount = lnCountR[kvpL.Key];

                    result[kvpL.Key] = Math.Min(lCount, rCount);
                }
            }
            return result;
        }

        /// <summary>
        /// 導音の進行
        /// </summary>
        private int CheckLeadingNoteProgression(Progression p) {
            var 導音リスト = p.PreC.Get導音リスト();
            if (導音リスト.Count == 0) {
                return 0;
            }
            System.Diagnostics.Debug.Assert(導音リスト.Count == 1);

            if (p.Is転調進行()) {
                // 転調進行の導音の扱いは転調進行()で判定する。
                return 0;
            }

            Part part = 導音リスト[0].part;
            if (p.PreC.ChordDegree == CD.V &&
                p.NowC.ChordDegree == CD.III) {
                // IIIp212 V→IIIのVの和音の第3音は限定進行しない。
                p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoIIIp212_76_3, part));
                return 0;
            }

            if (0 <= p.PartProgressionHigherPitch(part)) {
                // 保留か上行
                int intervalNumber = p.PartProgressionAbsInterval(part);
                if (intervalNumber <= 1) {
                    // 同度、増一度上行、2度上行の場合OK。
                    return 0;
                }
            } else {
                // 下行
                if (part == Part.Alt && 
                    (p.PreC.Is("V7密(5)") || p.PreC.Is("V密(5)")) &&
                    p.NowC.Is("VI密(3)")) {
                    int intervalNumber = p.PartProgressionHigherInterval(part);
                    if (intervalNumber == -1) {
                        // 1巻p124 D 2度下行OK。
                        p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp124_2D)); 
                        return 0;
                    }
                }
                if (p.PreC.ChordDegree == CD.V_V) {
                    if (p.NowC.ChordDegree == CD.V &&
                        (p.NowC.NumberOfNotes == NumberOfNotes.Seventh ||
                         p.NowC.NumberOfNotes == NumberOfNotes.Ninth)) {
                        int intervalNumber = p.PartProgressionHigherInterval(part);
                        if (intervalNumber == 0) {
                            // II巻p35,18 増一度下行OK。
                            return 0;
                        }
                    }
                }
                if (p.NowC.Is("[3転]VI7") && part == Part.Sop && p.PartProgressionHigherInterval(part) == -1) {
                    // III巻p219,77 3) 2度下行。許される。
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIIIp219_77_3_9));
                    return 0;
                }
            }

            // 不良。
            p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.RuleB2_1, part));
            return 1;
        }

        private int CheckOct6toClose5(Progression p)
        {
            if (!p.PreC.Is("VIOct(3)") || !p.NowC.Is("V密(5)")) {
                return 0;
            }

            // I巻p106
            p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIp106_2, Part.Alt));
            return 1;
        }

        //-----------------------------------------------------------------------------
        // 標準連結の判定

        /// <summary>
        /// [基]V→[基]VI I巻p34
        /// </summary>
        /// <returns>true: 先行和音、後続和音の音度は該当するもの(V→VI)だった。false:音度が違う。</returns>
        private bool 標準連結5to6(Progression p) {
            if (p.PreC.ChordDegree != CD.V ||
                p.NowC.ChordDegree != CD.VI) {
                return false;
            }
            if (!p.PreC.IsStandard) {
                return true;
            }

            Chord preC = p.PreC;

            // V→VIの場合だけ特別 I巻p34
            // バスV→VI
            // VII→I      (順次進行)
            // II→I       (順次進行)
            // 上3声V→III (3音下行)
            List<Part> prev7s = preC.GetPartListByDegree(7);
            List<Part> prev2s = preC.GetPartListByDegree(2);
            List<Part> prev5s = preC.GetPartListByDegree(5);
            if (prev7s.Count != 1 ||
                prev2s.Count != 1 ||
                prev5s.Count != 2) {
                return true;
            }

            if (p.PartProgressionHigherInterval(prev7s[0]) != 1 ||
                p.PartProgressionHigherInterval(prev2s[0]) != -1 ||
                p.PartProgressionHigherInterval(prev5s[1]) != -2) {
                return true;
            }

            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp34_1));
            return true;
        }

        /// <summary>
        /// [基]VI→[基]V I巻p36、p106参照
        /// </summary>
        /// <returns>true: 先行和音、後続和音の音度は該当するもの(V→VI)だった。false:音度が違う。</returns>
        private bool 標準連結5to6to5(Progression p) {
            if (p.PreC.ChordDegree != CD.VI ||
                p.NowC.ChordDegree != CD.V) {
                return false;
            }
            if (!p.NowC.IsStandard) {
                return true;
            }

            // VI→Vの場合 I巻p36、p106参照
            if (p.PrepreC == null) {
                return false;
            }

            if (p.PrepreC.ChordDegree != CD.V) {
                return false;
            }

            List<Part> part3List = p.PreC.GetPartListByInversion(Inversion.第3音);
            if (part3List.Count != 2) {
                // V→VI→Vの進行で、VIに第3音(I)が2個存在しない。
                // 標準連結ではない。
                return true;
            }
            int degree0 = p.NowC.GetPitch(part3List[0]).Degree;
            int degree1 = p.NowC.GetPitch(part3List[1]).Degree;
            if ((degree0 == 2 && degree1 == 7) ||
                (degree0 == 7 && degree1 == 2)) {
                p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp36));
            }
            return true;
        }

        /// <summary>
        /// [基]II→[基]V I巻p34
        /// </summary>
        /// <returns>true: 先行和音、後続和音の音度は該当するもの(II→V)だった。false:音度が違う。</returns>
        private bool 標準連結2to5(Progression p) {
            if (p.PreC.ChordDegree != CD.II) {
                return false;
            }
            if (p.NowC.ChordDegree != CD.V) {
                // [基]IIからは[基]Vにしか行かない。
                return true;
            }
            if (!p.PreC.IsStandard ||
                !p.NowC.IsStandard) {
                return true;
            }

            // II→V I巻p34
            // 配分一致
            bool upper3IIGotoVII = true;
            List<Part> part2List = p.PreC.GetPartListByInversion(Inversion.根音);
            foreach (Part part in part2List) {
                if (part == Part.Bas) {
                    // 行間を読むと、バスのIIはどうでもいいらしい。
                    continue;
                }
                if (p.NowC.GetPitch(part).Degree != 7) {
                    upper3IIGotoVII = false;
                }
            }
            if (upper3IIGotoVII && p.PreC.PositionOfAChord == p.NowC.PositionOfAChord) {
                p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp34_2));
            }

            // I巻p126 2 I
            // 長調では共通音IIを保留することもできる。標準外的連結。
            if (p.PreC.IsMajor() && 2 <= part2List.Count && p.PartProgressionHigherPitch(part2List[1]) == 0) {
                p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp126_2I));
            }

            return true;
        }

        /// <summary>
        /// [基]IV→[基]II I巻p34
        /// </summary>
        /// <returns>true: 先行和音、後続和音の音度は該当するものだった。false:音度が違う。</returns>
        private bool 標準連結4to2(Progression p) {
            if (p.PreC.ChordDegree != CD.IV ||
                p.PreC.SopInversion != Inversion.第3音 ||
                !p.NowC.IsMajor() ||
                p.NowC.ChordDegree != CD.II) {
                return false;
            }

            if (!p.PreC.IsStandard ||
                !p.NowC.IsStandard) {
                return true;
            }

            Chord preC = p.PreC;

            // 長調で、第3音高位のIV→II
            // 上3声全てが上行(保留不可)する
            // I→IV  (4度上行)
            // VI→II (4度上行)
            // IV→VI (3度上行)
            List<Part> prev1s = preC.GetPartListByDegree(1);
            List<Part> prev6s = preC.GetPartListByDegree(6);
            List<Part> prev4s = preC.GetPartListByDegree(4);
            if (prev1s.Count != 1 ||
                prev6s.Count != 1 ||
                prev4s.Count != 2) {
                return true;
            }

            if (p.PartProgressionHigherInterval(prev1s[0]) != 3 ||
                p.PartProgressionHigherInterval(prev6s[0]) != 3 ||
                p.PartProgressionHigherInterval(prev4s[1]) != 2) {
                return true;
            }
            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp34_3));
            return true;
        }

        /// <summary>
        /// 配分移行が可能な[基]VIか。I巻p35
        /// </summary>
        /// <returns>true: 配分移行が可能。</returns>
        private bool IsPositionOfAChordTransferable6(Chord c) {
            if (c.ChordDegree != CD.VI ||
                c.Inversion != Inversion.根音 ||
                c.NumberOfNotes != NumberOfNotes.Triad) {
                return false;
            }

            List<Part> deg1Parts = c.GetPartListByDegree(1);
            List<Part> deg3Parts = c.GetPartListByDegree(3);
            if (deg1Parts.Count != 2 ||
                deg3Parts.Count != 1) {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 多く用いられる1転II配置か。I巻p54
        /// </summary>
        /// <returns>true: 多く用いられる1転II配置</returns>
        private bool IsStandardFirstInv2Chord(Chord c) {
            if (!c.Is("[1転]II")) {
                return false;
            }

            if (0 == c.Upper3CountByInversion(Inversion.第3音)) {
                // 第3音を含まないOct(根)もOK
                // I巻p54,31注左から2番目、別巻課題I巻28-6
                return c.Is("Oct(根)");
            }

            // 第3音を含む密(根) I巻p54
            // 第3音を含む密(3)  I巻p54
            // 第3音を含む開(根) I巻p54
            // 第3音を含む開(3)  別巻p55補充課題I巻2-5
            // 第3音を含む開(5)  別巻p20課題I巻19-10、9番目の和音
            return c.Is("密(根)") || c.Is("密(3)") || c.PositionOfAChord == PositionOfAChord.開;
        }

        /// <summary>
        /// 標準連結になりうる配分連結が行われているかどうか。
        /// 基本的には標準配置の和音同士の配分一致が標準連結になりうる。
        /// しかしいくつかの例外あり。
        /// </summary>
        /// <returns>true: 標準連結になりうる配分連結。</returns>
        private bool IsStandardPointOfAChordProgression(Progression p) {
            Chord preC = p.PreC;
            Chord nowC = p.NowC;

            if (preC.IsOK() &&
                nowC.IsStandard &&
                preC.PositionOfAChord == nowC.PositionOfAChord) {
                return true;
            }

            if (p.Is転調進行()) {
                return true;
            }

            // 先行和音に起因する例外的標準連結
            switch (preC.NumberOfNotes) {
            case NumberOfNotes.Triad:
                switch (preC.Inversion) {
                case Inversion.根音: // [基]
                    if (preC.ChordDegree == CD.VI) {
                        // I巻p35参照。
                        // 先行和音がVIの上3声第3音重複、上3声第5音1個の場合。
                        // 後続和音は標準配置の密か標準配置の開になる。
                        // 配分一致しても配分移行しても配分転換してもよい。
                        // 別巻課題I巻28-10 VIOct根省(3)→標[1転]IV開の例あり
                        if (IsPositionOfAChordTransferable6(preC) &&
                            nowC.IsStandard &&
                            (nowC.PositionOfAChord == PositionOfAChord.密 ||
                             nowC.PositionOfAChord == PositionOfAChord.開)) {
                            return true;
                        }
                        if (nowC.IsStandard &&
                            preC.PositionOfAChord == nowC.PositionOfAChord) {
                            // 先行和音VIが標準外配置であっても
                            // 配分一致しており、後続和音が標準配置ならばOK。
                            return true;
                        }
                    }
                    if (p.PrepreC != null && p.PrepreC.Is("[基]V7") && p.PrepreC.Upper3CountByInversion(Inversion.第5音) == 1 &&
                        p.PreC.Is("[基]I5省")) {
                        // 先行和音が第5音を含む[基]V7(上部構成音b)の場合、
                        // 後続和音Iが第5音を省いたIになる。(I巻p75)
                        // 別巻課題I巻28-10
                        return true;
                    }
                    if (preC.PositionOfAChord == PositionOfAChord.Oct && p.PositionOfAChordIsTheSameOrTransfered()) {
                        // 先行和音が非標準のOct和音の場合次は密か開になってもよい。
                        return true;
                    }
                    break;
                case Inversion.第3音: // [1転]
                    switch (preC.ChordDegree) {
                    case CD.I:
                    case CD.IV:
                    case CD.V:
                    case CD.III: //< IIIp213
                    case CD.VII: //< IIIp214
                    case CD.VI:  //< IIIp24
                        // I巻p49参照。
                        if (preC.PositionOfAChord == PositionOfAChord.Oct) {
                            if (nowC.PositionOfAChord == PositionOfAChord.密 ||
                                nowC.PositionOfAChord == PositionOfAChord.開) {
                                // [1転]□Oct→[基または1転または2転]□密
                                // [1転]□Oct→[基または1転または2転]□開
                                return true;
                            }
                        } else {
                            if (nowC.Inversion == Inversion.第3音 &&
                                nowC.PositionOfAChord == PositionOfAChord.Oct) {
                                // [1転]□密→[1転]□Oct
                                // [1転]□開→[1転]□Oct
                                return true;
                            }
                        }
                        break;
                    case CD.II:
                        // I巻p54参照。
                        if (IsStandardFirstInv2Chord(preC) &&
                            nowC.ChordDegree == CD.V &&
                            nowC.Inversion == Inversion.根音) {
                            // 先行和音がよく用いられる[1転]II和音で、
                            // 後続和音が[基]Vの場合、配分一致していればOK。
                            return p.PositionOfAChordIsTheSameOrTransfered();
                        }
                        if (preC.PositionOfAChord == PositionOfAChord.Oct &&
                            nowC.ChordDegree == CD.V) {
                            if (nowC.PositionOfAChord == PositionOfAChord.密 ||
                                nowC.PositionOfAChord == PositionOfAChord.開) {
                                // 先行和音が[1転]IIOctで、後続和音が[基]Vの場合密か開
                                // 別巻課題I巻28-10 先行和音が[1転]IIOctで、後続和音が[3転]Vの場合もあった
                                return true;
                            }
                        }
                        if (nowC.ChordDegree == CD.V &&
                            nowC.Inversion == Inversion.第3音) {
                            // 先行和音が[1転]II(密、開、Oct)で、後続和音が[1転]Vならばなんでも可。
                            return true;
                        }
                        break;
                    default:
                        break;
                    }
                    break;
                case Inversion.第5音: // [2転]
                    // [2転]
                    // 特殊な連結はない。
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
                break;
            case NumberOfNotes.Seventh:
                if (p.ChordsAre("[2転]V7", "[1転]I")) {
                    if (p.NowC.Upper3CountByInversion(Inversion.根音) == 1 &&
                        p.NowC.Upper3CountByInversion(Inversion.第3音) == 1 &&
                        p.NowC.Upper3CountByInversion(Inversion.第5音) == 1) {
                        // I巻p73 上3声に第3音がふくまれていてもよい。
                        p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.RuleIp73_43));
                        return true;
                    } else {
                        // I巻p124 配分移行OK。
                        if (p.PositionOfAChordIsTheSameOrTransfered()) {
                            return true;
                        }
                    }
                } else if (p.PreC.Is("[基]V7") && p.PreC.Upper3CountByInversion(Inversion.第5音)==1 &&
                    p.NowC.Is("[基]I5省")) {
                    // 先行和音が第5音を含む[基]V7(上部構成音b)の場合、
                    // 後続和音Iが第5音を省いたIになる。(I巻p75)
                    return true;
                } else if (p.NowC.Is("[基]IOct根省")) {
                    // 別巻課題I巻28-7
                    return true;
                } else if (p.PreC.Is("[1転]V7根省") || p.PreC.Is("[3転]V7根省")) {
                    // I巻p126 3 配分移行OK。
                    // I巻p126 3 配分転換OK。
                    return true;
                }
                break;
            case NumberOfNotes.Ninth:
                // I巻p90 標準外の[1転]Iが許される。
                // I巻p128 配分移行OK。
                // II巻p57 配分転換OK。
                return true;
            default:
                System.Diagnostics.Debug.Assert(false);
                break;
            }

            // 後続和音に起因する例外的標準連結
            switch (nowC.NumberOfNotes) {
            case NumberOfNotes.Triad:
                switch (nowC.Inversion) {
                case Inversion.根音: // [基]
                    if (nowC.ChordDegree == CD.VI) {
                        // I巻p35参照。
                        if (IsPositionOfAChordTransferable6(nowC)) {
                            if (preC.IsStandard &&
                                preC.Inversion == Inversion.根音 &&
                                (preC.PositionOfAChord == PositionOfAChord.密 ||
                                 preC.PositionOfAChord == PositionOfAChord.開)) {
                                // 密または開→上3声第3音重複、上3声第5音1個のVIOctへの配分移行が可能。
                                return true;
                            }
                        }
                    }
                    break;
                case Inversion.第3音: // [1転]
                    switch (nowC.ChordDegree) {
                    case CD.I:
                    case CD.IV:
                    case CD.V:
                        // I巻p49参照。
                        if (nowC.PositionOfAChord == PositionOfAChord.Oct &&
                            nowC.Upper3CountByInversion(Inversion.第3音) == 0 &&
                            nowC.Upper3NumOfInversionVariation() == 2) {
                            if ((preC.Inversion == Inversion.根音 ||
                                 preC.Inversion == Inversion.第5音 ||
                                 preC.Inversion == Inversion.第7音) &&
                                (preC.PositionOfAChord == PositionOfAChord.密 ||
                                 preC.PositionOfAChord == PositionOfAChord.開)) {
                                // [基]□密→[1転]□Oct
                                // [基]□開→[1転]□Oct
                                // [2転]□密→[1転]□Oct
                                // [2転]□開→[1転]□Oct
                                // [3転]□密→[1転]□Oct p72
                                // [3転]□開→[1転]□Oct
                                return true;
                            }
                        }
                        break;
                    case CD.II:
                        // I巻p54参照。
                        if (IsStandardFirstInv2Chord(nowC)) {
                            // 後続和音が最適配置の[1転]IIの場合、先行和音は何でも可。
                            return true;
                        }
                        break;
                    default:
                        break;
                    }
                    break;
                case Inversion.第5音: // [2転]
                    if (nowC.Is("V7根省")) {
                        // 開⇔密でなければよい
                        return !(preC.PositionOfAChord == PositionOfAChord.密 && nowC.PositionOfAChord == PositionOfAChord.開) &&
                               !(preC.PositionOfAChord == PositionOfAChord.開 && nowC.PositionOfAChord == PositionOfAChord.密);
                    }
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
                break;
            case NumberOfNotes.Seventh:
                if (nowC.Is("V7根省")) {
                    // 開⇔密でなければよい
                    return !(preC.PositionOfAChord == PositionOfAChord.密 && nowC.PositionOfAChord == PositionOfAChord.開) &&
                           !(preC.PositionOfAChord == PositionOfAChord.開 && nowC.PositionOfAChord == PositionOfAChord.密);
                }
                break;
            case NumberOfNotes.Ninth:
                /* 配分転換も良好 I巻p91、[3転]V9についてはI巻p128に例あり。 */
                return true;
            default:
                System.Diagnostics.Debug.Assert(false);
                break;
            }

            return false;
        }

        enum 標準連結Rule
        {
            BasSopContrary,     //< BasとSopが反行
            FirstInvProgression, //< [基]⇔[1転]、[1転]⇔[1転]の場合、BasとSopが反行しなくても可。
        }

        enum 標準連結Type
        {
            Not標準連結,
            Upper3CommonNotExist,
            Upper3CommonExists,
            PrevChordIsClosed5SopranoNonInverted,
        }

        /// <summary>
        /// 一般的な標準連結かどうか。
        /// 配分一致。([1転]IIが含まれている場合配分移行も可。)
        /// 共通音がない場合はソプラノがバスに反行。
        /// 共通音がある場合は共通音を保留。
        /// 特定の共通音が2つある場合一方だけを保留する。
        /// </summary>
        /// <returns>true: 一般的な標準連結</returns>
        private bool Is標準連結(Progression p, 標準連結Rule rule, out 標準連結Type type) {
            if (!IsStandardPointOfAChordProgression(p)) {
                type = 標準連結Type.Not標準連結;
                return false;
            }

            Dictionary<LN, int> commonLN = Get上3声の共通音のペア(p);
            if (0 == commonLN.Count) {
                // 共通音がない場合
                // ソプラノがバスに反行 (I巻p32)
                // 配分一致
                // VI(上3声第3音2個、上3声第5音1個) → □(密)または
                // VI(上3声第3音2個、上3声第5音1個) → □(開)の場合も許容

                switch (rule) {
                case 標準連結Rule.BasSopContrary:
                    if (Motion.Contrary == p.TwoPartMotion(Part.Bas, Part.Sop)) {
                        type = 標準連結Type.Upper3CommonNotExist;
                    } else {
                        if (p.PreC.NumberOfNotes == NumberOfNotes.Seventh ||
                            p.NowC.NumberOfNotes == NumberOfNotes.Seventh) {
                            // IIIp218 77 3) 1 例外的 どちらか一方がI7 III7 VI7 VII7なら許容
                            type = 標準連結Type.Upper3CommonNotExist;
                            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIIp218_77_3_4));
                            return true;
                        }

                        type = 標準連結Type.Not標準連結;
                    }
                    return true;
                case 標準連結Rule.FirstInvProgression:
                    type = 標準連結Type.Upper3CommonNotExist;
                    return true;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
            } else {
                // 共通音がある場合
                // 共通音をすべて保留 I巻p30
                // 配分一致
                // VI(上3声第3音2個、上3声第5音1個) → □(密)または
                // VI(上3声第3音2個、上3声第5音1個) → □(開)の場合も許容
                List<Part> sustainPartList;
                if (CheckUpper3CommonSustain(commonLN, p, out sustainPartList)) {
                    type = 標準連結Type.Upper3CommonExists;
                    return true;
                }

                if (p.PreC.Is("V密(根)") || p.PreC.Is("[3転]V7密(根)")) {
                    // 先行和音が[□]V密(根)の和音または[3転]V7密(根)の和音の場合、共通音があっても保留しなくてよい。
                    // I巻p109 4 (I巻p51)
                    type = 標準連結Type.PrevChordIsClosed5SopranoNonInverted;
                    return true;
                }

            }
            type = 標準連結Type.Not標準連結;
            return false;
        }

        /// <summary>
        /// (1巻p109-6) [1転]II密(根)への連結の制約
        /// </summary>
        /// <returns>true: [1転]II密(根)への連結だった。</returns>
        private bool Progressionto1転2CloseSopNonInv(Progression p) {
            Chord preC = p.PreC;
            Chord nowC = p.NowC;

            if (nowC.Is("[1転]II密(根)")) {
                if (0 == nowC.Upper3CountByInversion(Inversion.第3音)) {
                    // 第3音が含まれている必要がある
                    return true;
                }

                Pitch prevSop = preC.GetPitch(Part.Sop);
                Pitch nowSop  = nowC.GetPitch(Part.Sop);
                int numOfSemitonesSop = p.PartProgressionHigherPitch(Part.Sop);
                int absIntervalNumber = p.PartProgressionAbsInterval(Part.Sop);

                if (absIntervalNumber <= 1 ||
                    (0 < numOfSemitonesSop && prevSop.Degree == 6) ||
                    (numOfSemitonesSop < 0 && prevSop.Degree == 4)) {
                    // ソプラノが保留、順次進行、または4度上行か3度下行の場合は最適配置。
                    // それ以外は『避けよ』
                    // 最適配置にするために配分移行や配分転換してもよい。
                    // I巻p109-6
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.BestProgressionIp54));
                    return true;
                }
                p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIp109_6));
                return true;
            }
            return false;
        }

        /// <summary>
        /// [基]□→[1転]IIの場合 I巻p54、I巻p109
        /// </summary>
        /// <returns>true: 先行和音、後続和音の音度は該当するもの([基]□→[1転]II)だった。false:音度が違う。</returns>
        private bool 標準連結基Xto1転2(Progression p) {
            Chord nowC = p.NowC;

            if (nowC.ChordDegree != CD.II) {
                return false;
            }

            if (Progressionto1転2CloseSopNonInv(p)) {
                // 第3音を含む[1転]II密(根)への最適連結。
                return true;
            }

            if (IsStandardPointOfAChordProgression(p)) {
                if (0 != nowC.Upper3CountByInversion(Inversion.第3音)) {
                    if ((nowC.Is("密(3)") || nowC.Is("開(根)"))) {
                        // 後続和音が第3音が含まれている[1転]II密(3)または[1転]II開(根)
                        // 上行、下行などはどうでもよい。(I巻p55)
                        nowC.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp54_2));
                    } else {
                        nowC.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.StandardIp54_2));
                    }
                } else {
                    if (0 != nowC.Upper3CountByInversion(Inversion.第5音) &&
                        nowC.Is("Oct(根)")) {
                        // 先行和音が標準[基]Xで
                        // 後続和音が上3声に第3音が含まれておらず第5音を含む[1転]IIOct(根)
                        // よくわからないがとにかくOKとする。(I巻p54)
                        nowC.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIp54_2_2));
                    } else {
                        // I巻p54 とにかく可能。
                        nowC.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.StandardIp54_2));
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// [基]□→[1転]I,IV,Vの場合 I巻p50
        /// </summary>
        /// <returns>true: 先行和音、後続和音の音度は該当するもの([基]□→[1転]I,IV,V)だった。false:音度が違う。</returns>
        private bool 標準連結基Xto1転145(Progression p) {
            if (!IsStandardPointOfAChordProgression(p)) {
                // 標準連結になりうる配分連結が行われている必要がある。
                return true;
            }
            if (0 < p.NowC.Upper3CountByInversion(Inversion.第3音)) {
                // [一転]I、[一転]IV、[一転]V和音は第3音省略形がよい。
                return true;
            }

            標準連結Type spt;
            if (Is標準連結(p, 標準連結Rule.FirstInvProgression, out spt)) {
                switch (spt) {
                case 標準連結Type.Upper3CommonExists:
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp51));
                    break;
                case 標準連結Type.Upper3CommonNotExist:
                    // 本当は、先行和音との連結で上3声がそれぞれもっとも近くなる標準配置だけが標準連結。
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp52));
                    break;
                case 標準連結Type.PrevChordIsClosed5SopranoNonInverted:
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp109_4));
                    break;
                case 標準連結Type.Not標準連結:
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
            }
            return true;
        }

        /// <summary>
        /// [1転]→[基]I,IV,V I巻p51
        /// [1転]III→[基]VI III巻p213,76 3)
        /// [1転]VII→[基]III III巻p214,76 4)
        /// [1転]VI→[基]II III巻p214,76 5)
        /// </summary>
        /// <returns>true: 先行和音、後続和音の音度は該当するもの([1転]→[基]I,IV,V)だった。false:音度が違う。</returns>
        private bool 標準連結1転to基(Progression p) {
            if (!IsStandardPointOfAChordProgression(p)) {
                // 標準連結になりうる配分連結が行われている必要がある。
                return true;
            }

            標準連結Type spt;
            if (Is標準連結(p, 標準連結Rule.FirstInvProgression, out spt)) {
                switch (spt) {
                case 標準連結Type.Upper3CommonExists:
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp51));
                    break;
                case 標準連結Type.Upper3CommonNotExist:
                    // 本当は、先行和音との連結で上3声がそれぞれもっとも近くなる標準配置だけが標準連結。
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp52));
                    break;
                case 標準連結Type.PrevChordIsClosed5SopranoNonInverted:
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp109_4));
                    break;
                case 標準連結Type.Not標準連結:
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
            }
            return true;
        }

        /// <summary>
        /// [1転]II→[基] p54
        /// </summary>
        /// <returns>true: 先行和音、後続和音の音度は該当するもの([1転]II→[基])だった。false:音度が違う。</returns>
        private bool 標準連結1転2to基(Progression p) {
            if (p.PreC.ChordDegree != CD.II ||
                p.PreC.Inversion != Inversion.第3音 ||
                p.NowC.Inversion != Inversion.根音) {
                return false;
            }
            
            if (!IsStandardPointOfAChordProgression(p)) {
                // 標準連結になりうる配分連結が行われている必要がある。
                return true;
            }

            // 上3声が全て下行(保留不可)すれば最適
            if (p.Upper3CheckAndIfAllTrueSetVerdictOnce(
                (part) => p.PartProgressionHigherInterval(part) < 0,
                new Verdict(VerdictValue.Good, VerdictReason.StandardIp54_31))) {
                return true;
            }

            // 標準連結→“可能”
            標準連結Type spt;
            if (Is標準連結(p, 標準連結Rule.FirstInvProgression, out spt)) {
                switch (spt) {
                case 標準連結Type.Upper3CommonExists:
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp54_31));
                    break;
                default:
                    break;
                }
            }

            return true;
        }

        /// <summary>
        /// [1転]II→[1転]□ p54
        /// </summary>
        /// <returns>true: 先行和音、後続和音の音度は該当するもの([1転]II→[1転]□)だった。false:音度が違う。</returns>
        private bool 標準連結1転2to1転(Progression p) {
            if (p.PreC.ChordDegree != CD.II) {
                return false;
            }
            if (p.NowC.ChordDegree != CD.V) {
                // 後続和音はVである必要がある。I巻p39
                return true;
            }

            標準連結Type spt;
            if (Is標準連結(p, 標準連結Rule.FirstInvProgression, out spt)) {
                switch (spt) {
                case 標準連結Type.Upper3CommonExists:
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp54_1_1));
                    p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RareIp54));
                    break;
                case 標準連結Type.Upper3CommonNotExist:
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp54_1_2));
                    p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RareIp54));
                    break;
                case 標準連結Type.Not標準連結:
                    break;
                default:
                    // 先行和音がVというのはこの関数の流れ上ありえない
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
            }
            return true;
        }

        /// <summary>
        /// [1転]□→[1転]□ I巻p53
        /// </summary>
        /// <returns>true: 先行和音、後続和音の音度は該当するもの([1転]□→[1転]□)だった。false:音度が違う。</returns>
        private bool 標準連結1転145to1転(Progression p) {
            if (Progressionto1転2CloseSopNonInv(p)) {
                // 後続和音が[1転]II密(根)の場合。I巻p106-2
                return true;
            }

            標準連結Type spt;
            if (Is標準連結(p, 標準連結Rule.FirstInvProgression, out spt)) {
                switch (spt) {
                case 標準連結Type.Upper3CommonExists:
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp53_1));
                    break;
                case 標準連結Type.Upper3CommonNotExist:
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp53_2));
                    break;
                case 標準連結Type.PrevChordIsClosed5SopranoNonInverted:
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp109_4));
                    break;
                case 標準連結Type.Not標準連結:
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
            }
            return true;
        }

        /// <summary>
        /// [基]I→[2転]V I巻p61
        /// </summary>
        /// <returns>true: 先行和音、後続和音の音度は該当するもの([基]I→[2転]V)だった。false:音度が違う。</returns>
        private bool 標準連結基1to2転5(Progression p) {
            if (p.PreC.ChordDegree != CD.I ||
                p.NowC.ChordDegree != CD.V) {
                return false;
            }

            標準連結Type spt;
            if (Is標準連結(p, 標準連結Rule.BasSopContrary, out spt)) {
                switch (spt) {
                case 標準連結Type.Upper3CommonExists: {
                        if (p.PartProgressionHigherInterval(Part.Bas) == 1) {
                            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp61_a1));
                            return true;
                        }
                    }
                    break;
                default:
                    break;
                }
            }

            p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIp61_a1));
            return true;
        }

        /// <summary>
        /// [2転]V→[1転]I I巻p61
        /// </summary>
        /// <returns>true: 先行和音、後続和音の音度は該当するもの([2転]V→[1転]I)だった。false:音度が違う。</returns>
        private bool 標準連結2転5to1転1(Progression p) {
            if (p.PreC.ChordDegree != CD.V ||
                p.NowC.ChordDegree != CD.I) {
                return false;
            }

            標準連結Type spt;
            if (Is標準連結(p, 標準連結Rule.BasSopContrary, out spt)) {
                switch (spt) {
                case 標準連結Type.Upper3CommonExists: {
                        if (p.PartProgressionHigherInterval(Part.Bas) == 1) {
                            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp61_a1_2));
                            return true;
                        }
                    }
                    break;
                default:
                    break;
                }
            }

            p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIp61_a1_2));
            return true;
        }

        /// <summary>
        /// [1転]I→[2転]V I巻p61
        /// </summary>
        /// <returns>true: 先行和音、後続和音の音度は該当するもの([1転]I→[2転]V)だった。false:音度が違う。</returns>
        private bool 標準連結1転1to2転5(Progression p) {
            if (p.PreC.ChordDegree != CD.I ||
                p.NowC.ChordDegree != CD.V) {
                return false;
            }

            標準連結Type spt;
            if (Is標準連結(p, 標準連結Rule.BasSopContrary, out spt)) {
                switch (spt) {
                case 標準連結Type.Upper3CommonExists: {
                        if (p.PartProgressionHigherInterval(Part.Bas) == -1) {
                            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp61_a2));
                            return true;
                        }
                    }
                    break;
                default:
                    break;
                }
            }

            p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIp61_a2));
            return true;
        }
        
        /// <summary>
        /// [1転]II→[2転]I I巻p64の37、I巻p110の8
        /// </summary>
        /// <returns>true: 先行和音、後続和音の音度は該当するもの([1転]II→[2転]I)だった。false:音度が違う。</returns>
        private bool 標準連結1転2to2転1(Progression p) {
            if (p.PreC.ChordDegree != CD.II ||
                p.NowC.ChordDegree != CD.I) {
                return false;
            }

            if (p.PreC.Is("密(根)")) {
                if (p.Upper3CheckAndIfAllTrueSetVerdictOnce(
                    (part) => p.PartProgressionHigherInterval(part) == -1,
                    new Verdict(VerdictValue.Good, VerdictReason.BestIp64_37))) {
                    p.NowC.UpdateFunction(FunctionType.Dominant);
                    return true;
                }
            }

            if (p.PreC.Is("開(根)") || p.PreC.Is("密(3)")) {
                if (p.Upper3CheckAndIfAllTrueSetVerdictOnce(
                    (part) => p.PartProgressionHigherInterval(part) == -1,
                    new Verdict(VerdictValue.Okay, VerdictReason.StandardIp64_37))) {
                    p.NowC.UpdateFunction(FunctionType.Dominant);
                    return true;
                }
            }

            if (p.PreC.Is("Oct(根)")) {
                if (p.Upper3CheckAndIfAllTrueSetVerdictOnce(
                    (part) => {
                        if (Part.Ten == part) {
                            return p.PartProgressionHigherInterval(part) == 1;
                        }
                        return p.PartProgressionHigherInterval(part) == -1;
                    },
                    new Verdict(VerdictValue.Okay, VerdictReason.StandardIp64_37))) {
                    p.NowC.UpdateFunction(FunctionType.Dominant);
                    return true;
                }
            }

            p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIp64_37)); 
            return true;
        }

        /// <summary>
        /// [基]II→[2転]I I巻p64の37、I巻p110の8
        /// </summary>
        /// <returns>true: 先行和音、後続和音の音度は該当するもの([根]II→[2転]I)だった。false:音度が違う。</returns>
        private bool 標準連結基2to2転1(Progression p) {
            if (p.PreC.ChordDegree != CD.II ||
                p.NowC.ChordDegree != CD.I) {
                return false;
            }

            if (p.PreC.Is("密(根)")) {
                if (p.Upper3CheckAndIfAllTrueSetVerdictOnce(
                    (part) => p.PartProgressionHigherInterval(part) == -1,
                    new Verdict(VerdictValue.Good, VerdictReason.BestIp64_37))) {
                    p.NowC.UpdateFunction(FunctionType.Dominant);
                    return true;
                }
            }

            if (p.PreC.Is("開(根)") || p.PreC.Is("密(3)")) {
                if (p.Upper3CheckAndIfAllTrueSetVerdictOnce(
                    (part) => p.PartProgressionHigherInterval(part) == -1,
                    new Verdict(VerdictValue.Okay, VerdictReason.StandardIp64_37))) {
                    p.NowC.UpdateFunction(FunctionType.Dominant);
                    return true;
                }
            }

            p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIp64_37));
            return true;
        }

        /// <summary>
        /// [基]VI→[2転]I I巻p125 2G
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool 標準連結基6to2転1(Progression p) {
            if (p.PreC.ChordDegree != CD.VI ||
                p.NowC.ChordDegree != CD.I) {
                return false;
            }

            p.NowC.UpdateFunction(FunctionType.Dominant);

            if (p.PreC.IsStandard) {
                if (p.Upper3CheckAndIfAllTrueSetVerdictOnce(part => {
                    switch (p.PreC.GetPitch(part).Inversion) {
                    case Inversion.根音: return p.PartProgressionHigherInterval(part) == 2;
                    case Inversion.第3音: return p.PartProgressionHigherInterval(part) == 2;
                    case Inversion.第5音: return p.PartProgressionHigherInterval(part) == 2;
                    default: System.Diagnostics.Debug.Assert(false); return false;
                    }
                }, new Verdict(VerdictValue.Good, VerdictReason.StandardIp125_2G))) {
                }
                return true;
            }

            // 標準外配置のVI 第3音が複数個ある。第3音を1個以上保留する
            int sustainCount = 0;
            if (p.Upper3CheckAndAccumulate(part => {
                switch (p.PreC.GetPitch(part).Inversion) {
                case Inversion.根音: return false;
                case Inversion.第3音: if (p.PartProgressionHigherPitch(part) == 0) { ++sustainCount; } return true;
                case Inversion.第5音: return true;
                default: System.Diagnostics.Debug.Assert(false); return false;
                }}).Count == 3) {
                if (1 <= sustainCount) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp125_2G));
                }
            }

            return true;
        }

        /// <summary>
        /// [2転]V→[基]I I巻p61
        /// </summary>
        /// <returns>true: 先行和音、後続和音の音度は該当するもの([2転]V→[基]I)だった。false:音度が違う。</returns>
        private bool 標準連結2転5to基1(Progression p) {
            if (p.PreC.ChordDegree != CD.V ||
                p.NowC.ChordDegree != CD.I) {
                return false;
            }

            標準連結Type spt;
            if (Is標準連結(p, 標準連結Rule.BasSopContrary, out spt)) {
                switch (spt) {
                case 標準連結Type.Upper3CommonExists:
                    if (p.PartProgressionHigherInterval(Part.Bas) == -1) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp61_a2_2));
                        return true;
                    }
                    break;
                default:
                    break;
                }
            }

            p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIp61_a2_2));
            return true;
        }

        /// <summary>
        /// [基]I→[2転]IV I巻p61
        /// </summary>
        /// <returns>true: 先行和音、後続和音の音度は該当するもの([基]I→[2転]IV)だった。false:音度が違う。</returns>
        private bool 標準連結基1to2転4(Progression p) {
            if (p.PreC.ChordDegree != CD.I ||
                p.NowC.ChordDegree != CD.IV) {
                return false;
            }

            標準連結Type spt;
            if (Is標準連結(p, 標準連結Rule.BasSopContrary, out spt)) {
                switch (spt) {
                case 標準連結Type.Upper3CommonExists:
                    if (p.PartProgressionHigherPitch(Part.Bas) == 0) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp61_b));
                        return true;
                    }
                    if (p.PartProgressionHigherPitch(Part.Bas) == 12) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RareIp61_b));
                        return true;
                    }
                    break;
                default:
                    break;
                }
            }

            p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIp61_b));
            return true;
        }

        /// <summary>
        /// [2転]IV→[基]I I巻p61
        /// </summary>
        /// <returns>true: 先行和音、後続和音の音度は該当するもの([2転]IV→[基]I)だった。false:音度が違う。</returns>
        private bool 標準連結2転4to基1(Progression p) {
            if (p.PreC.ChordDegree != CD.IV ||
                p.NowC.ChordDegree != CD.I) {
                return false;
            }

            標準連結Type spt;
            if (Is標準連結(p, 標準連結Rule.BasSopContrary, out spt)) {
                switch (spt) {
                case 標準連結Type.Upper3CommonExists:
                    if (p.PartProgressionHigherPitch(Part.Bas) == 0) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp61_b_2));
                        return true;
                    }
                    if (p.PartProgressionHigherPitch(Part.Bas) == -12) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.StandardIp61_b_2_2));
                        return true;
                    }
                    break;
                default:
                    break;
                }
            }

            p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIp61_b_2));
            return true;
        }

        /// <summary>
        /// [2転]I→[基]V I巻p61
        /// </summary>
        /// <returns>true: 先行和音、後続和音の音度は該当するもの([2転]I→[基]V)だった。false:音度が違う。</returns>
        private bool 標準連結2転1to基5(Progression p) {
            if (p.PreC.ChordDegree != CD.I ||
                p.NowC.ChordDegree != CD.V) {
                return false;
            }

            標準連結Type spt;
            if (Is標準連結(p, 標準連結Rule.BasSopContrary, out spt)) {
                switch (spt) {
                case 標準連結Type.Upper3CommonExists: {
                        int intervalNumber = p.PartProgressionHigherInterval(Part.Bas);
                        if (intervalNumber == 0 || intervalNumber == -7) {
                            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp61_c));
                            return true;
                        }
                        return true;
                    }
                default:
                    break;
                }
            }

            p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIp61_c));
            return true;
        }

        /// <summary>
        /// [基]IV→[2転]I
        /// </summary>
        /// <returns>true: 先行和音、後続和音の音度は該当するもの([基]IV→[2転]Iまたは[1転]IV→[2転]I)だった。false:音度が違う。</returns>
        private bool 標準連結4to2転1(Progression p) {
            if (p.PreC.ChordDegree != CD.IV ||
                p.NowC.ChordDegree != CD.I) {
                return false;
            }

            標準連結Type spt;
            if (Is標準連結(p, 標準連結Rule.BasSopContrary, out spt)) {
                switch (spt) {
                case 標準連結Type.Upper3CommonExists:
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp64_37_2));
                    p.NowC.UpdateFunction(FunctionType.Dominant);
                    return true;
                default:
                    break;
                }
            }

            p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIp64_37_2));
            return true;
        }

        /// <summary>
        /// [基]→[基]の通常の標準連結判定。
        /// </summary>
        private void 標準連結基to基(Progression p) {
            標準連結Type spt;
            if (Is標準連結(p, 標準連結Rule.BasSopContrary, out spt)) {
                switch (spt) {
                case 標準連結Type.Upper3CommonExists:
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp30));
                    break;
                case 標準連結Type.Upper3CommonNotExist:
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp32));
                    break;
                case 標準連結Type.PrevChordIsClosed5SopranoNonInverted:
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.StandardIp109_4));
                    break;
                case 標準連結Type.Not標準連結:
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
            }
        }

        private bool CheckSpecialProgression(Progression p) {
            if (p.PreC.IsStandard && p.NowC.IsStandard) {
                // 共通音を保留しない連結 I巻 p123 2 A 
                // 2A ? はすべて標準配置

                if (p.Is("[基]I密(根)", "[基]IV密(3)", "↑4Bas↓3Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2A, 1));
                    return true;
                }
                if (p.Is("[基]V密(根)","[1転]IOct3省(根)", "↓Bas↑Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2A, 2));
                    return true;
                }
                if (p.Is("[3転]V7密(根)", "[1転]IOct3省(根)", "↓Bas↑Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2A, 3));
                    return true;
                }
                if (p.Is("[基]I密(5)", "[1転]V7密(7)", "↓Bas↓Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2A, 4));
                    return true;
                }
                if (p.Is("[1転]IOct3省(根)", "[基]I開(5)", "↓Bas↑Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2A, 5));
                    return true;
                }

                // 配分転換を生ずる連結 I巻 p123 2 B
                // 2B ? はすべて標準配置

                if (p.Is("[基]VI密(3)", "[基]II開(3)", "↓Bas↑Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2B, 1));
                    return true;
                }
                if (p.Is("[基]II開(3)", "[基]V密(3)", "↑Bas↓Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2B, 2));
                    return true;
                }
                if (p.Is("[基]I密(根)", "[基]V開(5)", "↑Bas↑Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2B, 3));
                    return true;
                }
                if (p.Is("[基]I開(3)", "[基]V密(3)", "↑Bas↓Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2B, 4));
                    return true;
                }
                if (p.Is("[基]I開(3)", "[基]VI密(3)", "↓Bas↓Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2B, 5));
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2A));
                    return true;
                }
                if (p.Is("[基]I密(根)", "[基]VI開(5)", "↓Bas↑Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2B, 6));
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2A));
                    return true;
                }
                if (p.Is("[1転]V開3省5重(根)", "[基]I密(3)", "↑Bas↓Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2B, 7));
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2A));
                    return true;
                }
                if (p.Is("[基]I密(3)", "[1転]IV開3省5重(根)", "↓Bas↑Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2B, 8));
                    return true;
                }
                if (p.Is("[基]I密(3)", "[2転]IV開(3)", "→Bas↑Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2B, 9));
                    return true;
                }
                if (p.Is("[2転]I密(根)", "[基]V開(5)", "→Bas↑Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2B, 10));
                    return true;
                }
                if (p.Is("[2転]I開(3)", "[基]V密(3)", "→Bas↓Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2B, 11));
                    return true;
                }
                if (p.Is("[基]I密(根)", "[1転]V7開(7)", "↓Bas↑Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2B, 12));
                    return true;
                }
                if (p.Is("[基]I開(3)", "[2転]V7密(3)", "↑Bas↓Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2B, 13));
                    return true;
                }
                if (p.Is("[基]I開(5)", "[1転]V7密(7)", "↓Bas↓Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2B, 14));
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2A));
                    return true;
                }
                if (p.Is("[3転]V7密(3)", "[1転]I開3省5重(根)", "↓Bas↑Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2B, 15));
                    return true;
                }
                if (p.Is("[3転]V7開(根)", "[1転]I密3省根重(根)", "↓Bas↓Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2B, 16));
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2A));
                    return true;
                }
                if (p.Is("[基]I密(根)", "[1転]I開3省5重(5)", "↑Bas↑Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2B, 18));
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2A));
                    return true;
                }
                if (p.Is("[1転]I開3省5重(5)", "[基]I密(3)", "↓Bas↓Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2B, 19));
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2A));
                    return true;
                }
            }

            if (p.Is("標[基]V7開(根)", "[基]VI密根省3重(3)", "↑Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2B, 17));
                return true;
            }

            // 標準外的配置をふくむ連結 I巻 p124 2 C

            if (p.Is("外[基]VIOct根省(3)", "外[基]IVOct根省(5)", "↓Bas→Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp124_2C, 1));
                return true;
            }
            if (p.Is("標[基]VI開(3)", "外[基]IIOct根省(5)", "↑Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp124_2C, 2));
                return true;
            }
            if (p.Is("標[基]VI開(3)", "外[基]II開根省3重(5)", "↑Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp124_2C, 3));
                p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2A));
                return true;
            }
            if (p.Is("標[基]I密(5)", "外[基]VIOct根省(3)", "↑Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp124_2C, 4));
                return true;
            }
            if (p.Is("外[1転]IV密(根)", "標[1転]VOct3省(根)", "↑Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp124_2C, 5));
                return true;
            }
            if (p.Is("標[1転]IOct3省(根)", "外[基]IVOct根省(5)", "↑Bas→Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp124_2C, 6));
                return true;
            }
            if (p.Is("外[基]I開根省5重(5)", "標[2転]V密(3)", "↑Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp124_2C, 7));
                p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2B));
                return true;
            }
            if (p.Is("外[基]IOct根省(5)", "外[2転]VOct5省(根)", "↑Bas→Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp124_2C, 8));
                return true;
            }
            if (p.Is("標[1転]V7開(5)", "外[基]IOct根省(5)", "↑Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp124_2C, 9));
                return true;
            }
            if (p.Is("標[1転]V7密(5)", "外[基]IOct根省(3)", "↑Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp124_2C, 10));
                return true;
            }

            if (p.PreC.IsStandard && p.NowC.IsStandard) {

                // 第7音の2度上行をふくむ連結 I巻 p124 2 E

                if (p.Is("[2転]V7開(3)", "[1転]I開3省5重(根)", "↑1Alt")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp124_2E, 1));
                    return true;
                }
                if (p.Is("[2転]V7開(根)", "[1転]IOct3省(5)", "↑1Ten")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp124_2E, 2));
                    return true;
                }

                // 予備のない低音4度・低音2度 I巻 p125 2 F

                if (p.Is("[1転]IOct3省(根)", "[2転]V7密(3)", "↓Bas↓Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp125_2F, 1));
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp123_2A));
                    return true;
                }
                if (p.Is("[基]VI密(3)", "[2転]V7密(3)", "↑Bas↓Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp125_2F, 2));
                    return true;
                }
                if (p.Is("[基]IV開(5)", "[2転]V7開(3)", "↓Bas↓Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp125_2F, 3));
                    return true;
                }
                if (p.Is("[基]II開(3)", "[3転]V7密(3)", "↑Bas↓Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp125_2F, 4));
                    return true;
                }
                if (p.Is("[基]II開(根)", "[3転]V7開(3)", "↑Bas↓Sop")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp125_2F, 5));
                    return true;
                }
            }

            // 短調のVI→V I巻p126 2 H
            if (!p.PreC.IsMajor() && p.Is("外[基]VIOct根省(3)", "標[基]V密(3)", "↓Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp126_2H_2));
                return true;
            }

            // II→VまたはV7の標準外的連結。
            if (p.PreC.IsMajor() && p.Is("外[基]II開根省3重(5)", "標[基]V7開5省(3)", "↓Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp126_2I, 3));
                p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp124_2C));
                return true;
            }
            if (p.PreC.IsMajor() && p.Is("標[基]II密(5)", "標[3転]V7密(3)", "↑Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp126_2I, 4));
                p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp125_2F));
                return true;
            }

            if (p.Is("標[基]I密(3)", "標[1転]IV開3省5重(根)", "↑Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "I巻補充課題8-6"));
                return true;
            }

            if (p.Is("標[基]I密(根)", "標[1転]IV開3省5重(根)", "↑Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "I巻課題44-6"));
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.CommonPitchNoSustainedProgression, "I巻課題44-6"));
                return true;
            }

            if (p.Is("標[3転]V7密(3)", "外[1転]I密(根)", "↓Bas↑Ten→Alt↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.NonStandardChordProgression, "I巻課題34-4"));
                return true;
            }

            if (p.Is("標[2転]II7開(3)", "標[2転]I密(3)", "↓Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻p13,3"));
                return true;
            }

            if (p.Is("標[基]I開(3)", "標[基]VI密(3)", "↑Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻課題4-2"));
                return true;
            }
            if (p.Is("標[基]V開(根)", "標[1転]I密3省根重(根)", "↓Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻課題4-5)(II巻補充課題6-13"));
                return true;
            }
            if (p.Is("標[基]I密(根)", "標[1転]V開3省根重(根)", "↓Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻課題4-7"));
                return true;
            }
            if (p.Is("標[1転]IOct3省(根)", "外[基]VIOct根省(3)", "↑Bas→Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.NonStandardChordProgression, "II巻補充課題1-1"));
                return true;
            }
            if (p.Is("標[3転]II7密(根)", "標[1転]V開3省根重(根)", "↓Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題1-1"));
                return true;
            }
            if (p.Is("標[1転]I密3省(5)", "外[基]VIOct根省(3)", "↑Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.NonStandardChordProgression, "II巻補充課題1-5"));
                return true;
            }
            if (p.Is("標[2転]V9根省開(3)", "標[1転]I開3省5重(根)", "↑Alt")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.Inv4Up2Progression, "II巻課題8-5"));
                return true;
            }
            if (p.Is("標[基]I密(3)", "標[基]IV開(3)", "↓Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻課題8-9"));
                return true;
            }
            if (p.Is("標[1転]I開3省根重(根)", "標[2転]V7密(7)", "↓Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題2-1"));
                return true;
            }
            if (p.Is("標[2転]II7密(根)", "標[2転]I開(5)", "↓Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題2-7"));
                return true;
            }
            if (p.Is("標[1転]I密3省根重(根)", "標[1転]IV開3省5重(5)", "↑Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題3-1"));
                return true;
            }
            if (p.Is("標[基]I開(5)", "標[1転]I密3省(根)", "↑Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題3-3"));
                return true;
            }
            if (p.Is("標[1転]VOct3省(根)", "外[基]IOct根省(5)", "↑Bas→Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.NonStandardChordProgression, "II巻補充課題3-3)(II巻補充課題5-10"));
                return true;
            }
            if (p.Is("標[1転]I密3省根重(5)", "標[基]I開(根)", "↓Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.CommonPitchNoSustainedProgression, "II巻補充課題3-10"));
                return true;
            }
            if (p.Is("標[1転]I開3省5重(5)", "外[基]VI開根省3重(5)", "↑Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.NonStandardChordProgression, "II巻補充課題4-6"));
                return true;
            }
            if (p.Is("標[1転]I開3省5重(5)", "外[基]I開根省5重(5)", "↓Bas→Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.NonStandardChordProgression, "II巻課題24-2)(II巻課題27-3"));
                return true;
            }
            if (p.Is("外[基]I密根省5重(3)", "標[基]V7密根省(5)", "↓Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.NonStandardChordProgression, "II巻課題24-6"));
                return true;
            }
            if (p.Is("標[基]I密(根)", "外[基]VIOct根省5重(5)", "↓Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.CommonPitchNoSustainedProgression, "II巻課題24-10"));
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.NonStandardChordProgression, "II巻課題24-10"));
                return true;
            }
            if (p.Is("標[1転]I密3省5重(5)", "標[基]I開(3)", "↓Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻課題24-10)(I巻補充課題4-4)(I巻p102,64 実施B)(II巻課題24-10)(II巻課題30-6)(II巻課題30-8"));
                return true;
            }
            if (p.Is("標[1転]II密(3)", "標[基]V7開根省(7)", "↑Bas→Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題5-10"));
                return true;
            }
            if (p.Is("標[1転]V開3省根重(根)", "標[基]I密(3)", "↑Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻課題27-1"));
                return true;
            }
            if (p.Is("標[1転]I開3省根重(根)", "外[基]VI開根省3重(3)", "↑Bas→Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.NonStandardChordProgression, "II巻補充課題7-3"));
                return true;
            }
            if (p.Is("標[基]I開(根)", "標[1転]I密3省5重(5)", "↑Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.CommonPitchNoSustainedProgression, "II巻補充課題7-3"));
                return true;
            }
            if (p.Is("標[基]I密(根)", "標[1転]IV開3省根重(根)", "↑Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題7-6"));
                return true;
            }
            if (p.Is("外[基]VI開根省3重(5)", "標[1転]V7密3省(7)", "↑Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題7-7"));
                return true;
            }
            if (p.Is("標[1転]I密3省根重(根)", "標[基]IV開(3)", "↑Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻課題34-5"));
                return true;
            }
            if (p.Is("標[基]I密(3)", "標[1転]I開3省根重(根)", "↓Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題9-3"));
                return true;
            }
            if (p.Is("標[3転]V7開(根)", "標[1転]I開3省根重(根)", "↓Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.CommonPitchNoSustainedProgression, "II巻補充課題9-4"));
                return true;
            }
            if (p.Is("標[1転]IV開3省5重(根)", "標[2転]I密(3)", "↓Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題9-9"));
                return true;
            }
            if (p.Is("標[基]V開(5)", "外[基]IOct5省(根)", "↑Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.NonStandardChordProgression, "II巻補充課題9-10"));
                return true;
            }
            if (p.Is("外[基]I開根省3重(3)", "外[基]VIOct根省(3)", "↑Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.NonStandardChordProgression, "II巻課題44-1"));
                return true;
            }
            if (p.Is("標[1転]I密3省5重(根)", "外[基]IOct根省(3)", "↓Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.NonStandardChordProgression, "II巻課題44-6"));
                return true;
            }
            if (p.Is("標[基]IV開揃(3)", "標[基]II密揃(3)", "↓Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題10-2"));
                return true;
            }
            if (p.Is("標[1転]V7開3省(5)", "外[基]I開根省3重(3)", "↑Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.NonStandardChordProgression, "II巻補充課題10-8"));
                return true;
            }
            if (p.Is("標[1転]I開3省5重(根)", "標[基]IV密揃(3)", "↑Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題10-8"));
                return true;
            }
            if (p.Is("標[1転]I開3省5重(5)", "標[基]VI密揃(3)", "↑Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題10-9"));
                return true;
            }
            if (p.Is("標[基]I開揃(3)", "外[1転]II開3省5重(根)", "↑Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.NonStandardChordProgression, "II巻補充課題10-10"));
                return true;
            }
            if (p.Is("標[2転]I開揃(5)", "標[基]V7密(5)", "→Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題10-10"));
                return true;
            }
            if (p.Is("標[2転]V7密(5)", "標[1転]IOct3省(5)", "↑Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RareIp81, "II巻補充課題10-11その1"));
                return true;
            }
            if (p.Is("外[1転]II開3省5重(根)", "標[基]V密揃(3)", "↑Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.NonStandardChordProgression, "II巻補充課題10-11その2"));
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題10-11その2"));
                return true;
            }
            if (p.Is("標[1転]I開3省5重(5)", "外[基]IVOct根省(5)", "↑Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.NonStandardChordProgression, "II巻補充課題10-11その2"));
                return true;
            }
            if (p.Is("標[基]V密揃(根)", "標[1転]I開3省5重(5)", "↓Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.CommonPitchNoSustainedProgression, "II巻p.128例題15その1"));
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻p.128例題15その1"));
                return true;
            }
            if (p.Is("標[1転]V開3省根重(根)", "外[基]I密根省5重(3)", "↑Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.NonStandardChordProgression, "II巻課題50-1"));
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻課題50-1"));
                return true;
            }
            if (p.Is("標[1転]V開3省根重(根)", "外[基]IOct根省(3)", "↑Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.NonStandardChordProgression, "I巻補充課題4-4)(II巻課題50-2"));
                return true;
            }
            if (p.Is("標[基]I密揃(3)", "標[1転]I開3省根重(根)", "↑Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻課題50-7"));
                return true;
            }
            if (p.Is("標[1転]I開3省根重(根)", "標[1転]V7密3省(7)", "↓Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻課題50-10"));
                return true;
            }
            if (p.Is("標[3転]V7密(根)", "標[1転]I開3省5重(5)", "↓Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻課題50-10"));
                return true;
            }
            if (p.Is("標[基]I開揃(根)", "標[1転]V7開3省(根)", "↓Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.IIk50_12Progression));
                return true;
            }
            if (p.Is("外[基]VI密根省3重(3)", "標[1転]V7密3省(7)", "↑Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.NonStandardChordProgression, "II巻補充課題11-5"));
                return true;
            }
            if (p.Is("標[基]○IV密揃(根)", "標[基]V7開根省(7)", "↑Bas→Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題11-5"));
                return true;
            }
            if (p.Is("標[1転]I開3省根重(根)", "標[基]I密揃(3)", "↑Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題11-8"));
                return true;
            }
            if (p.Is("標[1転]I開3省根重(根)", "標[基]V_V密揃(3)", "↓Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題11-10"));
                return true;
            }

            /* Basの6度上行。救いようがない
            if (p.Is("標[基]V開揃(3)", "標[1転]I開3省5重(根)", "↑Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題12-01"));
                return true;
            }
            */
            if (p.Is("外[1転]II開3省5重(根)", "標[3転]V7密(3)", "→Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.NonStandardChordProgression, "II巻補充課題12-2"));
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題12-2"));
                return true;
            }
            if (p.Is("標[基]I開揃(根)", "標[1転]V7密3省(7)", "↓Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題12-3"));
                return true;
            }
            if (p.Is("標[1転]I開3省根重(根)", "外[基]VIOct根省(3)", "↑Bas→Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題12-3"));
                return true;
            }
            if (p.Is("外[1転]II開3省5重(根)", "標[基]V7開5省(3)", "↑Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.NonStandardChordProgression, "II巻補充課題12-4"));
                return true;
            }
            if (p.Is("標[基]VI開揃(3)", "標[基]II密揃(3)", "↑Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題12-5"));
                return true;
            }
            if (p.Is("標[基]II開揃(根)", "標[2転]V7密(3)", "→Bas↓Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題12-6"));
                return true;
            }
            if (p.Is("標[基]I開揃(3)", "外[1転]IV開揃(根)", "↓Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.NonStandardChordProgression, "II巻補充課題12-9"));
                return true;
            }
            if (p.Is("標[2転]I開(5)", "標[基]V7開(3)", "→Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.Exceptionalp166));
                return true;
            }
            if (p.Is("標[基]I密揃(根)", "標[3転]V7開(5)", "↑Bas↑Sop")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.PositionOfAChordChangeProgression, "II巻補充課題12-11"));
                return true;
            }
        
            return false;
        }

        /// <summary>
        /// 1巻p83に書いてあった[2転]V7の先行和音の制限。
        /// </summary>
        /// <returns>エラー回数</returns>
        private int CheckIp83_49_3_2(Progression p) {
            if (p.NowC.Is("[2転]V7") && 1 <= p.NowC.Upper3CountByInversion(Inversion.根音) &&
                (!p.PreC.Is("[基]I") && !p.PreC.Is("[1転]I") && !p.PreC.Is("[基]II"))) {
                p.UpdateVerdict(new Verdict(VerdictValue.NotSoGood, VerdictReason.NotSoGoodIp83_49_3_2));
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// 低音4度と低音2度のチェック。
        /// </summary>
        /// <returns>エラー回数</returns>
        private int CheckIp100_63_1(Progression p) {
            int result = 0;

            if (p.NowC.Inversion == Inversion.第5音 &&
                p.NowC.Upper3CountByInversion(Inversion.根音) == 1) {
                Part part = p.NowC.GetPartListByInversion(Inversion.根音)[0];
                if (p.NowC.TwoPartIntervalType(Part.Bas, part) == IntervalType.PerfectFourth &&
                    p.PartProgressionHigherPitch(Part.Bas) != 0 &&
                    p.PartProgressionHigherPitch(part) != 0) {
                    if (p.NowC.Is("V7")) {
                        // [2転]V7の低音4度。
                        p.UpdateVerdict(new Verdict(VerdictValue.NotSoGood, VerdictReason.NotSoGoodIp100_63_1_1, part));
                        ++result;
                    } else if (p.NowC.ChordDegree == CD.I) {
                        // [2転]Iの低音4度は良好。
                        //p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.NotSoGoodIp100_63_1_1, part));
                    } else if (p.NowC.ChordDegree == CD.V_V) {
                        // [2転]V_V7の低音4度。良好？(II巻課題15-9の実施例あり)
                        p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.NotSoGoodIp100_63_1_1, part));
                    } else if (p.NowC.ChordDegree == CD.IV) {
                        // [2転]IVはK3なので、低音4度でもOK。
                    } else {
                        // [2転]V7ではなく、[2転]Iでもなく、[2転]V_V7でも[2転]IVでもない場合。
                        p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.RuleD2, part));
                        ++result;
                    }
                }
            }
            if (p.NowC.ChordDegree == CD.V) {
                if (p.NowC.Inversion == Inversion.第7音 &&
                    p.NowC.Upper3CountByInversion(Inversion.根音) == 1) {
                    Part part = p.NowC.GetPartListByInversion(Inversion.根音)[0];
                    if (p.PartProgressionHigherPitch(Part.Bas) == 12) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RareIIk50_5));
                    } else if (p.PartProgressionHigherPitch(Part.Bas) != 0 &&
                        p.PartProgressionHigherPitch(part) != 0)
                    {
                        // Vの和音 低音2度
                        if (p.PreC.ChordDegree == CD.V_V &&
                            p.NowC.Is("[3転]V7")) {
                            p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoIIp35_18, part));
                        } else {
                            if (p.Is転調進行()) {
                                // @todo 別にいいらしい。？？？？
                            } else {
                                p.UpdateVerdict(new Verdict(VerdictValue.NotSoGood, VerdictReason.NotSoGoodIp100_63_1_2, part));
                                ++result;
                            }
                        }
                    }
                }
            } else {
                if (p.NowC.ChordDegree == CD.V_V ||
                    (p.NowC.ChordDegree == CD.IV && p.NowC.AlterationType == AlterationType.Dorian)) {
                    // 第7音の予備不要。
                } else {
                    // NowCはVの和音でもV_Vの和音でもドリアのIVでもない7の和音
                    var parts = p.NowC.GetPartListByInversion(Inversion.第7音);
                    foreach (Part part in parts) {
                        if (p.PartProgressionAbsPitch(part) == 12 && part == Part.Bas) {
                            p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RareBasOctaveOnSustain, 7)); 
                        } else if (p.PartProgressionHigherPitch(part) != 0) {
                            // 第7音が予備されていない。
                            if (p.NowC.IsMinor() && p.NowC.ChordDegree == CD.II) {
                                // 短調のII7の第7音は予備しなくてよい。
                                p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.MinorII77thNonSustain, part));
                            } else {
                                p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.RuleD1, part));
                                ++result;
                            }
                        }
                    }
                }
            }

            return result;
        }

        private bool 標準連結124toV7(Progression p) {
            if (!p.PreC.Is("I") && !p.PreC.Is("II") && !p.PreC.Is("IV")) {
                return false;
            }

            if (!IsStandardPointOfAChordProgression(p)) {
                // 標準連結になりうる配分連結が行われている必要がある。
                return true;
            }
            
            if (p.PreC.Inversion == Inversion.第5音) {
                if (p.PreC.ChordDegree != CD.I) {
                    // [2転]II→V7、[2転]IV→V7は連結できない。
                    return true;
                }

                var prePart5 = p.PreC.GetPartListByInversion(Inversion.第5音);
                if (p.NowC.Inversion == Inversion.根音) {
                    if (1 == p.NowC.Upper3CountByInversion(Inversion.根音)) {
                        if (prePart5.Count == 2 &&
                            p.PartProgressionHigherPitch(prePart5[1]) == 0) {
                            // [2転]I→[基]V7(上部構成音a)の場合、上3声の共通音Vを保留する。
                            // I巻p75実施例参照。
                            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIp74_1));
                            if (p.PartProgressionHigherPitch(Part.Bas) == 12) {
                                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RareIk44_5));
                            }
                            return true;
                        }
                    } else if (p.Upper3CheckAndAccumulate(part => p.PartProgressionHigherInterval(part) <= 0).Count >= 1) {
                        // [2転]I→[基]V7(上部構成音b)の場合、上3声をなるべく下行させる。
                        // I巻p78、課題28-2参照。
                        // I巻p125,2Gに1つしか下行していない例あり。
                        p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIp78_46_2));
                        if (p.PartProgressionHigherPitch(Part.Bas) == 12) {
                            p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RareIk44_5));
                        }
                        return true;
                    }
                }
                p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIp61_c));
                return true;
            }

            var nowPart5 = p.NowC.GetPartListByInversion(Inversion.第5音);
            if ((p.NowC.Is("[基]V7") && nowPart5.Count == 0) ||
                p.NowC.Inversion != Inversion.根音) {
                // 後続和音が[基]V7(上部構成音a)または[n転]V7

                if (p.PreC.Is("I")) {
                    var prePart5 = p.PreC.GetPartListByInversion(Inversion.第5音);
                    if (prePart5.Count == 2) {
                        // 先行和音Iの上3声にVが2つある場合
                        // ・片方を保留、他の声部は下行する(別巻課題I巻28-1、別巻課題I巻28-3)
                        // ・1個保留、1個下行、1個2度上行もOK(別巻課題I巻28-6)
                        // ・課題43-5その2 1個保留、1個3度上行
                        // …つまり共通音Vが1個保留していればあとはどうでもよい。
                        if (p.PartProgressionHigherPitch(prePart5[0]) == 0 ||
                            p.PartProgressionHigherPitch(prePart5[1]) == 0) {
                            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIp74_1));
                        }
                        return true;
                    }
                    if (prePart5.Count == 1 &&
                        p.PartProgressionHigherPitch(prePart5[0]) == 0) {
                        // 先行和音の上3声Vが保留している場合OK
                        p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIp74_1));
                        return true;
                    }
                    return true;
                }

                // 先行和音がIIかIVの場合。
                if (p.NowC.Inversion == Inversion.第7音) {
                    // 後続和音が[3転]V7の場合上3声に第7音(IV)がない。
                    if (p.Upper3CheckAndIfAllTrueSetVerdictOnce(part => p.PartProgressionHigherInterval(part) <= 0,
                        new Verdict(VerdictValue.Good, VerdictReason.GoodIp74_1))) {
                        // 上3声をすべて下行している場合OK。
                        // 別巻課題I巻28-10 共通音IIを保留、他の声部は下行
                    } else {
                        // 上3声を1つも下行していない場合もある。別巻課題I巻補充課題9-5
                        p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RareIh09_5));
                    }
                    return true;
                }
                if (p.PreC.ChordDegree == CD.II &&
                    p.NowC.Inversion == Inversion.第3音) {
                    // IIの和音の後続和音が[1転]V□の場合上3声のIIを保留していればOK
                    // I巻p109,5
                    p.Upper3CheckAndIfAllTrueSetVerdictOnce(
                        part => {
                            if (p.NowC.GetPitch(part).Inversion == Inversion.第5音) {
                                return p.PartProgressionHigherPitch(part) == 0;
                            }
                            return true;
                        },
                        new Verdict(VerdictValue.Good, VerdictReason.StandardIp109_5));
                }

                // 共通音IVを保留し、他の音を下行する。
                // I巻p74,44
                p.Upper3CheckAndIfAllTrueSetVerdictOnce(
                    part => {
                        if (p.NowC.GetPitch(part).Inversion == Inversion.第7音) {
                            return p.PartProgressionHigherPitch(part) == 0;
                        }
                        return p.PartProgressionHigherInterval(part) < 0;
                    },
                    new Verdict(VerdictValue.Good, VerdictReason.GoodIp74_1));

                // 別巻課題I巻28-10 共通音IVを保留し、他の音は2個とも2度上行もOK。
                p.Upper3CheckAndIfAllTrueSetVerdictOnce(
                    part => {
                        if (p.NowC.GetPitch(part).Inversion == Inversion.第7音) {
                            return p.PartProgressionHigherPitch(part) == 0;
                        }
                        return p.PartProgressionHigherPitch(part) == 1;
                    },
                    new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIk28_10));

                // 共通音IVが先行和音の上3声に2個ある場合、1個保留していればよい。
                // 別巻課題I巻43-7
                var IVparts = p.PreC.GetUpper3PartListByDegree(4);
                if (IVparts.Count == 2) {
                    if (p.PartProgressionHigherPitch(IVparts[0]) == 0 ||
                        p.PartProgressionHigherPitch(IVparts[1]) == 0) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIk43_7));
                    }
                }

                if (p.PreC.ChordDegree == CD.IV) {
                    // 別巻課題II巻27-7 IV→[1転]Vの和音で 共通音IVを保留し、他の1音は2度上行、もう一方の音は下行でもOK。
                    p.Upper3CheckAndIfAllTrueSetVerdictOnce(
                        part => {
                            if (p.NowC.GetPitch(part).Inversion == Inversion.第7音) {
                                return p.PartProgressionHigherPitch(part) == 0;
                            }
                            return p.PartProgressionHigherInterval(part) <= 1;
                        },
                        new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIIk27_7));
                }
                return true;
            }

            // 後続和音が[基]V7(上部構成音b)の場合
            // 共通音Vを保留しないでなるべく下行する。2音上行1個はOK。
            var down = p.Upper3CheckAndAccumulate(part => p.PartProgressionHigherInterval(part) < 0);
            if (down.Count == 3) {
                p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIp78_46_2));
            } else if (down.Count == 2) {
                var up = p.Upper3CheckAndAccumulate(part => p.PartProgressionHigherInterval(part) > 0);
                if (up.Count == 0 || up.Count == 1 && p.PartProgressionHigherInterval(up[0]) == 1) {
                    // 下行または保留のみの場合、または1個だけ2音上行はOK
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIp78_46_2));
                }
            } else {
                // 課題30-3に共通音Vを保留しないで2音下行、他の上3声は2音上行、3音上行の例あり
                p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIk30_3));
            }

            return true;
        }

        private bool 標準連結6toV7(Progression p) {
            if (!p.PreC.Is("VI")) {
                return false;
            }

            if (!IsStandardPointOfAChordProgression(p)) {
                // 標準連結になりうる配分連結が行われている必要がある。
                return true;
            }

            var nowPart5 = p.NowC.GetPartListByInversion(Inversion.第5音);
            if (p.NowC.Inversion != Inversion.根音 || nowPart5.Count != 1) {
                // 先行和音VIからV7の連結は
                // [基]V7(上部構成音b)の場合の例しか書かれていない。
                return true;
            }

            // なるべく上3声を上行させる。
            var up = p.Upper3CheckAndAccumulate(part => p.PartProgressionHigherInterval(part) > 0);
            if (up.Count == 3) {
                p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIp78_46_2));
            } else if (up.Count == 2) {
                var down = p.Upper3CheckAndAccumulate(part => p.PartProgressionHigherInterval(part) < 0);
                if (down.Count == 1 && p.PartProgressionHigherInterval(down[0]) == -1) {
                    // 1個だけ2音下行はOK
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIp78_46_2));
                }
            }

            return true;
        }

        private bool V7LimitedProgression(Progression p) {
            var part7 = p.PreC.GetPartListByInversion(Inversion.第7音);
            System.Diagnostics.Debug.Assert(part7.Count == 1);

            int failCount = 0;

            // 導音の進行はすでにチェック済みなので第7音の進行をチェックする。

            if (p.ChordsAre("[2転]V7開(3)", "[1転]I開(根)3省") ||
                p.ChordsAre("[2転]V7開(根)", "[1転]IOct(5)")) {
                // 第7音が2度上行してもよい。2度下行してもよい。I巻p124E
                int part7ProgInterval = p.PartProgressionHigherInterval(part7[0]);
                if (part7ProgInterval == 1) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp124_2E, part7[0]));
                }
                if (part7ProgInterval != -1 &&
                    part7ProgInterval != 1) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIp72_3, part7[0]));
                    ++failCount;
                }
            } else {
                if (p.PartProgressionHigherInterval(part7[0]) != -1) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIp72_3, part7[0]));
                    ++failCount;
                }
            }
            return failCount == 0;
        }

        private void 標準連結基V7to1(Progression p) {
            if (p.NowC.Inversion != Inversion.根音) {
                return;
            }

            var part1 = p.PreC.GetPartListByInversion(Inversion.根音);
            var part5 = p.PreC.GetPartListByInversion(Inversion.第5音);
            System.Diagnostics.Debug.Assert(1 <= part1.Count);
            System.Diagnostics.Debug.Assert(part5.Count <= 1);

            int failCount = 0;
            if (part1.Count == 2 && p.PartProgressionHigherPitch(part1[1]) != 0) {
                p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIp72_1, part1[1]));
                ++failCount;
            }
            if (part5.Count == 1 && p.PartProgressionHigherInterval(part5[0]) != -1) {
                p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIp75, part5[0]));
                ++failCount;
            }
            if (failCount == 0) {
                if (p.NowC.NumberOfNotes == NumberOfNotes.Seventh) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIIp218_77_3_5));
                } else {
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIp72));
                }
            }
        }

        private bool 標準連結V7toI(Progression p) {
            if (!p.NowC.Is("I") && !p.NowC.Is("I7")) {
                return false;
            }

            if (!IsStandardPointOfAChordProgression(p)) {
                // 標準連結になりうる配分連結が行われている必要がある。
                return true;
            }

            if (p.NowC.NumberOfNotes == NumberOfNotes.Triad) {
                // Basの定型
                switch (p.PreC.Inversion) {
                case Inversion.根音:
                    if (p.NowC.Inversion != Inversion.根音) {
                        return true;
                    }
                    break;
                case Inversion.第3音:
                    if (p.NowC.Inversion != Inversion.根音) {
                        return true;
                    }
                    break;
                case Inversion.第5音:
                    if (p.NowC.Inversion != Inversion.根音 &&
                        p.NowC.Inversion != Inversion.第3音) {
                        return true;
                    }
                    break;
                case Inversion.第7音:
                    if (p.NowC.Inversion != Inversion.第3音) {
                        return true;
                    }
                    break;
                default:
                    break;
                }
            }

            if (!V7LimitedProgression(p)) {
                return true;
            }

            switch (p.PreC.Inversion) {
            case Inversion.根音:
                // [基]V7 上部構成音a (上3声にVあり)
                // [基]V7 上部構成音b
                標準連結基V7to1(p);
                break;
            case Inversion.第3音:
            case Inversion.第5音:
            case Inversion.第7音:
                {
                    var part1 = p.PreC.GetPartListByInversion(Inversion.根音);
                    if (part1.Count == 1 && p.PartProgressionHigherPitch(part1[0]) == 0) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIp72));
                    }
                }
                break;
            }

            return true;
        }

        /// <summary>
        /// [2転]V7→上3声に第3音を含む[1転]I→□の進行の制限(I巻p73,43) cf.(II巻課題4-1)(II巻課題24-1)(II巻課題30-10)
        /// </summary>
        private bool 標準連結Ip73_43(Progression p) {
            if (p.PrepreC == null || !p.PrepreC.Is("[2転]V7") ||
                !p.PreC.Is("[1転]I") || p.PreC.GetUpper3PartListByInversion(Inversion.第3音).Count != 1) {
                return false;
            }

            if (p.NowC.Is("[1転]II") || p.NowC.Is("[1転]II7") || p.NowC.Is("[3転]V7") ||
                (p.NowC.ChordDegree == CD.V_V && p.NowC.Inversion == Inversion.第3音) ||
                p.NowC.Is("[基]IV7")) {
                // OKである
                // 後続和音は[1転]IIか[3転]V7しか無い。
                // II巻 課題4-1: [1転]II7も可。
                // II巻 課題24-1: [1転]V_V9も可。
                // II巻 課題30-10: [基]IV7も可。
            } else {
                // I巻p73,43 だめである。
                p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.RuleIp73_43));
                return true;
            }

            var prePart3s = p.PreC.GetPartListByInversion(Inversion.第3音);
            System.Diagnostics.Debug.Assert(prePart3s.Count == 2);

            if ((p.NowC.ChordDegree == CD.V_V && p.NowC.Inversion == Inversion.第3音) ||
                p.NowC.Is("[基]IV7")) {
                // [1転]V_V9の場合、上3声のIIIが保留や増1度進行でも良い。
                // [基]IV7も同様。
                int interval = p.PartProgressionHigherInterval(prePart3s[1]);
                if (interval == 0 || interval == -1) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.RuleIp73_43));
                    return true;
                }
            }

            if (p.PartProgressionHigherInterval(prePart3s[1]) != -1) {
                p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.RuleIp73_43));
                return true;
            }

            p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.RuleIp73_43));
            return true;
        }

        private bool 標準連結V7to6(Progression p) {
            if (!p.NowC.Is("VI")) {
                return false;
            }

            if (p.PreC.Inversion != Inversion.根音 ||
                p.NowC.Inversion != Inversion.根音) {
                // [基]V7のみ[基]VIに進行することができる。
                return true;
            }

            if (!V7LimitedProgression(p)) {
                return true;
            }

            var part5 = p.PreC.GetPartListByInversion(Inversion.第5音);
            System.Diagnostics.Debug.Assert(part5.Count <= 1);
            if (part5.Count == 1 && p.PartProgressionHigherInterval(part5[0]) == -1) {
                // 第5音はつねに2度下行する。
                p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIp77_46));
            }

            return true;
        }

        /// <summary>
        /// □→[2転]V7根省
        /// </summary>
        /// <returns></returns>
        private bool 標準連結to2転V7根省(Progression p) {
            if (!p.NowC.Is("[2転]V7根省")) {
                return false;
            }

            if (!IsStandardPointOfAChordProgression(p)) {
                // 標準連結になりうる配分連結が行われている必要がある。
                return true;
            }

            // Ip83 先行和音に特に制限なし。

            if (3 == p.NowC.Upper3NumOfInversionVariation()) {
                if (p.NowC.Is("密(3)") || p.NowC.Is("密(5)") || p.NowC.Is("開(7)")) {
                    // Ip81,49 上部構成音a)の最適配置
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.BestIp81_49_a));
                }
                if (p.NowC.Is("開(3)")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RareIh06_03));
                }
            }
            if (p.NowC.Is("Oct(7)")) {
                // Ip83,49 上部構成音b)の最適配置
                p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.BestIp83_49_b));
            } else if (p.NowC.Upper3CountByInversion(Inversion.第7音) == 2) {
                // 上部構成音b) I巻課題44-6
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RareIk44_6));
            }

            return true;
        }

        /// <summary>
        /// [2転]V7根省→□
        /// </summary>
        private bool 標準連結2転V7根省toX(Progression p) {
            if (!p.PreC.Is("[2転]V7根省")) {
                return false;
            }

            if (p.PartProgressionAbsInterval(Part.Bas) != 1) {
                p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIp80));
                return true;
            }

            if (p.PreC.Upper3CountByInversion(Inversion.第7音) == 2) {
                // 上部構成音b)の場合
                p.Upper3CheckAndIfAllTrueSetVerdictOnce(part => {
                    switch (p.PreC.GetPitch(part).Inversion) {
                    case Inversion.第3音: return p.PartProgressionHigherInterval(part) == 1;
                    case Inversion.第7音: {
                            int interval = p.PartProgressionHigherInterval(part);
                            if (part != Part.Sop) {
                                return interval == -1 || interval == 1;
                            }
                            return interval == -1;
                        }
                    default:
                        System.Diagnostics.Debug.Assert(false);
                        return false;
                    }
                }, new Verdict(VerdictValue.Good, VerdictReason.GoodIp82));
                return true;
            }

            if (p.PreC.Is("密(3)") || p.PreC.Is("密(5)") || p.PreC.Is("開(7)") || p.PreC.Is("開(3)")) {
                // 上部構成音a)の場合
                Part part7 = p.PreC.GetPartListByInversion(Inversion.第7音)[0];
                if (part7 != Part.Sop && part7 != Part.Bas &&
                    p.PartProgressionHigherInterval(part7) == 1) {
                    // 第3音は2度上行
                    // 第5音は順次進行する。
                    // 第7音が2度上行
                    p.Upper3CheckAndIfAllTrueSetVerdictOnce(part => {
                        switch (p.PreC.GetPitch(part).Inversion) {
                        case Inversion.第3音: return p.PartProgressionHigherInterval(part) == 1;
                        case Inversion.第5音: return p.PartProgressionAbsInterval(part) == 1;
                        case Inversion.第7音: return p.PartProgressionHigherInterval(part) == 1;
                        default:
                            System.Diagnostics.Debug.Assert(false);
                            return false;
                        }
                    }, new Verdict(VerdictValue.Good, VerdictReason.GoodIp81));
                } else {
                    // 第3音は2度上行
                    // 第5音は5度下行か4度上行
                    // 第7音は2度下行
                    p.Upper3CheckAndIfAllTrueSetVerdictOnce(part => {
                        switch (p.PreC.GetPitch(part).Inversion) {
                        case Inversion.第3音: return p.PartProgressionHigherInterval(part) == 1;
                        case Inversion.第5音: {
                                int interval = p.PartProgressionHigherInterval(part);
                                return interval == -4 || interval == 3;
                            }
                        case Inversion.第7音: return p.PartProgressionHigherInterval(part) == -1;
                        default:
                            System.Diagnostics.Debug.Assert(false);
                            return false;
                        }
                    }, new Verdict(VerdictValue.Good, VerdictReason.GoodIp81));
                }
            }

            return true;
        }

        /// <summary>
        /// □→[1転]V7根省または[3転]V7根省 Ip126
        /// </summary>
        private bool 標準連結to1or3転V7根省(Progression p) {
            if (!p.NowC.Is("[1転]V7根省") && !p.NowC.Is("[3転]V7根省")) {
                return false;
            }

            if (!IsStandardPointOfAChordProgression(p)) {
                // 標準連結になりうる配分連結が行われている必要がある。
                return true;
            }

            // Ip126 先行和音に特に制限なし。
            // 配置についても特に書いてない。
            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIp126_3));

            return true;
        }

        /// <summary>
        /// 2個の第5音のうち一方が2音下行しもう一方が5度下行または4度上行するとtrue
        /// </summary>
        private bool Inv5TwoPartsAreIp126Progression(Progression p) {
            List<Part> secondInvs = p.PreC.GetPartListByInversion(Inversion.第5音);
            if (secondInvs.Count != 2) {
                return false;
            }

            bool down2 = false;
            bool down5OrUp4 = false;
            foreach (Part part in secondInvs) {
                int interval = p.PartProgressionHigherInterval(part);
                if (interval == -1) {
                    down2 = true;
                }
                if (interval == -4 || interval == 3) {
                    down5OrUp4 = true;
                }
            }
            return down2 && down5OrUp4;
        }

        /// <summary>
        /// [1転]V7根省または[3転]V7根省→I Ip126
        /// </summary>
        private bool 標準連結1or3転V7根省to1(Progression p) {
            if (!p.PreC.Is("[1転]V7根省") && !p.PreC.Is("[3転]V7根省")) {
                return false;
            }

            if (!IsStandardPointOfAChordProgression(p)) {
                // 標準連結になりうる配分連結が行われている必要がある。
                return true;
            }

            // Ip127
            if (!Inv5TwoPartsAreIp126Progression(p)) {
                return true;
            }

            if (p.AllPartCheckAndIfAllTrueSetVerdictOnce(part => {
                switch (p.PreC.GetPitch(part).Inversion) {
                case Inversion.第3音: return p.PartProgressionHigherInterval(part) == 1;
                case Inversion.第5音: return true;
                case Inversion.第7音: return p.PartProgressionHigherInterval(part) == -1;
                default: System.Diagnostics.Debug.Assert(false); return false;
                }
            }, new Verdict(VerdictValue.Good, VerdictReason.GoodIp126_3_2))) { }

            return true;
        }

        /// <summary>
        /// V9の和音の第9音高位の並達9度。根音省略形体では起こらない。
        /// </summary>
        private bool CheckV9第9音高位の並達9度(Progression p) {
            // 15度でも9度という
            int interval = p.NowC.TwoPartIntervalNumber(Part.Bas, Part.Sop);
            bool is9度 = false;
            if (8 <= interval && (interval % 7) == 1) {
                is9度 = true;
            }

            if (p.NowC.SopInversion == Inversion.第9音 &&
                is9度 &&
                p.TwoPartMotion(Part.Bas, Part.Sop) == Motion.Parallel) {
                if (p.PartProgressionHigherInterval(Part.Sop) == 1) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoIp88_55));
                    return false;
                } else {
                    p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIp88_55));
                    return true;
                } 
            }
            return false;
        }

        /// <summary>
        /// V_V9の和音の第9音高位の並達9度。II巻p37,18 2)
        /// </summary>
        private bool CheckV_V9第9音高位の並達9度(Progression p) {
            // 15度でも9度という
            int interval = p.NowC.TwoPartIntervalNumber(Part.Bas, Part.Sop);
            bool is9度 = false;
            if (8 <= interval && (interval % 7) == 1) {
                is9度 = true;
            }

            if (p.NowC.SopInversion == Inversion.第9音 &&
                is9度 &&
                p.TwoPartMotion(Part.Bas, Part.Sop) == Motion.Parallel) {
                // 並達9度が発生。
                if (p.PreC.ChordDegree == CD.IV &&
                    p.PreC.Inversion == Inversion.第3音 &&
                    p.PartProgressionHigherInterval(Part.Sop) == -1) {
                    // [1転]IVからSopが2度下行で到達した並達9度は問題ない。
                    p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoIIp37_18_2));
                    return false;
                } else {
                    p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIp37_18_2));
                    return true;
                }
            }
            return false;
        }

        private bool 標準連結toV9(Progression p) {
            if (!IsStandardPointOfAChordProgression(p)) {
                // 標準連結になりうる配分連結が行われている必要がある。
                return true;
            }
            if (p.PreC.ChordDegree == CD.I &&
                p.PreC.Inversion == Inversion.第5音) {
                // [2転]I→V9という進行はないと思われる。
                return true;
            }

            if (p.NowC.Omission == Omission.None) {
                if (CheckV9第9音高位の並達9度(p)) {
                    return true;
                }

                if (p.NowC.Inversion == Inversion.第3音 || p.NowC.Inversion == Inversion.第7音) {
                    // I巻p128,4 Vはほとんど保留。
                    var vPart = p.NowC.GetPartListByInversion(Inversion.根音)[0];
                    if (p.PartProgressionHigherPitch(vPart) != 0) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RareIp128_4, vPart));
                    }
                }

                // 根音あり。
                if (p.NowC.IsMajor() && !p.NowC.Is準固有和音) {
                    if (p.NowC.SopInversion == Inversion.第9音) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.BestV9));
                    } else if (p.NowC.Is("[基](3)") || p.NowC.Is("[基](7)")) {
                        // 最適以外の配置 Ip128_5
                        p.UpdateVerdict(new Verdict(VerdictValue.Okay, VerdictReason.OkayIp128_5));
                    }
                } else {
                    if (p.NowC.SopInversion == Inversion.第9音) {
                        // 第9音高位 最適
                        p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.BestV9));
                    } else {
                        // 短調のV9 I巻p86
                        p.UpdateVerdict(new Verdict(VerdictValue.Okay, VerdictReason.OkayIp86_53));
                    }
                }
                return true;
            }

            // 根音省略形態。
            if (p.NowC.IsMajor() && !p.NowC.Is準固有和音) {
                switch (p.NowC.Inversion) {
                case Inversion.第3音:
                    if (p.NowC.Is("密(7)")) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.Best1転V9));
                    }
                    break;
                case Inversion.第5音:
                case Inversion.第7音:
                    if (p.NowC.SopInversion == Inversion.第9音) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.Best2転3転V9));
                    }
                    break;
                default:
                    break;
                }
                if (p.NowC.Is("[1転](5)") || p.NowC.Is("[1転](7)") || p.NowC.Is("[1転](9)") ||
                    p.NowC.Is("[2転](3)") || p.NowC.Is("[2転](7)") ||
                    p.NowC.Is("[3転](3)") || p.NowC.Is("[3転](5)")) {
                    // 最適以外の配置 Ip128_5
                    p.UpdateVerdict(new Verdict(VerdictValue.Okay, VerdictReason.OkayIp128_5));
                }
                return true;
            }
            // 短調のV9　I巻p94
            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIp94_59));
            return true;
        }

        private bool 標準連結V9toI(Progression p) {
            if (p.NowC.ChordDegree != CD.I) {
                p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIp87_54));
                return true;
            }

            if (!IsStandardPointOfAChordProgression(p)) {
                // 標準連結になりうる配分連結が行われている必要がある。
                return true;
            }

            switch (p.PreC.Inversion) {
            case Inversion.根音:
                if (p.PreC.Omission == Omission.None) {
                    // [基]V9の定型 →[基]I
                    if (p.NowC.Inversion != Inversion.根音) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIp87_54_2));
                        return true;
                    }
                }
                break;
            case Inversion.第3音:
                if (p.NowC.Inversion != Inversion.根音) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIp89_56_1));
                    return true;
                }
                if (p.PreC.Omission == Omission.First) {
                    // [1転]V9根省 →[基]I
                } else {
                    // [1転]V9 I巻p128 4
                    // 共通音Vを保留する
                    var vPart = p.PreC.GetPartListByInversion(Inversion.根音)[0];
                    if (p.PartProgressionHigherPitch(vPart) != 0) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIp128_4_1, vPart));
                        return true;
                    }
                }
                break;
            case Inversion.第5音:
                if (p.PreC.Omission == Omission.First) {
                    // [2転]V9根省 →[1転]I
                    if (p.NowC.Inversion != Inversion.第3音) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIp89_56_2));
                        return true;
                    }
                }
                break;
            case Inversion.第7音:
                if (p.NowC.Inversion != Inversion.第3音) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIp89_56_3));
                    return true;
                }
                if (p.PreC.Omission == Omission.First) {
                    // [3転]V9根省 →[1転]I
                } else {
                    // [3転]V9 I巻p128 4
                    // 共通音Vを保留する
                    var vPart = p.PreC.GetPartListByInversion(Inversion.根音)[0];
                    if (p.PartProgressionHigherPitch(vPart) != 0) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIp128_4_1, vPart));
                        return true;
                    }
                }
                break;
            default:
                break;
            }

            // 第3音は2度上行
            // 第7音は2度下行
            // 第9音は2度下行
            bool upper7Up = false;
            if (p.Upper3CheckAndIfAllTrueSetVerdictOnce(part =>
                {
                    switch (p.PreC.GetPitch(part).Inversion) {
                    case Inversion.根音: return true;
                    case Inversion.第3音: return p.PartProgressionHigherInterval(part) == 1;
                    case Inversion.第5音: return true;
                    case Inversion.第7音:
                        if (((p.PreC.IsMajor() && p.PreC.Is("[2転]V9根省開(9)"))) || (!p.PreC.IsMajor() && p.PreC.Is("[2転]V9根省")) &&
                            part < p.PreC.GetPartListByInversion(Inversion.第3音)[0]) {
                            if (p.PartProgressionHigherInterval(part) == 1) {
                                // 第7音の進行の例外(I巻p91)(I巻p114公理B2付則2)
                                // 2度上行推奨。
                                upper7Up = true;
                                return true;
                            }
                        }
                        return p.PartProgressionHigherInterval(part) == -1;
                    case Inversion.第9音: return p.PartProgressionHigherInterval(part) == -1;
                    default: System.Diagnostics.Debug.Assert(false); return false;
                    }
                }, new Verdict(VerdictValue.Good, VerdictReason.GoodIp87_54))) {
                if (upper7Up) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.RecommendIp91));
                }
                return true;
            }

            return true;
        }

        /// <summary>
        /// [2転]IからVへの定型進行ルールに従っているか。
        /// </summary>
        /// <returns>true: 関係ない和音か、定型ルールに従っている。false: 定型ルールに従っていない進行だった。</returns>
        private bool 定型ルール2転ItoV(Progression p) {
            if (p.NowC.ChordDegree != CD.V) {
                return false;
            }

            if (p.NowC.Inversion != Inversion.根音) {
                p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIp61_c));
                return false;
            }

            if (p.PartProgressionHigherInterval(Part.Bas) == 7) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RareIk44_5));
                return true;
            }
            return true;
        }

        /// <summary>
        /// [基]I→[2転]V→[1転]Iまたは[1転]I→[2転]V→[基]Iの定型進行ルールに従っているか。
        /// </summary>
        /// <returns>true: 関係ない和音か、定型ルールに従っている。false: 定型ルールに従っていない進行だった。</returns>
        private bool 定型ルール2転VtoI(Progression p) {
            if (p.PreC.Inversion != Inversion.第5音 ||
                p.PreC.ChordDegree != CD.V ||
                p.PreC.NumberOfNotes != NumberOfNotes.Triad ||
                p.PreC.Omission != Omission.None) {
                return true;
            }
            if (p.NowC.ChordDegree != CD.I) {
                return false;
            }

            if (p.PrepreC == null) {
                return true;
            }

            if (p.PrepreC.ChordDegree != CD.I) {
                return false;
            }

            if (p.PrePartProgressionHigherInterval(Part.Bas) == 1 &&
                p.PartProgressionHigherInterval(Part.Bas) != 1) {
                p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIp61_a2_2));
                return false;
            }
            if (p.PrePartProgressionHigherInterval(Part.Bas) == -1 &&
                p.PartProgressionHigherInterval(Part.Bas) != -1) {
                p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIp61_a1_2));
                return false;
            }
            return true;
        }

        /// <returns>true: 終止和音に見られる連結だった。false:関係ない和音だった。</returns>
        private bool 終止和音に見られる連結(Progression p) {
            if (p.NowC.TerminationType != TerminationType.Perfect ||
                p.NowC.ChordDegree != CD.I ||
                p.NowC.Inversion != Inversion.根音 ||
                p.PreC.ChordDegree != CD.V ||
                !p.PreC.IsStandard ||
                p.PreC.Inversion != Inversion.根音) {
                return false;
            }

            if (p.PreC.SopInversion != Inversion.第5音) {
                return false;
            }


            if (p.Upper3CheckAndIfAllTrueSetVerdictOnce(part => {
                    Pitch prePitch = p.PreC.GetPitch(part);
                    switch (prePitch.Degree) {
                    case 2: return p.PartProgressionHigherInterval(part) == -1;
                    case 7: return p.PartProgressionHigherInterval(part) == 1;
                    case 5: return p.PartProgressionHigherInterval(part) == -2;
                    default: return false;
                    }
                }, new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIp108))) {
                return true;
            }

            return false;
        }

        private bool 標準連結toII7(Progression p) {
            if (p.PositionOfAChordChanged()) {
                // II巻補充課題7-1 配分転換。
                p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIIh7_01));
            }

        /* CheckIp100_63_1()でチェック済。
            // II巻p13
            var nowPart7List = p.NowC.GetPartListByInversion(Inversion.第7音);
            foreach (Part part in nowPart7List) {
                int pitchDiff = p.PartProgressionHigherPitch(part);
                if (part == Part.Bas && pitchDiff == 12) {
                    // 別巻p159 8度上行
                } else if ((part != Part.Bas && pitchDiff != 0) ||
                    (part == Part.Bas && pitchDiff != 0)) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIIp13_4, part));
                    return false;
                } 
            }
         */

            // II巻p16
            if (p.NowC.IsMajor() && !p.NowC.Is準固有和音 &&
                p.NowC.Inversion == Inversion.第5音) {
                int pitchDiff = p.PartProgressionHigherPitch(Part.Bas);
                if (pitchDiff != 0) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIIp16_5));
                    return false;
                }
            }

            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIp13_4));
            return true;
        }

        private bool 標準連結II7toX(Progression p)
        {
            if (!p.NowC.Is("[2転]I") &&
                p.NowC.ChordDegree != CD.V) {
                p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIIp12_3));
                return false;
            }

            if (!IsStandardPointOfAChordProgression(p)) {
                // 標準連結になりうる配分連結が行われている必要がある。
                return true;
            }

            // IIp12
            var part7List = p.PreC.GetPartListByInversion(Inversion.第7音);
            foreach (Part part in part7List) {
                int interval = p.PartProgressionHigherInterval(part);
                if (p.NowC.ChordDegree == CD.V && interval != -1) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIIp12_3_V, part));
                    return false;
                }
                if (p.NowC.ChordDegree == CD.I && interval != 0) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIIp12_3_I, part));
                    return false;
                }
            }

            var part根List = p.PreC.GetUpper3PartListByInversion(Inversion.根音);
            foreach (Part part in part根List) {
                int interval = p.PartProgressionHigherInterval(part);
                if (interval == -1 || interval == -2) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIIp12_3_II, part));
                    return false;
                }
            }

            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIp12_3));
            return true;
        }

        /// <summary>
        /// 先行和音→V度のV度 3和音 II巻p40 配置はII巻p38
        /// </summary>
        private bool 標準連結toV_V3(Progression p) {
            if (p.NowC.Inversion == Inversion.第5音) {
                p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidDoNotUseYet, "II巻p33,17 注1"));
                return true;
            }

            if (p.NowC.Inversion == Inversion.根音 && p.NowC.PositionOfAChord == PositionOfAChord.Oct) {
                p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIp38_19_1));
                return true;
            }
            if (p.PreC.Is("II7")) {
                // 第7音を保留できない。
                p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIIp36_18_2));
                return true;
            }

            if (!p.NowC.IsStandard) {
                // 標準外和音
                return true;
            }

            if (!p.PositionOfAChordIsTheSameOrTransfered()) {
                // 配分転換が起きている。
                return true;
            }

            if (p.NowC.Inversion == Inversion.第3音) {
                if (p.NowC.SopInversion == Inversion.根音 && p.NowC.PositionOfAChord == PositionOfAChord.Oct) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIp39_19_1));
                } else if (p.NowC.Is("開3省(根)")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIp39_19_1_2));
                }
                return true;
            }
            // IIp36 すべてOK

            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIp36_18_2));
            return true;
        }

        /// <summary>
        /// 先行和音→V度のV度 7の和音 II巻p43,20 3) 配置はII巻p41,20 1)
        /// </summary>
        private bool 標準連結toV_V7(Progression p) {
            if (p.NowC.Is("[2転]V_V7根省")) {
                if (0 < p.NowC.Upper3CountByInversion(Inversion.第5音)) {
                    // [2転]V_V7根省では上部構成音b)が用いられる。
                    p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RareIIp33_17_1));
                }
            } else if (p.NowC.PositionOfAChord == PositionOfAChord.Oct) {
                // IIp41 密か開が使われる。Octは見られない。
                p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIp41_20_1));
                return true;
            }

            /* 配分転換しても良いらしい II巻課題19-5
            if (!p.PositionOfAChordIsTheSameOrTransfered()) {
                // 配分転換が起きている。
                return true;
            }
            */

            if (p.PreC.Is("II7")) {
                // 第7音を保留。
                if (p.PartProgressionHigherPitch(p.PreC.GetPartListByInversion(Inversion.第7音)[0]) != 0) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIIp36_18_2));
                    return true;
                }
            }

            // IIp43 すべてOK

            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIp43_20_3));
            return true;
        }

        /// <summary>
        /// 先行和音→V度のV度 9の和音 II巻p54,22 4) 配置はII巻p51,22 1) 2)
        /// </summary>
        private bool 標準連結toV_V9(Progression p) {

            if (p.NowC.Omission == Omission.First) {
                // 根省和音 高音位は自由 II巻p52,22 2)
            } else {
                // IIp51,22 1) 最適配置は 密(9)か開(9)
                if (p.NowC.SopInversion == Inversion.第9音) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIp51_22_1));
                } else {
                    p.UpdateVerdict(new Verdict(VerdictValue.Okay, VerdictReason.OkayIIp51_22_1));
                }
            }

            // IIp54、というか、IIp36～37
            if (p.PreC.Is("II7")) {
                // 第7音を保留。
                if (p.PartProgressionHigherPitch(p.PreC.GetPartListByInversion(Inversion.第7音)[0]) != 0) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIIp36_18_2));
                    return true;
                }
            }

            // 並達9度
            if (CheckV_V9第9音高位の並達9度(p)) {
                return true;
            }

            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIp54_22_4));
            return true;
        }

        /// <summary>
        /// V_V諸和音→Xの連結 II巻p34
        /// </summary>
        private bool 標準連結V_VtoX(Progression p) {
            if (p.PreC.IsMajor() &&
                p.PreC.AlterationType == AlterationType.Lowered &&
                p.NowC.NumberOfNotes == NumberOfNotes.Ninth &&
                p.NowC.Is固有和音) {
                // II巻p61,23 2b)長調のV_V下変和音の後続和音が固有のV9やV9根省であるのは稀である
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RareIIp61_23_2b));
            }

            if (p.NowC.ChordDegree == CD.V && p.NowC.NumberOfNotes == NumberOfNotes.Triad) {
                // II巻p34,18 1) 1
                if (p.Upper3CheckAndIfAllTrueSetVerdictOnce(part => {
                    var prePitch = p.PreC.GetPitch(part);
                    switch (prePitch.Inversion) {
                    case Inversion.第3音: return p.PartProgressionHigherInterval(part) == 1;
                    case Inversion.第7音:
                        if (p.PreC.Is("[2転]V_V7")) {
                            // II巻p48,21 2 片方は2度下行、もう片方は2度上行
                            return p.PartProgressionAbsInterval(part) == 1;
                        } else if (p.PreC.Is("[2転]V_V9根省") && p.NowC.Inversion == Inversion.第3音) {
                            // II巻p53,22 3 2度上行する
                            return p.PartProgressionHigherInterval(part) == 1;
                        } else {
                            return p.PartProgressionHigherInterval(part) == -1;
                        }
                    case Inversion.第9音: return p.PartProgressionHigherInterval(part) == -1;
                    case Inversion.第5音:
                        if (p.PreC.AlterationType == AlterationType.Lowered) {
                            // II巻p60,23 2度下行する。
                            return p.PartProgressionHigherInterval(part) == -1;
                        }
                        return true;
                    default: return true;
                    }
                }, new Verdict(VerdictValue.Good, VerdictReason.GoodIIp34_18_1_1))) {
                } else {
                    // 限定進行音が限定進行していない。
                    p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.GoodIIp34_18_1_1));
                }
                return true;
            }
            if (p.NowC.ChordDegree == CD.V &&
                (p.NowC.NumberOfNotes == NumberOfNotes.Seventh ||
                 p.NowC.NumberOfNotes == NumberOfNotes.Ninth)) {
                // II巻p34,18 1) 2
                if (p.Upper3CheckAndIfAllTrueSetVerdictOnce(part => {
                    var prePitch = p.PreC.GetPitch(part);
                    switch (prePitch.Inversion) {
                    case Inversion.第3音:
                        {
                            int interval = p.PartProgressionHigherInterval(part);
                            return interval == 0 || interval == 1;
                        }
                    case Inversion.第7音:
                        if (p.PreC.Is("[2転]V_V7")) {
                            // II巻p48,21 2 1 片方は2度下行、もう片方は2度上行
                            return p.PartProgressionAbsInterval(part) == 1;
                        } else if (p.PreC.Is("[2転]V_V9根省") && p.NowC.Inversion == Inversion.第3音) {
                            // II巻p53,22 3 第7音は2度上行する。
                            return p.PartProgressionHigherInterval(part) == 1;
                        } else {
                            return p.PartProgressionHigherInterval(part) == -1;
                        }
                    case Inversion.第9音: return p.PartProgressionHigherInterval(part) == -1;
                    case Inversion.第5音:
                        if (p.PreC.AlterationType == AlterationType.Lowered) {
                            if (p.NowC.NumberOfNotes == NumberOfNotes.Seventh) {
                                // II巻p60,23 2度下行する。
                                return p.PartProgressionHigherInterval(part) == -1;
                            } else {
                                // V9の和音 II巻p61 保留する。
                                return p.PartProgressionHigherPitch(part) == 0;
                            }
                        }
                        return true;
                    default: return true;
                    }
                }, new Verdict(VerdictValue.Good, VerdictReason.GoodIIp34_18_1_2))) {
                } else {
                    // 限定進行音が限定進行していない。
                    p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.GoodIIp34_18_1_2));
                }
                return true;
            }
            if (p.NowC.ChordDegree == CD.I && p.NowC.Inversion == Inversion.第5音) {
                // II巻p34,18 1) 3
                if (p.Upper3CheckAndIfAllTrueSetVerdictOnce(part => {
                    var prePitch = p.PreC.GetPitch(part);
                    switch (prePitch.Inversion) {
                    case Inversion.第3音: return p.PartProgressionHigherInterval(part) == 1;
                    case Inversion.第7音:
                        if (p.PreC.Is("[2転]V_V7")) {
                            // II巻p48,21 2 2 片方は保留、もう片方は3度上行
                            int interval = p.PartProgressionHigherInterval(part);
                            return interval == 0 || interval == 2;
                        } else {
                            return p.PartProgressionHigherPitch(part) == 0;
                        }
                    case Inversion.第9音: return p.PartProgressionHigherInterval(part) == 0; //< ここをHigherPitch(0)にすると、大量の問題が起こる。
                    case Inversion.第5音:
                        if (p.PreC.AlterationType == AlterationType.Lowered) {
                            // II巻p60,23 2度下行する。
                            return p.PartProgressionHigherInterval(part) == -1;
                        }
                        return true;
                    default: return true;
                    }
                }, new Verdict(VerdictValue.Good, VerdictReason.GoodIIp34_18_1_3))) {
                } else {
                    // 限定進行音が限定進行していない。
                    p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.GoodIIp34_18_1_3));
                }
                return true;
            }
            return true;
        }

        /// <summary>
        /// V_V 3和音→Xの連結 II巻p39
        /// </summary>
        private bool 標準連結V_V3toX(Progression p) {
            標準連結V_VtoX(p);
            if (p.PreC.Inversion == Inversion.根音 && p.NowC.Is("[2転]I")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RareIIp39_19_2));
            }
            if (p.PreC.Inversion == Inversion.第3音 && p.NowC.Is("[2転]I")) {
                if (p.PreC.GetPitch(Part.Sop).Inversion != Inversion.根音 || p.PartProgressionHigherInterval(Part.Sop) != -1) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIp40_19_2));
                }
            }

            return true;
        }

        /// <summary>
        /// V_V 7の和音→Xの連結 II巻p41
        /// </summary>
        private bool 標準連結V_V7toX(Progression p) {
            if (p.PreC.Inversion == Inversion.根音 && p.NowC.Is("[2転]I")) {
                if (!p.PreC.IsStandard || p.PreC.GetUpper3PartListByInversion(Inversion.根音).Count==0) {
                    // II巻p42 上部構成音a)を用いる。
                    p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIp42_20));
                    return true;
                }
            }

            標準連結V_VtoX(p);
            return true;
        }
        /// <summary>
        /// V_V 9の和音→Xの連結 II巻p53,22 3)
        /// </summary>
        private bool 標準連結V_V9toX(Progression p) {

            標準連結V_VtoX(p);
            return true;
        }

        /// <summary>
        /// [2転]V_V 7根省の和音→Xの連結 II巻p48
        /// </summary>
        private bool 標準連結2転V_V7根省toX(Progression p) {
            if (p.PreC.Inversion != Inversion.第5音 ||
                p.PreC.Omission != Omission.First) {
                return false;
            }

            var inv3List = p.PreC.GetUpper3PartListByInversion(Inversion.第3音);
            if (inv3List.Count != 1) {
                return true;
            }
            var inv7List = p.PreC.GetUpper3PartListByInversion(Inversion.第7音);
            if (inv7List.Count != 2) {
                return true;
            }
            int prog0 = p.PartProgressionHigherInterval(inv7List[0]);
            int prog1 = p.PartProgressionHigherInterval(inv7List[1]);
            if (p.NowC.ChordDegree == CD.V) {
                if ((prog0 == 1 && prog1 == -1) ||
                    (prog0 == -1 && prog1 == 1)) {
                    // 第7音 2度上行、2度下行。OK
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIp48_21_2));
                }
                return true;
            }
            if (p.NowC.ChordDegree == CD.I && p.NowC.Inversion == Inversion.第5音) {
                if ((prog0 == 0 && prog1 == 2) ||
                    (prog0 == 2 && prog1 == 0)) {
                    // 第7音 保留、3度下行。OK
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIp48_21_2));
                }
            }

            return true;
        }

        /// <summary>
        /// 先行和音→IV7の和音の連結。II巻p70
        /// </summary>
        private bool 標準連結toIV7(Progression p) {
            /*
            if (p.PositionOfAChordChanged()) {
                // 配分転換
                p.UpdateVerdict(new Verdict(VerdictValue.NotSoGood, VerdictReason.InfoPositionOfAChordChanged));
                return true;
            }
            */

            // II巻p70,25 4)
            var nowPart7List = p.NowC.GetPartListByInversion(Inversion.第7音);
            foreach (Part part in nowPart7List) {
                int pitchDiff = p.PartProgressionHigherPitch(part);
                if ((part != Part.Bas && pitchDiff != 0) ||
                    (part == Part.Bas && (pitchDiff != 0 && pitchDiff != 12))) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIIp70_25_4, part));
                    return false;
                }
            }

            // II巻p70,25 5)
            if (p.NowC.Is準固有和音) {
                if (p.PreC.ChordDegree != CD.VI ||
                    !p.PreC.Is準固有和音) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIIp70_25_5));
                    return false;
                }
            }

            // II巻p70,26
            if (p.NowC.Inversion == Inversion.第5音) {
                // Basを先行する和音から保留する必要がある
                int pitchDiff = p.PartProgressionAbsPitch(Part.Bas);
                if (pitchDiff != 0 && pitchDiff != 12) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIIp70_26));
                    return false;
                }
                if (pitchDiff == 12) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RareIIp70_26));
                    return false;
                }
            }

            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIp70_25_4));
            return true;
        }

        /// <summary>
        /// IV7→後続和音の連結。IIp68～69
        /// </summary>
        private bool 標準連結IV7toX(Progression p) {
            /* 配分転換OK (IIp68,25 2)の下の例
            if (p.PositionOfAChordChanged()) {
                // 配分転換
                p.UpdateVerdict(new Verdict(VerdictValue.NotSoGood, VerdictReason.InfoPositionOfAChordChanged));
                return true;
            }
            */

            var part7List = p.PreC.GetPartListByInversion(Inversion.第7音);
            foreach (Part part in part7List) {
                int interval = p.PartProgressionHigherInterval(part);
                if (interval != -1) {
                    // 第7音が2度下行していない。
                    p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIIp68_25_2, part));
                    return true;
                }
            }

            // IIp68,25 2) IV7→D諸和音。
            if (p.NowC.Is("[2転]I") ||
                p.NowC.ChordDegree == CD.V) {
                p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIp68_25_2));
                return true;
            }

            // IIp68,25 3) IV7→S諸和音。
            if (p.PreC.Inversion == Inversion.根音 &&
                (p.NowC.Is("[1転]II") ||
                 p.NowC.Is("[1転]V_V") ||
                 p.NowC.Is("[1転]V_V7"))) {
                // 先行和音は開(7)が多い
                if (p.PreC.SopInversion == Inversion.第7音 &&
                    p.PreC.PositionOfAChord == PositionOfAChord.開) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIp68_25_3));
                } else {
                    p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RareIIp68_25_3));
                }
                return true;
            }

            p.UpdateVerdict(new Verdict(VerdictValue.NotSoGood, VerdictReason.NotSoGoodIIp68_25_3));
            return true;
        }

        /// <summary>
        /// 先行和音→ドリアIV7の和音の連結。II巻p76,28 3)
        /// </summary>
        private bool 標準連結toドリアIV7(Progression p) {
            // II巻p76,28 3) ほとんど何も書いてない!
            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIp76_28_3));
            return true;
        }

        /// <summary>
        /// ドリアIV7→後続和音の連結。IIp74～75
        /// </summary>
        private bool 標準連結ドリアIV7toX(Progression p) {
            // 配分転換OK だろう

            // IIp74,28 2) ドリアIV7→V諸和音にしか進まない。
            if (p.NowC.ChordDegree != CD.V) {
                p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIIp74_28_2_3));
                return true;
            }

            {   // 第3音は2度上行
                var part3List = p.PreC.GetPartListByInversion(Inversion.第3音);
                if (part3List.Count != 1) {
                    // 第3音は1個のはずである
                    return true;
                }
                int interval = p.PartProgressionHigherInterval(part3List[0]);
                if (interval == 0) {
                    if (p.NowC.ChordDegree == CD.V &&
                    p.NowC.NumberOfNotes == NumberOfNotes.Ninth &&
                    p.NowC.Omission == Omission.First) {
                        // 後続和音がV9根省和音の場合 増一度下行OK II巻p75,28 2)
                    } else {
                        p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIIp74_28_2_1, part3List[0]));
                        return true;
                    }
                } else if (interval != 1) {
                    // 第3音が2度上行していない。
                    p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIIp74_28_2_1, part3List[0]));
                    return true;
                }
            }

            {   // 第7音は2度下行
                var part7List = p.PreC.GetPartListByInversion(Inversion.第7音);
                if (part7List.Count != 1) {
                    // 第7音は1個のはずである
                    return true;
                }
                int interval = p.PartProgressionHigherInterval(part7List[0]);
                if (interval == 1) {
                    if (p.PreC.Is("[2転]開(根)") &&
                        p.NowC.Is("[2転]V7Oct根省(7)")) {
                        // 2度上行OK II巻p75,28 2)
                    } else {
                        p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIIp74_28_2_2, part7List[0]));
                        return true;
                    }
                } else if (interval != -1) {
                    // 第7音が2度下行していない。
                    p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIIp74_28_2_2, part7List[0]));
                    return true;
                }
            }

            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIp74_28_2_3));
            return true;
        }

        /// <summary>
        /// 先行和音→ナポリII(ナポリの六)の和音の連結。
        /// </summary>
        private bool 標準連結toナポリII(Progression p) {
            if (p.NowC.Is("Oct(5)")) {
                // 十分用いうる配置 別巻課題の実施 II巻課題34-3 注
                p.UpdateVerdict(new Verdict(VerdictValue.Okay, VerdictReason.IIk34_3));
                return true;
            }

            // あとは通常のIIと同等
            return false;
        }

        /// <summary>
        /// ナポリII→後続和音の連結。IIp76～78
        /// </summary>
        private bool 標準連結ナポリIItoX(Progression p) {

            // -II→D諸和音
            if (p.NowC.Is("[2転]I") ||
                p.NowC.ChordDegree == CD.V) {
                if (p.NowC.NumberOfNotes == NumberOfNotes.Ninth &&
                    p.NowC.Inversion != Inversion.第7音) {
                    // 不良
                } else {
                    // 良好。
                    // 上3声を下行させるが、後続和音が[3転]V9根省和音の場合は第5音VIは保留。
                    if (p.Upper3CheckAndIfAllTrueSetVerdictOnce(part => {
                        var interval = p.PartProgressionHigherInterval(part);
                        var pitch = p.PreC.GetPitch(part);
                        switch (pitch.Inversion) {
                        case Inversion.第5音:
                            if (p.NowC.NumberOfNotes == NumberOfNotes.Ninth) {
                                // 先行和音がOctの場合、下行でも可。
                                if (p.PreC.PositionOfAChord == PositionOfAChord.Oct) {
                                    return interval <= 0;
                                } else {
                                    return interval == 0;
                                }
                            }
                            return interval < 0;
                        default:
                            return interval < 0;
                        }
                    }, new Verdict(VerdictValue.Good, VerdictReason.IIp77_30_2))) {
                        return true;
                    }
                    // 不良
                }
            }

            // -II→S諸和音
            if (p.NowC.ChordDegree == CD.V_V &&
                p.NowC.Inversion == Inversion.第3音 &&
                p.NowC.Omission == Omission.First) {
                // 根音IIが2度下行
                var pre根音List = p.PreC.GetUpper3PartListByInversion(Inversion.根音);
                if (pre根音List.Count == 1) {
                    if (p.PartProgressionHigherInterval(pre根音List[0]) == -1) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.IIp77_30_3));
                        return true;
                    }
                    // 不良
                }
            }

            p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIIp77_30));
            return true;
        }

        /// <summary>
        /// 先行和音→IV付加6和音の連結。IIp90,39 3)
        /// </summary>
        private bool 標準連結toIV付加6(Progression p) {
            // 第5音を予備する
            var nowInv5 = p.NowC.GetPartListByInversion(Inversion.第5音);
            if (nowInv5.Count != 1) {
                System.Diagnostics.Debug.Assert(false);
                return true;
            }

            if (p.PartProgressionHigherPitch(nowInv5[0]) == 0) {
                p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIp90_39_3));
            } else {
                p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIp90_39_3, nowInv5[0]));
            }
            return true;
        }

        /// <summary>
        /// IV付加6の和音→Xの連結。IIp90,39 2)
        /// </summary>
        private bool 標準連結IV付加6toX(Progression p) {
            if (p.NowC.ChordDegree != CD.I ||
                    p.NowC.Inversion != Inversion.根音) {
                // 後続和音は[基]Iである。
                p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoIIp90_39_2_1));
            }

            if (p.NowC.Is("[1転]I")) {
                // 後続和音は[1転]I
                // 第5音を保留する。
                // 第6音を4度上行する。
                if (!p.Upper3CheckAndIfAllTrueSetVerdictOnce(part => {
                    var prePitch = p.PreC.GetPitch(part);
                    switch (prePitch.Inversion) {
                    case Inversion.第5音: return p.PartProgressionHigherPitch(part) == 0;
                    case Inversion.第6音: return p.PartProgressionHigherInterval(part) == 3;
                    default: return true;
                    }
                }, new Verdict(VerdictValue.Good, VerdictReason.IIIp223_78))) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.IIIp223_78));
                }
                return true;
            }

            // 第5音を保留する。
            // 第6音を2度上行する。
            if (!p.Upper3CheckAndIfAllTrueSetVerdictOnce(part => {
                var prePitch = p.PreC.GetPitch(part);
                switch (prePitch.Inversion) {
                case Inversion.第5音: return p.PartProgressionHigherPitch(part) == 0;
                case Inversion.第6音: return p.PartProgressionHigherInterval(part) == 1;
                default: return true;
                }
            }, new Verdict(VerdictValue.Good, VerdictReason.GoodIIp90_39_2_2))) {
                p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIp90_39_2_3));
            }
            return true;
        }

        /// <summary>
        /// 先行和音→IV付加4-6の和音。別巻p165 II巻補充課題12-10
        /// </summary>
        private bool 標準連結toIV付加46(Progression p) {
            // 特にルールはないが、先行和音がT和音である必要がある
            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodtoIV46));
            return true;
        }
        /// <summary>
        /// IV付加4-6の和音→Xの連結。IIp92,42 2)
        /// </summary>
        private bool 標準連結IV付加46toX(Progression p) {
            // 後続和音は[基]Iである。
            if (p.NowC.ChordDegree != CD.I ||
                p.NowC.Inversion != Inversion.根音) {
                p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIp93_42_2_1));
                return true;
            }

            p.DebugProgression();

            // 第4音を2度上行する。
            // 第6音を2度上行する。
            // 第3音を2度下行する。
            if (!p.Upper3CheckAndIfAllTrueSetVerdictOnce(part => {
                var prePitch = p.PreC.GetPitch(part);
                switch (prePitch.Inversion) {
                case Inversion.第4音: return p.PartProgressionHigherInterval(part) == 1;
                case Inversion.第6音: return p.PartProgressionHigherInterval(part) == 1;
                case Inversion.第3音: return p.PartProgressionHigherInterval(part) == -1;
                default: return true;
                }
            }, new Verdict(VerdictValue.Good, VerdictReason.GoodIIp93_42_2_2))) {
                p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIp93_42_2_3));
            }
            return true;
        }

        /// <summary>
        /// IIIp212,76 3)
        /// </summary>
        private bool 標準連結IIItoX(Progression p) {

            // III triad はIIIp212,76 3)
            if (p.NowC.ChordDegree == CD.IV) {
                if (p.PreC.Inversion != Inversion.根音 ||
                        !p.NowC.Is("[基]IV")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIIp212_76_3_1));
                    return true;
                }

                // 上3声が全て下行する。
                if (p.Upper3CheckAndIfAnyTrueSetVerdictOnce(
                        part => { return p.PartProgressionHigherInterval(part) >= 0; },
                        new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIIp212_76_3_2))) {
                    return true;
                } else if (!p.PreC.Is("密(5)") || !p.NowC.Is("密")) {
                    // 先行和音が密(5)で、上3声が全て下行するのが最適。
                    p.UpdateVerdict(new Verdict(VerdictValue.Okay, VerdictReason.OkayIIIp212_76_3_1));
                }
            }

            if (p.NowC.ChordDegree == CD.II) {
                if (p.PreC.Inversion != Inversion.根音 ||
                        !p.NowC.Is("[1転]II")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIIp212_76_3_1));
                    return true;
                }

                if (p.Upper3CheckAndIfAnyTrueSetVerdictOnce(
                        part => { return p.PartProgressionHigherInterval(part) >= 0; },
                        new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIIp212_76_3_2))) {
                    return true;
                } else if (!p.NowC.Is("密") || (!p.PreC.Is("密(根)") && !p.PreC.Is("密(3)"))) {
                    // 先行和音が密(根)か密(3)で、上3声が全て下行するのが最適。
                    p.UpdateVerdict(new Verdict(VerdictValue.Okay, VerdictReason.OkayIIIp212_76_3_1));
                }
            }

            if (p.PreC.Inversion == Inversion.第3音) {
                if (!p.NowC.Is("[基]VI")) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIIp212_76_3_3));
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 第7音の限定進行 IIIp218 77 3)
        /// </summary>
        /// <returns>限定進行を守っていたらtrue。限定進行していないときはfalse</returns>
        private bool Check第7音の限定進行(Progression p) {
            var part7List = p.PreC.GetPartListByInversion(Inversion.第7音);
            foreach (Part part in part7List) {
                int interval = p.PartProgressionHigherInterval(part);
                if (interval != -1) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIIIp218_77_3_1, part));
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 後続和音の第7音の予備のチェック
        /// </summary>
        /// <returns>false: 不良和音。</returns>
        private bool Check後続和音の第7音の予備(Progression p) {
            var part7List = p.NowC.GetPartListByInversion(Inversion.第7音);
            foreach (Part part in part7List) {
                int interval = p.PartProgressionHigherInterval(part);
                if (interval != 0) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.WrongIIIp218_77_3_2, part));
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 2転7の和音のBas2度下行のチェック
        /// </summary>
        /// <returns>false: 不良和音。</returns>
        private bool Check2転7の和音のBas2度下行(Progression p) {
            if (p.PreC.Inversion == Inversion.第5音) {
                // [2転]7の和音 Basは2度下行する
                if (p.PartProgressionHigherInterval(Part.Bas) != -1) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIIp219_77_3_7));
                }
            }
            return true;
        }

        /// <summary>
        /// 7の和音の並達8度のチェック
        /// </summary>
        /// <returns>false: 不良和音。</returns>
        private bool Check7の和音の並達8度(Progression p) {
            bool 根音3度下行 = false;
            Part 根音3度下行part = Part.Invalid;
            bool 第7音2度下行 = false;
            Part 第7音2度下行part = Part.Invalid;

            foreach (Part part in p.PreC.GetPartListByInversion(Inversion.根音)) {
                if (p.PartProgressionHigherInterval(part) == -2) {
                    根音3度下行 = true;
                    根音3度下行part = part;
                }
            }
            foreach (Part part in p.PreC.GetPartListByInversion(Inversion.第7音)) {
                if (p.PartProgressionHigherInterval(part) == -1) {
                    第7音2度下行 = true;
                    第7音2度下行part = part;
                }
            }

            if (根音3度下行 && 第7音2度下行) {
                p.UpdateVerdict(new Verdict(VerdictValue.Prohibited, VerdictReason.ProhibitedIIIp219_77_3_8, 根音3度下行part, 第7音2度下行part));
                return false;
            }

            return true;
        }

        /// <summary>
        /// IIIp219 77 4)
        /// </summary>
        /// <returns>true: これ以上良否を調査する必要がない</returns>
        private bool 標準連結I7III7VI7VII7toX(Progression p) {
            if (!Check第7音の限定進行(p)) {
                return true;
            }
            if (!Check2転7の和音のBas2度下行(p)) {
                return true;
            }
            if (!Check7の和音の並達8度(p)) {
                return true;
            }

            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIIp220_77_4_1));
            return false;
        }

        /// <summary>
        /// 2転和音の低音4度の予備のチェック。予備は完全1度進行である必要がある。増1度進行も不可。
        /// </summary>
        /// <returns>false: 不良和音。</returns>
        private bool Check低音4度の予備(Progression p) {
            if (p.NowC.Inversion == Inversion.第5音 &&
                    p.NowC.Upper3CountByInversion(Inversion.根音) == 1) {
                Part part = p.NowC.GetPartListByInversion(Inversion.根音)[0];
                if (p.NowC.TwoPartIntervalType(Part.Bas, part) == IntervalType.PerfectFourth &&
                        p.PartProgressionHigherPitch(Part.Bas) != 0 &&
                        p.PartProgressionHigherPitch(part) != 0) {
                    // 低音4度が予備されていない。
                    p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIIp219_77_3_6, part));
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// V3和音またはV7の和音の第3音保留(するのが良い)
        /// </summary>
        /// <returns>false: 不良和音。</returns>
        private bool CheckVの和音の第3音保留(Progression p) {
            if ((p.PreC.Is("V") || p.PreC.Is("V7")) &&
                (p.NowC.Is("I7") || p.NowC.Is("III7"))) {
                var leadingNoteList = p.PreC.Get導音リスト();
                System.Diagnostics.Debug.Assert(leadingNoteList.Count == 1);
                var part = leadingNoteList[0].part;
                if (p.PartProgressionHigherInterval(part) > 1) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIIp219_77_3_10, part));
                    return true;
                }
            }

            return true;
        }

        /// <summary>
        /// 短調のV→III7の和音 (Vは固有のVIIを持つVの和音が良い)
        /// </summary>
        /// <returns>false: 不良和音。</returns>
        private bool CheckVtoIII7(Progression p) {
            if ((p.PreC.IsMinor() && p.NowC.IsMinor()) &&
                    (p.PreC.Is("V") || p.PreC.Is("V7")) &&
                    p.NowC.Is("III7")) {
                if (p.PreC.Has固有VII) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIIp221_77_5_2));
                } else {
                    p.UpdateVerdict(new Verdict(VerdictValue.Rare, VerdictReason.RareIIIp221_77_5_2));
                    // 不良ではないので継続して良否を調査する(trueを戻す)
                }
            }

            return true;
        }

        private bool 標準連結XtoI7III7VI7VII7(Progression p) {
            if (!Check後続和音の第7音の予備(p)) {
                return true;
            }

            if (!Check低音4度の予備(p)) {
                return true;
            }

            if (!CheckVの和音の第3音保留(p)) {
                return true;
            }

            if (!CheckVtoIII7(p)) {
                return true;
            }

            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIIp220_77_4_1));
            return false;
        }

        /// <summary>
        /// IIIp214,76 4)
        /// </summary>
        private bool 標準連結VIItoX(Progression p) {
            if (!p.NowC.Is("[基]III")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIIp214_76_4_1));
                return true;
            }
            return false;
        }

        /// <summary>
        /// IIIp214,76 5)
        /// </summary>
        private bool 標準連結1転VItoX(Progression p) {
            if (!p.NowC.Is("[基]II") && !p.NowC.Is("[3転]II7")) {
                p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIIp214_76_5_1));
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// IIIp225,79 3 b)
        /// </summary>
        private bool 標準連結VRaisedtoX(Progression p) {
            p.AllPartCheckAndIfTrueSetVerdict(
                part => {
                    switch (p.PreC.GetPitch(part).Inversion) {
                    case Inversion.第3音:
                    case Inversion.第5音:
                        return !p.IsPartProgression上行限定進行(part);
                    case Inversion.第7音:
                    case Inversion.第9音:
                        return !p.IsPartProgression下行限定進行(part);
                    default:
                        return false;
                    }
                }, new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIIp225_79_3_b_1));
            return false;
        }

        /// <summary>
        /// IIIp226,79 5)
        /// </summary>
        private bool 標準連結IVRaisedtoX(Progression p) {
            var part6 = p.PreC.GetPartListByInversion(Inversion.第6音);
            foreach (var part in part6) {
                if (!p.IsPartProgression上行限定進行(part)) {
                    p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIIp226_79_5_1, part));
                }
            }
            return false;
        }

        /// <summary>
        /// II巻p98～101 転調進行
        /// </summary>
        private bool 転調進行(Progression p) {
            if (Get増1度の音程関係のペア(p, true).Count > 0) {
                // II巻p98 a)
                // AugmentedUnisonのチェックは既に行われている。
                p.UpdateVerdict(new Verdict(VerdictValue.Info, VerdictReason.InfoIIp98_47_a));
            } else {
                // 共通音が含まれる場合 II巻p98 b)
                // 共通音が含まれない場合 II巻p99 c)
                標準連結Type type;
                if (Is標準連結(p, 標準連結Rule.BasSopContrary, out type)) {
                    if (type == 標準連結Type.Upper3CommonExists) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIp98_47_b));
                    } else {
                        p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIp98_47_c));
                    }
                } else {
                    if (type == 標準連結Type.Upper3CommonExists) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIp98_47_b));
                    } else {
                        p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIp98_47_c));
                    }
                }
            }

            { // II巻p99,47 e) 離脱和音の限定進行音の取扱い。
                var 限定進行音リスト = p.PreC.Get限定進行音リスト();
                foreach (限定進行音Info lpi in 限定進行音リスト) {
                    int interval = p.PartProgressionHigherInterval(lpi.part);
                    int pitch = p.PartProgressionHigherPitch(lpi.part);

                    switch (lpi.type) {
                    case 限定進行音Type.下行限定進行音:
                        if (interval == -1 || interval == 0) {
                            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIp99_47_e, lpi.part));
                        } else {
                            p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIp99_47_e, lpi.part));
                        }
                        break;
                    case 限定進行音Type.上行限定進行音:
                        if (interval == 1 || interval == 0) {
                            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIp99_47_e, lpi.part));
                        } else {
                            p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIp99_47_e, lpi.part));
                        }
                        break;
                    case 限定進行音Type.導音:
                        if (interval == 1 || interval == 0) {
                            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIp99_47_e, lpi.part));
                        } else if (0 < interval) {
                            // II巻p100,47 e) 導音の跳躍上行。@todo 本来はエンハーモニック転調の場合だけOKらしい。とりあえず全部OKとする。
                            p.UpdateVerdict(new Verdict(VerdictValue.Acceptable, VerdictReason.AcceptableIIp100_47_e, lpi.part));
                        } else {
                            // 導音が下行している。常に駄目である。
                            p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIp99_47_e, lpi.part));
                        }
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false);
                        break;
                    }
                }
            }

            { // II巻p100,47 f) 転入和音が7の和音の場合。
                if (p.NowC.NumberOfNotes == NumberOfNotes.Seventh &&
                    p.NowC.ChordDegree != CD.V && p.NowC.ChordDegree != CD.V_V) {
                    // 転入和音の第7音を予備する。公理D1
                    var part7List = p.NowC.GetPartListByInversion(Inversion.第7音);
                    foreach (Part part in part7List) {
                        if (0 != p.PartProgressionHigherPitch(part)) {
                            p.UpdateVerdict(new Verdict(VerdictValue.Avoid, VerdictReason.AvoidIIp100_47_f, part));
                        } else {
                            p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.GoodIIp100_47_f, part));
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 固有VIIを持つ短調のV7の第7音の予備
        /// </summary>
        /// <returns>false: 不良和音。</returns>
        private bool Check固有VIIを持つV7の第7音(Progression p) {
            if (p.NowC.Has固有VII && p.NowC.NumberOfNotes == NumberOfNotes.Seventh) {
                var part7 = p.NowC.GetPartListByInversion(Inversion.第7音);
                if (1 < part7.Count) {
                    foreach (var part in part7) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.IIIp222_77_5_3, part));
                    }
                    return false;
                }

                {
                    var part = part7[0];
                    if (p.PartProgressionAbsPitch(part7[0]) == 0) {
                        p.UpdateVerdict(new Verdict(VerdictValue.Good, VerdictReason.IIIp222_77_5_3, part));
                    } else {
                        p.UpdateVerdict(new Verdict(VerdictValue.Wrong, VerdictReason.IIIp222_77_5_3, part));
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 標準連結かどうか。
        /// </summary>
        private void Check標準連結(Progression p) {
            if (CheckSpecialProgression(p)) {
                return;
            }

            int failCount = 0;
            failCount += CheckIp100_63_1(p);
            if (0 < failCount) {
                return;
            }

            if (標準連結Ip73_43(p)) {
                return;
            }

            if (p.PreC.Is("[2転]I")) {
                if (!定型ルール2転ItoV(p)) { return; }
            }

            if (!定型ルール2転VtoI(p)) { return; }

            if (p.PreC.Is("III")) {
                if (標準連結IIItoX(p)) { return; }
            }

            if (p.NowC.Has固有VII) {
                if (!Check固有VIIを持つV7の第7音(p)) { return; }
            }

            if (p.PreC.NumberOfNotes == NumberOfNotes.Seventh) {
                switch (p.PreC.ChordDegree) {
                case CD.I:
                case CD.III:
                case CD.VI:
                case CD.VII:
                    if (標準連結I7III7VI7VII7toX(p)) { return; }
                    break;
                default:
                    break;
                }
            }
            if (p.NowC.NumberOfNotes == NumberOfNotes.Seventh) {
                switch (p.NowC.ChordDegree) {
                case CD.I:
                case CD.III:
                case CD.VI:
                case CD.VII:
                    if (標準連結XtoI7III7VI7VII7(p)) { return; }
                    break;
                default:
                    break;
                }
            }

            if (p.PreC.Is("VII")) {
                if (標準連結VIItoX(p)) { return; }
            }
            if (p.PreC.Is("[1転]VI")) {
                if (標準連結1転VItoX(p)) { return; }
            }
            
            if (p.PreC.AlterationType == AlterationType.Raised) {
                switch (p.PreC.ChordDegree) {
                case CD.V:
                case CD.V_V:
                    標準連結VRaisedtoX(p);
                    break;
                case CD.IV:
                    標準連結IVRaisedtoX(p);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
            }

            if (p.NowC.AddedTone == AddedToneType.Six) {
                標準連結toIV付加6(p);
            }
            if (p.PreC.AddedTone == AddedToneType.Six) {
                標準連結IV付加6toX(p);
            }
            if (p.NowC.AddedTone == AddedToneType.SixFour) {
                標準連結toIV付加46(p);
            }
            if (p.PreC.AddedTone == AddedToneType.SixFour) {
                標準連結IV付加46toX(p);
            }

            if (p.PreC.ChordDegree == CD.II &&
                p.PreC.AlterationType == AlterationType.Naples) {
                if (標準連結ナポリIItoX(p)) { return; }
            }
            if (p.NowC.ChordDegree == CD.II &&
                p.NowC.AlterationType == AlterationType.Naples) {
                if (標準連結toナポリII(p)) { return; }
            }

            if (p.NowC.Is("IV7")) {
                if (p.NowC.AlterationType == AlterationType.Dorian) {
                    if (標準連結toドリアIV7(p)) { return; }
                } else {
                    if (標準連結toIV7(p)) { return; }
                }
            }
            if (p.PreC.Is("IV7")) {
                if (p.PreC.AlterationType == AlterationType.Dorian) {
                    if (標準連結ドリアIV7toX(p)) { return; }
                } else {
                    if (標準連結IV7toX(p)) { return; }
                }
            }

            // 転調進行
            if (p.Is転調進行()) {
                if (転調進行(p)) { return; }
            }

            if (p.NowC.Is("V_V9")) {
                // X→V_V9の和音
                if (標準連結toV_V9(p)) { return; }
            }
            if (p.NowC.Is("V_V7")) {
                // X→V_V 7の和音
                if (標準連結toV_V7(p)) { return; }
            }
            if (p.PreC.Is("V_V9")) {
                if (標準連結V_V9toX(p)) { return; }
            }
            if (p.PreC.Is("V_V7")) {
                // V_V 7の和音→X
                if (標準連結2転V_V7根省toX(p)) { return; }
                if (標準連結V_V7toX(p)) { return; }
            }

            if (p.NowC.Is("V_V")) {
                // X→V_V 3和音
                if (標準連結toV_V3(p)) { return; }
            }
            if (p.PreC.Is("V_V")) {
                // V_V 3和音→X
                if (標準連結V_V3toX(p)) { return; }
            }

            if (p.NowC.Is("II7")) {
                if (標準連結toII7(p)) { return; }
            }
            if (p.PreC.Is("II7")) {
                if (標準連結II7toX(p)) { return; }
            }

            if (p.NowC.Is("V9")) {
                if (標準連結toV9(p)) { return; }
            }

            if (p.PreC.Is("V9")) {
                if (標準連結V9toI(p)) { return; }
            }

            if (p.NowC.Is("V7")) {
                CheckIp83_49_3_2(p);
                if (標準連結to1or3転V7根省(p)) { return; }
                if (標準連結to2転V7根省(p)) { return; }
                if (標準連結6toV7(p)) { return; }
                if (標準連結124toV7(p)) { return; }
                return;
            }

            if (p.PreC.Is("V7")) {
                if (標準連結1or3転V7根省to1(p)) { return; }
                if (標準連結2転V7根省toX(p)) { return; }
                if (標準連結V7toI(p)) { return; }
                if (標準連結V7to6(p)) { return; }
                return;
            }

            if (p.ChordsAre("[基]", "[基]")) {
                if (終止和音に見られる連結(p)) { return; }
                if (標準連結5to6(p)) { return; }
                if (標準連結5to6to5(p)) { return; }
                if (標準連結2to5(p)) { return; }
                if (標準連結4to2(p)) { return; }
                標準連結基to基(p);
                return;
            }
            if (p.ChordsAre("[基]", "[1転]")) {
                if (標準連結基Xto1転2(p)) { return; }
                if (標準連結基Xto1転145(p)) { return; }
                return;
            }
            if (p.ChordsAre("[1転]", "[基]")) {
                if (標準連結1転2to基(p)) { return; }
                if (標準連結1転to基(p)) { return; }
                return;
            }
            if (p.ChordsAre("[1転]", "[1転]")) {
                if (標準連結1転2to1転(p)) { return; }
                if (標準連結1転145to1転(p)) { return; }
                return;
            }
            if (p.ChordsAre("[基]", "[2転]")) {
                if (標準連結基6to2転1(p)) { return; }
                if (標準連結基1to2転5(p)) { return; }
                if (標準連結基1to2転4(p)) { return; }
                if (標準連結基2to2転1(p)) { return; }
                if (標準連結4to2転1(p)) { return; }
                return;
            }
            if (p.ChordsAre("[2転]", "[基]")) {
                if (標準連結2転4to基1(p)) { return; }
                if (標準連結2転1to基5(p)) { return; }
                if (標準連結2転5to基1(p)) { return; }
                return;
            }
            if (p.ChordsAre("[2転]", "[1転]")) {
                if (標準連結2転5to1転1(p)) { return; }
                return;
            }
            if (p.ChordsAre("[1転]", "[2転]")) {
                if (標準連結1転1to2転5(p)) { return; }
                if (標準連結1転2to2転1(p)) { return; }
                if (標準連結4to2転1(p)) { return; }
                return;
            }
        }
    }
}
