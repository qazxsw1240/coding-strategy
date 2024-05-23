namespace CodingStrategy.Network
{
    public interface IPlayerCommandNetworkDelegate
    {
        public abstract void RequestRefresh();

        public abstract void OnResponseReceive(object response);
    }
}
