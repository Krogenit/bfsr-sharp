using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
public class FixedStepThread
{
    private GameClock clock;
    private readonly TimeSpan maximumElapsedTime;
    private TimeSpan targetElapsedTime;
    private TimeSpan accumulatedElapsedGameTime;
    private TimeSpan lastFrameElapsedGameTime;
    private TimeSpan totalGameTime;
    private int updatesSinceRunningSlowly1;
    private int updatesSinceRunningSlowly2;
    private ThreadStart method;
    private Thread thread;
    private bool isExit;
    public FixedStepThread(ThreadStart method, double freq)
    {
        clock = new GameClock(freq);
        totalGameTime = TimeSpan.Zero;
        accumulatedElapsedGameTime = TimeSpan.Zero;
        lastFrameElapsedGameTime = TimeSpan.Zero;
        targetElapsedTime = TimeSpan.FromTicks(166667L);
        updatesSinceRunningSlowly1 = int.MaxValue; ;
        updatesSinceRunningSlowly2 = int.MaxValue;
        maximumElapsedTime = TimeSpan.FromMilliseconds(500.0);
        this.method = method;
        thread = new Thread(Update);
        thread.Start();
    }
    private void Update()
    {
        while (!isExit)
        {
            clock.UpdateElapsedTime();
            TimeSpan timeSpan1 = clock.ElapsedAdjustedTime;
            if (timeSpan1 < TimeSpan.Zero)
                timeSpan1 = TimeSpan.Zero;
            if (timeSpan1 > maximumElapsedTime)
                timeSpan1 = maximumElapsedTime;
            if (Math.Abs(timeSpan1.Ticks - targetElapsedTime.Ticks) < targetElapsedTime.Ticks >> 6)
                timeSpan1 = targetElapsedTime;
            TimeSpan timeSpan2 = accumulatedElapsedGameTime + timeSpan1;
            long num = timeSpan2.Ticks / targetElapsedTime.Ticks;
            lastFrameElapsedGameTime = TimeSpan.Zero;
            if (num == 0L)
            {

            }
            else
            {
                clock.AdvanceFrameTime();
                accumulatedElapsedGameTime = timeSpan2;
                TimeSpan timeSpan3 = targetElapsedTime;
                if (num > 1L)
                {
                    this.updatesSinceRunningSlowly2 = this.updatesSinceRunningSlowly1;
                    this.updatesSinceRunningSlowly1 = 0;
                }
                else
                {
                    if (updatesSinceRunningSlowly1 < int.MaxValue)
                        ++updatesSinceRunningSlowly1;
                    if (updatesSinceRunningSlowly2 < int.MaxValue)
                        ++updatesSinceRunningSlowly2;
                }
                while (num > 0L)
                {
                    --num;
                    try
                    {
                        method.Invoke();
                    }
                    finally
                    {
                        accumulatedElapsedGameTime -= timeSpan3;
                        lastFrameElapsedGameTime += timeSpan3;
                        totalGameTime += timeSpan3;
                    }
                }
            }
        }
    }
    public void Stop()
    {
        isExit = true;
    }
}
