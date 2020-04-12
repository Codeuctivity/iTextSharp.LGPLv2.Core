using iTextSharp.text.xml.simpleparser;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.util;

namespace iTextSharp.text.pdf
{
    /// <summary>
    /// @author Paulo Soares (psoares@consiste.pt)
    /// </summary>
    public sealed class SimpleNamedDestination : ISimpleXmlDocHandler
    {
        private Hashtable _xmlLast;
        private Hashtable _xmlNames;

        private SimpleNamedDestination()
        {
        }

        public static string EscapeBinaryString(string s)
        {
            var buf = new StringBuilder();
            var cc = s.ToCharArray();
            var len = cc.Length;
            for (var k = 0; k < len; ++k)
            {
                var c = cc[k];
                if (c < ' ')
                {
                    buf.Append('\\');
                    ((int)c).ToString("", System.Globalization.CultureInfo.InvariantCulture);
                    var octal = "00" + Convert.ToString(c, 8);
                    buf.Append(octal.Substring(octal.Length - 3));
                }
                else if (c == '\\')
                {
                    buf.Append("\\\\");
                }
                else
                {
                    buf.Append(c);
                }
            }
            return buf.ToString();
        }

        /// <summary>
        /// Exports the destinations to XML. The DTD for this XML is:
        ///
        ///
        /// &lt;?xml version='1.0' encoding='UTF-8'?&gt;
        /// &lt;!ELEMENT Name (#PCDATA)&gt;
        /// &lt;!ATTLIST Name
        /// Page CDATA #IMPLIED
        /// &gt;
        /// &lt;!ELEMENT Destination (Name)*&gt;
        ///
        /// whatever the encoding
        /// @throws IOException on error
        /// </summary>
        /// <param name="names">the names</param>
        /// <param name="outp">the export destination. The stream is not closed</param>
        /// <param name="encoding">the encoding according to IANA conventions</param>
        /// <param name="onlyAscii">codes above 127 will always be escaped with &amp;#nn; if  true ,</param>
        public static void ExportToXml(Hashtable names, Stream outp, string encoding, bool onlyAscii)
        {
            var wrt = new StreamWriter(outp, IanaEncodings.GetEncodingEncoding(encoding));
            ExportToXml(names, wrt, encoding, onlyAscii);
        }

        /// <summary>
        /// Exports the bookmarks to XML.
        /// whatever the encoding
        /// @throws IOException on error
        /// </summary>
        /// <param name="names">the names</param>
        /// <param name="wrt">the export destination. The writer is not closed</param>
        /// <param name="encoding">the encoding according to IANA conventions</param>
        /// <param name="onlyAscii">codes above 127 will always be escaped with &amp;#nn; if  true ,</param>
        public static void ExportToXml(Hashtable names, TextWriter wrt, string encoding, bool onlyAscii)
        {
            wrt.Write("<?xml version=\"1.0\" encoding=\"");
            wrt.Write(SimpleXmlParser.EscapeXml(encoding, onlyAscii));
            wrt.Write("\"?>\n<Destination>\n");
            foreach (string key in names.Keys)
            {
                var value = (string)names[key];
                wrt.Write("  <Name Page=\"");
                wrt.Write(SimpleXmlParser.EscapeXml(value, onlyAscii));
                wrt.Write("\">");
                wrt.Write(SimpleXmlParser.EscapeXml(EscapeBinaryString(key), onlyAscii));
                wrt.Write("</Name>\n");
            }
            wrt.Write("</Destination>\n");
            wrt.Flush();
        }

        public static Hashtable GetNamedDestination(PdfReader reader, bool fromNames)
        {
            var pages = new IntHashtable();
            var numPages = reader.NumberOfPages;
            for (var k = 1; k <= numPages; ++k)
            {
                pages[reader.GetPageOrigRef(k).Number] = k;
            }

            var names = fromNames ? reader.GetNamedDestinationFromNames() : reader.GetNamedDestinationFromStrings();
            var keys = new string[names.Count];
            names.Keys.CopyTo(keys, 0);
            foreach (var name in keys)
            {
                var arr = (PdfArray)names[name];
                var s = new StringBuilder();
                try
                {
                    s.Append(pages[(arr.GetAsIndirectObject(0)).Number]);
                    s.Append(' ').Append(arr[1].ToString().Substring(1));
                    for (var k = 2; k < arr.Size; ++k)
                    {
                        s.Append(' ').Append(arr[k]);
                    }

                    names[name] = s.ToString();
                }
                catch
                {
                    names.Remove(name);
                }
            }
            return names;
        }

        /// <summary>
        /// Import the names from XML.
        /// @throws IOException on error
        /// </summary>
        /// <param name="inp">the XML source. The stream is not closed</param>
        /// <returns>the names</returns>
        public static Hashtable ImportFromXml(Stream inp)
        {
            var names = new SimpleNamedDestination();
            SimpleXmlParser.Parse(names, inp);
            return names._xmlNames;
        }

        /// <summary>
        /// Import the names from XML.
        /// @throws IOException on error
        /// </summary>
        /// <param name="inp">the XML source. The reader is not closed</param>
        /// <returns>the names</returns>
        public static Hashtable ImportFromXml(TextReader inp)
        {
            var names = new SimpleNamedDestination();
            SimpleXmlParser.Parse(names, inp);
            return names._xmlNames;
        }

        public static PdfDictionary OutputNamedDestinationAsNames(Hashtable names, PdfWriter writer)
        {
            var dic = new PdfDictionary();
            foreach (string key in names.Keys)
            {
                try
                {
                    var value = (string)names[key];
                    var ar = CreateDestinationArray(value, writer);
                    var kn = new PdfName(key);
                    dic.Put(kn, ar);
                }
                catch
                {
                    // empty on purpose
                }
            }
            return dic;
        }

        public static PdfDictionary OutputNamedDestinationAsStrings(Hashtable names, PdfWriter writer)
        {
            var n2 = new Hashtable();
            foreach (string key in names.Keys)
            {
                try
                {
                    var value = (string)names[key];
                    var ar = CreateDestinationArray(value, writer);
                    n2[key] = writer.AddToBody(ar).IndirectReference;
                }
                catch
                {
                    // empty on purpose
                }
            }
            return PdfNameTree.WriteTree(n2, writer);
        }

        public static string UnEscapeBinaryString(string s)
        {
            var buf = new StringBuilder();
            var cc = s.ToCharArray();
            var len = cc.Length;
            for (var k = 0; k < len; ++k)
            {
                var c = cc[k];
                if (c == '\\')
                {
                    if (++k >= len)
                    {
                        buf.Append('\\');
                        break;
                    }
                    c = cc[k];
                    if (c >= '0' && c <= '7')
                    {
                        var n = c - '0';
                        ++k;
                        for (var j = 0; j < 2 && k < len; ++j)
                        {
                            c = cc[k];
                            if (c >= '0' && c <= '7')
                            {
                                ++k;
                                n = n * 8 + c - '0';
                            }
                            else
                            {
                                break;
                            }
                        }
                        --k;
                        buf.Append((char)n);
                    }
                    else
                    {
                        buf.Append(c);
                    }
                }
                else
                {
                    buf.Append(c);
                }
            }
            return buf.ToString();
        }

        public void EndDocument()
        {
        }

        public void EndElement(string tag)
        {
            if (tag.Equals("Destination"))
            {
                if (_xmlLast == null && _xmlNames != null)
                {
                    return;
                }
                else
                {
                    throw new ArgumentException("Destination end tag out of place.");
                }
            }
            if (!tag.Equals("Name"))
            {
                throw new ArgumentException("Invalid end tag - " + tag);
            }

            if (_xmlLast == null || _xmlNames == null)
            {
                throw new ArgumentException("Name end tag out of place.");
            }

            if (!_xmlLast.ContainsKey("Page"))
            {
                throw new ArgumentException("Page attribute missing.");
            }

            _xmlNames[UnEscapeBinaryString((string)_xmlLast["Name"])] = _xmlLast["Page"];
            _xmlLast = null;
        }

        public void StartDocument()
        {
        }

        public void StartElement(string tag, Hashtable h)
        {
            if (_xmlNames == null)
            {
                if (tag.Equals("Destination"))
                {
                    _xmlNames = new Hashtable();
                    return;
                }
                else
                {
                    throw new ArgumentException("Root element is not Destination.");
                }
            }
            if (!tag.Equals("Name"))
            {
                throw new ArgumentException("Tag " + tag + " not allowed.");
            }

            if (_xmlLast != null)
            {
                throw new ArgumentException("Nested tags are not allowed.");
            }

            _xmlLast = new Hashtable(h)
            {
                ["Name"] = ""
            };
        }

        public void Text(string str)
        {
            if (_xmlLast == null)
            {
                return;
            }

            var name = (string)_xmlLast["Name"];
            name += str;
            _xmlLast["Name"] = name;
        }

        internal static PdfArray CreateDestinationArray(string value, PdfWriter writer)
        {
            var ar = new PdfArray();
            var tk = new StringTokenizer(value);
            var n = int.Parse(tk.NextToken());
            ar.Add(writer.GetPageReference(n));
            if (!tk.HasMoreTokens())
            {
                ar.Add(PdfName.Xyz);
                ar.Add(new float[] { 0, 10000, 0 });
            }
            else
            {
                var fn = tk.NextToken();
                if (fn.StartsWith("/"))
                {
                    fn = fn.Substring(1);
                }

                ar.Add(new PdfName(fn));
                for (var k = 0; k < 4 && tk.HasMoreTokens(); ++k)
                {
                    fn = tk.NextToken();
                    if (fn.Equals("null"))
                    {
                        ar.Add(PdfNull.Pdfnull);
                    }
                    else
                    {
                        ar.Add(new PdfNumber(fn));
                    }
                }
            }
            return ar;
        }
    }
}