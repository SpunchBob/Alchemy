using System.Collections.Generic;
using System;

namespace Alchemy
{
    public class Poisk
    {
        // Поиск точного совпадения по названию вещества
        public ChemicalInfo Search(string name)
        {
            // Поиск
            if (Biblio.Chemicals.TryGetValue(name, out ChemicalInfo chemical))
            {
                return chemical;
            }
            return null;
        }

        // Поиск частичного совпадения по названию 
        public List<string> SearchPartial(string name)
        {
            var results = new List<string>();

            foreach (var chemical in Biblio.Chemicals.Keys)
            {
                if (chemical.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0) // Сравниваем без учета регистра
                {
                    results.Add(chemical); // Добавляет все подходящие варианты какие есть
                }
            }

            return results;
        }
    }
}