using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_Triolingo.Interface.Units;
using Web_Triolingo.Model;

namespace Web_Triolingo.Pages.Units
{
    public class ListAllModel : PageModel
    {
        private readonly IUnitService _unitService;
        public ListAllModel(IUnitService unitService) 
        {
            _unitService = unitService;
        }
        public List<Unit> ListAllUnit { get; set; }
        public List<Unit> AllUnitsById { get; set; }
        public void OnGet(int? id)
        {
            
        }
    }
}
