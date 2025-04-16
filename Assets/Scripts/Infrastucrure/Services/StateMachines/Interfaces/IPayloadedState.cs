public interface IPayloadedState<TPayload>
{
    void Enter(TPayload payload);
    void Exit();
}