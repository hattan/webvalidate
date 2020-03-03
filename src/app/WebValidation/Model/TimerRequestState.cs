﻿using System;

namespace WebValidation
{
    /// <summary>
    /// Shared state for the Timer Request Tasks
    /// </summary>
    class TimerRequestState
    {
        public int Index = 0;
        public int MaxIndex = 0;
        public long Count = 0;
        public Random Random = null;
        public object Lock = new object();
        public WebV Test;
        public DateTime CurrentLogTime;
    }
}
