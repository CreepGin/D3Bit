//DDFormsExtentions.DDFormFader - Class
//Copyright October 2009 by Rickey Ward (DiamondDrake)
//Email: RickeyWard@DiamondDrake.com
//-----
//This class provides more control over opacity in windowsforms applications
//by providing a more indepth wrapper for the WS_EX_LAYERED window api
//of windows 2k+
//
//THIS CLASS IS PROVIDED ON AN "AS IS" BASIS, AND RICKEY WARD EXPRESSLY DISCLAIMS ANY
//AND ALL WARRANTIES, EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION WARRANTIES OF 
//FITNESS FOR A PARTICULAR PURPOSE, WITH RESPECT TO THIS CLASS OR ANY OTHER MATERIALS.
//IN NO EVENT SHALL RIKEY WARD BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, PUNITIVE,
//OR CONSEQUENTIAL DAMAGES OF ANY KIND WHATSOEVER WITH RESPECT TO THIS CLASS OR EXAMPLE
//PROJECT. BY USING THIS CODE, YOU EXPRESSLY AGREE THAT USE IT AT YOUR OWN RISK.
//IN NO EVENT SHALL RICKEY WARD BE LIABLE FOR ANY INDIRECT, SPECIAL, INCIDENTAL, PUNITIVE,
//OR CONSEQUENTIAL DAMAGES ARISING OUT OF OR RELATED TO THE USE, INABILITY TO USE,
//PERFORMANCE OR NONPERFORMANCE OF THE CODE.
//
//this code may be modifed, distributed, and used freely so long as this notice stays
//intact. This class may be used in a commercial application without royalty.
//
//But as always giving credit to the original author is always appriciated. I hope this
//Class is helpful.
//
//Thanks, DiamondDrake - Rickey Ward --- RickeyWard@DiamondDrake.com

using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace D3BitGUI
{
    class FormFader
    {
        //local variables used by the class
        #region Private variables

        private IntPtr frmHandle; //Handle of form to be faded
        private bool isLayered = false; // Keeps track of if window is set to layered flag or not.
        private int _seekSpeedInterval = 300; //Timer interval used for fading
        private byte _currentTransparency = 255; //for easy reference to current transparency bit
        private byte _destinationTransparency = 255; //keeps track of target fading opacity

        //fading vars
        private int startval = 0; //used in timer tick this is the var that gets incremented
        private int incrementVal = 0; //holds the amount to incriment opacity per step;
        private int totalSteps = 10; //amount of steps or ticks it will take to reach target opacity
        private bool up = true;//holds value indicated if fade is incremented or decremented.

        //timer control used to fade the form
        private Timer timer_fade;

        #endregion

        //Constructor
        public FormFader(IntPtr Handle)
        {
            frmHandle = Handle; // set local variable to hold passed form handle


            //create timer and hook event handler
            timer_fade = new Timer();
            timer_fade.Interval = _seekSpeedInterval;
            timer_fade.Tick += new EventHandler(timer_fade_Tick);

        }
        //public events exposed by this class
        #region Events

        /// <summary>
        /// Announces that a fade seek is complete
        /// </summary>
        public event EventHandler OnFadeComplete;

        #endregion

        //Events handled in the class
        #region event handlers

        void timer_fade_Tick(object sender, EventArgs e)
        {
            if (up)//check if we are going up or down
            {
                if ((startval + incrementVal) < (int)_destinationTransparency) //see if we will pass our goal
                {
                    startval = startval + incrementVal; //if not then increment
                }
                else
                {
                    startval = (int)_destinationTransparency; //if we over steped it, then jump back to the goal
                }

                updateOpacity((byte)startval, true); //update the form's transparency


            }
            else
            {
                if ((startval + incrementVal) > (int)_destinationTransparency) //same as above just the other way around
                {
                    startval = startval + incrementVal;
                }
                else
                {
                    startval = (int)_destinationTransparency;
                }



                updateOpacity((byte)startval, true);
            }

            //check if we are done
            if (_currentTransparency == _destinationTransparency)
            {
                timer_fade.Stop();//if we are done, stop the timer

                //announce to all subscibed objects that the fade has finished
                if (OnFadeComplete != null)
                    OnFadeComplete(this, new EventArgs());
            }
        }

        #endregion

        //All constant values used by the win APIs fpr the class
        #region constants

        //I just threw all these in here incase you wanted to play arround, only 6 values are actually used

        //WS_EX constants
        private const int GWL_EXSTYLE = (-20); // use extended style bitflag
        private const int WS_EX_LAYERED = 0x80000; // use layered window bitflag
        private const long WS_EX_TRANSPARENT = 0x20L;
        private const long LWA_ALPHA = 0x2L;
        private const int LWA_COLORKEY = 0x1;

        //redraw flags

        const int RDW_INVALIDATE = 0x0001; //calls the actuall invalidation of the contol to windows
        const int RDW_INTERNALPAINT = 0x0002;
        const int RDW_ERASE = 0x0004;

        const int RDW_VALIDATE = 0x0008;
        const int RDW_NOINTERNALPAINT = 0x0010;
        const int RDW_NOERASE = 0x0020;

        const int RDW_NOCHILDREN = 0x0040;
        const int RDW_ALLCHILDREN = 0x0080; //causes form to repaint all child controls

        const int RDW_UPDATENOW = 0x0100; //specifies immediate redraw
        const int RDW_ERASENOW = 0x0200;

        const int RDW_FRAME = 0x0400; //used on windows with titlebars!
        const int RDW_NOFRAME = 0x0800;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        #endregion

        //Imported Win API methods
        #region WIN APIs

        //gets information about the windows
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        //sets bigflags that control the windows styles
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        //changes flags that modify attributes of the layered window such as alpha(opacity)
        [DllImport("user32.dll")]
        static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey,
           byte bAlpha, uint dwFlags);

        //calls refresh or invalidation of windows and controls
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool RedrawWindow(IntPtr hWnd, [In] ref RECT lprcUpdate,
           IntPtr hrgnUpdate, uint flags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate,
                          IntPtr hrgnUpdate, uint flags);


        #endregion

        //Public methods exposed by the class
        #region public methods

        /// <summary>
        /// invokes an immediate refresh of the form.
        /// </summary>
        public void Refresh()
        {
            RedrawWindow(frmHandle, IntPtr.Zero, IntPtr.Zero,
             0x0400/*RDW_FRAME*/ | 0x0100/*RDW_UPDATENOW*/
             | 0x0001/*RDW_INVALIDATE*/ | 0x0080 /*RDW_ALLCHILDREN*/);
        }

        /// <summary>
        /// Set form to layered window in order to change opacity
        /// </summary>
        public void setTransparentLayeredWindow()
        {
            if (!isLayered)//check if form has already be sent to layred window,if so do nothing.
            {
                SetWindowLong(frmHandle, GWL_EXSTYLE, GetWindowLong(frmHandle, GWL_EXSTYLE) ^ WS_EX_LAYERED); //set layered window bit flag on form
                SetLayeredWindowAttributes(frmHandle, 0, _currentTransparency, (uint)LWA_ALPHA); //update attributes of layred window, this is important!

                isLayered = true; //set local variable
            }
        }

        public void clearTransparentLayeredWindow()
        {
            if (isLayered)//check if form has already be sent to layred window,if NOT, do nothing.
            {
                SetWindowLong(frmHandle, GWL_EXSTYLE, GetWindowLong(frmHandle, GWL_EXSTYLE) ^ WS_EX_LAYERED);// calling with same flags as before acts as toggle, disabling layred window

                isLayered = false; //sete local variable
            }
        }


        /// <summary>
        /// set the form's Opacity by byte value, 0 = transparent, 255 = Opaque.
        /// </summary>
        /// <param name="transparency">Byte (0-255) level of opacity, 0 = transparent, 255 = opaque.</param>
        /// <param name="forceRefresh">True causes an immediate refresh of entire form.</param>
        public void updateOpacity(byte Opacity, bool forceRefresh)
        {
            if (isLayered)//again make sure we don't call the api unless its active
            {
                SetLayeredWindowAttributes(frmHandle, 0, Opacity, (uint)LWA_ALPHA); //set the attributes of the layered window to update ites transparency

                _currentTransparency = Opacity; //set the form's local variable to keep track of transparency


                if (forceRefresh)
                    Refresh();//refresh the form
            }
        }

        /// <summary>
        /// set the form's Opacity via an INT of percentage value, 0 = transparent, 100 = Opaque.
        /// </summary>
        /// <param name="TransparencyPercent">Int (percent 0 - 100) level of opacity, 0 = transparent, 100 = opaque.</param>
        /// <param name="forceRefresh">True causes an immediate refresh of entire form.</param>
        public void updateOpacity(int OpacityAsPercent, bool forceRefresh)
        {
            if (isLayered)//same as above except with conversions for percent to byte
            {
                byte trans = (byte)((255 * OpacityAsPercent) / 100);

                SetLayeredWindowAttributes(frmHandle, 0, trans, (uint)LWA_ALPHA);

                _currentTransparency = trans;

                if (forceRefresh)
                    Refresh();
            }
        }

        /// <summary>
        /// starts a seek to the destination set by the DestinationOpacity property
        /// </summary>
        public void seekToDestination()
        {
            if (isLayered)
            {
                if (_currentTransparency < _destinationTransparency) //check if we are going up or down
                {
                    up = true;
                }
                else if (_currentTransparency > _destinationTransparency)
                {
                    up = false;
                }
                else //if value are the same then obviouly nothing changes and we exit
                {
                    return;
                }

                //Calculate the Increment Value based on the distance and the amount of stepts to take
                //Formula (Destination - start) / Steps_to_take = Increment_value
                incrementVal = (_destinationTransparency - _currentTransparency) / totalSteps;

                timer_fade.Interval = _seekSpeedInterval; //ensure our timer interval is updated before we start.

                timer_fade.Start(); //Start our Fading!
            }
        }


        /// <summary>
        /// Starts a fade to the provided Opacity
        /// </summary>
        /// <param name="destinationOpacity">Byte (0-255) value to fade to.</param>
        public void seekTo(byte destinationOpacity)
        {
            if (isLayered)
            {
                //set destination
                _destinationTransparency = destinationOpacity;

                if (_currentTransparency < destinationOpacity)
                {
                    up = true;
                }
                else if (_currentTransparency > destinationOpacity)
                {
                    up = false;
                }
                else
                {
                    return;
                }

                startval = currentTransparency;

                incrementVal = (_destinationTransparency - _currentTransparency) / totalSteps;

                timer_fade.Interval = _seekSpeedInterval;

                timer_fade.Start();
            }
        }

        /// <summary>
        /// Starts a fade to the provided Opacity.
        /// </summary>
        /// <param name="destinationOpacityAsPercent">Percent as Int (0-100) to fade to.</param>
        public void seekTo(int destinationOpacityAsPercent)
        {
            if (isLayered)//this method same as above except with some conversions of int percent to byte
            {

                //set destination
                DestinationTransparencyPercent = destinationOpacityAsPercent;

                if (_currentTransparency < (byte)((255 * destinationOpacityAsPercent) / 100))
                {
                    up = true;
                }
                else if (_currentTransparency > (byte)((255 * destinationOpacityAsPercent) / 100))
                {
                    up = false;
                }
                else
                {
                    return;
                }

                startval = currentTransparency;

                incrementVal = (_destinationTransparency - _currentTransparency) / totalSteps;

                timer_fade.Interval = _seekSpeedInterval;

                timer_fade.Start();
            }
        }

        #endregion

        //Public properties exposed by the class
        #region public properties

        /// <summary>
        /// return if form is layred
        /// </summary>
        public bool frmIsLayered
        {
            get
            {
                return isLayered;
            }
        }

        /// <summary>
        /// Interval for fade timer
        /// </summary>
        public int seekSpeed
        {
            get
            {
                return _seekSpeedInterval;
            }
            set
            {
                _seekSpeedInterval = value;
                timer_fade.Interval = value;
            }
        }

        /// <summary>
        /// returns current destination transparency as byte (0-255)
        /// </summary>
        public byte DestinationTransparency
        {
            get
            {
                return _destinationTransparency;
            }
            set
            {
                _destinationTransparency = value;
            }
        }

        /// <summary>
        ///  returns current destination transparency as int percent (0-100)
        /// </summary>
        public int DestinationTransparencyPercent
        {
            get
            {
                return (_destinationTransparency * 100) / 255;
            }
            set
            {
                _destinationTransparency = (byte)((255 * value) / 100);
            }
        }

        public byte currentTransparency
        {
            get
            {
                return _currentTransparency;
            }
        }

        /// <summary>
        /// how many steps for the seek to opacity to take
        /// </summary>
        public int StepsToFade
        {
            get
            {
                return totalSteps;
            }
            set
            {
                totalSteps = value;
            }
        }

        #endregion
    }
}
