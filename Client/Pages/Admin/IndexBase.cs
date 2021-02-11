using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Profile.Shared.Models;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.JSInterop;

namespace Profile.Client.Pages.Admin
{
  
    public class IndexBase : ComponentBase {

        [Inject] HttpClient client { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Parameter] public string username { get; set; }
        [Parameter] public string linkType { get; set; }
        public int Count = 1;
        public Link[] Links { get; set; }
        public Account[] Accounts { get; set; }
        public Project[] Projects { get; set; }
        public Video[] Videos { get; set; }
        public Favorite[] Favorites { get; set; }
        public Schedule[] Schedules { get; set; }
        public Account Account { get; set; }
        
    
        protected override async Task OnParametersSetAsync() {
            await GetData(linkType);
        }
    
        public async Task GetData(string linkType) {
        
            var user = await client.GetFromJsonAsync<ProfileUser>("api/user");
            username = user.UserName;
            
            if(linkType.ToUpper() == "LINK") {
                Links = await client.GetFromJsonAsync<Link[]>($"api/{linkType}");
            }
            else if(linkType.ToUpper() == "ACCOUNT") {
                Accounts = await client.GetFromJsonAsync<Account[]>($"api/{linkType}");
            }
            else if(linkType.ToUpper() == "PROJECT") {
                Projects = await client.GetFromJsonAsync<Project[]>($"api/{linkType}");
            }
            else if(linkType.ToUpper() == "VIDEO") {
                Videos = await client.GetFromJsonAsync<Video[]>($"api/{linkType}");
            }
            else if(linkType.ToUpper() == "FAVORITE") {
                Favorites = await client.GetFromJsonAsync<Favorite[]>($"api/{linkType}");
                Accounts = await client.GetFromJsonAsync<Account[]>($"api/account");
            }
            else if(linkType.ToUpper() == "SCHEDULE") {
                Schedules = await client.GetFromJsonAsync<Schedule[]>($"api/{linkType}");
            }
            
        }
    
        public string GetModalId()
        {
            var num = Count++;
            return "modal" + num;
        }
    
        public void SetAccount(string accountId){ 
          Account = Accounts.FirstOrDefault(a => a.AccountId == accountId);
        }
    
        public async Task DeleteLink(string linkId, string linkType, string linkName) { 
            bool isConfirmed = await JSRuntime.InvokeAsync<bool>("confirm", $"Are you sure you want to delete your {linkType} {linkName}?");
            if(isConfirmed) {
                await client.DeleteAsync($"api/{linkType}/{linkId}");
                await OnParametersSetAsync();
            }
        }
    
            
        public async Task OnClickUpAsync(string linkId, string linkType) {         
            if(linkType.ToUpper() == "FAVORITE") {
                var link = Favorites.FirstOrDefault(f => f.FavoriteId == linkId);
                if(link != null && link.Order > 1) {
                    var otherLink = Favorites.FirstOrDefault(f => f.Order ==  link.Order - 1);
                    link.Order -= 1;
                    otherLink.Order += 1;
                    await client.PutAsJsonAsync($"api/{linkType}/{link.FavoriteId}", link);
                    await client.PutAsJsonAsync($"api/{linkType}/{otherLink.FavoriteId}", otherLink);
                    await OnParametersSetAsync();
                }  
            }
        }
        public async Task OnClickDownAsync(string linkId, string linkType) {         
            if(linkType.ToUpper() == "FAVORITE") {
                var link = Favorites.FirstOrDefault(f => f.FavoriteId == linkId);
                if(link != null && link.Order < Favorites.Count()) {
                    var otherLink = Favorites.FirstOrDefault(f => f.Order ==  link.Order + 1);
                    link.Order += 1;
                    otherLink.Order -= 1;
                    await client.PutAsJsonAsync($"api/{linkType}/{link.FavoriteId}", link);
                    await client.PutAsJsonAsync($"api/{linkType}/{otherLink.FavoriteId}", otherLink);
                    await OnParametersSetAsync();
                }  
            }
        }
    }
}