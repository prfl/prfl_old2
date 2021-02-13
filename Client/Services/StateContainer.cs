using System;

namespace Profile.Client.Services {
    public class StateContainer
    {
        public string Username { get; set; }
        public string FavoriteIconUrl { get; set; }
        public string AccountIconUrl { get; set; }
        public string LinkIconUrl { get; set; }
        public string ProjectIconUrl { get; set; }
        public string VideoIconUrl { get; set; }
        public string ScheduleIconUrl { get; set; }
        public string ChefIconUrl { get; set; }
        public string BartenderIconUrl { get; set; }
        public event Action OnChange;
    
        public void SetProperty(string value)
        {
            Username = value;
            NotifyStateChanged();
        }
        public void SetAccountIcon(string value) {
            AccountIconUrl = value;
        }
        public void SetLinkIcon(string value) {
            LinkIconUrl = value;
        }
        public void SetFavoriteIcon(string value) {
            FavoriteIconUrl = value;
        }
        public void SetProjectIcon(string value) {
            ProjectIconUrl = value;
        }
        public void SetVideoIcon(string value) {
            VideoIconUrl = value;
        }
        public void SetScheduleIcon(string value) {
            ScheduleIconUrl = value;
        }

        public void SetChefIcon(string value) {
            ChefIconUrl = value;
        }

        public void SetBartenderIcon(string value) {
            BartenderIconUrl = value;
        }

        public string GetProperty() {
            return Username;
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
