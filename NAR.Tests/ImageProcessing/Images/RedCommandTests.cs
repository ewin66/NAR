﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NAR.Tests.ImageProcessing.Images
{
    [TestFixture]
    public class RedCommandTests : BaseCommand
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
            NAR.ImageProcessing.Images.RedCommand test = new NAR.ImageProcessing.Images.RedCommand();

            NAR.Model.IImage result = test.Execute(base.Model);

            NAR.Model.IImage bitmap = new NAR.Model.ImageBitmap(base.ReadBitmap(".\\Resources\\Commands\\RedCommand.bmp"));

            base.CheckAllBytes(result.Bytes, bitmap.Bytes);
        }

        #endregion
    }
}
