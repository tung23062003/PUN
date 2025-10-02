namespace FSM
{
    public interface ITransition
    {
        IState From { get; set; }
        IState To { get; set; }
        bool TransitionCallback();
        bool ShouldBengin();
    }
}