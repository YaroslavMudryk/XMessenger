using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using XMessenger.Infrastructure.Data.EntityFramework.Context;

namespace XMessenger.Application.Sessions
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

    public class SessionManager : ISessionManager
    {
        private List<SessionModel> _sessions;

        public List<SessionModel> Sessions => _sessions;

        public SessionManager(IServiceScopeFactory serviceScopeFactory)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();

            GetActualTokensFromDb(dbContext);
        }

        public void AddSession(SessionModel sessionModel)
        {
            var session = _sessions.FirstOrDefault(s => s.Id == sessionModel.Id);
            if (session == null)
            {
                _sessions.Add(sessionModel);
            }
            else
            {
                _sessions.Remove(session);
                _sessions.Add(sessionModel);
            }
        }

        public void AddToken(Guid sessionId, TokenModel tokenModel)
        {
            var session = _sessions.FirstOrDefault(s => s.Id == sessionId);
            if (session == null)
                return;
            session.Tokens.Add(tokenModel);
        }

        public bool IsActiveSession(string accessToken)
        {
            var now = DateTime.Now;

            var session = _sessions.Where(s => s.Tokens.FirstOrDefault(s => s.Token == accessToken) != null).FirstOrDefault();

            if (session == null)
                return false;

            var token = session.Tokens.FirstOrDefault(s => s.Token == accessToken);

            if (token.ExpiredAt < now)
            {
                session.Tokens.Remove(token);
                return false;
            }

            return true;
        }

        public void RemoveRangeTokens(IEnumerable<string> tokens)
        {
            foreach (var token in tokens)
            {
                RemoveToken(token);
            }
        }

        public void RemoveSession(Guid sessionId)
        {
            var session = _sessions.FirstOrDefault(s => s.Id == sessionId);
            if (session == null)
                return;
            _sessions.Remove(session);
        }

        public void RemoveSessions(IEnumerable<Guid> sessionIds)
        {
            foreach (var sessionId in sessionIds)
            {
                RemoveSession(sessionId);
            }
        }

        public void RemoveToken(string token)
        {
            var session = _sessions.Where(s => s.Tokens.FirstOrDefault(s => s.Token == token) != null).FirstOrDefault();
            if (session == null)
                return;

            var tokenForRemove = session.Tokens.FirstOrDefault(s => s.Token == token);
            if (tokenForRemove == null)
                return;

            session.Tokens.Remove(tokenForRemove);
        }

        private void GetActualTokensFromDb(IdentityContext db)
        {
            _sessions = new List<SessionModel>();
            var now = DateTime.Now;

            var activeSessions = db.Sessions.AsNoTracking().Where(s => s.IsActive).ToList();

            var activeTokens = db.Tokens.AsNoTracking().Where(s => activeSessions.Select(s => s.Id).Contains(s.SessionId) && s.ExpiredAt >= now).ToList();

            foreach (var session in activeSessions)
            {
                var newSession = new SessionModel
                {
                    Id = session.Id,
                    RefreshToken = session.RefreshToken,
                    UserId = session.UserId,
                    Tokens = activeTokens.Where(s => s.SessionId == session.Id).Select(s => new TokenModel { Token = s.JwtToken, ExpiredAt = s.ExpiredAt }).ToList()
                };
                _sessions.Add(newSession);
            }
        }
    }
}
