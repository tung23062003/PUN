namespace FSM
{
    public delegate bool TransitionDelegate();
    public class Transition : ITransition
    {
        public event TransitionDelegate OnTransition;
        public event TransitionDelegate OnCheck;
        public IState From
        {
            get
            {
                return _from;
            }
            set
            {
                _from = value;
            }
        }
        public IState To
        {
            get
            {
                return _to;
            }
            set
            {
                _to = value;
            }
        }
       
        public Transition( IState fromState, IState toState)
        {
            _from = fromState;
            _to = toState;
        }

        public bool TransitionCallback()
        {
            if (OnTransition != null)
            {
                return OnTransition();
            }
            return true;
        }

        public bool ShouldBengin()
        {
            if (OnCheck != null)
            {
                return OnCheck();
            }
            return false;
        }

        private IState _from;
        private IState _to;
    }
}