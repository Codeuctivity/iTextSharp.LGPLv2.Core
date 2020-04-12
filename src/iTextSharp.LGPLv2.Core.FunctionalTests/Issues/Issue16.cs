using iTextSharp.text.pdf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.IO;

namespace iTextSharp.LGPLv2.Core.FunctionalTests.Issues
{
    /// <summary>
    /// https://github.com/VahidN/iTextSharp.LGPLv2.Core/issues/16
    /// </summary>
    [TestClass]
    public class Issue16
    {
        [TestMethod]
        public void Verify_Issue16_CanBe_Processed()
        {
            var pdfFilePath = TestUtils.GetOutputFileName();
            var stream = new FileStream(pdfFilePath, FileMode.Create);

            var path = TestUtils.GetPdfsPath("issue16.pdf");
            var reader = new PdfReader(path);
            var stamper = new PdfStamper(reader, stream);

            var form = stamper.AcroFields;

            foreach (DictionaryEntry field in form.Fields)
            {
                Console.WriteLine(field.Key);
            }

            form.SetField("Text Field0", "Value 1");

            stamper.Close();
            reader.Close();
            stream.Dispose();
        }
    }
}