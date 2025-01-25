using System;
using System.Collections.Generic;
using UnityEngine;

public class DelayUtility : MonoBehaviour
{
    private static DelayUtility _instance;

    // Base class for timed or framed actions
    private abstract class BaseAction
    {
        public Action Action { get; set; } // The action to execute
        public bool RequiresMainThreadExecution { get; set; } = false; // Flag indicating if action requires Unity's main thread
    }

    // Class representing a timed action
    private class TimedAction : BaseAction
    {
        public float TimeRemaining { get; set; } // Time left before action is executed
    }

    // Class representing a frame-based action
    private class FramedAction : BaseAction
    {
        public int FramesRemaining { get; set; } // Frames left before action is executed
    }

    // Lists for storing timed and framed actions
    private readonly List<TimedAction> _timedActions = new List<TimedAction>();
    private readonly List<FramedAction> _framedActions = new List<FramedAction>();
    private readonly List<Action> _mainThreadActions = new List<Action>(); // Actions to be executed on the main thread

    // Locks to ensure thread-safe access to the lists
    private readonly object _timedActionsLock = new object();
    private readonly object _framedActionsLock = new object();
    private readonly object _mainThreadActionsLock = new object();

    private void Awake()
    {
        // Singleton pattern to ensure a single instance of DelayUtility
        if (_instance == null)
        {
            _instance = this;
        }
    }

    /// <summary>
    /// Schedule an action to execute after a specified delay in seconds.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <param name="time">The delay in seconds.</param>
    /// <param name="requiresMainThreadExecution">True if the action requires execution on Unity's main thread.</param>
    public static void ExecuteAfterSeconds(Action action, float time, bool requiresMainThreadExecution = false)
    {
        if (action == null || time < 0)
        {
            Debug.LogWarning("Invalid parameters for ExecuteAfterSeconds. Action is null or time is negative.");
            return;
        }

        EnsureInstance();

        lock (_instance._timedActionsLock)
        {
            _instance._timedActions.Add(new TimedAction
            {
                Action = action,
                TimeRemaining = time,
                RequiresMainThreadExecution = requiresMainThreadExecution
            });
        }
    }

    /// <summary>
    /// Schedule an action to execute after a specified number of frames.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <param name="frames">The delay in frames.</param>
    /// <param name="requiresMainThreadExecution">True if the action requires execution on Unity's main thread.</param>
    public static void ExecuteAfterFrames(Action action, int frames, bool requiresMainThreadExecution = false)
    {
        if (action == null || frames < 0)
        {
            Debug.LogWarning("Invalid parameters for ExecuteAfterFrames. Action is null or frames are negative.");
            return;
        }

        EnsureInstance();

        lock (_instance._framedActionsLock)
        {
            _instance._framedActions.Add(new FramedAction
            {
                Action = action,
                FramesRemaining = frames,
                RequiresMainThreadExecution = requiresMainThreadExecution
            });
        }
    }

    /// <summary>
    /// Cancel a scheduled timed action.
    /// </summary>
    /// <param name="action">The action to cancel.</param>
    public static void CancelTimedAction(Action action)
    {
        EnsureInstance();

        lock (_instance._timedActionsLock)
        {
            _instance._timedActions.RemoveAll(t => t.Action == action);
        }
    }

    /// <summary>
    /// Cancel a scheduled frame-based action.
    /// </summary>
    /// <param name="action">The action to cancel.</param>
    public static void CancelFrameAction(Action action)
    {
        EnsureInstance();

        lock (_instance._framedActionsLock)
        {
            _instance._framedActions.RemoveAll(f => f.Action == action);
        }
    }

    private void Update()
    {
        // Process and execute timed and framed actions
        ProcessTimedActions();
        ProcessFramedActions();

        // Execute actions requiring main thread
        ExecuteMainThreadActions();
    }

    /// <summary>
    /// Process all timed actions and execute them if their time has elapsed.
    /// </summary>
    private void ProcessTimedActions()
    {
        lock (_timedActionsLock)
        {
            // Use RemoveAll to handle expired actions
            _timedActions.RemoveAll(action =>
            {
                action.TimeRemaining -= Time.unscaledDeltaTime; // Decrease remaining time
                bool shouldRemove = action.TimeRemaining <= 0; // Check if time is up

                if (shouldRemove)
                {
                    try
                    {
                        if (action.RequiresMainThreadExecution)
                        {
                            // Add to main thread actions if required
                            lock (_mainThreadActionsLock)
                            {
                                _mainThreadActions.Add(action.Action);
                            }
                        }
                        else
                        {
                            // Execute action immediately if it doesn't require the main thread
                            action.Action?.Invoke();
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning($"Error while invoking timed action: {e}");
                    }
                }
                return shouldRemove; // Mark action for removal
            });
        }
    }

    /// <summary>
    /// Process all frame-based actions and execute them if their frame count has elapsed.
    /// </summary>
    private void ProcessFramedActions()
    {
        lock (_framedActionsLock)
        {
            // Use RemoveAll to handle expired actions
            _framedActions.RemoveAll(action =>
            {
                action.FramesRemaining--; // Decrease remaining frames
                bool shouldRemove = action.FramesRemaining <= 0; // Check if frames are up

                if (shouldRemove)
                {
                    try
                    {
                        if (action.RequiresMainThreadExecution)
                        {
                            // Add to main thread actions if required
                            lock (_mainThreadActionsLock)
                            {
                                _mainThreadActions.Add(action.Action);
                            }
                        }
                        else
                        {
                            // Execute action immediately if it doesn't require the main thread
                            action.Action?.Invoke();
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning($"Error while invoking framed action: {e}");
                    }
                }
                return shouldRemove; // Mark action for removal
            });
        }
    }

    /// <summary>
    /// Execute all actions queued for the main thread.
    /// </summary>
    private void ExecuteMainThreadActions()
    {
        lock (_mainThreadActionsLock)
        {
            if (_mainThreadActions.Count > 0)
            {
                foreach (var action in _mainThreadActions)
                {
                    try
                    {
                        action?.Invoke(); // Execute action safely
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning($"Error while executing main thread action: {e}");
                    }
                }
                _mainThreadActions.Clear(); // Clear executed actions
            }
        }
    }

    /// <summary>
    /// Ensure that a singleton instance of DelayUtility exists.
    /// </summary>
    private static void EnsureInstance()
    {
        if (_instance == null)
        {
            var utilityObject = new GameObject("DelayUtility");
            _instance = utilityObject.AddComponent<DelayUtility>();
        }
    }

    public void ClearInstance()
    {
        _instance = null;
    }

    private void OnApplicationQuit()
    {
        ClearInstance();
    }
}
