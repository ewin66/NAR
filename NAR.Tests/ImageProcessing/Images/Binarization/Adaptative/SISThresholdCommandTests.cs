﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NAR.Tests.ImageProcessing.Images.Binarization.Adaptative
{
    [TestFixture]
    public class SISThresholdCommandTests : BaseCommand
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
        [Ignore]
        public void TestExecuteGrayScale()
        {
            //NAR.ImageProcessing.Images.Binarization.Adaptative.SISThresholdCommand test = new NAR.ImageProcessing.Images.Binarization.Adaptative.SISThresholdCommand();

            //NAR.Model.IImage result = test.Execute(base.Model);

            ////result.Image.Save("C:\\temp\\cap.bmp");

            //NAR.Model.IImage bitmap = new NAR.Model.ImageBitmap(base.ReadBitmap(".\\Resources\\Commands\\AverageCommand.bmp"));

            //base.CheckAllBytes(result.Bytes, bitmap.Bytes);

        }

        #endregion
    }
}
