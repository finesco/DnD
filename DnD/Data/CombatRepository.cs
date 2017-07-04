using DnD.Models;
using log4net;
using System;

namespace DnD.Data
{
    public class CombatRepository : ICombatRepository
    {
        private ILog _log;

        public CombatRepository(ILog logger)
        {
            _log = logger;
        }

        public Encounter LoadEncounter(int encounterId)
        {
            throw new NotImplementedException();
        }

        public Session LoadSession(int sessionId)
        {
            return new Session(sessionId) { Trials = 1 };
        }

        public void SaveResults()
        {
            throw new NotImplementedException();
        }
    }
}
