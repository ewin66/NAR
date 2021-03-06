﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NAR.Tests.ImageProcessing.Images
{
    [TestFixture]
    public class SepiaCommandTests : BaseCommand
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
        public void TestExecute()
        {
            NAR.ImageProcessing.Images.SepiaCommand test = new NAR.ImageProcessing.Images.SepiaCommand();

            NAR.Model.IImage result = test.Execute(base.Model);

            //result.Image.Save(".\\Resources\\Commands\\SepiaCommand.bmp");

            NAR.Model.IImage bitmap = new NAR.Model.ImageBitmap(base.ReadBitmap(".\\Resources\\Commands\\SepiaCommand.bmp"));

            base.CheckAllBytes(result.Bytes, bitmap.Bytes);
        }

        #endregion
    }
}
