using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FourPartHarmony2
{
    public enum VerdictReason {
        BestProgressionIp54, //< 最適な連結(I巻p54)。
        BestIp64_37, //< 最適な連結。[1転]II密(根)→[2転]I I巻p64,37
        BestV9,      //< 最適なV9配置。 (I巻p86,53)
        Best1転V9,   //< 最適な[1転]V9根省の配置。(I巻p89,57)
        Best2転3転V9,   //< 最適な[2転][3転]V9根省の配置。(I巻p89,57)
        BestIp81_49_a, //< 最適な配置。[2転]V7根省 上部構成音 a) I巻p81,49
        BestIp83_49_b, //< 最適な配置。[2転]V7根省 上部構成音 b) I巻p83,49
        BestStartChord, //< 最適な開始和音。
        BestTermination, //< I巻p108,3(4)、終止のSopは主音が最適
        OkayIIIp212_76_3_1, //< 最適な配置ではない配置。[基]III密(5)→IV
        RecommendIp91,  //< 推奨される。(I巻p91,57)
        StandardIp30, //< 標準連結。共通音がある場合 I巻p30
        StandardIp32, //< 標準連結。共通音がない場合 I巻p32
        StandardIp34_1, //< 標準連結。V→VI I巻p34
        StandardIp34_2, //< 標準連結。II→V I巻p34
        StandardIp34_3, //< 標準連結。IV→II I巻p34
        StandardIp36, //< 標準連結。VI→V I巻p36, I巻p106
        StandardIp51, //< 標準連結。[1転]□⇔[基]□ 共通音のある場合 I巻p51
        StandardIp52, //< 標準連結。[1転]□⇔[基]□ 共通音のない場合 I巻p52
        StandardIp53_1, //< 標準連結。[1転]□→[1転]□ 共通音のある場合 I巻p53
        StandardIp53_2, //< 標準連結。[1転]□→[1転]□ 共通音のない場合 I巻p53
        StandardIp54_1_1, //< 標準連結。[1転]II→[1転]□ 共通音のある場合 I巻p54
        StandardIp54_1_2, //< 標準連結。[1転]II→[1転]□ 共通音のない場合 I巻p54
        StandardIp54_2, //< 標準連結。[基]□→[1転]II I巻p54, I巻p109
        GoodIp54_2_2, //< [基]□→[1転]IIOct(根) I巻p54には書いてないがよく現れる(I巻p54)(I巻p109,6)
        StandardIp54_31, //< 標準連結。[1転]II→[基]□ I巻p54
        StandardIp61_a1, //< 標準連結。[基]I→[2転]V I巻p61～62
        StandardIp61_a1_2, //< 標準連結。[2転]V→[基]I I巻p61～62
        StandardIp61_a2, //< 標準連結。[1転]I→[2転]V I巻p61～62
        StandardIp61_a2_2, //< 標準連結。[2転]V→[基]I I巻p61～62
        StandardIp61_b, //< 標準連結。[基]I→[2転]IV I巻p61～62
        StandardIp61_b_2, //< 標準連結。[2転]IV→[基]I I巻p61～62
        StandardIp61_b_2_2, //< 標準連結。[2転]IV→[基]I 2巻補充課題11その１
        StandardIp61_c, //< 標準連結。[2転]I→[基]V I巻p61～62
        StandardIp64_37, //< 標準連結。[1転]II→[2転]I I巻p64,37
        StandardIp64_37_2, //< 標準連結。S和音→[2転]I I巻p64,37
        StandardIp109_4, //< 標準連結。先行和音が[□]V密(根)の場合、共通音を保留しなくてもよい。
        StandardIp109_5, //< 標準連結。IIの和音→[1転]V□の場合、共通音IIを保留する。
        StandardIp125_2G, //< 標準連結。[基]VI→[2転]I I巻p125 2G
        GoodIp72,      //< 良。V7→I I巻p72,43、I巻76,45
        GoodIp74_1, //< 良。I II IV→V7(上部構成音a) I巻p74
        GoodIp77_46, //< 良。[基]V7→[基]VI I巻p77,46
        GoodIp78_46_2, //< 良。I II IV VI→V7 I巻p78
        GoodIp81,      //< 良。[2転]V7根音省略形体(上部構成音a)→□ I巻p81,49
        RareIp81,      //< まれ。[2転]V7根音省略形体(上部構成音a)→[1転]I cf.(I巻p81,49)
        GoodIp82,      //< 良。[2転]V7根音省略形体(上部構成音b)→□ I巻p82,49
        OkayIp86_53,   //< 短調のV9。最適ではない
        GoodIp87_54,   //< 良。[基]V9→[基]I I巻p87,54
        GoodIp94_59,   //< 短調のV9根省の配置。
        GoodIp126_3,   //< 良。□→[1転]V7根省または[3転]V7根省 I巻p126,3
        GoodIp126_3_2, //< 良。[1転]V7根省または[3転]V7根省→Iの和音 I巻p126,3
        OkayIp128_5, //< V9の和音 最適以外の配置。
        GoodIIp12_3, //< II7→後続和音の連結。(II巻p12,3)
        GoodIIp13_4, //< 先行和音→II7の和音の連結。(II巻p13,4)
        GoodIIp34_18_1_1, //< V_V諸和音→V 3和音の連結。(II巻p34,18 1)
        GoodIIp34_18_1_2, //< V_V諸和音→V7またはV9の連結。(II巻p34,18 1)
        GoodIIp34_18_1_3, //< V_V諸和音→[2転]Iの連結。(II巻p34,18 1)
        GoodIIp36_18_2, //< 先行和音→V_V 3和音の連結。(II巻p36,18 2)
        GoodIIp39_19_1, //< [1転]V_V 3和音の配置。『最適である』(II巻p39,19 1)
        GoodIIp39_19_1_2, //< [1転]V_V 3和音の配置。『用いうる』(II巻p39,19 1)
        GoodIIp43_20_3,   //< 先行和音→V_V 7の和音の連結。(II巻p43,20 3)
        GoodIIp48_21_2,   //< [2転]V_V7根省→Xの連結。(II巻p48,21 2)
        GoodIIp51_22_1,   //< V_V9の和音 最適配置。(II巻p51,22 1)
        GoodIIp54_22_4,   //< 先行和音→V_V 9の和音の連結 (II巻p54,22 4)
        GoodIIp68_25_2,   //< IV7→D諸和音の連結 (II巻p68,25 2)
        GoodIIp68_25_3,   //< IV7開(7)→S諸和音の連結 (II巻p68,25 3)
        RareIIp68_25_3,   //< IV7開(7)以外のIV7→S諸和音の連結 (II巻p68,25 3)
        GoodIIp70_25_4,   //< 先行和音→IV7の和音の配置・連結 (II巻p70,25～II巻p70,26)
        OkayIIp51_22_1,   //< V_V9の和音 最適ではない配置。(II巻p51,22 1)
        AcceptableIp54_31, //< I巻p54 31 [1転]II→Vで標準連結
        AcceptableIp108,   //< I巻p108,3 終結和音に先行するVのSopがIIの場合の標準外連結
        AcceptableIp123_2A, //< I巻p123 2A 共通音を保留しない連結
        AcceptableIp123_2B, //< I巻p123 2B 配分転換を生ずる連結
        AcceptableIp124_2C, //< I巻p124 2C 標準外的配置を含む連結
        AcceptableIp124_2D, //< I巻p124 2D 導音の下行を含む連結
        AcceptableIp124_2E, //< I巻p124 2E 第7音の2度上行をふくむ連結。
        AcceptableIp125_2F, //< I巻p125 2F 予備のない低音4度・低音2度を生ずる連結。
        AcceptableIp126_2H_2,//< I巻p126 2H 短調のVI→Vの連結 例2
        AcceptableIp126_2I, //< I巻p126 2i II→VまたはV7の標準外的連結。
        PositionOfAChordChangeProgression, //< 配分転換を生ずる連結
        CommonPitchNoSustainedProgression, //< 共通音を保留しない連結。
        NonStandardChordProgression, //< 標準外配置をふくむ連結。
        Exceptionalp166,   //< 例外的。Sopの旋律線の考慮。別巻課題の実施p.166
        Inv4Up2Progression,          //< 第7音の2度上行をふくむ連結。
        AcceptableIk28_10, //< 別巻課題の実施 I巻課題28-10 [1転]IV→[1転]Vで共通音IVは保留するが他の音は2度上行。
        AcceptableIk30_3, //< 別巻課題の実施 I巻課題30-3 [□]I→[基]V7で上3声があまり下行していない。
        AcceptableIk43_7, //< 別巻課題の実施 I巻課題43-7 [1転]IV→V7で先行和音の上3声にIVが2個あり、片方だけ保留。
        AcceptableIIp22_11, //< IIp22,11 対斜
        AcceptableIIp35_18, //< IIp35,18 対斜
        AcceptableIIk27_7, //< 別巻課題の実施 II巻課題24-7 IVの和音→[1転]Vの連結で、共通音IVを保留し、他の1音は2度上行、もう1音は下行 cf.(I巻p.74,44)
        AcceptableIIIp134_53c, //< 対斜が起きているが、後続音が第9音なので許容される。(III巻p134,53 2 c)
        InfoIIp77_30_2,        //< 対斜が起きている。先行和音が-IIの和音で、対斜が起きている声部が根音なので許容される。(II巻p77,30 2)
        AcceptableIIh7_01, //< 先行和音→II7の和音で、配分転換。(II巻補充課題7-1)
        RuleA2_1,   //< 導音の重複。
        RuleA3,   //< 9の和音でpart0とpart1が近すぎる
        RuleA4,   //< 9の和音の第9音
        RuleB1_1, //< part0が7度音程
        RuleB1_2, //< part0が増1度を除く増音程
        RuleB1_3, //< part0が複音程
        RuleB2_1, //< 導音の限定進行
        RuleC1,   //< part0とpart1が連続8度、連続1度
        RuleC2,   //< part0とpart1が連続5度
        RuleC2_1,   //< part0とpart1が連続5度だが許容される
        RuleC2_S1,   //< part0とpart1が連続5度だが後続の5度をなす2音のどちらかが第9音なので許容される
        RuleC2_S2,   //< part0とpart1が連続5度だが後続の5度をなす2音のどちらかが第9音で、V9の根省の場合。
        RuleC3_1, //< 並達8度
        InfoRuleC3_1_2, //< 並達8度 OKの場合。Sop順次進行
        InfoRuleC3_1_3, //< 並達8度 OKの場合。別巻p.166 注4
        RuleC3_2, //< 並達5度
        InfoRuleC3_2, //< 並達5度。良好である
        InfoRuleC3_2_2, //< 並達5度 OKの場合。I巻p91
        InfoRuleC3_2_3, //< 並達5度 OKの場合。II巻p37,18 2)
        RuleC3_3_1, //< 並達1度
        RuleC3_3_2, //< 並達1度
        RuleD1,     //< 第7音の予備
        MinorII77thNonSustain, //< 短調II7の第7音予備しない
        RuleD2,     //< 低音4度の予備
        RuleIp73_43,       //< I巻p73 [2転]V7→[1転]I→([1転]IIまたは[3転]V7)の進行のルール
        RareIh06_03, //< [2転]V7根省 上部構成音 a) I巻補充課題6-3
        RareIp54,          //< I巻p54 [1転]II→[1転]Vは稀である。
        RareIp128_4,       //< I巻p128,4 [1転]V9または[3転]V9のVが先行和音から保留されないのは稀である。
        RareIk44_5, //< 別巻課題の実施 I巻課題44-5 [2転]I→V7でBasが8度上行。
        RareIk44_6, //< 別巻課題の実施 I巻課題44-6 先行和音→[2転]V7 上部構成音b)
        RareIh09_5,  //< 別巻課題の実施 I巻補充課題9-5 [基]IV→[3転]V7で上3声があまり下行していない。
        RareIIp33_17_1, //< [2転]V_V7の上部構成音 a) ほとんど用いない。(II巻p33,17 1)
        RareIIp39_19_2, //< [基]V_V 3和音は[2転]Iに進むことはまれである。(II巻p39,19 2)
        RareIp61_b,     //< まれな[基]I→[2転]IVの連結(II巻補充課題3-10 注2)
        RarePitch,  //< ピッチがまれ。
        RareInterval, //< 離隔 なるべく避けよ。
        RareStartChord, //< 開始和音が標準外配置。
        RareIIp61_23_2b, //< 長調のV_V下方変位和音に固有のV9の和音が後続するのはまれである。準固有のV9の和音が続くことが多い(II巻p61,23 2b)
        RareIIp62_23_3,  //< 長調の固有のV_V9根省下変和音の使用はまれである。(II巻p62,23 3)
        ExceptionalTermination, //< 例外的終止
        NotSoGoodNonStandardProgression, //< 標準外連結。
        NotSoGoodBp22_3, //< 別巻p22 注3
        NotSoGoodIp83_49_3_2, //< I巻p83,49 3 [2転]V7の先行和音は[基]I、[1転]I、IIがよい。
        NotSoGoodIp100_63_1_1, //< I巻p100,63 1) 低音4度の予備。
        NotSoGoodIp100_63_1_2, //< I巻p100,63 1) 低音2度の予備。
        NotSoGoodIIp68_25_3, //< IV7の和音→後続和音の連結で、II巻p68～p69に載っていない連結。
        AvoidIp106_2, //< I巻p106 2
        AvoidIp109_6, //< I巻p109 6
        AvoidIp61_a1, //< I巻p61 [基]I→[2転]Vの定型
        AvoidIp61_a1_2, //< 標準連結。[2転]V→[1転]Iの定型 I巻p61～62
        AvoidIp61_a2, //< 標準連結。[1転]I→[2転]Vの定型  I巻p61～62
        AvoidIp61_a2_2, //< I巻p61 [2転]V→[基]Iの定型
        AvoidIp61_b, //< I巻p61 [基]I→[2転]IVの定型
        AvoidIp61_b_2, //< I巻p61 [2転]IV→[基]Iの定型
        AvoidIp61_c, //< I巻p61 [2転]I→[基]Vの定型
        AvoidIp64_37, //< I巻p64,37 [1転]II→[2転]Iの連結
        AvoidIp64_37_2, //< I巻p64,37 S和音→[2転]I
        AvoidIp38_Cadence, //< I巻p38 カデンツの法則
        AvoidIp80, //< I巻p80,49 [2転]V7根省の連結
        AvoidIp88_55, //< I巻p88,55 V9の和音の並達9度 避けよ
        AvoidIIIp212_76_3_1, //< III巻p212,76 3) [基]IIIの後続和音。
        AvoidIIIp212_76_3_2, //< III巻p212,76 3) [基]IIIの後続和音IVへの進行で上3声が下行していない。
        AvoidIIIp212_76_3_3, //< III巻p212,76 3) [1転]IIIの後続和音。
        AvoidIIIp214_76_4_1, //< III巻p214,76 4) VIIの後続和音は[基]III
        InfoIIIp214_76_4_2, //< III巻p214,76 4) VIIの和音は反復進行以外ではほとんど用いない。
        AvoidIIIp214_76_5_1, //< III巻p214,76,5) [1転]VIの後続和音。
        InfoIp88_55,  //< I巻p88,55 V9の和音の並達9度 良好である
        AvoidIIp37_18_2, //< [基]V_V 9の和音 並達9度 避けなければならない場合。(II巻p37,18 2)
        InfoIIp37_18_2,  //< [基]V_V 9の和音 並達9度 良好である。(II巻p37,18 2)
        AvoidIIp38_19_1, //< [基]V_V 3和音 Oct配置は使用しない。(II巻p38,19 1)
        AvoidIIp40_19_2, //< [1転]V_V 3和音→[2転]IではSopを2度下行するのがよい。(II巻p40,19 2)
        AvoidIIp41_20_1, //< [2転]V_V7 以外のV_V7の和音 Oct配置は使用しない。(II巻p41,20 1)
        AvoidIIp42_20,   //< [基]V_V7→[2転]I V_V7は上部構成音a)を用いる。(II巻p42,20)
        AvoidDoNotUseYet, //< 現段階（?）では使用しない
        WrongDoNotUseChordType,      //< 用いない和音形体。(III巻p.211 [2転]III7等)
        RuleA5,    //< 変位構成音{0}と第3音{1}が単音程減3度 (公理A5)(II巻p60,23)
        WrongBasTenInterval,      //< BasとTenのIntervalが12度以上ある。
        WrongIp75,         //< I巻p75 V7に第5音を含む場合は2度下行しなければならない
        WrongIp72_1,       //< I巻p72 V7の上3声中の根音はつねに保留する
        WrongIp72_2,       //< I巻p72 V7の上3声中の第3音は上行限定進行音(2度上行する)
        WrongIp72_3,       //< I巻p72 V7の上3声中の第7音は下行限定進行音(2度下行する)
        WrongIp87_54,      //< I巻p87,54 V9の和音はIの和音に進む。
        WrongIp87_54_2,    //< I巻p87,54 [基]V9は[基]Iに進む。
        WrongIp89_56_1,    //< I巻p89,56 [1転]V9は[基]Iに進む。
        WrongIp89_56_2,    //< I巻p89,56 [2転]V9は[1転]Iに進む。
        WrongIp89_56_3,    //< I巻p89,56 [3転]V9は[1転]Iに進む。
        WrongIp128_4_1,    //< I巻p128,4 [1転]V9または[3転]V9のVは保留する。
        WrongTermination, //< I巻p40,24、II巻p94 終止の定型
        WrongIIp13_4,      //< II巻p13,4 II7の和音の第7音が予備されていない。
        RareBasOctaveOnSustain, //< 例外的。第7音の予備。Basのオクターブ跳躍は保留に準ずる。(別巻課題の実施p159 注)
        WrongIIp16_5,      //< II巻p16,5 公理D2 長調のII7の和音のBasの第5音が予備されていない。
        WrongIIp12_3,      //< IIp12,3 II7の後続和音はD諸和音
        WrongIIp12_3_V,    //< IIp12,3 II7→Vの和音の連結のII7の第7音
        WrongIIp12_3_I,    //< IIp12,3 II7→Iの和音の連結のII7の第7音
        WrongIIp12_3_II,   //< IIp12,3 II7の上3声の根音はIやVIIに下行しない。
        WrongIIp22_11,     //< IIp22,11 対斜
        WrongIIp25_13,     //< IIp25,13 II巻p25,13 準固有和音→固有和音に進めない。
        WrongIIp36_18_2,    //< IIp36,18 II7の第7音を保留できない。(IIp36,18 2)
        WrongIIp37_18,      //< Vへの短2度上行以外の進行は不良。IIp37,18
        WrongIIp68_25_2,    //< IV7→後続和音の連結で第7音が2度下行していない(IIp68,25 2)(IIp68,25 3)
        WrongIIp70_25_4,    //< 先行和音→IV7の和音の連結で、第7音が予備されていない。(IIp70,25,4)
        WrongIIp70_25_5,    //< 先行和音→○IV7の和音 先行和音が○VI7の和音でない。(IIp70,25,5)
        WrongIIp70_26,      //< 先行和音→[2転]IV7の和音の連結で、低音4度の予備がなされていない。(IIp70,26)
        RareIIp70_26,       //< 先行和音→[2転]IV7の和音の連結で、Basがオクターブ上行。({1}) cf.(IIp70,26)
        InfoBp87_1,         //< 別巻課題の実施p87注2 第9音高位の準固有V9の和音はSopの↓VIが不自然にきこえることがある。
        InfoBp87_2,         //< 別巻課題の実施p87注5 第3音高位の準固有IVの和音はSopの↓VIが不自然にきこえることがある。
        InfoBasTenInterval, //< BasとTenのIntervalが12度以上ある。
        InfoIIp35_18,       //< 低音2度だが、さしつかえない(II巻p35,18)
        RareIIk50_5,        //< 低音2度。Basのオクターブ上行。(II巻課題50-5)
        InfoPositionOfAChordChanged, //< 配分転換が生じている。
        AcceptableIIp37_18,       //< 増2度上行 許される(II巻p37,18)(公理B1付則)
        IIp37_18_2,         //< 長調で外声がIII→↑IVの上行をした後はVへの短2度上行をする必要がある。(II巻p37,18)
        InvalidAlteredChord, //< 変位和音なのに、肝心の変位音が含まれない。
        UnusedLoweredV,      //< ほとんど用いない形体。(II巻p59,23 1)
        IV7Upper3A,          //< [基]IV7 上部構成音a) (II巻p67,25)
        IV7Upper3B,          //< [基]IV7 上部構成音b) (II巻p67,25)
        RareDorian,          //< ドリアの和音 あまり用いられない形体 (II巻p73,27)
        DorianBestPosition,  //< ドリアの和音 最適配置(II巻p74,28 1)
        DorianNotBestPosition,  //< ドリアの和音 最適配置ではない配置(II巻p74,28 1)
        GoodIIp76_28_3,      //< 先行和音→ドリアのIV7の和音の連結(II巻p76,28 3)
        WrongIIp74_28_2_1,   //< ドリアのIV7の和音の第3音の進行ルールを守っていない(II巻p76,28 2)
        WrongIIp74_28_2_2,   //< ドリアのIV7の和音の第7音の進行ルールを守っていない(II巻p76,28 2)
        GoodIIp74_28_2_3,    //< ドリアのIV7の和音→V諸和音の連結 (II巻p74,28 2)
        WrongIIp74_28_2_3,    //< ドリアのIV7の和音はV諸和音以外に進むことはない。 (II巻p74,28 2)
        RareNapolitan,       //< -II (ナポリの六)の和音 あまり用いられない形体(II巻p76,30 1)
        BestNapolitan,       //< -II (ナポリの六)の和音 最適配置(II巻p76,30 1)
        OkayNapolitan,       //< -II (ナポリの六)の和音 最適ではない配置(II巻p76,30 1)
        GoodOct5Napolitan,   //< -II (ナポリの六)の和音 十分用いうる配置(別巻課題の実施II巻課題34-3 注)
        IIp77_30_2,          //< -II (ナポリの六)の和音→D諸和音の連結 (II巻p77,30 2)
        IIp77_30_3,          //< -II (ナポリの六)の和音→S和音の連結 (II巻p77,30 3)
        WrongIIp77_30,       //< -II (ナポリの六)の和音→後続和音の連結 (II巻p77,30)
        IIk34_3,             //< →[1転]-IIOct(5) (別巻課題の実施 II巻課題34-3)
        BestAddedSix,        //< 付加6の和音。最適配置。
        BestAddedSixFour,    //< 付加4-6の和音。最適配置。
        UnusedAddedSixFour,  //< 付加4-6の和音。ほとんど用いられない配置。
        GoodIIp90_39_3,      //< 先行和音→IV付加6の和音。(II巻p90,39 3)
        AvoidIIp90_39_3,      //< 先行和音→IV付加6の和音。{1}を予備していない。(II巻p90,39 3)
        InfoIIp90_39_2_1,    //< IV付加6の和音→後続和音の連結 後続和音は[基]Iである。(II巻p90,39 2)
        GoodIIp90_39_2_2,     //< IV付加6の和音→後続和音の連結(II巻p90,39 2)
        AvoidIIp90_39_2_3,    //< IV付加6の和音→後続和音の連結 第5音、第6音の限定進行ルールを守っていない。(II巻p90,39 2)
        GoodtoIV46,           //< →IV付加4-6の和音 (別巻p.165 II巻補充課題12-10)
        AvoidIIp93_42_2_1,    //< IV付加4-6の和音→後続和音の連結 後続和音は[基]Iである。(II巻p93,42 2)
        GoodIIp93_42_2_2,     //< IV付加4-6の和音→後続和音の連結(II巻p93,42 2)
        AvoidIIp93_42_2_3,    //< IV付加4-6の和音→後続和音の連結 第4音、第6音、第3音の限定進行ルールを守っていない。(II巻p93,42 2)
        InfoIIp98_47_a,       //< 転調進行 上3声に半音階的関係をなす2音が含まれる場合。(II巻p98,47 a)
        InfoIIp98_47_a2,      //< 転調進行 対斜が起きているが、後続和音が減7の和音なので許容される。(II巻p98,47 a)
        GoodIIp98_47_b,       //< 転調進行 上3声に共通音が含まれる場合。(II巻p98,47 b)
        AvoidIIp98_47_b,      //< 転調進行 上3声に共通音が含まれる場合、保留する。(II巻p98,47 b)
        GoodIIp98_47_c,       //< 転調進行 上3声に共通音が含まれない場合。(II巻p98,47 c)
        AvoidIIp98_47_c,      //< 転調進行 上3声に共通音が含まれない場合。(II巻p98,47 c)
        AcceptableIIp99_47_d, //< 転調進行 増2度上行だが、許される。公理B1付則 (II巻p99,47 d)
        GoodIIp99_47_e,       //< 転調進行 離脱和音の限定進行音の取り扱い。(II巻p99,47 e)
        AvoidIIp99_47_e,      //< 転調進行 離脱和音の限定進行音の取り扱い。(II巻p99,47 e)
        AcceptableIIp100_47_e, //< 転調進行 離脱和音の導音の跳躍上行。許されることがある。(II巻p100,47 e)
        AvoidIIp100_47_f,     //< 転調進行 転入和音が7の和音。第7音{1}の予備が必要。(II巻p100,47 f)
        GoodIIp100_47_f,      //< 転調進行 転入和音が7の和音。第7音{1}の予備が必要。(II巻p100,47 f)
        NotSoGoodBas6Progression, //< Basの6度の進行は、I→VIの上行、I→IIIの下行以外はなるべく避けよ。(II巻p122,55 3 a)
        RareBas6Progression,  //< Basの6度上行。後続和音が[3転]V9根省和音の場合はまれに見られる。(I巻補充課題9-5)(II巻補充課題2-3)(II巻補充課題3-8)(II巻補充課題5-6)
        NotSoGoodBas6Progression2, //< Basの6度跳躍の次は反対方向への進行をするのがよい。(II巻p123,55 3 b)
        NotSoGoodBas5ProgInv7, //< Basが5度下行で第7音に到達している。(II巻p123,55 3 c)
        NotSoGoodBas5ProgLeadingNote, //< Basが5度上行で導音に到達している。(II巻p123,55 3 c)
        NotSoGoodBasTooFarProgression, //< Basが2回連続して跳躍し、合計した音程が{1}度。(II巻p123,55 3 d)
        InfoIIp114,           //< ソプラノ定型情報。
        InfoIIp95_2,          //< Bas定型情報 和音2個
        InfoIIp95_3,          //< Bas定型情報 和音3個
        InfoIIp95_4,          //< Bas定型情報 和音4個
        InfoIIp117_2,          //< Sop定型情報 和音2個
        InfoIIp117_3,          //< Sop定型情報 和音3個
        InfoIIp117_4,          //< Sop定型情報 和音4個
        InfoIIp117chordOK,     //< 和音進行がSop定型情報{1}のとおりである。
        InfoIIp117chordNotOK,  //< 和音進行がSop定型情報{1}のとおりでない。
        InfoIIp117chordOKBasOK, //< 和音進行OK、Bas定型OK。
        InfoIIp117chordNotOKBasOK, //< 和音進行NotOK、Bas定型OK。
        InfoIIp117chordOKBasNotOK, //< 和音進行OK、Bas定型NotOK。
        InfoIIp117chordNotOKBasNotOK, //< 和音進行NotOK、Bas定型NotOK。
        InfoIIp117TerminationDoesNotMatch, //< 終止になりえない箇所に終止記号がつけられている。
        NotSoGoodIIp121_55_2_a, //< 同型の対応バス定型が連続している。(IIp122,55 2 a)
        IIk50_12Progression,    //< I→[1転]V7(上部構成音a)で、I巻p74のルールを守っていない。(II巻課題50-12)
        InfoIIIp212_76_3,       //< V→IIIのVの導音は限定進行しなくて良い。
        RareIIIp215_76_6_1,     //< [基]VI→[1転]I
        WrongIIIp218_77_3_1,    //< I7 III7 VI7 VII7の和音の第7音は限定進行する (III巻p218 77 3)
        WrongIIIp218_77_3_2,    //< I7 III7 VI7 VII7の和音の第7音には予備で到達する (III巻p218 77 3)
        GoodIIIp218_77_3_4,       //< 7の和音を含む連結で共通音がない場合にBasとSopが反行していなくても良い(III巻p218 77 3)
        GoodIIIp218_77_3_5,      //< V7→I7 (III巻p218 77 3)
        AvoidIIIp219_77_3_6,     //< 低音4度の予備
        AvoidIIIp219_77_3_7,     //< [2転]のBasは2度下行する
        ProhibitedIIIp219_77_3_8, //< 7の和音の並達8度
        AcceptableIIIp219_77_3_9, //< 導音の2度下行。許される。(III巻p219 77 3)
        AvoidIIIp219_77_3_10,      //< 導音の保留。(III巻p219 77 3)
        GoodIIIp220_77_4_1,      //< I7→X III7→X VI7→X VII7→X (III巻p220 77 4)
        GoodIIIp220_77_4_2,      //< X→I7 X→III7 X→VI7 X→VII7 (III巻p220 77 4)
        GoodIIIp221_77_5_2,      //< V→III7の和音。先行和音は固有のVIIを持つVが良い。
        RareIIIp221_77_5_2,      //< V→III7の和音。先行和音は固有のVIIを持つVが良い。
        IIIp222_77_5_3,          //< 短調固有のVIIを持つV7の和音の第7音の予備(III巻p222 77 5)
        IIIp223_78,            //< IV+6→[1転]I 第6音は4度上行する。(III巻p223 78)
        UnusedRaisedV_V,         //< V_Vの上方変位。3和音しか用いない。
        UnusedRaisedV7,          //< V7根省の上方変位。用いられない。
        RuleA5_2,                //< 変位第5音と第7音が単音程減三度。
        OkayRaisedV,             //< 最適ではない配置の上方変位V和音。
        AvoidIIIp225_79_3_b_1,   //< V上変和音 第5音が上行していない。
        RareRaisedV,             //< 短調のV上変和音
        AvoidIIIp226_79_5_1,     //< IV+6 上変和音 第6音が上行していない。
        RareRaisedIV,             //< 短調のIV+6 IV+46上変和音
        InfoChord,               //< 和音のひと通りの説明
    }

    public partial class Chord
    {
        public string GetVerdictText() {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("総評 {0}\r\n", VerdictValueToString(verdictValue));
            foreach (Verdict v in verdicts) {
                switch (v.reason) {
                case VerdictReason.BestProgressionIp54: sb.AppendFormat("{0}: [1転]IIの配置 『最適である』(I巻p54～55), (I巻p109,6)\r\n",VerdictValueToString(v.value));
                    break;
                case VerdictReason.BestIp64_37: sb.AppendFormat("{0}: [1転]II→[2転]I『最適である』(I巻p64,37)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.BestV9: sb.AppendFormat("{0}: [基]V9の配置。『最適である』 (I巻p86,53)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.Best1転V9: sb.AppendFormat("{0}: 最適な[1転]V9根省の配置。(I巻p89,57)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.Best2転3転V9: sb.AppendFormat("{0}: 最適な[2転]または[3転]V9根省の配置。(I巻p89,57)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.RecommendIp91: sb.AppendFormat("{0}:  推奨される。(I巻p91,57)(I巻p95,59)(I巻p114公理B2付則2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.BestIp81_49_a: sb.AppendFormat("{0}: [2転]V7根音省略形体 上部構成音 a) 最適な配置。(I巻p81,49)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.BestIp83_49_b: sb.AppendFormat("{0}: [2転]V7根音省略形体 上部構成音 b) 最適な配置。(I巻p83,49)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.BestStartChord: sb.AppendFormat("{0}: 第1和音は[基]Iだけを用いる。(I巻p150)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.BestTermination: sb.AppendFormat("{0}: 終止のSopは主音が最適。(I巻p108,3(4))\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.OkayIIIp212_76_3_1: sb.AppendFormat("{0}: 最適ではない配置。[基]III密(5)→[基]IV(密)か[基]III密(根)→[1転]II(密)か[基]III密(3)→[1転]II(密)で、上3声が下行するのが良い。(III巻p212,76(3))\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.OkayIp86_53: sb.AppendFormat("{0}: 短調のV9の和音 最適以外の配置 (I巻p86,53)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIp94_59: sb.AppendFormat("{0}: 短調のV9根音省略形態の和音の配置 (I巻p94,59)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIp126_3: sb.AppendFormat("{0}: □→[1転]V7根省または[3転]V7根省 (I巻p126,3)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIp126_3_2: sb.AppendFormat("{0}: [1転]V7根省または[3転]V7根省→Iの和音 (I巻p126,3)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.OkayIp128_5: sb.AppendFormat("{0}: 長調のV9の和音 最適以外の配置 (I巻p128,5)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIIp12_3: sb.AppendFormat("{0}: II7→後続和音の連結。(II巻p12,3)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIIp13_4: sb.AppendFormat("{0}: 先行和音→II7の和音の連結。(II巻p13,4)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIIp34_18_1_1: sb.AppendFormat("{0}: V_V諸和音→V 3和音の連結。(II巻p34,18 1)(II巻p53,22,3)(II巻p60,23)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIIp34_18_1_2: sb.AppendFormat("{0}: V_V諸和音→V7またはV9の連結。(II巻p34,18 1)(II巻p53,22,3)(II巻p60,23)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIIp34_18_1_3: sb.AppendFormat("{0}: V_V諸和音→[2転]Iの連結。(II巻p34,18 1)(II巻p60,23)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIIp36_18_2: sb.AppendFormat("{0}: 先行和音→V_V 3和音の連結。(II巻p36,18 2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIIp39_19_1: sb.AppendFormat("{0}: [1転]V_V 3和音の配置。『最適である』(II巻p39,19 1)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIIp39_19_1_2: sb.AppendFormat("{0}: [1転]V_V 3和音の配置。『用いうる』(II巻p39,19 1)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIIp43_20_3: sb.AppendFormat("{0}: 先行和音→V_V7の和音の連結。(II巻p43,20 3))\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIIp48_21_2: sb.AppendFormat("{0}: [2転]V_V7根省→□の連結。(II巻p48,21 2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIIp51_22_1: sb.AppendFormat("{0}: V_V9の和音 最適配置。(II巻p51,22 1)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIIp54_22_4: sb.AppendFormat("{0}: 先行和音→V_V 9の和音の連結 (II巻p54,22 4)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIIp68_25_2: sb.AppendFormat("{0}: IV7→D諸和音の連結 (II巻p68,25 2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIIp68_25_3: sb.AppendFormat("{0}: IV7→S諸和音の連結 (II巻p68,25 3)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.RareIIp68_25_3: sb.AppendFormat("{0}: IV7→S諸和音の連結 先行和音は開(7)が多い (II巻p68,25 3)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIIp70_25_4: sb.AppendFormat("{0}: 先行和音→IV7の和音の配置・連結 (II巻p70,25～II巻p70,26)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.OkayIIp51_22_1: sb.AppendFormat("{0}: V_V9の和音 最適ではない配置。(II巻p51,22 1)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp30: sb.AppendFormat("{0}: 標準連結。[基]□→[基]□ 共通音がある場合。(I巻p30)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp32: sb.AppendFormat("{0}: 標準連結。[基]□→[基]□ 共通音がない場合 (I巻p32)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp34_1: sb.AppendFormat("{0}: 標準連結。[基]V→[基]VI (I巻p34)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp34_2: sb.AppendFormat("{0}: 標準連結。[基]II→[基]V (I巻p34)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp34_3: sb.AppendFormat("{0}: 標準連結。[基]IV→[基]II (I巻p34)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp36: sb.AppendFormat("{0}: 標準連結。[基]VI→[基]V (I巻p36), (I巻p106)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp51: sb.AppendFormat("{0}: 標準連結。[1転]□⇔[基]□ 共通音のある場合(I巻p49～p51), (I巻p109,5), (III巻p213,76 3), (III巻p214,76 4), (III巻p214,76 5)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp52: sb.AppendFormat("{0}: 標準連結。[1転]□⇔[基]□ 共通音のない場合(I巻p49～p52)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp53_1: sb.AppendFormat("{0}:標準連結。 [1転]□→[1転]□ 共通音のある場合 (I巻p53)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp53_2: sb.AppendFormat("{0}: 標準連結。[1転]□→[1転]□ 共通音のない場合 (I巻p53)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp54_1_1: sb.AppendFormat("{0}: 標準連結。[1転]II→[1転]□ 共通音のある場合 (I巻p54), (I巻p109,5)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp54_1_2: sb.AppendFormat("{0}: 標準連結。[1転]II→[1転]□ 共通音のない場合 (I巻p54)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp54_2: sb.AppendFormat("{0}: 標準連結。[基]□→[1転]II [1転]IIは第3音を含む密(根)が最適である(I巻p54-55)(I巻p109,6)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIp54_2_2: sb.AppendFormat("{0}: [基]□→[1転]IIOct(根) I巻p54には書いてないが課題の実施例に頻出する(I巻p54)(I巻p109,6)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp54_31: sb.AppendFormat("{0}: 標準連結。[1転]II→[基]□ (I巻p54)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp61_a1: sb.AppendFormat("{0}: 標準連結。[基]I→[2転]V (I巻p61～62)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp61_a1_2: sb.AppendFormat("{0}: 標準連結。[2転]V→[基]I (I巻p61～62)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp61_a2: sb.AppendFormat("{0}: 標準連結。[1転]I→[2転]V (I巻p61～62)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp61_a2_2: sb.AppendFormat("{0}: 標準連結。[2転]V→[基]I (I巻p61～62)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp61_b: sb.AppendFormat("{0}: 標準連結。[基]I→[2転]IV (I巻p61～62)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp61_b_2: sb.AppendFormat("{0}: 標準連結。[2転]IV→[基]I (I巻p61～62)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp61_b_2_2: sb.AppendFormat("{0}: [2転]IV→[基]I Basがオクターブ下行。(2巻補充課題11その１)cf.(I巻p61～62)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp61_c: sb.AppendFormat("{0}: 標準連結。[2転]I→[基]V (I巻p61～62)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp64_37: sb.AppendFormat("{0}: 標準連結。[1転]II→[2転]I (I巻p64,37)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp64_37_2: sb.AppendFormat("{0}: 標準連結。S和音→[2転]I (I巻p64,37)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp109_4: sb.AppendFormat("{0}: 標準連結。先行和音が[□]V密(根)の場合、または[3転]V7密(根)の場合は、共通音を保留しなくてもよい。(I巻p109,4)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp109_5: sb.AppendFormat("{0}: 標準連結。IIの和音→[1転]V□の場合、共通音IIを保留する。(I巻p109,5)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.StandardIp125_2G: sb.AppendFormat("{0}: 標準連結。[基]VI→[2転]I (I巻p125,2G)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIp72: sb.AppendFormat("{0}: V7→I (I巻p72,43), (I巻p75,45)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIp74_1: sb.AppendFormat("{0}: I II IV→V7(上部構成音a)または[□転]V7 (I巻p74,44)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIp77_46: sb.AppendFormat("{0}: V7→VI (I巻p77,46)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIp78_46_2: sb.AppendFormat("{0}: I II IV VI→V7(上部構成音b) なるべく下行させるのがよい。 (I巻p78,46)(I巻p125,2Gに2つ上行している例あり)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIp81: sb.AppendFormat("{0}: [2転]V7根音省略形体(上部構成音a)→□ (I巻p81,49)(I巻p114公理B2付則2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.RareIp81: sb.AppendFormat("{0}: [2転]V7根音省略形体(上部構成音a)→[1転]I 教科書通りでない進行。({1}) cf.(I巻p81,49)\r\n", VerdictValueToString(v.value), v.str0);
                    break;
                case VerdictReason.GoodIp82: sb.AppendFormat("{0}: [2転]V7根音省略形体(上部構成音b)→□ (I巻p82,49)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIp87_54: sb.AppendFormat("{0}: V9の和音→Iの和音 (I巻p87,54)(I巻p89,57)(I巻p94,59)(I巻p127,4)(I巻p128,5)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.RuleA2_1: sb.AppendFormat("{0}: 公理A2: 導音の重複。{1}\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.RuleA3: sb.AppendFormat("{0}: 公理A3: 第9音{2}が根音{1}の9度以上上に来ていない。\r\n", VerdictValueToString(v.value), v.part0, v.part1);
                    break;
                case VerdictReason.RuleA4: sb.AppendFormat("{0}: 公理A4: 長調で第9音{2}が第3音{1}の7度以上上に来ておらず、どちらも予備されていない。\r\n", VerdictValueToString(v.value), v.part0, v.part1);
                    break;
                case VerdictReason.RuleB1_1: sb.AppendFormat("{0}: 公理B1: {1}が7度音程進行。\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.RuleB1_2: sb.AppendFormat("{0}: 公理B1: {1}が増1度を除く増音程進行。\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.RuleB1_3: sb.AppendFormat("{0}: 公理B1: {1}が複音程進行。\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.RuleB2_1: sb.AppendFormat("{0}: 公理B2: {1}が導音の限定進行ルールに従っていない。\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.RuleC1: sb.AppendFormat("{0}: 公理C1: {1}と{2}が連続8度または連続1度を形成している。\r\n", VerdictValueToString(v.value), v.part0, v.part1);
                    break;
                case VerdictReason.RuleC2: sb.AppendFormat("{0}: 公理C2: {1}と{2}が連続5度で、しかも後続音程が完全5度。\r\n", VerdictValueToString(v.value), v.part0, v.part1);
                    break;
                case VerdictReason.RuleC2_1: sb.AppendFormat("{0}: {1}と{2}が連続5度だが例外的に許される。(I巻p36)\r\n", VerdictValueToString(v.value), v.part0, v.part1);
                    break;
                case VerdictReason.RuleC2_S1: sb.AppendFormat("{0}: {1}と{2}が連続5度だが後続の5度をなす2音のどちらかが第9音なので許容される。(別巻課題の実施p.171 注)(III巻p.136,53 c)\r\n", VerdictValueToString(v.value), v.part0, v.part1);
                    break;
                case VerdictReason.RuleC2_S2: sb.AppendFormat("{0}: {1}と{2}が連続5度だが許容される(別巻課題の実施p.171 注)(III巻p.136,53 c)(総合和声 p.114) N.B.この連結を許容するか、不良とするかは先生によって意見が分かれる。\r\n", VerdictValueToString(v.value), v.part0, v.part1);
                    break;
                case VerdictReason.RuleC3_1: sb.AppendFormat("{0}: 公理C3: {1}と{2}が並達8度で、Sopが順次進行でない。\r\n", VerdictValueToString(v.value), v.part0, v.part1);
                    break;
                case VerdictReason.InfoRuleC3_1_2: sb.AppendFormat("{0}: 外声間並達8度。Sopが順次進行なので許容される。\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.InfoRuleC3_1_3: sb.AppendFormat("{0}: 例外的に許容される外声間並達8度。[基]VI→[1転]II(3) (別巻p.166 注4)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.RuleC3_2: sb.AppendFormat("{0}: 公理C3: {1}と{2}が並達5度で、Sopが順次進行でない。\r\n", VerdictValueToString(v.value), v.part0, v.part1);
                    break;
                case VerdictReason.InfoRuleC3_2: sb.AppendFormat("{0}: 公理C3: 外声間並達5度だが、Sopが順次進行なので良好である。\r\n", VerdictValueToString(v.value), v.part0, v.part1);
                    break;
                case VerdictReason.InfoRuleC3_2_2: sb.AppendFormat("{0}: 公理C3付則: 先行和音→[2転]V9根音省略形第9音高位の連結の外声間並達5度は許される。\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.InfoRuleC3_2_3: sb.AppendFormat("{0}: 外声間並達5度だが、許される。(II巻p37,18 2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.RuleC3_3_1: sb.AppendFormat("{0}: 公理C3: {1}と{2}が並達1度で、BasとTenの形成する並達1度ではない。\r\n", VerdictValueToString(v.value), v.part0, v.part1);
                    break;
                case VerdictReason.RuleC3_3_2: sb.AppendFormat("{0}: 公理C3: {1}と{2}が並達1度で、Basが完全4度上行していない、またはTenが短2度上行していない。\r\n", VerdictValueToString(v.value), v.part0, v.part1);
                    break;
                case VerdictReason.RuleD1: sb.AppendFormat("{0}: 公理D1: 第7音の予備が必要 ({1})。\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.MinorII77thNonSustain: sb.AppendFormat("{0}: 例外的: 短調のII7の第7音({1})の予備は免除される。(別巻課題の実施p.166 注3)(III巻 p.218)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.RuleD2: sb.AppendFormat("{0}: 公理D2: 低音4度の予備が必要 ({1})。\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.AvoidIp106_2: sb.AppendFormat("{0}: AltのIII→VIIの5度上行が不自然にきこえる。(I巻p106,2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIp109_6: sb.AppendFormat("{0}: □→[1転]密(根)IIのSopへ跳躍進行するときは、VI→IIの上行か、IV→IIの下行で到達するのがよい。(I巻p109,6)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIp61_a1: sb.AppendFormat("{0}: [基]I→[2転]Vの定型進行ルールに従っていない。(I巻p61,35 a)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIp61_a1_2: sb.AppendFormat("{0}: [2転]V→[1転]Iの定型進行ルールに従っていない。(I巻p61,35 a)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIp61_a2: sb.AppendFormat("{0}: [1転]I→[2転]Vの定型進行ルールに従っていない。(I巻p61,35 a')\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIp61_a2_2: sb.AppendFormat("{0}: [2転]V→[基]Iの定型進行ルールに従っていない。(I巻p61,35 a')\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIp61_b: sb.AppendFormat("{0}: [基]I→[2転]IVの定型進行ルールに従っていない。(I巻p61,35 b)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIp61_b_2: sb.AppendFormat("{0}: [2転]IV→[基]Iの定型進行ルールに従っていない。(I巻p61,35 b)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIp61_c: sb.AppendFormat("{0}: [2転]I→[基]Vの定型進行ルールに従っていない。(I巻p61,35 c)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIp64_37: sb.AppendFormat("{0}: [1転]II→[2転]Iの連結ルールに従っていない。(I巻p64,37)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIp64_37_2: sb.AppendFormat("{0}: S和音→[2転]Iの連結ルールに従っていない。(I巻p64,37)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIp38_Cadence: sb.AppendFormat("{0}: カデンツの法則に従っていない。(I巻p38)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIp80: sb.AppendFormat("{0}: [2転]V7根省の連結の定型進行ルールに従っていない。 (I巻p80,49)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIp88_55: sb.AppendFormat("{0}: V9和音の外声間の並達9度で、上方が2度上行していない。(I巻p88,55)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIIIp212_76_3_1: sb.AppendFormat("{0}: [基]IIIの後続和音は[基]IVか[1転]IIか[基]VI。(III巻p212,76 3)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIIIp212_76_3_2: sb.AppendFormat("{0}: [基]IIIの後続和音IVへの進行で上3声が下行していない。(III巻p212,76 3)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIIIp212_76_3_3: sb.AppendFormat("{0}: [1転]IIIの後続和音は[基]VI。(III巻p212,76 3)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIIIp214_76_4_1: sb.AppendFormat("{0}: VIIの後続和音は[基]III (III巻p214,76 4)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.InfoIIIp214_76_4_2: sb.AppendFormat("{0}: VIIの和音は[基]IIIへ進行。VIIの和音は主に反復進行で用いる (III巻p214,76 4)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIIIp214_76_5_1: sb.AppendFormat("{0}: [1転]VIの後続和音は[基]IIか[3転]II7。 (III巻p214,76 5)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.InfoIp88_55: sb.AppendFormat("{0}: V9和音の外声間の並達9度。良好である。(I巻p88,55)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIIp37_18_2: sb.AppendFormat("{0}: [基]V_V 9の和音 並達9度 避けなければならない場合。(II巻p37,18 2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.InfoIIp37_18_2: sb.AppendFormat("{0}: [基]V_V 9の和音 並達9度 良好である。(II巻p37,18 2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIIp38_19_1: sb.AppendFormat("{0}: [基]V_V 3和音 Oct配置は使用しない。(II巻p38,19 1)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIIp40_19_2: sb.AppendFormat("{0}: [1転]V_V 3和音→[2転]IではSopを2度下行するのがよい。(II巻p40,19 2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIIp41_20_1: sb.AppendFormat("{0}: [2転]V_V7 以外のV_V7の和音 Oct配置は使用しない。(II巻p41,20 1)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIIp42_20: sb.AppendFormat("{0}: [基]V_V7→[2転]I V_V7は上部構成音a)を用いる。(II巻p42,20)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.RuleA5: sb.AppendFormat("{0}: 変位構成音{1}と第3音{2}が単音程減3度 (公理A5)(II巻p60,23)\r\n", VerdictValueToString(v.value), v.part0, v.part1);
                    break;
                case VerdictReason.AvoidDoNotUseYet: sb.AppendFormat("{0}: 現段階では使用しない。({1})\r\n", VerdictValueToString(v.value), v.str0);
                    break;
                case VerdictReason.WrongDoNotUseChordType: sb.AppendFormat("{0}: 使用しない和音形体。({1})\r\n", VerdictValueToString(v.value), v.str0);
                    break;
                case VerdictReason.NotSoGoodBp22_3: sb.AppendFormat("{0}: [1転]開(5)(上3声3省)(上3声根音重複)の3和音はなるべく避けよ。(別巻p22 注3)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.NotSoGoodNonStandardProgression: sb.AppendFormat("{0}: 標準外連結。使用にあたって音楽的判断が必要。(I巻p118)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.NotSoGoodIp83_49_3_2: sb.AppendFormat("{0}: [2転]V7の先行和音は[基]I、[1転]I、IIがよい。(I巻p83,49 3)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.NotSoGoodIp100_63_1_1: sb.AppendFormat("{0}: 低音4度の予備が行われていない。{1}。(I巻p100,63 1) cf.(II巻課題15-9の実施例)(II巻課題50-12の実施例)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.NotSoGoodIp100_63_1_2: sb.AppendFormat("{0}: 低音2度の予備が行われていない。{1}。(I巻p100,63 1)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.NotSoGoodIIp68_25_3: sb.AppendFormat("{0}: IV7の和音→後続和音の連結で、II巻p68～p69に載っていない連結。(II巻p68～69)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AcceptableIp123_2A: sb.AppendFormat("{0}: 2 連結 A 共通音を保留しない連結 {1}。(I巻p123)\r\n", VerdictValueToString(v.value), (v.ordinal0 == 0) ? "-" : v.ordinal0.ToString());
                    break;
                case VerdictReason.AcceptableIp123_2B: sb.AppendFormat("{0}: 2 連結 B 配分転換を生ずる連結 {1}。(I巻p123)\r\n", VerdictValueToString(v.value), (v.ordinal0 == 0) ? "-" : v.ordinal0.ToString());
                    break;
                case VerdictReason.AcceptableIp124_2C: sb.AppendFormat("{0}: 2 連結 C 標準外的配置を含む連結 {1}。(I巻p124)\r\n", VerdictValueToString(v.value), (v.ordinal0 == 0) ? "-" : v.ordinal0.ToString());
                    break;
                case VerdictReason.AcceptableIp124_2D: sb.AppendFormat("{0}: 2 連結 D 導音の下行を含む連結(公理B2付則2)。(I巻p124)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AcceptableIp124_2E: sb.AppendFormat("{0}: 2 連結 E 第7音の2度上行をふくむ連結 {1}。(I巻p124)\r\n", VerdictValueToString(v.value), (v.ordinal0 == 0) ? "-" : v.ordinal0.ToString());
                    break;
                case VerdictReason.AcceptableIp125_2F: sb.AppendFormat("{0}: 2 連結 F 予備のない低音4度・低音2度を生ずる連結 {1}。(I巻p125)\r\n", VerdictValueToString(v.value), (v.ordinal0 == 0) ? "-" : v.ordinal0.ToString());
                    break;
                case VerdictReason.AcceptableIp126_2H_2: sb.AppendFormat("{0}: 2 連結 H 短調のVI→Vの連結 例2 (I巻p126,2H)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AcceptableIp126_2I: sb.AppendFormat("{0}: II→VまたはV7の標準外的連結 {1}。( I巻p126 2I)\r\n", VerdictValueToString(v.value), (v.ordinal0 == 0) ? "-" : v.ordinal0.ToString());
                    break;
                case VerdictReason.AcceptableIp54_31: sb.AppendFormat("{0}: 上3声が全部下行していない連結は“可能”。(I巻p54,31)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AcceptableIp108: sb.AppendFormat("{0}: 終結和音に先行するVのSopがIIの場合の標準外連結。(I巻p108,3)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.PositionOfAChordChangeProgression: sb.AppendFormat("{0}: 配分転換を生ずる連結。旋律線の考慮のために使用可能(別巻課題の実施p31 注2) ({1})\r\n", VerdictValueToString(v.value), v.str0);
                    break;
                case VerdictReason.CommonPitchNoSustainedProgression: sb.AppendFormat("{0}: 共通音を保留しない連結。旋律線の考慮のために使用可能(別巻課題の実施p31 注2) ({1})\r\n", VerdictValueToString(v.value), v.str0);
                    break;
                case VerdictReason.NonStandardChordProgression: sb.AppendFormat("{0}: 標準外配置をふくむ連結。旋律線の考慮のために使用可能(別巻課題の実施p31 注2) ({1})\r\n", VerdictValueToString(v.value), v.str0);
                    break;
                case VerdictReason.Exceptionalp166: sb.AppendFormat("{0}: 例外的。Sopの旋律線の考慮。(別巻課題の実施p.166 注9)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.Inv4Up2Progression: sb.AppendFormat("{0}: 第7音の2度上行をふくむ連結。({1}) cf.(I巻p124 2 連結 E)\r\n", VerdictValueToString(v.value), v.str0);
                    break;
                case VerdictReason.AcceptableIk28_10: sb.AppendFormat("{0}: 先行和音→([基]V7(上部構成音a)または[n転]V7)の連結において共通音IVを保留、上3声の他の音を2度上行する連結。(別巻課題の実施 I巻課題28-10)(別巻課題の実施 I巻補充課題7-10) cf.(I巻p74)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AcceptableIk30_3: sb.AppendFormat("{0}: 先行和音→[基]V7(上部構成音b)で上3声があまり下行していない。(別巻課題の実施 I巻課題30-3)(別巻課題の実施 I巻補充課題7-1) cf.(I巻p78)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AcceptableIk43_7: sb.AppendFormat("{0}: [1転]IV→V7で先行和音の上3声にIVが2個あり、片方だけ保留。(別巻課題の実施 I巻課題43-7) cf.(I巻p74)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.InfoBasTenInterval: sb.AppendFormat("{0}: BasとTenが12度以上離れている。(I巻p18)(I巻p75)(I巻p122,F1)(II巻p112注2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.InfoIIp35_18: sb.AppendFormat("{0}: 低音2度({1})だが、さしつかえない(II巻p35,18))\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.RareIIk50_5: sb.AppendFormat("{0}: 低音2度。Basのオクターブ上行。(II巻課題50-5)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AcceptableIIp37_18: sb.AppendFormat("{0}: {1}の増2度上行 許される(II巻p37,18)(公理B1付則)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.IIp37_18_2: sb.AppendFormat("{0}: ({1})長調で外声がIII→↑IVの上行をした後はVへの短2度上行をする必要がある。(II巻p37,18)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.WrongBasTenInterval: sb.AppendFormat("{0}: BasとTenが12度以上離れた状態が連続している。(I巻補充課題9-7)(II巻補充課題1-5)(II巻補充課題3-8)(II巻補充課題10-9)(II巻補充課題10-11その1)(II巻補充課題10-11その2) cf.(I巻p18)(I巻p75)(I巻p122,F1)(II巻p112注2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.WrongIp75: sb.AppendFormat("{0}: V7の上3声に第5音を含む場合({1})はかならず2度下行する。(I巻p75)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.WrongIp72_1: sb.AppendFormat("{0}: V7の上3声中の根音({1})はつねに保留する。(I巻p72)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.WrongIp72_2: sb.AppendFormat("{0}: V7の上3声中の第3音({1})は上行限定進行音(2度上行する)。(I巻p72)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.WrongIp72_3: sb.AppendFormat("{0}: V7の上3声中の第7音({1})は下行限定進行音(2度下行する)。(I巻p72)(I巻p124E)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.WrongIp87_54: sb.AppendFormat("{0}: V9の和音はIの和音に進む。(I巻p87,54)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.WrongIp87_54_2: sb.AppendFormat("{0}: [基]V9は[基]Iに進む。(I巻p87,54)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.WrongIp89_56_1: sb.AppendFormat("{0}: [1転]V9は[基]Iに進む。(I巻p89,56)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.WrongIp89_56_2: sb.AppendFormat("{0}: [2転]V9は[1転]Iに進む。(I巻p89,56)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.WrongIp89_56_3: sb.AppendFormat("{0}: [3転]V9は[1転]Iに進む。(I巻p89,56)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.WrongIp128_4_1: sb.AppendFormat("{0}: V({1})は保留する。(I巻p128,4)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.WrongTermination: sb.AppendFormat("{0}: 終止の定型ルールに従っていない。(I巻p40,24)(II巻p94)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.WrongIIp13_4: sb.AppendFormat("{0}: II7の和音の第7音({1})が予備されていない。(II巻p13,4)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.RareBasOctaveOnSustain: sb.AppendFormat("{0}: 例外的。第{1}音の予備。Basのオクターブ跳躍は保留に準ずる。(別巻課題の実施p159 注)\r\n", VerdictValueToString(v.value), v.ordinal0);
                    break;
                case VerdictReason.WrongIIp16_5: sb.AppendFormat("{0}: 長調のII7の和音のBasの第5音が予備されていない。(II巻p16,5)(公理D2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.WrongIIp12_3: sb.AppendFormat("{0}: II7の後続和音はD諸和音。(IIp12,3)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.WrongIIp12_3_V: sb.AppendFormat("{0}: II7→Vの和音 II7の第7音({1})は2度下行する。(公理B2)(IIp12,3)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.WrongIIp12_3_I: sb.AppendFormat("{0}: II7→[2転]I II7の第7音({1})は保留する。(公理B2)(IIp12,3)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.WrongIIp12_3_II: sb.AppendFormat("{0}: II7の上3声の根音はIやVIIに下行しない。(IIp13,3)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.RuleIp73_43: sb.AppendFormat("{0}: [2転]V7→上3声に第3音を含む[1転]I→□の進行の制限(I巻p73,43) cf.(II巻課題4-1)(II巻課題24-1)(II巻課題30-10)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.RareIp54: sb.AppendFormat("{0}: [1転]II→[1転]Vは稀である。(I巻p54,31)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.RareIp128_4: sb.AppendFormat("{0}:[1転]V9または[3転]V9のV({1})が先行和音から保留されないのは稀である。(I巻p128,4)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.RareIh06_03: sb.AppendFormat("{0}: [2転]V7根省 上部構成音 a) (別巻課題の実施 I巻補充課題6-3) cf.(I巻p81,49)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.RareIk44_5: sb.AppendFormat("{0}: [2転]I→V7でBasが8度上行。(別巻課題の実施 I巻課題44-5) cf.(I巻p61,35)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.RareIk44_6: sb.AppendFormat("{0}: 先行和音→[2転]V7 上部構成音b) (別巻課題の実施 I巻課題44-6) cf.(I巻p82,49 2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.RareIh09_5: sb.AppendFormat("{0}: 先行和音→[3転]V7 上3声に共通音IVがふくまれない場合で、上3声があまり下行していない (別巻課題の実施 I巻補充課題9-5) cf.(I巻p74,44)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.RareIIp33_17_1: sb.AppendFormat("{0}: [2転]V_V7の上部構成音 a) ほとんど用いない。(II巻p33,17 1)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.RareIIp39_19_2: sb.AppendFormat("{0}: [基]V_V 3和音は[2転]Iに進むことはまれである。(II巻p39,19 2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.RareIp61_b: sb.AppendFormat("{0}: 例外的な[基]I→[2転]IVの連結。Basがオクターブ跳躍(II巻補充課題3-10 注2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.RarePitch: sb.AppendFormat("{0}: {1}の音域が定められた音域の範囲外である。(I巻p18,7)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.RareInterval: sb.AppendFormat("{0}: {1}-{2}間の離隔。(別巻課題の実施 II巻補充課題9-2 p149 注)\r\n", VerdictValueToString(v.value), v.part0, v.part1);
                    break;
                case VerdictReason.RareStartChord: sb.AppendFormat("{0}: 開始和音が標準外配置。(別巻課題の実施p94 注3)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.RareIIp61_23_2b: sb.AppendFormat("{0}: 長調のV_V下変和音に固有のV9の和音が後続するのはまれ。(II巻p61,23 2b)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.RareIIp62_23_3: sb.AppendFormat("{0}: 長調の固有のV_V9根省下変和音の使用はまれ。(II巻p62,23 3)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.ExceptionalTermination: sb.AppendFormat("{0}: 例外的終止 (I巻p67 注1) cf.(I巻p40,24)(II巻p94)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.WrongIIp22_11: sb.AppendFormat("{0}: 対斜({1}と{2})。(II巻p22,11)\r\n", VerdictValueToString(v.value), v.part0, v.part1);
                    break;
                case VerdictReason.AcceptableIIp22_11: sb.AppendFormat("{0}: 対斜({1}と{2})。許容される。後続和音が減7の和音か、減7の和音の変位によって生じた和音。(II巻p22,11)(II巻p61,23)(公理B3付則)\r\n", VerdictValueToString(v.value), v.part0, v.part1);
                    break;
                case VerdictReason.AcceptableIIp35_18: sb.AppendFormat("{0}: 対斜({1}と{2})。許容される。[基]V_Vの和音→[3転]V7または[1転]V_Vの和音→[基]V7。(II巻p35,18)(公理B3付則)\r\n", VerdictValueToString(v.value), v.part0, v.part1);
                    break;
                case VerdictReason.AcceptableIIIp134_53c: sb.AppendFormat("{0}: 対斜({1}と{2})。後続音が第9音なので許容される。(III巻p134,53 2 c)\r\n", VerdictValueToString(v.value), v.part0, v.part1);
                    break;
                case VerdictReason.InfoIIp77_30_2: sb.AppendFormat("{0}: 対斜({1}と{2})。先行和音が-IIの和音で、対斜が起きている声部が根音なので許容される。(II巻p77,30 2)\r\n", VerdictValueToString(v.value), v.part0, v.part1);
                    break;
                case VerdictReason.AcceptableIIk27_7: sb.AppendFormat("{0}: IVの和音→[1転]V7の連結で、共通音IVを保留し、他の1音は2度上行、もう1音は下行。 (II巻課題24-7) cf.(I巻p.74,44)\r\n", VerdictValueToString(v.value), v.part0, v.part1);
                    break;
                case VerdictReason.AcceptableIIh7_01: sb.AppendFormat("{0}: 先行和音→II7の和音で、配分転換。以下に用例あり:(II巻課題4-12)(II巻補充課題1-1)(II巻補充課題1-2)(II巻補充課題7-1)\r\n", VerdictValueToString(v.value), v.part0, v.part1);
                    break;
                case VerdictReason.WrongIIp25_13: sb.AppendFormat("{0}: 準固有和音→固有和音に進めない。(II巻p25,13)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.WrongIIp36_18_2: sb.AppendFormat("{0}: II7→V_Vの和音 II7の第7音を保留していない。(IIp36,18 2))\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.WrongIIp37_18: sb.AppendFormat("{0}: {1} Vへの短2度上行以外の進行は不良。(IIp37,18)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.WrongIIp68_25_2: sb.AppendFormat("{0}: IV7→後続和音の連結で第7音が2度下行していない(IIp68,25 2)(IIp68,25 3)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.WrongIIp70_25_4: sb.AppendFormat("{0}: 先行和音→IV7の和音の連結で、第7音({1})が予備されていない。(IIp70,25,4)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.WrongIIp70_25_5: sb.AppendFormat("{0}: 先行和音→○IV7の和音 先行和音が○VI7の和音でない。(IIp70,25,5)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.WrongIIp70_26: sb.AppendFormat("{0}: 先行和音→[2転]IV7の和音の連結で低音4度の予備がなされていない。(IIp70,26)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.RareIIp70_26: sb.AppendFormat("{0}: [基]I→[2転]IV7の和音の連結で、Basがオクターブ上行。(II巻補充課題の実施10-11その1) cf.(IIp70,26)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.InfoBp87_1: sb.AppendFormat("{0}: Sopの↓VIが不自然にきこえることがある。(別巻課題の実施p87注2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.InfoBp87_2: sb.AppendFormat("{0}: Sopの↓VIが不自然にきこえることがある。(別巻課題の実施p87注5)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.InfoPositionOfAChordChanged: sb.AppendFormat("{0}: 配分転換が生じている。\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.InvalidAlteredChord: sb.AppendFormat("{0}: 変位和音に、肝心の変位音が含まれない。\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.UnusedLoweredV: sb.AppendFormat("{0}: ほとんど用いない形体。(II巻p59,23 1)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.IV7Upper3A: sb.AppendFormat("{0}: [基]IV7 上部構成音a) (II巻p67,25)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.IV7Upper3B: sb.AppendFormat("{0}: [基]IV7 上部構成音b) (II巻p67,25)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.RareDorian: sb.AppendFormat("{0}: ドリアの和音 あまり用いられない形体 (II巻p73,27)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.DorianBestPosition: sb.AppendFormat("{0}: ドリアの和音 最適配置(II巻p74,28 1)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.DorianNotBestPosition: sb.AppendFormat("{0}: ドリアの和音 最適配置ではない配置(II巻p74,28 1)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIIp76_28_3: sb.AppendFormat("{0}: 先行和音→ドリアのIV7の和音の連結(II巻p76,28 3)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.WrongIIp74_28_2_1: sb.AppendFormat("{0}: ドリアのIV7の和音の第3音の進行ルールを守っていない {1} (II巻p76,28 2)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.WrongIIp74_28_2_2: sb.AppendFormat("{0}: ドリアのIV7の和音の第7音の進行ルールを守っていない {1} (II巻p76,28 2)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.GoodIIp74_28_2_3: sb.AppendFormat("{0}: ドリアのIV7の和音→V諸和音の連結 (II巻p74,28 2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.WrongIIp74_28_2_3: sb.AppendFormat("{0}: ドリアのIV7の和音はV諸和音以外に進むことはない。(II巻p74,28 2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.RareNapolitan: sb.AppendFormat("{0}: -II (ナポリの六)の和音 あまり用いられない形体(II巻p76,30 1)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.BestNapolitan: sb.AppendFormat("{0}: -II (ナポリの六)の和音 最適配置(II巻p76,30 1)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.OkayNapolitan: sb.AppendFormat("{0}: -II (ナポリの六)の和音 最適ではない配置(II巻p76,30 1)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodOct5Napolitan: sb.AppendFormat("{0}: -II (ナポリの六)の和音 十分用いうる配置(別巻課題の実施II巻課題34-3 注)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.IIp77_30_2: sb.AppendFormat("{0}: -II (ナポリの六)の和音→D諸和音の連結 (II巻p77,30 2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.IIp77_30_3: sb.AppendFormat("{0}: -II (ナポリの六)の和音→S和音の連結 (II巻p77,30 3)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.WrongIIp77_30: sb.AppendFormat("{0}: -II (ナポリの六)の和音→後続和音の連結 (II巻p77,30)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.IIk34_3: sb.AppendFormat("{0}: 先行和音→[1転]-IIOct(5)の連結。(別巻課題の実施II巻課題34-3 注)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.BestAddedSix: sb.AppendFormat("{0}: 付加6の和音。最適配置。(II巻p90,39)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.BestAddedSixFour: sb.AppendFormat("{0}: 付加4-6の和音。最適配置。(II巻p92,42)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.UnusedAddedSixFour: sb.AppendFormat("{0}: 長調の固有の付加4-6の和音はほとんど用いられない。(II巻p92,41)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIIp90_39_3: sb.AppendFormat("{0}: 先行和音→IV付加6の和音。(II巻p90,39 3)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIIp90_39_3: sb.AppendFormat("{0}: 先行和音→IV付加6の和音。{1}を予備していない。(II巻p90,39 3)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.InfoIIp90_39_2_1: sb.AppendFormat("{0}: IV付加6の和音→後続和音の連結 後続和音は[基]Iであると(II巻p90,39 2)にあるが、(III巻p222,78)にはいかなるT和音でも良いとある。\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIIp90_39_2_2: sb.AppendFormat("{0}: IV付加6の和音→後続和音の連結(II巻p90,39 2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIIp90_39_2_3: sb.AppendFormat("{0}: IV付加6の和音→後続和音の連結 限定進行ルールを守っていない。(II巻p90,39 2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodtoIV46: sb.AppendFormat("{0}: T和音→IV付加4-6の和音の連結。(別巻p.165 II巻補充課題12-10)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIIp93_42_2_1: sb.AppendFormat("{0}: IV付加4-6の和音→後続和音の連結 後続和音は[基]Iである。(II巻p93,42 2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIIp93_42_2_2: sb.AppendFormat("{0}: IV付加4-6の和音→後続和音の連結(II巻p93,42 2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIIp93_42_2_3: sb.AppendFormat("{0}: IV付加4-6の和音→後続和音の連結 第4音、第6音、第3音の限定進行ルールを守っていない。(II巻p93,42 2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.InfoIIp98_47_a: sb.AppendFormat("{0}: 転調進行 上3声に半音階的関係をなす2音が含まれる場合。(II巻p98,47 a)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.InfoIIp98_47_a2: sb.AppendFormat("{0}: 転調進行 対斜({1}と{2})が起きているが、後続和音が減7の和音なので許容される。(II巻p98,47 a)\r\n", VerdictValueToString(v.value), v.part0, v.part1);
                    break;
                case VerdictReason.GoodIIp98_47_b: sb.AppendFormat("{0}: 転調進行 共通音が含まれる場合。(II巻p98,47 b)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIIp98_47_b: sb.AppendFormat("{0}: 転調進行 上3声に共通音が含まれる場合、保留する。(II巻p98,47 b)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIIp98_47_c: sb.AppendFormat("{0}: 転調進行 上3声に共通音が含まれない場合。(II巻p98,47 c)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIIp98_47_c: sb.AppendFormat("{0}: 転調進行 上3声に共通音が含まれない場合。(II巻p98,47 c)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AcceptableIIp99_47_d: sb.AppendFormat("{0}: 転調進行 {1}の増2度上行だが、許される。(公理B1付則)(II巻p99,47 d)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.GoodIIp99_47_e: sb.AppendFormat("{0}: 転調進行 離脱和音の限定進行音{1}の取り扱い。(II巻p99,47 e)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.AvoidIIp99_47_e: sb.AppendFormat("{0}: 転調進行 離脱和音の限定進行音{1}が、正規の限定進行も保留も増1度進行もしていない。(II巻p99,47 e)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.AcceptableIIp100_47_e: sb.AppendFormat("{0}: 転調進行 離脱和音の導音{1}の跳躍上行。許される。(II巻p100,47 e)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.GoodIIp100_47_f: sb.AppendFormat("{0}: 転調進行 転入和音の第7音{1}が正しく予備されている。(II巻p100,47 f)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.AvoidIIp100_47_f: sb.AppendFormat("{0}: 転調進行 転入和音の第7音{1}の予備が必要。(II巻p100,47 f)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.NotSoGoodBas6Progression: sb.AppendFormat("{0}: (先行和音が終止ではない場合の)Basの6度の進行は、I→VIの上行、I→IIIの下行以外はなるべく避けよ。(II巻p122,55 3 a)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.RareBas6Progression: sb.AppendFormat("{0}: Basの6度上行。後続和音が[3転]V9根省和音の場合はまれに見られる。(I巻補充課題9-5)(II巻補充課題2-3)(II巻補充課題3-8)(II巻補充課題5-6) cf.(II巻p122,55 3 a)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.NotSoGoodBas6Progression2: sb.AppendFormat("{0}: Basの6度跳躍の次は反対方向への進行をするのがよい。(II巻p123,55 3 b)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.NotSoGoodBas5ProgInv7: sb.AppendFormat("{0}: Basが5度下行で第7音に到達している。(II巻p123,55 3 c)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.NotSoGoodBas5ProgLeadingNote: sb.AppendFormat("{0}: Basが5度上行で導音に到達している。(II巻p123,55 3 c)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.NotSoGoodBasTooFarProgression: sb.AppendFormat("{0}: Basが2回連続して跳躍し、合計した音程が{1}度。(II巻p123,55 3 d)\r\n", VerdictValueToString(v.value), v.ordinal0);
                    break;
                case VerdictReason.InfoIIp114: sb.AppendFormat("{0}: D諸和音 ソプラノ定型{1}:バス定型{2} (II巻p114,52)\r\n", VerdictValueToString(v.value), v.str0, v.str1);
                    break;
                case VerdictReason.InfoIIp95_2: sb.AppendFormat("{0}: Basの動きが{1}の終止定式{2}。和音構成が({3})→({4})であれば{1}になりうる。(II巻p95,44 4)\r\n", VerdictValueToString(v.value), v.str0, v.str1, v.str2, v.str3);
                    break;
                case VerdictReason.InfoIIp95_3: sb.AppendFormat("{0}: Basの動きが{1}の終止定式{2}。和音構成が({3})→({4})→({5})であれば{1}になりうる。(II巻p95,44 4)\r\n", VerdictValueToString(v.value), v.str0, v.str1, v.str2, v.str3, v.str4);
                    break;
                case VerdictReason.InfoIIp95_4: sb.AppendFormat("{0}: Basの動きが{1}の終止定式{2}。和音構成が({3})→({4})→({5})→({6})であれば{1}になりうる。(II巻p95,44 4)\r\n", VerdictValueToString(v.value), v.str0, v.str1, v.str2, v.str3, v.str4, v.str5);
                    break;
                case VerdictReason.InfoIIp117_2: sb.AppendFormat("{0}: Sopの動きが{1}の終止定式{2}。和音構成が({3})→({4})であれば{1}になりうる。(II巻p117,54)\r\n", VerdictValueToString(v.value), v.str0, v.str1, v.str2, v.str3);
                    break;
                case VerdictReason.InfoIIp117_3: sb.AppendFormat("{0}: Sopの動きが{1}の終止定式{2}。和音構成が({3})→({4})→({5})であれば{1}になりうる。(II巻p117,54)\r\n", VerdictValueToString(v.value), v.str0, v.str1, v.str2, v.str3, v.str4);
                    break;
                case VerdictReason.InfoIIp117_4: sb.AppendFormat("{0}: Sopの動きが{1}の終止定式{2}。和音構成が({3})→({4})→({5})→({6})であれば{1}になりうる。(II巻p117,54)\r\n", VerdictValueToString(v.value), v.str0, v.str1, v.str2, v.str3, v.str4, v.str5);
                    break;
                case VerdictReason.InfoIIp117chordOK: sb.AppendFormat("{0}: 和音進行はSop終止定型{1}の通り。Basは終止定型進行していない。したがってこの地点は{2}になりえない。(II巻p117,54)\r\n", VerdictValueToString(v.value), v.str0, v.str1);
                    break;
                case VerdictReason.InfoIIp117chordNotOK: sb.AppendFormat("{0}: 和音進行がSop終止定型{1}の通りではない。Basは終止定型進行していない。したがってこの地点は{2}になりえない。(II巻p117,54)\r\n", VerdictValueToString(v.value), v.str0, v.str1);
                    break;
                case VerdictReason.InfoIIp117chordOKBasOK: sb.AppendFormat("{0}: 和音進行はSop終止定型{1}の通り。Bas定型{2}はSop終止定型{1}に対応するものである。したがってこの地点は{3}になりうる。(II巻p117,54)\r\n", VerdictValueToString(v.value), v.str0, v.str1, v.str2);
                    break;
                case VerdictReason.InfoIIp117chordNotOKBasOK: sb.AppendFormat("{0}: 和音進行はSop終止定型{1}の通りではない。Bas定型{2}はSop終止定型{1}に対応するものである。(II巻p117,54)\r\n", VerdictValueToString(v.value), v.str0, v.str1, v.str2);
                    break;
                case VerdictReason.InfoIIp117chordOKBasNotOK: sb.AppendFormat("{0}: 和音進行はSop終止定型{1}の通り。Basの動きはSop終止定型{1}に対応するものではない。(II巻p117,54)\r\n", VerdictValueToString(v.value), v.str0, v.str1);
                    break;
                case VerdictReason.InfoIIp117chordNotOKBasNotOK: sb.AppendFormat("{0}: 和音進行はSop終止定型{1}の通りではない。Basの動きはSop終止定型{1}に対応するものではない。(II巻p117,54)\r\n", VerdictValueToString(v.value), v.str0, v.str1);
                    break;
                case VerdictReason.InfoIIp117TerminationDoesNotMatch: sb.AppendFormat("{0}: 終止になりえない箇所に終止記号がつけられている。(II巻p117,54)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.NotSoGoodIIp121_55_2_a: sb.AppendFormat("{0}: 同型の対応バス定型が連続している。(II巻p122,55 2 a)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.IIk50_12Progression: sb.AppendFormat("{0}: I→[1転]V7(上部構成音a)で、I巻p74のルールを守っていない。(II巻課題50-12)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.InfoIIIp212_76_3: sb.AppendFormat("{0}: V→IIIのVの第3音{1}は限定進行しなくて良い。(III巻p212 76 3)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.RareIIIp215_76_6_1: sb.AppendFormat("{0}: [基]VI→[1転]I。(III巻p215 76 6)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.WrongIIIp218_77_3_1: sb.AppendFormat("{0}: I7 III7 VI7 VII7の和音の第7音({1})は限定進行する (III巻p218 77 3)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.WrongIIIp218_77_3_2: sb.AppendFormat("{0}: I7 III7 VI7 VII7の和音の第7音({1})には予備で到達する (III巻p218 77 3)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.GoodIIIp218_77_3_4: sb.AppendFormat("{0}: 7の和音を含む連結で共通音がない場合にBasとSopが反行していなくても良い(III巻p218 77 3)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIIIp218_77_3_5: sb.AppendFormat("{0}: V7→I7 (III巻p218 77 3)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIIIp219_77_3_6: sb.AppendFormat("{0}: □→[2転]□7 後続和音のBas-{1}間の低音4度が予備されていない(III巻p219 77 3)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.AvoidIIIp219_77_3_7: sb.AppendFormat("{0}: [2転]□7→X Basが2度下行していない(III巻p219 77 3)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.ProhibitedIIIp219_77_3_8: sb.AppendFormat("{0}: 7の和音の並達8度 {1}と{2}(III巻p219 77 3)(公理C5)\r\n", VerdictValueToString(v.value), v.part0, v.part1);
                    break;
                case VerdictReason.AcceptableIIIp219_77_3_9: sb.AppendFormat("{0}: 導音の2度下行(Sop)。許される。(III巻p219 77 3)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIIIp219_77_3_10: sb.AppendFormat("{0}: V3和音またはV7の和音の導音({1})は跳躍進行は許されない。(III巻p219 77 3)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.GoodIIIp220_77_4_1: sb.AppendFormat("{0}: [□]I7→X または [□]III7→X または [□]VI7→X または [□]VII7→X (III巻p220 77 4)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIIIp220_77_4_2: sb.AppendFormat("{0}: X→[□]I7 または X→[□]III7 または X→[□]VI7 または X→[□]VII7 (III巻p220 77 4)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.GoodIIIp221_77_5_2: sb.AppendFormat("{0}: 短調のV→III7の和音。先行和音は固有のVIIを持つVの和音が良い。(III巻p221 77 5)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.RareIIIp221_77_5_2: sb.AppendFormat("{0}: 短調のV→III7の和音。先行和音は固有のVIIを持つVの和音が良い。(III巻p221 77 5)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.IIIp222_77_5_3: sb.AppendFormat("{0}: 短調固有のVIIを持つV7の和音の第7音{1}の予備(III巻p222 77 5)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.IIIp223_78: sb.AppendFormat("{0}: IV+6→[1転]I 第5音は保留、第6音は4度上行する。(III巻p223 78)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.UnusedRaisedV_V: sb.AppendFormat("{0}: V_Vの上方変位。3和音しか用いない。(III巻p226 79 4)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.UnusedRaisedV7: sb.AppendFormat("{0}: V7の上方変位和音の根音省略形体は4声体では用いない。(III巻p226 79 2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.RuleA5_2: sb.AppendFormat("{0}: 変位第5音{1}と第7音{2}が単音程減3度。(III巻p224 79 3a)(公理A5)\r\n", VerdictValueToString(v.value), v.part0, v.part1);
                    break;
                case VerdictReason.OkayRaisedV: sb.AppendFormat("{0}: 最適ではない配置の上方変位Vの和音。(III巻p224 79 3a)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIIIp225_79_3_b_1: sb.AppendFormat("{0}: V上方変位和音 {1}が限定進行ルールに従っていない。(III巻p224 79 3a)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.RareRaisedV: sb.AppendFormat("{0}: 短調のV上変和音。転調進行の離脱和音以外には用いられない。(III巻p224 79 2)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.AvoidIIIp226_79_5_1: sb.AppendFormat("{0}: IV+6 IV+46の上方変位和音 第6音{1}が2度上行していない。(III巻p226 79 5)\r\n", VerdictValueToString(v.value), v.part0);
                    break;
                case VerdictReason.RareRaisedIV: sb.AppendFormat("{0}: 短調のIV+6 IV+46上変和音。あまり用いられない。(III巻p224 79 5)\r\n", VerdictValueToString(v.value));
                    break;
                case VerdictReason.InfoChord: sb.AppendFormat("{0}。{1}\r\n", v.str0, v.str1);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false);
                    break;
                }
            }
            return sb.ToString();
        }
    }
}
