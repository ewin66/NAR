﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NAR.Tests.ImageProcessing.Images.Binarization
{
    [TestFixture]
    public class JarvisJudiceNinkeDitheringCommandTests : BaseCommand
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
            NAR.ImageProcessing.Images.Binarization.JarvisJudiceNinkeDitheringCommand test = new NAR.ImageProcessing.Images.Binarization.JarvisJudiceNinkeDitheringCommand();

            NAR.Model.IImage result = test.Execute(base.Model);

            //result.Image.Save("C:\\temp\\cap.bmp");

            NAR.Model.IImage bitmap = new NAR.Model.ImageBitmap(base.ReadBitmap(".\\Resources\\Commands\\JarvisJudiceNinkeDitheringCommand.bmp"));

            base.CheckAllBytes(result.Bytes, bitmap.Bytes);

        }

        #endregion
    }
}
