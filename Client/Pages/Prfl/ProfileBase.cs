using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Profile.Shared.Models;

namespace Profile.Client.Pages.Prfl
{
    public class ProfileBase : ComponentBase {

        [Inject] IHttpClientFactory ClientFactory { get; set; }
        [Inject] Services.StateContainer StateContainer { get; set; }

        public int Count = 1;

        [Parameter] public string username { get; set; }
        public Favorite[] Favorites { get; set; }
        public Account[] Accounts { get; set; }
        public Link[] Links { get; set; }
        public Project[] Projects { get; set; }
        public Video[] Videos { get; set; }
        public Account Account { get; set; }
        public  Schedule Schedule { get; set; }
        public Schedule[] Schedules { get; set; }
        public Recipe[] Recipes { get; set; }
        public Ingredient[] Ingredients { get; set; }
        public Product[] Products { get; set; }
        public string PhoneNumber { get; set; }
        public User User { get; set;}
        

        public async void ChangePropertyValue(string username, HttpClient client)
        {
            var user = await client.GetFromJsonAsync<User>($"api/user/u/{username}"); 

            StateContainer.SetImageURl(user.ImageUrl);

            StateContainer.OnChange += StateHasChanged;
        }

        public string GetModalId()
        {
            Count += 1;
            return "modal" + Count;
        }

        public void SetAccount(string accountId){ 
          Account = Accounts.FirstOrDefault(a => a.AccountId == accountId);
        }
        public void SetSchedule(string scheduleId){ 
          Schedule = Schedules.FirstOrDefault(a => a.ScheduleId == scheduleId);
        }

        public Recipe SetRecipe(string recipeId){ 
          return Recipes.FirstOrDefault(a => a.RecipeId == recipeId);
        }

        public async Task GetIngredients(string recipeId) {
            var client = ClientFactory.CreateClient("ServerAPI.NoAuthenticationClient");
            Ingredients = await client.GetFromJsonAsync<Ingredient[]>($"api/ingredient/recipe/{recipeId}");
            StateHasChanged();
        }

        public void Dispose()
        {
            StateContainer.OnChange -= StateHasChanged;
        }
    }
    
}
