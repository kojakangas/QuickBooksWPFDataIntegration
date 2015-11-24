using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace AffirmMedicalMainPortal.Components
{
    class DataBaseParent
    {
        protected static affirmmedicalweightlossEntities db = new affirmmedicalweightlossEntities();
    }
}
