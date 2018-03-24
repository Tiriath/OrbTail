using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Stack of drivers, used to handle overrides (for example a ship movement may change due to a power up for some limited amount of time).
/// Usually only the last attached driver determines the behavior, the rest remain silent until others are removed from the stack.
/// </summary>
/// <typeparam name="TDriver">Type of the driver handled by this stack.</typeparam>
public class DriverStack<TDriver> where TDriver : IDriver
{
    /// <summary>
    /// Get the default driver of this stack.
    /// </summary>
    /// <returns>Returns the default driver.</returns>
    public TDriver GetDefaultDriver()
    {
        return default_driver;
    }

    /// <summary>
    /// Set the default driver for this stack.
    /// </summary>
    /// <param name="driver">New default driver.</param>
    public void SetDefaultDriver(TDriver driver)
    {
        default_driver = driver;
    }

    /// <summary>
    /// Gets the top driver on the stack.
    /// </summary>
    /// <returns>Returns the top of the stack.</returns>
    public TDriver Top()
    {
        while (stack.Count > 0 && !stack.Peek().IsActive())
        {
            stack.Pop();                                                    // Drill down to the first active driver on the stack.
        }

        return stack.Count == 0 ? default_driver : stack.Peek();            // Either the first driver on the stack or the default one if empty.
    }

    /// <summary>
    /// Push a new driver on the stack.
    /// This method does nothing if the driver is not active.
    /// </summary>
    /// <param name="driver">New driver to add to the stack.</param>
    /// <returns>Returns the provided driver.</returns>
    public TDriver Push(TDriver driver)
    {
        if(driver.IsActive())
        {
            stack.Push(driver);
        }

        return driver;
    }

    /// <summary>
    /// Stack of drivers.
    /// </summary>
    private Stack<TDriver> stack = new Stack<TDriver>();

    /// <summary>
    /// Default driver for this stack, to be used when the stack is empty.
    /// </summary>
    private TDriver default_driver;
}
