﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NAR.Tests.ImageProcessing.Pixels
{
    [TestFixture]
    public class PixelCounterTests : BaseCommand
    {
        #region Constructors/Destructors

        [TestFixtureSetUp]
        public void Init()
        {
        }
        [TearDown]
        public void Cleanup()
        {
        }

        #endregion

        #region Methods

        [Test]
        public void TestCount()
        {
            NAR.ImageProcessing.Pixels.PixelCounter test = new NAR.ImageProcessing.Pixels.PixelCounter();

            int x1 = 10;
            int x2 = 200;
            int y1 = 10;
            int y2 = 200;
            bool fill = true;

            NAR.Model.IImage result = null;

            double value = test.Count(base.Pen, x1, x2, y1, y2, fill, out result);

            Assert.AreEqual(7.9168975069252081, value);

            //result.Image.Save("C:\\temp\\cap.bmp");

            NAR.Model.IImage bitmap = new NAR.Model.ImageBitmap(base.ReadBitmap(".\\Resources\\Commands\\CountPixelCommand.bmp"));

            base.CheckAllBytes(result.Bytes, bitmap.Bytes);
        }

        #endregion

    }
}
