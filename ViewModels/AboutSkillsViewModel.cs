using System;

namespace ModernPortfolio.ViewModels;

public class AboutSkillsViewModel 
{
    public AboutViewModel? About { get; set; }
    public List<SkillViewModel> Skills { get; set; } = [];
    public int GetSkillCount() => Skills.Count;
}
