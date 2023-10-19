namespace TrackFinance.Core.Interfaces;

public interface IHistoricCacheContextService
{
    public bool existCache(string userId);
    public void setCache(string userId, object value);
    public object getCache(string userId);
}