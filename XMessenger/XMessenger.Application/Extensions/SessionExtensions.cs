using XMessenger.Application.ViewModels.Identity;
using XMessenger.Domain.Models.Identity;

namespace XMessenger.Application.Extensions
{
    public static class SessionExtensions
    {
        public static List<SessionViewModel> MapToView(this IEnumerable<Session> sessions, Guid? currentSessionId = null)
        {
            return sessions.Select(session => MapToView(session, currentSessionId)).ToList();
        }

        public static SessionViewModel MapToView(this Session session, Guid? currentSessionId = null)
        {
            if (session == null)
                return null;
            return new SessionViewModel
            {
                Id = session.Id,
                CreatedAt = session.CreatedAt,
                IsActive = session.IsActive,
                Status = session.Status,
                Current = currentSessionId.HasValue ? session.Id == currentSessionId : false,
                Language = session.Language,
                App = session.App,
                Location = session.Location,
                Client = session.Client,
                DeactivatedAt = session.DeactivatedAt
            };
        }
    }
}