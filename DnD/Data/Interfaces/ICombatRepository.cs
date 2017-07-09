using DnD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DnD.Data
{
    public interface ICombatRepository
    {
        void SaveResults();
        Encounter LoadEncounter(int encounterId); //need DTO for this?
        Session LoadSession(int sessionId);
        Task<Character> getGenericMonster(string cr);
    }
}
