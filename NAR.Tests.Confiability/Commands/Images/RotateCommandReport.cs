﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NAR.Tests.Confiability.Commands.Images
{
    class RotateCommandReport: Base.ColorBaseTest
    {
        #region Constructors/Destructors
        public RotateCommandReport(int totalTests, string imageListFolder, IReport reportGenerator)
            : base (totalTests, imageListFolder, reportGenerator)
        {
        }
        #endregion

        #region Methods
        protected override void Initialize(ref ImageProcessing.ICommand command)
        {
            command = new NAR.ImageProcessing.Images.RotateCommand();

            base.Initialize(ref command);
        }
        #endregion
    }
}
