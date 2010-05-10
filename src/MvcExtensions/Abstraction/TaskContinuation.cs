namespace MvcExtensions
{
    /// <summary>
    /// Describes how the next task in the chain will be handled.
    /// </summary>
    public enum TaskContinuation
    {
        /// <summary>
        /// Executes the next task
        /// </summary>
        Continue,

        /// <summary>
        /// Skips the next task.
        /// </summary>
        Skip,

        /// <summary>
        /// Stops the execution.
        /// </summary>
        Break
    }
}