using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Internal.System.Collections.Sequences;
using WebApplication1.Data;
using WebApplication1.ViewModels;
using Zad1;

namespace WebApplication1.Controllers
{   [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        public TodoController(ITodoRepository repository,UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var items=_repository.GetActive(user.GuidId);
            
            
            var vms= items.Select(item => new TodoViewModel
                {
                    Id = item.Id,
                    Text = item.Text,
                    DateDue = item.DateDue,
                   RemamingText = item.DateDue==null?"Rok nije specificiran":createRemainingText(item.DateDue.Value),
                   DateCompleted=item.DateCompleted
            })
                .ToList();
            IndexViewModel vm = new IndexViewModel
            {
                TodoModels = vms
            };
            return View( vm);
        }
        public async Task<IActionResult> Completed()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var items = _repository.GetCompleted(user.GuidId);


            var vms = items.Select(item => new TodoViewModel
                {
                    Id = item.Id,
                    Text = item.Text,
                    DateDue = item.DateDue,
                    RemamingText = "",
                    DateCompleted = item.DateCompleted
                })
                .ToList();
            CompletedViewModel vm = new CompletedViewModel() { TodoModels =vms};
            return View(vm);
        }
        private String createRemainingText(DateTime dateDue)
        {
            int days = Convert.ToInt32(Math.Floor((dateDue - DateTime.Now).TotalDays));
            if (days < 0)
            {
                return "Rok je prosao";
            }
            else switch (days)
            {
                case 0:
                    return "Rok je danas";
                case 1:
                    return "Rok je sutra";
                default:
                    return "Rok je za " + days + " dana";
            }

        }
        public async Task<IActionResult> MarkAsCompleted(Guid vm)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            _repository.MarkAsCompleted(vm, user.GuidId);
            return Redirect("Index");
        }
        public async Task<IActionResult> Remove(Guid vm)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            _repository.Remove(vm, user.GuidId);
            return Redirect("Completed");
        }
        public  ViewResult Add()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddTodoViewModel vm)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (!ModelState.IsValid) return View(vm);
            
            var item = new TodoItem(vm.Text, user.GuidId)
            {
                DateDue = vm.DateDue,
                DateCreated = DateTime.Now
            };
           
            _repository.Add(item);

            if (vm.Labels == null) return RedirectToAction("Index");

            string[] labels = vm.Labels.Split(';');
            foreach (string label in labels)
            {
                _repository.AddLabelToTodoItem(label,item.Id,item.UserId);
            }

            return RedirectToAction("Index");
        }
    }
}