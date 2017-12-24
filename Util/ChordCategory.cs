using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FourPartHarmony2
{
    enum ChordMode
    {
        Major,
        HarmonicMinor,
        NaturalMinor
    }

    /// <summary>
    /// 和音の種類情報。度数、形体等から“和音の種類”と教科書で扱われているページ数を得る。
    /// </summary>
    class ChordCategoryInfo
    {
        public CD chordDegree;
        public ChordMode mode;
        public NumberOfNotes nofn;
        public Omission omission;
        public AlterationType alteration;
        public AddedToneType addedTone;

        public ChordConstructionType construction;
        public FunctionType function;

        public ChordCategoryInfo(
                CD chordDegree,
                ChordMode mode,
                NumberOfNotes nofn,
                Omission omission,
                AlterationType alteration,
                AddedToneType addedTone,
                ChordConstructionType construction,
                FunctionType function) {
            this.chordDegree = chordDegree;
            this.mode = mode;
            this.nofn = nofn;
            this.omission = omission;
            this.alteration = alteration;
            this.addedTone = addedTone;
            this.construction = construction;
            this.function = function;
        }
    }

    /// <summary>
    /// 度数、形体等から“和音の種類”と教科書で扱われているページ数を得る。
    /// </summary>
    class ChordCategory
    {
        /// <summary>
        /// 3和音 長調
        /// </summary>
        private static readonly List<ChordCategoryInfo> cclMajorTriad;

        /// <summary>
        /// 3和音 短調(Vは和声的短旋法、他は自然短旋法)
        /// </summary>
        private static readonly List<ChordCategoryInfo> cclMinorTriad;

        /// <summary>
        /// 7の和音 長調
        /// </summary>
        private static readonly List<ChordCategoryInfo> cclMajorSeventh;

        /// <summary>
        /// 7の和音 短調(Vは和声的短旋法、他は自然短旋法)
        /// </summary>
        private static readonly List<ChordCategoryInfo> cclMinorSeventh;

        private static readonly ChordCategoryInfo ccMajorV7Omitted;
        private static readonly ChordCategoryInfo ccMinorV7Omitted;

        private static readonly ChordCategoryInfo ccMajorV9;
        private static readonly ChordCategoryInfo ccMinorV9;

        private static readonly ChordCategoryInfo ccMajorV9Omitted;
        private static readonly ChordCategoryInfo ccMinorV9Omitted;

        private static readonly ChordCategoryInfo ccMinorNapolitanII;
        private static readonly ChordCategoryInfo ccMinorNapolitanII7;

        private static readonly ChordCategoryInfo ccMinorDorianIV;
        private static readonly ChordCategoryInfo ccMinorDorianIV7;

        private static readonly ChordCategoryInfo ccMajorIV付加6;
        private static readonly ChordCategoryInfo ccMinorIV付加6;

        private static readonly ChordCategoryInfo ccMajorIV付加46;
        private static readonly ChordCategoryInfo ccMinorIV付加46;

        private static readonly ChordCategoryInfo ccNaturalMinorV;
        private static readonly ChordCategoryInfo ccNaturalMinorV7;

        /*
        public static string
        GetChordOverviewText(ChordType ct) {
            switch (ct.alteration) {
            case AlterationType.Lowered:
                switch (ct.chordDegree) {
                case CD.V_V: return Properties.Resources.DescLoweredVV;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    return string.Empty;
                }
            case AlterationType.Raised:
                switch (ct.chordDegree) {
                case CD.IV: return Properties.Resources.DescRaisedIV6;
                case CD.V:  return Properties.Resources.DescRaisedV;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    return string.Empty;
                }
            case AlterationType.Dorian:
                System.Diagnostics.Debug.Assert(ct.chordDegree == CD.IV);
                return Properties.Resources.DescDorianIV7;
            case AlterationType.Naples:
                System.Diagnostics.Debug.Assert(ct.chordDegree == CD.II);
                return Properties.Resources.DescNapolitanII;
            default:
                break;
            }

            switch (ct.addedTone) {
            case AddedToneType.Six:     return Properties.Resources.DescIV6;
            case AddedToneType.SixFour: return Properties.Resources.DescIV46;
            default:
                break;
            }

            if (ct.has固有VII) {
                // 短調である
                System.Diagnostics.Debug.Assert(ct.chordDegree == CD.V);
                System.Diagnostics.Debug.Assert(ct.omission == Omission.None);
                return Properties.Resources.DescNaturalMinorV;
            }
            if (ct.omission == Omission.First) {
                switch (ct.chordDegree) {
                case CD.V_V: return Properties.Resources.DescVV;
                case CD.V:
                    switch (ct.numberOfNotes) {
                    case NumberOfNotes.Seventh: return Properties.Resources.DescV7Omitted;
                    case NumberOfNotes.Ninth:   return Properties.Resources.DescV9Omitted;
                    default:
                        break;
                    }
                    break;
                default:
                    break;
                }
                System.Diagnostics.Debug.Assert(false);
                return string.Empty;
            }
            if (ct.chordDegree == CD.V_V) {
                return Properties.Resources.DescVV;
            }
            if (ct.is準固有) {
                return Properties.Resources.DescBorrowedII;
            }
            if (ct.numberOfNotes == NumberOfNotes.Ninth) {
                return Properties.Resources.DescV9;
            }

            if (ct.numberOfNotes == NumberOfNotes.Seventh) {
                switch (ct.chordDegree) {
                case CD.I:
                case CD.III:
                case CD.VI:
                case CD.VII:
                    return Properties.Resources.DescI7;
                case CD.II: return Properties.Resources.DescII7;
                case CD.IV: return Properties.Resources.DescIV7;
                case CD.V: return Properties.Resources.DescV7;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    return string.Empty;
                }
            }

            // 基 3和音
            System.Diagnostics.Debug.Assert(ct.numberOfNotes == NumberOfNotes.Triad);
            switch (ct.chordDegree) {
            case CD.III:
            case CD.VII:
                return Properties.Resources.DescIII;
            default:
                break;
            }
            if (ct.bassInversion == Inversion.第5音) {
                return Properties.Resources.Desc2転I;
            }
            if (ct.bassInversion == Inversion.第3音) {
                switch (ct.chordDegree) {
                case CD.VI: return Properties.Resources.Desc1転VI;
                default:    return Properties.Resources.Desc1転I;
                }
            }
            if (ct.Is内部調は短調()) {
                return Properties.Resources.Desc基IMinor;
            } else {
                return Properties.Resources.Desc基I;
            }
        }
    */

        public static ChordConstructionType
        GetConstructionTypeFromChordType(ChordType ct) {
            // 和音の音度。
            var cd = ct.chordDegree;
            if (cd == CD.V_V) {
                // V_VとVはChordConstructionの判定方法が同じ
                cd = CD.V;
            }

            switch (ct.alteration) {
            case AlterationType.Lowered:
            case AlterationType.Raised:
                return ChordConstructionType.変位和音;
            case AlterationType.Naples:
                System.Diagnostics.Debug.Assert(cd == CD.II);
                switch (ct.numberOfNotes) {
                case NumberOfNotes.Triad:   return ccMinorNapolitanII.construction;
                case NumberOfNotes.Seventh: return ccMinorNapolitanII7.construction;
                default: System.Diagnostics.Debug.Assert(false);
                    break;
                }
                break;
            case AlterationType.Dorian:
                System.Diagnostics.Debug.Assert(cd == CD.IV);
                switch (ct.numberOfNotes) {
                case NumberOfNotes.Triad:   return ccMinorDorianIV.construction;
                case NumberOfNotes.Seventh: return ccMinorDorianIV7.construction;
                default: System.Diagnostics.Debug.Assert(false);
                    break;
                }
                break;
            default:
                break;
            }

            switch (ct.addedTone) {
            case AddedToneType.Six:
                if (ct.Is内部調は長調() && !ct.is準固有) {
                    return ccMajorIV付加6.construction;
                } else {
                    return ccMinorIV付加6.construction;
                }
            case AddedToneType.SixFour:
                if (ct.Is内部調は長調() && !ct.is準固有) {
                    return ccMajorIV付加46.construction;
                } else {
                    return ccMinorIV付加46.construction;
                }
            default:
                break;
            }

            if (ct.has固有VII) {
                /* 短調である */
                System.Diagnostics.Debug.Assert(cd == CD.V);
                System.Diagnostics.Debug.Assert(ct.omission == Omission.None);
                switch (ct.numberOfNotes) {
                case NumberOfNotes.Triad:
                    return ccNaturalMinorV.construction;
                case NumberOfNotes.Seventh:
                    return ccNaturalMinorV7.construction;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
            }

            if (ct.omission == Omission.First) {
                System.Diagnostics.Debug.Assert(cd == CD.V);
                switch (ct.numberOfNotes) {
                case NumberOfNotes.Seventh:
                    if (ct.Is内部調は長調() && !ct.is準固有) {
                        return ccMajorV7Omitted.construction;
                    } else {
                        return ccMinorV7Omitted.construction;
                    }
                case NumberOfNotes.Ninth:
                    if (ct.Is内部調は長調() && !ct.is準固有) {
                        return ccMajorV9Omitted.construction;
                    } else {
                        return ccMinorV9Omitted.construction;
                    }
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
            }

            switch (ct.numberOfNotes) {
            case NumberOfNotes.Triad:
                if (ct.Is内部調は長調() && !ct.is準固有) {
                    return cclMajorTriad[(int)cd].construction;
                } else {
                    return cclMinorTriad[(int)cd].construction;
                }
            case NumberOfNotes.Seventh:
                if (ct.Is内部調は長調() && !ct.is準固有) {
                    return cclMajorSeventh[(int)cd].construction;
                } else {
                    return cclMinorSeventh[(int)cd].construction;
                }
            case NumberOfNotes.Ninth:
                System.Diagnostics.Debug.Assert(cd == CD.V);
                if (ct.Is内部調は長調() && !ct.is準固有) {
                    return ccMajorV9.construction;
                } else {
                    return ccMinorV9.construction;
                }
            default:
                System.Diagnostics.Debug.Assert(false);
                break;
            }

            // ここに来たらいけない
            return ChordConstructionType.変位和音;
        }

        static ChordCategory() {
            cclMajorTriad = new List<ChordCategoryInfo>();
            cclMajorTriad.Add(new ChordCategoryInfo(CD.I, ChordMode.Major, NumberOfNotes.Triad,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.長3和音, FunctionType.Tonic));
            cclMajorTriad.Add(new ChordCategoryInfo(CD.II, ChordMode.Major, NumberOfNotes.Triad,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.短3和音, FunctionType.Subdominant));
            cclMajorTriad.Add(new ChordCategoryInfo(CD.III, ChordMode.Major, NumberOfNotes.Triad,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.短3和音, FunctionType.Tonic));
            cclMajorTriad.Add(new ChordCategoryInfo(CD.IV, ChordMode.Major, NumberOfNotes.Triad,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.長3和音, FunctionType.Subdominant));
            cclMajorTriad.Add(new ChordCategoryInfo(CD.V, ChordMode.Major, NumberOfNotes.Triad,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.長3和音, FunctionType.Dominant));
            cclMajorTriad.Add(new ChordCategoryInfo(CD.VI, ChordMode.Major, NumberOfNotes.Triad,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.短3和音, FunctionType.Tonic));
            cclMajorTriad.Add(new ChordCategoryInfo(CD.VII, ChordMode.Major, NumberOfNotes.Triad,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.減3和音, FunctionType.Dominant));

            cclMinorTriad = new List<ChordCategoryInfo>();
            cclMinorTriad.Add(new ChordCategoryInfo(CD.I, ChordMode.NaturalMinor, NumberOfNotes.Triad,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.短3和音, FunctionType.Tonic));
            cclMinorTriad.Add(new ChordCategoryInfo(CD.II, ChordMode.NaturalMinor, NumberOfNotes.Triad,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.減3和音, FunctionType.Subdominant));
            cclMinorTriad.Add(new ChordCategoryInfo(CD.III, ChordMode.NaturalMinor, NumberOfNotes.Triad,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.長3和音, FunctionType.Tonic));
            cclMinorTriad.Add(new ChordCategoryInfo(CD.IV, ChordMode.NaturalMinor, NumberOfNotes.Triad,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.短3和音, FunctionType.Subdominant));
            cclMinorTriad.Add(new ChordCategoryInfo(CD.V, ChordMode.HarmonicMinor, NumberOfNotes.Triad,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.長3和音, FunctionType.Dominant));
            cclMinorTriad.Add(new ChordCategoryInfo(CD.VI, ChordMode.NaturalMinor, NumberOfNotes.Triad,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.長3和音, FunctionType.Tonic));
            cclMinorTriad.Add(new ChordCategoryInfo(CD.VII, ChordMode.NaturalMinor, NumberOfNotes.Triad,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.長3和音, FunctionType.Dominant));

            cclMajorSeventh = new List<ChordCategoryInfo>();
            cclMajorSeventh.Add(new ChordCategoryInfo(CD.I, ChordMode.Major, NumberOfNotes.Seventh,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.長7の和音, FunctionType.Tonic));
            cclMajorSeventh.Add(new ChordCategoryInfo(CD.II, ChordMode.Major, NumberOfNotes.Seventh,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.短7の和音, FunctionType.Subdominant));
            cclMajorSeventh.Add(new ChordCategoryInfo(CD.III, ChordMode.Major, NumberOfNotes.Seventh,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.短7の和音, FunctionType.Tonic));
            cclMajorSeventh.Add(new ChordCategoryInfo(CD.IV, ChordMode.Major, NumberOfNotes.Seventh,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.長7の和音, FunctionType.Subdominant));
            cclMajorSeventh.Add(new ChordCategoryInfo(CD.V, ChordMode.Major, NumberOfNotes.Seventh,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.属7の和音, FunctionType.Dominant));
            cclMajorSeventh.Add(new ChordCategoryInfo(CD.VI, ChordMode.Major, NumberOfNotes.Seventh,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.短7の和音, FunctionType.Tonic));
            cclMajorSeventh.Add(new ChordCategoryInfo(CD.VII, ChordMode.Major, NumberOfNotes.Seventh,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.減57の和音, FunctionType.Dominant));

            cclMinorSeventh = new List<ChordCategoryInfo>();
            cclMinorSeventh.Add(new ChordCategoryInfo(CD.I, ChordMode.NaturalMinor, NumberOfNotes.Seventh,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.短7の和音, FunctionType.Tonic));
            cclMinorSeventh.Add(new ChordCategoryInfo(CD.II, ChordMode.NaturalMinor, NumberOfNotes.Seventh,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.減57の和音, FunctionType.Subdominant));
            cclMinorSeventh.Add(new ChordCategoryInfo(CD.III, ChordMode.NaturalMinor, NumberOfNotes.Seventh,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.長7の和音, FunctionType.Tonic));
            cclMinorSeventh.Add(new ChordCategoryInfo(CD.IV, ChordMode.NaturalMinor, NumberOfNotes.Seventh,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.短7の和音, FunctionType.Subdominant));
            cclMinorSeventh.Add(new ChordCategoryInfo(CD.V, ChordMode.HarmonicMinor, NumberOfNotes.Seventh,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.属7の和音, FunctionType.Dominant));
            cclMinorSeventh.Add(new ChordCategoryInfo(CD.VI, ChordMode.NaturalMinor, NumberOfNotes.Seventh,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.長7の和音, FunctionType.Tonic));
            cclMinorSeventh.Add(new ChordCategoryInfo(CD.VII, ChordMode.NaturalMinor, NumberOfNotes.Seventh,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.属7の和音, FunctionType.Dominant));

            ccMajorV7Omitted = new ChordCategoryInfo(CD.V, ChordMode.Major, NumberOfNotes.Seventh,
                Omission.First, AlterationType.None, AddedToneType.None,
                ChordConstructionType.減3和音, FunctionType.Dominant);
            ccMinorV7Omitted = new ChordCategoryInfo(CD.V, ChordMode.HarmonicMinor, NumberOfNotes.Seventh,
                Omission.First, AlterationType.None, AddedToneType.None,
                ChordConstructionType.減3和音, FunctionType.Dominant);

            ccMajorV9 = new ChordCategoryInfo(CD.V, ChordMode.Major, NumberOfNotes.Ninth,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.長9の和音, FunctionType.Dominant);
            ccMinorV9 = new ChordCategoryInfo(CD.V, ChordMode.HarmonicMinor, NumberOfNotes.Ninth,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.短9の和音, FunctionType.Dominant);

            ccMajorV9Omitted = new ChordCategoryInfo(CD.V, ChordMode.Major, NumberOfNotes.Ninth,
                Omission.First, AlterationType.None, AddedToneType.None,
                ChordConstructionType.減57の和音, FunctionType.Dominant);
            ccMinorV9Omitted = new ChordCategoryInfo(CD.V, ChordMode.HarmonicMinor, NumberOfNotes.Ninth,
                Omission.First, AlterationType.None, AddedToneType.None,
                ChordConstructionType.減7の和音, FunctionType.Dominant);

            ccMinorNapolitanII = new ChordCategoryInfo(CD.II, ChordMode.NaturalMinor, NumberOfNotes.Triad,
                Omission.None, AlterationType.Naples, AddedToneType.None,
                ChordConstructionType.長3和音, FunctionType.Subdominant);
            ccMinorNapolitanII7 = new ChordCategoryInfo(CD.II, ChordMode.NaturalMinor, NumberOfNotes.Seventh,
                Omission.None, AlterationType.Naples, AddedToneType.None,
                ChordConstructionType.長7の和音, FunctionType.Subdominant);

            ccMinorDorianIV = new ChordCategoryInfo(CD.IV, ChordMode.NaturalMinor, NumberOfNotes.Triad,
                Omission.None, AlterationType.Dorian, AddedToneType.None,
                ChordConstructionType.長3和音, FunctionType.Subdominant);

            ccMinorDorianIV7 = new ChordCategoryInfo(CD.IV, ChordMode.NaturalMinor, NumberOfNotes.Seventh,
                Omission.None, AlterationType.Dorian, AddedToneType.None,
                ChordConstructionType.属7の和音, FunctionType.Subdominant);

            ccMajorIV付加6 = new ChordCategoryInfo(CD.IV, ChordMode.Major, NumberOfNotes.Seventh,
                Omission.None, AlterationType.None, AddedToneType.Six,
                ChordConstructionType.短7の和音, FunctionType.Subdominant);
            ccMinorIV付加6 = new ChordCategoryInfo(CD.IV, ChordMode.NaturalMinor, NumberOfNotes.Seventh,
                Omission.None, AlterationType.None, AddedToneType.Six,
                ChordConstructionType.減57の和音, FunctionType.Subdominant);
            ccMajorIV付加46 = new ChordCategoryInfo(CD.IV, ChordMode.Major, NumberOfNotes.Ninth,
                Omission.None, AlterationType.None, AddedToneType.SixFour,
                ChordConstructionType.減57の和音, FunctionType.Subdominant);
            ccMinorIV付加46 = new ChordCategoryInfo(CD.IV, ChordMode.NaturalMinor, NumberOfNotes.Ninth,
                Omission.None, AlterationType.None, AddedToneType.SixFour,
                ChordConstructionType.減7の和音, FunctionType.Subdominant);

            ccNaturalMinorV = new ChordCategoryInfo(CD.V, ChordMode.NaturalMinor, NumberOfNotes.Triad,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.短3和音, FunctionType.Dominant);

            ccNaturalMinorV7 = new ChordCategoryInfo(CD.V, ChordMode.NaturalMinor, NumberOfNotes.Seventh,
                Omission.None, AlterationType.None, AddedToneType.None,
                ChordConstructionType.短7の和音, FunctionType.Dominant);
        }
    }
}
