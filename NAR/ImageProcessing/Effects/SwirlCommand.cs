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
using NAR.ImageProcessing.Base;
using System.Drawing;

namespace NAR.ImageProcessing.Effects
{
    public class SwirlCommand : Base.OffsetFilterCommand, ICommand
    {
        #region Variables
        private double _degree; 
        #endregion

        #region Properties
        public double Degree
        {
            get { return _degree; }
        }
        #endregion

        #region Constructors/Destructors
        public SwirlCommand(double degree)
            : base(true)
        {
            _degree = degree;
        }
        #endregion

        #region ICommand Members
        public new Model.IImage Execute(Model.IImage image)
        {
            int width = image.Image.Width;
            int height = image.Image.Height;
            int size = width * height * 3;

            FloatPoint[,] fp = new FloatPoint[width, height];
            Point[,] pt = new Point[width, height];

            Point ptMid = new Point();
            ptMid.X = width / 2;
            ptMid.Y = height / 2;

            double theta, radius;
            double newX, newY;

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    int iTrueX = x - ptMid.X;
                    int iTrueY = y - ptMid.Y;
                    theta = Math.Atan2((iTrueY), (iTrueX));

                    radius = Math.Sqrt(iTrueX * iTrueX + iTrueY * iTrueY);

                    newX = ptMid.X + (radius * Math.Cos(theta + _degree * radius));
                    if (newX > 0 && newX < width)
                    {
                        fp[x, y].X = newX;
                        pt[x, y].X = (int)newX;
                    }
                    else
                        fp[x, y].X = pt[x, y].X = x;

                    newY = ptMid.Y + (radius * Math.Sin(theta + _degree * radius));
                    if (newY > 0 && newY < height)
                    {
                        fp[x, y].Y = newY;
                        pt[x, y].Y = (int)newY;
                    }
                    else
                        fp[x, y].Y = pt[x, y].Y = y;
                }
            }

            base.OffsetPoints = pt;

            return base.Execute(image);
        }
        #endregion
    }
}
