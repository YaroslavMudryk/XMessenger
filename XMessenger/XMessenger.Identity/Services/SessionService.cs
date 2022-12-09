using Microsoft.EntityFrameworkCore;
using XMessenger.Domain.Models.Identity;
using XMessenger.Helpers;
using XMessenger.Helpers.Services;
using XMessenger.Identity.Db.Context;
using XMessenger.Identity.Extensions;
using XMessenger.Identity.Sessions;
using XMessenger.Identity.ViewModels;

namespace XMessenger.Identity.Services
{
    public class SessionService : ISessionService
    {
        private readonly IdentityContext _db;
        private readonly IIdentityService _identityService;
        private readonly ISessionManager _sessionManager;
        public SessionService(IdentityContext db, IIdentityService identityService, ISessionManager sessionManager)
        {
            _db = db;
            _identityService = identityService;
            _sessionManager = sessionManager;
        }

        public async Task<Result<int>> CloseSessionsByIdsAsync(Guid[] ids)
        {
            var now = DateTime.Now;
            var userId = _identityService.GetUserId();
            var sessionId = _identityService.GetCurrentSessionId();

            var sessionsToClose = await _db.Sessions.AsNoTracking().Where(s => ids.Contains(s.Id)).ToListAsync();

            foreach (var session in sessionsToClose)
            {
                if (session.UserId != userId)
                    return Result<int>.Error($"Can't close session with ID ({session.Id})");

                if (!session.IsActive || session.Status == SessionStatus.Close)
                    return Result<int>.Error($"Session with ID ({session.Id}) already closed");
            }

            sessionsToClose.ForEach(session =>
            {
                session.Status = SessionStatus.Close;
                session.IsActive = false;
                session.DeactivatedAt = now;
                session.DeactivatedBySessionId = sessionId;
            });

            //ToDo: add notify about close sessions

            _db.Sessions.UpdateRange(sessionsToClose);
            var affectedSessions = await _db.SaveChangesAsync();

            _sessionManager.RemoveSessions(sessionsToClose.Select(s => s.Id).ToArray());

            return Result<int>.SuccessWithData(affectedSessions);
        }

        public async Task<Result<List<SessionViewModel>>> GetUserSessionsAsync(int q, int page)
        {
            var userId = _identityService.GetUserId();
            var currentSessionId = _identityService.GetCurrentSessionId();

            var query = (IQueryable<Session>)_db.Sessions;
            query = query.Where(x => x.UserId == userId);
            query = query.Where(x => q == 0 ? x.Status == SessionStatus.Active || x.Status == SessionStatus.New : x.Status == SessionStatus.Close);
            query = query.Skip((page - 1) * Paginations.PerPage).Take(Paginations.PerPage);
            query = query.OrderByDescending(x => x.CreatedAt);
            var sessions = await query.ToListAsync();

            var totalSessions = await _db.Sessions.AsNoTracking().CountAsync(x => x.UserId == userId && q == 0 ? x.Status == SessionStatus.Active || x.Status == SessionStatus.New : x.Status == SessionStatus.Close);

            var sessionsToView = sessions.MapToView(currentSessionId);

            return Result<List<SessionViewModel>>.SuccessList(sessionsToView, Meta.FromMeta(totalSessions, page));
        }
    }
}
