using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FourPartHarmony2
{
    public class ChordListGenerator
    {
        private readonly ChordType ct;
        public ChordType ChordType { get { return ct; } }

        private List<Chord> chordList;
        private readonly List<LnDeg> chordLnDeg123467; ///< I, II, III, IV, VI, VII ドレミファラシ
        private readonly List<LnDeg> chordLnDeg5;      ///< V、VII、II, IV, VI

        public bool CompleteDeg { get; set; }

        public ChordListGenerator(ChordType chordType) {
            CompleteDeg = false;

            this.ct = chordType;
            chordList = new List<Chord>();

            chordLnDeg123467 = new List<LnDeg>();
            chordLnDeg5 = new List<LnDeg>();

            MusicKeyInfo mki = new MusicKeyInfo(ct.musicKey, ct.keyRelation);
            for (int i = 0; i < 7; ++i) {
                chordLnDeg123467.Add(new LnDeg(mki.GetLnFromDegree(i), i + 1));
            }

            if (ct.is準固有) {
                // 準固有和音
                chordLnDeg123467[2] = new LnDeg(chordLnDeg123467[2].LetterName.Flat(), 3);
                chordLnDeg123467[5] = new LnDeg(chordLnDeg123467[5].LetterName.Flat(), 6);
            }

            chordLnDeg5.Add(chordLnDeg123467[4]); // 根音  V
            chordLnDeg5.Add(chordLnDeg123467[6]); // 第3音 VII
            chordLnDeg5.Add(chordLnDeg123467[1]); // 第5音 II
            chordLnDeg5.Add(chordLnDeg123467[3]); // 第7音 IV
            chordLnDeg5.Add(chordLnDeg123467[5]); // 第9音 VI

            if (ct.Is内部調は短調()) {
                if (ct.has固有VII) {
                    // 短調で短調固有のVIIを持つ。
                } else {
                    // 短調固有のVIIではなく和声的VIIを持つ。第3音(VIIの音)を半音上げる。
                    chordLnDeg5[1] = new LnDeg(chordLnDeg123467[6].LetterName.Sharp(), 7);
                }
            }

            if (ct.addedTone == AddedToneType.SixFour &&
                ct.chordDegree == CD.IV &&
                ct.Is内部調は短調()) {
                // 短調のIV付加4－6の和音の場合 付加第4音(VIIの音)を半音上げる。
                chordLnDeg123467[6] = new LnDeg(chordLnDeg123467[6].LetterName.Sharp(), 7);
            }

            if (ct.alteration == AlterationType.Lowered) {
                switch (ct.chordDegree) {
                case CD.V:
                    // V_V 下方変位 第5音(II)がフラットになる
                    chordLnDeg5[2] = new LnDeg(chordLnDeg123467[1].LetterName.Flat(), 2);
                    CompleteDeg = true;
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
            }

            if (ct.alteration == AlterationType.Naples) {
                switch (ct.chordDegree) {
                case CD.II:
                    // II ナポリの和音 根音(II)がフラットになる
                    chordLnDeg123467[1] = new LnDeg(chordLnDeg123467[1].LetterName.Flat(), 2);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
            }

            if (ct.alteration == AlterationType.Dorian) {
                switch (ct.chordDegree) {
                case CD.IV:
                    // IV ドリアの和音 第3音(VI)がシャープになる
                    chordLnDeg123467[5] = new LnDeg(chordLnDeg123467[5].LetterName.Sharp(), 6);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
            }

            if (ct.alteration == AlterationType.Raised) {
                switch (ct.chordDegree) {
                case CD.V:
                case CD.V_V:
                    // V上方変位 第5音がシャープになる(III巻p.224)
                    chordLnDeg5[2] = new LnDeg(chordLnDeg123467[1].LetterName.Sharp(), 2);
                    CompleteDeg = true;
                    break;
                case CD.IV:
                    // IV+6, IV+46の和音 第6音(II)がシャープになる(III巻p.226)
                    chordLnDeg123467[1] = new LnDeg(chordLnDeg123467[1].LetterName.Sharp(), 2);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
            }
        }

        public bool Is(ChordType rhs)
        {
            if (ct.Equals(rhs)) {
                return true;
            }
            return false;
        }

        public int Count
        {
            get { return chordList.Count; }
        }

        public Chord At(int idx)
        {
            return chordList[idx];
        }

        /// <summary>
        /// 和音の構成音を戻す。そのままバスの音名を戻す関数としても使える。
        /// </summary>
        /// <param name="bassInversion">None: 根音、First: 第3音、Second: 第5音、Third: 第7音、Fourth: 第9音</param>
        /// <returns></returns>
        public LnDegInversion GetChordLnDegInversion(Inversion inversion)
        {
            System.Diagnostics.Debug.Assert(0<= (int)inversion);

            switch (ct.chordDegree) {
            case CD.I:
            case CD.II:
            case CD.III:
            case CD.IV:
            case CD.VI:
            case CD.VII:
                {
                    int idx = (int)ct.chordDegree + (int)inversion * 2;
                    idx %= 7;
                    return new LnDegInversion(chordLnDeg123467[idx], inversion);
                }
            case CD.V:
                return new LnDegInversion(chordLnDeg5[(int)inversion], inversion);
            default:
                break;
            }

            System.Diagnostics.Debug.Assert(false);
            return LnDegInversion.INVALID;
        }

        /// <summary>
        ///  上3声ten、alt、sopの構成音列を戻す。
        /// </summary>
        /// <param name="bas"></param>
        /// <param name="variation">-1を渡すと、構成音列のバリエーションの数を戻すだけ</param>
        /// <param name="standard"></param>
        /// <returns>構成音列の構成音列のバリエーションの数</returns>
        private int ObtainUpper3LnDeg(LnDegInversion bas, int variation,
                out List<LnDegInversion> upper3, out bool standard)
        {
            int count = 0; //< 構成音列のバリエーションの数
            standard = false;
            if (variation < 0) {
                upper3 = null;
            } else {
                upper3 = new List<LnDegInversion>();
            }

            // bas, ten, alt, sopの音を取得。

            // 付加6の和音、付加4-6の和音の上3声
            switch (ct.addedTone) {
            case AddedToneType.Six:
                if (bas.Inversion != Inversion.根音) {
                    // [基]のみである(II巻p90,39)
                    return 0;
                }
                count = 1; // 1通り
                switch (variation) {
                case -1:
                    return count;
                case 0: // ミソラ
                    upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                    upper3.Add(GetChordLnDegInversion(Inversion.第5音));
                    upper3.Add(GetChordLnDegInversion(Inversion.第6音));
                    standard = true;
                    return count;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
                break;
            case AddedToneType.SixFour:
                if (bas.Inversion != Inversion.根音) {
                    // [基]のみである(II巻p92,42)
                    return 0;
                }
                count = 1; // 1通り
                switch (variation) {
                case -1:
                    return count;
                case 0: // ミファラ
                    upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                    upper3.Add(GetChordLnDegInversion(Inversion.第4音));
                    upper3.Add(GetChordLnDegInversion(Inversion.第6音));
                    standard = true;
                    return count;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
                break;
            default:
                break;
            }

            switch (ct.numberOfNotes) {
            case NumberOfNotes.Triad:
                if (Omission.First == ct.omission) {
                    // 3和音に根音省略形体はない。
                    return 0;
                }

                switch (bas.Inversion) {
                case Inversion.根音:
                    count = 5; // 5通り
                    if (CompleteDeg) {
                        // 第5音省略不可なので3通り。
                        count = 3;
                    }

                    switch (variation) {
                    case -1:
                        return count;
                    case 0: // ドミソ
                        upper3.Add(GetChordLnDegInversion(Inversion.根音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第5音));
                        standard = true;
                        break;
                    case 1: // ミソソ
                        upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第5音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第5音));
                        break;
                    case 2: // ミミソ
                        upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第5音));
                        break;
                    case 3: // ドドミ
                        upper3.Add(GetChordLnDegInversion(Inversion.根音));
                        upper3.Add(GetChordLnDegInversion(Inversion.根音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                        break;
                    case 4: // ドミミ
                        upper3.Add(GetChordLnDegInversion(Inversion.根音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false);
                        break;
                    }
                    break;
                case Inversion.第3音:
                    count = 5; // 5通り
                    if (CompleteDeg) {
                        // 第5音省略不可なので3通り。
                        count = 3;
                    }

                    switch (variation) {
                    case -1:
                        return count;
                    case 0: // ドドソ
                        upper3.Add(GetChordLnDegInversion(Inversion.根音));
                        upper3.Add(GetChordLnDegInversion(Inversion.根音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第5音));
                        if (ct.chordDegree != CD.II) {
                            standard = true;
                        }
                        break;
                    case 1: // ドソソ
                        upper3.Add(GetChordLnDegInversion(Inversion.根音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第5音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第5音));
                        if (ct.chordDegree != CD.II) {
                            standard = true;
                        }
                        break;
                    case 2: // ドミソ
                        upper3.Add(GetChordLnDegInversion(Inversion.根音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第5音));
                        if (ct.chordDegree == CD.II) {
                            standard = true;
                        }
                        break;
                    case 3: // ドドミ
                        upper3.Add(GetChordLnDegInversion(Inversion.根音));
                        upper3.Add(GetChordLnDegInversion(Inversion.根音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                        break;
                    case 4: // ドドド
                        upper3.Add(GetChordLnDegInversion(Inversion.根音));
                        upper3.Add(GetChordLnDegInversion(Inversion.根音));
                        upper3.Add(GetChordLnDegInversion(Inversion.根音));
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false);
                        break;
                    }
                    break;
                case Inversion.第5音:
                    count = 3; // 3通り
                    switch (variation) {
                    case -1:
                        return count;
                    case 0: // ドミソ
                        upper3.Add(GetChordLnDegInversion(Inversion.根音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第5音));
                        standard = true;
                        break;
                    case 1: // ドドミ
                        upper3.Add(GetChordLnDegInversion(Inversion.根音));
                        upper3.Add(GetChordLnDegInversion(Inversion.根音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                        break;
                    case 2: // ドミミ
                        upper3.Add(GetChordLnDegInversion(Inversion.根音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false);
                        break;
                    }
                    break;
                }
                break;
            case NumberOfNotes.Seventh:
                switch (bas.Inversion) {
                case Inversion.根音:
                    if (Omission.First == ct.omission) {
                        return 0; // ない
                    } else {
                        count = 3; // 3通り
                        if (CompleteDeg) {
                            count = 1;
                        }

                        switch (variation) {
                        case -1:
                            return count;
                        case 0: // ミソシ
                            upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第5音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第7音));
                            standard = true;
                            break;
                        case 1: // ドミシ
                            upper3.Add(GetChordLnDegInversion(Inversion.根音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第7音));
                            standard = true;
                            break;
                        case 2: // ミミシ
                            upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第7音));
                            break;
                        default:
                            System.Diagnostics.Debug.Assert(false);
                            break;
                        }
                    }
                    break;
                case Inversion.第3音:
                    if (Omission.First == ct.omission) {
                        count = 1; // 1通り Ip126
                        switch (variation) {
                        case -1:
                            return count;
                        case 0: // ソソシ
                            upper3.Add(GetChordLnDegInversion(Inversion.第5音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第5音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第7音));
                            standard = true;
                            break;
                        default:
                            System.Diagnostics.Debug.Assert(false);
                            break;
                        }
                    } else {
                        count = 3; // 3通り
                        if (CompleteDeg) {
                            count = 1;
                        }

                        switch (variation) {
                        case -1:
                            return count;
                        case 0: // ドソシ
                            upper3.Add(GetChordLnDegInversion(Inversion.根音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第5音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第7音));
                            standard = true;
                            break;
                        case 1: // ドドシ
                            upper3.Add(GetChordLnDegInversion(Inversion.根音));
                            upper3.Add(GetChordLnDegInversion(Inversion.根音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第7音));
                            break;
                        case 2: // ドミシ
                            upper3.Add(GetChordLnDegInversion(Inversion.根音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第7音));
                            break;
                        default:
                            System.Diagnostics.Debug.Assert(false);
                            break;
                        }
                    }
                    break;
                case Inversion.第5音:
                    if (Omission.First == ct.omission) {
                        count = 2; // 2通り
                        if (CompleteDeg) {
                            count = 1;
                        }
                        switch (variation) {
                        case -1:
                            return count;
                        case 0: // ミソシ
                            upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第5音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第7音));
                            standard = true;
                            break;
                        case 1: // シミシ
                            upper3.Add(GetChordLnDegInversion(Inversion.第7音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第7音));
                            standard = true;
                            break;
                        default:
                            System.Diagnostics.Debug.Assert(false);
                            break;
                        }
                    } else {
                        count = 1; // 1通り
                        switch (variation) {
                        case -1:
                            return count;
                        case 0: // ドミシ
                            upper3.Add(GetChordLnDegInversion(Inversion.根音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第7音));
                            standard = true;
                            break;
                        default:
                            System.Diagnostics.Debug.Assert(false);
                            break;
                        }
                    }
                    break;
                case Inversion.第7音:
                    if (Omission.First == ct.omission) {
                        count = 1; // 1通り Ip126
                        switch (variation) {
                        case -1:
                            return count;
                        case 0: // ミソソ
                            upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第5音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第5音));
                            standard = true;
                            break;
                        default:
                            System.Diagnostics.Debug.Assert(false);
                            break;
                        }
                    } else {
                        count = 3; // 3通り
                        if (CompleteDeg) {
                            count = 1;
                        }
                        switch (variation) {
                        case -1:
                            return count;
                        case 0: // ドミソ
                            upper3.Add(GetChordLnDegInversion(Inversion.根音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第5音));
                            standard = true;
                            break;
                        case 1: // ドドミ
                            upper3.Add(GetChordLnDegInversion(Inversion.根音));
                            upper3.Add(GetChordLnDegInversion(Inversion.根音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                            break;
                        case 2: // ドミミ
                            upper3.Add(GetChordLnDegInversion(Inversion.根音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                            upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                            break;
                        default:
                            System.Diagnostics.Debug.Assert(false);
                            break;
                        }
                    }
                    break;
                }
                break;
            case NumberOfNotes.Ninth:
                count = 1; // 1通り
                if (-1 == variation) {
                    return count;
                }
                switch (bas.Inversion) {
                case Inversion.根音:
                    if (Omission.First == ct.omission) {
                        return 0;
                    }
                    // ミシレ
                    upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                    upper3.Add(GetChordLnDegInversion(Inversion.第7音));
                    upper3.Add(GetChordLnDegInversion(Inversion.第9音));
                    standard = true;
                    break;
                case Inversion.第3音:
                    if (Omission.First == ct.omission) {
                        // ソシレ
                        upper3.Add(GetChordLnDegInversion(Inversion.第5音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第7音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第9音));
                        standard = true;
                    } else {
                        // ドシレ
                        upper3.Add(GetChordLnDegInversion(Inversion.根音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第7音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第9音));
                        standard = true;
                    }
                    break;
                case Inversion.第5音:
                    if (Omission.First == ct.omission) {
                        // ミシレ
                        upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第7音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第9音));
                        standard = true;
                    } else {
                        return 0;
                    }
                    break;
                case Inversion.第7音:
                    if (Omission.First == ct.omission) {
                        // ミソレ
                        upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第5音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第9音));
                        standard = true;
                    } else {
                        // ドミレ
                        upper3.Add(GetChordLnDegInversion(Inversion.根音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第3音));
                        upper3.Add(GetChordLnDegInversion(Inversion.第9音));
                        standard = true;
                    }
                    break;
                }
                break;
            default:
                System.Diagnostics.Debug.Assert(false);
                break;
            }
            if (-1 == variation) {
                // この組み合わせの和音はない。
                return 0;
            }

            /*
            // 第5音省略の場合、Inversion.Secondの音を除外する。
            switch (ct.omission) {
            case Omission.Fifth:
                while (upper3.Remove(GetChordLnDegInversion(Inversion.第5音))) { }
                break;
            }
            */

            return count;
        }

        private List<Pitch> GenerateBassPositions()
        {
            List<Pitch> bassPositions = new List<Pitch>();

            for (int oct = Pitch.BasLowestOctave(); oct <= Pitch.BasHighestOctave(); ++oct) {
                Pitch pitch = new Pitch(GetChordLnDegInversion(ct.bassInversion), oct);
                if (pitch.IsWithinTheRange(Part.Bas)) {
                    bassPositions.Add(pitch);
                }
            }
            return bassPositions;
        }

        private void GenerateUpper3Step(bool standard, Pitch bas, List<LnDegInversion> upper3, int step, PositionOfAChord positionOfAChord)
        {
            for (int i=0; i < upper3.Count; ++i) {
                for (int tenOct = Pitch.TenLowestOctave(); tenOct <= Pitch.TenHighestOctave(); ++tenOct) {
                    Pitch ten = new Pitch(upper3[i], tenOct);
                    if (!ten.IsWithinTheRange(Part.Ten)) {
                        continue;
                    }

                    for (int altOct = Pitch.AltLowestOctave(); altOct <= Pitch.AltHighestOctave(); ++altOct) {
                        Pitch alt = new Pitch(upper3[(i + step) % upper3.Count], altOct);
                        if (!alt.IsWithinTheRange(Part.Alt))
                        {
                            continue;
                        }

                        for (int sopOct = Pitch.SopLowestOctave(); sopOct <= Pitch.SopHighestOctave(); ++sopOct) {
                            Pitch sop = new Pitch(upper3[(i + step*2) % upper3.Count], sopOct);
                            if (!sop.IsWithinTheRange(Part.Sop))
                            {
                                continue;
                            }

                            Chord chord = new Chord(ct, standard, bas, ten, alt, sop);
                            if (chord.Verdict != VerdictValue.Delist
                                && chord.IsWithinTheRange()
                                && chord.Upper3PositionIsCorrect()
                                && !chordList.Contains(chord, new ChordEqualityComparer())) {
                                //System.Console.WriteLine("{0} {1} {2}", ten.LetterName.LN, alt.LetterName.LN, sop.LetterName.LN);
                                chordList.Add(chord);
                            }
                        }
                    }
                }
            }
        }

        private void GenerateUpper3(bool standard, Pitch bas, List<LnDegInversion> upper3, PositionOfAChord positionOfAChord)
        {
            GenerateUpper3Step(standard, bas, upper3, 1, positionOfAChord);
            GenerateUpper3Step(standard, bas, upper3, 2, positionOfAChord);
        }

        /// <summary>
        /// V以外の3和音の配置一覧を生成。
        /// I巻p118
        /// </summary>
        private void EnumerateAllChords()
        {
            List<Pitch> bassPositions = GenerateBassPositions();

            foreach (Pitch bas in bassPositions) {
                // それぞれのバスの上に密集配置、開離配置を試みる。
                bool standard;
                List<LnDegInversion> upper3;
                int count = ObtainUpper3LnDeg(bas.LnDegInversion, -1, out upper3, out standard);
                
                for (int i=0; i < count; ++i) {
                    ObtainUpper3LnDeg(bas.LnDegInversion, i, out upper3, out standard);
                    GenerateUpper3(standard, bas, upper3, ct.positionOfAChord);
                }
            }
            //System.Console.WriteLine("");
        }

        /// <summary>
        /// 和音の配置の可能性を列挙し、それぞれの和音の妥当性をチェックする。
        /// </summary>
        public List<Chord> Generate()
        {
            EnumerateAllChords();
            return chordList;
        }

        public List<Chord> GetChordList()
        {
            return chordList;
        }
    }

    public class ChordListFactory
    {
        private ChordListFactory() { }

        public static List<Chord> ObtainChordList(ChordType chordType)
        {
            var tmpChordType = chordType;
            if (chordType.chordDegree == CD.V_V) {
                // ドッペルドミナント等の前処理。
                // まず、この和音の内部調のV調を一時的に主調とする。(tmpChordType.musicKeyに入れる。)
                // 内部調をI度調とする。
                tmpChordType.chordDegree = CD.V;
                tmpChordType.musicKey = new MusicKeyInfo(tmpChordType.musicKey, tmpChordType.keyRelation).RelatedKey(CD.V);
                tmpChordType.keyRelation = KeyRelation.I調;
            } else if (chordType.keyRelation != KeyRelation.I調) {
                // 内部調の前処理。
                // この和音の内部調を一時的に主調とする。(tmpChordType.musicKeyに入れる。)
                // 内部調をI度調とする。
                tmpChordType.musicKey = new MusicKeyInfo(tmpChordType.musicKey, tmpChordType.keyRelation).InternalKey;
                tmpChordType.keyRelation = KeyRelation.I調;
            }

            var clg = new ChordListGenerator(tmpChordType);
            clg.Generate();
            var chordList = clg.GetChordList();
            
            if (chordType.chordDegree == CD.V_V) {
                // ドッペルドミナント等の後処理。
                // 元の主調、内部調に戻す。
                // 生成されたVの和音の機能をサブドミナントとし、音符の音度を付け直す。
                foreach (var c in chordList) {
                    c.ChordType = chordType;
                    c.UpdateFunction(FunctionType.Subdominant);
                    // 順番重要。調を主調に戻してから音度を付け直す。
                    c.RenumberNoteDegrees();
                }
            } else if (chordType.keyRelation != KeyRelation.I調) {
                // 内部調の後処理。
                // 元の主調、内部調に戻す。
                // 音符の音度を付け直す。
                foreach (var c in chordList) {
                    c.ChordType = chordType;
                    // 順番重要。調を主調に戻してから音度を付け直す。
                    c.RenumberNoteDegrees();
                }
            }

            // chordListが完成した。和音の説明をつける。
            foreach (var c in chordList) {
                c.ReevaluateChordVerdictAndChordFunction();
            }

            return chordList;
        }
    }
}
