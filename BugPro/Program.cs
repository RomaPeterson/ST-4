using Stateless;

namespace BugPro;

public class Bug
{
    public enum State { Created, Accepted, Working, Fixed, Done, Returned }
    public enum Trigger { Accept, StartWork, CompleteFix, Finish, Return }

    private readonly StateMachine<State, Trigger> _fsm;
    private State _state;

    public Bug()
    {
        _fsm = new StateMachine<State, Trigger>(State.Created);
        _state = State.Created;

        _fsm.Configure(State.Created)
            .Permit(Trigger.Accept, State.Accepted);

        _fsm.Configure(State.Accepted)
            .Permit(Trigger.StartWork, State.Working)
            .Permit(Trigger.Finish, State.Done);

        _fsm.Configure(State.Working)
            .Permit(Trigger.CompleteFix, State.Fixed)
            .Permit(Trigger.Finish, State.Done);

        _fsm.Configure(State.Fixed)
            .Permit(Trigger.Finish, State.Done)
            .Permit(Trigger.Return, State.Returned);

        _fsm.Configure(State.Done)
            .Permit(Trigger.Return, State.Returned)
            .Ignore(Trigger.Finish);

        _fsm.Configure(State.Returned)
            .Permit(Trigger.Accept, State.Accepted)
            .Permit(Trigger.Finish, State.Done)
            .Ignore(Trigger.Return);

        _fsm.OnTransitioned(t => _state = t.Destination);
    }

    public State GetState() => _state;

    public void Accept() => _fsm.Fire(Trigger.Accept);
    public void StartWork() => _fsm.Fire(Trigger.StartWork);
    public void CompleteFix() => _fsm.Fire(Trigger.CompleteFix);
    public void Finish() => _fsm.Fire(Trigger.Finish);
    public void Return() => _fsm.Fire(Trigger.Return);
}

public static class Program
{
    public static void Main()
    {
        var bug = new Bug();
        Console.WriteLine($"State: {bug.GetState()}");

        bug.Accept();
        Console.WriteLine($"State: {bug.GetState()}");

        bug.StartWork();
        Console.WriteLine($"State: {bug.GetState()}");

        bug.CompleteFix();
        Console.WriteLine($"State: {bug.GetState()}");

        bug.Finish();
        Console.WriteLine($"State: {bug.GetState()}");

        bug.Return();
        Console.WriteLine($"State: {bug.GetState()}");

        bug.Accept();
        Console.WriteLine($"State: {bug.GetState()}");

        bug.Finish();
        Console.WriteLine($"State: {bug.GetState()}");

        Console.WriteLine("Done.");
    }
}
