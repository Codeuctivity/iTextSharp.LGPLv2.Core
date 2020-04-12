using System;
using System.Collections;

namespace iTextSharp.text.xml.simpleparser
{

    /// <summary>
    /// This class contains entities that can be used in an entity tag.
    /// </summary>

    public class EntitiesToSymbol
    {

        /// <summary>
        /// This is a map that contains all possible id values of the entity tag
        /// that can be translated to a character in font Symbol.
        /// </summary>
        public static readonly Hashtable Map;

        static EntitiesToSymbol()
        {
            Map = new Hashtable
            {
                ["169"] = (char)227,
                ["172"] = (char)216,
                ["174"] = (char)210,
                ["177"] = (char)177,
                ["215"] = (char)180,
                ["247"] = (char)184,
                ["8230"] = (char)188,
                ["8242"] = (char)162,
                ["8243"] = (char)178,
                ["8260"] = (char)164,
                ["8364"] = (char)240,
                ["8465"] = (char)193,
                ["8472"] = (char)195,
                ["8476"] = (char)194,
                ["8482"] = (char)212,
                ["8501"] = (char)192,
                ["8592"] = (char)172,
                ["8593"] = (char)173,
                ["8594"] = (char)174,
                ["8595"] = (char)175,
                ["8596"] = (char)171,
                ["8629"] = (char)191,
                ["8656"] = (char)220,
                ["8657"] = (char)221,
                ["8658"] = (char)222,
                ["8659"] = (char)223,
                ["8660"] = (char)219,
                ["8704"] = (char)34,
                ["8706"] = (char)182,
                ["8707"] = (char)36,
                ["8709"] = (char)198,
                ["8711"] = (char)209,
                ["8712"] = (char)206,
                ["8713"] = (char)207,
                ["8717"] = (char)39,
                ["8719"] = (char)213,
                ["8721"] = (char)229,
                ["8722"] = (char)45,
                ["8727"] = (char)42,
                ["8729"] = (char)183,
                ["8730"] = (char)214,
                ["8733"] = (char)181,
                ["8734"] = (char)165,
                ["8736"] = (char)208,
                ["8743"] = (char)217,
                ["8744"] = (char)218,
                ["8745"] = (char)199,
                ["8746"] = (char)200,
                ["8747"] = (char)242,
                ["8756"] = (char)92,
                ["8764"] = (char)126,
                ["8773"] = (char)64,
                ["8776"] = (char)187,
                ["8800"] = (char)185,
                ["8801"] = (char)186,
                ["8804"] = (char)163,
                ["8805"] = (char)179,
                ["8834"] = (char)204,
                ["8835"] = (char)201,
                ["8836"] = (char)203,
                ["8838"] = (char)205,
                ["8839"] = (char)202,
                ["8853"] = (char)197,
                ["8855"] = (char)196,
                ["8869"] = (char)94,
                ["8901"] = (char)215,
                ["8992"] = (char)243,
                ["8993"] = (char)245,
                ["9001"] = (char)225,
                ["9002"] = (char)241,
                ["913"] = (char)65,
                ["914"] = (char)66,
                ["915"] = (char)71,
                ["916"] = (char)68,
                ["917"] = (char)69,
                ["918"] = (char)90,
                ["919"] = (char)72,
                ["920"] = (char)81,
                ["921"] = (char)73,
                ["922"] = (char)75,
                ["923"] = (char)76,
                ["924"] = (char)77,
                ["925"] = (char)78,
                ["926"] = (char)88,
                ["927"] = (char)79,
                ["928"] = (char)80,
                ["929"] = (char)82,
                ["931"] = (char)83,
                ["932"] = (char)84,
                ["933"] = (char)85,
                ["934"] = (char)70,
                ["935"] = (char)67,
                ["936"] = (char)89,
                ["937"] = (char)87,
                ["945"] = (char)97,
                ["946"] = (char)98,
                ["947"] = (char)103,
                ["948"] = (char)100,
                ["949"] = (char)101,
                ["950"] = (char)122,
                ["951"] = (char)104,
                ["952"] = (char)113,
                ["953"] = (char)105,
                ["954"] = (char)107,
                ["955"] = (char)108,
                ["956"] = (char)109,
                ["957"] = (char)110,
                ["958"] = (char)120,
                ["959"] = (char)111,
                ["960"] = (char)112,
                ["961"] = (char)114,
                ["962"] = (char)86,
                ["963"] = (char)115,
                ["964"] = (char)116,
                ["965"] = (char)117,
                ["966"] = (char)102,
                ["967"] = (char)99,
                ["9674"] = (char)224,
                ["968"] = (char)121,
                ["969"] = (char)119,
                ["977"] = (char)74,
                ["978"] = (char)161,
                ["981"] = (char)106,
                ["982"] = (char)118,
                ["9824"] = (char)170,
                ["9827"] = (char)167,
                ["9829"] = (char)169,
                ["9830"] = (char)168,
                ["Alpha"] = (char)65,
                ["Beta"] = (char)66,
                ["Chi"] = (char)67,
                ["Delta"] = (char)68,
                ["Epsilon"] = (char)69,
                ["Eta"] = (char)72,
                ["Gamma"] = (char)71,
                ["Iota"] = (char)73,
                ["Kappa"] = (char)75,
                ["Lambda"] = (char)76,
                ["Mu"] = (char)77,
                ["Nu"] = (char)78,
                ["Omega"] = (char)87,
                ["Omicron"] = (char)79,
                ["Phi"] = (char)70,
                ["Pi"] = (char)80,
                ["Prime"] = (char)178,
                ["Psi"] = (char)89,
                ["Rho"] = (char)82,
                ["Sigma"] = (char)83,
                ["Tau"] = (char)84,
                ["Theta"] = (char)81,
                ["Upsilon"] = (char)85,
                ["Xi"] = (char)88,
                ["Zeta"] = (char)90,
                ["alefsym"] = (char)192,
                ["alpha"] = (char)97,
                ["and"] = (char)217,
                ["ang"] = (char)208,
                ["asymp"] = (char)187,
                ["beta"] = (char)98,
                ["cap"] = (char)199,
                ["chi"] = (char)99,
                ["clubs"] = (char)167,
                ["cong"] = (char)64,
                ["copy"] = (char)211,
                ["crarr"] = (char)191,
                ["cup"] = (char)200,
                ["dArr"] = (char)223,
                ["darr"] = (char)175,
                ["delta"] = (char)100,
                ["diams"] = (char)168,
                ["divide"] = (char)184,
                ["empty"] = (char)198,
                ["epsilon"] = (char)101,
                ["equiv"] = (char)186,
                ["eta"] = (char)104,
                ["euro"] = (char)240,
                ["exist"] = (char)36,
                ["forall"] = (char)34,
                ["frasl"] = (char)164,
                ["gamma"] = (char)103,
                ["ge"] = (char)179,
                ["hArr"] = (char)219,
                ["harr"] = (char)171,
                ["hearts"] = (char)169,
                ["hellip"] = (char)188,
                ["horizontal arrow extender"] = (char)190,
                ["image"] = (char)193,
                ["infin"] = (char)165,
                ["int"] = (char)242,
                ["iota"] = (char)105,
                ["isin"] = (char)206,
                ["kappa"] = (char)107,
                ["lArr"] = (char)220,
                ["lambda"] = (char)108,
                ["lang"] = (char)225,
                ["large brace extender"] = (char)239,
                ["large integral extender"] = (char)244,
                ["large left brace (bottom)"] = (char)238,
                ["large left brace (middle)"] = (char)237,
                ["large left brace (top)"] = (char)236,
                ["large left bracket (bottom)"] = (char)235,
                ["large left bracket (extender)"] = (char)234,
                ["large left bracket (top)"] = (char)233,
                ["large left parenthesis (bottom)"] = (char)232,
                ["large left parenthesis (extender)"] = (char)231,
                ["large left parenthesis (top)"] = (char)230,
                ["large right brace (bottom)"] = (char)254,
                ["large right brace (middle)"] = (char)253,
                ["large right brace (top)"] = (char)252,
                ["large right bracket (bottom)"] = (char)251,
                ["large right bracket (extender)"] = (char)250,
                ["large right bracket (top)"] = (char)249,
                ["large right parenthesis (bottom)"] = (char)248,
                ["large right parenthesis (extender)"] = (char)247,
                ["large right parenthesis (top)"] = (char)246,
                ["larr"] = (char)172,
                ["le"] = (char)163,
                ["lowast"] = (char)42,
                ["loz"] = (char)224,
                ["minus"] = (char)45,
                ["mu"] = (char)109,
                ["nabla"] = (char)209,
                ["ne"] = (char)185,
                ["not"] = (char)216,
                ["notin"] = (char)207,
                ["nsub"] = (char)203,
                ["nu"] = (char)110,
                ["omega"] = (char)119,
                ["omicron"] = (char)111,
                ["oplus"] = (char)197,
                ["or"] = (char)218,
                ["otimes"] = (char)196,
                ["part"] = (char)182,
                ["perp"] = (char)94,
                ["phi"] = (char)102,
                ["pi"] = (char)112,
                ["piv"] = (char)118,
                ["plusmn"] = (char)177,
                ["prime"] = (char)162,
                ["prod"] = (char)213,
                ["prop"] = (char)181,
                ["psi"] = (char)121,
                ["rArr"] = (char)222,
                ["radic"] = (char)214,
                ["radical extender"] = (char)96,
                ["rang"] = (char)241,
                ["rarr"] = (char)174,
                ["real"] = (char)194,
                ["reg"] = (char)210,
                ["rho"] = (char)114,
                ["sdot"] = (char)215,
                ["sigma"] = (char)115,
                ["sigmaf"] = (char)86,
                ["sim"] = (char)126,
                ["spades"] = (char)170,
                ["sub"] = (char)204,
                ["sube"] = (char)205,
                ["sum"] = (char)229,
                ["sup"] = (char)201,
                ["supe"] = (char)202,
                ["tau"] = (char)116,
                ["there4"] = (char)92,
                ["theta"] = (char)113,
                ["thetasym"] = (char)74,
                ["times"] = (char)180,
                ["trade"] = (char)212,
                ["uArr"] = (char)221,
                ["uarr"] = (char)173,
                ["upsih"] = (char)161,
                ["upsilon"] = (char)117,
                ["vertical arrow extender"] = (char)189,
                ["weierp"] = (char)195,
                ["xi"] = (char)120,
                ["zeta"] = (char)122
            };
        }

        /// <summary>
        /// Gets a chunk with a symbol character.
        /// </summary>
        /// <param name="e">a symbol value (see Entities class: alfa is greek alfa,...)</param>
        /// <param name="font">the font if the symbol isn't found (otherwise Font.SYMBOL)</param>
        /// <returns>a Chunk</returns>

        public static Chunk Get(string e, Font font)
        {
            var s = GetCorrespondingSymbol(e);
            if (s == '\0')
            {
                try
                {
                    return new Chunk("" + (char)int.Parse(e), font);
                }
                catch (Exception)
                {
                    return new Chunk(e, font);
                }
            }
            var symbol = new Font(Font.SYMBOL, font.Size, font.Style, font.Color);
            return new Chunk(s.ToString(), symbol);
        }

        /// <summary>
        /// Looks for the corresponding symbol in the font Symbol.
        /// </summary>
        /// <param name="name">the name of the entity</param>
        /// <returns>the corresponding character in font Symbol</returns>

        public static char GetCorrespondingSymbol(string name)
        {
            if (Map.ContainsKey(name))
            {
                return (char)Map[name];
            }
            else
            {
                return '\0';
            }
        }
    }
}