﻿using System;
using System.Diagnostics;

namespace lib12.Misc
{
    /// <summary>
    /// Helper class for checking performance
    /// </summary>
    public static class PerformanceCheck
    {
        /// <summary>
        /// Check performance of action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static long Check(Action action)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            action.Invoke();
            stopwatch.Stop();

            var elapsedTime = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();
            return elapsedTime;
        }
    }
}