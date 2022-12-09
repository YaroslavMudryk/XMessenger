namespace XMessenger.Identity.Sessions
{
    public interface ISessionManager
    {
        List<SessionModel> Sessions { get; }
        void AddSession(SessionModel sessionModel);
        void AddToken(Guid sessionId, TokenModel tokenModel);
        bool IsActiveSession(string token);
        void RemoveSession(Guid sessionId);
        void RemoveSessions(IEnumerable<Guid> sessionIds);
        void RemoveToken(string token);
        void RemoveRangeTokens(IEnumerable<string> tokens);
    }
}
