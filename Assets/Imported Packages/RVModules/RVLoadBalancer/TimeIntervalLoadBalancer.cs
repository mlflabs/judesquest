// Created by Ronis Vision. All rights reserved
// 05.11.2019.

using System;
using System.Collections.Generic;

namespace RVLoadBalancer
{
    /// <summary>
    /// Updates every Tickable setted amount of times per setted time interval
    /// TODO figure out how to synchonize it, as it gets out on sync with time - nothing important, but it should be as reliable as possible 
    /// - because totally deterministic working is impossible because of unpredictable framerate
    /// </summary>
    public sealed class TimeIntervalLoadBalancer : LoadBalancer
    {
        #region Fields

        private int ticksPerInterval;

        private float intervalTime;

        //
        private int framesPerPhase;

        private float timeLapsedPerPhase;

        // in seconds
        private float avgFrameTimePerPhase = 0.0166f;

        private int tickablesToUpdatePerPhase;

        // if you update less than one Tickable per frames
        private int framesToOneTick;

        private bool waitingForNextPhase;

        #endregion

        #region Properties

        #endregion

        public TimeIntervalLoadBalancer(int _ticksPerInterval = 3, float _intervalTime = 5f, bool _calculateDt = false, bool _useUnscaledDeltaTime = false) : base(_calculateDt, _useUnscaledDeltaTime)
        {
            ticksPerInterval = _ticksPerInterval;
            intervalTime = _intervalTime;
        }

        #region Public methods

        public override void Tick(float _deltaTime)
        {
            if (Actions.Count == 0) return;

            time += _deltaTime;

            if (tickablesToUpdatePerPhase == -1) ResetPhase();

            //
            timeLapsedPerPhase += _deltaTime;
            framesPerPhase++;
            avgFrameTimePerPhase = timeLapsedPerPhase / (1.0f * framesPerPhase);
            // 

            //
            var timeLeftToPhaseEnd = intervalTime - timeLapsedPerPhase;
            var framesLeftToPhaseEnd = timeLeftToPhaseEnd / avgFrameTimePerPhase;
            float tickablesToTickPerFrame = 0;
            if (waitingForNextPhase == false)
                tickablesToTickPerFrame = 1.0f * tickablesToUpdatePerPhase / framesLeftToPhaseEnd;

            //
            if (waitingForNextPhase == false)
            {
                // If you need to update more than one Tickable per frame
                if (tickablesToTickPerFrame >= 1)
                {
                    for (var i = 0; i < tickablesToTickPerFrame; i++)
                    {
                        if (indexToTick >= Actions.Count) indexToTick = 0;

                        InvokeAction();

                        tickablesToUpdatePerPhase--;
                        indexToTick++;
                        //if (indexToTick >= Actions.Count) indexToTick = 0;

                        if (tickablesToUpdatePerPhase <= 0)
                        {
                            waitingForNextPhase = true;
                            break;
                        }
                    }
                }
                // If you need to update one Tickable per few frames
                else
                {
                    if (framesToOneTick > 1.0f / tickablesToTickPerFrame)
                    {
                        if (indexToTick >= Actions.Count) indexToTick = 0;

                        InvokeAction();

                        tickablesToUpdatePerPhase--;
                        indexToTick++;
                        //if (indexToTick >= Actions.Count) indexToTick = 0;

                        framesToOneTick = 0;
                    }

                    framesToOneTick++;
                }
            }

            if (tickablesToUpdatePerPhase <= 0)
                waitingForNextPhase = true;

            if (timeLeftToPhaseEnd < avgFrameTimePerPhase /*&& tickablesToUpdatePerPhase <= 0*/)
                ResetPhase();
        }

        #endregion

        #region Not public methods

        private void ResetPhase()
        {
            waitingForNextPhase = false;
            tickablesToUpdatePerPhase = Actions.Count * ticksPerInterval;
            timeLapsedPerPhase -= intervalTime;
            framesPerPhase = 0;
        }

        #endregion
    }
}