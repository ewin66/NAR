﻿/******************************************************************************
* Copyright (c) 2012, TAP Consulting Group
* All rights reserved.
*
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions are met:
*     * Redistributions of source code must retain the above copyright
*       notice, this list of conditions and the following disclaimer.
*     * Redistributions in binary form must reproduce the above copyright
*       notice, this list of conditions and the following disclaimer in the
*       documentation and/or other materials provided with the distribution.
*     * Neither TAP Consulting Group nor the
*       names of its contributors may be used to endorse or promote products
*       derived from this software without specific prior written permission.
*
* THIS SOFTWARE IS PROVIDED BY TAP Consulting Group ''AS IS'' AND ANY
* EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
* WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
* DISCLAIMED. IN NO EVENT SHALL TAP Consulting Group BE LIABLE FOR ANY
* DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
* (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
* LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
* ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
* (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
* SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NAR.ImageProcessing.Borders
{
    public class MaskData
    {
        #region Variables

        private int _lines;
        private int _columns;
        private byte[,] _bytes;
        
        #endregion

        #region Properties
        
        public int Lines
        {
            get { return _lines; }
        }
        public int Columns
        {
            get { return _columns; }
        }
        public byte[,] Data
        {
            get { return _bytes; }
            set { _bytes = value; }
        }
        
        #endregion

        #region Constructors/Destructors
        public MaskData(int lines, int columns)
        {
            _lines = lines;
            _columns = columns;
            _bytes = new byte[lines, columns];
            
        }
        #endregion
    }


    public class MaskCommand : ICommand
    {
        #region Variables
        private MaskData _mask;
        #endregion

        #region Properties
        public MaskData Mask
        {
            get { return _mask; }
        }
        #endregion

        #region Constructors/Destructors
        public MaskCommand(MaskData mask)
        {
            _mask = mask;
        }
        #endregion

        #region ICommand Members
        
        public Model.IImage Execute(Model.IImage image)
        {
            int width = image.Image.Width;
            int height = image.Image.Height;
            int size = width * height * 3;


            
            //Creating the size of bytes similar to the image considering 3 bytes of each pixel (RGB)
            byte[] result = new byte[size];

            Array.Copy(image.Bytes, result, size);




            for (int line = 1; line < result.Length - 1; line++)
            {
                for (int column = _mask.Columns; column < width - _mask.Columns; column += _mask.Columns)
                {

                }

            }//end for line

            Model.IImage imageResult = new Model.ImageBitmap(width, height, result);
            

            return imageResult;
        }

        #endregion
    }
}
