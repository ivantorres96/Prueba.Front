using Microsoft.AspNetCore.Components;
using Prueba.Front.DTOs;
using System.Net.Http.Json;

namespace Prueba.Front.Pages
{
    public partial class Home
    {
        [Inject] private HttpClient Http { get; set; }

        private List<StateDto> States { get; set; } = new();
        private List<TaskFilterDto> TaskListFilter { get; set; } = new();

        private TaskDto TaskDto { get; set; } = new();
        private TaskSum TaskSumDto { get; set; } = new();
        private int stateId = 0;
        private string textBtnFormulario = "Abrir formulario";
        private bool flagFormulario = false;
        private bool flagMenssageError = false;
        private bool flagMenssageErrorSearch = false;
        private bool flagUpdateOrCreate = false;

        protected override async Task OnInitializedAsync()
        {
            var response = await Http.GetAsync("api/State");
            States = await response.Content.ReadFromJsonAsync<List<StateDto>>();
            await Analicis();
            //var responseTask
        }

        private async Task Analicis()
        {
            TaskSumDto = await Http.GetFromJsonAsync<TaskSum>("api/Task/sum");
        }

        private async Task BtnRegisterTask()
        {
            flagFormulario = !flagFormulario;
            textBtnFormulario = !flagFormulario ? "Abrir formulario" : "Cerrar formulario";
            flagUpdateOrCreate = false;
        }

        private async Task OnSumitTask()
        {
            if (TaskDto.StateId != 0)
            {
                if (flagUpdateOrCreate)
                {
                    await Http.PutAsJsonAsync("api/Task/Update", TaskDto);
                }
                else
                {
                    await Http.PostAsJsonAsync("api/Task/Create", TaskDto);
                }
                await BtnFilterTask();
                flagMenssageError = false;
                TaskDto = new();
                flagFormulario = false;
            }
            else
            {
                flagMenssageError = true;
            }
            flagMenssageErrorSearch = false;
            await Analicis();
        }

        private async Task BtnFilterTask()
        {
            if (stateId != 0)
            {
                Http.DefaultRequestHeaders.Remove("stateId");
                Http.DefaultRequestHeaders.Add("stateId", stateId.ToString());
                TaskListFilter = await Http.GetFromJsonAsync<List<TaskFilterDto>>("api/Task/filter");
                Http.DefaultRequestHeaders.Remove("stateId");
                flagMenssageErrorSearch = false;
            }
            else
            {
                flagMenssageErrorSearch = true;
            }
        }

        private async Task BtnUpdate(TaskFilterDto taskFilterDto)
        {
            if (taskFilterDto.StateId != 0)
            {
                TaskDto.Id = taskFilterDto.Id;
                TaskDto.Name = taskFilterDto.Name;
                TaskDto.Description = taskFilterDto.Description;
                TaskDto.Priority = taskFilterDto.Priority;
                TaskDto.StateId = taskFilterDto.StateId;
                flagFormulario = true;
                flagUpdateOrCreate = true;
                flagMenssageError = false;
            }
            else
            {
                flagFormulario = true;
                flagMenssageError = true;
            }
        }

        private async Task BtnDelete(TaskFilterDto taskFilterDto)
        {
            Http.DefaultRequestHeaders.Remove("taskId");
            Http.DefaultRequestHeaders.Add("taskId", taskFilterDto.Id.ToString());
            await Http.GetAsync("api/Task/Delete");
            Http.DefaultRequestHeaders.Remove("taskId");

            await BtnFilterTask();
        }
    }
}
