// ------------------------------------------------------------------
// DirectX.Capture
//
// History:
//	2003-Jan-24		BL		- created
//
// Copyright (c) 2003 Brian Low
// ------------------------------------------------------------------

using System;
using System.Runtime.InteropServices;
#if DSHOWNET
using NAR.Capture.Drivers.DirectShow.DShowNET;
#else
using DirectShowLib;
#endif
namespace NAR.Capture.Drivers.DirectShow.DSCapture
{
	/// <summary>
	///  Represents a physical connector or source on an audio/video device.
	/// </summary>
	public class Source : IDisposable
    {
        #region Variables
        // --------------------- Private/Internal properties -------------------------

        /// <summary>
        /// Name of the source
        /// </summary>
		protected string				name;

        #endregion

        #region Properties
        // ----------------------- Public properties -------------------------

		/// <summary> The name of the source. Read-only. </summary>
		public string Name { get { return( name ); } }

		/// <summary> Obtains the String representation of this instance. </summary>
		public override string ToString() { return( Name ); }

		/// <summary> Is this source enabled. </summary>
		public virtual bool Enabled 
		{
			get { throw new NotSupportedException( "This method should be overriden in derrived classes." ); } 
			set { throw new NotSupportedException( "This method should be overriden in derrived classes." ); } 
		}
        #endregion

        #region Constructors/Destructors
        // -------------------- Constructors/Destructors ----------------------

		/// <summary> Release unmanaged resources. </summary>
		~Source()
		{
			Dispose();
		}
        #endregion

        #region Methods
        // -------------------- IDisposable -----------------------

		/// <summary> Release unmanaged resources. </summary>
		public virtual void Dispose()
		{
			name = null;
        }
        #endregion
    }
}
