using Microsoft.AspNetCore.Mvc;
using ModernPortfolio.Models;
using ModernPortfolio.Services;
using ModernPortfolio.ViewModels;

namespace ModernPortfolio.Areas.Admin.Controllers
{
    public class SkillsController : BaseAdminController
    {
        private readonly ISkillService _skillService;

        public SkillsController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        public async Task<ActionResult> Index()
        {
            var skills = await _skillService.GetAllSkillsAsync();
            var models = skills.Select(s => new SkillListViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Percentage = s.Percentage,
                DisplayOrder = s.DisplayOrder,
                CreatedDate = s.CreatedAt.ToShortDateString()
            }).ToList();
            return View(models);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new SkillCreateViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create(SkillCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var skill = new Skill
            {
                Name = model.Name,
                Percentage = (int)model.Percentage!,
                DisplayOrder = model.DisplayOrder,
                CreatedAt = DateTime.UtcNow
            };
            try
            {
                var skillId = await _skillService.CreateSkillAsync(skill);
                if (skillId > 0)
                {
                    TempData["SuccessMessage"] = "Yetenek başarıyla eklendi";
                    return RedirectToAction(nameof(Index));
                }
                TempData["ErrorMessage"]= "Yetenek eklerken bir sorun oluştu.";
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"]=$"Hata: {e.Message}";

            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var skill = await _skillService.GetSkillByIdAsync(id);
            if (skill is null)
            {
                TempData["ErrorMessage"] = "Yetenek bulunamadı!";
                return RedirectToAction(nameof(Index));
            }
            var model = new SkillEditViewModel
            {
                Id = skill.Id,
                Name = skill.Name,
                Percentage = skill.Percentage,
                DisplayOrder = skill.DisplayOrder
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(SkillEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var skill = await _skillService.GetSkillByIdAsync(model.Id);
            if (skill is null)
            {
                TempData["ErrorMessage"] = "Yetenek bulunamadı!";
                return RedirectToAction(nameof(Index));
            }
            skill.Name = model.Name;
            skill.Percentage = (int)model.Percentage!;
            skill.DisplayOrder = model.DisplayOrder;
            try
            {
                var result = await _skillService.UpdateSkillAsync(skill);
                if (result)
                {
                    TempData["SuccessMessage"] = "Yetenek başarıyla güncellendi";
                    return RedirectToAction(nameof(Index));
                }
                TempData["ErrorMessage"]= "Yetenek eklerken bir sorun oluştu.";
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"]=$"Hata: {e.Message}";
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _skillService.DeleteSkillAsync(id);
                if (result)
                {
                    TempData["SuccessMessage"] = "Yetenek başarıyla silindi";
                }
                else
                {
                    TempData["ErrorMessage"] = "Yetenek silinirken hata oluştu!";
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = $"Hata: {e.Message}";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}




