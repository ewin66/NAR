﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NAR.Tests.Confiability.Commands.Model
{
    class ImageBitmapImageReport : Base.ColorBaseTest
    {
        #region Constructors/Destructors
        public ImageBitmapImageReport(int totalTests, string imageListFolder, IReport reportGenerator)
            : base (totalTests, imageListFolder, reportGenerator)
        {
        }
        #endregion

        #region Methods
        protected override void Initialize(ref ImageProcessing.ICommand command)
        {
            base.Initialize(ref command);
        }
        protected override ResultData Test(ImageProcessing.ICommand command, string fileName, NAR.Model.IImage image, bool executeFullTest, params NAR.ImageProcessing.ICommand[] commandResults)
        {
            DateTime startDate = DateTime.Now;

            NAR.Model.IImage result = new NAR.Model.ImageBitmap(image.Image);

            DateTime endDate = DateTime.Now;

            TimeSpan time = endDate.Subtract(startDate);


            //Creating the new image which contains the final result
            NAR.Model.IImage newResult = (NAR.Model.IImage)result.Clone();

            if (commandResults != null && executeFullTest)
            {
                foreach (NAR.ImageProcessing.ICommand newCommand in commandResults)
                {
                    newResult = newCommand.Execute(newResult);
                }
            }

            ResultData resultData = null;
            
            if (executeFullTest)
                resultData = new ResultData(fileName, startDate, endDate, time, command, result, newResult);
            else
                resultData = new ResultData(fileName, startDate, endDate, time, command, null, null);
            

            return resultData;
        }
        #endregion
    }
}
