﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NAR.Tests.Confiability.Base
{
    public class BorderBaseTest : GrayscaleBaseTest
    {
        #region Variables
        private NAR.ImageProcessing.Borders.IBorderDetector _borderDetector;
        #endregion

        #region Constructors/Destructors
        public BorderBaseTest(int totalTests, string imageListFolder, IReport reportGenerator, NAR.ImageProcessing.Borders.IBorderDetector borderDetector)
            : base (totalTests, imageListFolder, reportGenerator)
        {
            _borderDetector = borderDetector;


        }
        #endregion

        #region Methods
        protected override void Initialize(ref ImageProcessing.ICommand command)
        {
            base.Initialize(ref command);

            TimeSpan average = new TimeSpan();

            for (int c = 0; c < base.ImageList.Count; c++)
            {
                DateTime initial = DateTime.Now;

                base.ImageList[c] = _borderDetector.Execute(base.ImageList[c]);

                average += DateTime.Now.Subtract(initial);
            }

            base.TimeImagePrepared += new TimeSpan(average.Ticks / base.ImageList.Count);
        }


        public new void Initialize()
        {
            this.Initialize(ref _command);
        }
        #endregion
    }
}
