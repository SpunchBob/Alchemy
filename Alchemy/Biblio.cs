using System.Collections.Generic;

namespace Alchemy
{
    public static class Biblio
    {
        //// Словарь с названием и формулой о химических веществах 
        public static Dictionary<string, ChemicalInfo> Chemicals { get; } = new Dictionary<string, ChemicalInfo>
        {
            {"Перекись водорода", new ChemicalInfo("Перекись водорода", "H₂O₂")},
            {"Поваренная соль", new ChemicalInfo("Поваренная соль", "NaCl")},
            {"Глюкоза", new ChemicalInfo("Глюкоза", "C₆H₁₂O₆")},
            {"Этанол", new ChemicalInfo("Этанол", "C₂H₅OH")},
            {"Ацетилсалициловая кислота", new ChemicalInfo("Ацетилсалициловая кислота", "C₉H₈O₄")},
            {"Вода", new ChemicalInfo("Вода", "H₂O")},
            {"Нитроглицерин ", new ChemicalInfo("Нитроглицерин", "O2NOCH2CH(ONO2)CH2ONO2")},
            {"Серная кислота", new ChemicalInfo("Серная кислота", "H₂SO₄")},
            {"Алмаз", new ChemicalInfo("Алмаз", "C")},
            {"Аметист", new ChemicalInfo("Аметист", "SiO₂ ")},
            {"Рубин", new ChemicalInfo("Рубин", "Al₂O₃")},
            {"Изумруд", new ChemicalInfo("Изумруд", "Be₃Al₂(SiO₃)₆")},
            {"Малахит", new ChemicalInfo("Малахит", "Cu₂CO₃(OH)₂")},
            {"Гранат", new ChemicalInfo("Гранат", "X₃Y₂(SiO₄)₃")},
            {"Сапфир", new ChemicalInfo("Сапфир", "Al₂O₃")},
            {"Агат", new ChemicalInfo("Агат", "SiO₂")},
        };
    }

    // Класс для хранения информации о веществе (название и формула)
    public class ChemicalInfo
    {
        public string Name { get; }// Название вещества
        public string Formula { get; } // Формула вещества

        public ChemicalInfo(string name, string formula)
        {
            Name = name;
            Formula = formula;
        }
    }
}