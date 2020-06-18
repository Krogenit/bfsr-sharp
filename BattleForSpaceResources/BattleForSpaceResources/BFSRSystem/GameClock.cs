using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class GameClock
{
    private long baseRealTime;
    private long lastRealTime;
    private bool lastRealTimeValid;
    private int suspendCount;
    private long suspendStartTime;
    private long timeLostToSuspension;
    private long lastRealTimeCandidate;
    private TimeSpan currentTimeOffset;
    private TimeSpan currentTimeBase;
    private TimeSpan elapsedTime;
    private TimeSpan elapsedAdjustedTime;
    private double frequency;
    public GameClock(double freq)
    {
        this.frequency = freq;
        this.Reset();
    }
    public TimeSpan CurrentTime
    {
        get
        {
            return this.currentTimeBase + this.currentTimeOffset;
        }
    }

    public TimeSpan ElapsedTime
    {
        get
        {
            return this.elapsedTime;
        }
    }

    public TimeSpan ElapsedAdjustedTime
    {
        get
        {
            return this.elapsedAdjustedTime;
        }
    }

    public static long Counter
    {
        get
        {
            return Stopwatch.GetTimestamp();
        }
    }

    public static long Frequency
    {
        get
        {
            return Stopwatch.Frequency;
        }
    }

    public void Reset()
    {
        this.currentTimeBase = TimeSpan.Zero;
        this.currentTimeOffset = TimeSpan.Zero;
        this.baseRealTime = GameClock.Counter;
        this.lastRealTimeValid = false;
    }

    public void UpdateElapsedTime()
    {
        long counter = GameClock.Counter;
        if (!this.lastRealTimeValid)
        {
            this.lastRealTime = counter;
            this.lastRealTimeValid = true;
        }
        try
        {
            this.currentTimeOffset = GameClock.CounterToTimeSpan(counter - this.baseRealTime, frequency);
        }
        catch (OverflowException)
        {
            this.currentTimeBase += this.currentTimeOffset;
            this.baseRealTime = this.lastRealTime;
            try
            {
                this.currentTimeOffset = GameClock.CounterToTimeSpan(counter - this.baseRealTime, frequency);
            }
            catch (OverflowException)
            {
                this.baseRealTime = counter;
                this.currentTimeOffset = TimeSpan.Zero;
            }
        }
        try
        {
            this.elapsedTime = GameClock.CounterToTimeSpan(counter - this.lastRealTime, frequency);
        }
        catch (OverflowException)
        {
            this.elapsedTime = TimeSpan.Zero;
        }
        try
        {
            long num = this.lastRealTime + this.timeLostToSuspension;
            this.elapsedAdjustedTime = GameClock.CounterToTimeSpan(counter - num, frequency);
        }
        catch (OverflowException)
        {
            this.elapsedAdjustedTime = TimeSpan.Zero;
        }
        this.lastRealTimeCandidate = counter;
    }

    public void AdvanceFrameTime()
    {
        this.lastRealTime = this.lastRealTimeCandidate;
        this.timeLostToSuspension = 0L;
    }

    public void Suspend()
    {
        ++this.suspendCount;
        if (this.suspendCount != 1)
            return;
        this.suspendStartTime = GameClock.Counter;
    }

    public void Resume()
    {
        --this.suspendCount;
        if (this.suspendCount > 0)
            return;
        this.timeLostToSuspension += GameClock.Counter - this.suspendStartTime;
        this.suspendStartTime = 0L;
    }

    private static TimeSpan CounterToTimeSpan(long delta, double freq)
    {
        long num = 10000000L;
        return TimeSpan.FromTicks(checked(delta * num) / (long)(GameClock.Frequency * freq));
    }
}