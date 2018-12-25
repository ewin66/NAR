﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NAR.Tests.ImageProcessing.Images.Binarization
{
    [TestFixture]
    public class BayerDitheringCommandTests : BaseCommand
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
        public void TestExecuteGrayScale()
        {
            NAR.ImageProcessing.Images.Binarization.BayerDitheringCommand test = new NAR.ImageProcessing.Images.Binarization.BayerDitheringCommand();

            NAR.ImageProcessing.Images.GrayscaleCommand gray = new NAR.ImageProcessing.Images.GrayscaleCommand(false);
            NAR.Model.IImage grayModel = gray.Execute(base.Model);

            NAR.Model.IImage result = test.Execute(grayModel);

            //result.Image.Save("C:\\temp\\cap.bmp");

            NAR.Model.IImage bitmap = new NAR.Model.ImageBitmap(base.ReadBitmap(".\\Resources\\Commands\\BayerDitheringCommand.bmp"));

            base.CheckAllBytes(result.Bytes, bitmap.Bytes);

        }

        #endregion
    }
}
