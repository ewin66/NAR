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
using NAR.Model;

namespace NAR.ImageProcessing.Base
{
    public class Mask2x2Command : ICommand
    {
        #region Variables
        private short[,] _maskX = new short[2, 2];
        private byte _divisor;
        private byte _threshold;
        private byte _offset;
        protected bool _grayscale;
        protected readonly int _baseMask = 2;
        #endregion

        #region Properties
        protected bool Grayscale
        {
            get { return _grayscale; }
            set { _grayscale = value; }
        }
        protected short[,] MaskX
        {
            get { return _maskX; }
        }
        protected byte Divisor
        {
            get { return _divisor; }
        }
        protected byte Threshold
        {
            get { return _threshold; }
        }
        protected byte Offset
        {
            get { return _offset; }
        }
        #endregion

        #region Constructors/Destructors
        public Mask2x2Command(bool grayscale, byte divisor, byte threshold, byte offset, short[,] maskX)
        {
            _grayscale = grayscale;
            _maskX = maskX;
            _divisor = divisor;
            _threshold = threshold;
            _offset = offset;
        }
        #endregion

        #region Methods

        protected byte[] ApplyMask(Model.IImage image, bool grayscale, short[,] mask, byte divisor, byte threshold, byte offset)
        {

            int width = image.Image.Width;
            int height = image.Image.Height;
            int size = width * height * 3;

            int[] auxByte = new int[3];
            byte[] newByte = new byte[3];
            int calculed = 0;

            //Creating the vector of positions from byte array
            long[] lines = new long[2];

            //Creating the size of bytes similar to the image considering 3 bytes of each pixel (RGB)
            byte[] result = new byte[size];
            byte[] newBytes = new byte[size];

            Array.Copy(image.Bytes, result, size);


            //Calculating the end of the column from the byte array
            long colEnd = (width * 3) - 3;

            //Foreach line in the byte array
            for (int line = 1; line < height - 1; line++)
            {
                //Foreach pixel from the mask
                for (int col = 3; col < colEnd; col += 3)
                {
                    //Calculating the position of near to the focal point
                    lines[0] = (col) + width * (line - 1) * 3;
                    lines[1] = (col) + width * (line) * 3;
                    
                    auxByte[ImageBitmap.COLOR_RED] = result[lines[0] + ImageBitmap.COLOR_RED] * mask[0, 0];
                    auxByte[ImageBitmap.COLOR_RED] += result[lines[1] + ImageBitmap.COLOR_RED] * mask[1, 0];
                    auxByte[ImageBitmap.COLOR_RED] += result[lines[0] + ImageBitmap.COLOR_RED + 3] * mask[0, 1];
                    auxByte[ImageBitmap.COLOR_RED] += result[lines[1] + ImageBitmap.COLOR_RED + 3] * mask[1, 1];


                    calculed = (int)System.Math.Abs((auxByte[ImageBitmap.COLOR_RED] / divisor) + offset);
                    if (calculed > 255)
                        newBytes[lines[1] + ImageBitmap.COLOR_RED] = 255;
                    else if (calculed < 0)
                        newBytes[lines[1] + ImageBitmap.COLOR_RED] = 0;
                    else
                        newBytes[lines[1] + ImageBitmap.COLOR_RED] = (byte)calculed;


                    if (!grayscale)
                    {

                        auxByte[ImageBitmap.COLOR_GREEN] = result[lines[0] + ImageBitmap.COLOR_GREEN] * mask[0, 0];
                        auxByte[ImageBitmap.COLOR_GREEN] += result[lines[1] + ImageBitmap.COLOR_GREEN] * mask[1, 0];
                        auxByte[ImageBitmap.COLOR_GREEN] += result[lines[0] + ImageBitmap.COLOR_GREEN + 3] * mask[0, 1];
                        auxByte[ImageBitmap.COLOR_GREEN] += result[lines[1] + ImageBitmap.COLOR_GREEN + 3] * mask[1, 1];

                        calculed = (int)System.Math.Abs((auxByte[ImageBitmap.COLOR_GREEN] / divisor) + offset);
                        if (calculed > 255)
                            newBytes[lines[1] + ImageBitmap.COLOR_GREEN] = 255;
                        else if (calculed < 0)
                            newBytes[lines[1] + ImageBitmap.COLOR_GREEN] = 0;
                        else
                            newBytes[lines[1] + ImageBitmap.COLOR_GREEN] = (byte)calculed;


                        auxByte[ImageBitmap.COLOR_BLUE] = result[lines[0] + ImageBitmap.COLOR_BLUE] * mask[0, 0];
                        auxByte[ImageBitmap.COLOR_BLUE] += result[lines[1] + ImageBitmap.COLOR_BLUE] * mask[1, 0];
                        auxByte[ImageBitmap.COLOR_BLUE] += result[lines[0] + ImageBitmap.COLOR_BLUE + 3] * mask[0, 1];
                        auxByte[ImageBitmap.COLOR_BLUE] += result[lines[1] + ImageBitmap.COLOR_BLUE + 3] * mask[1, 1];

                        calculed = (int)System.Math.Abs((auxByte[ImageBitmap.COLOR_BLUE] / divisor) + offset);
                        if (calculed > 255)
                            newBytes[lines[1] + ImageBitmap.COLOR_BLUE] = 255;
                        else if (calculed < 0)
                            newBytes[lines[1] + ImageBitmap.COLOR_BLUE] = 0;
                        else
                            newBytes[lines[1] + ImageBitmap.COLOR_BLUE] = (byte)calculed;
                    }
                    else
                    {
                        newBytes[lines[1] + ImageBitmap.COLOR_GREEN] = newBytes[lines[1] + ImageBitmap.COLOR_RED];
                        newBytes[lines[1] + ImageBitmap.COLOR_BLUE] = newBytes[lines[1] + ImageBitmap.COLOR_RED];
                    }


                    if (threshold != 0)
                    {
                        newBytes[lines[1] + ImageBitmap.COLOR_RED] = newBytes[lines[1] + ImageBitmap.COLOR_RED] > threshold ? (byte)255 : (byte)0;
                        newBytes[lines[1] + ImageBitmap.COLOR_GREEN] = newBytes[lines[1] + ImageBitmap.COLOR_GREEN] > threshold ? (byte)255 : (byte)0;
                        newBytes[lines[1] + ImageBitmap.COLOR_BLUE] = newBytes[lines[1] + ImageBitmap.COLOR_BLUE] > threshold ? (byte)255 : (byte)0;

                    }


                }//end for each pixel from the mask

            }//end for line




            return newBytes;
        }

        #endregion

        #region ICommand Members
        public Model.IImage Execute(Model.IImage image)
        {
            return new Model.ImageBitmap(
                   image.Image.Width,
                   image.Image.Height,
                   ApplyMask(image, _grayscale, _maskX, _divisor, _threshold, _offset));
        }
        #endregion
    }
}
