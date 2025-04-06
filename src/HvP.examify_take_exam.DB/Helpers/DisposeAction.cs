namespace HvP.examify_take_exam.DB.Helpers
{
    public class DisposeAction : IDisposable
    {
        private readonly Action _disposeAction;

        public DisposeAction(Action disposeAction)
        {
            _disposeAction = disposeAction;
        }

        public void Dispose()
        {
            _disposeAction.Invoke();
        }
    }
}
