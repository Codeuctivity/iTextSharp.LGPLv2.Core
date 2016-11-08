﻿using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iTextSharp.LGPLv2.Core.FunctionalTests
{
    [TestClass]
    public class PdfPTableTests
    {
        [TestMethod]
        public void Verify_MultiColumn_Report_CanBe_Processed()
        {
            var pdfFilePath = TestUtils.GetOutputFileName();
            var fileStream = new FileStream(pdfFilePath, FileMode.Create);
            var pdfDoc = new Document(PageSize.A4);
            var pdfWriter = PdfWriter.GetInstance(pdfDoc, fileStream);

            pdfDoc.AddAuthor(TestUtils.Author);
            pdfDoc.Open();

            var table1 = new PdfPTable(1)
            {
                WidthPercentage = 100f,
                HeaderRows = 3,
                FooterRows = 1
            };

            //header row
            var headerCell1 = new PdfPCell(new Phrase("header row-1"));
            table1.AddCell(headerCell1);

            var headerCell2 = new PdfPCell(new Phrase("header row-2"));
            table1.AddCell(headerCell2);

            //footer row
            var footerCell = new PdfPCell(new Phrase(" footer "));
            table1.AddCell(footerCell);

            //adding some rows
            for (int i = 0; i < 400; i++)
            {
                var rowCell = new PdfPCell(new Phrase(i.ToString()));
                table1.AddCell(rowCell);
            }

            table1.SkipFirstHeader = true;

            // wrapping table1 in multiple columns
            ColumnText ct = new ColumnText(pdfWriter.DirectContent)
            {
                RunDirection = PdfWriter.RUN_DIRECTION_RTL
            };
            ct.AddElement(table1);

            int status = 0;
            int count = 0;
            int l = 0;
            int columnsWidth = 100;
            int columnsMargin = 7;
            int columnsPerPage = 4;
            int r = columnsWidth;
            bool isRtl = true;

            // render the column as long as it has content
            while (ColumnText.HasMoreText(status))
            {
                if (isRtl)
                {
                    ct.SetSimpleColumn(
                        pdfDoc.Right - l, pdfDoc.Bottom,
                        pdfDoc.Right - r, pdfDoc.Top
                    );
                }
                else
                {
                    ct.SetSimpleColumn(
                        pdfDoc.Left + l, pdfDoc.Bottom,
                        pdfDoc.Left + r, pdfDoc.Top
                    );
                }

                var delta = columnsWidth + columnsMargin;
                l += delta;
                r += delta;

                // render as much content as possible
                status = ct.Go();

                // go to a new page if you've reached the last column
                if (++count > columnsPerPage)
                {
                    count = 0;
                    l = 0;
                    r = columnsWidth;
                    pdfDoc.NewPage();
                }
            }

            pdfDoc.Close();
            fileStream.Dispose();

            TestUtils.VerifyPdfFileIsReadable(pdfFilePath);
        }
    }
}