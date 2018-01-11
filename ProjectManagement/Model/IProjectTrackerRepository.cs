using System;
using System.Collections.Generic;

namespace ProjectManagement.Model
{
    public interface IProjectTrackerRepository
    {
        List<JabsomAffil> GetAffiliations(string affiliationId);
        JabsomAffil GetAffiliationByName(string affilName);
        int AddAffiliation(JabsomAffil newAffil);
        bool UpdateAffiliation(JabsomAffil affilEdit);
    }
}
