using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMessenger.Domain.Models.Identity
{
    public class RoleClaim : BaseModel<int>
    {
        public int ClaimId { get; set; }
        public Claim Claim { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

    }
}
