using Microsoft.Maui.Controls;
using System;
using System.Diagnostics;

namespace StopwatchBehaviour
{
    public class StopwatchBehavior : Behavior<Button>
    {
        private Stopwatch stopwatch;
        private bool isRunning = false;
        private DateTime pressStartTime;

        protected override void OnAttachedTo(Button button)
        {
            base.OnAttachedTo(button);
            button.Clicked += OnButtonClicked;
            button.Pressed += OnButtonPressed;
            button.Released += OnButtonReleased;
            stopwatch = new Stopwatch();
        }

        private void OnButtonPressed(object sender, EventArgs e)
        {
            pressStartTime = DateTime.Now;
        }

        private void OnButtonReleased(object sender, EventArgs e)
        {
            if (CheckForLongPress())
            {
                ResetStopwatch(sender);
            }
            pressStartTime = DateTime.MinValue;
        }

        protected override void OnDetachingFrom(Button button)
        {
            base.OnDetachingFrom(button);
            button.Clicked -= OnButtonClicked;
            button.Released -= OnButtonReleased;
        }

        private void OnButtonClicked(object sender, EventArgs e)
        {
            if (isRunning)
            {
                stopwatch.Stop(); // Stop the stopwatch
                isRunning = false;
            }
            else
            {
                stopwatch.Start(); // Start or resume the stopwatch
                isRunning = true;

                // Text time updater
                Device.StartTimer(TimeSpan.FromMilliseconds(1), () => 
                {
                    ((Button)sender).Text = stopwatch.Elapsed.ToString("mm':'ss':'fff");
                    return isRunning;
                });
            }
        }

        private bool CheckForLongPress()
        {
            // Long press is set to 1s duration, basically hold to reset the stopwatch
            const int longPressDurationMilliseconds = 1000;
            return DateTime.Now.Subtract(pressStartTime).TotalMilliseconds >= longPressDurationMilliseconds;
        }
    

        private void ResetStopwatch(object sender)
        {
            stopwatch.Reset();
            isRunning = false;
            ((Button)sender).Text = "Start";
        }
    }
}